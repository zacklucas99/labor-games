using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //Script to make player only emit sound, if he is moving
    public ThirdPersonMovement player;
    public SoundObject obj;
    public DoorTrigger global_door;
    void Update()
    {
        if (player.IsMoving && !player.IsSneaking)
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


        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Got E");
            global_door.doorObject.Open();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var door = other.gameObject.GetComponent<DoorTrigger>();
        if (!door)
        {
            return;
        }
        if (door.isFrontTrigger)
        {
            door.doorObject.IsStandingAtFront = true;
            door.doorObject.IsStandingAtBack = false;
        }
        else if (door.isBackTrigger)
        {
            door.doorObject.IsStandingAtFront = false;
            door.doorObject.IsStandingAtBack = true;
        }
        global_door = door;
    }

    private void OnTriggerExit(Collider other)
    {
        if (global_door)
        {
            global_door.doorObject.IsStandingAtFront = false;
            global_door.doorObject.IsStandingAtBack = false;
        }
    }
}
