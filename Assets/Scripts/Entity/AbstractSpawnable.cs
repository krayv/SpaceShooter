using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractSpawnable : MonoBehaviour
{
    [SerializeField] private float SpawnRange;
    [SerializeField] private float DespawnRange;
    [SerializeField] private float spawnWeight;
    [SerializeField] private int MaxCount;
    [SerializeField] private float minDifficultToSpawn;
    [SerializeField] private float maxDifficultToSpawn = -1f;
    [SerializeField] private float spawnDelay;
    [SerializeField] private float zPosition;


    public abstract void OnSpawn(Vector3 spawnPoint);

    public abstract void Despawn();

    public float GetSpawnDelay()
    {
        return spawnDelay;
    }
    public float GetMaxCount()
    {
        return MaxCount;
    }
    public float GetSpawnWeight()
    {
        return spawnWeight;
    }
    public float GetDespawnRange()
    {
        return DespawnRange;
    }
    public float GetSpawnRange()
    {
        return SpawnRange;
    }
    public float GetMaximumDiffucultToSpawn()
    {
        return maxDifficultToSpawn;
    }
    public float GetMinimalDifficultToSpawn()
    {
        return minDifficultToSpawn;
    }
    public float GetZPosition()
    {
        return zPosition;
    }
}
