using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedMovementFromTo : MonoBehaviour
{
    [SerializeField] float movementSpeed;

    [SerializeField] private Transform pointFrom;
    [SerializeField] private Transform pointTo;

    private Transform targetPoint;
    private bool isBacking;


    // Start is called before the first frame update
    void Start()
    {
        transform.position = pointFrom.position;
        targetPoint = pointTo;
        isBacking = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPoint.position, movementSpeed * Time.deltaTime);
        if((transform.position - targetPoint.position).magnitude < 0.3f)
        {
            targetPoint = isBacking ? pointFrom : pointTo;
            isBacking = !isBacking;
        }
    }
}
