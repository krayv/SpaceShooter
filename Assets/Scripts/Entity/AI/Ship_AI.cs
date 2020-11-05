using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementController))]
public class Ship_AI : MonoBehaviour, IEnemy
{
    [SerializeField] float detectRadius = 5f;
    [SerializeField] float timeToChangeDirection = 2f;
    [SerializeField] float largeRangeToTarget = 12f;
    [SerializeField] float mediumRangeToTarget = 9f;
    [SerializeField] float shortRangeToTarget = 5f;
    [SerializeField] float minRangeToTarget = 0.5f;
    [SerializeField] MovementController movementController;
    enum Ranges { MinRange, ShortRange, MediumRange, LargeRange, AboveLongRange };

    private Ranges currentRange;

    private Transform targetTransform;
    private float currentTime;
    private float timeFromChangeDirection = 0f;

    private void Awake()
    {
        movementController = GetComponent<MovementController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentTime += Time.fixedDeltaTime;
        if (targetTransform == null)
        {
            RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, detectRadius, Vector2.one);
            foreach (RaycastHit2D raycastHit in hits)
            {
                if (raycastHit.transform.GetComponent<PlayerInputController>() != null)
                {
                    targetTransform = raycastHit.transform;
                    CheckRange(CalculateDistance());
                }           
            }
            PatrolFly();
        }
        else
        {           
            FlyNextToTarget();
        }
        
    }

    private void FlyNextToTarget()
    {
        if(timeFromChangeDirection >= timeToChangeDirection)
        {
            CheckRange(CalculateDistance());
            timeFromChangeDirection = 0f;
        }
        timeFromChangeDirection += Time.fixedDeltaTime;

        switch (currentRange)
        {
            case Ranges.AboveLongRange: 
            {
                targetTransform = null;
                return;
            }
            case Ranges.LargeRange:
            {
                DirectionTowardTarget();
                break;
            }
            case Ranges.MediumRange:
            {
                SoftRotateArountTarget();
                break;
            }
            case Ranges.ShortRange:
            {
                RotateArountTarget();
                break;
            }
            case Ranges.MinRange:
            {
                movementController.Move(Vector3.zero);
                return;
            }
        }

        movementController.Move(transform.up);
    }

    private void PatrolFly()
    {
        ChangeToRandomDirection();
        movementController.Move(transform.up);
    }

    private void ChangeToRandomDirection()
    {
        if (timeToChangeDirection <= currentTime)
        {
            currentTime = 0f;
            transform.Rotate(new Vector3(0f, 0f, Random.Range(-90f, 90f)), Space.Self);
        }    
    }
    private void DirectionTowardTarget()
    {
        float angle = Lib.GetAngleBetweenTranfsorm(transform.position, targetTransform.position);
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
    private void SoftDirectionTowardTarget()
    {
        float angle = Lib.GetAngleBetweenTranfsorm(transform.position, targetTransform.position);
        transform.rotation = Quaternion.Euler(0f, 0f, angle - Random.Range(-30f, 30f));
    }
    private void SoftRotateArountTarget()
    {
        float angle = Lib.GetAngleBetweenTranfsorm(transform.position, targetTransform.position);
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 60f);
    }
    private void RotateArountTarget()
    {
        float angle = Lib.GetAngleBetweenTranfsorm(transform.position, targetTransform.position);
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f) ;
    }

    private void CheckRange(float range)
    {

        if(range > largeRangeToTarget)
        {
            currentRange = Ranges.AboveLongRange;
            return;
        }
        if(range > mediumRangeToTarget)
        {
            currentRange = Ranges.LargeRange;
            return;
        }
        if (range > shortRangeToTarget)
        {
            currentRange = Ranges.MediumRange;
            return;
        }
        if (range > minRangeToTarget)
        {
            currentRange = Ranges.ShortRange;
            return;
        }
        currentRange = Ranges.MinRange;
        return;
    }

    private float CalculateDistance()
    {
        if(targetTransform != null)
        {
            return (transform.position - targetTransform.position).magnitude;
        }
        return 0f;
    }
}
