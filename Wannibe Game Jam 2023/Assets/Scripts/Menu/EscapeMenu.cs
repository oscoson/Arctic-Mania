using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class EscapeMenu : MonoBehaviour
{
    public GameObject escapeCanvas; // Reference to the Canvas GameObject
    public GameObject deathCanvas; 
    [SerializeField] public Player player;
    // private bool isPaused = false;
    
    private void Start()
    {
        escapeCanvas.SetActive(false);
        deathCanvas.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleEscapeCanvas();
        }
        // if player dies, show death canvas
        if (player.isDead == true)
        {
            ToggleDeathCanvas();
        }
    }

    public void ToggleDeathCanvas()
    {
        Time.timeScale = 0f;
        deathCanvas.SetActive(true);

    }

    public void ToggleEscapeCanvas()
    {
        bool canvasActive = escapeCanvas.activeSelf;
        escapeCanvas.SetActive(!canvasActive);

        Time.timeScale = canvasActive ? 1f : 0f; // Pause the game when the menu is active
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
