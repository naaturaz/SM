using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

/// <summary>
/// Will load a random town and will place it in random spot
/// </summary>
internal class TownLoader
{
    private static SMe m = new SMe();

    private static int _loadedBuildCalls;
    static private int _buildCounts;
    private static bool _townLoaded;
    private static string _dataPath;
    private static int _initRegion;
    private static Vector3 _initialPosition;

    public static int InitRegion
    {
        get { return TownLoader._initRegion; }
        set { TownLoader._initRegion = value; }
    }

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
            Debug.Log("randTown:" + randTown);

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

    private static bool _isTemplate = false;

    public static bool IsTemplate
    {
        get { return _isTemplate; }
        set { _isTemplate = value; }
    }

    public static Vector3 InitialPosition
    {
        get
        {
            return _initialPosition;
        }

        set
        {
            _initialPosition = value;
        }
    }

    /// <summary>
    /// Gets random Town*.xml file
    /// </summary>
    /// <returns></returns>
    private static string GetRandomTownFile()
    {
        ///to  create Template towns.
        //new:
        //-make _isTemplate = true
        //-make isDev = true

        //old instruccions:
        //-uncomment 2 line below
        //-Also make sure in PErsonController the amt of people spawned will be zero
        //-Also make sure that the saved BuildingData.BuildingControllerData.TypeOfGame = H.None
        //other wise will give bugg changing btw Freewill and Traditional Mode
        //may need to create that type of game jst for this purpose of edit mannually with text editor.
        //If not then BuildingSaveLoad(472) will bugg
        //-To do that uncomment last line on NewGameWindow.ClickOnTypeOfGame()

        if (IsTemplate)
        {
            return "";
        }

        //game Difficulty is added for load 'Town4A.xml' for example
        var townName = "Town" + Program.MyScreen1.HoldDifficulty + "*.xml";

        var xmls = Directory.GetFiles(_dataPath, townName).ToList();
        return xmls[UMath.GiveRandom(0, xmls.Count)];
    }

    private static int prot;

    private static Vector3 GetRandomMapPos()
    {
        var randPos = MeshController.CrystalManager1.ReturnTownIniPos();
        return randPos;
    }

    /// <summary>
    /// Moves the builds positions to random
    /// </summary>
    /// <param name="bData"></param>
    /// <returns></returns>
    private static BuildingData ShiftToRandBuildsPos(BuildingData bData)
    {
        var randIniPos = GetRandomMapPos();
        InitialPosition = randIniPos;

        _initRegion = MeshController.CrystalManager1.ReturnMyRegion(randIniPos);

        //UVisHelp.CreateHelpers(randIniPos, Root.blueCubeBig);
        var townDim = GetTownDim(bData);

        //Debug.Log(prot + " TownLoaded Fit:" + fit);
        MoveAllBuildingsToNewSpot(bData, randIniPos, townDim);
        return bData;
    }

    private static Vector3 difference;

    /// <summary>
    /// Moves all the building to the new spot
    /// </summary>
    /// <param name="randIniPos"></param>
    /// <param name="bData"></param>
    private static void MoveAllBuildingsToNewSpot(BuildingData bData, Vector3 spot, List<Vector3> list)
    {
        difference = spot - UPoly.MiddlePoint(list);

        for (int i = 0; i < bData.All.Count; i++)
        {
            var newPos = bData.All[i].IniPos + difference;
            //its is important to get the newPOs in the closest vertex so is aling with new buildings to
            //spawn by user
            //newPos = m.Vertex.FindClosestVertex(newPos, m.AllVertexs.ToArray());
            bData.All[i].IniPos = newPos;
        }
    }

    private static List<Vector3> GetTownDim(BuildingData bData)
    {
        List<Vector3> allAnchors = new List<Vector3>();

        for (int i = 0; i < bData.All.Count; i++)
        {
            allAnchors.AddRange(bData.All[i].Anchors);
        }

        return Registro.FromALotOfVertexToPoly(allAnchors);
    }
}