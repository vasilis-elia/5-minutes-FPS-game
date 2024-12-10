using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BossMovement : MonoBehaviour
{
    public GameObject player;
    public AudioClip bossWalk;
    public AudioClip bossIdle;
    public AudioClip bossLand;
    public Transform groundCheck;

    public float speed = 5f;
    public float gravity = -80f;
    public float groundDistance = 0.5f;
    public float idleDistance = 5f;
    public LayerMask groundMask;

    private Vector3 velocity;
    float walkTime = 0f;
    float walkDuration;
    bool hasLanded; // For the first time the boss lands (when it spawns)

    Animator animator;
    AudioSource audioSource; // To know when to play the clip again
    CharacterController controller;

    bool isGrounded;

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        walkDuration = bossWalk.length;
        animator.SetTrigger(StringRepo.IdleAnimation); // Spawn on idle animation but don't play the corresponding clip since it will laugh at first
   
        player = GameObject.Find(StringRepo.Player);
        if (player == null)
            Debug.Log("Player object not found");
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Is walking = " + animator.GetCurrentAnimatorStateInfo(0).IsName(StringRepo.Walk));
        //Debug.Log("Is idling = " + animator.GetCurrentAnimatorStateInfo(0).IsName(StringRepo.IdleAnimation));    

        if (GetComponent<Boss>().IsDead())
            return;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Do these the once when then boss lands after being spawned
        if (isGrounded && !hasLanded)
        {
            hasLanded = true;         
            audioSource.PlayOneShot(bossLand);
        }

        // If character is on the ground then reset the vertical speed
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Update the positions and direction towards player on each frame
        Vector3 playerPosition = player.transform.position;
        Vector3 currentPosition = transform.position;
        float distance = (playerPosition - currentPosition).magnitude;
        Vector3 direction = (playerPosition - currentPosition).normalized;

        // Simulating gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Don't perform any specific actions until it lands
        if (!hasLanded)
            return;

        //Debug.Log(distance);
        // Based on the distance from the player perform a different action and a corresponding animation
        if (distance < idleDistance)
        {
            Idle();
        }
        else
        {            
            Walk(direction);            
        }
    }

    void Idle()
    {
        // Only play the idle audio if transitioning to idle animation or have been on the loop for a long time
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(StringRepo.IdleAnimation) ||
            animator.GetCurrentAnimatorStateInfo(0).IsName(StringRepo.IdleAnimation) &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 3f)
        {
            audioSource.priority = 220;
            audioSource.volume = 0.1f;
            audioSource.reverbZoneMix = 0f;
            audioSource.PlayOneShot(bossIdle);
        }

        // For idle animation do not look at the player
        animator.SetTrigger(StringRepo.IdleAnimation);
    }

    void Walk(Vector3 direction)
    {
        walkTime += Time.deltaTime;

        // Only play the walk audio if transitioning to walk animation or have finished the audio clip and still walking
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(StringRepo.Walk) ||
            (animator.GetCurrentAnimatorStateInfo(0).IsName(StringRepo.Walk) && walkTime >= walkDuration))
        {
            audioSource.priority = 220;
            audioSource.volume = 0.07f;
            audioSource.reverbZoneMix = 0f;
            audioSource.PlayOneShot(bossWalk);
            walkTime = 0f;
        }

        LookAt(direction);
        animator.SetTrigger(StringRepo.Walk);

        controller.Move(speed * Time.deltaTime * direction);
    }

    void LookAt(Vector3 direction)
    {
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        controller.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }
        public bool HasLanded()
    {
        return hasLanded;
    }
 }