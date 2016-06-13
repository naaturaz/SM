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
        }
    }

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
        var xmls = Directory.GetFiles(_dataPath, "Town*.xml").ToList();
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
        Debug.Log("Rand Ini Pos:" + randIniPos);
        UVisHelp.CreateHelpers(randIniPos, Root.blueCubeBig);


        return bData;
    }


}

