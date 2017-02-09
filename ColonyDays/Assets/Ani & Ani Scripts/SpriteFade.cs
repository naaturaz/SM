using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

//ommitted using unity engine and other

public class SpriteFade : MonoBehaviour
{
    public float Speed = 0.02f;
    Color color;

    void Start()
    {
       color = GetComponent<Image>().color;

    }

    void Update()
    {
        color.a -= Speed;
        GetComponent<Image>().color = color;
    }


    void MakeAlphaColorZero(GameObject g)
    {
        Color bl = Color.white;
        bl.a = 0f;
    }
    
}