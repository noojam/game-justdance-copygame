using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    public ParticleSystem ParticleSystem;
    private void OnParticle()
    {
        ParticleSystem.Play();
    }
}
