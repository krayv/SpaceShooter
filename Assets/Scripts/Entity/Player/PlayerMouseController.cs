using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(MovementController))]
public class PlayerMouseController : MonoBehaviour
{
    [SerializeField] private MovementController movemenController;
    private Vector2 _targetPos;
    private Vector2 _lockedMovementVector;
    private bool isLocked;

    public void LockInput(Vector2 lockedMovement)
    {
        isLocked = true;
        _lockedMovementVector = lockedMovement;
    }
    
    void Awake()
    {
        movemenController = GetComponent<MovementController>();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if(isLocked)
        {
            movemenController.Move(Lib.Vector3ToVector2(transform.position) + _lockedMovementVector);
            return;
        }
        Vector2 movementDelta = Vector2.zero;
        if (Input.GetMouseButton(0))
        {
            RaycastHit2D rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));
            if (rayHit)
            {
                GameObject hitObject = rayHit.transform.gameObject;
                _targetPos = rayHit.point;
                movemenController.RotateToward(_targetPos);
            }

        }
        if (!Input.GetMouseButton(0))
        {
            _targetPos = Vector3.zero;

        }
        if (_targetPos != Vector2.zero)
        {
            if ((new Vector2(transform.position.x, transform.position.y) - new Vector2(_targetPos.x, _targetPos.y)).magnitude < 0.2f)
            {
                _targetPos = Vector3.zero;
            }
            else
            {
                movementDelta = Vector2.MoveTowards(transform.position, _targetPos, 1);
                movementDelta -= new Vector2(transform.position.x, transform.position.y);
                if (movementDelta.magnitude < 0.2f && movementDelta.magnitude > 0)
                {
                    movementDelta = new Vector3(Mathf.Abs(movementDelta.x) < 0.3f && Mathf.Abs(movementDelta.x) > 0 ? (0.3f * movementDelta.x) / Mathf.Abs(movementDelta.x) : 0, 0
                        , Mathf.Abs(movementDelta.y) < 0.3f && Mathf.Abs(movementDelta.y) > 0 ? (0.3f * movementDelta.y) / Mathf.Abs(movementDelta.y) : 0);
                }
            }
        }
        movemenController.Move(movementDelta);
    }
}
