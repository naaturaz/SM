using System;
using System.Collections.Generic;
using System.IO;
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

    public static void OKCancelDialog(H type)
    {
        RoutineSetUp();

        _type = type;
        _dialogGo = DialogGO.Create(Root.dialogOKCancel, _canvas, _middleOfScreen, _type);
    }  
    
    public static void OKDialog(H type, string str1 = "")
    {
        RoutineSetUp();

        _type = type;
        _dialogGo = DialogGO.Create(Root.dialogOK, _canvas, _middleOfScreen, _type, str1);
    }   
    
    public static void InputFormDialog(H type, string str1 = "")
    {
        RoutineSetUp();

        _type = type;
        _dialogGo = DialogGO.Create(Root.inputFormDialog, _canvas, _middleOfScreen, _type, str1);
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
                DataController.SaveNow(true);
            }
            else if (_type == H.Delete)
            {
                DataController.DeleteNow();
            } 
            else if (_type == H.GameOverPirate)
            {
                Program.InputMain.EscapeKey();
            } 
            else if (_type == H.BuyRegion)
            {
                MeshController.BuyRegionManager1.CurrentRegionBuy();               
            }
            else if (_type == H.Feedback || _type == H.BugReport)
            {
                CreateFile(_type+"");
            }

        }

        DestroyCurrDialog();
    }


    static void DestroyCurrDialog()
    {
        //when calling a BuyRegion it will try to delete current dialog.
        //thi is to prevent NullRef exp if is none
        if (_dialogGo != null)
        {
            _dialogGo.Destroy();
        }
    }





    private static void RoutineSetUp()
    {
        DestroyCurrDialog();

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









    static void CreateFile(string type)
    {
        var nameFile = type + "-" + GameScene.TimeStamp() + ".sm";

        var path = Application.dataPath + "/" + nameFile;
        Debug.Log(path);
        File.WriteAllText(path, FileHeader()  + _dialogGo.InputText.text);
    }

    private static string FileHeader()
    {
        return "Current Version: " + GameScene.VersionLoaded() 
            
            
            + "\n" +
            "___________________________________________" +
            "\n\n";
    }




    public static bool IsActive()
    {
        return _dialogGo != null;
    }
}

