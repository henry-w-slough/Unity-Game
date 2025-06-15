using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


//This class is used as THE reference for all other movement classes. It holds variables to see if the player is crouching, sliding, etc.
//so each class with that variable doesn't have to be changed individually every time the player does something.


public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]

    private Rigidbody rb;
    private Transform orientation;



    [Header("Player")]

    [SerializeField] private float moveSpeed;
    [SerializeField] private float groundDrag;

    public bool isSliding;





    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        orientation = GameObject.Find("Orientation").transform;

        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }





    private void Update()
    {
        bool grounded = Physics.Raycast(transform.position, Vector3.down, transform.localScale.y * 0.5f + 1f);

        if (grounded)
        {
            Move();
        }
    }




    private void Move()
    {
        //GROUND MOVEMENT //
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        //getting move direction based on facing direction and input direction
        Vector3 moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //pushing rb based on move direction normalized times movement speed, meaning player goes movement speed.
        rb.AddForce(moveDirection.normalized * moveSpeed, ForceMode.Force);

        //controlling the speed of the player so he doesn't sonic
        SpeedControl();

        rb.linearDamping = groundDrag;
    }






    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        // limit velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }
}