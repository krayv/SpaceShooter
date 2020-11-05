using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : AbstractSpawnable
{
    public override void OnSpawn(Vector3 spawnPoint)
    {
        transform.position = spawnPoint;
    }

    public override void Despawn()
    {
        Destroy(gameObject);
    }
}
