using UnityEngine;

public class Jumping : MonoBehaviour
{

    [Header("Components")]
    Rigidbody rb;


    [Header("Values")]
    [SerializeField] float jumpForce;



    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        bool grounded = Physics.Raycast(transform.position, Vector3.down, transform.localScale.y * 0.5f + 1f);

        if (grounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }
    } 


    void Jump()
    {
        // reset y velocity
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
}
