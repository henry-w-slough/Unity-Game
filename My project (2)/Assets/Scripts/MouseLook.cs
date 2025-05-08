using UnityEngine;

public class MouseLook : MonoBehaviour
{

    [Header("Components")]

    public Transform orientation;
    public Transform playerCamPosition;



    [Header("Mouse Looking")]

    public float sensitivity;

    float xRotation;
    float yRotation;

    public float fov = 90f;








    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = playerCamPosition.position; 
        
        //getting mouse input in a float value
        float mouseX = Input.GetAxis("Mouse X") * sensitivity; 
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity; 
                                                               
        yRotation += mouseX;
        xRotation -= mouseY;

        //preventing flipping too far
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //changing the rotation of the camera
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
