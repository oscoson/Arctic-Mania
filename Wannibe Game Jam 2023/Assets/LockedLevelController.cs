using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LockedLevelController : MonoBehaviour
{
    [SerializeField] private string levelName;
    [SerializeField] private string buttonName;
    
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.HasKey(levelName))
        {
            gameObject.GetComponent<Button>().enabled = true;
            gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = buttonName;
        }
        else
        {
            gameObject.GetComponent<Button>().enabled = false;
        }
    }

}
