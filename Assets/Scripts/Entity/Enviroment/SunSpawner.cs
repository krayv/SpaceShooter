using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[RequireComponent(typeof(BackgroundParalaxController))] 
[RequireComponent(typeof(LightController))]
public class SunSpawner : AbstractSpawnable
{
    [SerializeField] private ParticleSystem corona;
    [SerializeField] private ParticleSystem plasma;
    [SerializeField] private GameObject mesh;

    private Color mainColor;

    public override void OnSpawn(Vector3 position)
    {
        transform.position = position;
        float size = 1f - Random.Range(-1f, 0.8f);
        transform.localScale *= size;
        plasma.transform.localScale *= size;
        corona.transform.localScale *= size;

        BackgroundParalaxController paralaxController = GetComponent<BackgroundParalaxController>();
        paralaxController.SetOffset(Random.Range(0.95f, 0.995f));

        var plasmaColorOverTime = plasma.colorOverLifetime;
        plasmaColorOverTime.enabled = true;
        Gradient plasmaGradient = GetGradient();
        plasmaColorOverTime.color = plasmaGradient;
        Renderer meshMaterial = mesh.GetComponent<Renderer>();
        meshMaterial.material.color = plasmaGradient.colorKeys[0].color;

        var coronaColorOverTime = corona.colorOverLifetime;
        coronaColorOverTime.enabled = true;
        coronaColorOverTime.color = GetGradient();

    }
    public override void Despawn()
    {
        ParticleSystemQuality[] particleSystemQualities = GetComponentsInChildren<ParticleSystemQuality>();
        foreach(ParticleSystemQuality particleSystemQuality in particleSystemQualities)
        {
            particleSystemQuality.OnDespawn();
        }
        LightController lightController = GetComponent<LightController>();
        lightController.OnDespawn();
        Destroy(gameObject);
    }

    private Gradient GetGradient()
    {
        Gradient gradient = new Gradient();
        GradientColorKey[] colorkeys;
        GradientAlphaKey[] alphaKeys;
        colorkeys = new GradientColorKey[2];
        Color color = GetColor();
        colorkeys[0].color = color;
        colorkeys[0].time = 0f;
        colorkeys[1].color = color;
        colorkeys[1].time = 1f;
        alphaKeys = new GradientAlphaKey[4];
        alphaKeys[0].alpha = 0f;
        alphaKeys[0].time = 0f;
        alphaKeys[1].alpha = 1f;
        alphaKeys[1].time = 0.15f;
        alphaKeys[2].alpha = 1f;
        alphaKeys[2].time = 0.85f;
        alphaKeys[3].alpha = 0f;
        alphaKeys[3].time = 1f;
        gradient.alphaKeys = alphaKeys;
        gradient.colorKeys = colorkeys;
        return gradient;
    }

    private Color GetColor()
    {
        return new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
    }
}
