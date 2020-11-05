using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementController))]

public class PermanentRotaion : MonoBehaviour
{
    [SerializeField] float rotationSpeed;

    private MovementController movementController;

    private void Awake()
    {
        movementController = GetComponent<MovementController>();
    }

    // Update is called once per frame
    void Update()
    {
        movementController.Rotate(rotationSpeed);
    }
}
