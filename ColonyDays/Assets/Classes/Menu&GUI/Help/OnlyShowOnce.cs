using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyShowOnce : MonoBehaviour
{

    public string PlayerPrefStringParam;

    // Use this for initialization
    void Start()
    {
        if (!string.IsNullOrEmpty(PlayerPrefs.GetString(PlayerPrefStringParam)))
        {
            gameObject.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Called from GUI
    /// 
    /// closes this and marks the PlayerPref 
    /// so this is not used more on this PC  
    /// </summary>
    public void Close()
    {
        gameObject.SetActive(false);
        PlayerPrefs.SetString(PlayerPrefStringParam, "Used");
    }
}
