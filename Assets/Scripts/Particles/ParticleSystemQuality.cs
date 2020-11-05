using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent (typeof(ParticleSystem))]
public class ParticleSystemQuality : MonoBehaviour
{
    private ParticleSystem currentParticleSystem;
    private int startParticleCount;

    public void OnDespawn()
    {
        Messenger.RemoveListener<float>(GameEvents.CHANGE_QUALITY, ChangeQuality);
    }

    private void Awake()
    {
        currentParticleSystem = GetComponent<ParticleSystem>();
        currentParticleSystem.Pause();
        var main = currentParticleSystem.main;
        startParticleCount = main.maxParticles;
        StartCoroutine(GetCurrentQualityValue());       
    }

    private void Start()
    {       
        Messenger.AddListener<float>(GameEvents.CHANGE_QUALITY, ChangeQuality);   
    }

    private void ChangeQuality(float value)
    {
        if(currentParticleSystem == null)
        {
            currentParticleSystem = GetComponent<ParticleSystem>();
        }
        var main = currentParticleSystem.main;
        main.maxParticles = (int)(startParticleCount * value);        
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener<float>(GameEvents.CHANGE_QUALITY, ChangeQuality);
    }

    private IEnumerator GetCurrentQualityValue()
    {
        yield return new WaitForEndOfFrame();
        ChangeQuality(GameObject.FindGameObjectWithTag(LayersAndTags.GAMECONTROLLER_TAG).GetComponent<SettingsController>().GetCurrentQualityValue());
        currentParticleSystem.Play();
    }
}
