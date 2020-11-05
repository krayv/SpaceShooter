using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameStats : MonoBehaviour
{
    [SerializeField] int money;
    [SerializeField] int score;
    [SerializeField] int moneySession;
    [SerializeField] List<Label> labels = new List<Label>();
    [SerializeField] List<GameObject> ownedShipPrefabs;
    [SerializeField] HangarController hangar;
    private string prefabShipsPath = GameConstants.prefabShipPath;
    [SerializeField] GameObject defaultPrefabShip;

    private string filename;
    private GameObject currentPlayerShipPrefab;
    private SpawnPlayer spawnPlayer;
    private float sessionTime;

    public void SaveGame()
    {
        Dictionary<string, object> gamestate = new Dictionary<string, object>();
        gamestate.Add("money", money);
        if (currentPlayerShipPrefab != null)
        {
            try
            {
                Stats stats = currentPlayerShipPrefab.GetComponent<Stats>();
                string pathForCheckOnExist = prefabShipsPath + stats.GetPrefabName();
                GameObject gameObject = Resources.Load(pathForCheckOnExist, typeof(GameObject)) as GameObject;
                if (gameObject == null)
                {
                    throw (new Exception());
                }
                gamestate.Add("prefab_name", stats.GetPrefabName());
            }
            catch (Exception exc)
            {
                gamestate.Add("prefab_name", "");
                Debug.LogError("Error on saving " + currentPlayerShipPrefab.name);
                Debug.LogError(exc.Message);
            }
        }
        else
        {
            gamestate.Add("prefab_name", "");
        }
        List<string> ownedShipPrefabNames = new List<string>();
        foreach (GameObject prefabShip in ownedShipPrefabs)
        {
            ownedShipPrefabNames.Add(prefabShip.GetComponent<Stats>().GetPrefabName());
        }
        gamestate.Add("owned_ships", ownedShipPrefabNames);
        FileStream stream = File.Create(filename);
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(stream, gamestate);
        stream.Close();
    }
    private void LoadGame()
    {
        if (!File.Exists(filename))
        {
            currentPlayerShipPrefab = defaultPrefabShip;
            ownedShipPrefabs.Add(defaultPrefabShip);
            return;
        }
        Dictionary<string, object> gamestate;

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = File.Open(filename, FileMode.Open);
        gamestate = formatter.Deserialize(stream) as Dictionary<string, object>;
        stream.Close();

        money = (int)gamestate["money"];
        string prefabName = (string)gamestate["prefab_name"];
        if (prefabName != null && prefabName != "")
        {
            currentPlayerShipPrefab = LoadShip(prefabName);
        }
        else
        {
            currentPlayerShipPrefab = defaultPrefabShip;
        }
        ownedShipPrefabs = new List<GameObject>();
        List<string> ownedShipPrefabNames = (List<string>)gamestate["owned_ships"];
        foreach(string name in ownedShipPrefabNames)
        {
            ownedShipPrefabs.Add(LoadShip(name));
        }
    }

    public GameObject LoadShip(string prefabName)
    {
        try
        {
            string pathForCheckOnExist = prefabShipsPath + prefabName;
            GameObject loadedPrefab = Resources.Load(pathForCheckOnExist, typeof(GameObject)) as GameObject;
            if (loadedPrefab != null)
            {
                return loadedPrefab;
            }
            else
            {
                throw (new Exception());
            }
        }
        catch (Exception exc)
        {
            currentPlayerShipPrefab = defaultPrefabShip;
            Debug.LogError("Error on load " + prefabName);
            Debug.LogError(exc.Message);
            return null;
        }
    }

    public void BuyShip(GameObject ship)
    {
        Stats stats = ship.GetComponent<Stats>();
        RemoveMoney(stats.GetPrice());
        ownedShipPrefabs.Add(ship);
        Messenger.Broadcast(GameEvents.SHIP_SWITCHED, ship);
        Messenger.Broadcast(GameEvents.SHIP_SELECTED, ship);
        RefreshLabelInfo();
        SaveGame();
    }

    public GameObject GetPlayerPrefab()
    {
        return currentPlayerShipPrefab;
    }

    public float GetSessionTime()
    {
        return sessionTime;
    }

    public float GetScore()
    {
        return score;
    }

    public void IsCanBuyShip(GameObject shipPrefab)
    {

    }

    public int GetSessionMoney()
    {
        return moneySession;
    }


    public bool IsPreviousShipbuyed(GameObject shipPrefab)
    {
        List<GameObject> storeShips = hangar.GetShips();
        int currentIndex = storeShips.IndexOf(shipPrefab);
        if(currentIndex == 0)
        {
            return true;
        }
        GameObject previousShip = storeShips[currentIndex - 1];
        return ownedShipPrefabs.Contains(previousShip);

    }

    public bool IsHaveThatShip(GameObject shipPrefab)
    {
        return ownedShipPrefabs.Contains(shipPrefab);
    }

    public bool IsEnoughtMoney(int price)
    {
        return money >= price;
    }


    public string GetCurrentPlayerShipPrefabName()
    {
        Stats stats = currentPlayerShipPrefab.GetComponent<Stats>();
        return stats.GetPrefabName();
    }

    public void OnWatchRewardAdds()
    {
        AddSessionMoney(moneySession);
        AddMoney(moneySession);
        Messenger.Broadcast(GameEvents.GAMEDATA_VALUE_CHANGED, new KeyValuePair<GameDataList, object>(GameDataList.session_money, moneySession));
        Messenger.Broadcast(GameEvents.GAMEDATA_VALUE_CHANGED, new KeyValuePair<GameDataList, object>(GameDataList.money, money));
    }

    private void Awake()
    {
        filename = Path.Combine(Application.persistentDataPath, "game.dat");
        spawnPlayer = GetComponent<SpawnPlayer>();
        if(ownedShipPrefabs == null)
        {
            ownedShipPrefabs = new List<GameObject>();
        }
        
    }

    private void Start()
    {
        Messenger.AddListener<GameObject>(GameEvents.ENTITY_DEAD, OnEntityDead);
        Messenger.AddListener<BossStats>(GameEvents.BOSS_WERE_KILLED, OnBossDead);
        Messenger.AddListener(GameEvents.PLAYER_DEAD, OnPlayerDead);
        Messenger.AddListener<GameDataList, Action<object>>(GameEvents.REQUEST_GAME_DATA, AnswerToRequest);
        Messenger.AddListener<GameObject>(GameEvents.SHIP_SELECTED, OnSwitchShip);
        moneySession = 0;
        score = 0;
        string path = prefabShipsPath + GameConstants.defaultPrefabShip;
        defaultPrefabShip = Resources.Load<GameObject>(path);
        LoadGame();
        RefreshLabelInfo();

        if (hangar != null)
        {
            hangar.SelectShip(currentPlayerShipPrefab);
        }
        if(spawnPlayer != null)
        {
            spawnPlayer.Spawn();
        }
    }

    private void OnSwitchShip(GameObject selectedShipPrefab)
    {
        if(IsHaveThatShip(selectedShipPrefab))
        {
            currentPlayerShipPrefab = selectedShipPrefab;
        }     
    }

    private void RefreshLabelInfo()
    {
        foreach(Label label in labels)
        {
            switch (label.GetGameData())
            {
                case GameDataList.money:
                    {
                        label.ChangeValue(money);
                        break;
                    }
                case GameDataList.score:
                    {
                        label.ChangeValue(score);
                        break;
                    }
                case GameDataList.session_money:
                    {
                        label.ChangeValue(moneySession);
                        break;
                    }
            }
        }
    }

    private void Update()
    {
        sessionTime += Time.deltaTime;
    }

    private void AnswerToRequest(GameDataList gameData, Action<object> callback)
    {
        switch(gameData)
        {
            case GameDataList.money:
            {
                callback(money);
                break;
            }
            case GameDataList.score:
            {
                callback(score);
                break;
            }
            case GameDataList.session_money:
            {
                callback(moneySession);
                break;
            }
        }
    }
    
    private void OnEntityDead(GameObject gameObject)
    {
        Stats stats = gameObject.GetComponent<Stats>();
        if(stats != null)
        {
            int moneyReward = stats.GetMoneyReward();
            if(moneyReward != 0)
            {
                AddSessionMoney(moneyReward);
                AddMoney(moneyReward);
                Messenger.Broadcast(GameEvents.GAMEDATA_VALUE_CHANGED, new KeyValuePair<GameDataList, object>(GameDataList.session_money, moneySession));
                Messenger.Broadcast(GameEvents.GAMEDATA_VALUE_CHANGED, new KeyValuePair<GameDataList, object>(GameDataList.money, money));
            }
            int scoreReward = stats.GetScoreReward();
            if(scoreReward != 0)
            {
                AddScore(scoreReward);
                Messenger.Broadcast(GameEvents.GAMEDATA_VALUE_CHANGED, new KeyValuePair<GameDataList, object>(GameDataList.score, score));
            }       
        }
      
    }

    private void OnBossDead(BossStats bossStats)
    {
        int newMoneyReward = ((int)(moneySession * bossStats.GetMoneyRewardMultiplier())) - moneySession;
        int newScore = ((int)(score * bossStats.GetScoreRewardMultiplier())) - score;
        AddSessionMoney(newMoneyReward);
        AddMoney(newMoneyReward);
        AddScore(newScore);
    }

    private void OnPlayerDead()
    {
        
    }

    private void AddScore(int value)
    {
        
        score += value;
    }

    private void RemoveScore(int value)
    {
        score -= value;
    }

    private void AddMoney(int value)
    {
        money += value;
    }

    private void RemoveMoney(int value)
    {
        money -= value;
    }
    private void AddSessionMoney(int value)
    {
        moneySession += value;
    }

    private void RemoveSessionMoney(int value)
    {
        moneySession -= value;
    }

    
}
