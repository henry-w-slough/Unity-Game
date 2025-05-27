using UnityEngine;

public class WallRunning : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] LayerMask wallLayer;
    public LayerMask groundLayer;




    [Header("Player")]
    [SerializeField] Rigidbody rb;
    [SerializeField] Transform orientation;

    [SerializeField] float maxWallDistance;
    private float playerHeight;



    private RaycastHit wallRayLeft;
    private RaycastHit wallRayRight;

    private bool canWallJump;



    void Update()
    {


        bool grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 1f, groundLayer);

        if (!grounded)
        {
            canWallJump = true;
        }
        if (grounded)
        {
            canWallJump = false;
        }




        if (canWallJump)
        {
            //detecting wall collision on right
            if (Physics.Raycast(transform.position, orientation.right, out wallRayLeft, maxWallDistance, wallLayer))
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    rb.AddForce(0f, 50f, 10f);
                }
            }

            //detecting wall collision on left
            if (Physics.Raycast(transform.position, -orientation.right, out wallRayLeft, maxWallDistance, wallLayer))
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    rb.AddForce(0f, 50f, -10f);
                }
            }
        }






    }

}
