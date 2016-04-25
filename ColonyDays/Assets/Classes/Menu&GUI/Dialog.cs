using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;


public class Dialog
{
    private static H _type;
    private static DialogGO _dialogGo;
    private static Transform _canvas;
    private static Vector3 _middleOfScreen;

    public static void Start()
    {
        
    }

    public static void OKCancel(H type)
    {
        RoutineSetUp();

        _type = type;
        _dialogGo = DialogGO.Create(Root.dialogOKCancel, _canvas, _middleOfScreen, _type);

    }

    /// <summary>
    /// Listen will be called directly from GUI button
    /// </summary>
    /// <param name="cmd"></param>
    public static void Listen(string action)
    {
        var sub = action.Substring(7);

        if (sub == "OKBtn")
        {
            if (_type == H.OverWrite)
            {
                DataController.SaveNow();
            }
            else if (_type == H.Delete)
            {
                DataController.DeleteNow();
            }
        }
        _dialogGo.Destroy();
    }



    private static void RoutineSetUp()
    {
        RefindCanvas();
        RedoMiddleOfScreen();    
    }

    /// <summary>
    /// if resolution is chagned 
    /// </summary>
    public static void RedoMiddleOfScreen()
    {
        _middleOfScreen = new Vector3(Screen.width/2, Screen.height/2, 0);
    }

    static void RefindCanvas()
    {
        var myForms = MonoBehaviour.FindObjectsOfType<MyForm>();

        for (int i = 0; i < myForms.Length; i++)
        {
            if (myForms[i].name.Contains("MainMenu"))
            {
                CanvasStep(myForms[i].gameObject);
                return;
            }
            if (myForms[i].name.Contains("MainGUI"))
            {
                CanvasStep(myForms[i].gameObject);
                return;
            }
        }
    }

    static void CanvasStep(GameObject go)
    {
        var canvGO = General.GetChildCalledOnThis("Canvas", go);

        if (canvGO != null)
        {
            _canvas = canvGO.transform;
        }
    }




    
}

