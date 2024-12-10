using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public static bool isPaused = false; // Need this to check before doing any inputs (otherwise input work in pause)
    
    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false); // Disable initially the pause screen and enable it when ESC is pressed
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.gameOver && Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {               
                ResumeGame();
            }
            else
            {                
                PauseGame();
            }                
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        AudioListener.pause = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f; // Pauses the in game time
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        isPaused = false;
        AudioListener.pause = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f; // Resumes the in game time
        Cursor.lockState = CursorLockMode.Locked; // To lock cursor in game window 
        Cursor.visible = false;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(StringRepo.MainMenu);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
