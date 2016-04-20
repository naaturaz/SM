using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

    /// <summary>
    /// Each save is a directory
    /// </summary>
    /// <param name="name"></param>
    public static void SaveGame(string name)
    {
        var locPath = _path + @"\" + name;
        var exist = Directory.Exists(locPath);

        //ask wants overWrite
        if (exist)
        {

        }
        else
        {
            Directory.CreateDirectory(locPath);
            XMLSerie.SaveGame(locPath);  
            SaveNow();
        }
    }

    /// <summary>
    /// Saving all XMLS
    /// </summary>
    static void SaveNow()
    {
        //print("Resave Spawner and Buildings");
        Program.gameScene.controllerMain.TerraSpawnController.ReSaveData();
        BuildingPot.SaveLoad.Save();
        PersonPot.SaveLoad.Save();
        CamControl.CAMRTS.InputRts.SaveLastCamPos();
    }

    internal static string SugarMillPath()
    {
        return _path;
    }
}
