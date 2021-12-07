using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsCamera : MonoBehaviour
{
    public Camera mainCamera;
    // Update is called once per frame

    public void Start()
    {
        if(mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }
    void Update()
    {
        // Determine which direction to rotate towards
        Vector3 targetDirection = transform.position - mainCamera.transform.position;

        // The step size is equal to speed times frame time.
        float singleStep = 360 * Time.deltaTime;

        // Rotate the forward vector towards the target direction by one step
        Vector3 newDirection = Vector3.RotateTowards(mainCamera.transform.forward, targetDirection, singleStep, 0f);
        // Draw a ray pointing at our target in
        Debug.DrawRay(mainCamera.transform.position, newDirection, Color.red);

        // Calculate a rotation a step closer to the target and applies rotation to this object
        transform.rotation = Quaternion.LookRotation(newDirection);
    }
}
