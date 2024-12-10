using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{   
    public AudioClip laughClip;
    public GameObject gameManager;

    public float health = 20f;

    Rigidbody rb;
    AudioSource audiosource;
    Animator animator;

    bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {       
        audiosource = GetComponent<AudioSource>(); // Audio source is only for the death sound in order to know when it ends to go to victory screen
        audiosource.volume = 0.7f;
        audiosource.PlayOneShot(laughClip);
        animator = GetComponent<Animator>();
        gameManager = GameObject.Find(StringRepo.GameManager);
        if (gameManager == null)
            Debug.Log("Boss Ceiling object not found");
    }

    // If the damage is lethal call die, otherwise take damage and play Hit animation
    public void TakeDamage(float damage)
    {      
        health -= damage;

        if (health <= 0f)
        {
            isDead = true;         
            StartCoroutine(Die());
        }    
    }

    // Death triggers a death audio clip and after the audio clip ends goes to victory screen. Need to wait therefore use enumerator
    private IEnumerator Die()
    {          
        audiosource.reverbZoneMix = 0f;
        audiosource.volume = 0.3f;
        animator.SetTrigger(StringRepo.IdleAnimation);
        audiosource.Play(); // Death audio clip         

        yield return new WaitForSeconds(audiosource.clip.length); // Wait for audio death sound to end before proceeding to the victory screen      
        gameManager.GetComponent<Victory>().SetVictory();
    }
    
    public bool IsDead()
    {
        return isDead;
    }
}