using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_AI : MonoBehaviour, IEnemy
{
    [SerializeField] GameObject[] weapons;
    [SerializeField] float detectRadius;

    private Transform targetTransform;

    public void Start()
    {
        if(targetTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag(LayersAndTags.PLAYER_TAG);
            if (player != null)
            {
                targetTransform = player.transform;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {      
        if(targetTransform != null)
        {
            if((transform.position - targetTransform.position).magnitude < detectRadius)
            {
                foreach(GameObject weapon in weapons)
                {
                    IWeapon weaponInterface = weapon.GetComponent<IWeapon>();
                    if(weaponInterface != null)
                    {
                        weaponInterface.RotateToTarget(targetTransform);
                        weaponInterface.Fire(targetTransform);
                    }                  
                }
            }
        }
        else
        {
            
            GameObject player = GameObject.FindGameObjectWithTag(LayersAndTags.PLAYER_TAG);
            if(player != null)
            {
                targetTransform = player.transform;
            }
        }
    }
}
