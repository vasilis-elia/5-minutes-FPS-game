using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static int maxHealth = 2;
    public static int currentHealth;
    public GameObject healthBar;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    Boolean isDead = false; // After dying don't do anything else

    // If the damage is lethal call die, otherwise take damage
    public void TakeDamage(int damage)
    {      
        currentHealth -= damage;

        // Update the health bar canvas
        healthBar.GetComponent<HealthBar>().UpdateHealthBar();

        if (!isDead && currentHealth <= 0f)
        {
            Die();
        }  
    }
    private void Die()
    {
        isDead = true;
        // Show game over screen
        Destroy(gameObject);
    }
    public Boolean IsDead()
    {
        return isDead;
    }
}