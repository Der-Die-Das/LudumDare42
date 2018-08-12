using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool PlayTutorial
    {
        get
        {
            return PlayerPrefs.GetInt("Tutorial", 0) == 0 ? true : false;
        }
    }

    private void Start()
    {
        if (FindObjectsOfType<GameManager>().Length > 1)
        {
            DestroyImmediate(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
    public void TutorialPlayed()
    {
        PlayerPrefs.SetInt("Tutorial", 1);
    }

    public void ReplayTutorial()
    {
        PlayerPrefs.SetInt("Tutorial", 0);
    }
}
