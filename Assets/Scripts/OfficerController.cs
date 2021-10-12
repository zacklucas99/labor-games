using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

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

    private Transform goBackDestination;


    private ThirdPersonCharacter character;

    private bool isFollowingPlayer;

    public float playerFollowingSpeed = 1f;
    public float walkingSpeed = 0.5f;

    public bool setToStartPoint;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (route)
        {
            points = route.GetPoints();
        }
        if(setToStartPoint &&points.Length > 0)
        {
            transform.position = points[0].transform.position;
            if (points.Length > 1)
            {
                transform.LookAt(points[1].transform.position);
            }
        }
        if (route && route.GetPoints().Length > 0)
        {

            GotoNextPoint(false);
        }

        character = GetComponent<ThirdPersonCharacter>();

    }

    IEnumerator GotoNextPoint( bool wait)
    {
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
        if (!agent.pathPending && agent.remainingDistance < agent.stoppingDistance && !destinationSet && route!= null && route.GetPoints().Length > 1)
        {
            destinationSet = true;
            StartCoroutine(GotoNextPoint(true));
        };

        if (agent.remainingDistance > agent.stoppingDistance) {
            character.Move(agent.desiredVelocity.normalized * (isFollowingPlayer?playerFollowingSpeed:walkingSpeed), false, false);
        }
        else
        {
            character.Move(Vector3.zero, false, false);
        }

    }

    public void FoundPlayer(GameObject player) {
        GetComponent<MeshRenderer>().material.color = foundColor;
        agent.SetDestination(player.transform.position);
        goBackDestination = lastPoint.transform;
        isFollowingPlayer = true;

    }

    public void LostPlayer()
    {
        GetComponent<MeshRenderer>().material.color = lostColor;
        destinationSet = false;

        agent.SetDestination(goBackDestination.position);
        isFollowingPlayer = false;
        Debug.Log("Lost_player");


    }


}
