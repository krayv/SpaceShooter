using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineController : MonoBehaviour
{
    [SerializeField] ParticleSystem[] curParticleSystems;
    [SerializeField] float startLifetime = 1f;
    
    public void EngineOn()
    {
        foreach (ParticleSystem particle in curParticleSystems)
        {
            ParticleSystem.MainModule newMain = particle.main;
            newMain.startLifetime = startLifetime;
        }
    }
    public void EngineOff()
    {
        foreach (ParticleSystem particle in curParticleSystems)
        {
            ParticleSystem.MainModule newMain = particle.main;
            newMain.startLifetime = 0f;
        }
    }
}
