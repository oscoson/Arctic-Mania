using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class MainMenuController : MonoBehaviour
{
    
    public void PlayGame()
    {
        // Load the "Game" scene
        SceneManager.LoadScene("GameScene");
    }

    public void Options()
    {
        // Load the "Options" scene
        SceneManager.LoadScene("OptionsScene");
    }
    public void Tutorial()
    {
        // Load the "Tutorial" scene
        SceneManager.LoadScene("TutorialScene");
    }
    public void MainMenu()
    {
        // Load the "MainMenu" scene
        SceneManager.LoadScene("MainMenu"); 
    }
    public void EnterBoss()
    {
        // Load "Boss" Scene
        SceneManager.LoadScene("BossScene");
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
