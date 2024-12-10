using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool gameOver;
    public static bool isDead; // After dying don't do anything else
    public static bool bossSpawned;
    public static bool bossLanded;
    public static bool timesUp;
    public static bool hasFinished; // The player has finished all the necessary kills (no more spawns should happen)
    public static int currentCount; // Current number of kills 
    public static int currentHealth;   

    public GameObject rake;
    public GameObject portal;
    public GameObject portalCollider;
    public GameObject bossEntranceDetector;
    public GameObject player;
    public GameObject bossCeiling;
    public GameObject bossSpawner;
    public GameObject boss;

    public int killThreshold = 5; // Number of kills required to open the door   

    // Start is called before the first frame update
    void Start()
    {    
        PauseMenu.isPaused = false;
        AudioListener.pause = false;
        timesUp = false;
        isDead = false;
        gameOver = false;
        hasFinished = false;
        currentCount = 0;
        bossSpawned = false;
        bossLanded = false;
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked; // To lock cursor in game window 
        Cursor.visible = false;
        bossCeiling.SetActive(false); // This is enabled back via BossMovement (when boss lands)
    }

    // Update is called once per frame
    void Update()
    {
        if (timesUp)
            return;

        if (!bossSpawned)
            CheckForCompletion();

        // If the player has not finished with the kills and the timer is up then update timesUp variable
        if (!hasFinished && GetComponent<CountdownTimer>().IsTimeUp())
            TimerUp();

        // If the player has finished with kills, check their distance to the boss area and trigger events if so
        if (hasFinished && !bossSpawned)
            CheckForBossRoomEntry();

        // Enables the ceiling in the boss area
        if (bossLanded)
            bossCeiling.SetActive(true);      
    }

    // If the timer is up then all spawners spawn a rake and each rake has now infinite aggro distance
    void TimerUp()
    {  
        timesUp = true;
    }
    
    // Triggers events and sets hasFinished to true
    void CheckForCompletion()
    {
        if (!hasFinished && currentCount >= killThreshold)
        {
            // To change the color of the portal to green (has multiple objects of the same shader)
            Transform parentPortal = portal.transform;
            Color green = new Color(0, 30, 0);
            foreach (Transform child in parentPortal)
            {                
                child.GetComponent<Renderer>().material.SetColor("_Fill_Color", green);               
            }
            portal.GetComponent<Renderer>().material.SetColor("_Fill_Color", green);

            // Plays a sound for portal opening
            portal.GetComponent<AudioSource>().Play();          

            // Removes the portal collider
            portalCollider.SetActive(false);            
            
            hasFinished = true;            
        }            
    }

    // Trigger boss events when the player nears the boss area
    void CheckForBossRoomEntry()
    {
        Vector3 playerPosition = player.transform.position;
        Vector3 bossPosition = bossEntranceDetector.transform.position;
        float distance = (playerPosition - bossPosition).magnitude;

        if (distance < 80)
        {
            bossSpawned = true;
            TriggerBossEvents();
        }
    }

    void TriggerBossEvents()
    {
        // Bring back the portal collider
        portalCollider.SetActive(true);

        // Turn the portal color back to red
        Transform parentPortal = portal.transform;
        Color red = new Color(30, 0, 0);
        foreach (Transform child in parentPortal)
        {
            child.GetComponent<Renderer>().material.SetColor("_Fill_Color", red);
        }
        portal.GetComponent<Renderer>().material.SetColor("_Fill_Color", red);

        // Spawns boss
        bossSpawner.GetComponent<BossSpawner>().Spawn();   
    }
}
