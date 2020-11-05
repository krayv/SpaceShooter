using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    [SerializeField] float lifeTime;

    private float existTime;


    // Update is called once per frame
    void Update()
    {
        if(existTime > lifeTime)
        {
            Destroy(gameObject);
        }
        else
        {
            existTime += Time.deltaTime;
        }
        
    }
}
