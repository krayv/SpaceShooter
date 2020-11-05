using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour, IWeapon
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] WeaponPoint point;

    [SerializeField] float serializeDamage;

    [SerializeField] float serializeDelay;

    [SerializeField] float serializeProjectileSpeed;

    [SerializeField] float projetileLifeTime;

    public float baseDamage { get; set; }
    public float baseDelay { get; set; }
    public float baseProjectileSpeed { get; set; }

    public float baseProjectileLifeTime { get; set; }

    private float timeFromPreviousShot = 0f;

    private Stats gameObjectStats;

    private void Awake()
    {
        baseDamage = serializeDamage;
        baseDelay = serializeDelay;
        baseProjectileSpeed = serializeProjectileSpeed;
        baseProjectileLifeTime = projetileLifeTime;
        gameObjectStats = GetComponentInParent<Stats>();
        if (point != null)
        {
            transform.SetParent(point.transform);
        }
    }

    public void Fire(Transform targetTransform = null)
    {
        if(Time.time - timeFromPreviousShot > baseDelay && projectilePrefab != null)
        {
            GameObject projectile = Instantiate(projectilePrefab);
            projectile.transform.position = transform.position;
            projectile.transform.rotation = transform.rotation;
            IProjectile[] projectiles = projectile.GetComponentsInChildren<IProjectile>();
            foreach(IProjectile projectileComponent in projectiles)
            {
                projectileComponent.damage = GetCurrentDamage();
                projectileComponent.moveSpeed = baseProjectileSpeed;
                projectileComponent.liveTime = baseProjectileLifeTime;
                projectileComponent.isAlliedForEnemy = (GetComponentInParent<IEnemy>() != null);
                projectileComponent.isAlliedForPlayer = (GetComponentInParent<PlayerInputController>() != null);
            }
            if(targetTransform != null)
            {
                HauntTarget[] hauntTargets = projectile.GetComponentsInChildren<HauntTarget>();
                foreach (HauntTarget haunt in hauntTargets)
                {
                    haunt.SetTargetTranfsorm(targetTransform);
                }
            }            
            timeFromPreviousShot = Time.time;
        }     
    }

    public void Rotate(float angle)
    {
        if(point != null)
        {
            point.transform.Rotate(0f,0f, angle);
        }
        else
        {
            transform.Rotate(0f, 0f, angle);
        }
        
    }
    
    public void RotateToTarget(Transform targetTransform)
    {
        if(point != null)
        {
            float angle = Lib.GetAngleBetweenTranfsorm(point.transform.position, targetTransform.position);
            point.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
        else
        {
            float angle = Lib.GetAngleBetweenTranfsorm(transform.position, targetTransform.position);
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

    }
    public void RotateToTarget(Vector2 targetPoint)
    {
        if (point != null)
        {
            float angle = Lib.GetAngleBetweenTranfsorm(point.transform.position, targetPoint);
            point.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
        else
        {
            float angle = Lib.GetAngleBetweenTranfsorm(transform.position, targetPoint);
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

    }

    public float GetCurrentDamage()
    {
        return gameObjectStats != null ? baseDamage * gameObjectStats.GetDamageMultiplier() : baseDamage;
    }
}
