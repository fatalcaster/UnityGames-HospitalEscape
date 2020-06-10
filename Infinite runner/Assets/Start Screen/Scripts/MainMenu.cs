using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    
    /// <summary>
    /// Function that corresponds to a play button
    /// </summary>
    public void PlayGame()
    {
        Player.health = 3;
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
        PauseMenu.GameIsPaused = false;
    }

    /// <summary>
    /// Function that corresponds to a quit button
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
