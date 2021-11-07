using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class OfficerController : MonoBehaviour, SoundReceiver
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


    private CustomThirdPerson character;

    private bool isFollowingPlayer;

    public bool FollowingPlayer =>isFollowingPlayer;

    public float playerFollowingSpeed = 1f;
    public float walkingSpeed = 0.5f;

    public bool setToStartPoint;

    public Vector3 playerDestination;
    public Vector3 soundDestination;
    public MeshRenderer meshRenderer;

    private Animator animator;

    public float rotSpeed = 180;
    public Rigidbody rigidbody;
    public float rotationThreshold = 5f;

    private bool needsMoveFlag;
    public bool NeedsMoveFlag => needsMoveFlag;

    public float searchRad = 1f;

    public Color searchRadColor;
    public bool drawGizmos = true;

    private bool isTurning = false;
    private bool turningFinished = false;
    private float currentRotationSpeed;

    public int maxSearchSteps = 5;

    public LayerMask environmentLayer;
    public bool isFollowingSound = false;

    private SoundObject soundObjectToHandle;
    private bool soundTurnedOff = true;

    public bool SoundTurnedOff => soundTurnedOff;
    public SoundObject SoundObj => soundObjectToHandle;

    private bool playerCloseBy;
    public bool PlayerCloseBy => playerCloseBy;
    private Vector3 approximationPoint;

    private bool isTurningApprox = false;
    public LayerMask closeByMask;
    public float minRotationCloseBy = 90f;

    private Vector3 notifyPosition;
    private bool isMovingToAlarm;

    public bool Notified { get; private set; }

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

        character = GetComponent<CustomThirdPerson>();

    }

    void GotoNextPoint( bool wait)
    {
        lastPoint = points[pointIndex % points.Length];

        agent.SetDestination(points[(pointIndex++) % points.Length].transform.position);
    }

    public void StopMovement()
    {
        // Stopping the current movement and animation
        character.Move(Vector3.zero, false, false);
        agent.SetDestination(transform.position);
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

    public bool RunToAlarm()
    {
        agent.SetDestination(notifyPosition);
        character.Move(agent.desiredVelocity.normalized , false, false);
        bool arrived = ArrivedAtAlarm();
        isMovingToAlarm = true;
        return arrived;
    }

    public void FollowSound()
    {
        // Todo: rewrite more beautifully
        agent.SetDestination(soundDestination);
        character.Move(agent.desiredVelocity.normalized * walkingSpeed, false, false);
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
    }
    public bool RunToLastActionPoint()
    {
        agent.SetDestination(playerDestination);
        return Move();
    }

    #region Turning
    public bool TurnToLastPoint()
    {
        var point = points[(pointIndex - 2 >= 0 ? pointIndex - 2 : 0) % points.Length];
        var angle = Vector3.Angle(transform.forward, new Vector3(point.transform.position.x- transform.position.x  , 0,  point.transform.position.z - transform.position.z));
        if (!isTurning)
        {
            if(Math.Abs(angle) < rotationThreshold) { 
                return false;
            }
            isTurning = true;
            turningFinished = false;
            currentRotationSpeed = angle / 180f;
            character.SetRotation(-currentRotationSpeed);
        }
        if (turningFinished && Math.Abs(angle) < rotationThreshold)
        {
            character.SetRotation(0);
            currentRotationSpeed = 0;
            isTurning = false;
            return false;
        }
        character.SetRotation(-currentRotationSpeed);

        return true;
    }

    public bool TurnToNextPoint()
    {
       var nextPoint = points[(pointIndex) % route.GetPoints().Length];
        var angle = Vector3.SignedAngle(new Vector3(nextPoint.transform.position.x-transform.position.x, 0, nextPoint.transform.position.z-transform.position.z), 
            transform.forward, Vector3.up);

        if (!isTurning)
        {
            if (Math.Abs(angle) < rotationThreshold)
            {
                return false;
            }
            isTurning = true;
            turningFinished = false;
            currentRotationSpeed = angle / 180f;
            character.SetRotation(-currentRotationSpeed);

        }
        if (turningFinished && Math.Abs(angle) < rotationThreshold)
        {
            character.SetRotation(0);
            currentRotationSpeed = 0;
            isTurning = false;
            return false;
        }
        character.SetRotation(-currentRotationSpeed);

        return true;
    }


    public bool TurnToApproxPoint()
    {
        var angle = Vector3.SignedAngle(new Vector3(approximationPoint.x - transform.position.x, 0, approximationPoint.z - transform.position.z),
            transform.forward, Vector3.up);

        if (!isTurning)
        {
            isTurningApprox = true;
            if (Math.Abs(angle) < rotationThreshold)
            {
                return false;
            }
            isTurning = true;
            turningFinished = false;
            if(Math.Abs(angle) < 90)
            {
                // Make officer turning faster for smaller angles to make it easier to discover player
                if(angle > 0)
                {
                    angle += minRotationCloseBy;
                }
                else { 
                    angle -= minRotationCloseBy;
                }
            }
            currentRotationSpeed = angle / 180f;
            character.SetRotation(-currentRotationSpeed);

        }
        if (turningFinished && Math.Abs(angle) < rotationThreshold)
        {
            character.SetRotation(0);
            currentRotationSpeed = 0;
            isTurning = false;
            approximationPoint = Vector3.zero;
            playerCloseBy = false;
            isTurningApprox = false;
            return false;
        }
        character.SetRotation(-currentRotationSpeed);

        return true;
    }

    public void ResetTurn()
    {
        // Temporary fix, if turn was aborted, reset turning 
        if (!turningFinished)
        {
            character.SetRotation(0);
            currentRotationSpeed = 0;
            isTurning = false;
            approximationPoint = Vector3.zero;
            playerCloseBy = false;
            turningFinished = true;
        }
    }
    #endregion

    public void SetNeedsMoveFlag() {
        needsMoveFlag = true;
    }

    public void FindRandomPoint()
    {
        RaycastHit hit;
        float maxDist = 0;
        Vector3 dest = transform.position;
        for (int i = 0; i < maxSearchSteps; i++) {
            if (Physics.Raycast(transform.position, Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0) * Vector3.forward, out hit, searchRad)) {
                if (hit.distance > maxDist)
                {
                    dest = hit.point;
                }
                if(hit.distance > searchRad)
                {
                    break;
                }
            }
            else
            {
                dest = transform.position + Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0) * Vector3.forward;
                break;
            }
        }
        agent.SetDestination(dest);
    }

    private void RotationEnded()
    {
        turningFinished = true;
    }


    public void ReceiveSound(SoundObject obj)
    {
        isFollowingSound = true;
        soundDestination = obj.transform.position;
        goBackDestination = lastPoint.transform;
        soundObjectToHandle = obj;
        soundTurnedOff = false;
    }

    public bool NearSound()
    {
        return soundObjectToHandle != null && 
            (soundObjectToHandle.transform.position - transform.position).magnitude <= agent.stoppingDistance;
    }

    public bool TurnSoundOff()
    {
        if (!soundTurnedOff)
        {
            animator.SetBool("TurnOff", true);
        }
        return soundTurnedOff;
    }

    public void FinishTurnSoundOff()
    {
        soundObjectToHandle.SetTurnedOn(false);
        isFollowingSound = false;
        soundDestination = Vector3.zero;
    }

    public void FinishTurnOffAnimation()
    {
        animator.SetBool("TurnOff", false);
        soundTurnedOff = true;
    }

    public void GotPlayerCloseBy(GameObject player)
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, (player.transform.position- transform.position).normalized,out hit, float.MaxValue, closeByMask);
        if (hit.collider.gameObject.tag == "Player")
        {
            playerCloseBy = true;
            if (!isTurning)
            {
                approximationPoint = player.transform.position;
            }
        }
    }

    public void LostPlayerCloseBy()
    {
        playerCloseBy = false;
    }

    public void ReceivedAlarm(Vector3 playerPosition)
    {
        Debug.Log("Received Alarm");
        notifyPosition = playerPosition;
        Notified = true;
    }

    public bool ArrivedAtAlarm()
    {
        return isMovingToAlarm && agent.remainingDistance < agent.stoppingDistance;
    }

    public void ResetNotification()
    {
        Notified = false;
        isMovingToAlarm = false;
        notifyPosition = Vector3.zero;
    }


    private void OnDrawGizmos()
    {
        if (!drawGizmos)
        {
            return;
        }
        Handles.color = searchRadColor;
        Handles.DrawWireDisc(transform.position, Vector3.up, searchRad);
    }

}
