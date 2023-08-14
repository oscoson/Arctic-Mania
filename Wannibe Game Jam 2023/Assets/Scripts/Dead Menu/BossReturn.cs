using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossReturn : MonoBehaviour
{

    public void RestartScene()
    {
        SceneManager.LoadScene("BossScene");
    }
}
