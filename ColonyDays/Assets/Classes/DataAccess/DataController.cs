using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using UnityEngine;

/// <summary>
/// The saves and Loads etc 
/// </summary>
public class DataController
{
    private static string _path;

    public static void Start()
    {
        DefinePath();
        CheckIfSugarMillFolderExists();
    }



    private static void DefinePath()
    {
        var a = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + @"\SugarMill";
        Debug.Log(a);
        _path = a;
    }

    private static void CheckIfSugarMillFolderExists()
    {
        var exist = Directory.Exists(_path);

        if (!exist)
        {
            Directory.CreateDirectory(_path);
        }
    }

    
    private static string savePath;
    /// <summary>
    /// Each save is a directory
    /// </summary>
    /// <param name="name"></param>
    public static void SaveGame(string name, bool quickSave = false)
    {
        savePath = _path + @"\" + name;
        var exist = Directory.Exists(savePath);
        var hasSpace = HasHDDSpace();
        
        if (!hasSpace)
        {
            //todo notify cant bz space
            Debug.Log("Not enough space on " + _path.ToCharArray()[0] + " Drive to save");
            Dialog.OKDialog(H.NotHDDSpace, _path.ToCharArray()[0].ToString());
            return;
        }

        //ask wants overWrite
        if (exist)
        {
            Dialog.OKCancelDialog(H.OverWrite);
        }
        else
        {
            //if is not quickSave then needs to call EscapeKey
            SaveNow(!quickSave);
            PlayerPrefs.SetString("Last_Saved", name);
        }
    }

    public static bool ThereIsALastSavedFile()
    {
        var tileNameSelected = PlayerPrefs.GetString("Last_Saved");
        return !string.IsNullOrEmpty(tileNameSelected);
    }




    /// <summary>
    /// Saving all XMLS
    /// 
    /// if 'callEscapeKey' will call : Program.InputMain.EscapeKey();
    /// should be called always unless is a quicksave
    /// </summary>
    public static void SaveNow(bool callEscapeKey)
    {
        Directory.CreateDirectory(savePath);
        XMLSerie.SaveGame(savePath); 

        //print("Resave Spawner and Buildings");
        Program.gameScene.controllerMain.TerraSpawnController.ReSaveData();
        BuildingPot.SaveLoad.Save();
        PersonPot.SaveLoad.Save();
        CamControl.CAMRTS.InputRts.SaveLastCamPos();

        savePath = "";

        if (callEscapeKey)
        {
            //so goes back to show the game 
            Program.InputMain.EscapeKey();
        }
    }

    private static bool HasHDDSpace()
    {
        //bytes
        ulong space = 100 * 1024 * 1024; //*1024*1024 makes it MegaBytes
        return DriveSpace.DriveFreeBytes(_path, out space);
    }


    public static bool ThereIsAtLeastAGameToLoad()
    {
        var saves = Directory.GetDirectories(SugarMillPath()).ToList();
        return saves.Count > 0;
    }


    internal static string SugarMillPath()
    {
        return _path;
    }

    internal static void LoadGame(string p)
    {
        PlayerPrefs.SetString("Last_Saved", p);

        XMLSerie.LoadGame(_path + @"\" + p);
        Program.RedoGame();
        
        //now action the continue button event
        BuildingPot.LoadBuildingsNow();

        Program.MyScreen1.DestroyCurrLoadLoading();
    }

    internal static void ContinueGame()
    {
        var name = PlayerPrefs.GetString("Last_Saved");
        LoadGame(name);
    }

    public static void DeleteGame(string p)
    {
        savePath = _path + @"\" + p;

        if (Directory.Exists(savePath))
        {
            Dialog.OKCancelDialog(H.Delete);
        }
    }

    public static void DeleteNow()
    {
        if (Directory.Exists(savePath))
        {
            System.IO.DirectoryInfo di = new DirectoryInfo(savePath);
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            Directory.Delete(savePath);
            Program.MyScreen1.DeleteSavedGameCallBack();
        }

        savePath = "";
    }


}



class DriveSpace
{
    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetDiskFreeSpaceEx(string lpDirectoryName,
    out ulong lpFreeBytesAvailable,
    out ulong lpTotalNumberOfBytes,
    out ulong lpTotalNumberOfFreeBytes);

    public static bool DriveFreeBytes(string folderName, out ulong freespace)
    {
        freespace = 0;
        if (string.IsNullOrEmpty(folderName))
        {
            throw new ArgumentNullException("folderName");
        }

        if (!folderName.EndsWith("\\"))
        {
            folderName += '\\';
        }

        ulong free = 0, dummy1 = 0, dummy2 = 0;

        if (GetDiskFreeSpaceEx(folderName, out free, out dummy1, out dummy2))
        {
            freespace = free;
            return true;
        }
        else
        {
            return false;
        }
    }
}