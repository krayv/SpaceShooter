using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class FPS : MonoBehaviour
{
    private Text text;
    private float i = 1;
    private void Awake()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {      
        if(i <= 0f)
        {
            text.text = ((int)(1f / Time.deltaTime)).ToString();
            i = 1;
        }
        i -= Time.deltaTime;
    }
}
