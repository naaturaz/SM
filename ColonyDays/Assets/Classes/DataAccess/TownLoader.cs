using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// Will load a random town and will place it in random spot 
/// </summary>
class TownLoader
{
    static SMe m = new SMe();

    private static int _loadedBuildCalls;
    static private int _buildCounts;
    static bool _townLoaded;
    static string _dataPath;
    public static bool TownLoaded
    {
        get { return _townLoaded; }
        set { _townLoaded = value; }
    }  
    
    /// <summary>
    /// Called from builing.cs when a building is loaded 
    /// </summary>
    public static void NewBuildingLoaded()
    {
        _loadedBuildCalls++;
        if (_buildCounts == _loadedBuildCalls)
        {
            Debug.Log("TownLoaded = false");
            TownLoaded = false;
            _loadedBuildCalls = 0;
            
            //so the DimOnMap works
            BuildingPot.Control.Registro.RedoDimAndResaveAllBuildings();
        }
    }

    /// <summary>
    /// Returns the Data of a Random town for initial game 
    /// </summary>
    /// <returns></returns>
    public static BuildingData LoadDefault()
    {
        _dataPath = Application.dataPath;

        BuildingData res = null;
        var difficulty = 0;
        if (difficulty == 0)
        {
            var randTown = GetRandomTownFile();
            Debug.Log("randTown:"+randTown);

            var file = DataContainer.Load(randTown);
            if (file != null)
            {
                Debug.Log("TownLoaded = true");
                _buildCounts = file.BuildingData.All.Count;
                TownLoaded = true;

                res = ShiftToRandBuildsPos(file.BuildingData);
            }
        }
        return res;
    }

    /// <summary>
    /// Gets random Town*.xml file
    /// </summary>
    /// <returns></returns>
    static string GetRandomTownFile()
    {
        //todo read below if needed
        //uncomment line below to create Template towns. Also make sure in PErsonController
        //the amt of people spawned will be zero
        //return "";
       
        //game Difficulty is added for load 'Town4A.xml' for example
        var townName = "Town" + Program.MyScreen1.Difficulty + "*.xml";
         
        var xmls = Directory.GetFiles(_dataPath, townName).ToList();
        return xmls[UMath.GiveRandom(0, xmls.Count)];
    }

    private static int prot;
    static Vector3 GetRandomMapPos()
    {
        var randPos = m.AllVertexs[UMath.GiveRandom(0, m.AllVertexs.Count)];
        if (Building.IsVector3OnTheFloor(randPos, m.SubMesh.mostCommonYValue))
        {
            return randPos;
        }

        if (prot > 1000)
        {
            throw new Exception("GetRandomMapPos() over 1000 times. could not find spot to starts Loading town");
        }
        prot++;
        return GetRandomMapPos();
    }

    /// <summary>
    /// Moves the builds positions to random
    /// </summary>
    /// <param name="bData"></param>
    /// <returns></returns>
    static BuildingData ShiftToRandBuildsPos(BuildingData bData)
    {
        var randIniPos = GetRandomMapPos();
        //UVisHelp.CreateHelpers(randIniPos, Root.blueCubeBig);
        var townDim = GetTownDim(bData);
        var fit = DoesSpotFitTown(randIniPos, townDim);

        if (!fit)
        {
            return ShiftToRandBuildsPos(bData);
        }

        Debug.Log(prot + " TownLoaded Fit:" + fit);
        MoveAllBuildingsToNewSpot(bData);
        return bData;
    }

    /// <summary>
    /// Moves all the building to the new spot 
    /// </summary>
    /// <param name="randIniPos"></param>
    /// <param name="bData"></param>
    private static void MoveAllBuildingsToNewSpot(BuildingData bData)
    {
        for (int i = 0; i < bData.All.Count; i++)
        {
            var newPos = bData.All[i].IniPos + difference;
            //its is important to get the newPOs in the closest vertex so is aling with new buildings to
            //spawn by user 
            newPos = m.Vertex.FindClosestVertex(newPos, m.AllVertexs.ToArray());
            bData.All[i].IniPos = newPos;
        }
    }

    /// <summary>
    /// Will say if each corner of the town falls in ground. otherwise ret false
    /// Will also make sure all points are in the terrain limit
    /// and also they all are in the same region
    /// </summary>
    /// <param name="spot"></param>
    /// <param name="townDim"></param>
    /// <returns></returns>
    static bool DoesSpotFitTown(Vector3 spot, List<Vector3> townDim)
    {
        var movedTown = MoveTownToSpot(spot, townDim);
        //UVisHelp.CreateHelpers(movedTown, Root.yellowCube);
        var region = MeshController.CrystalManager1.ReturnMyRegion(movedTown[0]);
        
        for (int i = 0; i < movedTown.Count; i++)
        {
            //throws ray to check where is in real ground
            var inRealGroundVal = m.Vertex.BuildVertexWithXandZ(movedTown[i].x, movedTown[i].z);
            var inFloor = Building.IsVector3OnTheFloor(inRealGroundVal, m.IniTerr.MathCenter.y);
            
            //will check if the point is on terrain. Also manipulates '-50' the size of terrain and 
            //makes it a bit smaller so they town loaded is not right no the edge of terrain
            var inTerrain = UTerra.IsOnTerrainManipulateTerrainSize(movedTown[i], -1.2f);//-1

            //so all fall into the same region 
            var regionThisIndex = MeshController.CrystalManager1.ReturnMyRegion(movedTown[i]);
            var isSameRegion = region == regionThisIndex;

            //if one is not in floor then is false 
            if (!inFloor || !inTerrain || !isSameRegion)
            {
                return false;
            }
        }
        return true;
    }


    private static Vector3 difference;
    /// <summary>
    /// It moves 'list' to the spot
    /// </summary>
    /// <param name="spot"></param>
    /// <param name="list"></param>
    /// <returns></returns>
    private static List<Vector3> MoveTownToSpot(Vector3 spot, List<Vector3> list)
    {
        difference = spot - list[0];
        List<Vector3> movedTown = new List<Vector3>();

        for (int i = 0; i < list.Count; i++)
        {
            movedTown.Add(list[i] + difference);
        }
        return movedTown;
    }

    static List<Vector3> GetTownDim(BuildingData bData)
    {
        List<Vector3> allAnchors = new List<Vector3>();

        for (int i = 0; i < bData.All.Count; i++)
        {
            allAnchors.AddRange(bData.All[i].Anchors);
        }

        return Registro.FromALotOfVertexToPoly(allAnchors);
    }



 
}

