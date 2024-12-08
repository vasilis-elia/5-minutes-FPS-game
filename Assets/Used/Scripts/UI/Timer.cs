using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountdownTimer : MonoBehaviour
{
    public float startTime = 60f;
    private float currentTime;    
    public TMP_Text timerText;       

    void Start()
    {
        currentTime = startTime; // Initialize the timer
        UpdateTimerDisplay();    // Update the text display at start
    }

    void Update()
    {
        // Reduce time if it hasn't reached 0
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;          
            UpdateTimerDisplay();
        }
        else
            currentTime = 0;
    }

    void UpdateTimerDisplay()
    {
        // Convert time to minutes and seconds
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);

        // Update the text component
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
