using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamButton : MonoBehaviour
{
    public GameObject pressed;
    public GameObject notPressed;

    public void press()
    {
        Debug.Log("cam button logic");

        notPressed.SetActive(false);
        pressed.SetActive(true);

        GameObject[] objs = GameObject.FindGameObjectsWithTag("SecurityCam");
        foreach(GameObject cam in objs)
        {
            cam.GetComponent<CameraController>().TurnOff();
        }
    }
}
