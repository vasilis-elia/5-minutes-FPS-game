using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RakeDoDamage : MonoBehaviour 
{ 
    public GameObject player;
    public GameObject rakeModel;

    Animator animator;

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
        // Need to get the distance between to know if player is about to take damage
        Vector3 targetPosition = player.transform.position;
        Vector3 currentPosition = transform.position;
        float distance = (targetPosition - currentPosition).magnitude;

        // Damage should only be given when the Rake animation is finishing the attack
        float animationTiming = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        Debug.Log("distance = " + distance);
        Debug.Log("time = " + animationTiming);    
        Debug.Log(animator.GetCurrentAnimatorStateInfo(0).IsName(StringRepo.Attack2Animation));
        if (distance < 10f && animator.GetCurrentAnimatorStateInfo(0).IsName(StringRepo.Attack2Animation)
            && animationTiming > 0.5f && animationTiming < 0.8f)
        {
            player.GetComponent<Player>().TakeDamage(1f);
        }
    }
    IEnumerator Waiter()
    {
        yield return new WaitForSeconds(0.5f);
    }
}