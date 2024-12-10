using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Victory : MonoBehaviour
{
    public GameObject victoryScreen;

    // Start is called before the first frame update
    void Start()
    {
        victoryScreen.SetActive(false);
    }

    public void SetVictory()
    {
        ShowVictoryScreen();
    }
    void ShowVictoryScreen()
    {
        Time.timeScale = 0f;
        AudioListener.pause = true;
        GameManager.gameOver = true;
        victoryScreen.SetActive(true);        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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

