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

    public float rotSpeed = 180;
    public Rigidbody rigidbody;
    public float rotationThreshold = 5f;

    private bool needsMoveFlag;
    public bool NeedsMoveFlag => needsMoveFlag;

    public float searchRad = 1f;

    public Color searchRadColor;

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
                lastPoint = points[1];
                transform.LookAt(points[1].transform.position);
            }
        }
        if (route && route.GetPoints().Length > 0)
        {

            GotoNextPoint(false);
        }

        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();

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
        needsMoveFlag = false;
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
        var angle = Vector3.Angle(transform.forward, new Vector3(transform.position.x - lastPoint.transform.position.x, 0, transform.position.z - lastPoint.transform.position.z));
        if (Math.Abs(angle) > rotationThreshold)
        {
            animator.SetFloat("Turn", -1);
            transform.Rotate(Vector3.up, -rotSpeed * Time.deltaTime);
            return true;
        }
        animator.SetFloat("Turn", 0);
        return false;
    }

    public bool TurnToNextPoint()
    {
        var nextPoint = points[(pointIndex) % route.GetPoints().Length];
        var angle = Vector3.Angle(transform.forward, new Vector3(nextPoint.transform.position.x-transform.position.x, 0, nextPoint.transform.position.z-transform.position.z));
        if (Math.Abs(angle) > rotationThreshold)
        {
            animator.SetFloat("Turn", 1*angle > 0?-1:1);
            transform.Rotate(Vector3.up, (1 * angle > 0 ? -1 : 1)*rotSpeed * Time.deltaTime);
            return true;
        }
        animator.SetFloat("Turn", 0);
        return false;
    }

    public void SetNeedsMoveFlag() {
        needsMoveFlag = true;
    }

    public void FindRandomPoint()
    {
        agent.SetDestination(transform.position + Quaternion.Euler(0, UnityEngine.Random.Range(0, 360),0)*Vector3.forward * searchRad);
    }

    private void OnDrawGizmos()
    {
        Handles.color = searchRadColor;
        Handles.DrawWireDisc(transform.position, Vector3.up, searchRad);
    }
}
