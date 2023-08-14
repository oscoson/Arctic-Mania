using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] AudioSource MusicSource;

    [SerializeField] AudioMixer mixer;
    public const string MASTER_KEY = "masterVolume";
    public const string MUSIC_KEY = "musicVolume";
    public const string SFX_KEY = "sfxVolume";

    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.Log("Destroying duplicate AudioManager");
            Destroy(gameObject);
        }
        loadVolume();

        SceneManager.sceneUnloaded += OnSceneUnloaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneUnloaded(Scene scene)
    {
        
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // If a scene should have new music on load, add it here
        if (scene.name == "GameScene" || scene.name == "BossScene" || scene.name == "DeadScene")
        {
            instance = null;
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void loadVolume()
    {
        Debug.Log("Loading volume");
        float masterVolume = PlayerPrefs.GetFloat(MASTER_KEY, 1f);
        float musicVolume = PlayerPrefs.GetFloat(MUSIC_KEY, 0.75f);
        float sfxVolume = PlayerPrefs.GetFloat(SFX_KEY, 0.75f);

        mixer.SetFloat(VolumeSettings.MIXER_MASTER, Mathf.Log10(masterVolume) * 20);
        mixer.SetFloat(VolumeSettings.MIXER_MUSIC, Mathf.Log10(musicVolume) * 20);
        mixer.SetFloat(VolumeSettings.MIXER_SFX, Mathf.Log10(sfxVolume) * 20);
    }

    public void PlaySFX(AudioClip clip)
    {
        MusicSource.PlayOneShot(clip);
    }

    public void PlayTestSound()
    {
        MusicSource.PlayOneShot(musicClip);
    }

    public void StopMusic()
    {
        MusicSource.Stop();
    }
}
