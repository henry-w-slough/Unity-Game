using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Sliding : MonoBehaviour
{



    [Header("Values")]
    [SerializeField] private KeyCode slideKey;
    [SerializeField] private float slideVelocity;

    [SerializeField] private float crouchSpeed;

    [SerializeField] float crouchHeight;
    [SerializeField] float standHeight;

    private float playerHeight;



    [Header("Components")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform slideOrientation;

    [SerializeField] private LayerMask groundLayer;



    private bool isSliding = false;

    private float horizontalInput;
    private float verticalInput;



    private void Update()
    {

        // ground check
        bool grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 1f, groundLayer);


        if (!grounded)
        {
            isSliding = false;
        }

        if (grounded)
        {
            if (Input.GetKey(slideKey))
            {
                Slide();
            }

            if (Input.GetKeyUp(slideKey))
            {
                isSliding = false;
            }
        }



        if (!isSliding) {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1, standHeight, 1), Time.deltaTime * 10f);
            playerHeight = standHeight;
        }




    }





    void Slide()
    {
        //shortening player
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1, crouchHeight, 1), Time.deltaTime * 10f);
        playerHeight = crouchHeight;

        //if the player isn't already sliding, change the slide direction. Allows for one direction sliding
        if (!isSliding)
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");

            slideOrientation.rotation = orientation.rotation;

            isSliding = true;
        }

        //changing move direction based on slide orientation and inputs ^^^
        Vector3 moveDirection = slideOrientation.forward * verticalInput + slideOrientation.right * horizontalInput;

        //adding force in move direction
        rb.AddForce(moveDirection * slideVelocity * 10f, ForceMode.Force);

    }
}
