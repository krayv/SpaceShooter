using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] float checkDelay = 0.01f;
    List<IProjectile> projectiles = new List<IProjectile>();

    private float currentDelay;
    private float skipedTimeSum;
    // Start is called before the first frame update
    private List<IProjectile> destroyedProjectiles = new List<IProjectile>();

    void Start()
    {
        Messenger.AddListener<IProjectile>(GameEvents.PROJECTILE_SPAWNED, OnSpawnProjectile);
        Messenger.AddListener<IProjectile>(GameEvents.PROJECTILE_DESTROYED, OnDestroyProjectile);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (currentDelay <= 0f)
        {
            ClearProjectiles();
            foreach (Projectile projectile in projectiles)
            {
                projectile.AddExistTime(skipedTimeSum);           
                if (projectile.IsNeedTODestroy())
                {
                    projectile.Destroy();                  
                }
            }
            skipedTimeSum = 0f;
            currentDelay = checkDelay;
        }
        else
        {
            currentDelay -= Time.deltaTime;
            skipedTimeSum += Time.deltaTime;
        }
    }

    private void ClearProjectiles()
    {
        while(destroyedProjectiles.Count > 0)
        {
            projectiles.Remove(destroyedProjectiles.First());
            destroyedProjectiles.Remove(destroyedProjectiles.First());
        }
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener<IProjectile>(GameEvents.PROJECTILE_SPAWNED, OnSpawnProjectile);
        Messenger.RemoveListener<IProjectile>(GameEvents.PROJECTILE_DESTROYED, OnSpawnProjectile);
    }

    private void OnSpawnProjectile(IProjectile projectile)
    {
        projectiles.Add(projectile);
    }

    private void OnDestroyProjectile(IProjectile projectile)
    {
        destroyedProjectiles.Add(projectile);
    }
}
