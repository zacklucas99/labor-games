using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class OfficerController : MonoBehaviour
{
    // Start is called before the first frame update

    private NavMeshAgent agent;
    private RoutePoint[] points;
    public int pointIndex;
    public RouteVisualization route;
    private bool destinationSet = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (route)
        {
            points = route.GetPoints();
        }
        if (route && route.GetPoints().Length > 0)
        {

            GotoNextPoint();
        }

    }

    IEnumerator GotoNextPoint()
    {
        if (points[(pointIndex) % points.Length].isStoppable)
        {
            yield return new WaitForSeconds(points[(pointIndex) % points.Length].waitTime);
        }
        agent.SetDestination(points[(pointIndex++) % points.Length].transform.position);
        destinationSet = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f && !destinationSet && route!= null && route.GetPoints().Length > 1)
        {
            destinationSet = true;
            StartCoroutine(GotoNextPoint());
        };

    }


}
