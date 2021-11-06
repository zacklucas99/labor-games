using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update

    public List<GameObject> rotPoints;
    public GameObject bone;
    float speed = 1;
    int currentIndex = 0;

    public float angleThreshold = 0.1f;
    private bool rotatedToTarget = false;

    public float cameraNotifyRad = 10;
    public bool RotatedToTarget => rotatedToTarget;

    public Color cameraGizmosColor;
    public bool drawGizmos = true;

    public Color playerFoundColor;
    public Color playerLostColor;

    public Vector3 playerPosition;

    public bool FoundPlayer { private set; get; }

    public MeshRenderer meshRenderer;
    void Start()
    {
        currentIndex = rotPoints.Count - 1;


        GameObject rotPoint = rotPoints[currentIndex];
        Vector3 targetDirection = rotPoint.transform.position - bone.transform.position;

        // Rotate the forward vector towards the target direction by one step
        Vector3 newDirection = Vector3.RotateTowards(bone.transform.forward, targetDirection, 360f, 0f);
        rotatedToTarget = Mathf.Abs(Vector3.Angle(targetDirection, newDirection)) < angleThreshold;
        // Draw a ray pointing at our target in
        Debug.DrawRay(bone.transform.position, newDirection, Color.red);

        // Calculate a rotation a step closer to the target and applies rotation to this object
        bone.transform.rotation = Quaternion.LookRotation(newDirection);
    }

    public void IncrementIndex()
    {
        currentIndex = (currentIndex + 1) % rotPoints.Count;
    }


    // Update is called once per frame

    // Taken from: https://docs.unity3d.com/ScriptReference/Vector3.RotateTowards.html
    public void Rotate()
    {
        // Determine which direction to rotate towards
        GameObject rotPoint = rotPoints[currentIndex];
        Vector3 targetDirection = rotPoint.transform.position - bone.transform.position;

        // The step size is equal to speed times frame time.
        float singleStep = speed * Time.deltaTime;

        // Rotate the forward vector towards the target direction by one step
        Vector3 newDirection = Vector3.RotateTowards(bone.transform.forward, targetDirection, singleStep, 0f);
        rotatedToTarget = Mathf.Abs(Vector3.Angle(targetDirection, newDirection)) < angleThreshold;
        // Draw a ray pointing at our target in
        Debug.DrawRay(bone.transform.position, newDirection, Color.red);

        // Calculate a rotation a step closer to the target and applies rotation to this object
        bone.transform.rotation = Quaternion.LookRotation(newDirection);
    }

    public void FollowPlayer()
    {
        // Determine which direction to rotate towards
        Vector3 targetDirection = playerPosition - bone.transform.position;

        // The step size is equal to speed times frame time.
        float singleStep = speed * Time.deltaTime;

        // Rotate the forward vector towards the target direction by one step
        Vector3 newDirection = Vector3.RotateTowards(bone.transform.forward, targetDirection, singleStep, 0f);
        rotatedToTarget = Mathf.Abs(Vector3.Angle(targetDirection, newDirection)) < angleThreshold;
        // Draw a ray pointing at our target in
        Debug.DrawRay(bone.transform.position, newDirection, Color.red);

        // Calculate a rotation a step closer to the target and applies rotation to this object
        bone.transform.rotation = Quaternion.LookRotation(newDirection);
    }

    public void PlayerFound(GameObject playerObj)
    {
        Debug.Log("Player found");
        meshRenderer.material.color = playerFoundColor;
        playerPosition = playerObj.transform.position;
        FoundPlayer = true;
    }

    public void LostPlayer()
    {
        meshRenderer.material.color = playerLostColor;
        playerPosition = Vector3.zero;
        FoundPlayer = false;
    }

    private void OnDrawGizmos()
    {
        if (!drawGizmos)
        {
            return;
        }

        Handles.color = cameraGizmosColor;
        Handles.DrawWireDisc(transform.position, Vector3.up, cameraNotifyRad);
    }
}
