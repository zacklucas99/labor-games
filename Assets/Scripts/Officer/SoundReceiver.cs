using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface SoundReceiver 
{
    public void ReceiveSound(SoundObject obj, float receiveVolume);
}
