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
    public bool blockedMovement = false;

    public bool IsMoving => isMoving;
    public bool IsSneaking => isSneaking;

    private float paintingClipLength;

    public GameObject thiefRender;
    public GameObject shadowRender;

    private bool shiftTriggered = false;

    public float splatoonCooldown = 5f;
    private float splatoonCooldownInit;
    private float cooldownCount = 0;
    public float cooldownWaitTime = 3f;
    public float slowerCooldown = 3f; // e.g. if set to 3, bar increases 3 times slower as it would decrease

    public RectTransform cooldownBar;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        splatoonCooldownInit = splatoonCooldown;

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

        if (isSplatooning && splatoonCooldown > 0)
        {
            cooldownCount = 0;
            splatoonCooldown -= Time.deltaTime;
        }
        
        if (!isSplatooning && splatoonCooldown < splatoonCooldownInit)
        {
            if (cooldownCount > cooldownWaitTime)
            {
                splatoonCooldown += Time.deltaTime / slowerCooldown;

            }
            else
            {
                cooldownCount += Time.deltaTime;
            }
            
        }

        cooldownBar.localScale = new Vector3(splatoonCooldown / splatoonCooldownInit, 1, 1);

        if (!blockedMovement)
        {
            SplatoonMovement();

            if (Input.GetButton("Jump"))
            {
                if (isSplatooning)
                {
                    currentSpeed = moveSpeed;
                    shadowRender.SetActive(false);
                    thiefRender.SetActive(true);
                    isSplatooning = false;
                    isHiding = false;
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



        UpdateAnimator(inputDir); //updates player animations

    }

    void UpdateAnimator(Vector3 move)
    {
        if (!isPainting && !blockedMovement)
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
            shiftTriggered = true;
        }

        if (shiftTriggered && Input.GetKey(KeyCode.LeftShift) && isMoving && splatoonCooldown > 0)
        {
            shiftTriggered = false;
            isSplatooning = true;
            isHiding = true;
            currentSpeed = splatoonSpeed;
            thiefRender.SetActive(false);
            shadowRender.SetActive(true);
            
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) || !isMoving || splatoonCooldown <= 0)
        {
            ResetSplatooning();
        }
    }

    public void ResetSplatooning()
    {
        if (isSplatooning)
        {
            currentSpeed = moveSpeed;
            shadowRender.SetActive(false);
            thiefRender.SetActive(true);
            isSplatooning = false;
            isHiding = false;
            if (grounded)
            {
                velocityY = Mathf.Sqrt(-2 * gravity * jumpHeight);
                grounded = false;
            }
        }
        
    }

}