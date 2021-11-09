using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    public float moveSpeed = 3f;
    public float gravity = -12f;
    public float jumpHeight = 1f;
    public float turnSmoothTime = 0.2f;
    float turnSmoothVelocity;
    float velocityY = 0;

    Animator anim;
    float legOffset = 0.2f;
    bool grounded = true;

    public float maxDistance = 7f;
    public LayerMask interactionMask;
    private Transform interactionObj;
    public LayerMask collisionMask;
    public float collisionRadius = 0.4f;

    private Vector3 hitPosition;

    private Shader shaderNoOutline;
    private Shader shaderOutline;

    private bool isMoving = false;
    public bool IsMoving => isMoving;

    public float throwForce;
    public GameObject coinPrefab;

    public bool highlightCoin = false;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        anim = GetComponent<Animator>();
        interactionObj = null;
        shaderNoOutline = Shader.Find("Unlit/Basic");
        shaderOutline = Shader.Find("Unlit/Outline");
}

    void Update()
    {
        Cursor.visible = false;
        Vector3 inputDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;

        if (Input.GetButton("Jump"))
        {
            if (controller.isGrounded)
            {
                velocityY = Mathf.Sqrt(-2 * gravity * jumpHeight);
                grounded = false;
            }
        }

        Vector3 moveDir = velocityY * Vector3.up; //adds y direction movement (jumping/falling)
        if (inputDir.magnitude >= 0.1f)
        {
            float rotationAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + cam.eulerAngles.y; //rotation angle depends on the cameras looking direction
            float smoothRotationAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationAngle, ref turnSmoothVelocity, turnSmoothTime); //smoothens rotation of player
            transform.rotation = Quaternion.Euler(0f, smoothRotationAngle, 0f);
            moveDir += Quaternion.Euler(0f, rotationAngle, 0f) * Vector3.forward * moveSpeed; //adds x, z direction movement

            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        velocityY += gravity * Time.deltaTime;
        controller.Move(moveDir * Time.deltaTime); //applies movement

        if (controller.isGrounded)
        {
            velocityY = 0;
            grounded = true;
        }

        UpdateAnimator(inputDir); //updates player animations

        UpdateObjectInteraction(); //updates iteraction objects

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log("Tossing coin");
            GameObject coin = Instantiate(coinPrefab, transform.position + (cam.transform.forward*0), Quaternion.identity);
            coin.GetComponent<Rigidbody>().AddForce((cam.transform.forward + new Vector3(0,1,0)) * throwForce, ForceMode.Impulse);


            //Debug settings to make coin more visible
            if (highlightCoin)
            {
                coin.GetComponentInChildren<Renderer>().material.shader = shaderOutline;
                coin.GetComponentInChildren<Renderer>().material.SetFloat("_OutlineWidth", 2.0f);
            }
        }

    }

    void UpdateAnimator(Vector3 move)
    {
        anim.SetFloat("Forward", move.magnitude, 0.1f, Time.deltaTime);
        anim.SetBool("OnGround", grounded);

        //determining which leg is in front of the other while jumping
        float runCycle =
            Mathf.Repeat(
                anim.GetCurrentAnimatorStateInfo(0).normalizedTime + legOffset, 1);
        float jumpLeg = (runCycle < 0.5f ? 1 : -1) * move.magnitude;
        if (grounded)
        {
            anim.SetFloat("JumpLeg", jumpLeg);
        }
    }

    void UpdateObjectInteraction()
    {
        Ray ray = cam.GetComponent<Camera>().ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f));
        Debug.DrawLine(ray.origin, ray.GetPoint(maxDistance));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance, collisionMask)) //if raycast hits interation obj
        {
            hitPosition = hit.point;
            Collider[] hitColliders = Physics.OverlapSphere(hitPosition, collisionRadius, interactionMask);

            if (hitColliders.Length >0)
            {
                if (interactionObj != null && interactionObj != hitColliders[0].transform) //if facing new interaction obj
                {
                    interactionObj.GetComponent<Renderer>().material.shader = shaderNoOutline;
                }

                interactionObj = hitColliders[0].transform;
                interactionObj.GetComponent<Renderer>().material.shader = shaderOutline;
            }
            else 
            {
                ResetOutline();
            }

        }
        else
        {
            ResetOutline();
        }

        if (Input.GetKeyDown(KeyCode.E) && interactionObj != null)
        {
            if (interactionObj.GetComponent<PaintingBorder>())
            {
                interactionObj.GetComponent<PaintingBorder>().ChangeCanvasTexture(); //texture change on canvas
            }
            else
            {
                interactionObj.GetComponent<Renderer>().material.SetColor("_Color", Color.red); //color active interaction obj
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(hitPosition, collisionRadius);
    }

    public void ResetOutline()
    {
        if (interactionObj != null)
        {
            interactionObj.GetComponent<Renderer>().material.shader = shaderNoOutline;
            interactionObj = null;
        }
    }
}