using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraRenderer : MonoBehaviour
{

    private Camera currentCamera;

    private void Awake()
    {
        currentCamera = GetComponent<Camera>();
        currentCamera.backgroundColor = new Color(Random.Range(0f, 0.2f), Random.Range(0f, 0.2f), Random.Range(0f, 0.2f));
    }
}
