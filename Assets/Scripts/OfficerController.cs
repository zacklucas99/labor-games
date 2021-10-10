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
    private RoutePoint lastPoint;

    public int pointIndex;
    public RouteVisualization route;
    private bool destinationSet = false;

    public Color lostColor;
    public Color foundColor;

    public Transform goBackDestination;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (route)
        {
            points = route.GetPoints();
        }
        if (route && route.GetPoints().Length > 0)
        {

            GotoNextPoint(false);
        }

    }

    IEnumerator GotoNextPoint( bool wait)
    {
        Debug.Log("GoToNextPoint");
        if (points[(pointIndex) % points.Length].isStoppable && wait)
        {
            yield return new WaitForSeconds(points[(pointIndex) % points.Length].waitTime);
        }
        lastPoint = points[pointIndex % points.Length];

        agent.SetDestination(points[(pointIndex++) % points.Length].transform.position);
        if(pointIndex >= points.Length)
        {
            pointIndex = 0;
        }
        destinationSet = false;
    }


    // Update is called once per frame
    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f && !destinationSet && route!= null && route.GetPoints().Length > 1)
        {
            Debug.Log("Go To Next Point");
            destinationSet = true;
            StartCoroutine(GotoNextPoint(true));
        };

    }

    public void FoundPlayer(GameObject player) {
        GetComponent<MeshRenderer>().material.color = foundColor;
        agent.SetDestination(player.transform.position);
        goBackDestination = lastPoint.transform;

    }

    public void LostPlayer()
    {
        GetComponent<MeshRenderer>().material.color = lostColor;
        destinationSet = false;

        agent.SetDestination(goBackDestination.position);
        Debug.Log("Lost_player");


    }


}
