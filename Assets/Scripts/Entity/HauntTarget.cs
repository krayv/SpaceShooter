using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(MovementController))]
[RequireComponent(typeof(IProjectile))]
public class HauntTarget : MonoBehaviour
{
    [SerializeField] float rotationSpeed;
    [SerializeField] float maxDegreesToRotate;
    [SerializeField] bool isCanSwitchTarget;
    [SerializeField] float detectRadius;
    [SerializeField] float switchTargetDelay;

    private MovementController movementController;
    private Transform targetTransform;
    private float currentDelay;
    private float moveSpeed;
    private LayerMask ignoreLayer;

    public void SetTargetTranfsorm(Transform target)
    {
        targetTransform = target;
    }

    private void Awake()
    {
        movementController = GetComponent<MovementController>();
        ignoreLayer = LayerMask.NameToLayer(LayersAndTags.PROJECTILE_LAYER);
    }

    private void Start()
    {
        moveSpeed = GetComponent<IProjectile>().moveSpeed;
    }

    // Update is called once per frame
    private void Update()
    {
        if(targetTransform != null)
        {
            movementController.Move(transform.up, moveSpeed);
            movementController.RotateToward(targetTransform.position, rotationSpeed, maxDegreesToRotate);
        }
        else
        {
            if(isCanSwitchTarget)
            {
                if(currentDelay > switchTargetDelay)
                {
                    List<Transform> targetsTransform = new List<Transform>();
                    RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, detectRadius, Vector2.one, detectRadius, ~ignoreLayer);
                    foreach (RaycastHit2D raycastHit in hits)
                    {
                        if (raycastHit.transform.GetComponent<IEnemy>() != null)
                        {
                            targetsTransform.Add(raycastHit.transform);
                        }
                    }
                    if (targetsTransform.Any())
                    {
                        targetTransform = targetsTransform.OrderBy(target => (target.position - transform.position).magnitude).First();
                    }
                    currentDelay = 0f;
                }
                else
                {
                    currentDelay += Time.deltaTime;
                }

            }
        }
    }
}
