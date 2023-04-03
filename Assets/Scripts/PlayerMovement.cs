using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 3.0f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheckPos;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;

    private CharacterController characterController;
    

    float xInput, zInput=0;
    Vector3 moveDirection;
    Vector3 velocity;
    bool isGround;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        HandleInput();
        GroundCheck();
        HandleMovement();
    }

    void HandleInput()
    {
        xInput = Input.GetAxis("Horizontal");
        zInput = Input.GetAxis("Vertical");
        if (Input.GetButtonDown("Jump"))
            Jump();
    }

    void HandleMovement()
    {
        if (characterController != null)
        {
            if(isGround && velocity.y < 0)
            {
                velocity.y = 0;
            }

            moveDirection = transform.forward * zInput + transform.right* xInput;
            characterController.Move(moveDirection * speed * Time.deltaTime);

            velocity.y += gravity * Time.deltaTime;

            characterController.Move(velocity * Time.deltaTime);
        }
    }

    void GroundCheck()
    {
        isGround = Physics.CheckSphere(groundCheckPos.position, groundCheckDistance, groundMask);
    }

    void Jump()
    {
        if(isGround)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }
    }
}
