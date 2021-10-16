using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BindFieldOfViewToBone : MonoBehaviour
{
    public GameObject bone;

    // Update is called once per frame
    void Update()
    {
        transform.position = bone.transform.position;
        var euler_angles = bone.transform.eulerAngles;
        euler_angles.x = 0;
        transform.rotation = Quaternion.Euler(euler_angles);
        
    }
}
