using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MovementController))]
public class PlayerInputController : MonoBehaviour
{
    private Joystick joystick;
    private MovementController movementController;
    private Vector2 _lockedMovementVector;
    private bool isLocked;

    private void Awake()
    {
        movementController = GetComponent<MovementController>();
        GameObject joyStickGameObject = GameObject.FindGameObjectWithTag(LayersAndTags.JOYSTICK_TAG);
        if(joyStickGameObject != null)
        {
            joystick = joyStickGameObject.GetComponent<Joystick>();
        }      
    }

    public void LockInput(Vector2 lockedMovement)
    {
        isLocked = true;
        _lockedMovementVector = lockedMovement;
    }

    // Update is called once per frame
    void Update()
    {
        if(isLocked == true)
        {
            movementController.Move(Lib.Vector3ToVector2(transform.position) + _lockedMovementVector);
            return;
        }
        if(joystick != null)
        {
            Vector2 direction = joystick.Direction;
            if (direction.magnitude > 0.1f)
            {
                movementController.Move(joystick.Direction);
                movementController.RotateToward(new Vector3(joystick.Direction.x, joystick.Direction.y, transform.position.z) + transform.position);
            }
            else
            {
                movementController.Move(Vector2.zero);
            }
        }       
    }
}
