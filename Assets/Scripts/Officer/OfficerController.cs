using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.ThirdPerson;

public class OfficerController : MonoBehaviour, SoundReceiver
{
    // Start is called before the first frame update

    private NavMeshAgent agent;
    private RoutePoint[] points;
    private RoutePoint lastPoint;

    public int pointIndex;
    public RouteVisualization route;
    public RouteVisualization alarmRoute;
    private bool destinationSet = false;

    public Color lostColor;
    public Color foundColor;

    private Transform goBackDestination;


    private CustomThirdPerson character;

    private bool isFollowingPlayer;

    public bool FollowingPlayer => isFollowingPlayer;

    public float playerFollowingSpeed = 1f;
    public float walkingSpeed = 0.5f;
    public float alarmedWalkingSpeed = 0.75f;

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
    public bool GotNewSound { get; set; } // Notifying that suddenly a new sound was applied, while already following

    public float hearableVolume = 0; // threshold for what the character can hear

    public bool IsPickingUp { get; set; } = false;
    public bool IsTurningSoundOff { get; set; } = false;

    public bool OverTurning { get; set; }

    public GameObject rightHand;

    private List<SoundObject> soundObjects = new List<SoundObject>();
    private HashSet<SoundObject> soundObjectsMemory = new HashSet<SoundObject>();//Storing the sound objects that were already handled

    public bool GotEnvironmentNotification { get; private set; }
    private NotifierObject notifierObject;
    HashSet<NotifierObject> notificatedObjects = new HashSet<NotifierObject>();

    public GameObject exclamationMark;
    public GameObject questionMark;

    public PlayerAlarmState PlayerAlarmState { get; set; } = PlayerAlarmState.IDLE;

    public float MoveSpeed=>PlayerAlarmState == PlayerAlarmState.IDLE ? walkingSpeed : alarmedWalkingSpeed;

    public Vector3 moveToPosition;

    public bool RestartOnCollision = true;
    

    public bool ArrivedAtWayPoint{
        get {
            Vector2 distVector = new Vector2(agent.transform.position.x - agent.destination.x, agent.transform.position.z - agent.destination.z);
            return distVector.magnitude < agent.stoppingDistance;
        }
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (route)
        {
            points = route.GetPoints();
        }
        // Setting character to first route point
        if(setToStartPoint &&points.Length > 0)
        {
            transform.position = points[0].transform.position;
            if (points.Length > 1)
            {
                lastPoint = points[1];
                transform.LookAt(points[1].transform.position);
            }
        }

        //Move to point
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
        //Function for moving the player to a specific route point 
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
        // Setting movement to new route point
        if (!agent.pathPending && agent.remainingDistance < agent.stoppingDistance && route != null && route.GetPoints().Length > 1)
        {
            GotoNextPoint(false);
        };
    }

    public void ResetWayPointTarget()
    {
        // Resetting the next point to move to
        agent.SetDestination(lastPoint.transform.position);
    }

    // Update is called once per frame
    public bool Move()
    {
        /// Function for animating the player and checking if the player can still move
        needsMoveFlag = false;
        if (agent.remainingDistance > agent.stoppingDistance) {
            // player too far away fro mtarget
            character.Move(agent.desiredVelocity.normalized * (MoveSpeed), false, false);
            return true;
        }
        else
        {
            // player reached  target
            character.Move(Vector3.zero, false, false);
            return false;
        }
    }

    public void Reset()
    {
        // Function for resetting the player movement
        character.Move(Vector3.zero, false, false);
        agent.SetDestination(transform.position);
    }

    public void FollowPlayer()
    {
        // Function for moving the officer towards a destination, where a player was noticed
        agent.SetDestination(playerDestination);
        character.Move(agent.desiredVelocity.normalized * (playerFollowingSpeed), false, false);
    }

    public bool RunToAlarm()
    {
        // Function for moving the officer towards a destination, where a player was noticed by a camera
        agent.SetDestination(notifyPosition);
        character.Move(agent.desiredVelocity.normalized , false, false);
        bool arrived = ArrivedAtAlarm();
        return arrived;
    }

    public void FollowSound()
    {
        // Funciton for moving the player towards sound source
        // Todo: rewrite more beautifully
        agent.SetDestination(soundDestination);
        character.Move(agent.desiredVelocity.normalized * MoveSpeed, false, false);
    }

    public bool FollowNotification()
    {
        // Function for moving the officer to a position, where the player was last seen 
        if(notifierObject == null)
        {
            return false;
        }
        agent.SetDestination(notifierObject.moveToPoint == null ? notifierObject.transform.position : notifierObject.moveToPoint.transform.position);
        character.Move(agent.desiredVelocity.normalized * MoveSpeed, false, false);
        Vector2 distVector = new Vector2(agent.transform.position.x - agent.destination.x, agent.transform.position.z - agent.destination.z);
        // Returning boolean for determining, whether player has arrived
        return !ArrivedAtWayPoint && (distVector.magnitude > notifierObject.interactRadius);
    }


    public void FoundPlayer(GameObject player) {
        // Callback for when player discovered. Change color of field of view
        meshRenderer.material.color = foundColor;
        playerDestination = player.transform.position;
        goBackDestination = lastPoint.transform;
        moveToPosition = playerDestination;
        isFollowingPlayer = true;

    }

    public void LostPlayer()
    {
        // Callvack for when player lost. Change color of field of view
        meshRenderer.material.color = lostColor;
        destinationSet = false;

        isFollowingPlayer = false;
    }
    public bool RunToLastActionPoint()
    {
        // Fucntion for running towards the last position, where the player was seen
        agent.SetDestination(playerDestination);
        return Move();
    }

    #region Turning
    public bool TurnToLastPoint()
    {
        // Function for turning towards the last route point
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
        // Functio nfor turning towards the next route point
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
        // Function for turning towards player, if player got too close
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

    public bool TurnToSound()
    {
        // Function for turning towards sound
        var angle = Vector3.SignedAngle(new Vector3(soundDestination.x - transform.position.x, 0, soundDestination.z - transform.position.z),
            transform.forward, Vector3.up);

        if (!isTurning)
        {
            if (Math.Abs(angle) < rotationThreshold)
            {
                return false;
            }
            isTurning = true;
            turningFinished = false;
            if (Math.Abs(angle) < 90 && SoundObj.GetComponent<ThirdPersonMovement>() != null)
            {
                // Make officer turning faster for smaller angles to make it easier to discover player
                OverTurning = true;

                /*if (angle > 0)
                {
                    angle += minRotationCloseBy;
                }
                else
                {
                    angle -= minRotationCloseBy;
                }
                currentRotationSpeed = angle / 180f;
                character.SetRotation(-currentRotationSpeed);
                */
            }
            currentRotationSpeed = angle / 180f;
            character.SetRotation(-currentRotationSpeed);

        }

        if (turningFinished && Math.Abs(angle) < rotationThreshold)
        {
            character.SetRotation(0);
            currentRotationSpeed = 0;
            isTurning = false;
            OverTurning = false;
            return false;
        }
        character.SetRotation(-currentRotationSpeed);

        return true;
    }


    public bool TurnToNotifier()
    {
        // Function for turning towards the notification
        if(notifierObject == null)
        {
            return true;
        }
        var angle = Vector3.SignedAngle(new Vector3(notifierObject.transform.position.x - transform.position.x, 0, notifierObject.transform.position.z - transform.position.z),
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
            OverTurning = false;
            return false;
        }
        character.SetRotation(-currentRotationSpeed);

        return true;
    }

    public void ResetTurn()
    {
        // Functio nfor resetting the turning. For example, when turning failed and node has to be reset
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

    public bool FacingSoundObj()
    {
        // Checking, if officer is looking into the direction of the sound object
        var angle = Vector3.SignedAngle(new Vector3(soundDestination.x - transform.position.x, 0, soundDestination.z - transform.position.z),
                    transform.forward, Vector3.up);

        return Math.Abs(angle) < rotationThreshold;
    }

    public void SetNeedsMoveFlag() {
        // Setting that the officer should move
        needsMoveFlag = true;
    }

    public void FindRandomPoint()
    {
        // Function for getting a new point close to the officer
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
        // Function indicating that turning finished
        turningFinished = true;
    }


    public void ReceiveSound(SoundObject obj, float receiveVolume)
    {
        // Functionfor  receiving a sound
        if (IsPickingUp || IsTurningSoundOff && obj.gameObject != this.gameObject)
        {
            // As the dog can make a sound, we have to create a special case for the dog not detecting its own sound
            return;
        }
        if (soundObjectsMemory.Contains(obj))
        {
            return;
        }
        if (receiveVolume > hearableVolume)
        {
            if(soundObjects.Contains(obj)) {
                if (obj != SoundObj)
                {
                    return;
                }

            }
            if(SoundObj != null && obj != SoundObj)
            {
                GotNewSound = true;
            }
            isFollowingSound = true;
            soundDestination = obj.transform.position;
            goBackDestination = lastPoint.transform;
            soundObjectToHandle = obj;
            soundTurnedOff = false;
            soundObjects.Add(obj);
        }
    }

    public bool NearSound()
    {
        // Checking if officer close to sound
        Vector3 soundDest = new Vector3(soundDestination.x, 0, soundDestination.z);
        Vector3 pos = new Vector3(transform.position.x, 0, transform.position.z);
        return soundObjectToHandle != null && 
            (soundDest - pos).magnitude <= agent.stoppingDistance;
    }

    public bool CanTurnSoundOff()
    {
        // Checking whether officer can turn the sound off
        return SoundObj && SoundObj.canTurnSoundOff;
    }

    public bool CanPickUpObj()
    {
        // Function for checking whether officer can pick up object
        return (SoundObj && SoundObj.canPickUp) || (notifierObject && notifierObject.canPickUp);
    }

    public bool CanCleanUpObj()
    {
        // Function for checking whether officer can clean up object
        return (SoundObj && SoundObj.canCleanUp) || (notifierObject && notifierObject.canCleanUp);
    }

    public void FinishPickingUp()
    {
        // Function for finishing the picking up behavior
        animator.SetBool("PickUp", false);
        animator.SetBool("CleanUp", false);

        if(notifierObject != null && IsPickingUp)
        {
            if (notifierObject.destroyObject != null)
            {
                Destroy(notifierObject.destroyObject);
            }
            else if (notifierObject.interactObject == null)
            {
                Destroy(notifierObject.gameObject);
            }
            else
            {
                Destroy(notifierObject.interactObject);
            }
            ResetEnvironmentNotification();
        }

        else if (SoundObj != null && IsPickingUp)
        {
            Destroy(SoundObj.gameObject);
            ResetSoundToHandle();
        }

        IsPickingUp = false;
    }



    public void PickUpObj()
    {
        // Starting the picking up behavior
        IsPickingUp = true;
        animator.SetBool("PickUp", true);
    }

    public void CleanUpObj()
    {
        // Starting the picking up behavior
        Debug.Log("Clean Up");
        IsPickingUp = true;
        animator.SetBool("CleanUp", true);
    }

    public void SetCoinToHand()
    {
        // Setting the coint to a hand, when the officer is picking up an object
        // Called by an event
        Debug.Log("SetCoin To Hand");
        if (notifierObject != null)
        {
            var interact_obj = notifierObject.interactObject == null ? notifierObject.gameObject : notifierObject.interactObject;
            interact_obj.GetComponent<PickableObject>().ResetCollider();
            interact_obj.GetComponent<PickableObject>().parentObject = rightHand;
        }

        else if (SoundObj != null)
        {
            SoundObj.GetComponent<PickableObject>().ResetCollider();//Disable collider to get rid of bugs
            SoundObj.GetComponent<PickableObject>().parentObject = rightHand;
        }
    }

    public bool TurnSoundOff()
    {
        // Function for animating turning the sound off
        if (!soundTurnedOff)
        {
            IsTurningSoundOff = true;
            animator.SetBool("TurnOff", true);
        }
        return soundTurnedOff;
    }

    public void FinishTurnSoundOff()
    {
        // Function for finishing turning the sound off
        soundObjectToHandle.SetTurnedOn(false);
        soundObjectToHandle.isLocked = false;
        ResetSoundToHandle();
    }

    public void ResetSoundToHandle()
    {
        // TODO: add comment
        IsTurningSoundOff = false;
        isFollowingSound = false;
        soundDestination = Vector3.zero;
        soundObjectToHandle = null;
        soundObjects.Clear();
    }

    public void ResetSoundInteractionAnimation()
    {
        // Resetting interacting with objects and turning off objects
        animator.SetBool("TurnOff", false);
        animator.SetBool("PickUp", false);
        if (SoundObj != null && IsPickingUp)
        {
            Destroy(SoundObj.gameObject);
        }
        IsPickingUp = false;
    }

    public void FinishTurnOffAnimation()
    {
        // Finishing the turning off. Called by an event
        animator.SetBool("TurnOff", false);
        soundTurnedOff = true;
    }

    public void GotPlayerCloseBy(GameObject player)
    {
        // Function for determining whether a player is close by
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

    public bool WallBetweenSound()
    {
        // Determining, whether wall between sound and officer
        return (Physics.Raycast(new Ray(transform.position, soundDestination - transform.position), 
            (transform.position - soundDestination).magnitude, environmentLayer));
    }

    public void LostPlayerCloseBy()
    {
        // Function for resetting the officer following the player
        playerCloseBy = false;
    }

    public void ReceivedAlarm(Vector3 playerPosition)
    {
        // Function for officer receiving an alarm
        notifyPosition = playerPosition;
        Notified = true;
    }

    public bool ArrivedAtAlarm()
    {
        // Function for checking whether officer arrived at alarm point
        Vector2 distVector = new Vector2(agent.transform.position.x - agent.destination.x, agent.transform.position.z - agent.destination.z);
        return isMovingToAlarm && distVector.magnitude >= agent.stoppingDistance;
    }

    public void ResetNotification()
    {
        // Function for resetting officer getting notification
        Notified = false;
        isMovingToAlarm = false;
        notifyPosition = Vector3.zero;
    }

    public void SetNotification()
    {
        isMovingToAlarm = true;
    }


    public void ReceiveNotifcation(NotifierObject notifierObject)
    {
        // Function for player getting notification by event, by a notification object
        if (notifierObject &&!this.notifierObject && !notificatedObjects.Contains(notifierObject) && SoundObj == null) {
            GotEnvironmentNotification = true;
            notificatedObjects.Add(notifierObject);
            this.notifierObject = notifierObject;
        }
    }

    public void ResetEnvironmentNotification()
    {
        // Resetting officer getting notification
        GotEnvironmentNotification = false;
        notifierObject = null;
    }

    public void Alarm()
    {
        // Function for handling when player switches into alarm mode (e.g. got notified image was drawn onto)
        PlayerAlarmState = PlayerAlarmState.NOTIFIED;
        route = alarmRoute;
        
        points = route.GetPoints();

        pointIndex = 0;
        //Move to point
        if (route && route.GetPoints().Length > 0)
        {

            GotoNextPoint(false);
        }
    }

    public void AddToMemory(SoundObject obj)
    {
        if (obj != null) {
            soundObjectsMemory.Add(obj);
        }
    }

    private void OnDrawGizmos()
    {
        if (!drawGizmos)
        {
            return;
        }
        Handles.color = searchRadColor;
        Handles.DrawWireDisc(transform.position, Vector3.up, searchRad);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(moveToPosition, 0.5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player" && RestartOnCollision)
        {
            Debug.Log("arrivedPlayer");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        }
    }

}

public enum PlayerAlarmState
{
    IDLE, NOTIFIED
}
