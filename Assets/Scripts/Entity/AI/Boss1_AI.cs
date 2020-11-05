using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Boss1_AI : MonoBehaviour, IEnemy
{
    [SerializeField] Gun[] gunsGroup1;
    [SerializeField] Gun[] rotationsGuns;
    [SerializeField] Gun[] longRangeGun;
    [SerializeField] Gun[] sweepGuns;
    [SerializeField] private float baseTimeToChangeAttackPattern;
    [SerializeField] private float baseAttackDelay;
    [SerializeField] private float baseTimeToChangeDirection;
    [SerializeField] private float roationSpeed;
    [SerializeField] private float minRangeForAttackFromLongRangeWeapon;

    private MovementController movementController;
    private Transform targetTransform;
    private Stats stats;
    private Collider2D currentCollider2D;
    private BossStats bossStats;

    private float currentAttackDelay;
    private float attackDelay;

    private float currentChangePatternDelay;
    private float changePatternDelay;

    private float timeToChangeDirection;
    private float currentChangeDirectionDelay;

    private Action currentAttackPattern;
    


    // Start is called before the first frame update
    private void Awake()
    {
        currentAttackPattern = SingleAttackFromAllGunsGroup1;
        movementController = GetComponent<MovementController>();
        stats = GetComponent<Stats>();
        currentCollider2D = GetComponent<Collider2D>();
        bossStats = GetComponent<BossStats>();
    }

    private void Start()
    {
        GetPlayerTransform();
    }

    private void GetPlayerTransform()
    {
        GameObject player = GameObject.FindGameObjectWithTag(LayersAndTags.PLAYER_TAG);
        if (player != null)
        {
            targetTransform = player.transform;
        }
    }

    private float GetAttackDelay()
    {
        return baseAttackDelay * UnityEngine.Random.Range(1f, 2f) / GetCurrentAnger(); 
    }
    private float GetDirectionDelay()
    {
        return baseTimeToChangeDirection * UnityEngine.Random.Range(1f, 2f);
    }
    private float GetChangePatternDelay()
    {
        return baseTimeToChangeAttackPattern * UnityEngine.Random.Range(1f, 2f) / GetCurrentAnger();
    }

    private float GetCurrentAnger()
    {
        float currentAnger = stats.GetCurLife() == 0 ? 1 : stats.GetMaxLife() / stats.GetCurLife();
        currentAnger = Mathf.Clamp(currentAnger, 1f, 3f);
        return currentAnger + (bossStats.GetCurrentIteration() - 1);
    }

    private void Walk()
    {
        if (currentChangeDirectionDelay > timeToChangeDirection)
        {
            float angle = GetRandomAngle();
            StartCoroutine(movementController.RotateCorountin(angle, roationSpeed));
            currentChangeDirectionDelay = 0f;
            timeToChangeDirection = GetDirectionDelay();

        }
        movementController.Move(transform.up);
        currentChangeDirectionDelay += Time.deltaTime;
    }

    private void Attack()
    {
        if(targetTransform != null)
        {
            if (currentChangePatternDelay > changePatternDelay)
            {
                int randomValue = UnityEngine.Random.Range(0, 9);
                if (randomValue >= 0 && randomValue < 3)
                {
                    currentAttackPattern = SingleAttackFromAllGunsGroup1;
                }
                else if (randomValue >= 4 && randomValue < 6)
                {
                    currentAttackPattern = RotationAttackFromAllGunsGroup1;
                }
                else if (randomValue >= 6 && randomValue < 8)
                {
                    currentAttackPattern = SweepAttack;
                }
                else if(randomValue >= 8 && randomValue < 9)
                {
                    currentAttackPattern = AttackFromRotationGuns;
                }

                changePatternDelay = GetChangePatternDelay();
                currentChangePatternDelay = 0f;
            }
            if ((Lib.Vector3ToVector2(targetTransform.position) - currentCollider2D.ClosestPoint(targetTransform.position)).magnitude > minRangeForAttackFromLongRangeWeapon)
            {
                currentAttackPattern = AttackFromLongRangeWeapon;
            }
            currentChangePatternDelay += Time.deltaTime;
            if (currentAttackDelay > attackDelay)
            {
                currentAttackPattern();
                attackDelay = GetAttackDelay();
                currentAttackDelay = 0;
            }
            currentAttackDelay += Time.deltaTime;
        }
        else
        {
            GetPlayerTransform();
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Walk();
        Attack();
    }

    private float GetRandomAngle()
    {
        float angle = UnityEngine.Random.Range(5f, 15f);
        return UnityEngine.Random.Range(-1, 1) < 0 ? -angle : angle;
    }

    private void SingleAttackFromAllGunsGroup1()
    {
        foreach(Gun gun in gunsGroup1)
        {
            gun.Fire();
        }
    }
    private void RotationAttackFromAllGunsGroup1()
    {
        StartCoroutine(CorountinRotationAttackFromAllGunsGroup1(UnityEngine.Random.Range(1,3)));
    }
    private IEnumerator CorountinRotationAttackFromAllGunsGroup1(int rotationsCount)
    {
        for(int i = 0; i <= rotationsCount; i++)
        {
            foreach (Gun gun in gunsGroup1)
            {
                gun.Fire();
                yield return new WaitForSeconds(1f);
            }
        }       
    }
    private void AttackFromRotationGuns()
    {
        StartCoroutine(CorountinAttackFromRotationGuns(UnityEngine.Random.Range(1f, 4f)));
    }

    private IEnumerator CorountinAttackFromRotationGuns(float timeAttack)
    {
        while(timeAttack > 0f)
        {
            foreach(Gun gun in rotationsGuns)
            {
                gun.Rotate(360f * Time.deltaTime);
                gun.Fire();
            }
            timeAttack -= Time.deltaTime;
            yield return null;
        }
        
    }

    private void SweepAttack()
    {
        StartCoroutine(CorountinSweepAttack(1f));
    }

    private IEnumerator CorountinSweepAttack(float timeAttack)
    {
        Vector2 targetPoint = targetTransform.position;
        while (timeAttack > 0)
        {
            foreach (Gun gun in sweepGuns)
            {
                gun.RotateToTarget(targetPoint);
                gun.Fire();
            }
            timeAttack -= Time.deltaTime;
            yield return null;
        }
    }

    private void AttackFromLongRangeWeapon()
    {
        StartCoroutine(CorountinAttackFromLongRangeWeapon(1f));
    }

    private IEnumerator CorountinAttackFromLongRangeWeapon(float timeAttack)
    {
        while(timeAttack > 0)
        {
            foreach(Gun gun in longRangeGun)
            {
                gun.RotateToTarget(targetTransform);
                gun.Fire(targetTransform);
            }
            timeAttack -= Time.deltaTime;
            yield return null;
        }
        
    }
}
