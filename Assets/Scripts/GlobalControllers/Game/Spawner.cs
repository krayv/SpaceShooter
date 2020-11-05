using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Spawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> prefabSpawnEnemy;
    [SerializeField] private List<GameObject> prefabSpawnConsumable;
    [SerializeField] private List<GameObject> prefabSpawnBackground;
    [SerializeField] private List<BossStats> bossList;
    [SerializeField] private Transform targetTransform;
    [SerializeField] private float maxSumSpawnWeight;
    [SerializeField] private float difficult;
    [SerializeField] private float increaseDifficultPerSecond;

    private Dictionary<GameObject, GameObject> spawnedObjects;
    private Dictionary<GameObject, float> timings;
    private Dictionary<GameObject, AbstractSpawnable> spawnables;

    private bool isBossSpawned;

    private float sumSpawnedObjectWeight;
    private float sumKilledWeight;
    private int killedBossIteration = 1;

    private void Awake()
    {
        spawnedObjects = new Dictionary<GameObject, GameObject>();
        timings = new Dictionary<GameObject, float>();
        spawnables = new Dictionary<GameObject, AbstractSpawnable>();    
    }

    private void Start()
    {
        Messenger.AddListener<GameObject>("ENTITY_DEAD", EntityDead);
        Messenger.AddListener<BossStats>(GameEvents.BOSS_WERE_KILLED, OnKilledBoss);
        
        CacheArray(prefabSpawnEnemy);
        CacheArray(prefabSpawnConsumable);
        CacheArray(prefabSpawnBackground);

        GameObject player = GameObject.FindGameObjectWithTag(LayersAndTags.PLAYER_TAG);
        if(player != null)
        {
            targetTransform = player.transform;
        }
    }

    private void CacheArray(List<GameObject> gameObjects)
    {
        foreach (GameObject prefabObject in gameObjects)
        {
            if (prefabObject == null)
            {
                continue;
            }
            timings.Add(prefabObject, 0f);
        }

        foreach (GameObject prefabObject in gameObjects)
        {
            if (prefabObject == null)
            {
                continue;
            }
            AbstractSpawnable spawnable = prefabObject.GetComponent<AbstractSpawnable>();
            if (spawnable == null)
            {
                Debug.LogError("\nSpawnObject not founded in" + prefabObject.name);
                continue;
            }
            spawnables.Add(prefabObject, spawnable);
        }
    }

    private void FixedUpdate()
    {
        if (targetTransform == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag(LayersAndTags.PLAYER_TAG);
            if (playerObject == null)
            {
                return;
            }
            else
            {
                targetTransform = playerObject.transform;
            }
        }
        SpawnAndCheckObjects(prefabSpawnBackground);
        SpawnAndCheckObjects(prefabSpawnConsumable);
        if (isBossSpawned)
        {
            return;
        }
        SpawnAndCheckObjects(prefabSpawnEnemy);


    }

    private void SpawnAndCheckObjects(List<GameObject> gameObjects)
    {
        ShufflePrefabList(gameObjects);
        foreach (GameObject prefabObject in gameObjects)
        {
            try
            {
                if (prefabObject == null)
                {
                    continue;
                }
                if (CheckDelay(prefabObject) && CheckCount(prefabObject) && CheckWeight(prefabObject) && CheckDifficult(prefabObject))
                {
                    AbstractSpawnable spawnable = GetSpawnObject(prefabObject);
                    if (spawnable != null)
                    {
                        GameObject spawnedGameObject = Instantiate(prefabObject);
                        AbstractSpawnable spawnableInGameObject = spawnedGameObject.GetComponent<AbstractSpawnable>();
                        spawnableInGameObject.OnSpawn(GetRandomPositionForSpawn(spawnableInGameObject));
                        sumSpawnedObjectWeight += spawnableInGameObject.GetSpawnWeight();
                        spawnedObjects.Add(spawnedGameObject, prefabObject);
                        spawnables.Add(spawnedGameObject, spawnedGameObject.GetComponent<AbstractSpawnable>());
                        SetTimeSpawnForPrefab(prefabObject);
                    }
                    else
                    {
                        Debug.LogError("\nSpawnObject not founded in" + prefabObject.name);
                    }
                }
            }
            catch (Exception exception)
            {
                Debug.Log(exception.Message);
            }


        }
        List<GameObject> gameObjectsForDespawn = new List<GameObject>();
        foreach (KeyValuePair<GameObject, GameObject> spawnedGameObject in spawnedObjects)
        {
            try
            {
                if (CheckRange(spawnedGameObject.Key))
                {

                }
                else
                {
                    gameObjectsForDespawn.Add(spawnedGameObject.Key);
                }
            }
            catch (Exception exception)
            {
                Debug.Log(exception.Message);
            }


        }
        DespawnObjects(gameObjectsForDespawn);
    }

    private bool TryToSpawnBoss()
    {
        BossStats boss = bossList.Where(b => b.GetMinimalKilledWeightToSpawn(killedBossIteration) < sumKilledWeight).FirstOrDefault();
        if(boss != null)
        {
            
            boss.Spawn(GetRandomPositionForSpawn(), killedBossIteration);
            isBossSpawned = true;
            DespawnObjectsByPrefabList(prefabSpawnEnemy);
            return true;

        }
        return false;
    }

    private void DespawnObjectsByPrefabList(List<GameObject> gameObjects)
    {
        List<GameObject> spawnedObjectsList = spawnedObjects.Where(s => gameObjects.Contains(s.Value)).Select(s => s.Key).ToList();
        DespawnObjects(spawnedObjectsList);
    }

    private void OnKilledBoss(BossStats stats)
    {
        isBossSpawned = false;
        killedBossIteration++;
    }

    private void ShufflePrefabList(List<GameObject> gameObjects)
    {
        List<GameObject> prefabs = new List<GameObject>();
        prefabs.AddRange(gameObjects);
        int i = UnityEngine.Random.Range(6, 12);
        while(i > 1)
        {
            List<GameObject> cuttedPrefabs = new List<GameObject>();
            int cutRange = UnityEngine.Random.Range(1, gameObjects.Count / 2 + 1);
            for(int j = cutRange; j <= gameObjects.Count; j++)
            {
                cuttedPrefabs.Add(prefabs[j - 1]);
            }
            for (int j = 1; j < cutRange; j++)
            {
                cuttedPrefabs.Add(prefabs[j - 1]);
            }
            prefabs = cuttedPrefabs;
            i--;
        }
        gameObjects = prefabs;
    }

    private void DespawnObjects(List<GameObject> gameObjects)
    {
        foreach(GameObject currentGameObject in gameObjects)
        {
            AbstractSpawnable abstractSpawnable = GetSpawnObject(currentGameObject);
            ClearCache(currentGameObject);
            abstractSpawnable.Despawn();
        }
    }

    private float GetCurrentDifficult()
    {
        return difficult + (Time.timeSinceLevelLoad * increaseDifficultPerSecond);
    }

    private float GetCurrentMaxEntitySpawn()
    {
        return maxSumSpawnWeight * GetCurrentDifficult();
    }

    private Vector3 GetRandomPositionForSpawn(AbstractSpawnable spawnable)
    {
        Vector3 spawnPoint = Lib.RandomPointOnUnitCircle(spawnable.GetSpawnRange())  + Lib.Vector3ToVector2(targetTransform.position);
        spawnPoint = new Vector3(spawnPoint.x, spawnPoint.y, spawnable.GetZPosition());
        return spawnPoint;
    }

    private Vector3 GetRandomPositionForSpawn()
    {
        Vector3 spawnPoint = Lib.RandomPointOnUnitCircle(15f) + Lib.Vector3ToVector2(targetTransform.position);
        spawnPoint = new Vector3(spawnPoint.x, spawnPoint.y, -0.5f);
        return spawnPoint;
    }

    private bool CheckDifficult(GameObject targetObject)
    {
        AbstractSpawnable spawnable = GetSpawnObject(targetObject);
        float currentDifficult = GetCurrentDifficult();
        float maxDiff = spawnable.GetMaximumDiffucultToSpawn();
        float minDiff = spawnable.GetMinimalDifficultToSpawn();
        if (spawnable != null)
        {
            if ((maxDiff <= 0 || maxDiff > currentDifficult) && (minDiff < currentDifficult))
            {
                return true;
            }
        }
        return false;
    }
    private bool CheckDelay(GameObject targetObject)
    {
        float timeFromSpawn = GetTimeFromSpawn(targetObject);
        AbstractSpawnable spawnable = GetSpawnObject(targetObject);    
        if (spawnable != null)
        {
            if(Time.time - timeFromSpawn >= (spawnable.GetSpawnDelay() / Mathf.Sqrt(GetCurrentDifficult())))
            {
                return true;
            }
        }
        return false;
    }

    private bool CheckCount(GameObject targetObject)
    {
        int count = GetCountSpawnedObjects(targetObject);
        AbstractSpawnable spawnable = GetSpawnObject(targetObject);
        if(count < spawnable.GetMaxCount())
        {
            return true;
        }
        return false;
    }

    private bool CheckWeight(GameObject targetObject)
    {
        AbstractSpawnable spawnable = GetSpawnObject(targetObject);
        if(sumSpawnedObjectWeight + spawnable.GetSpawnWeight() <= GetCurrentMaxEntitySpawn())
        {
            return true;
        }
        return false;
    }
   
    private AbstractSpawnable GetSpawnObject(GameObject targerObject)
    {
            return spawnables[targerObject];
    }

    private float GetTimeFromSpawn(GameObject targerObject)
    {
        return timings[targerObject];
    }

    private int GetCountSpawnedObjects(GameObject targerObject)
    {
        return spawnedObjects.Where(t => t.Value == targerObject).Count();
    }

    private void SetTimeSpawnForPrefab(GameObject targerObject)
    {
        timings[targerObject] = Time.time;
    }

    private void EntityDead(GameObject targerObject)
    {
        if (targerObject.transform == targetTransform)
        {
            targetTransform = null;
        }
        if(spawnedObjects.Any(go=>go.Key == targerObject))
        {
            AbstractSpawnable killedSpawnable = targerObject.GetComponent<AbstractSpawnable>();
            if(killedSpawnable != null)
            {
                sumKilledWeight += killedSpawnable.GetSpawnWeight();
                if(!isBossSpawned)
                {
                    if(TryToSpawnBoss())
                    {
                        return;
                    }
                }              
            }
            ClearCache(targerObject);
        }
    }
    private void ClearCache(GameObject targetObject)
    {
        sumSpawnedObjectWeight -= GetSpawnObject(targetObject).GetSpawnWeight();
        spawnables.Remove(targetObject);
        spawnedObjects.Remove(targetObject);
    }

    private bool CheckRange(GameObject targerObject)
    {
        AbstractSpawnable spawnable = GetSpawnObject(targerObject);
        if ((Lib.Vector3ToVector2(targetTransform.position) - Lib.Vector3ToVector2(targerObject.transform.position)).magnitude > spawnable.GetDespawnRange())
        {
            return false;
        }
        return true;
    }
}
