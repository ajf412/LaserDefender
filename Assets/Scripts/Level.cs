using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    // LOAD START MENU
    public void LoadStartMenu()
    {
        SceneManager.LoadScene("Start Menu");
    }

    // LOAD GAME
    public void LoadGameScene()
    {
        SceneManager.LoadScene("Game");
        FindObjectOfType<GameSession>().ResetGame();
    }
    
    // LOAD GAME OVER
    public void LoadGameOver()
    {
        StartCoroutine(DeathDelay());
    }
    // death delay
    private IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("Game Over");
    }

    // QUIT GAME
    public void QuitGame()
    {
        Application.Quit();
    }
}
