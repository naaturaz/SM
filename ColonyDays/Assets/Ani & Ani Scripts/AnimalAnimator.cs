using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class AnimalAnimator : MonoBehaviour
{
    private Animator myAnimator;

    void Start()
    {
        myAnimator = gameObject.GetComponent<Animator>();
        
    }

    void Update()
    {
        myAnimator.speed = Program.gameScene.GameSpeed;
    }

}

