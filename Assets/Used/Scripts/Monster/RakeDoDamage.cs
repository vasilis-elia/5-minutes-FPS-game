using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class RakeDoDamage : MonoBehaviour 
{ 
    public GameObject player;
    public GameObject rakeModel;
    public AudioClip attackClip;

    Animator animator;
    bool canAttack = true;
    float audioAttackCooldown = 1.5f;
    float audioTimer = 0f;

    public void Start()
    {
        animator = rakeModel.GetComponent<Animator>();

        // So the player is certain to be instantiated first
        StartCoroutine(Waiter());

        player = GameObject.Find(StringRepo.Player);
        if (player == null)
            Debug.Log("Player object not found");
    }
    private void Update()
    {
        // If the rake is dead nothing should happen
        if (GetComponent<Rake>().IsDead())
            return;   

        // Need to get the distance between to know if player is about to take damage
        Vector3 targetPosition = player.transform.position;
        Vector3 currentPosition = transform.position;
        float distance = (targetPosition - currentPosition).magnitude;

        //Debug.Log(animator.GetCurrentAnimatorStateInfo(0).IsName(StringRepo.Attack2Animation));
       
        // Damage should only be given when the Rake animation is finishing the attack
        float animationTiming = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        audioTimer += Time.deltaTime;

        if (animationTiming > 0.3f && animationTiming < 0.5f && audioTimer > audioAttackCooldown)
        {
            GetComponent<AudioSource>().PlayOneShot(attackClip);
            audioTimer = 0f;
        }            

        // If the animation timing is below the threshold of the animation that it can do 
        // damage then it's a new animation and the rake can do damage again
        if (animationTiming < 0.5f)
        {
            canAttack = true;
        }   
   
        if (canAttack && distance < 10f && animator.GetCurrentAnimatorStateInfo(0).IsName(StringRepo.Attack2Animation)
            && animationTiming > 0.5f && animationTiming < 0.8f)
        {           
            canAttack = false;          
            player.GetComponent<Player>().TakeDamage(1);
        }
    }
    IEnumerator Waiter()
    {
        yield return new WaitForSeconds(0.5f);
    }
}