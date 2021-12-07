using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public SoundObject soundObject;

    public float maxTime = 2f;

    private float timer;

    private void OnCollisionEnter(Collision collision)
    {
        soundObject.turnedOn = true;
        timer = maxTime;
    }



    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0 && soundObject.turnedOn)
        {
            soundObject.turnedOn = false;
        }
    }
}
