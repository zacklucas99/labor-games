using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    public float moveSpeed = 3.5f; 
    public float sneakSpeed = 1.6f;
    public float splatoonSpeed = 6.5f;
    private float currentSpeed;
    public float gravity = -12f;
    public float jumpHeight = 1f;
    public float turnSmoothTime = 0.2f;
    float turnSmoothVelocity;
    float velocityY = 0;

    Animator anim;
    float legOffset = 0.2f;
    bool grounded = true;

    private bool isMoving = false;
    private bool isSneaking = false;
    public bool isPainting = false;
    public bool isHiding = false;
    public bool isSplatooning = false;

    public bool IsMoving => isMoving;
    public bool IsSneaking => isSneaking;

    private float paintingClipLength;

    public GameObject thiefRender;
    public GameObject sphereRender;


    public Transform camPos;
    public float camOffset = 0f;
    private float camOffsetInit;
    public float camOffsetTarget = -0.7f;
    private float camOffsetTargetInit;


    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        camOffsetInit = camOffset;
        camOffsetTargetInit = camOffsetTarget;
        camOffsetTarget = camOffset;

        anim = GetComponent<Animator>();
        currentSpeed = moveSpeed;

        RuntimeAnimatorController ac = anim.runtimeAnimatorController;
        for (int i = 0; i < ac.animationClips.Length; i++)
        {
            if (ac.animationClips[i].name == "Picking Up Object")
            {
                paintingClipLength = ac.animationClips[i].length;
            }
        }
    }

    void Update()
    {
        Cursor.visible = false;
        Vector3 inputDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;

        

        if (!isHiding)
        {
            if (Input.GetButton("Jump"))
            {
                if (isSplatooning)
                {
                    ResetSplatooning();
                }
                isPainting = false;
                if (controller.isGrounded)
                {
                    velocityY = Mathf.Sqrt(-2 * gravity * jumpHeight);
                    grounded = false;
                }
            }

            if (!isSplatooning)
            {
               

                if (Input.GetKeyDown(KeyCode.LeftControl))
                {
                    currentSpeed = sneakSpeed;
                    isSneaking = true;
                }

                if (Input.GetKeyUp(KeyCode.LeftControl))
                {
                    currentSpeed = moveSpeed;
                    isSneaking = false;
                }
            }

            SplatoonMovement();

            Vector3 moveDir = velocityY * Vector3.up; //adds y direction movement (jumping/falling)
            if (inputDir.magnitude >= 0.1f)
            {
                isPainting = false;
                float rotationAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + cam.eulerAngles.y; //rotation angle depends on the cameras looking direction
                float smoothRotationAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationAngle, ref turnSmoothVelocity, turnSmoothTime); //smoothens rotation of player
                transform.rotation = Quaternion.Euler(0f, smoothRotationAngle, 0f);
                moveDir += Quaternion.Euler(0f, rotationAngle, 0f) * Vector3.forward * currentSpeed; //adds x, z direction movement

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

        } else
        {
            if (isSplatooning)
            {
                ResetSplatooning();
            }
        }

        UpdateCamMovement();



        UpdateAnimator(inputDir); //updates player animations

    }

    void UpdateAnimator(Vector3 move)
    {
        if (!isPainting && !isHiding)
        {
            anim.SetFloat("Forward", move.magnitude * currentSpeed / moveSpeed, 0.1f, Time.deltaTime);
        } else
        {
            anim.SetFloat("Forward", 0, 0.1f, Time.deltaTime);
        }

        anim.SetBool("OnGround", grounded);
        anim.SetBool("Painting", isPainting);

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

    public float GetPaintingClipLength()
    {
        return paintingClipLength;
    }

    public void SplatoonMovement()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            camOffsetTarget = camOffsetTargetInit;
            currentSpeed = splatoonSpeed;
            thiefRender.SetActive(false);
            sphereRender.SetActive(true);
            isSplatooning = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            ResetSplatooning();
        }
    }

    public void ResetSplatooning()
    {
        camOffsetTarget = camOffsetInit;
        currentSpeed = moveSpeed;
        sphereRender.SetActive(false);
        thiefRender.SetActive(true);
        isSplatooning = false;
    }

    private void UpdateCamMovement()
    {

        float camOffsetDelta = camOffsetTarget - camOffset;
        camOffsetDelta *= Time.deltaTime * 2;
        camOffset += camOffsetDelta;
        camPos.localPosition = new Vector3(0, camOffset, 0);
    }

}