using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Color color;
    public bool drawGizmos;
    private void OnDrawGizmos()
    {


        if (!drawGizmos)
        {
            return;
        }
        
        Gizmos.color = color;
        for (int i = 0; i < transform.childCount; i++)
        {
            var pos = transform.GetChild(i).transform.position;
            Handles.DrawBezier(transform.position, pos, Vector3.zero, new Vector3(pos.x, transform.position.y, pos.z), color, null, 2f);
        }
    }

    public void TurnOff()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var camera = transform.GetChild(i).GetComponent<CameraController>();
            camera.TurnOff();
        }
    }
}
