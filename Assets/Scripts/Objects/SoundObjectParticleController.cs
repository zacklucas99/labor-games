using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundObjectParticleController : MonoBehaviour
{
    public SoundObject soundObject;
    public ParticleSystem particleSystem;
    private bool turnedOnBefore;

    public void Update()
    {
        if (soundObject.turnedOn)
        {
            if (!turnedOnBefore)
            {
                particleSystem.Play();
            }
            turnedOnBefore = true;
        }
        else {
            turnedOnBefore = false;
            particleSystem.Stop();
        }
    }
}
