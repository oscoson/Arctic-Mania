using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class EscapeMenu : MonoBehaviour
{
    public GameObject escapeCanvas; // Reference to the Canvas GameObject

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleEscapeCanvas();
        }
    }

    public void ToggleEscapeCanvas()
    {
        bool canvasActive = escapeCanvas.activeSelf;
        escapeCanvas.SetActive(!canvasActive);

        Time.timeScale = canvasActive ? 1f : 0f; // Pause the game when the menu is active
    }

    public void ReturnToMainMenu()
    {
        // Load the main menu scene or perform any necessary actions
        SceneManager.LoadScene("MainMenu"); // Uncomment and customize this line
    }

}
