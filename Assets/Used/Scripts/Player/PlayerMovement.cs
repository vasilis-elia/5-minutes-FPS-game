using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller; // Linked with CharacterController component
    public float speed = 30f;
    public float gravity = -30f;
    public float jumpHeight = 3f;

    public Transform groundCheck; // Linked with GroundCheck object
    public float groundDistance = 0.6f; // Sphere radius
    public LayerMask groundMask; // To only collide with ground

    Vector3 velocity;
    bool isGrounded;

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // If character is on the ground and is not jumping (y < 0) then stay on the ground
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis(StringRepo.HorizontalInput);
        float z = Input.GetAxis(StringRepo.VerticalInput);

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);
        

        if (Input.GetButtonDown(StringRepo.JumpButton) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * (-2f) * gravity); // Conservation of energy to find necessary inital vertical speed
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}
