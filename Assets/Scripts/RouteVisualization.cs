using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RouteVisualization : MonoBehaviour
{
    public float point_radius = 1f;
    public bool isVisible = true;
    public Color color;
    private void OnDrawGizmos()
    {
        if (!isVisible)
        {
            return;
        }

        Gizmos.color = color;
        if(transform.childCount > 0)
        {
            Gizmos.DrawCube(transform.GetChild(0).transform.position,new Vector3(point_radius, point_radius, point_radius));
            var pos = transform.GetChild(0).transform.position;
            Handles.Label(new Vector3(pos.x, pos.y + point_radius, pos.z), "Start");
        }
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            var pos = transform.GetChild(i + 1).transform.position;
            Handles.Label(new Vector3(pos.x, pos.y + point_radius, pos.z), "" + (i + 1));
            Gizmos.DrawLine(transform.GetChild(i).transform.position, transform.GetChild(i + 1).transform.position);
        }

        if (transform.childCount > 1)
        {
            var pos = transform.GetChild(transform.childCount - 1).transform.position;
            Gizmos.DrawSphere(transform.GetChild(transform.childCount-1).transform.position, point_radius);
            Handles.Label(new Vector3(pos.x, pos.y + point_radius*2, pos.z), "End");

        }
    }

    public RoutePoint[] GetPoints()
    {
        RoutePoint[] points = new RoutePoint[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            points[i] = transform.GetChild(i).GetComponent<RoutePoint>();
        }
        return points;
    }
}
