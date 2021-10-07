using System.Collections;
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
    public int waitTime = 5;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        points = route.GetPoints();

        GotoNextPoint();
    }

    IEnumerator GotoNextPoint()
    {
        if (points[(pointIndex + 1) % points.Length].isStoppable)
        {
            yield return new WaitForSeconds(waitTime);
        }
        agent.SetDestination(points[(pointIndex++) % points.Length].transform.position);
        destinationSet = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f && !destinationSet)
        {
            destinationSet = true;
            StartCoroutine(GotoNextPoint());
        } 
    }
}
