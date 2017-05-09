using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnReset : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Reset()
    {
        PlayerPrefs.SetString("Tuto", "");
        PlayerPrefs.SetInt("Rate", 0);
        PlayerPrefs.SetString("F1", "");
    }

}
