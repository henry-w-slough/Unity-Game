using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerSliding : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] private KeyCode slideKey;
    [SerializeField] private float slideVelocity;

    [SerializeField] private float crouchSpeed;

    [SerializeField] Vector3 crouchScale;
    [SerializeField] Vector3 standingScale;

    [Header("Components")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform slideOrientation;

    [SerializeField] private LayerMask groundLayer;

    Vector3 moveDirection;

    private bool isSliding = false;
    private bool isCrouching = false;


    private float horizontalInput;
    private float verticalInput;




    private void Update()
    {


        isSliding = false;

        // ground check
        bool grounded = Physics.Raycast(transform.position, Vector3.down, transform.localScale.y * 0.5f + 0.3f, groundLayer);




        if (Input.GetKeyDown(slideKey))
        {
            
        }



        if (Input.GetKey(slideKey))
        {
            //sliding if there is momentum in a certain direction
            if (rb.linearVelocity != new Vector3(0, 0, 0))
            {
                isSliding = true;
            }

        }





        if (!isSliding) {
            transform.localScale = Vector3.Lerp(transform.localScale, standingScale, Time.deltaTime * 10f);
        }



        if (isSliding)
        {
            Debug.Log("sldiing");
            Vector3 moveDirection = moveDirection = slideOrientation.forward * verticalInput + slideOrientation.right * horizontalInput;

            Slide();
            //adding force in move direction
            rb.AddForce(moveDirection * slideVelocity * 10f, ForceMode.Force);
        }

    }



    void Crouch()
    {
        //shortening player
        transform.localScale = Vector3.Lerp(transform.localScale, crouchScale, Time.deltaTime * 10f);
    }


    void Slide()
    {
        if (!isSliding)
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");

            slideOrientation.rotation = orientation.rotation;
        }

        //shortening player
        transform.localScale = Vector3.Lerp(transform.localScale, crouchScale, Time.deltaTime * 10f);


        
    }
}
