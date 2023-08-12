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
        if (scene.name == "GameScene")
        {
            StopMusic();
        }
        else
        {
            PlayMusic();
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
