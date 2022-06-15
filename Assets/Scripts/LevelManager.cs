using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// This whole new class is created for the purposes of Scene and Level Management.
public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject levelCompleteMenu;
    [SerializeField] private int currentSceneIndex;

    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        currentSceneIndex = currentScene.buildIndex;
    }

    // If the player dies, the Game Over screen pops up. The player can either restart the level, or quit.
    public void ShowGameOver()
    {
        gameOverMenu.SetActive(true);
    }

    // If the player hits the goal, the player wins. The player can move on to the next level, restart the level, or quit.
    public void ShowLevelComplete()
    {
        levelCompleteMenu.SetActive(true);
    }

    // Note: I am aware that this is not the best way to handle loading the next level,
    // but I figured that if I have a final scene with a quit button, it should still work.
    public void StartNextLevel()
    {
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void QuitGame()
    {
        // It won't work while in Play Mode in Unity, but if the game is actually built, this will shut down the program.
        Application.Quit();
    }
}
