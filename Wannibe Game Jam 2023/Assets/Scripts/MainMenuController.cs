using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class MainMenuController : MonoBehaviour
{
    
    public void PlayGame()
    {
        // Load the "Game" scene
        SceneManager.LoadScene("GameScene"); // Replace "Game" with your actual game scene name
    }

    public void Options()
    {
        // Load the "Options" scene
        SceneManager.LoadScene("OptionsScene"); // Replace "Options" with your actual options scene name
    }
    public void MainMenu()
    {
        // Load the "MainMenu" scene
        SceneManager.LoadScene("MainMenu"); // Replace "MainMenu" with your actual main menu scene name
    }

    public void QuitGame()
    {
        // Quit the application
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
        Debug.Log("Quit Game");
    }


}
