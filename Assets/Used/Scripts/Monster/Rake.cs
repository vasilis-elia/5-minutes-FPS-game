using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rake : MonoBehaviour
{
    public float health = 2f;
    public GameObject rake;
    public AudioClip deathClip;    

    Animator animator;
    Boolean isDead = false; // After dying don't do anything else

    private void Start()
    {
        animator = rake.GetComponent<Animator>();
    }

    // If the damage is lethal call die, otherwise take damage and play Hit animation
    public void TakeDamage(float damage)
    {
        health -= damage;

        if (!isDead && health <= 0f)
        {
            Die();           
        }
        else if (!isDead)
        {
            animator.SetTrigger(StringRepo.HitAnimation);   
        }
    }

    // Death triggers a death animation and destruction of the object after 20 seconds
    private void Die()
    {
        isDead = true;
        GetComponent<AudioSource>().PlayOneShot(deathClip, 2.0f);
        animator.SetTrigger(StringRepo.DieAnimation);
        GameManager.currentCount++; // Count the kill towards the total count
        Destroy(gameObject, 20);
    }

    public Boolean IsDead()
    {
        return isDead;
    }
}
