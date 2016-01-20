using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
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

        DataCollection.Save(Path.Combine(dataPath, "cameraSave.xml"));
    }

    public static List<RTSData> ReadXML()
    {


        var SaveInfoRTSCollection =
            DataContainer.Load(Path.Combine(dataPath, "cameraSave.xml"));

        List<RTSData> res = SaveInfoRTSCollection.SaveInfoRTSs;
        return res;
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
            DataContainer.Load(Path.Combine(dataPath, Program.gameScene.Terreno.name + ".xml"));

        SubMeshData res = null;
        if (loaded != null) {res = loaded.SubMeshData; }

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

        GameScene.ScreenPrint("XML last modified:"+lastWriteTimeXMLFile.ToString());
        GameScene.ScreenPrint("FBX last modified:"+lastWriteTimeFBXFile.ToString());

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

    public static SpawnedData ReadXMLSpawned()
    {
        var loaded =
            DataContainer.Load(Path.Combine(dataPath, Program.gameScene.Terreno.name + ".Spawned" + ".xml"));

        SpawnedData res = null;
        if (loaded != null) { res = loaded.SpawnedData; }

        return res;
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
        else res = LoadDefaultTown();

        return res;
    }

    private static BuildingData LoadDefaultTown()
    {
        string locPath = @"\Resources\Town";

        BuildingData res = new BuildingData();
        var difficulty = 0;
        if (difficulty == 0)
        {
            res = DataContainer.Load(Path.Combine(dataPath+locPath, "fourHouse.xml")).BuildingData;
        }
        return res;
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
}
