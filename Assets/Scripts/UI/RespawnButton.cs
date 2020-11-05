using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RespawnButton : MonoBehaviour
{
    [SerializeField] float lifeTime;
    [SerializeField] Text timerText;

    private float currentLifeTime;

    // Start is called before the first frame update
    void Start()
    {
        currentLifeTime = lifeTime;
    }

    // Update is called once per frame
    void Update()
    {
        timerText.text = ((int)currentLifeTime).ToString();
        currentLifeTime -= Time.deltaTime;
        if(currentLifeTime <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
