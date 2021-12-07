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

    private bool isMoving = false;
    public bool IsMoving => isMoving;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        anim = GetComponent<Animator>();
        
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
}