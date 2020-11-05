using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Stats))]

public class MovementController : MonoBehaviour
{
    [SerializeField] Rigidbody2D curRigidbody;
    [SerializeField] Stats stats;

    private void Awake()
    {
        curRigidbody = GetComponent<Rigidbody2D>();
        stats = GetComponent<Stats>();
    }
    public void Move(Vector2 movementDelta)
    {
        movementDelta *= stats.GetMaxSpeed();
        movementDelta = Vector2.ClampMagnitude(movementDelta, stats.GetMaxSpeed());
        if (curRigidbody != null)
        {
            
            curRigidbody.velocity = movementDelta;
            stats.SetCurSpeed(movementDelta.magnitude);
        }
        else
        {
            transform.position += new Vector3(movementDelta.x, movementDelta.y, 0f) * Time.deltaTime;
        }
        
    }

    public void Move(Vector2 movementDelta, float speedMultiplier)
    {
        movementDelta *= speedMultiplier;
        movementDelta = Vector2.ClampMagnitude(movementDelta, speedMultiplier);
        if (curRigidbody != null)
        {
            
            curRigidbody.velocity = movementDelta;
            stats.SetCurSpeed(movementDelta.magnitude);
        }
        else
        {
            transform.position += new Vector3(movementDelta.x, movementDelta.y, 0f) * Time.deltaTime;
        }
    }

    public void Stop(float speed)
    {
        if(curRigidbody != null)
        {
            curRigidbody.velocity = Vector2.Lerp(curRigidbody.velocity, Vector2.zero, speed);
        }     
    }

    public void RotateToward(Vector2 point)
    {
        float angle = Lib.GetAngleBetweenTranfsorm(transform.position, point);
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    public void RotateToward(Vector2 point, float rotationsSpeed, float maxDegreesToRotate)
    {
        float angle = Lib.GetAngleBetweenTranfsorm(transform.position, point);
        float angleDelta = (Quaternion.Angle(transform.rotation, Quaternion.Euler(0f, 0f, angle)));
        if(angleDelta < maxDegreesToRotate)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, angle), rotationsSpeed);
        }       
    }

    public void Rotate(float value)
    {
        transform.Rotate(0, 0, value);
    }

    public IEnumerator RotateCorountin(float angle, float speed)
    {
        Quaternion newRotation = Quaternion.Euler(0f, 0f, transform.rotation.z + angle);
        while(transform.localRotation != newRotation)
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, newRotation, speed * Time.deltaTime);
            yield return null;
        }
        
    }

    public bool IsHaveRigidBody()
    {
        return curRigidbody != null;
    }

}
