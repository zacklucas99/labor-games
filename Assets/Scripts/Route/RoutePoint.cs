using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoutePoint : MonoBehaviour
{
    public static float routePointRad = 0.5f;
    
    public bool isStoppable = true;
    public int waitTime = 5;
    private void OnDrawGizmos()
    {
        if (transform.parent.GetComponent<RouteVisualization>().isVisible)
        {
            Gizmos.color = transform.parent.GetComponent<RouteVisualization>().color;
            if (isStoppable)
            {
                Gizmos.DrawSphere(transform.position, routePointRad);
            }
            else
            {
                Gizmos.DrawCube(transform.position, new Vector3(routePointRad, routePointRad, routePointRad));
            }
        }
    }
}
