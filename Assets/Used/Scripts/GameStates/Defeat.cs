using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Defeat : MonoBehaviour
{
    public GameObject defeatScreen;

    // Start is called before the first frame update
    void Start()
    {            
        defeatScreen.SetActive(false);      
    }

    // Method to be called from other objects
    public void SetDefeat()
    {        
        ShowDefeatScreen();      
    }

    void ShowDefeatScreen()
    {
        Time.timeScale = 0f;
        defeatScreen.SetActive(true);
        GameManager.gameOver = true;
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

