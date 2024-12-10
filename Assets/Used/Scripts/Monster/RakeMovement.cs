using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Animations;

public class RakeMovement : MonoBehaviour
{
    public CharacterController controller; // Linked with CharacterController component
    public GameObject player;
    public GameObject rakeModel;  
  
    public float speed = 60f;
    public float gravity = -40f;
    public float aggroDistance = 200f;
    public float jumpHeight = 55f;
    public float jumpTimer = 5f; // Jump cooldown

    public Transform groundCheck; // Linked with GroundCheck object
    public float groundDistance = 0.5f; // Sphere radius
    public LayerMask groundMask; // To know when the character is on the ground, in order to reset vertical velocity

    private Vector3 velocity;
    private Animator animator;

    private bool isGrounded;
    float currentTimer = 0f;

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
        if (GetComponent<Rake>().IsDead())
        {
            // Makes the body fall on the ground
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
            return;
        }           

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // If character is on the ground then reset the vertical speed
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
      
        // Update the positions and direction towards player on each frame
        Vector3 targetPosition = player.transform.position;
        Vector3 currentPosition = transform.position;
        float distance = (targetPosition - currentPosition).magnitude;
        Vector3 direction = (targetPosition - currentPosition).normalized;

        if (GameManager.timesUp)
            setTimeUpValues();
     
        // Jump high to avoid obstacles       
        velocity.y = jump(velocity.y);

        // Simulating gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);     

        // Based on the distance from the player perform a different action and a corresponding animation
        if (distance > aggroDistance)
        {
            Idle();
        }
        else if (distance < aggroDistance && distance > 14f)
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
        // If the rake hit wait for the animation to end 
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(StringRepo.HitAnimation)
            && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            return;

        // Need to check that the animation to be activated is not already activated in order to not reset
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(StringRepo.IdleAnimation))
        {
            animator.SetTrigger(StringRepo.IdleAnimation);
        }
    }
    void Run(Vector3 direction)
    {
        LookAt(direction);

        // If the rake hit wait for the animation to end 
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(StringRepo.HitAnimation)
            && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            return;

        // So it breaks the attacking loop
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(StringRepo.Attack2Animation)
            && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            animator.SetTrigger(StringRepo.IdleAnimation);

        // If it is attacking move slower otherwise run
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(StringRepo.Attack2Animation))
        {
            controller.Move(speed * Time.deltaTime / 8f * direction);
        }
        else
        {
            controller.Move(speed * Time.deltaTime * direction);
            animator.SetTrigger(StringRepo.RunAnimation);
        }        
    }

    void Attack(Vector3 direction)
    {
        // If the rake hit wait for the animation to end 
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(StringRepo.HitAnimation)
            && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            return;

        LookAt(direction);

        // Move when attacking but slower
        controller.Move(speed * Time.deltaTime / 8f * direction);
        animator.SetTrigger(StringRepo.Attack2Animation);                
    }

    // To rotate the character smoothly
    void LookAt(Vector3 direction)
    {       
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        controller.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }

    // Jump high every jumpTimer seconds (to avoid obstacles)
    float jump(float vel)
    {       
        if (currentTimer >= jumpTimer)
        {
            vel = Mathf.Sqrt(jumpHeight * (-2f) * gravity);
            currentTimer = 0f;
        }
        currentTimer += Time.deltaTime;
        return vel;
    }

    // Rake has infinite range, is faster and jumps heigher when the time is up
    void setTimeUpValues()
    {
        aggroDistance = 5000f;
        speed = 100f;
        jumpHeight = 200f;
    }

    IEnumerator Waiter()
    {
        yield return new WaitForSeconds(0.5f);
    }
}

