using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool gameOver = false;
    public static Boolean isDead = false; // After dying don't do anything else
    public static int currentHealth;

    // Start is called before the first frame update
    void Start()
    {    
        PauseMenu.isPaused = false;
        isDead = false;
        gameOver = false;
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
