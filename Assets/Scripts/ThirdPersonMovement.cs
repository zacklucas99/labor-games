using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform camera;
    public float moveSpeed = 3f;
    public float gravity = -12f;
    public float jumpHeight = 1f;
    public float turnSmoothTime = 0.2f;
    float turnSmoothVelocity;
    float velocityY;

    private void Start()
    {
        Cursor.visible = false;
    }

    void Update()
    {

        Vector3 inputDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;

        if (Input.GetButton("Jump"))
        {
            if (controller.isGrounded)
            {
                velocityY = Mathf.Sqrt(-2 * gravity * jumpHeight);
            }
        }

        if (inputDir.magnitude >= 0.1f || velocityY != 0)
        {
            float rotationAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + camera.eulerAngles.y;
            float smoothRotationAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, smoothRotationAngle, 0f);

            Vector3 moveDir = velocityY * Vector3.up;
            if (inputDir.magnitude >= 0.1f)
            {
                moveDir += Quaternion.Euler(0f, rotationAngle, 0f) * Vector3.forward * moveSpeed;
            }
            velocityY += gravity * Time.deltaTime;
            controller.Move(moveDir * Time.deltaTime);

            if (controller.isGrounded)
            {
                velocityY = 0;
            }
        }
    }

}
