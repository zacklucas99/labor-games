using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GramophoneSound : MonoBehaviour
{
    private AudioSource audio;
    private SoundObject soundObj;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        soundObj = GetComponent<SoundObject>();
    }

    void Update()
    {
        if(soundObj.turnedOn && !audio.isPlaying)
        {
            audio.Play();
        }
        if (!soundObj.turnedOn && audio.isPlaying)
        {
            audio.Stop();
        }
    }
}
