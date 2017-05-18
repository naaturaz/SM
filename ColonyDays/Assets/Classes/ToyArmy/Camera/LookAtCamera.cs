using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour {

    public bool InvertY;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(Camera.main.transform);

        if (InvertY)
        {
            transform.Rotate(0, 180, 0);

        }
    }
}
