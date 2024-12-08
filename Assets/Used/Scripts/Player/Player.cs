using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float health = 2f;

    Boolean isDead = false; // After dying don't do anything else

    // If the damage is lethal call die, otherwise take damage
    public void TakeDamage(float damage)
    {
        Debug.Log("test damage");
        health -= damage;

        if (!isDead && health <= 0f)
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