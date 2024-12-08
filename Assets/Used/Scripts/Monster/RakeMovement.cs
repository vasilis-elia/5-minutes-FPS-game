using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;

public class RakeMovement : MonoBehaviour
{
    public CharacterController controller; // Linked with CharacterController component
    public GameObject player;
    public GameObject rakeModel;

    public float speed = 30f;
    public float gravity = -30f;
    public float aggroDistance = 80f;

    public Transform groundCheck; // Linked with GroundCheck object
    public float groundDistance = 0.6f; // Sphere radius
    public LayerMask groundMask; // To know when the character is on the ground, in order to reset vertical velocity

    private Vector3 verticalVelocity;
    private Animator animator;

    private bool isGrounded;

    private void Start()
    {
        animator = rakeModel.GetComponent<Animator>();
        // So the player is certain to be instantiated first
        StartCoroutine(Waiter());

        player = GameObject.Find(StringRepo.Player);
        if (player == null)
            Debug.Log("Player object not found");
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // If character is on the ground then reset the vertical speed
        if (isGrounded && verticalVelocity.y < 0)
        {
            verticalVelocity.y = -2f;
        }

        // Update the positions and direction towards player on each frame
        Vector3 targetPosition = player.transform.position;
        Vector3 currentPosition = transform.position;
        float distance = (targetPosition - currentPosition).magnitude;
        Vector3 direction = (targetPosition - currentPosition).normalized;

        // Ignore the difference in y position
        direction.y = 0f;

        // Simulating gravity
        verticalVelocity.y += gravity * Time.deltaTime;
        controller.Move(verticalVelocity * Time.deltaTime);

        // Do not move or change animations if it's dead
        if (GetComponent<Rake>().IsDead())
            return;

        // Based on the distance from the player perform a different action and a corresponding animation
        if (distance > aggroDistance)
        {
            Idle();
        }
        else if (distance < aggroDistance && distance > 10f)
        {
            Run(direction);
        }
        else
        {
            Attack(direction);
        }
    }
    void Idle()
    {
        // Need to check that the animation to be activated is not already activated in order to not reset
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(StringRepo.IdleAnimation))
        {
            animator.SetTrigger(StringRepo.IdleAnimation);
        }
    }
    void Run(Vector3 direction)
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(StringRepo.RunAnimation)
            && !animator.GetCurrentAnimatorStateInfo(0).IsName(StringRepo.Attack2Animation))
            animator.SetTrigger(StringRepo.RunAnimation);

        LookAt(direction);
        controller.Move(speed * Time.deltaTime * direction);
    }

    void Attack(Vector3 direction)
    {
        // Do the attack animation only if it's not doing the attack animation
        // or if the attack animation is almost done
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(StringRepo.Attack2Animation)
            || (animator.GetCurrentAnimatorStateInfo(0).IsName(StringRepo.Attack2Animation)
            && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f))
        {
            animator.SetTrigger(StringRepo.Attack2Animation);
        }
        LookAt(direction);
    }

    // To rotate the character smoothly
    void LookAt(Vector3 direction)
    {       
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        controller.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }
    IEnumerator Waiter()
    {
        yield return new WaitForSeconds(0.5f);
    }
}

