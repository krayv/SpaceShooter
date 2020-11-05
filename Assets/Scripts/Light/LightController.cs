using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightController : MonoBehaviour
{
    [SerializeField] private Light2D lightWithNormalMap;
    [SerializeField] private Light2D lightWithoutNormalMap;

    private void Awake()
    {
        StartCoroutine(GetCurrentQualityValue());
    }


    void Start()
    {
        Messenger.AddListener<float>(GameEvents.CHANGE_QUALITY, OnChangeQuality);
    }

    public void OnDespawn()
    {
        Messenger.RemoveListener<float>(GameEvents.CHANGE_QUALITY, OnChangeQuality);
    }

    
    private void OnChangeQuality(float value)
    {
        if(value <= 0.35f)
        {
            lightWithNormalMap.gameObject.SetActive(false);
            lightWithoutNormalMap.gameObject.SetActive(true);
        }
        else
        {
            lightWithNormalMap.gameObject.SetActive(true);
            lightWithoutNormalMap.gameObject.SetActive(false);
        }

    }
    private IEnumerator GetCurrentQualityValue()
    {
        yield return new WaitForEndOfFrame();
        OnChangeQuality(GameObject.FindGameObjectWithTag(LayersAndTags.GAMECONTROLLER_TAG).GetComponent<SettingsController>().GetCurrentQualityValue());
    }
}
