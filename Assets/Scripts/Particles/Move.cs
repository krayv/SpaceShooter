using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] float speedMultiplier;
    void Update()
    {
        transform.position += (Vector3.up * Time.deltaTime) * speedMultiplier;
    }
}
