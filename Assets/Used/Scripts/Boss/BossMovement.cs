using UnityEngine;
using static UnityEngine.SpriteMask;

public class BossMovement : MonoBehaviour
{
    public GameObject player;
    public AudioClip bossWalk;
    public AudioClip bossIdle;
    public AudioClip bossLand;
    public Transform groundCheck;    

    public float speed = 12f;
    public float gravity = -80f;
    public float groundDistance = 1f;
    
    public LayerMask groundMask;

    private Vector3 velocity;
    float idleDistance; // Same as AoE damage range
    bool hasLanded; // For the first time the boss lands (when it spawns)

    Animator animator;
    AudioSource audioSource; // To know when to play the clip again
    AudioSource walkSource; // To stop the walk audio when boss stops walking
    AudioSource idleSource;
    CharacterController controller;

    bool isGrounded;

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        idleSource = gameObject.AddComponent<AudioSource>();
        walkSource = gameObject.AddComponent<AudioSource>(); 
        animator.SetTrigger(StringRepo.IdleAnimation); // Spawn on idle animation but don't play the corresponding clip since it will laugh at first

        idleDistance = GetComponent<BossAoE>().range;
        Debug.Log(idleDistance);

        player = GameObject.Find(StringRepo.Player);
        if (player == null)
            Debug.Log("Player object not found");
    }

    // Update is called once per frame
    void Update()
    {
        // When boss dies perform idle animation, but don't move
        if (GetComponent<Boss>().IsDead())
        {
            animator.SetTrigger(StringRepo.IdleAnimation);
            return;
        }
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Do these the once when then boss lands after being spawned
        if (isGrounded && !hasLanded)
        {
            hasLanded = true;
            GameManager.bossLanded = true;
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

        // When the player is close the boss the boss idles (does AoE damage), and walk to them when their away
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
        // Walking sound effect should when the boss stops walking
        walkSource.Stop();
        
        // If idleAnimation is on but they corresponding audio clip is not playing then play it
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(StringRepo.IdleAnimation) && !idleSource.isPlaying)
        {
            idleSource.PlayOneShot(bossIdle, 0.5f);
          
        }
        // For idle animation do not look at the player
        animator.SetTrigger(StringRepo.IdleAnimation);
    }

    void Walk(Vector3 direction)
    {      
        // Allow the boss to perform at least one idle animation cycle before interrupting him to walk
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(StringRepo.IdleAnimation) &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1f)
            return;

        // If walkAnimation is on but they corresponding audio clip is not playing then play it
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(StringRepo.Walk) && !walkSource.isPlaying)
        {
            walkSource.PlayOneShot(bossWalk, 0.3f);
            walkSource.spatialBlend = 0.4f;
            walkSource.reverbZoneMix = 0.2f;
        }
        animator.SetTrigger(StringRepo.Walk);

        // Only move and look at player when walking
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(StringRepo.Walk))
        {
            LookAt(direction);
            controller.Move(speed * Time.deltaTime * direction);
        }
    }

    void LookAt(Vector3 direction)
    {
        direction.y = 0; // So the boss only looks forward
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        controller.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }
        public bool HasLanded()
    {
        return hasLanded;
    }
 }