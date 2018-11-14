﻿using UnityEngine;

[RequireComponent(typeof(CharacterController))] //safety first 

public class PlayerMovementScript : MonoBehaviour
{
    public float walkSpeed = 4.0F;
    public float jumpSpeed = 8.0F;
    public float runSpeed = 8.0F; //it can be use as a sprint fuction in the near future
    public float gravity = 20.0F;
    public Transform cameraTransform { set; get; } //  Take the properties of the Camera

    private Vector3 moveDirection;
    private Vector3 targetDirection;

    private CharacterController cController;
    private InputManager input;
    private bool isGrounded;
    private float distToGround;

    void Awake()
    {
        input = GameObject.FindGameObjectWithTag("Manager").GetComponent<InputManager>(); //Accessing the InputManager component

        cController = GetComponent<CharacterController>(); // //Accessing the CharacterController component
        cameraTransform = Camera.main.transform; // We want the position and the rotation 
    }
    private Vector3 dir;
    void Update()
    {
        
        if (cController.isGrounded)
        {
            Move();

            Sprint();

            Jump();
        }
        else
        {
            //AirMove(); //Implement me?
        }


        moveDirection.y -= gravity * Time.deltaTime; // Implementation of gravity 
        cController.Move(moveDirection * Time.deltaTime); // Making that little boy move
    }

    public void Move()   // Receiving forces in a Vector2
    {
        moveDirection = new Vector3(-input.Vertical, 0, input.Horizontal);

        dir = cameraTransform.TransformDirection(moveDirection);

        moveDirection = transform.TransformDirection(dir.x, 0, dir.z); /* So here we are, this code line here is an attempt to move player where camera is looking BUT at first when u looked up it was trying to move the player in the Y axis */
    }

    public void AirMove()
    {
        //Implement me?
    }

    public void Jump()
    {
        if (input.Jump)
        {
            moveDirection.y = jumpSpeed; // If space is pressed then the player jumps accordingly( maintains his velocity)
        }
    }

    public void Sprint()
    {
        if (input.IsRunning)
        {
            moveDirection *= runSpeed;
        }
        else
        {
            moveDirection *= walkSpeed;
        }
    }
}