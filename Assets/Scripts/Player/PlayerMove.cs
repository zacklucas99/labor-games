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
            if (!obj.turnedOn)
            {
                obj.turnedOn = true;
            }
        }
        else
        {
            if (obj.turnedOn)
            {
                obj.turnedOn = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var door = other.gameObject.GetComponent<DoorTrigger>();
        if (door.isFrontTrigger)
        {
            door.doorObject.IsStandingAtFront = true;
            door.doorObject.IsStandingAtBack = false;
        }
        else if (door.isFrontTrigger)
        {
            door.doorObject.IsStandingAtFront = false;
            door.doorObject.IsStandingAtBack = true;
        }
    }
}
