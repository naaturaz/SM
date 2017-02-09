using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// If a new emoticon is added on Good or Bad please update the random range 
/// </summary>

public class EmoticonManager
{
    static Transform _canvas;

    static List<string> _goodWords = new List<string>()
    {
        "Built", "Hired"
    };

    static List<string> _badWords = new List<string>()
    {
        "Demolish", "Fired"
    };


    static bool wasInit;
    static void Init()
    {
        if (wasInit)
        {
            return;
        }
        wasInit = true;

        _canvas = GameObject.Find("Canvas").transform;
    }

    public static void Show(string which, Vector3 pos)
    {
        Init();

        which = ReturnIsGoodOrBad(which);
        var root = ReturnRootOfEmoticon(which);

        //root = "Prefab/GUI/Emoticon/Good_1";

        var p2 = CamControl.CAMRTS.GetComponent<Camera>().WorldToScreenPoint(pos);

        var sp = (Image)Resources.Load(root, typeof(Image));
        var ins = GameObject.Instantiate(sp, p2, Quaternion.identity, _canvas);
        ins.transform.SetParent(_canvas);
    }

    static string ReturnIsGoodOrBad(string which)
    {
        if (_goodWords.Contains(which))
        {
            return "Good";
        }
        else if(_badWords.Contains(which))
        {
            return "Bad";
        }
        //other wise is somehting else 
        return which;
    }

    static string ReturnRootOfEmoticon(string which)
    {
        if (which == "Good")
        {
            var r = UMath.GiveRandom(1, 5);
            return "Prefab/GUI/Emoticon/Good_" + r;
        }
        else if (which == "Bad")
        {
            var r = UMath.GiveRandom(1, 4);
            return "Prefab/GUI/Emoticon/Bad_" + r;
        }
        else if (which == "Heart")
        {
            return "Prefab/GUI/Emoticon/Heart";
        }
        return "Prefab/GUI/Emoticon/Stork";

    }
}


