using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Security.Permissions;

//to be able to serializea an obj cant inherit from monobeaviour

//Tutorial:
//http://wiki.unity3d.com/index.php?title=Saving_and_Loading_Data:_XmlSerializer

public class XMLSerie
{

    //public static string dataPath = @"C:\Temp";
    //public static string dataPath = @"D:\Temp";

    public static string dataPath = Application.dataPath;


    public static void WriteXML(List<RTSData> listP)
    {
        DataContainer DataCollection = new DataContainer();
        DataCollection.SaveInfoRTSs = listP;

        DataCollection.Save(Path.Combine(Application.dataPath, "cameraSave.xml"));
    }

    public static List<RTSData> ReadXML()
    {
        var SaveInfoRTSCollection =
            DataContainer.Load(Path.Combine(Application.dataPath, "cameraSave.xml"));

        if (SaveInfoRTSCollection == null)
        {
            //no file saved
            return null;
        }

        List<RTSData> res = SaveInfoRTSCollection.SaveInfoRTSs;
        return res;
    }










    private static ProgramData _programData;
    public static ProgramData ProgramData1
    {
        get { return _programData; }
        set { _programData = value; }
    }
    public static ProgramData ReadXMLProgram()
    {
        var load =
            DataContainer.Load(Path.Combine(Application.dataPath, "program.xml"));

        Debug.Log("program path: " + Path.Combine(Application.dataPath, "program.xml"));

        if (load == null)
        {
            Debug.Log("programXML NUll ");
            //no file saved
            return null;
        }

        Debug.Log("programXML loaded:" + load.ProgramData.GameVersion);
        ProgramData res = load.ProgramData;
        return res;
    }

    public static void WriteXMLProgram(ProgramData program)
    {
        DataContainer DataCollection = new DataContainer();
        DataCollection.ProgramData = program;

        DataCollection.Save(Path.Combine(Application.dataPath, "program.xml"));
    }

    public static void WriteXMLMesh(SubMeshData subMesh)
    {
        DataContainer DataCollection = new DataContainer();
        DataCollection.SubMeshData = subMesh;

        DataCollection.Save(Path.Combine(dataPath, Program.gameScene.Terreno.name + ".xml"));
    }

    public static SubMeshData ReadXMLMesh()
    {
        var loaded =
            DataContainer.Load(Path.Combine(Application.dataPath, Program.gameScene.Terreno.name + ".xml"));

        Debug.Log("ReadXMLMesh: " + Path.Combine(Application.dataPath, Program.gameScene.Terreno.name + ".xml"));
        ProgramData1 = ReadXMLProgram();

        SubMeshData res = null;
        if (loaded != null) { res = loaded.SubMeshData; }

        ////if the FBX is newer than the pertaining XML will return null
        ////in that way ScanTerra() in MeshController.cs will fire up
        //if (IsFBXNewerThanXML())
        //{
        //    res = null;
        //}
        ////if ress is null I will delete the Spawneed.xml for this terrain
        //if (res == null)
        //{
        //    File.Delete(dataPath + "/" + Program.gameScene.Terreno.name + ".Spawned" + ".xml");
        //}

        return res;
    }

    //Will check if FBX file was written after the pertaining submesh XML
    //XML File, FBX File and Program.gameScene.Terreno.name must have exactly same name for this to work
    //FBX files must be in same location 
    static bool IsFBXNewerThanXML()
    {
        DateTime lastWriteTimeXMLFile = File.GetLastWriteTime(dataPath + "/" + Program.gameScene.Terreno.name + ".xml");
        DateTime lastWriteTimeFBXFile = File.GetLastWriteTime(dataPath + "/RawFiles/3d/Geometry/All_Terrains/" + Program.gameScene.Terreno.name + ".fbx");

        GameScene.ScreenPrint("XML last modified:" + lastWriteTimeXMLFile.ToString());
        GameScene.ScreenPrint("FBX last modified:" + lastWriteTimeFBXFile.ToString());

        if (lastWriteTimeFBXFile > lastWriteTimeXMLFile)
        {
            return true;
        }
        return false;
    }



    public static void WriteXMLSpawned(SpawnedData spawnedData)
    {
        DataContainer DataCollection = new DataContainer();
        DataCollection.SpawnedData = spawnedData;
        DataCollection.Save(Path.Combine(dataPath, Program.gameScene.Terreno.name + ".Spawned" + ".xml"));
    }

    /// <summary>
    /// read on caller of this if trying to create Terrain.Spawner file 
    /// </summary>
    /// <param name="defaultP"></param>
    /// <returns></returns>
    public static SpawnedData ReadXMLSpawned(bool defaultP = false)
    {
        DataContainer loaded = null;

        if (defaultP)
        {
            //which is a small Spawed terrain of bayAndMountain1River
            loaded =
                DataContainer.Load(Path.Combine(ReturnRightSpawnerRoot(), "DefaultLoad.xml"));
        }
        else
        {
            loaded =
                DataContainer.Load(Path.Combine(ReturnRightSpawnerRoot(), Program.gameScene.Terreno.name + ".Spawned" + ".xml"));
        }

        SpawnedData res = null;
        if (loaded != null) { res = loaded.SpawnedData; }
        return res;
    }

    static string ReturnRightSpawnerRoot()
    {
        //is a new game 
        if (dataPath == "")
        {
            return Application.dataPath;
        }
        return dataPath;
    }



    public static void WriteXMLBuilding(BuildingData buildingData)
    {
        DataContainer DataCollection = new DataContainer();
        DataCollection.BuildingData = buildingData;

        DataCollection.Save(Path.Combine(dataPath, "building.xml"));
    }

    public static BuildingData ReadXMLBuilding()
    {
        var loaded = DataContainer.Load(Path.Combine(dataPath, "building.xml"));

        BuildingData res = null;
        if (loaded != null)
        {
            res = loaded.BuildingData;
        }
        //if not saved town found will load Deffault town         
        else
        {
            res = LoadDefaultTown(); 
        }

        return res;
    }


    private static BuildingData LoadDefaultTown()
    {
        return TownLoader.LoadDefault();
    }






    public static void WriteXMLPerson(PersonData personData)
    {
        DataContainer DataCollection = new DataContainer();
        DataCollection.PersonData = personData;

        DataCollection.Save(Path.Combine(dataPath, "person.xml"));
    }

    public static PersonData ReadXMLPerson()
    {
        var loaded =
            DataContainer.Load(Path.Combine(dataPath, "person.xml"));

        PersonData res = null;
        if (loaded != null) { res = loaded.PersonData; }

        return res;
    }

    #region Main Menu

    public static void NewGame()
    {
        dataPath = "";
    }

    public static void SaveGame(string path)
    {
        //set  last saved dataPath, 

        //and dataPath to newName
        dataPath = path;

    }

    public static void LoadGame(string path)
    {
        //set  last saved dataPath

        //and dataPath to newName
        dataPath = path;
    }

    public static bool IsTutorial()
    {
        return dataPath == Application.dataPath + @"\" + "Tutorial";
    }

    #endregion

    #region Reports
    public static string WriteXMLFinalReport(FinalReport report)
    {
        DataContainer DataCollection = new DataContainer();
        DataCollection.FinalReport = report;

        var root = Path.Combine(Application.dataPath, "Final.txt");
        DataCollection.Save(root);

        return root;
    }
    #endregion








    #region Mod
    //Mods

    public static PeopleModData ReadXMLPeopleModData()
    {
        string path = Application.dataPath + "/Mod/MyMod/People.xml";

        var loaded = DataContainer.Load(path);



        PeopleModData res = null;
        if (loaded != null) { res = loaded.PeopleModData; }

        return res;
    }

    #endregion
}
