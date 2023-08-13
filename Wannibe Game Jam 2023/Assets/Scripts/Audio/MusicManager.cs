using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance; 

    private AudioSource audioSource;
    private bool isMusicPlaying = false; 

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        PlayMusic();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene loaded: " + scene.name);
        if (scene.name == "MainMenu" || scene.name == "OptionsScene" || scene.name == "TutorialScene" || scene.name == "DeadScene" )
        {
            PlayMusic();
        }
        else
        {
            StopMusic();
        }
    }

    private void PlayMusic()
    {
        if (!isMusicPlaying)
        {        
            audioSource.Play();
            isMusicPlaying = true;
        }
    }

    private void StopMusic()
    {
        if (isMusicPlaying)
        {
            audioSource.Stop();
            isMusicPlaying = false;
        }
    }
}
