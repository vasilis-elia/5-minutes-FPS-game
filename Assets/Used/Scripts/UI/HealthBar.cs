using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealthBar : MonoBehaviour
{        
    public GameObject healthBlockPrefab;
    public GameObject player;

    private Transform healthBar;
    private int oldHealth; // To know what hp blocks to remove

    void Start()
    {
        oldHealth = player.GetComponent<Player>().maxHealth; // Initialize current health to max health
        healthBar = transform;       

        // Fill the health bar with blocks based on the max health
        for (int i = 0; i < player.GetComponent<Player>().maxHealth; i++)
        {
            Instantiate(healthBlockPrefab, healthBar); 
        }
    }

    public void UpdateHealthBar()
    {
        // Destroy all the hp blocks that correspond to the hp that the player has lost.
        // If currentHealth goes under 0, don't do anything
        for (int i = oldHealth - 1; i >= Mathf.Max(0, GameManager.currentHealth); i--)
        {
            Destroy(healthBar.GetChild(i).gameObject);
        }       
         
        oldHealth = GameManager.currentHealth;
    }
}
