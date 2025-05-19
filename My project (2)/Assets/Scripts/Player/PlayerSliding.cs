using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerSliding : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] private KeyCode slideKey;
    [SerializeField] private float slideVelocity;

    [SerializeField] Vector3 crouchScale;
    [SerializeField] Vector3 standingScale;

    [Header("Components")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform slideOrientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    private bool isSliding = false;



    private void Update()
    {

        // sliding  
        Vector3 playerScale = standingScale;

        if (Input.GetKey(slideKey))
        {
            if (!isSliding)
            {
                if (rb.linearVelocity != new Vector3(0, 0, 0))
                {
                    slideOrientation.rotation = orientation.rotation;

                    //calcualting move inputs before setting isSliding
                    //so you can slide in one direction per slide
                    horizontalInput = Input.GetAxisRaw("Horizontal");
                    verticalInput = Input.GetAxisRaw("Vertical");

                    isSliding = true;
                }

            }

            // shortening player  
            playerScale = crouchScale;

            // calculate movement direction  
            moveDirection = slideOrientation.forward * verticalInput + orientation.forward * horizontalInput;
            rb.AddForce(moveDirection.normalized * slideVelocity * 10f, ForceMode.Acceleration);


                
            


            
        }

        //resetting player height if not sliding
        else
        {
            isSliding = false;
            playerScale = standingScale;
        }




        //lerping to crouched scale
        transform.localScale = Vector3.Lerp(transform.localScale, playerScale, Time.deltaTime * 10f);
    }
}
