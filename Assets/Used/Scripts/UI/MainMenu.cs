using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{    public void PlayGame()
    {
        //Time.timeScale = 0f;
        //PauseMenu.isPaused = true;
        SceneManager.LoadScene(StringRepo.GameScene);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
