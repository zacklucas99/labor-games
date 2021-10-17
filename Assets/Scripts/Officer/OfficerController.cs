using System;
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

    public bool FollowingPlayer =>isFollowingPlayer;

    public float playerFollowingSpeed = 1f;
    public float walkingSpeed = 0.5f;

    public bool setToStartPoint;

    public Vector3 playerDestination;
    public MeshRenderer meshRenderer;

    private Animator animator;

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

        animator = GetComponent<Animator>();

        character = GetComponent<ThirdPersonCharacter>();

    }

    void GotoNextPoint( bool wait)
    {
        lastPoint = points[pointIndex % points.Length];

        agent.SetDestination(points[(pointIndex++) % points.Length].transform.position);
        if(pointIndex >= points.Length)
        {
            pointIndex = 0;
        }
    }

    public bool StopMovement()
    {
        character.Move(Vector3.zero, false, false);
        agent.SetDestination(transform.position);
        return !isFollowingPlayer;
    }

    public void FindNewPoint()
    {

        if (!agent.pathPending && agent.remainingDistance < agent.stoppingDistance && route != null && route.GetPoints().Length > 1)
        {
            GotoNextPoint(false);
        };
    }

    public void ResetWayPointTarget()
    {
        agent.SetDestination(lastPoint.transform.position);
    }


// Update is called once per frame
    public bool Move()
    {
        if (agent.remainingDistance > agent.stoppingDistance) {
            character.Move(agent.desiredVelocity.normalized * (walkingSpeed), false, false);
            return true;
        }
        else
        {
            character.Move(Vector3.zero, false, false);
            return false;
        }
    }

    public void Reset()
    {
        character.Move(Vector3.zero, false, false);
        agent.SetDestination(transform.position);
    }

    public void FollowPlayer()
    {
        agent.SetDestination(playerDestination);
        character.Move(agent.desiredVelocity.normalized * (playerFollowingSpeed), false, false);
    }


    public void FoundPlayer(GameObject player) {
        meshRenderer.material.color = foundColor;
        playerDestination = player.transform.position;
        goBackDestination = lastPoint.transform;
        isFollowingPlayer = true;

    }

    public void LostPlayer()
    {
        meshRenderer.material.color = lostColor;
        destinationSet = false;

        isFollowingPlayer = false;
        Debug.Log("Lost_player");


    }
    public bool RunToLastActionPoint()
    {
        agent.SetDestination(playerDestination);
        return Move();
    }

    public bool TurnToLastPoint()
    {
        Debug.Log(animator.applyRootMotion);
        animator.applyRootMotion = true; 
        animator.SetFloat("Turn", -1);
        return true;




    }
}
