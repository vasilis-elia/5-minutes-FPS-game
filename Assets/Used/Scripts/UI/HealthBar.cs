using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealthBar : MonoBehaviour
{        
    public GameObject healthBlockPrefab;  

    private Transform healthBar;
    private int oldHealth; // To know what hp blocks to remove

    void Start()
    {
        oldHealth = Player.currentHealth;  // Initialize current health to max health
        healthBar = transform;      

        // Fill the health bar with blocks based on the max health
        for (int i = 0; i < 2; i++)
        {
            Instantiate(healthBlockPrefab, healthBar); 
        }
    }

    public void UpdateHealthBar()
    {
        // Destroy all the hp blocks that correspond to the hp that the player has lost
        for (int i = oldHealth; i < Player.currentHealth; i++)
        {
            Destroy(healthBar.GetChild(i).gameObject);
        }       
         
        oldHealth = Player.currentHealth;
    }
}
