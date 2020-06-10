using UnityEngine;
using UnityEngine.SceneManagement;


class RestartGameMenu : MonoBehaviour
{
    public static bool GameEnded = false;

    // Reference to GameOverUI 
    public GameObject RestartMenuUI;

    /// <summary>
    /// Function that gets called  evey frame
    /// </summary>
    private void Update()
    {
        if (!GameEnded && Player.Dead())
            StopTheGame();
    }

    /// <summary>
    /// Function that calls UI  part when player die
    /// </summary>
    public void StopTheGame()
    {
        RestartMenuUI.SetActive(true);
        GameEnded = true;
    }

    /// <summary>
    /// Function that corresponds to a restart button
    /// </summary>
    public void RestartGame()
    {
        ScoreCounter.resetScore();
        Player.Reset();
        GameEnded = false;
        SceneManager.LoadScene(1);
    }

    public static void RestartGameScene()
    {
        ScoreCounter.resetScore();
        GameEnded = false;
        SceneManager.LoadScene(1);
    }
    /// <summary>
    /// Function that corresponds to a menu button
    /// </summary>
    public void LoadMenu()
    {
        GameEnded = false;
        SceneManager.LoadScene(0);
    }
}
