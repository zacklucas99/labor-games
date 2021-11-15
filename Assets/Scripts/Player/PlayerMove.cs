using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //Script to make player only emit sound, if he is moving
    public ThirdPersonMovement player;
    public SoundObject obj;
    void Update()
    {
        if (player.IsMoving)
        {
            if (!obj.enabled)
            {
                obj.turnedOn = true;
            }
        }
        else
        {
            if (obj.enabled)
            {
                obj.turnedOn = false;
            }
        }
    }
}
