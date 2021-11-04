using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject rotPoint;
    public GameObject bone;
    float speed = 1;
    void Start()
    {
        
    }

    // Update is called once per frame

    // Taken from: https://docs.unity3d.com/ScriptReference/Vector3.RotateTowards.html
    void Update()
    {
        // Determine which direction to rotate towards
        Vector3 targetDirection = rotPoint.transform.position - bone.transform.position;

        // The step size is equal to speed times frame time.
        float singleStep = speed * Time.deltaTime;

        // Rotate the forward vector towards the target direction by one step
        Vector3 newDirection = Vector3.RotateTowards(bone.transform.forward, targetDirection, singleStep, 0f);

        // Draw a ray pointing at our target in
        Debug.DrawRay(bone.transform.position, newDirection, Color.red);

        // Calculate a rotation a step closer to the target and applies rotation to this object
        bone.transform.rotation = Quaternion.LookRotation(newDirection);
    }
}
