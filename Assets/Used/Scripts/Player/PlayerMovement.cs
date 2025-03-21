using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller; // Linked with CharacterController component
    public float speed = 20f;
    public float gravity = -80f;
    public float jumpHeight = 10f;

    public Transform groundCheck; // Linked with GroundCheck object
    public float groundDistance = 0.5f; // Sphere radius
    public LayerMask groundMask; // To know when the character is on the ground, in order to reset vertical velocity

    Vector3 velocity;
    bool isGrounded;

    void Update()
    {
        if (GameManager.gameOver)
            return;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // If character is on the ground and is not jumping (y < 0) then stay on the ground
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        float x = 0f;
        float z = 0f;

        if (!PauseMenu.isPaused)
        {
            x = Input.GetAxis(StringRepo.HorizontalInput);
            z = Input.GetAxis(StringRepo.VerticalInput);
        }           

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        // To keep the body of the player correctly oriented
        Vector3 currentRotation = transform.eulerAngles;       
        transform.eulerAngles = new Vector3(0f, currentRotation.y, 0f);

        if (!PauseMenu.isPaused)
            if (Input.GetButtonDown(StringRepo.JumpButton) && isGrounded)        
                velocity.y = Mathf.Sqrt(jumpHeight * (-2f) * gravity); // Conservation of energy to find necessary inital vertical speed
        
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}