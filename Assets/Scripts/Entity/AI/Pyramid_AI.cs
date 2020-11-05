using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementController))]
[RequireComponent(typeof(Stats))]
public class Pyramid_AI : MonoBehaviour, IEnemy
{
    [SerializeField] Gun[] guns;
    [SerializeField] float minimalDistance;
    [SerializeField] float shotDelay;


    private MovementController movementController;
    private Transform targetTranfsorm;
    private Stats stats;
    private float currentShotDelay;

    private void Awake()
    {
        movementController = GetComponent<MovementController>();
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
            targetTranfsorm = player.transform;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {     
        if(targetTranfsorm != null)
        {
            if((targetTranfsorm.position - transform.position).magnitude > minimalDistance)
            {
                movementController.Move((targetTranfsorm.position - transform.position).normalized);
            }
            else
            {
                movementController.Stop(0.1f);
            }

            if(currentShotDelay > shotDelay)
            {
                SwitchShot();
                currentShotDelay = 0;
            }
            else
            {
                currentShotDelay += Time.deltaTime;
            }
        }
        else
        {
            GetPlayerTransform();
        }

    }

    private int previousShotedGunIndex = 0;

    private void SwitchShot()
    {
        if(previousShotedGunIndex > guns.Length - 1)
        {
            previousShotedGunIndex = 0;
        }
        guns[previousShotedGunIndex].Fire();
        previousShotedGunIndex++;
    }

}
