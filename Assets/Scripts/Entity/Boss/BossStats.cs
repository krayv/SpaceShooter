using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStats : MonoBehaviour
{
    [SerializeField] private float MoneyRewardMultiplier;
    [SerializeField] private float ScoreRewardMultiplier;
    [SerializeField] private float minKilledWeightToSpawn;
    [SerializeField] GameObject arena;

    private int currentIteration;

    public void Spawn(Vector3 pos, int bossKillIteration)
    {
        GameObject spawnedBoss = Instantiate(gameObject);
        spawnedBoss.transform.position = pos;
        GameObject spawnedArena = Instantiate(arena);

        currentIteration = bossKillIteration;

        Stats stats = spawnedBoss.GetComponent<Stats>();
        stats.SetDamageMultiplier(currentIteration);
        stats.SetLiveMultiplier(currentIteration);

        BossStats bossStats = spawnedBoss.GetComponent<BossStats>();
        bossStats.SetIteration(currentIteration);

        spawnedArena.transform.position = spawnedBoss.transform.position;
        spawnedArena.GetComponent<FollowToTarget>().SetTarget(spawnedBoss.transform);

        Messenger.Broadcast(GameEvents.BOSS_SPAWNED, gameObject);
    }

    public float GetMinimalKilledWeightToSpawn(int iteration)
    {
        return minKilledWeightToSpawn * iteration;
    }

    public void OnDestroy()
    {
        Messenger.Broadcast(GameEvents.BOSS_WERE_KILLED, this);
    }

    public float GetMoneyRewardMultiplier()
    {
        return MoneyRewardMultiplier;
    }

    public float GetScoreRewardMultiplier()
    {
        return ScoreRewardMultiplier;
    }

    public int GetCurrentIteration()
    {
        return currentIteration;
    }

    public void SetIteration(int value)
    {
        currentIteration = value;
    }
}
