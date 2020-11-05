using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    float baseDamage { get; set; }
    float baseDelay { get; set; }
    float baseProjectileSpeed { get; set; }

    float baseProjectileLifeTime { get; set; }

    void Fire(Transform targetTransform);

    void RotateToTarget(Transform transform);

}
