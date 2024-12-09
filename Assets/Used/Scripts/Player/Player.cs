using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{   
    public GameObject healthBar;
    public GameObject gameManager;   
    public int maxHealth = 2;  

    private void Start()
    {
        GameManager.currentHealth = maxHealth;       
    }    

    // If the damage is lethal call die, otherwise take damage
    public void TakeDamage(int damage)
    {
        if (GameManager.currentHealth == 0)
            return;

        GameManager.currentHealth -= damage;    

        // Update the health bar canvas
        healthBar.GetComponent<HealthBar>().UpdateHealthBar();

        if (!GameManager.isDead && GameManager.currentHealth <= 0)
        {
            Die();
        }  
    }
    private void Die()
    {          
        GameManager.isDead = true;
        gameManager.GetComponent<Defeat>().SetDefeat();
    }
}