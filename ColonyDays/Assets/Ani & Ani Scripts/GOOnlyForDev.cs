using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


class GOOnlyForDev : MonoBehaviour
{
    void Start()
    {
        if (!Developer.IsDev)
        {
            gameObject.SetActive(false);
        }
    }
}

