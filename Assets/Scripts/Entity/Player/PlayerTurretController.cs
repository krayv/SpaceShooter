using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerTurretController : MonoBehaviour
{
    IWeapon[] weapons;
    [SerializeField] float detectRadius;

    private Transform targetTransform;
    private LayerMask ignoreLayer;

    private void Awake()
    {
        weapons = GetComponentsInChildren<IWeapon>();
        ignoreLayer = LayerMask.NameToLayer(LayersAndTags.PROJECTILE_LAYER);
    }

    // Update is called once per frame
    void Update()
    {
        List<Transform> targetsTransform = new List<Transform>();
        RaycastHit2D[] hits = Physics2D.CircleCastAll(origin:transform.position, radius:detectRadius, direction:Vector2.one,distance:detectRadius ,layerMask:ignoreLayer);
        foreach (RaycastHit2D raycastHit in hits)
        {
            if (raycastHit.transform.GetComponent<IEnemy>() != null)
            {
                targetsTransform.Add(raycastHit.transform);
            }
        }
        if(targetsTransform.Any())
        {
            targetTransform = targetsTransform.OrderBy(target => (target.position - transform.position).magnitude).First();
            if (targetTransform != null)
            {
                if ((Lib.Vector3ToVector2(transform.position) - targetTransform.gameObject.GetComponent<Collider2D>().ClosestPoint(transform.position)).magnitude < detectRadius)
                {
                    foreach (IWeapon weapon in weapons)
                    {
                        weapon.RotateToTarget(targetTransform);
                        weapon.Fire(targetTransform);
                    }
                }
            }
        }
        
    }
}
