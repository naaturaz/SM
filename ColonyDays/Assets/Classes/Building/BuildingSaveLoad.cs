using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/*The bulidings new RegFiles are created on Registro.AddBuildToAll()
 */

public class BuildingSaveLoad : BuildingPot
{
    private int counter;//the counter of buildigns being recreated
    private BuildingData _buildingData;
    private bool _isToRecreateNow;//if true is Loading Buildings 

    public BuildingData BuildingData
    {
        get { return _buildingData; }
        set { _buildingData = value; }
    }

    public bool IsToRecreateNow
    {
        get { return _isToRecreateNow; }
        set { _isToRecreateNow = value; }
    }

    public void Save()
    {
        BuildingControllerData local = PullAllVarFromBuildingController();

        Control.Registro.ResaveAllBuildings();
        BuildingData = new BuildingData(Control.Registro.AllRegFile, local);
        XMLSerie.WriteXMLBuilding(BuildingData);
    }

    public void Load()
    {
        BuildingData = XMLSerie.ReadXMLBuilding();
        if (BuildingData != null)
        {
            counter = 0;//in case something was loaded first 
            _isToRecreateNow = true;
            Control.Registro.AllRegFile = BuildingData.All;
        }
        //need to be called here in case he scene dostn have any building at all
        else CreatePersonPot();
    }



    /// <summary>
    /// Recreate all the buildings that are in the BuildingData.All 
    /// which were read it from  XMLSerie.ReadXMLBuilding()
    /// </summary>
    public void LoadAllBuildings()
    {
        if (counter < BuildingData.All.Count)
        {
            CreateAllBuildings(BuildingData.All[counter]);
            counter++;
        }
        else
        {
            _isToRecreateNow = false;
            //needed here so the last obj loaded doesnt Destroy when creating a new Building
            //in the loaded scene
            Control.CurrentSpawnBuild = null;
            CreatePersonPot();

            //the pos where cam was last saved by system 

        }
    }

    /// <summary>
    /// Game is fully loaded will:
    /// </summary>
    void CreatePersonPot()
    {
        BuildingPot.Control.Registro.IsFullyLoaded = true;

        //The person pot creating is called when we loaded all buildings 
        Program.InputMain.CreatePersonPot();

        Program.gameScene.GameController1.NotificationsManagerInit();

        CamControl.CAMRTS.ReportAudioNow();

        Program.gameScene.BatchInitial();

    }

    /// <summary>
    /// Routine that create all the building based on the category
    /// that regFile brings 
    /// </summary>
    /// <param name="regFile">The file that has all the information to load a new building</param>
    void CreateAllBuildings(RegFile regFile)
    {
        if (regFile.Category == Ca.Structure || regFile.Category == Ca.Shore)
        {
            CreateStructure(regFile);
        }
        else if (regFile.Category == Ca.Way)
        {
            CreateWay(regFile);
        }
        else if(regFile.Category == Ca.DraggableSquare)
        {
            CreateDraggableSquare(regFile);
        }
    }

    /// <summary>
    /// Will create a draggable category building (Farm, StockPile, Farm)
    /// </summary>
    void CreateDraggableSquare(RegFile regFile)
    {
        Control.CurrentSpawnBuild = Way.CreateWayObj(Root.farm, regFile.IniPos,
            previewObjRoot: Root.previewTrail, hType: regFile.HType, isLoadingFromFile: true
            , container: Program.BuildsContainer.transform);

        Control.CurrentSpawnBuild.MyId = regFile.MyId;
        Control.CurrentSpawnBuild.transform.name = regFile.MyId;

        DragSquare f = (DragSquare) Control.CurrentSpawnBuild;
        f.PlanesSoil = CreatePlanes(regFile.PlaneOnAirPos, f.transform, regFile.TileScale, regFile.MaterialKey, Control.CurrentSpawnBuild);



        f.AddBoxCollider(regFile);
        f.PositionFixed = true;
        f.PeopleDict = regFile.PeopleDict;

        f.transform.position = regFile.IniPos;


        Program.gameScene.BatchAdd(f);

        Control.Registro.Farms.Add(regFile.MyId, Control.CurrentSpawnBuild as DragSquare);
        Control.Registro.AllBuilding.Add(regFile.MyId, Control.CurrentSpawnBuild);
    }

    /// <summary>
    /// Creates the plane of pos lineanly
    /// </summary>
    List<CreatePlane> CreatePlanes(List<Vector3> pos, Transform containerP, Vector3 scaleP, string materialKey, Building spawner)
    {
        List<CreatePlane> res = new List<CreatePlane>();
        for (int i = 0; i < pos.Count; i++)
        {
            if (spawner.HType == H.Road)
            {
                res.Add(CreatePlane.CreatePlanSmartTile(spawner,Root.createPlane, Root.RetMaterialRoot(materialKey),
                 pos[i], scale: scaleP, container: containerP, isLoadingFromFile:true));
            }
            else
            {
                res.Add(CreatePlane.CreatePlan(Root.createPlane, Root.RetMaterialRoot(materialKey),
                 pos[i], scale: scaleP, container: containerP));
            }


        }
        return res;
    }

    string FindRootForStructure(RegFile regFile)
    {
        if (!string.IsNullOrEmpty(regFile.Root))
        {
            return regFile.Root;
        }
        return Root.RetBuildingRoot(regFile.HType);
    }

    /// <summary>
    /// Creates the the new building, category: structure
    /// </summary>
    void CreateStructure(RegFile regFile)
    {
        Control.CurrentSpawnBuild = Building.CreateBuild(FindRootForStructure(regFile), regFile.IniPos, 
            regFile.HType, isLoadingFromFile: true, materialKey: regFile.MaterialKey
            , container: Program.BuildsContainer.transform);
            
        //this is part of the Reduce Draw calls experiment
        //Material n = Batcher.BuildsStatic[H.Tavern + "." + Ma.matBuildBase];
        //n.name = "BaseOriginalRenamed";
        //Control.CurrentSpawnBuild.Geometry.renderer.sharedMaterial = n;

        Control.CurrentSpawnBuild.MyId = regFile.MyId;
        Control.CurrentSpawnBuild.transform.name = regFile.MyId;

        Structure s = Control.CurrentSpawnBuild as Structure;
        s.StartingStage = regFile.StartingStage;
        s.HType = regFile.HType;
        s.Category = regFile.Category;
        s.RotationFacerIndex = regFile.RotationFacerIndex;
        s.PositionFixed = true;
        
        s.Inventory = regFile.Inventory;

        s.Instruction = regFile.Instruction;
        s.PeopleDict = regFile.PeopleDict;
        s.BookedHome1 = regFile.BookedHome1;


        s.Dispatch1 = regFile.Dispatch1;
        s.BuildersManager1 = regFile.BuildersManager1;
        s.Families = regFile.Familes;
        s.LandZone1 = regFile.LandZone1;
        s.DollarsPay = regFile.DollarsPay;

        s.Dock1 = regFile.Dock1;
        s.PlantSave1 = regFile.PlantSave1;
        s.CurrentProd = regFile.CurrentProd;
        s.Anchors = regFile.Anchors.ToList();

        s.ProductionReport = regFile.ProductionReport;
        //s.MaxPeople = regFile.MaxPeople;
        s.Decoration1 = regFile.Decoration;
        s.Name1 = regFile.Name;

        Program.gameScene.BatchAdd(s);
        Control.Registro.Structures.Add(s.MyId, Control.CurrentSpawnBuild as Structure);
        Control.Registro.AllBuilding.Add(s.MyId, Control.CurrentSpawnBuild);
    }

    private Trail trail;
    /// <summary>
    /// Creates a new way object 
    /// </summary>
    void CreateWay(RegFile regFile)
    {
        if (regFile.HType == H.Trail)
        {
            trail = (Trail)Way.CreateWayObj(Root.trail, regFile.IniPos,
                previewObjRoot: Root.previewTrail, hType: H.Trail, isLoadingFromFile: true
                , container: Program.BuildsContainer.transform);
        }
        else if (regFile.HType == H.BridgeTrail)
        {
             trail = (Bridge)
                Way.CreateWayObj(Root.bridge, regFile.IniPos, previewObjRoot: Root.previewTrail,
                hType: H.BridgeTrail, isLoadingFromFile: true
                , container: Program.BuildsContainer.transform);
        }
        else if (regFile.HType == H.BridgeRoad)
        {
            trail = (Bridge)
                Way.CreateWayObj(Root.bridge, regFile.IniPos, previewObjRoot: Root.previewRoad,
                hType: H.BridgeRoad, wideSquare: 5, radius: 5f, planeScale: 0.11f, maxStepsWay: 20,
                isLoadingFromFile: true
                , container: Program.BuildsContainer.transform);
        }

        trail.CurrentLoop = H.Done;
        trail.FinishPlacingMode(H.Done);

        if (regFile.HType.ToString().Contains(H.Bridge.ToString()))
        {
            trail.Pieces = CreateBridgePartList(regFile, trail.transform);
        }

        trail.name = regFile.MyId;
        trail.MyId = regFile.MyId;
        trail.PeopleDict = regFile.PeopleDict;
        trail.LandZone1 = regFile.LandZone1;
        trail.Instruction = regFile.Instruction;
        trail.MaterialKey = regFile.MaterialKey;
        trail.Anchors = regFile.Anchors.ToList();

        //if (trail.name.Contains("Bridge"))
        //{
        //    UVisHelp.CreateHelpers(trail.Anchors, Root.blueCube);
            
        //}

        trail.StartingStage = regFile.StartingStage;

        //if is not a bridge
        if (!regFile.HType.ToString().Contains(H.Bridge.ToString()))
        {
            trail.PlanesListVertic = CreatePlanesVertAndHor(regFile, H.Vertic, trail.transform, trail);
            trail.PlanesListHor = CreatePlanesVertAndHor(regFile, H.Horiz, trail.transform, trail);
        }
        else
        {
            trail = CreateBridgePlanes(trail, regFile);
            trail.AddBoxCollider(regFile.Min, regFile.Max);
            //trail.LandZoningBridge();
        }

        Program.gameScene.BatchAdd(trail);
        
        Control.CurrentSpawnBuild = trail;
        Control.Registro.Ways.Add(trail.MyId, Control.CurrentSpawnBuild as Way);
        Control.Registro.AllBuilding.Add(trail.MyId, Control.CurrentSpawnBuild);
        Control.CurrentSpawnBuild = null;
    }

    /// <summary>
    /// Creates the planes of a bridge, The first and last one
    /// </summary>
    Trail CreateBridgePlanes(Trail current, RegFile regFile)
    {
        if (regFile.DominantSide == H.Vertic)
        {
            current.PlanesListVertic.Add(CreatePlane.CreatePlan(Root.createPlane,
                Root.RetMaterialRoot(regFile.MaterialKey),
                regFile.TilePosVert[0], scale: regFile.TileScale, container: current.transform));

            current.PlanesListVertic.Add(CreatePlane.CreatePlan(Root.createPlane,
                Root.RetMaterialRoot(regFile.MaterialKey),
                regFile.TilePosVert[regFile.TilePosVert.Count - 1], scale: regFile.TileScale,
                container: current.transform));
        }
        else if (regFile.DominantSide == H.Horiz)
        {
            current.PlanesListHor.Add(CreatePlane.CreatePlan(Root.createPlane,
                Root.RetMaterialRoot(regFile.MaterialKey),
                regFile.TilePosHor[0], scale: regFile.TileScale, container: current.transform));

            current.PlanesListHor.Add(CreatePlane.CreatePlan(Root.createPlane,
                Root.RetMaterialRoot(regFile.MaterialKey),
                regFile.TilePosHor[regFile.TilePosHor.Count - 1], scale: regFile.TileScale,
                container: current.transform));
        }
        return current;
    }

    /// <summary>
    /// Creates the  parts of a bridge visualiiy
    /// </summary>
    /// <param name="iniPos"></param>
    List<StructureParent> CreateBridgePartList(RegFile regFile, Transform containerP)
    {
        List<StructureParent> res = new List<StructureParent>();
        //below brdige instance is a dummy instance. Only useed to instantiate parts 
        Bridge b = new Bridge(regFile.HType, regFile.StartingStage);
        res = b.CreatePartListOnAir(regFile.PlaneOnAirPos, regFile.PartsOnAir, containerP, regFile.DominantSide);
        res.AddRange(b.CreatePartListOnGround(regFile.PlanesOnSoil, regFile.PartsOnSoil, containerP, regFile.DominantSide));
        return res;
    }

    //this cretes planes vertically and horizontally
    List<CreatePlane> CreatePlanesVertAndHor(RegFile regFile, H which, Transform containerP, Trail trail)
    {
        List<CreatePlane> res = new List<CreatePlane>();
        if (which == H.Vertic)
        {
            for (int i = 0; i < regFile.TilePosVert.Count; i++)
            {
                res.Add(CreatePlane.CreatePlan(Root.createPlane, Root.RetMaterialRoot(regFile.MaterialKey),
                    regFile.TilePosVert[i], scale: regFile.TileScale, container: containerP));

                //to Refine
                //Trail.AddToCrystals(H.PlanesVertic, i, regFile.TilePosVert[i], regFile.TilePosVert.Count,
                //    regFile.TilePosHor.Count, trail);
            }
        }
        else if (which == H.Horiz)
        {
            for (int i = 0; i < regFile.TilePosHor.Count; i++)
            {
                res.Add(CreatePlane.CreatePlan(Root.createPlane, Root.RetMaterialRoot(regFile.MaterialKey),
                    regFile.TilePosHor[i], scale: regFile.TileScale, container: containerP));

                //to Refine
                //Trail.AddToCrystals(H.PlanesHor, i, regFile.TilePosHor[i], regFile.TilePosVert.Count,
                //    regFile.TilePosHor.Count, trail);
            }
        }
        return res;
    }

    // Use this for initialization
	void Start (){}
	
	// Update is called once per frame
	public void Update () 
    {
	    if (_isToRecreateNow)
	    {
	        LoadBuildingController();
	        LoadAllBuildings();
	    }
	}

    #region Save Load 
    //Save Load Building Controller .cs

    //For Saving
    BuildingControllerData PullAllVarFromBuildingController()
    {
        BuildingControllerData res = new BuildingControllerData();

        res._foodSources = Control.FoodSources;
        res._workOpenPos = Control.WorkOpenPos;
        res._housesWithSpace = Control.HousesWithSpace;
        res._religiousBuilds = Control.ReligiousBuilds;
        res._chillBuilds = Control.ChillBuilds;
        res._wayBuilds = Control.WayBuilds;

        res._isfoodSourceChange = Control.IsfoodSourceChange;
        res._isWorkChanged = Control.AreNewWorkPos;
        res._isHouseSpaceChanged = Control.IsNewHouseSpace;
        res._isReligionChanged = Control.IsNewReligion;
        res.IsChillChanged = Control.IsNewChill;
        res.DispatchManager1 = Control.DispatchManager1;

        res.DockManager1 = Control.DockManager1;
        res.ShipManager1 = Control.ShipManager1;

        res._GameTime = Program.gameScene.GameTime1;
        res.GameTimePeople = Program.gameScene.GameTimePeople;
        res._GameController = Program.gameScene.GameController1;

        res.TypeOfGame = Program.TypeOfGame;

        return res;
    }

    //for Loading the Building Controller
    private void LoadBuildingController()
    {
        Control.FoodSources = BuildingData.BuildingControllerData._foodSources;
        Control.WorkOpenPos = BuildingData.BuildingControllerData._workOpenPos;
        Control.HousesWithSpace = BuildingData.BuildingControllerData._housesWithSpace;
        Control.ReligiousBuilds = BuildingData.BuildingControllerData._religiousBuilds;
        Control.ChillBuilds = BuildingData.BuildingControllerData._chillBuilds;
        Control.WayBuilds = BuildingData.BuildingControllerData._wayBuilds;

        Control.IsfoodSourceChange = BuildingData.BuildingControllerData._isfoodSourceChange;
        Control.AreNewWorkPos = BuildingData.BuildingControllerData._isWorkChanged;
        Control.IsNewHouseSpace = BuildingData.BuildingControllerData._isHouseSpaceChanged;
        Control.IsNewReligion = BuildingData.BuildingControllerData._isReligionChanged;
        Control.IsNewChill = BuildingData.BuildingControllerData.IsChillChanged;

        if (BuildingData.BuildingControllerData.DispatchManager1 != null)
        {
            Control.DispatchManager1 = BuildingData.BuildingControllerData.DispatchManager1;    
        }

        if (BuildingData.BuildingControllerData._GameTime != null)
        {
            Program.gameScene.GameTime1 = BuildingData.BuildingControllerData._GameTime;    
        }
        if (BuildingData.BuildingControllerData.GameTimePeople != null)
        {
            Program.gameScene.GameTimePeople = BuildingData.BuildingControllerData.GameTimePeople;    
        }

        if (BuildingData.BuildingControllerData._GameController != null)
        {
            Program.gameScene.GameController1 = BuildingData.BuildingControllerData._GameController;
        }


        if (BuildingData.BuildingControllerData.DockManager1 != null)
        {
            Control.DockManager1 = BuildingData.BuildingControllerData.DockManager1;
        }
        if (BuildingData.BuildingControllerData.ShipManager1 != null)
        {
            Control.ShipManager1 = BuildingData.BuildingControllerData.ShipManager1;
        }
        if (Control.ShipManager1!=null)
        {
            Control.ShipManager1.MarkToLoadShips();
        }

        var type = BuildingData.BuildingControllerData.TypeOfGame;
        if (type != H.None)//loaded default town 
        {
            Program.TypeOfGame = type;
        }


    }
    #endregion

    /// <summary>
    /// Will say if that building was loaded from File 
    /// </summary>
    /// <param name="build"></param>
    /// <returns></returns>
    internal bool IsWasLoaded(Building build)
    {
        var found = _buildingData.All.Find(a => a.MyId == build.MyId);
        return found != null;
    }
}
