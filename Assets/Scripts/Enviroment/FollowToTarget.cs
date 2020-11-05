using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowToTarget : MonoBehaviour
{
    [SerializeField] Transform targetTransform;
    [SerializeField] Vector3 delta = Vector3.zero;

    private void Start()
    {
        Messenger.AddListener<GameObject>("ENTITY_DEAD", IsTargetDead);
    }
 
    // Update is called once per frame
    void LateUpdate()
    {
        if (targetTransform == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag(LayersAndTags.PLAYER_TAG);
            if (playerObject == null)
            {

            }
            else
            {
                targetTransform = playerObject.transform;
            }
        }
        if (targetTransform != null)
        {
            transform.position = new Vector3(targetTransform.position.x, targetTransform.position.y, transform.position.z) - delta;
        }
        
    }

    private void IsTargetDead(GameObject targerObject)
    {
        if(targerObject.transform == targetTransform)
        {
            targetTransform = null;
        }
    }

    public void SetTarget(Transform target)
    {
        targetTransform = target;
    }
    
}
