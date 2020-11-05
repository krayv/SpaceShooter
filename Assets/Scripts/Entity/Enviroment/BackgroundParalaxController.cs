using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParalaxController : MonoBehaviour
{
    [SerializeField] Transform targetTransform;
    [SerializeField] Vector3 startCameraPositionDelta;
    [SerializeField] float offset;
    private float zPosition;
    private Vector3 previousTargetPosition;

    private void Awake()
    {
        zPosition = transform.position.z;     
    }

    private void Start()
    {
        Messenger.AddListener<GameObject>("ENTITY_DEAD", IsTargetDead);
        GameObject playerGameObject = GameObject.FindWithTag(LayersAndTags.PLAYER_TAG);
        if(playerGameObject != null)
        {
            targetTransform = playerGameObject.transform;
        }
        previousTargetPosition = targetTransform.position;


    }
    void Update()
    {
        if (targetTransform != null)
        {
            offset = Mathf.Clamp(offset, 0f, 1f);
            Vector3 delta = targetTransform.position - previousTargetPosition;
            delta *= offset;
            transform.position = transform.position + delta;
            transform.position = new Vector3(transform.position.x, transform.position.y, zPosition);
            previousTargetPosition = targetTransform.position;
        }
        else
        {
            GameObject playerGameObject = GameObject.FindWithTag(LayersAndTags.PLAYER_TAG);
            if (playerGameObject != null)
            {
                targetTransform = playerGameObject.transform;
            }
        }

    }

    public void SetOffset(float value)
    {
        value = Mathf.Clamp(value, 0f, 1f);
        offset = value;
    }

    private void IsTargetDead(GameObject targerObject)
    {
        if (targerObject.transform == targetTransform)
        {
            targetTransform = null;
        }
    }
}
