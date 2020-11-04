using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Fields:
    public CharacterController player;
    public Transform cam;
    public Animator animator;

    public float currentHealth = 100f;
    public float baseSpeed = 11.5f;
    public float sprintSpeed = 20f;
    public float speed = 10f;
    public float gravity = -0.5f;
    public float currentJumpHeight = 25f;
    public float regJumpHeight = 25f;
    public float sprintJumpHeight = 35f;
    public float flexibility = 5f;

    public LayerMask groundMask;
    bool isGrounded;
    public Transform groundCheck;
    public float groundDistance = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        // Calls CheckSphere() method from Physics class to see if the current player is on the ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // If player is on the ground, reset gravity value(this way, the next time the player falls, it gradually does so)
        if (isGrounded && gravity < 0)
        {
            gravity = -0.9f;
        }

        // Retrieves the value of the player's right/left direction and the value of the player's forward/backward direction
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        // Creates Vector3 object based on the player's direction
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        // Checks if the Vector3 object "direction" is greater than 0.1; if so, move the player
        if (direction.magnitude >= 0.1f)
        {
            // Obtains the angle which the player is moving to relative to the origin; cam.eulerAngles.y retrieves y-value of the camera's angle. This way, moving mouse right/left also moves player in that direction   
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            animator.SetBool("isWalking", true);
            if (vertical < 0f)
            {
                animator.SetBool("isRunningBackwards", true);
                animator.SetBool("isWalking", false);
            }

            // If player presses left control, sprint
            if (Input.GetKey(KeyCode.LeftControl))
            {
                animator.SetBool("isSprinting", true);
                while (speed < sprintSpeed)
                {
                    speed += 1f;
                }
                currentJumpHeight = sprintJumpHeight;
            }
            if (Input.GetKeyUp(KeyCode.LeftControl))    // Upon releasing left control, stop sprinting
            {
                animator.SetBool("isSprinting", false);
                while (speed > baseSpeed)
                {
                    speed -= 1f;
                }
                currentJumpHeight = regJumpHeight;
            }
            player.Move(moveDir.normalized * speed * Time.deltaTime);   // Moves the player in a certain direction
        }
        else
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isSprinting", false);
            animator.SetBool("isRunningBackwards", false);
        }
        CallJump(currentJumpHeight);
        CallGravity();
    }

    // Checks if the character is currently on the ground
    public void CheckGrounded()
    {
        this.isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
    }

    // Adds gravity to character
    public void CallGravity()
    {
        CheckGrounded();
        if (!isGrounded)
        {
            Vector3 fall = new Vector3(0f, gravity, 0f);
            player.Move(fall * Time.deltaTime);
            gravity -= 0.9f;
        }
    }

    // Calls the CheckJump() method if player presses space and is currently on the ground
    public void CallJump(float height)
    {
        CheckGrounded();
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            animator.SetBool("isJumping", true);
            CheckJump(height);
        }
        else
        {
            animator.SetBool("isJumping", false);
        }    
    }

    // Continuously calls Jump() method to mimic the act of jumping
    public void CheckJump(float height)
    {   
        for (int i = 0; i < 7; i++)
        {
            Jump(height);
            height += (i * 2);
        }
    }

    // Moves the player upward
    public void Jump(float jumpHeight)
    {
        player.Move(new Vector3(0f, jumpHeight, 0f) * Time.deltaTime);
    }

    // Crouches player
    public void Crouch(float flexibility)
    {
        // Will be implemented in later version
        // Must have a crouch animation to be coded; currently, no crouch animation is part of the model
    }

    // Returns character's current health
    public float GetHealth()
    {
        return this.currentHealth;
    }

    // DO NOT WORRY ABOUT THIS
    /*private void OnTriggerEnter(Collider other)
    {
        // Damage over time counter
        if (other.tag == "DamagingItem")
        {
            print("Contact Made");
        }
    }*/
}

