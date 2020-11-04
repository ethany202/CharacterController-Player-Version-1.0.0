using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSMouseLook : MonoBehaviour
{
    // Fields
    public Transform player;
    public float sensitivity = 120f;
    float xRotation = 0f;


    // Start is called before the first frame update
    void Start()
    {
        // Locks cursor in center and hides it
        Cursor.lockState = CursorLockMode.Locked;
    }


    // Update is called once per frame
    void Update()
    {
        // Retrieves x-axis and y-axis motion of mouse
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime; // Right/left motion
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime; // forward/backward motion
        xRotation -= mouseY;                                // xRotation represents looking along the x-axis(up/down vision)
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);      // Clamps movement of the mouse by preventing it from going beyond -90 and 90 degrees

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);      // Local rotation moves the local xyz position
        player.Rotate(Vector3.up * mouseX);     // Rotates player
    }
}
