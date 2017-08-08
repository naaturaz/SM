using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Random = UnityEngine.Random;
using UnityEngine.AI;

public class Building : Hoverable, Iinfo
{
    #region Fields and Prop

    ConstructionProgress _constructionProgress;

    bool _wasGreenlit;
    /// <summary>
    /// if I was greenlit on the BuildersManager
    /// </summary>
    public bool WasGreenlit
    {
        get { return _wasGreenlit; }
        set { _wasGreenlit = value; }
    }

    /// <summary>
    /// The root of a building 
    /// </summary>
    public string RootBuilding { get; set; }



    //Rotate Building /// 0 is up, 1 is right, 2 is down, 3 is left
    private int _rotationFacerIndex;

    protected float _maxDiffAllowOnTerrain = 0.1f;//for all buildings
    protected float _maxDiffAllowOnTerrainForARoad = 0.01f;//for all buildings

    private Vector3 _min;//this is the Min point on the bound
    private Vector3 _max;//this is the Max point on the bound
    private List<Vector3> _bounds = new List<Vector3>();
    private List<Vector3> _anchors = new List<Vector3>();

    protected bool _isEven; //is on a even terrain
    private bool _isColliding; //is colliding with another building 
    protected bool _isGoodWaterHeight;//indicates if this building is ok with water height
    private bool _isBuildOk; //created so we dont have to execute CheckIfEvenTerrainAndNotColl()

    public Vector3 ClosestSubMeshVert = new Vector3();
    public Vector3 ClosestVertOld = new Vector3();

    //none of them are: except for the Way.cs, 
    //have to be set to true on start() of the obj 
    protected bool _isFakeObj;
   
    protected bool _isMarine;//all marine structures should have this marked as true(dock, bridge, etc)
    protected float _minHeightToSpawn;

    protected bool _isOrderToDestroy;//will indicate childs that need to destryo the obj
    protected General _arrow;

    private bool _isLoadingFromFile;//this indicates if the creating of a obj is being call from a Load File or not
    string _materialKey;//the key of the material of an building










    //the zone this building landed. The bridges have two landing zones and are not kept here
    private List<VectorLand> _landZone = new List<VectorLand>();
    /// <summary>
    /// Geograpihcally is in the spawnpoint of a building . If is a Bridge will have two
    /// each in each Bottom Middile
    /// 
    /// IMPORTANT: IF LANDZONES ARE NOT SET THE ROUTING SYSTEM WONT WORK
    /// </summary>
    public List<VectorLand> LandZone1
    {
        get { return _landZone; }
        set { _landZone = value; }
    }

    protected H _startingStage = H.None;//this is is  used to load structure class from file
    public H StartingStage
    {
        get { return _startingStage; }
        set { _startingStage = value; }
    }

    public string MaterialKey
    {
        get { return _materialKey; }
        set { _materialKey = value; }
    }

    public virtual bool IsEven
    {
        //if is giving u null or error and send u here
        //"Make sure u are calling the Update() of this class");}
        get { return _isEven; }
    }

    /// <summary>
    /// Bounds of the building 
    /// </summary>
    public virtual List<Vector3> Bounds
    {
        //is only defined when we check if is even. So if that never was checked this wont have a good value
        get
        {
            return _bounds;
        }
        set { _bounds = value; }
    }

    public int RotationFacerIndex
    {
        get { return _rotationFacerIndex; }
        set { _rotationFacerIndex = value; }
    }

    public bool IsBuildOk
    {
        get { return _isBuildOk; }
    }

    public bool IsColliding
    {
        get { return _isColliding; }
    }

    public bool IsLoadingFromFile
    {
        get { return _isLoadingFromFile; }
        set { _isLoadingFromFile = value; }
    }

    public List<Vector3> Anchors
    {
        get
        {
            if (MyId.Contains("Bridge"))
            {
                var a = 1;
            }
            
            return _anchors;
        }
        set
        {
            if (MyId.Contains("Bridge"))
            {
                var a = 1;
            }

            _anchors = value;

           
            
        }
    }

    public Vector3 Max
    {
        get { return _max; }
        set { _max = value; }
    }

    public Vector3 Min
    {
        get { return _min; }
        set { _min = value; }
    }



    #endregion

    internal string NameBuilding()
    {
        if (string.IsNullOrEmpty(Name))
        {
            Name = Languages.ReturnString( HType + "");
        }

        return Name;
    }


    //create method
    static public Building CreateBuild(string root, Vector3 origen, H hType, string name = "", Transform container = null,
        bool isLoadingFromFile = false, string materialKey = "")
    {
        WAKEUP = true;
        Building obj = null;
        obj = (Building)Resources.Load(root, typeof(Building));
        obj = (Building)Instantiate(obj, origen, Quaternion.identity);
        obj.HType = hType;
        obj.transform.name = obj.MyId = obj.Rename(obj.transform.name, obj.Id, obj.HType, name);
        obj.IsLoadingFromFile = isLoadingFromFile;

        obj.ClosestSubMeshVert = origen;
        if (name != "") { obj.name = name; }
        if (container != null){obj.transform.SetParent(container);}

        if (materialKey == "")
        {materialKey = hType + "." + Ma.matBuildBase;}

        obj.MaterialKey = materialKey;

        //obj.AssignToAllGeometryAsSharedMat(obj.gameObject, materialKey);

        return obj;
    }



    /// <summary>
    /// Checks if Terrain below the build _isEven or _isColliding, and is tall enough
    /// </summary>
    /// <returns>True if terrain is even, not colliding and tall enough</returns>
    public virtual bool CheckEvenTerraCollWater()
    {
        //if is fake obj will be terminated 
        if (_isFakeObj || HType == H.Dummy) { return false; }

        _isEven = CheckIfIsEvenRoutine();
        _isColliding = CheckIfColliding();//must be check after CheckIfIsEvenRoutine()
        _isGoodWaterHeight = checkWaterHeight();

        if (_isGoodWaterHeight)
        {
            _isGoodWaterHeight = IsOnTheFloor(Anchors);
        }

        var isScaledOnFloor = false;
        if (_isGoodWaterHeight)
        {
            isScaledOnFloor = IsScaledAnchorsOnFloor();
        }

        if (!IsThisADoubleBoundedStructure())
        {
            NotifyBuildingProblem(isScaledOnFloor);
            
        }
        
        return _isEven && !_isColliding && _isGoodWaterHeight && isScaledOnFloor 
            && AreAnchorsOnUnlockRegions() //&& IfIsLampIsFarEnough()

            ;
    }


   

    ///// <summary>
    ///// If building is a Lamp must be far enough from other lamps 
    ///// </summary>
    ///// <returns></returns>
    //private bool IfIsLampIsFarEnough()
    //{
    //    if (HType != H.StandLamp)
    //    {
    //        return true;
    //    }

    //    //return BuildingPot.Control.Registro.IsFarEnoughFromLights();

    //}

    /// <summary>
    /// Will notify why current buliding cant be placed here 
    /// </summary>
    /// <param name="isScaledOnFloor"></param>
    private void NotifyBuildingProblem(bool isScaledOnFloor)
    {
        if (!Program.gameScene.GameFullyLoaded() || IsLoadingFromFile || PositionFixed)
        {
            return;
        }

        if (!isScaledOnFloor && !IsThisADoubleBoundedStructure())
        {
            Program.gameScene.GameController1.NotificationsManager1.MainNotify("NotScaledOnFloor");
        }
        else if (!_isEven && !IsThisADoubleBoundedStructure())
        {
            Program.gameScene.GameController1.NotificationsManager1.MainNotify("NotEven");

        }
        else if (_isColliding)
        {
            if (HType == H.BullDozer)
            {
                Program.gameScene.GameController1.NotificationsManager1.MainNotify("Colliding.BullDozer");
                
            }
            else
            {
                Program.gameScene.GameController1.NotificationsManager1.MainNotify("Colliding");
            }
        }
        else if (!_isGoodWaterHeight && !IsThisADoubleBoundedStructure())
        {
            Program.gameScene.GameController1.NotificationsManager1.MainNotify("BadWaterHeight");
            
        }
        else if (!AreAnchorsOnUnlockRegions() && !IsThisADoubleBoundedStructure())
        {
            Program.gameScene.GameController1.NotificationsManager1.MainNotify("LockedRegion");
        }
            //if none is true then needs to be hidden bz might have being showed already by a quick
            //true of any above. so needs to be hidden
        else
        {
            Program.gameScene.GameController1.NotificationsManager1.HideMainNotify();
        }
    }

    bool AreAnchorsOnUnlockRegions()
    {
        if (TownLoader.IsTemplate || HType == H.BullDozer)
        {
            return true;
        }

        if (MeshController.BuyRegionManager1 == null)
        {
            return true;
        }

        var res = MeshController.BuyRegionManager1.AreAnchorsOnUnlockRegions(Anchors);

        if (!res && !IsThisADoubleBoundedStructure())//double bounded strucutres dont need unlocked regions to be placed  
        {
            MeshController.BuyRegionManager1.ShowRegions();
        }
        return res;
    }

    /// <summary>
    /// Tells u if the Anchors out of the anchors are on the Floor
    /// 
    /// Creted to avoid Streuctures be too close to Water or Mountain
    /// Farms needs to be further still more 
    /// </summary>
    /// <returns></returns>
    bool IsScaledAnchorsOnFloor()
    {
        if (HType == H.BullDozer)
        {
            return true;
        }

        var scale = ScaleVal();//0.2f was choose arbitrary. How far I gonna check
        //I scaled to address the problem when building is too close to water or cliff or moutain.
        //I make Anchors a bit bigger so checks for a bit bigger that Anchor area 
        var scaledAnchors = UPoly.ScalePolyNewList(Anchors, scale);
        //bz needs to find the real Y values
        scaledAnchors = RectifyOnY(scaledAnchors);
        //checks if is on the floor
        return IsOnTheFloor(scaledAnchors, 0.25f);
    }

    float ScaleVal()
    {
        if (MyId.Contains("Farm"))
        {
            //Farms need to check Further so they are not close to a shore 
            return 10f;//16
        }
        return 1.25f;
    }


    /// <summary>
    /// Will return the exact Y of each element on list along wth its X and Z.
    /// The Y will be find using RayCasting
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    List<Vector3> RectifyOnY(List<Vector3> list)
    {
        List<Vector3> res = new List<Vector3>();
        for (int i = 0; i < list.Count; i++)
        {
            res.Add(m.Vertex.BuildVertexWithXandZ(list[i].x, list[i].z));
        }
        return res;
    }

    /// <summary>
    /// Created so if is not defined will be defined here 
    /// 
    /// If is 'forced' wont care already some sort of acnhors exist it will find them again
    /// </summary>
    public List<Vector3> GetAnchors(bool forced=false)
    {
        //will find anchors
        UpdateMinAndMaxVar();
        _bounds = FindBounds(_min, _max);
        _anchors = FindAnchors(_bounds);

        //UVisHelp.CreateHelpers(_anchors, Root.blueCube);

        return _anchors;
    }



    /// <summary>
    /// Returns true if the building is place over the _minHeightToSpawn 
    /// _minHeightToSpawn is the water body plus a number. : _minHeightToSpawn
    /// </summary>
    /// <returns></returns>
    bool checkWaterHeight()
    {
        bool res = false;
        if (!_isMarine)
        {
            if (transform.position.y >= _minHeightToSpawn)
            {
                res = true;
            }
        }
        return res;
    }
    
    /// <summary>
    ///will find the farest point in a gameObj is lokking for the NW, NE, SE, SW . will retu
    ///in that sequence. 
    /// </summary>
    /// <param name="min">Bound.min, will work to if we pass SW</param>
    /// <param name="max">Bound.max, will work to if we pass NE</param>
    /// <returns>a List Vector3 wit sequence: NW, NE, SE, SW</returns>
    protected List<Vector3> FindBounds(Vector3 min, Vector3 max)//
    {
        float yMed = (min.y + max.y)/2;
        Vector3 NW = new Vector3(min.x, yMed, max.z);
        Vector3 NE = new Vector3(max.x, yMed, max.z);
        Vector3 SE = new Vector3(max.x, yMed, min.z);
        Vector3 SW = new Vector3(min.x, yMed, min.z);
        List<Vector3> res = new List<Vector3>() {NW, NE, SE, SW};

        //UVisHelp.CreateHelpers(res, Root.blueCubeBig);
        return res;
    }

    /// <summary>
    ///will find the farest point in a gameObj is lokking for the NW, NE, SE, SW . will retu
    ///in that sequence. Will use _min and _max
    /// </summary>
    /// <returns>a List Vector3 wit sequence: NW, NE, SE, SW</returns>
    protected List<Vector3> FindBounds()
    {
        Vector3 min = _min;
        Vector3 max = _max;
        float yMed = (min.y + max.y) / 2;
        Vector3 NW = new Vector3(min.x, yMed, max.z);
        Vector3 NE = new Vector3(max.x, yMed, max.z);
        Vector3 SE = new Vector3(max.x, yMed, min.z);
        Vector3 SW = new Vector3(min.x, yMed, min.z);
        List<Vector3> res = new List<Vector3>() { NW, NE, SE, SW };

        return res;
    }

    /// <summary>
    /// Find where this point are hitting the ground
    /// </summary>
    protected List<Vector3> FindAnchors(List<Vector3> list)
    {
        List<Vector3> res = new List<Vector3>();
        for (int i = 0; i < list.Count; i++)
        {
            //if (IsLoadingFromFile)
            //{
            //    res.Add(new Vector3(list[i].x, m.IniTerr.MathCenter.y, list[i].z));
            //}
            //else
                res.Add(m.Vertex.BuildVertexWithXandZ(list[i].x, list[i].z));
        }
        //UVisHelp.CreateHelpers(res, Root.blueCube);
        return res;
    }


#region LineUpTool
    //the poly on the grid that ocupies this building 
    //NW, NE , SE, SW
    List<Vector3> _polyOnGrid = new List<Vector3>();
    List<LineUpHelper> _lineUpHelpers = new List<LineUpHelper>(); 
    private bool setLineUp;
    void SetLineUpVertexs()
    {
        if (setLineUp || !PositionFixed || HType==H.Road || MyId.Contains("Bridge"))
        {
            return;
        }
        setLineUp = true;

        //bz was used to show prev 
        _polyOnGrid.Clear();
        
        //DefinePolyOnGrid();
        //SpawnLineUpHelpers();
    }

    private void SpawnLineUpHelpers()
    {
        for (int i = 0; i < _polyOnGrid.Count; i++)
        {
            _lineUpHelpers.Add(LineUpHelper.Create(Root.lineUpHelper, _polyOnGrid[i], container:transform));
        }
    }

    internal bool IsThisAHouseType()
    {
        return Building.IsHouseType(MyId);
    }

    /// <summary>
    /// Defines : _polyOnGrid. use to spawn Preview Box , and helper to aling Lines 
    /// </summary>
    private void DefinePolyOnGrid()
    {
        var scale = UPoly.ScalePoly(Anchors, 0.2f);

        //west
        var westOnAnchor = new Vector3(scale[0].x, scale[0].y, transform.position.z);
        var distCenterWest = Vector3.Distance(transform.position, westOnAnchor);
        var xS =  distCenterWest/m.SubDivide.XSubStep;

        int xSInt = (int)Math.Ceiling(xS);
        var westOnGrid = new Vector3(transform.position.x - (xSInt * m.SubDivide.XSubStep), scale[0].y, transform.position.z);

        //east
        var eastOnAnchor = new Vector3(scale[1].x, scale[0].y, transform.position.z);
        var distCenterEast = Vector3.Distance(transform.position, eastOnAnchor);
        var xSEast = distCenterEast / m.SubDivide.XSubStep;

        int xSIntEast = (int)Math.Ceiling(xSEast);
        var eastOnGrid = new Vector3(transform.position.x + (xSIntEast * m.SubDivide.XSubStep), scale[0].y, transform.position.z);


        //north
        var northOnAnchor = new Vector3(transform.position.x, scale[0].y, scale[0].z);
        var distCenterNorth = Vector3.Distance(transform.position, northOnAnchor);
        var zS = distCenterNorth / m.SubDivide.ZSubStep;

        int zSInt = (int)Math.Ceiling(zS) ;//+1 correction
        var northOnGrid = new Vector3(transform.position.x, scale[0].y, transform.position.z + (zSInt * m.SubDivide.ZSubStep));

        //south
        var southOnAnchor = new Vector3(transform.position.x, scale[0].y, scale[3].z);
        var distCenterSouth = Vector3.Distance(transform.position, southOnAnchor);
        var zSSouth = distCenterSouth / m.SubDivide.ZSubStep;

        int zSIntSouth = (int)Math.Ceiling(zSSouth) ;
        var southOnGrid = new Vector3(transform.position.x, scale[0].y, transform.position.z - (zSIntSouth * m.SubDivide.ZSubStep));

        _polyOnGrid.Add(new Vector3(westOnGrid.x, scale[0].y + .03f, northOnGrid.z));//NW
        _polyOnGrid.Add(new Vector3(eastOnGrid.x, scale[0].y + .03f, northOnGrid.z));//NE
        _polyOnGrid.Add(new Vector3(eastOnGrid.x, scale[0].y + .03f, southOnGrid.z));//SE
        _polyOnGrid.Add(new Vector3(westOnGrid.x, scale[0].y + .03f, southOnGrid.z));//SW

        //UVisHelp.CreateHelpers(westOnGrid, Root.yellowCube);
        //UVisHelp.CreateHelpers(eastOnGrid, Root.yellowCube);
        //UVisHelp.CreateHelpers(northOnGrid, Root.yellowCube);
        //UVisHelp.CreateHelpers(southOnGrid, Root.yellowCube);
        //UVisHelp.CreateHelpers(_polyOnGrid, Root.yellowCube);

    }


    public void ShowLineUpHelpers()
    {
        ShowBulidingPrev();
        for (int i = 0; i < _lineUpHelpers.Count; i++)
        {
            _lineUpHelpers[i].BringToEarth();
        }
    }

    public void HideLineUpHelpers()
    {
        HideBuildingPrev();
        for (int i = 0; i < _lineUpHelpers.Count; i++)
        {
            _lineUpHelpers[i].BackToSky();
        }
    }

#endregion


    /// <summary>
    /// Update _min, _max, _bounds, _anchors and then will call CheckIfIsEven(_anchors, maxDiffAllowOnTerrain)
    /// </summary>
    /// <returns></returns>
    public virtual bool CheckIfIsEvenRoutine()
    {
        if (HType == H.Dummy)
        {
            return true;
        }

        UpdateMinAndMaxVar();
        _bounds = FindBounds(_min, _max);
        _anchors = FindAnchors(_bounds);
        return CheckIfIsEven(_anchors, _maxDiffAllowOnTerrain);
    }

    /// <summary>
    /// Will check if the manyPolys passed has a bigger dif of height that maxDiff
    /// </summary>
    /// <param name="manyPoly"></param>
    /// <param name="maxDiff"></param>
    /// <returns>True if the diference of height is less or equeal than maxDiff</returns>
    protected bool CheckIfIsEven(List<Vector3> manyPoly, float maxDiff)
    {
        bool res = false;
        float min = manyPoly[0].y;
        float max = manyPoly[0].y;

        for (int i = 0; i < manyPoly.Count; i++)
        {
            if (manyPoly[i].y < min)
            {
                min = manyPoly[i].y;
            }
            else if (manyPoly[i].y > max)
            {
                max = manyPoly[i].y;
            }
        }

        float diff = max - min;
        if (diff <= maxDiff)
        {
            res = true;
        }

        return res;
    }

    /// <summary>
    /// Will return bounds Building must have a collider attached 
    /// </summary>
    /// <returns></returns>
    public List<Vector3> ReturnBounds()
    {
        UpdateMinAndMaxVar();
        return FindBounds();
    }

    /// <summary>
    /// Updates the Min and Max of the gameObject.transform.collider.bounds
    /// </summary>
    protected void UpdateMinAndMaxVar()
    {
        _min = gameObject.transform.GetComponent<Collider>().bounds.min;
        _max = gameObject.transform.GetComponent<Collider>().bounds.max;
        //UVisHelp.CreateText(_min, "min", 60);
        //UVisHelp.CreateText(_max, "max", 60);
    }

    // Use this for initialization
    protected void Start()
    {

     

        base.Start();
        float minHeightAboveSeaLevel = 1f;

        //if is not loading then the _rotationFacerIndex will be taken from 'static univRotationFacer'
        if (!IsLoadingFromFile && HType != H.Dummy)
        {
            _rotationFacerIndex = UnivRotationFacer;
            Inventory = new Inventory(MyId, HType); 
        }


        InitMilitar();
        InitWheelBarrow();
        InitDockDryDockAndSupplier();

        //if gives a null ex here ussually is that u forgot a prefab on scene
        _minHeightToSpawn = Program.gameScene.WaterBody.transform.position.y + minHeightAboveSeaLevel;

        if(!IsLoadingFromFile){CreateProjector();}

        //this is for init builds propertes such as Families, inventory
        Init();
        LayerRoutine("init");

        //brand new building .other wise was saved 
        if (!IsLoadingFromFile)
        {
            CurrentProd = BuildingPot.Control.ProductionProp.ReturnDefaultProd(HType);
        }

        InitJobRelated();




        StartCoroutine("ThirtySecUpdate");
        StartCoroutine("SixtySecUpdate");
        StartCoroutine("OneSecUpdate");

        DefinePreferedStorage();

        if (IsLoadingFromFile)
        {
            _lastStageTime = Time.time;
            DestroyDoubleBoundHelp();
        }



        var smokes = FindAllChildsGameObjectInHierarchyContain(gameObject, "Smoke");
        if (smokes != null)
        {
            _pSystem = new List<ParticleSystem>();

            for (int i = 0; i < smokes.Length; i++)
            {
                _pSystem.Add(smokes[i].GetComponent<ParticleSystem>());
            }
        }
        _stageManager = FindObjectOfType<StageManager>();
    }

#region Building preview

    BigBoxPrev buildingPrev;
    private Vector3 buildingPrevPos;
    private void ShowPreviewBoxForBuilding()
    {
        if (Anchors.Count == 0 || buildingPrev!=null || HType == H.Road || HType == H.Dummy)
        {
            return;
        }
        //UVisHelp.CreateHelpers(Anchors, Root.blueCube);

        DefinePolyOnGrid();
        buildingPrev = (BigBoxPrev)CreatePlane.CreatePlan(Root.bigBoxPrev, Root.graySemi, container:transform);
        buildingPrev.transform.name = "Building Preview: " + MyId;
        buildingPrev.UpdatePos(_polyOnGrid, .25f);

        //thisi is a Loading Building 
        if (PositionFixed)
        {
            HideBuildingPrev();
        }
    }

    void HideBuildingPrev()
    {
        if (buildingPrev!=null)
        {
            if (buildingPrevPos==new Vector3())
            {
                buildingPrevPos = buildingPrev.transform.position;
            }
           
            buildingPrev.transform.position = new Vector3(buildingPrevPos.x, buildingPrevPos.y - 30, buildingPrevPos.z);
        }
    }

    void ShowBulidingPrev()
    {
        if (buildingPrev != null)
        {
            buildingPrev.transform.position = buildingPrevPos;
        }
    }


    /// <summary>
    /// Return true if has all inputs 
    /// </summary>
    /// <returns></returns>
    internal bool DoIHaveInput()
    {
        var doIHaveInput = DoBuildHaveRawResources();
        return doIHaveInput;
    }

    internal string MissingInputs()
    {
        return Languages.ReturnString("Missing.Input")+
            BuildingPot.Control.ProductionProp.ReturnInputsINeed(this);
    }


    /// <summary>
    /// Will return the current missing inputs. May be more than one but will give
    /// you one that at least is missign in this moment
    /// 
    /// If is missing none then will return P.None
    /// </summary>
    /// <returns></returns>
    internal P MissingInput()
    {
        var ingre = BuildingPot.Control.ProductionProp.ReturnIngredients(CurrentProd.Product);

        if (ingre == null)
        {
            return P.None;
        }

        for (int i = 0; i < ingre.Count; i++)
        {
            if (!Inventory.Contains(ingre[i].Element))
            {
                return ingre[i].Element;
            }
        }
        return P.None;
    }


    /// <summary>
    /// When rotates needs to redo the whole thing 
    /// </summary>
    void DestroyPreviewBaseBuilding()
    {
        buildingPrev.Destroy();
        buildingPrev = null;
        _polyOnGrid.Clear();
    }

#endregion

    private MDate _checkDate;
    private IEnumerator SixtySecUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(60);



            if (Instruction == H.WillBeDestroy && PeopleDict.Count == 0 && 
                (Inventory == null || Inventory.IsEmpty()))
            {
                if (_checkDate != null)
                {
                    //bz they saty there forever now if they do they will add them self there after 30 days
                    if (Program.gameScene.GameTime1.ElapsedDateInDaysToDate(_checkDate) > 30)
                    {
                        PersonPot.Control.Queues.AddToDestroyBuildsQueue(Anchors, MyId);
                        _checkDate = Program.gameScene.GameTime1.CurrentDate();
                    }
                }
                //start
                else
                {
                    _checkDate = Program.gameScene.GameTime1.CurrentDate();
                }
            }
        }
    }


    private Decoration _decoration;
    private bool isDecorated;

    public Decoration Decoration1
    {
        get { return _decoration; }
        set { _decoration = value; }
    }

    protected void InitDecoration()
    {
        if (isDecorated || !PositionFixed || Anchors.Count == 0 )
        {
            return;
        }
        if (MyId.Contains("Bridge") || MyId.Contains("Farm") || doubleBounds.Contains(HType))
        {
            isDecorated = true;
            return;
        }

        isDecorated = true;

        if (!IsLoadingFromFile || _decoration == null)//for town loader 
        {
            _decoration = new Decoration(this);
        }
        else
        {
            //so it doesnt loose the loaded val
            _decoration.LoadDecora(this);
        }
    }

    #region Current Product

    /// <summary>
    /// Will show all the products this Building can produce 
    /// </summary>
    /// <returns></returns>
    public List<ProductInfo> ShowProductsOfBuild()
    {
        return BuildingPot.Control.ProductionProp.ReturnProducts(HType);
    }

    /// <summary>
    /// Will set CurrentProd to 'newProd'
    /// </summary>
    /// <param name="newProd"></param>
    public void SetProductToProduce(string newProd)
    {
        //the newProd comes with the name of the prod a . and a number that is the ID of the ProductInfo 
        var split = newProd.Split('.');
        int id = int.Parse(split[1]);
        var foundProd = BuildingPot.Control.ProductionProp.ReturnExactProduct(id);
        CurrentProd = foundProd;
       //Debug.Log("now Prod curr: "+CurrentProd.Product +" on:"+MyId);

        AddressNewProductOnBuilding();
        ReloadInventory();

        Quest(0, CurrentProd.Product);
    }



    /// <summary>
    /// So far used by: FieldFarm, and AnimalFarm
    /// </summary>
    private void AddressNewProductOnBuilding()
    {
        //nothing needs to get done 
        if (CurrentProd.Product == P.Stop)
        {
            return;
        }

        if (MyId.Contains("FieldFarm"))
        {
            var st = (Structure) this;
            st.ChangeProduct(CurrentProd.Product);
        }
        else if (MyId.Contains("AnimalFarm"))
        {
            RedoAnimalFarmIfNeeded();
        }
    }

    #endregion


    /// <summary>
    /// Address wht to do with a buliding that is marked as  H.WillBeDestroy and is loading from file 
    /// 
    /// Will make IsLoadingFromFile = false once executed 
    /// </summary>
    void LoadingWillBeDestroyBuild()
    {
        //if we deactivated , or hasnt loaded yet, or simply is not a loading Building 
        if (!IsLoadingFromFile || Instruction != H.WillBeDestroy || PersonPot.Control == null || PersonPot.Control.BuildersManager1 == null)
        {
            return;
        }

        if (IsLoadingFromFile && Instruction == H.WillBeDestroy)
        {
            var b = this;
            RemovePeople();

            //so dont call this method anymore
            IsLoadingFromFile = false;


            //if (Category != Ca.Way)
            //{
            //    //so the action of demolish happens again 
            //    var b = (Structure)this;
            //    b.Demolish();
            //}
            //else
            //{
            //    var b = (Trail)this;
            //    b.Demolish();
            //}
        }
    }


  


    #region Bad Ass

    private static MyProjector _projector;
    private static General _light;
    private static General _reachArea;

    /// <summary>
    /// this is the projector that hover when creating a nw building, or the current selected building
    /// </summary>
    public MyProjector Projector
    {
        get { return _projector; }
        set { _projector = value; }
    }
    public static General ReachArea
    {
        get { return _reachArea; }
        set { _reachArea = value; }
    }

    public void CreateProjector()
    {
        if (Category != Ca.None && Category != Ca.DraggableSquare 
            && Category != Ca.Way && Projector == null && !MyId.Contains("Dummy") 
            && HType != H.None)//none are the CreatePlanes 
        {
            Projector = (MyProjector) Create(Root.projector, container: transform);
            _light = Create(Root.lightCilWithProjScript, container: transform);


            _reachArea = Create(Root.reachArea, transform.position, container: transform);
            // *2 bz is from where the person is at so 'Brain.Maxdistance' is a  Radius
            _reachArea.transform.localScale = new Vector3(Brain.Maxdistance * 2, 0.1f, Brain.Maxdistance * 2);

            if (CurrentProd != null)
            {
                AudioPlayer.PlayThisSound1Time(HType + "", CurrentProd.Product.ToString());
            }
        }
    }

    protected void DestroyProjector()
    {
        if (Projector != null)
        {
            Projector.SwitchColorLight(true);
            Projector.Destroy();
            Projector = null;

            _light.Destroy();
            _light = null;

            _reachArea.Destroy();
            _reachArea = null;
        }
    }

    /// <summary>
    /// This is here bz when a building is selected a new proj has to be created
    /// </summary>
    public void CreateProjectorConditionals()
    {
        if (Projector == null)
        {
            CreateProjector();
        }
    }

    /// <summary>
    /// This is here bz when a building is unselected the proj has to be destroyed
    /// </summary>
    public void UnSelectBuilding()
    {
        DestroyProjector();
    }

    #endregion




    private void DebugShowAnchors()
    {
        if (debugShowAnchors)
        {
            return;
        }
        debugShowAnchors = true;

        //UVisHelp.CreateHelpers(GetAnchors(), Root.blueCube);
    }


    bool wasPersonControlRestarted;
    private bool debugShowAnchors;
    //this need to be called in derived classes 
    protected new void Update()
    {
        //DebugShowAnchors();
        SetLineUpVertexs();
        ShowPreviewBoxForBuilding();
        //NeutralizeDummy();

        InitFarm();
        InitDecoration();

        CheckIfNightSmoke();

        if (_militar!=null)
        {
            _militar.Update();
        }

        if (_constructionProgress != null)
        {
            _constructionProgress.Update();
        }

        //if is way not need to know this.
        //bz we will be going btw buildings 
        if (Category != Ca.Way)
        {
            LandZoneLoader(); 
        }

        //here I set : IsLoadingFromFile = false
        LoadingWillBeDestroyBuild();

        if (!PositionFixed)
        {
            base.Update();

            //will updtae if is not beinfg ordered to destroy and the instrucion not = = H.WillBeDestroy
            //added the last part to avoid Exception when class was checking on Building that was marked for be destroyed
            if(!_isOrderToDestroy && Instruction != H.WillBeDestroy)
            {UpdateBuild();}
        }



        if (_dock != null)
        {
            _dock.Update();
        }


        //bz now is waiting for a nw build to be placed to work 
        //this is here so it prompts the destruction of a building after the evacuation of the
        //inv has ocurred
        if (!wasPersonControlRestarted && _isOrderToDestroy && evacAll && Inventory.IsEmpty() && PeopleDict.Count == 0
            //&& PersonPot.Control.IsPeopleCheckFull()
            )//make sure all are checked so it doesnt interrupt anything 
        {
            wasPersonControlRestarted = true;
            //PersonPot.Control.RestartController();
            //DestroyOrdered(true);
        }
    }

    /// <summary>
    /// Updates all from Bounds to anchors 
    /// </summary>
    protected void UpdateBuild()
    {
        Program.gameScene.controllerMain.MeshController.UpdateHitMouseOnTerrain();
        UpdateClosestSubMeshVert();

        //this will check if is a double bounded strucuture
        if (IsThisADoubleBoundedStructure())
        {
            //so the arrow moves and follow the building
            //and anchors get updated and checks collision with others
            CheckEvenTerraCollWater();

            if (_isColliding)
            {
                _isBuildOk = false;
            }
            else
            {
                _isBuildOk = CheckDoubleBoundedStructureIsOkRoutine();
            }
            NotifyBuildingProblem(true);
        }
        else
        {
            _isBuildOk = CheckEvenTerraCollWater();
        }
    }

    /// <summary>
    /// Created for instance where need to update '_isBuildOk' from external class 
    /// </summary>
    public void UpdateBuildExternally()
    {
        UpdateBuild();
    }

    /// <summary>
    /// Updates the ClosestSubMeshVert 
    /// </summary>
    protected void UpdateClosestSubMeshVert()
    {
        ClosestSubMeshVert = m.Vertex.FindClosestVertex(m.HitMouseOnTerrain.point, m.CurrentHoverVertices.ToArray());
    }


    /// <summary>
    /// Updates the ClosestVertOld = ClosestSubMeshVert if the distance is further than 0.01f
    /// 
    /// This is what moves a building around the grid
    /// </summary>
    public virtual void UpdateClosestVertexAndOld()
    {
        if (!UMath.nearEqualByDistance(ClosestVertOld, ClosestSubMeshVert, 0.01f))
        {
            AudioCollector.PlayOneShot("ClickWood4",0);
            ClosestVertOld = ClosestSubMeshVert;
        }
    }

    /// <summary>
    /// Is called when a building needs to be rotated
    /// </summary>
    public void RotationAction()
    {
        _rotationFacerIndex = RotationFacer(_rotationFacerIndex);
        UnivRotationFacer = _rotationFacerIndex;
        gameObject.transform.Rotate(0, 90, 0);
        UpdateBuild();

        DestroyPreviewBaseBuilding();
    }

    /// <summary>
    /// Is called when we are done placing a new building 
    /// </summary>
    public virtual void DonePlace()
    {
        if (IsEven && !_isColliding)
        {
            FinishPlacingMode(H.Done);
        }
        else if (!IsEven)
        {
            //GameScene.ScreenPrint("Can't place, uneven terrain.Building.cs");
        }
        else if (_isColliding)
        {
            //GameScene.ScreenPrint("Is colliiding.Building.cs");
        }
    }
       
    /// <summary>
    /// 0 is up, 1 is right, 2 is down, 3 is left
    /// </summary>
    /// <param name="currentVal"></param>
    /// <returns></returns>
    private int RotationFacer(int currentVal)
    {
        currentVal++;
        if (currentVal > 3)
        {
            currentVal = 0;
        }
        return currentVal;
    }

    void DestroyDoubleBoundHelp()
    {
        //so it donest show the help 
        if (doubleBounds.Contains(HType))
        {
            _terraBound = GetChildLastWordIs(H.TerraBound);
            _maritimeBound = GetChildLastWordIs(H.MaritimeBound);
            _underTerraBound = GetChildLastWordIs(H.TerraUnderBound);

            Destroy(_maritimeBound);
            Destroy(_terraBound);
            Destroy(_underTerraBound);
        }
    }

    /// <summary>
    /// This is call when we finish placing a building 
    /// </summary>
    public virtual void FinishPlacingMode(H action)
    {
        //bz this action needs to be immediate 
        if (action == H.Cancel)
        {
            if (MyId.Contains(H.Bridge+""))
            {
                Trail t = (Trail) this;
                //means the brdige is currenty being built at the time by the class
                if (t.CurrentLoop!=H.None)
                {
                    return;
                }
            }

            if (Category==Ca.Way)
            {
                Way t = (Way)this;
            
            }

            HideBuildingPrev();
            DestroyCool();
            Program.MouseListener.HidePersonBuildOrderNotiWindows();
            ManagerReport.AddInput("Building Canceled: " + transform.name);
            return;
        }

        PlacedBuildingFX();

        AddNavMeshObst();
        ManagerReport.AddInput("Building Placed: " + transform.name);

        LayerRoutine("done");
        PositionFixed = true;

        //UnityEngine.AI.NavMeshBuilder.BuildNavMesh();
        //UnityEngine.AI.NavMeshBuilder.BuildNavMesh();


        DestroyDoubleBoundHelp();

        //Preview of the Base to help aling
        if (!IsLoadingFromFile)
        {
            BuildingPot.InputU.AddToOrginizeStructures(this);
        }
        
        if (!HType.ToString().Contains("Unit") && !IsLoadingFromFile && HType != H.BullDozer)
        {
            PrivHandleZoningAddCrystals(); ;
        }




        if (!HType.ToString().Contains("Unit") && !IsLoadingFromFile && HType != H.BullDozer
            && HType != H.Road)
        {
            //_constructionProgress = new ConstructionProgress(this);
        }

        //the brdige was calling since a brand new was not even set to ground yet
        if (IsLoadingFromFile || MyId.Contains("Bridge"))
        {
            return;
        }

        //calling here bz now since Builds are placed on ground need to be seen 
        //by all
        BuildingPot.Control.AddToQueuesRestartPersonControl(MyId);
    }


    #region PlacedFX

    List<General> _placedBuildFX = new List<General>();
    void PlacedBuildingFX()
    {
        if (IsLoadingFromFile            )
        {
            return;
        }

        //placed building
        var dust = General.Create("Prefab/Particles/PlaceBuildDust", MiddlePoint());
        AudioCollector.PlayOneShot("ConstructionPlaced", transform.position);

        if (HType == H.StandLamp)
        {
            return;
        }

        _placedBuildFX.Add(Create("Prefab/Building/Show/Center", MiddlePoint(), "Placed", transform));
        for (int i = 0; i < _anchors.Count; i++)
        {
            var c = Create("Prefab/Building/Show/Corner", _anchors[i], "Placed", transform);
            c.transform.LookAt(MiddlePoint());
            _placedBuildFX.Add(c);
        }
    }

    void DestroyAllPlacedFX()
    {
        for (int i = 0; i < _placedBuildFX.Count; i++)
        {
            Destroy(_placedBuildFX[i].gameObject);
        }
        _placedBuildFX.Clear();
    }

    #endregion

    #region NavMesh

    NavMeshObstacle _nav;
    float _lastStageTime = -1;
    bool _wasNavSet;
    Vector3 _navInitSize;

    /// <summary>
    /// if is not here then will be reduced standard amt , on SetNavMeshObstacle() (-16, 0, -16)
    /// </summary>
    Dictionary<H, Vector3> _percetagesReduction = new Dictionary<H, Vector3>()
    {


        {H.Bohio, new Vector3(-37,0,-53)},
        {H.WoodHouseA, new Vector3(-20,0,-20)},
        {H.WoodHouseB, new Vector3(-20,0,-25)},
        {H.WoodHouseC, new Vector3(-20,0,-20)},

        { H.BrickHouseA, new Vector3(-25,0,-20)},
        { H.BrickHouseB, new Vector3(-25,0,-30)},
        { H.BrickHouseC, new Vector3(-25,0,-20)},

        {H.Shack, new Vector3(-45,0,-50)},
        {H.MediumShack, new Vector3(-30,0,-40)},
        {H.LargeShack, new Vector3(-28,0,-42)},

        { H.FieldFarmSmall, new Vector3(-19,0,-32)},
        { H.FieldFarmMed, new Vector3(-19,0,-20)},
        { H.FieldFarmLarge, new Vector3(-10,0,-20)},

        { H.FishingHut, new Vector3(-40,0,-40)},

        {H.StandLamp, new Vector3(0,0,0)},//wont get carved
        { H.HeavyLoad, new Vector3(-8,0,-8)},
        { H.LightHouse, new Vector3(-20,0,-40)},

        { H.LumberMill, new Vector3(-19,0,-45)},
        { H.BlackSmith, new Vector3(-10,0,-16)},
        { H.Mortar, new Vector3(-16,0,-10)},
        { H.QuickLime, new Vector3(-40,0,-40)},

        { H.Carpentry, new Vector3(-30,0,-30)},
        { H.Cigars, new Vector3(-16,0,-10)},
        { H.Armory, new Vector3(-10,0,-10)},
        { H.Tailor, new Vector3(-40,0,-25)},
        { H.Mill, new Vector3(-35,0,-45)},
        { H.Chocolate, new Vector3(-16,0,-10)},

        { H.Ink, new Vector3(-15,0,-12)},
        { H.GunPowder, new Vector3(-10,0,-16)},
        { H.PaperMill, new Vector3(-16,0,-12)},
        { H.CoinStamp, new Vector3(-10,0,-16)},
        { H.Foundry, new Vector3(-16,0,-12)},


        { H.StorageMed, new Vector3(-30,0,-20)},
        { H.StorageSmall, new Vector3(-35,0,-25)},
        { H.StorageBig, new Vector3(-55,0,-35)},

        { H.Dock, new Vector3(-5,0,-5)},


        { H.School, new Vector3(-10,0,-14)},
        { H.TradesSchool, new Vector3(-16,0,-10)},
        { H.Library, new Vector3(-17,0,-12)},


        { H.Church, new Vector3(-8,0,-20)},
        { H.Tavern, new Vector3(-25,0,-20)},


    };

    /// <summary>
    /// called on Start
    /// </summary>
    void AddNavMeshObst()
    {
        Geometry.AddComponent<NavMeshObstacle>();
        _nav = Geometry.GetComponent<NavMeshObstacle>();
        _navInitSize = _nav.size;

        Vector3 perc = new Vector3(10,0, 10);
        //it grows a bit so move people away a bit so when carving the are not
        //in the middle of noWhere
        _nav.size = new Vector3(
            UMath.ScalePercentage(_nav.size.x, perc.x),
            UMath.ScalePercentage(_nav.size.y, perc.y),
            UMath.ScalePercentage(_nav.size.z, perc.z));
    }

    void CheckOnSetNavMeshObst()
    {
        if (!_wasNavSet && Time.time > _lastStageTime + 0.01f && _lastStageTime > 0)
        {
            _wasNavSet = true;
            if (_nav!=null)
            {
                SetNavMeshObstacle();
            }
        }
    }

    /// <summary>
    /// Called once is fully built 
    /// Needs to be added to Main GameObect or Geometry GO
    /// </summary>
    private void SetNavMeshObstacle()
    {
        //restores initial size 
        _nav.size = _navInitSize;

        var perc = new Vector3(-16, 0, -16);
        if (_percetagesReduction.ContainsKey(HType))
        {
            perc = _percetagesReduction[HType];
        }
        if (perc == new Vector3())
        {
            return;
        }

        //then scales 
        _nav.size = new Vector3(
            UMath.ScalePercentage(_nav.size.x, perc.x),
            UMath.ScalePercentage(_nav.size.y, perc.y),
            UMath.ScalePercentage(_nav.size.z, perc.z));
        _nav.carving = true;
        _nav.carvingMoveThreshold = 0;
    }


    private IEnumerator OneSecUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(1); // wait
            CheckOnSetNavMeshObst();
        }
    }


    #endregion



    /// <summary>
    /// Checks if is colliding with another building 
    /// </summary>
    /// <returns>True if collides</returns>
    public virtual bool CheckIfColliding()
    {
        if (HType == H.BullDozer)
        {
            return false;
        }

        var tBounds = _bounds.ToArray();
        tBounds = UPoly.ScalePoly(tBounds, 0.05f);

        return BuildingPot.Control.Registro.IsCollidingWithExisting(tBounds.ToList());
    }

    /// <summary>
    /// Checks if current game obj is colliding with boundsP pass as param 
    /// </summary>
    /// <returns>True if collide</returns>
    public virtual bool CheckIfColliding(List<Vector3> boundsP)
    {
        return BuildingPot.Control.Registro.IsCollidingWithExisting(boundsP);
    }

    /// <summary>
    /// Will destroy the current obj and, the _isOrderToDestroy is set in Building.cs
    /// but the call comes from child 
    /// </summary>
    protected virtual void DestroyOrdered(bool forced = false)
    {
        if ((_isOrderToDestroy           
            //this is for addres the problem where routing is happening and a Building is destroyed
            && PersonController.UnivCounter == -1) || forced)
        {
            if (_arrow != null)
            {
                _arrow.Destroy();
                _arrow = null;
            }

            //if was CancelDemolish
            if (Instruction!=H.WillBeDestroy)
            {
                return;
            }

            BuildingPot.Control.Registro.RemoveItem(Category, MyId);
            BuildingPot.Control.Registro.AllBuilding.Remove(MyId);
            MeshController.CrystalManager1.Delete(this);

            DestroyProjector();
            Program.MouseListener.BuildingWindow1.HideIfSameBuilding(this);

            //so saveLoad of buildings is not affected 
            BuildingPot.Control.Registro.RemoveFromAllRegFile(MyId);
            BuildingPot.Control.EditBuildRoutine(MyId, H.Remove, HType);

            //only usefull for loaded buildings that were Destroy before finish construcion
            //and were loaded 
            PersonPot.Control.BuildersManager1.RemoveConstruction(MyId);//so its removed from the BuilderManager

            //in case was destroyed directly needs to be removed so the next time user 
            //wants to demolish works 
            BuildingPot.Control.Registro.RemoveFromDestroyBuildings(this);


            var dust = General.Create("Prefab/Particles/PlaceBuildDust", MiddlePoint());
            var remainings = Create("Prefab/Building/Show/Remainings", MiddlePoint(), "Remainings");
            remainings.transform.Rotate(new Vector3(0, UMath.Random(0, 360), 0));
            AudioCollector.PlayOneShot("BUILDING_DEMOLISH_1", 0);

            Destroy();
        }
    }

    /// <summary>
    /// To be used by the queues
    /// </summary>
    public void DestroyOrderedForced()
    {
        DestroyOrdered(true);   
    }




    #region Mark Terra Spawn Obj When Create Building
    
    /// <summary>
    /// Still spawners will call this to check
    /// </summary>
    public void CheckOnMarkTerra()
    {
        MarkTerraSpawnRoutine(20, from: transform.position);
    }
    
    
    /// <summary>
    /// This is the routine to gather the object we are surroundig and then 
    /// marking them
    /// </summary>
    /// <param name="listFrom">This will cast ffrom the whole list, stuitable for way childs</param>
    /// <param name="radiusP">Radius of the sphere casting</param>
    /// <param name="from">From is to be use mainly by structures or one obj casting</param>
    protected void MarkTerraSpawnRoutine(float radiusP, List<Vector3> listFrom = null, Vector3 from = new Vector3())
    {
        List<string> collidWith = new List<string>();

        //if from is new Vector3() will use the list 
        if (from == new Vector3())
        {
            for (int i = 0; i < listFrom.Count; i++)
            {
                collidWith.AddRange(URayCast.CastSphere(listFrom[i], radiusP));
            }
        }
        //other wise will use Vector3 from
        else collidWith.AddRange(URayCast.CastSphere(from, radiusP));

        collidWith = CleanList(p.TerraSpawnController.AllRandomObjList, collidWith);
        for (int i = 0; i < collidWith.Count; i++)
        {
            var key = collidWith[i];
            DestroySpawn(key);
        }
        if (listFrom == null && from == new Vector3())
        { print("Error: Both obj passed were null. Building.MarkTerraSpawnRoutine"); }
    }

   protected void DestroySpawn(string key)
    {
        if (p.TerraSpawnController.AllRandomObjList.Contains(key))
        {
            if (!CrystalsAreContainedInThisBuilding(key) && 
                !key.Contains("Orna") && !key.Contains("Grass"))//refer to StillElement.AddCrystals. They
                //dont add to the crsytals manager, so they will be removed this way
            {
                return;
            }

            StillElement still = (StillElement)p.TerraSpawnController.AllRandomObjList[key];
            //so they get remved from their region 
            Program.gameScene.BatchRemove(still);
            //so they disappear, remove Crystals and Routing can work properly
            still.DestroyCool();

            if (HType == H.BullDozer)
            {
                Program.gameScene.TutoStepCompleted("BullDozer.Tuto");
            }
        }
        else
            Debug.Log("key not cointained in AllRandomObjList." + key);
    }

    /// <summary>
    /// Will say if the Spawn has any cristals that faill in in this building 
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
   private bool CrystalsAreContainedInThisBuilding(string key)
   {
        if (Bounds == null || Bounds.Count == 0)
        {
            Bounds = FindBounds();
        }


       var loBound = Bounds.ToArray();
       var scale = UPoly.ScalePoly(loBound, 0.4f).ToArray();
       var rect = Registro.ReturnDimOnMap(scale.ToList());

       return MeshController.CrystalManager1.AreTheyContained(key, rect);
   }

    /// <summary>
    /// Given a list of strings will reutn only the ones tat are contained in the KeyedColl
    /// </summary>
    List<string> CleanList(KeyedCollection<string, TerrainRamdonSpawner> coll, List<string> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (!coll.Contains(list[i]))
            {
                list.RemoveAt(i);
                i--;
            }
        }
        return list;
    }
    #endregion

    /// <summary>
    /// Will check if a path is even , default _maxDiffAllowOnTerrainForARoad if is bigger than taht is not even
    /// </summary>
    protected bool AreAllPointsEven(List<Vector3> path, float maxDiff = 0)
    {
        if (maxDiff == 0)
        {maxDiff = _maxDiffAllowOnTerrainForARoad;}

        //print(UMath.ReturnDiffBetwMaxAndMin(path, H.Y) + ".diff");

        if (UMath.ReturnDiffBetwMaxAndMin(path, H.Y) > maxDiff)
        {
            return false;
        }
        return true;
    }

    #region Create For Double Bound Strucutres Such as Maritimes and UnderTerra

    List<H> doubleBounds = new List<H>(){H.FishingHut, 
        H.Dock, H.Shipyard, H.Supplier,
        H.MountainMine, H.ShoreMine, H.LightHouse,
        H.PostGuard
    };

    private GameObject _maritimeBound;
    private GameObject _terraBound;
    private GameObject _underTerraBound;

    //for the primary bound TerraBound
    protected Vector3 _minPrim;
    protected Vector3 _maxPrim;
    private List<Vector3> _boundsPrim = new List<Vector3>();
    private List<Vector3> _anchorsPrim = new List<Vector3>();

    //for the secondary bound Maritime or UnderTerrsa bound
    protected Vector3 _minSec;
    protected Vector3 _maxSec;
    private List<Vector3> _boundsSec = new List<Vector3>();
    private List<Vector3> _anchorsSec = new List<Vector3>();

    /// <summary>
    /// Tells u if param passed is a bounded structure. Is comparing to items on List<H> doubleBounds
    /// </summary>
    protected bool IsThisADoubleBoundedStructure()
    {
        for (int i = 0; i < doubleBounds.Count; i++)
        {
            if (HType == doubleBounds[i]) { return true;}
        }
        return false;
    }


    /// <summary>
    /// Check if the double bouded structure is ok
    /// </summary>
    /// <returns>True if is good</returns>
    protected bool CheckDoubleBoundedStructureIsOkRoutine()
    {
        if (HType == H.MountainMine)
        {
           DefineBoundsGameObj(H.TerraUnderBound);
           return RoutineToFindIfAnchorsAreGood(_terraBound, _underTerraBound, H.TerraUnderBound) 
               //&& AreAnchorsOnUnlockRegions()
               ;
        }
        else
        {
            //for DockTypes
            var reachRoute = true;
            if (IsNaval())//if is not DockType wont be affected 
            {
                reachRoute = _dock.CanIReachRoute();
            }

            DefineBoundsGameObj(H.MaritimeBound);
            return RoutineToFindIfAnchorsAreGood(_terraBound, _maritimeBound, H.MaritimeBound) && reachRoute
                //&& AreAnchorsOnUnlockRegions()
                ;
        }
    }


    /// <summary>
    /// Update the bounds and anchors of the 2 bounds
    /// </summary>
    /// <param name="prim">Primary bound</param>
    /// <param name="sec">Secondary bound</param>
    void UpdateDoubleBounds(GameObject prim, GameObject sec)
    {
        UpdateMinAndMaxVar(prim, 1);
        UpdateMinAndMaxVar(sec, 2);

        _boundsPrim = FindBounds(_minPrim, _maxPrim);
        _anchorsPrim = FindAnchors(_boundsPrim);

        _boundsSec = FindBounds(_minSec, _maxSec);
        _anchorsSec = FindAnchors(_boundsSec);
    }

    /// <summary>
    /// Routine To Find If Anchors Are Good
    /// </summary>
    /// <param name="prim">Primary bound</param>
    /// <param name="sec">Secondary bound</param>
    /// <param name="typeOfDoubleBound">Type of bound TerraBound or MaritimeBound</param>
    /// <returns>True if they are good</returns>
    bool RoutineToFindIfAnchorsAreGood(GameObject prim, GameObject sec, H typeOfDoubleBound)
    {
        UpdateDoubleBounds(prim, sec);
        
        bool res = FindIfListIsAboveThisHeight(Program.gameScene.WaterBody.transform.position.y + 0.1f, _anchorsPrim);
        if (res)
        {
            if (typeOfDoubleBound == H.MaritimeBound)
            {
                res = FindIfListIsBelowThisHeight(Program.gameScene.WaterBody.transform.position.y - 0.1f, _anchorsSec)
                    && AreAllPointsEven(_anchorsPrim) && IsOnTheFloor(_anchorsPrim);
            }
            else if (typeOfDoubleBound == H.TerraUnderBound)
            {
                res = FindIfListIsAboveThisHeight(_maxSec.y + 0.1f, _anchorsSec)
                    && AreAllPointsEven(_anchorsPrim) && IsOnTheFloor(_anchorsPrim);
            }
        }
        return res;
    }

    /// <summary>
    /// Check if is on the floor not to high not to low. Comparing to the most Y know value on the mesh 
    /// </summary>
    protected bool IsOnTheFloor(List<Vector3> anchorsListP, float variance = 0.1f)
    {
        for (int i = 0; i < anchorsListP.Count; i++)
        {
            bool isV3onFloor = IsVector3OnTheFloor(anchorsListP[i], m.SubMesh.mostCommonYValue, variance);
            if (!isV3onFloor)
            {
                return false;
            }
        }
        return true;
    }

    public static bool IsVector3OnTheFloor(Vector3 v3, float mostCommonYValue, float variance = 0.1f)
    {
        Vector3 v3Copy = new Vector3(v3.x, v3.y, v3.z);

        if (v3Copy.y > mostCommonYValue + variance
                || v3Copy.y < mostCommonYValue - variance)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// Finds if all the elements are above the 'heightToEval'
    /// </summary>
    /// <param name="heightToEval">height to be evaluated</param>
    /// <param name="listP">list of vector3</param>
    /// <returns>True if all point are above</returns>
    bool FindIfListIsAboveThisHeight(float heightToEval, List<Vector3> listP )
    {
        for (int i = 0; i < listP.Count; i++)
        {
            if (listP[i].y < heightToEval)
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Find if the whole list is below 
    /// </summary>
    /// <param name="heightToEval">height to be evaluated</param>
    /// <param name="listP">list of vector3</param>
    /// <returns>True if all point are below</returns>
    bool FindIfListIsBelowThisHeight(float heightToEval, List<Vector3> listP)
    {
        for (int i = 0; i < listP.Count; i++)
        {
            if (listP[i].y > heightToEval)
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Define the Bounds of the game obj 
    /// Depending of the typeOfDoubleBound type will define what bound it is 
    /// _terraBound for both , and the second one could be _maritimeBound or _underTerraBound
    /// </summary>
    /// <param name="typeOfDoubleBound"></param>
    void DefineBoundsGameObj(H typeOfDoubleBound)
    {
        if (_terraBound == null)
        {
            if (typeOfDoubleBound == H.MaritimeBound)
            {
                _terraBound = GetChildLastWordIs(H.TerraBound);
                _maritimeBound = GetChildLastWordIs(H.MaritimeBound);
            }
            else if (typeOfDoubleBound == H.TerraUnderBound)
            {
                _terraBound = GetChildLastWordIs(H.TerraBound);
                _underTerraBound = GetChildLastWordIs(H.TerraUnderBound);
            }
        }
    }

    /// <summary>
    /// Updates the _minPrim, _maxPrim, _minSec, _maxSec. 
    /// Based on the whichBound number if is 1 will update Prim
    /// if is 2 will update the Sec 
    /// </summary>
    /// <param name="passP">Game obj that has the bound</param>
    /// <param name="whichBound">whichBound number if is 1 will update Prim if is 2 will update the Sec</param>
    void UpdateMinAndMaxVar(GameObject passP, int whichBound)
    {
        if (whichBound == 1)
        {
            _minPrim = passP.transform.GetComponent<Collider>().bounds.min;
            _maxPrim = passP.transform.GetComponent<Collider>().bounds.max;
        }
        else if (whichBound == 2)
        {
            _minSec = passP.transform.GetComponent<Collider>().bounds.min;
            _maxSec = passP.transform.GetComponent<Collider>().bounds.max;
        }
    }

    #endregion















    #region House In Game Properties. Such as Families, Invetorey, Workers, Pay, Production

    //this will flag instructions on the building that will be utilize by other class
    private H _instruction;
    //all the people is related to this building... includes all
    //people living, or working, or attending to church 
    private List<string> _peopleDict = new List<string>(); 
    private Family[] _families;//the family or famililes living in a house (if is 2 floors)

    private ProductInfo _currentProd ;//product currently is being created on this building 
    private ProductInfo _oldProd ;//product currently is being created on this building 
    private int _rationsPay = 2;//the pay in rations to the workers of a building 
    private int _dollarsPay = 5;//in dollars 

    private int _comfort;//the confort of a house
    private BookedHome _bookedHome;//will hold the information if a

    public ProductInfo CurrentProd
    {
        get { return _currentProd; }
        set { _currentProd = value; }
    }

    public Family[] Families
    {
        get { return _families; }
        set { _families = value; }
    }

    public H Instruction
    {
        get { return _instruction; }
        set { _instruction = value; }
    }

    public List<string> PeopleDict
    {
        get { return _peopleDict; }
        set
        {

            _peopleDict = value;

            //no need if is loading from file now 
            if (IsLoadingFromFile)
            {
                return;
            }
            //so its update it there so when SaveLoad is current
            BuildingPot.Control.Registro.ResaveOnRegistro(MyId);
        }
    }

    public int RationsPay
    {
        get { return _rationsPay; }
        set { _rationsPay = value; }
    }

    public int DollarsPay
    {
        get { return _dollarsPay; }
        set { _dollarsPay = value; }
    }

    public int Comfort
    {
        get { return _comfort; }
        set { _comfort = value; }
    }

    public BookedHome BookedHome1
    {
        get { return _bookedHome; }
        set { _bookedHome = value; }
    }

    void Init()
    {

        Comfort =  ReturnHouseConfort(HType);

        InitBasePays();

        _oldProd = _currentProd;
    }




    #region Salary

    /// <summary>
    /// this is the base pay for each building. From here will move to change salary
    /// </summary>
    private void InitBasePays()
    {
        if (IsLoadingFromFile)
        {
            return;
        }

        _dollarsPay = BasePay();
    }

    /// <summary>
    /// the base pay for each job
    /// </summary>
    /// <returns></returns>
    int BasePay()
    {
        if (HType == H.Pottery)
        {
            //return  10;
        }
        if (HType == H.Church)
        {
            //return 20;
        }
        return 5;
    }

    public bool IsACoverageBuilding()
    {
        return HType == H.School || HType == H.TradesSchool || HType == H.Church
               || HType == H.Tavern;
    }

    /// <summary>
    /// The user wil click on the checkbox and tht will change the salary
    /// 
    /// checkBox val : 1-5
    /// </summary>
    /// <param name="which"></param>
    public string ChangeSalary(string which)
    {
        if (which == "Less")
        {
            DollarsPay -= 1;
        } 
        if (which == "More")
        {
            DollarsPay += 1;
        }

        //wont be less than 1 
        if (DollarsPay < 1)
        {
            DollarsPay = 1;
        }

        BuildingPot.Control.Registro.ResaveOnRegistro(MyId);

        AudioCollector.PlayOneShot("BoughtLand", 0);

        return DollarsPay+"";
    }

    /// <summary>
    /// Use to duisplay the checkbox right in the Building Windows 
    /// </summary>
    /// <returns></returns>
    public int WhichIsTheBuildingSalaryStatus()
    {
        var diff = DollarsPay - BasePay();

        return diff;
    }


#endregion


    

    /// <summary>
    /// Adds items to the Storage Buildings
    /// </summary>
    void InitStorage()
    {
        if (!HType.ToString().Contains("Storage"))
        {
            return;
        }

        BuildingPot.Control.DispatchManager1.ActiveDormantList();
        
        if (PersonPot.Control == null || Inventory == null) { return;}

        //
        var amtOfStorages = BuildingPot.Control.FoodSources.Count;


        if (Inventory.IsEmpty() && amtOfStorages == 1)
        {
            int amtFood = PersonPot.Control.CurrentCondition().iniFood;
            Inventory.Add(P.Bean, amtFood);


            Program.gameScene.GameController1.SetInitialLote();


            UpdateInfo();
        }
        
    }

    public void UpdateInfo(string v = "")
    {
        info = Inventory.Info();
    }



    protected void InitHouseProp()
    {
        if (IsLoadingFromFile)
        {
            return;
        }

        //can hhave 1 famili with 3 kids
        if (HType == H.Bohio || HType == H.Shack)
        {
            Families = new Family[1];
            Families[0] = new Family(UMath.GiveRandom(2, 3), MyId, 0);
        }
        else if (HType == H.MediumShack)
        {
            Families = new Family[1];
            Families[0] = new Family(UMath.GiveRandom(2, 4), MyId, 0);
        }
        else if (HType == H.LargeShack)
        {
            Families = new Family[1];
            Families[0] = new Family(UMath.GiveRandom(2, 5), MyId, 0);
        }


        //can hhave 1 famili with 3 kids
        else if (HType == H.WoodHouseA || HType == H.WoodHouseC)
        {
            Families = new Family[1];
            Families[0] = new Family(UMath.GiveRandom(2, 4), MyId, 0);
        }
        //can hhave 2 famili with 3 kids each
        else if (HType == H.WoodHouseB)
        {
            Families = new Family[1];
            Families[0] = new Family(UMath.GiveRandom(3, 5), MyId, 0);
        }
        //can hhave 1 famili with 5 kids
        else if (HType == H.BrickHouseA)
        {
            Families = new Family[1];
            Families[0] = new Family(3, MyId, 0);
        }
        else if (HType  ==  H.BrickHouseB)
        {
            Families = new Family[1];
            Families[0] = new Family(UMath.GiveRandom(3, 5), MyId, 0);
        }
        else if (HType == H.BrickHouseC)
        {
            Families = new Family[1];
            Families[0] = new Family(4, MyId, 0);
        }

        //resave familie
        BuildingPot.Control.Registro.ResaveOnRegistro(MyId);
    }

    /// <summary>
    /// Will find the family of the person asking .
    /// If not found will return null
    /// </summary>
    /// <param name="person"></param>
    /// <returns></returns>
    public Family FindMyFamilyChecksFamID(Person person)
    {
        for (int i = 0; i < Families.Length; i++)
        {
            if (Families[i].DoIBelongToThisFamilyChecksFamID(person))
            {
                return Families[i];
            }
        }
        return null;
    }    
    
    public Family FindMyFamily(Person person)
    {
        for (int i = 0; i < Families.Length; i++)
        {
            if (Families[i].DoIBelongToThisFamily(person))
            {
                return Families[i];
            }
        }
        return null;
    }

    /// <summary>
    /// Will find the family by ID 
    /// 
    /// Will see if pers.MyId or pers.Spouse is contained in any of this building Family ID, 
    /// bz families get one or other spouse ID
    /// </summary>
    /// <param name="pers"></param>
    /// <returns></returns>
    internal Family FindOldFamilyById(Person pers)
    {
        for (int i = 0; i < Families.Length; i++)
        {
            if ((Families[i].FamilyId.Contains(pers.MyId)) ||
                (!string.IsNullOrEmpty(pers.Spouse) && Families[i].FamilyId.Contains(pers.Spouse)))
            {
                return Families[i];
            }
        }
        return null;
    }

    /// <summary>
    /// Will find the family by ID 
    /// 
    /// Used by teens moving out 
    /// IF they were given a family ID then then should get into that one
    /// </summary>
    /// <param name="pers"></param>
    /// <returns></returns>
    internal Family FindFamilyById(string famID)
    {
        for (int i = 0; i < Families.Length; i++)
        {
            if ((Families[i].FamilyId == famID))
            {
                return Families[i];
            }
        }
        return null;
    }

    /// <summary>
    /// Will tell if the person asking could fit in any of the families of the building
    /// </summary>
    /// <param name="asker"></param>
    /// <returns></returns>
    public bool WouldAdultFitInThisHouseInAFamily(Person asker, ref string familyID)
    {
        for (int i = 0; i < Families.Length; i++)
        {
            if (Families[i].WouldAdultFitInThisFamily(asker))
            {
                familyID = Families[i].FamilyId;
                asker.PersonReport.whoGreenMeToBecomeMajor = MyId;
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Will return true if at least one family is empty in this building 
    /// </summary>
    /// <returns></returns>
    public bool IsALeastOneFamilyEmpty()
    {
        for (int i = 0; i < Families.Length; i++)
        {
            if (Families[i].IsFamilyEmpty())
            {
                return true;
            }
        }
        return false;
    }

    //public bool AtLeastHasOneVirginFamily()
    //{
    //    var vFam = FindVirginFamily();

    //    if (vFam == null)
    //    {
    //        return false;
    //    }
    //    return true;
    //}

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public int EmptyFamilies()
    {
        int count = 0;
        for (int i = 0; i < Families.Length; i++)
        {
            if (Families[i].IsFamilyEmpty())
            {
                count++;
            }
        }
        return count;
    }

    /// <summary>
    /// All Families fuul is created so Houses tht only are formed (wife and husband) are open 
    /// to recive new kids 
    /// </summary>
    /// <returns></returns>
    public bool AllFamiliesFull()
    {
        int count = 0;
        for (int i = 0; i < Families.Length; i++)
        {
            if (Families[i].AFamilyIsFull())
            {
                count++;
            }
        }

        if (count == Families.Length)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Will return the first emty family in the builing. If no empty families will return null
    /// </summary>
    /// <returns></returns>
    public Family ReturnEmptyFamily()
    {
        for (int i = 0; i < Families.Length; i++)
        {
            if (Families[i].IsFamilyEmpty())
            {
                return Families[i];
            }
        }
        return null;
    }


    public static int ReturnHouseConfort(H HTypeP)
    {
        if ( HTypeP == H.Bohio || HTypeP == H.Shack || HTypeP == H.MediumShack)
        {
            return  1;
        }
        if (HTypeP == H.LargeShack || HTypeP == H.WoodHouseA)
        {
            return  2;
        }
        else if (HTypeP == H.WoodHouseB)
        {
            return  3;
        }
        else if (HTypeP == H.WoodHouseC || HTypeP == H.BrickHouseA || HTypeP == H.BrickHouseC)
        {
            return 4;
        }
        else if ( HTypeP == H.BrickHouseB)
        {
            return  5;
        }
        return 1;
    }

   

    public bool ThisPersonFitInThisHouse(Person newPerson, ref string famID)
    {
        if (Families == null)
        {
            return false;
        }

        if(!UPerson.IsMajor(newPerson.Age))
        {
            return ANewKidFitsInThisHouse(newPerson, ref famID);
        }
        return ANewAdultFitsInThisHouse(newPerson, ref famID);
    }






    bool ANewKidFitsInThisHouse(Person newP, ref string famID)
    {
        for (int i = 0; i < Families.Length; i++)
        {
            if (Families[i].CanGetAnotherKid(newP))
            {
                famID = Families[i].FamilyId;
                return true;
            }
        }
        return false;
    }

    bool ANewAdultFitsInThisHouse(Person newP, ref string famID)
    {
        for (int i = 0; i < Families.Length; i++)
        {
            //some times people ask before build Start() 
            if (!BuildingPot.Control.Registro.AllBuilding.ContainsKey(Families[i].Home))
            {
                return false;
            }

            if (Families[i].CanGetAnotherAdult(newP))
            {
                famID = Families[i].FamilyId;

                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Here so calls the  BuilderPot.Control and updates the method that is for the specific type of building 
    /// </summary>
    public void UpdateOnBuildControl(H action)
    {
        if (action == H.Remove)
        {
            //is needed here otherwise router.cs might detyected and then give a null ref 
            //AssignLayer(0);//Default
            RemovePeople();
        }
        BuildingPot.Control.EditBuildRoutine(MyId, action, HType);
    }

    private int countRemoves;//bz it makes a loop 
    void RemovePeople()
    {
        //Instruction = H.WillBeDestroy;
        if (PeopleDict.Count == 0 && countRemoves == 0)//no one is registered on the build
        {
            countRemoves++;
            DestroydHiddenBuild();
        }
    }

    #endregion

    #region Layer Manangement
    //When is placing the building needs to have the default layer so the router wont notice any of those obj on scene
    //then when is placed on scene should be restored back to the prefab layer that was ....
    //for building we are using "PersonBlock" layer 

    int prefabLayer;//initial layer for prefab
    /// <summary>
    /// The routinte to initialize layer
    /// </summary>
    void LayerRoutine(string command)
    {
        if (IsLoadingFromFile){return;}//if is loading from file doesnt need to change anything 
        if (command == "init")
        {
            GrabPrefabLayer();
            //AssignLayer(0);//default
        }
        else if (command == "done")
        {
            //AssignLayer(prefabLayer);//restore layer to initial one
        }
    }

    void GrabPrefabLayer()
    {
        prefabLayer = gameObject.layer;
    }


    #endregion

    public void DestroydHiddenBuild()
    {
        //the invetory needs to be empty to be destroyed  
        if (Inventory != null && !Inventory.IsEmpty() )
        {
            return;
        }

        //was CancelDestroy
        if (BuildingPot.Control.DispatchManager1.DoIHaveAnyOrderOnAnyDispatch(this) || Instruction!=H.WillBeDestroy)
        {
            return;
        }

        //BuildingPot.Control.Registro.RemoveFromAllRegFile(MyId);
        //BuildingPot.Control.EditBuildRoutine(MyId, H.Remove, HType);
        PositionFixed = false;
        _isOrderToDestroy = true;

        //farms have not decoration
        if (_decoration != null)
        {
            _decoration.RemoveFromBatchMesh();
        }

        //getting the Main GameObject render back
        Program.gameScene.BatchRemove(this);
        DestroyOrdered();

        //so people can Reroutes if new build fell in the midle of one
        PersonPot.Control.Queues.AddToDestroyBuildsQueue(Anchors, MyId);
        BuildingPot.Control.Registro.RemoveFromDestroyBuildings(this);

    }

    /// <summary>
    /// Im adding a box collider to the Router.cs will work witthout much changes
    /// </summary>
    public void AddBoxCollider(RegFile refFile)
    {
        gameObject.layer = 10;//person bloick layer

        float xDim = Mathf.Abs(refFile.Min.x - refFile.Max.x) + refFile.TileScale.x;
        float zDim = Mathf.Abs(refFile.Min.z - refFile.Max.z) + refFile.TileScale.z;
        Vector3 newScale = new Vector3(xDim, 5f, zDim);

        Vector3 center = (refFile.Min + refFile.Max) / 2;
        gameObject.AddComponent<BoxCollider>();

        BoxCollider b = gameObject.GetComponent<BoxCollider>();
        if (b != null)
        {
            b.size = newScale;
            b.transform.position = center;
            //b.center = center;
            //print(newScale+".newScale");
        }
    }

    /// <summary>
    /// Created for brdiges
    /// 
    /// It only adds the box coll if has abosoultuy not colliders 
    /// </summary>
    /// <param name="minP">In our poly system, if u pass a poly will need be input poly[1]</param>
    /// <param name="maxP">This needs input poly[3]</param>
    public void AddBoxCollider(Vector3 minP, Vector3 maxP)
    {
        if (gameObject.GetComponent<BoxCollider>() != null)
        {return;}

        gameObject.layer = 10;//person bloick layer

        float xDim = Mathf.Abs(minP.x - maxP.x) ;
        float zDim = Mathf.Abs(minP.z - maxP.z) ;
        Vector3 newScale = new Vector3(xDim, 5f, zDim);

        Vector3 center = (minP + maxP) / 2;

        gameObject.AddComponent<BoxCollider>();

        BoxCollider b = gameObject.GetComponent<BoxCollider>();
        if (b != null)
        {
            b.size = newScale;
            b.transform.position = center;
            //b.center = center;
            //print(newScale+".newScale");
        }
    }


    #region Construction

    private float constructionAmt;
    private float amtNeeded;
    BuildStat buildStat = new BuildStat();


    internal float PercentageBuilt()
    {
        return (constructionAmt / amtNeeded) * 100;
    }

    private float oldPercent =0f;
    internal string PercentageBuiltCured()
    {
        var percent = PercentageBuilt();
        if (float.IsNaN(percent))
        {
            SetConstructionPercent("0%");
            return "0";
        }

        if (oldPercent != percent)
        {
            oldPercent = percent;
            SetConstructionPercent(percent.ToString("N0") + "%");
        }

        return percent.ToString("N0");
    }


    public bool IsFullyBuilt()
    {
        if (MyId.Contains("Road"))
        {
            //so user will never be able to be removed 
            return false;
        }

        if (!MyId.Contains("Bridge"))
        {
            var st = (Structure)this;
            if (st.CurrentStage == 4)
            {
                return true;
            }
        }
        //addres bridge 
        else
        {
            var bridge = (Bridge)this;
            if (bridge.StartingStageForPieces == H.Done)
            {
                return true;
            }
        }
        return false;
    }


    //todo call 
    public void AddToConstruction(float amt, Person person=null)
    {
        DefineAmtNeeded();
        constructionAmt += (int)amt;
        CheckIfNewStageOrDone(person);

        //so updates that if needed
        PercentageBuiltCured();
    }

    /// <summary>
    /// Defined how much amout of work is needed get this building fullybuilt
    /// </summary>
    private void DefineAmtNeeded()
    {
        buildStat = Book.GiveMeStat(HType);
        amtNeeded = buildStat.AmountOfLabour;
    }

    /// <summary>
    /// Will determnine if a new stage was reached or the building is fully built 
    /// </summary>
    private void CheckIfNewStageOrDone(Person person)
    {
        var sP = ReturnCurrentStructureParent();
        var amtPerStage = amtNeeded/4;
        //is contruction more advandec that current stage 
        bool isBigger = sP.CurrentStage < constructionAmt/amtPerStage;

        // The bridge need to use a 'if' bz the new stage show call is async. Everytin else uses  a 'while;
        if (MyId.Contains("Bridge"))
        {
            if (isBigger && sP.CurrentStage != 4)
            {
                ShowNextStage();
            }
        }
        else
        {
            while(isBigger && sP.CurrentStage != 4)
            {
                isBigger = sP.CurrentStage < constructionAmt / amtPerStage;
                ShowNextStage();
            }
        }

        //if is Done
        if (sP.CurrentStage == 4)
        {
            HandleLastStage(person);
        }
    }

    /// <summary>
    /// Crated for modularity. And reutnr the indicate Structure Parent . if is a bridge will return the first piece
    /// </summary>
    /// <returns></returns>
    public StructureParent ReturnCurrentStructureParent()
    {
        StructureParent sP = null;

        //if is bridge will look at the first pieces stages 
        if (HType.ToString().Contains(H.Bridge.ToString()))
        {
            var br = (Bridge)this;
            sP = br.Pieces[0];
        }
        else
        {
            sP = (StructureParent) this;
        }
        return sP;
    }

    /// <summary>
    /// Created to modularity. And Handle Structures and Brdiges
    /// 
    /// </summary>
    void ShowNextStage()
    {
        if (HType.ToString().Contains(H.Bridge.ToString()))
        {
            Bridge b = this as Bridge;
            b.ShowNextStageOfParts();
        }
        else
        {
            StructureParent sP = (StructureParent)this;
            sP.ShowNextStage();
        }
    }


    private General _debugPercentage;
    void SetConstructionPercent(string newVal)
    {
        var sp = this as StructureParent;

        if (sp == null || sp.CurrentStage == 4)
        {
            return;
        }

        if (MyId.Contains("Unit") || MyId.Contains("Box"))
        {
            return;
        }

        if (_debugPercentage != null)
        {
            _debugPercentage.Destroy();
        }

        _debugPercentage = UVisHelp.CreateText(transform.position + new Vector3(0, 1f, 0), newVal);
        _debugPercentage.transform.SetParent(transform);
    }

    bool didBuiltNotify;
    protected General _construcionSign;
    /// <summary>
    /// Created for modularity. Handles all things related onces the building is fully built 
    /// </summary>
    protected void HandleLastStage(Person person=null)
    {
        _lastStageTime = Time.time;
        DestroyAllPlacedFX();

        //Debug.Log("construction built 100%:"+MyId+"." + Program.gameScene.GameTime1.TodayYMD());
        PersonPot.Control.RoutesCache1.RemoveAllMine(MyId);
        Quest();

        PersonPot.Control.OnBuildDone(EventArgs.Empty);

        //bz when demolishes adds 10,000
        if (constructionAmt < 9500)
        {
            if (!didBuiltNotify && !IsLoadingFromFile)
            {
                didBuiltNotify = true;
                Program.gameScene.GameController1.NotificationsManager1.Notify("Built", HType + "");
            } 
        }
        if (_debugPercentage != null)
        {
            _debugPercentage.Destroy();
        }
        if (IsNaval())
        {
            BuildingPot.Control.DockManager1.AddToDockStructure(MyId, HType);
        }
        if (person!=null)
        {
            person.Work.BuildersManager1.RemoveConstruction(MyId);
        }
        if (_construcionSign != null)
        {
            _construcionSign.Destroy();
            _construcionSign = null;
        }
        //if is a Unit from a bridge doesnt need to be added there 
        //Bridge bz needs to be called when all bridge elements are spanwed
        if (HType.ToString().Contains(H.Unit.ToString()))
        {
            return;
        }
        UpdateOnBuildControl(H.Add);
        
        //needs to be called here other wise Dormant Orders will not become active
        InitStorage();
        Program.gameScene.BatchAdd(this);

        Program.MouseListener.MStatsAndAchievements.CheckOnManualAchievements(HType+"");
    }

    #endregion

    #region LandZoning

    private bool landZoneLoaded;
    /// <summary>
    /// Loads the land Zone and adds the Crystals of this Building to Crystal Manager 
    /// </summary>
    void LandZoneLoader()
    {
        if (landZoneLoaded || !PositionFixed || !IsLoadingFromFile
            //|| XMLSerie.TownLoaded
            )
        {
            return;
        }
        landZoneLoaded = true;

        //so it can add the corners on CrystalManager
        Anchors = GetAnchors();
        //UVisHelp.CreateHelpers(Anchors, Root.largeBlueCube);
        MeshController.CrystalManager1.Add(this);
    }


    /// <summary>
    /// bz could have being must likely saved in another Map woith other landZones 
    /// </summary>
    protected void HandleSavedTownBuilding()
    {
        //this is only needed for the initial Loaded Town
        if (Category != Ca.Structure)
        {
            return;
        }

        if (TownLoader.TownLoaded)
        {
            Anchors.Clear();
            _polyOnGrid.Clear();

            //Debug.Log("townLoaded:" + MyId);
            Anchors = GetAnchors();
            
            LandZone1.Clear();
            HandleLandZoning();
            TownLoader.NewBuildingLoaded();
        }
    }

    /// <summary>
    ///  The last step of the buils th are loaded with the town
    /// 
    /// so MarkTerraSpawnRoutine is called and Spawner terrains around this building disappear
    ///
    /// </summary>
    public void TownBuildingLastStep()
    {
        MarkTerraSpawnRoutine(10, from: transform.position);//20

        if (!HType.ToString().Contains("Storage"))
        {
            return;
        }

        // if is a Storage then the inv should be clean and fill with current difficulty
        Inventory.Delete();
        InitStorage();
    }


    protected void PrivHandleZoningAddCrystals()
    {
        HandleLandZoning();
        //UVisHelp.CreateHelpers(Anchors, Root.yellowCube);
        MeshController.CrystalManager1.Add(this);
    }

    /// <summary>
    /// Can only be called when brdige is being current spawn in this session 
    /// bz uses var that are not saveLoad
    /// </summary>
    protected void PrivHandleZoningAddCrystalsForBridge()
    {
        LandZoningBridge();
        MeshController.CrystalManager1.Add(this);
    }

    private bool isToFindLandZone;
    /// <summary>
    /// This will return the land zone a given point of a build is
    /// 
    /// for normal buildins we need to know the door.
    /// 
    /// For bridges . Both bottom ends 
    /// </summary>
    public void HandleLandZoning()
    {
        if (!MyId.Contains("Bridge"))
        {
            var sp = ReturnCurrentStructureParent();
            var spawnPoint = MoveSpawnPointAwayFromBuildingIfShoreBuild(sp);

            var landZonName = MeshController.CrystalManager1.ReturnLandingZone(spawnPoint);

            LandZone1.Add(new VectorLand(landZonName, sp.SpawnPoint.transform.position));
        }
    }

    /// <summary>
    /// Bz structures that are close to shores in some conditions cant get a LinkRect to link 
    /// then I will push the SpawnPoint pos a bit away from building
    /// </summary>
    /// <param name="spPoint"></param>
    /// <returns></returns>
    Vector3 MoveSpawnPointAwayFromBuildingIfShoreBuild(StructureParent sp)
    {
        //if is nota shore building will return spawn point 
        if (!Builder.IsAShoreBuilding(sp))
        {
            return sp.SpawnPoint.transform.position;
        }

        //else will move it away from builidng 
        var spawnPnt = sp.SpawnPoint.transform.position;
        return Vector3.MoveTowards(spawnPnt, sp.transform.position, -5);
    }

    /// <summary>
    /// To define the landzone of a dummy by geetting the LandZone name from the c'onstructing' and
    /// the position
    /// 
    /// Useful to dummy spawns in Corners of a bulding that we know already the landzone
    /// </summary>
    /// <param name="constructing"></param>
    /// <param name="position"></param>
    internal void HandleLandZoning(Building constructing, Vector3 position)
    {
        LandZone1.Add(new VectorLand(constructing.LandZone1[0].LandZone, position));
    }

    /// <summary>
    /// Is made public so when is loding is called 
    /// </summary>
    void LandZoningBridge()
    {
        Bridge br = (Bridge)this;
        var ends = br.GiveTwoRoughEnds();

        //will move the ends a bit away from buidliing so if the bridge is too close
        //to rivers edges can link to the LinkRects are deeper in land 
        var end0 = Vector3.MoveTowards(ends[0], transform.position, -8);
        var end1 = Vector3.MoveTowards(ends[1], transform.position, -8);

        var zone0 = MeshController.CrystalManager1.ReturnLandingZone(end0);
        var zone1 = MeshController.CrystalManager1.ReturnLandingZone(end1);

        //bz they were being save loaded in Poly Anchors
        //this is really pointless bz somehow if u move the bottom gameObj in Part12 of brdigeTrails works 
        var end0bit = Vector3.MoveTowards(ends[0], transform.position, HowFarPush());//0
        var end1bit = Vector3.MoveTowards(ends[1], transform.position, HowFarPush());

        LandZone1.Add(new VectorLand(zone0, end0bit));
        LandZone1.Add(new VectorLand(zone1, end1bit));
    }

    float HowFarPush()
    {
        //if (HType == H.BridgeRoad)
        //{
        //    return -25;
        //}
        return 0;
    }

    #endregion



    #region Production

    //BuildingWindow.cs will reload inv
    private bool _isToReloadInv;

    /// <summary>
    /// The Prefered storage where this Structue production should be taken to
    /// </summary>


    /// <summary>
    /// Will find out if pass has a val if does will return so. other wise CurrProd
    /// </summary>
    /// <param name="pass"></param>
    /// <returns></returns>
    P DefineProdHere(P pass)
    {
        if (pass != P.None)
        {
            return pass;
        }
        return CurrentProd.Product;
    }
    
    /// <summary>
    /// Produce what this Building is set to. ex Fisherman produce fish
    /// 
    /// 'amt' the amount the person calling this can produce in a shift 
    /// </summary>
    internal void Produce(float amt, Person person, bool addToBuildInv = true, P prod = P.None)
    {
        P prodHere = DefineProdHere(prod);
        if (prodHere == P.Stop)
        {
            return;
        }

        DecideIfReloadInventoryWithThisProduction(prodHere);

        SomethingWasProduce();

        var doIHaveInput = DoBuildHaveRawResources();
        var hasStorageRoom = DoesStorageHaveCapacity();
        var hasThisBuildRoom = DoWeHaveCapacityInThisBuilding();

        if (doIHaveInput && (hasStorageRoom || hasThisBuildRoom))
        {
            amt = ConsumeInputs(amt);
            SmokePlay(true);

            //if is a farm
            if (MyId.Contains("Farm"))
            {
                var farm = (Structure)this;
                farm.AddWorkToFarm();
            }
            else if (hasThisBuildRoom && addToBuildInv && !MyId.Contains("Farm"))
            {

                Inventory.Add(prodHere, amt);
                AddProductionThisYear(prodHere, amt);
            }
            else if (!hasThisBuildRoom && hasStorageRoom && addToBuildInv && !MyId.Contains("Farm"))
            {
                person.Inventory.Add(prodHere, amt);
                AddProductionThisYear(prodHere, amt);

            }
            else if (!addToBuildInv && !MyId.Contains("Farm"))
            {
                person.Inventory.Add(prodHere, amt);
                AddProductionThisYear(prodHere, amt);
            }
        }
        else if (!hasStorageRoom && !hasThisBuildRoom && person.FoodSource != null)
        {
            //todo show 3d icon
            AddEvacuationOrderOfProdThatAreNotInput();
            //Debug.Log("Both full" + person.FoodSource.MyId + ".and." + MyId + " AddEvacuationOrder() called");
        }
        //if doesnt have input will see if can get anything out of that buliding that is not an inpput
            //product 
        else if (!doIHaveInput)
        {
            //todo show 3d icon
            //Debug.Log(MyId+" doesnt have input");
            var prodToEvac = BuildingPot.Control.ProductionProp.ReturnProductsOnInvThatAreNotInput(Inventory, HType);

            for (int i = 0; i < prodToEvac.Count; i++)
            {
                var got = Inventory.RemoveByWeight(prodToEvac[i], person.HowMuchICanCarry());
                person.Inventory.Add(prodToEvac[i], got);
                return;
            }
        }
        
        //if has more thn 2000Kg of current prd can add Evac order as weell
        if (Inventory.ReturnAmtOfItemOnInv(_currentProd.Product) > 2000)
        {
            //todo show 3d icon
            AddEvacuationOrderOfProdThatAreNotInput();
        }
        if (hasStorageRoom && !hasThisBuildRoom && person.FoodSource != null)
        {
            //todo show 3d icon
            AddEvacuationOrderOfProdThatAreNotInput();
            //Debug.Log("Building store full" + MyId + " AddEvacuationOrder() called. " + person.MyId);
        }
    }

    #region Production INput

    /// <summary>
    /// Consumre the inputs needed to create the current product 
    /// 
    /// Return what actually was produced based on the input
    /// </summary>
    private float ConsumeInputs(float amtToProd)
    {
        //the ones dont need inputs
        if (CurrentProd.Ingredients == null || CurrentProd.Ingredients.Count == 0)
        {
            return amtToProd;
        }

        List<float> howMuchCanProduce = new List<float>();
        for (int i = 0; i < CurrentProd.Ingredients.Count; i++)
        {
            var ingredient = CurrentProd.Ingredients[i];
            howMuchCanProduce.Add(HowMuchCanProduce(ingredient, amtToProd));
        }

        //order by lowest.
        //bz if we have 100kg of clay and 50kk of wood we can only produce
        //50kg of brick bz the limit is set by the clay
        howMuchCanProduce = howMuchCanProduce.OrderBy(a=>a).ToList();
        ProduceThisKGAndRemove(howMuchCanProduce[0]);

        return howMuchCanProduce[0];
    }

    /// <summary>
    /// Wil produce that exact amout of KG bz was found that that was
    /// the max amt we could produce with current inputs
    /// </summary>
    /// <param name="p"></param>
    private void ProduceThisKGAndRemove(float p)
    {
        for (int i = 0; i < CurrentProd.Ingredients.Count; i++)
        {
            var ingredient = CurrentProd.Ingredients[i];
            var kg = ingredient.Units*p;
            Inventory.RemoveByWeight(ingredient.Element, kg);
        }
    }

    /// <summary>
    /// Will find out how much can produce of a Brick for ex
    /// Will see how much Clay is, ex if is 100kg clay then can produce only 50kg brick
    /// And if there is 50kg of wood will be able to produce 500kg of brick.
    /// 
    /// Then the restiction will be imposed by the amot of clay on inventory and that
    /// will be what at the end will be produced by the worker 
    /// </summary>
    /// <param name="ingredient"></param>
    /// <param name="amtToProd"></param>
    /// <returns></returns>
    float HowMuchCanProduce(InputElement ingredient, float amtToProd)
    {
        // 100 * 2 for Brick for ex = 200. he needs 200KG of clay to produce 100kg of brick
        // 
        var kgNeeded = amtToProd * ingredient.Units;
        var onInv = Inventory.ReturnAmtOfItemOnInv(ingredient.Element);

        //will return wht we need if can be covered 
        if (onInv > kgNeeded)
        {
            return kgNeeded / ingredient.Units;
        }
        //otherwise what is on inv
        return onInv / ingredient.Units;
    }



    #endregion


    #region Reload Inventory

    int oldYear = -1;

    /// <summary>
    /// Called every time something is produced
    /// </summary>
    void SomethingWasProduce()
    {
        if (oldYear != Program.gameScene.GameTime1.Year)
        {
            oldYear = Program.gameScene.GameTime1.Year;
            _isToReloadInv = true;
        }
    }

    /// <summary>
    /// For be use at least the first time a product is produced 
    /// </summary>
    /// <param name="prod"></param>
    void DecideIfReloadInventoryWithThisProduction(P prod)
    {
        if (!Inventory.Contains(prod))
        {
            ReloadInventory();
        }
    }

    /// <summary>
    /// Called when a new Product is selected to be produced in the building
    /// </summary>
    protected void ReloadInventory()
    {
        oldYear = -1;
        _isToReloadInv = true;
    }

    internal bool IsToReloadInv()
    {
        if (_isToReloadInv)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Once is used to reload the inv ,, _isToReloadInv be set to False 
    /// </summary>
    public void InvWasReloaded()
    {
        _isToReloadInv = false;
    }

    #endregion

    #region Production Reporting (for report purposes)

    ProductionReport _productionReport;
    public ProductionReport ProductionReport
    {
        get { return _productionReport; }
        set { _productionReport = value; }
    }

    private void AddProductionThisYear(P p, float amt)
    {
        if (_productionReport == null)
        {
            _productionReport = new ProductionReport();
        }

        Quest(amt, p);


        _productionReport.AddProductionThisYear(p, amt);
        BulletinWindow.AddProduction(p, amt, "Prod");
    }  
    
    public void AddConsumeThisYear(P p, float amt)
    {
        if (_productionReport == null)
        {
            _productionReport = new ProductionReport();
        }

        _productionReport.AddConsumeThisYear(p, amt);
        BulletinWindow.AddProduction(p, amt, "Consume");
    }

#endregion

    /// <summary>
    /// Use for plants lie Corn to add produced ammt
    /// </summary>
    /// <param name="amt"></param>
    internal void Produce(float amt)
    {
        var doIHaveInput = DoBuildHaveRawResources();
        var hasThisBuildRoom = DoWeHaveCapacityInThisBuilding();

        if (doIHaveInput && hasThisBuildRoom)
        {
            Inventory.Add(CurrentProd.Product, amt);
            AddProductionThisYear(CurrentProd.Product, amt);
            DecideIfReloadInventoryWithThisProduction(CurrentProd.Product);
        }
        else if (!hasThisBuildRoom)
        {
            AddEvacuationOrderOfProdThatAreNotInput();
            //           //Debug.Log("Both full" + person.FoodSource.MyId + ".and." + MyId);
        }
        else if (!doIHaveInput)
        {
            //todo show 3d icon
            //Debug.Log(MyId + " doesnt have input");
        }
    }

    private void Quest(float amt = 0, P newProduct = P.None)
    {
        if (CurrentProd.Product == P.Bean && HType == H.FieldFarmSmall)
        {
            Program.gameScene.QuestManager.AddToQuest("FarmProduce", amt);
        }

        if (CurrentProd.Product == P.Weapon && HType == H.BlackSmith && amt > 0)
        {
            Program.gameScene.QuestManager.AddToQuest("WeaponsProduce", amt);
        }

        if (HType == H.FieldFarmSmall && _maxPeople == 2)
        {
            Program.gameScene.QuestManager.QuestFinished("FarmHire");
        }

        if (HType == H.Dock && _maxPeople == 1)
        {
            Program.gameScene.QuestManager.QuestFinished("HireDocker");
        }
        if (HType == H.BlackSmith && _maxPeople == 2)
        {
            Program.gameScene.QuestManager.QuestFinished("BlackSmithHire");
        }

        if (newProduct == P.Weapon && HType == H.BlackSmith)
        {
            Program.gameScene.QuestManager.QuestFinished("ChangeProductToWeapon");
        }

        //called from Handle Last stage quest, tuto
        //bz when demolishes adds 10,000
        if (constructionAmt < 9500)
        {
            if (HType == H.Dock)
            {
                Program.gameScene.TutoStepCompleted("FinishDock.Tuto");
            }
            else if (HType == H.FieldFarmSmall)
            {
                Program.gameScene.QuestManager.QuestFinished("SmallFarm");
            }
            else if (HType == H.Shack)
            {
                Program.gameScene.QuestManager.QuestFinished("Shack");
            }
            else if (HType == H.HeavyLoad)
            {
                Program.gameScene.QuestManager.QuestFinished("HeavyLoad");
            }
            else if (HType == H.StandLamp)
            {
                Program.gameScene.QuestManager.QuestFinished("Lamp");
            }
            else if (HType == H.BlackSmith)
            {
                Program.gameScene.QuestManager.QuestFinished("Production");
            }
        }
    }

 

    /// <summary>
    /// If is all full an evacuation order is add to Dispatch so at least this 
    /// building room will be clear .
    /// 
    /// When this starts to happen over and over again is when u want to start to Export this Product
    /// In the Port should be a Dispatch to Export and Import 
    /// </summary>
    public void AddEvacuationOrderOfProdThatAreNotInput()
    {
        var prodToEvac = BuildingPot.Control.ProductionProp.ReturnProductsOnInvThatAreNotInput(Inventory, HType);

        for (int i = 0; i < prodToEvac.Count; i++)
        {
            Order t = new Order(prodToEvac[i], "", MyId);
            AddToClosestWheelBarrowAsOrder(t, H.Evacuation);

        }
    }



    float lastNoti;
    /// <summary>
    /// Will tell worker if can take products out of the biulding
    /// 
    /// Used to express if a person can take goods out of building to a Storage or should leave it here in this building 
    /// </summary>
    /// <returns></returns>
    public bool CanTakeItOut(Person person)
    {
        var res = (person.FoodSource != null && DoesStorageHaveCapacity());

        if (!res && (lastNoti == 0 || Time.time > lastNoti + NotificationsManager.NotiFrec)
            && DoBuildHaveRawResources())
        {
            Program.gameScene.GameController1.NotificationsManager1.
                Notify("CantProduceBzFullStore", person.Name + " " + Languages.ReturnString("cannot produce")
                + " " + CurrentProd.Product);
            lastNoti = Time.time;
        }

        return res;
    }


    float lastNoti2;
    /// <summary>
    /// For the buildings that need raw products as an input for the output will will tell u if 
    /// has input enough or not 
    /// </summary>
    /// <returns></returns>
    public bool DoBuildHaveRawResources()
    {
        var res = BuildingPot.Control.ProductionProp.DoIHaveEnoughOnInvToProdThis(this);

        if (!res && (lastNoti2 == 0 || Time.time > lastNoti2 + NotificationsManager.NotiFrec))
        {
            Program.gameScene.GameController1.NotificationsManager1.
              Notify("NoInput", CurrentProd.Product+"");
            lastNoti2 = Time.time;
        }

        return res;
    }

    /// <summary>
    /// Will tell u if a Storage has enoguh capacity to hold this new amt of goods
    /// </summary>
    /// <returns></returns>
    bool DoesStorageHaveCapacity()
    {
        DefinePreferedStorage();


        return PreferedStorage != null && !PreferedStorage.Inventory.IsFull();
    }


    #region Preferred Storage
    private Structure _preferedStorage;
    public Structure PreferedStorage
    {
        get
        {
            if (_preferedStorage == null)
            {
                DefinePreferedStorage();
            }
            return _preferedStorage;
        }
        set { _preferedStorage = value; }
    }

    List<string>oldFoodSrcs= new List<string>(); 
    /// <summary>
    /// Define the closest storage that its inventory is not full
    /// 
    /// Must be redifined  if new Storage is added to the game 
    /// </summary>
    private void DefinePreferedStorage()
    {
        if (BuildingPot.Control.FoodSources.Count == 0 || !Program.gameScene.GameFullyLoaded())
        {
            return;
        }

        if (_preferedStorage == null || _preferedStorage.Inventory.IsFull())
        {
            PreferedStorageSerach();
        }
        else if (oldFoodSrcs != BuildingPot.Control.FoodSources)
        {
            //search again for the closest 
            PreferedStorageSerach();
        }
    }

    void PreferedStorageSerach()
    {
        oldFoodSrcs.Clear();
        //masonry will check the closest
        if (HType == H.Masonry)
        {
            _preferedStorage =
                BuildingController.FindTheClosestOfContainTypeFullyBuilt("Storage", transform.position, true);

            //is null bz was only one then 
            if (_preferedStorage == null)
            {
                _preferedStorage =
                    BuildingController.FindTheClosestOfContainTypeFullyBuilt("Storage", transform.position, false);
            }
        }
        //other else strucutres will select the Prefered Storage of their closest Masory 
        //as wheelBarrowers always go back to mansory makes sense to use their closer Storage
        else
        {
            _preferedStorage =
            BuildingController.FindTheClosestOfContainTypeFullyBuilt("Storage", transform.position, false);
        }
        oldFoodSrcs.AddRange(BuildingPot.Control.FoodSources);
    }

#endregion


    /// <summary>
    /// Will tell u if a this building has enoguh capacity to hold this new amt of goods
    /// 
    /// All buildings have small storage 
    /// </summary>
    /// <returns></returns>
    bool DoWeHaveCapacityInThisBuilding()
    {
        return !Inventory.IsFull(); 
    }

    /// <summary>
    /// Based on current inventory will return a list of the products more needed for current
    /// type of production
    /// </summary>
    /// <returns></returns>
    public List<P> OrderedListOfInputNeeded()
    {
        var list = Inventory.InventItems.OrderByDescending(a => a.Amount).ToList();
        var ingredientsNeeded = BuildingPot.Control.ProductionProp.ReturnIngredients(CurrentProd.Product);

        List<P> res = new List<P>();

        for (int i = 0; i < list.Count; i++)
        {
            for (int j = 0; j < ingredientsNeeded.Count; j++)
            {
                if (list[i].Key == ingredientsNeeded[j].Element)
                {
                    res.Add(list[i].Key);
                } 
            }
        }

        return res;
    }

    /// <summary>
    /// Will return true if Building has 'prod' on inventory
    /// </summary>
    public bool HaveThisProdOnInv(P prod)
    {
        for (int i = 0; i < Inventory.InventItems.Count; i++)
        {
            if (Inventory.InventItems[i].Key == prod)
            {
                return true;
            }
        }
        return false;
    }


    /// <summary>
    /// This is the one will add order to the Dispatch if dont have the Raw input
    /// </summary>
    void CheckIfOrdersAreNeeded()
    {
        //only order inpu twhen is has workers
        if (PeopleDict.Count==0 || Instruction==H.WillBeDestroy)
        {
            return;
        }

        var rawsOnNeed = BuildingPot.Control.ProductionProp.ReturnIngredients(CurrentProd.Product);
        if (rawsOnNeed == null)
        {
            return;
        }

        var doIHaveInput = DoBuildHaveRawResources();

        for (int i = 0; i < rawsOnNeed.Count; i++)
        {
            P prod = rawsOnNeed[i].Element;
            //so for nails for example for a Furnitrue will only order 0.2 x 30 = 6kg
            var amtNeeded = rawsOnNeed[i].Units * 10;

            if (!HaveThisProdOnInv(prod) || !doIHaveInput)
            {
                //todo use 10000 to put a large number of units needed
                Order prodNeed = new Order(prod, MyId, amtNeeded);//300

                //BuildingPot.Control.Dispatch1.AddToOrders(prodNeed);
                AddToClosestWheelBarrowAsOrder(prodNeed, H.None);

                AddOrderToOurWorkers(prodNeed);
            }
        }
    }

    /// <summary>
    /// Now all workers will bring input if its needed 
    /// </summary>
    /// <param name="prodNeed"></param>
    private void AddOrderToOurWorkers(Order prodNeed)
    {
        for (int i = 0; i < PeopleDict.Count; i++)
        {
            var pers = Family.FindPerson(PeopleDict[i]);
            pers.AddWorkInputOrder(prodNeed);
        }
    }







    /// <summary>
    /// Will find closest WheelBarrow office from here and will add the order 
    /// </summary>
    /// <param name="order"></param>
    public void AddToClosestWheelBarrowAsOrder(Order order, H typeOfOrder)
    {
        var closest = FindClosestWheelBarrowerAndHeavyLoad();

        for (int i = 0; i < closest.Count; i++)
        {
            AddOrderToBuild(order, typeOfOrder, closest[i]);
        }
    }

    void AddOrderToBuild(Order order, H typeOfOrder, Structure closest)
    {
        //only for debug bz a WheelBarrow always should be up
        if (closest == null)
        {
            //todo Notify not wheelBarrow close enought to me 3d icon
            Debug.Log("Not Masonry close enought to " + MyId + " found");
            return;
        }

        if (typeOfOrder == H.None)
        {
            closest.Dispatch1.AddToOrdersToWheelBarrow(order);
        }
        else if (typeOfOrder == H.Evacuation)
        {
            closest.Dispatch1.AddEvacuationOrderToWheelBarrow(order);
        }
    }










    private bool evacAll;
    /// <summary>
    /// Bz when a building is set to be destroyed u need to remove all items on it 
    /// 
    /// this mtehod can be only once
    /// </summary>
    internal void AddToClosestWheelBarrowAsOrderEvacuateAllInv()
    {
        //created to address when destroyig a building adddng the same order twice 
        if (evacAll)
        {
            return;
        }

        var closest = FindClosestWheelBarrowerAndHeavyLoad();

        for (int i = 0; i < closest.Count; i++)
        {
            AddEvaToAllInv(closest[i]);
        }
    }

    void AddEvaToAllInv(Structure closest)
    {
        //only for debug bz a WheelBarrow always should be up
        if (closest == null)
        {
            //todo Notify not wheelBarrow close enought to me 3d icon
            Debug.Log("Not Masonry close enought to " + MyId + " found");
            return;
        }

        evacAll = true;
        var orders = Inventory.CreateOrderToEvacWholeInv();

        for (int i = 0; i < orders.Count; i++)
        {
            closest.Dispatch1.AddEvacuationOrderToWheelBarrow(orders[i]);
        }
    }









    /// <summary>
    /// If amount is less than 500kg will only look for WheelBarrowOffice but if is bigger
    /// will try to find a HeavyLoad around
    /// 
    /// Here loader will be called if load is bigger thn 1000KG
    /// Here heavyloader will be called if load is bigger thn 2000KG
    /// </summary>
    /// <returns></returns>
    List<Structure> FindClosestWheelBarrowerAndHeavyLoad()
    {
        var wheel = BuildingController.FindTheClosestOfThisType(H.Masonry, transform.position, Brain.Maxdistance);
        //var loader = BuildingController.FindTheClosestOfThisType(H.Loader, transform.position, Brain.Maxdistance);
        var heavy = BuildingController.FindAllStructOfThisType(H.HeavyLoad);

        var res = new List<Structure> { wheel };

        //if (Inventory.CurrentKGsOnInv() > 000 && loader != null)//1000
        //{
        //    res.Add(loader);
        //} 
        if (Inventory.CurrentKGsOnInv() > 000 && heavy != null)//2000
        {
            res.AddRange(heavy);
        }

        return res;
    }




    private IEnumerator ThirtySecUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(10, 20)); // wait
            CheckIfOrdersAreNeeded();


        }
    }

    /// <summary>
    /// Will be called when WheelBarrower pick phisically product in there 
    /// </summary>
    public void CheckIfCanBeDestroyNow(P prod)
    {
        if (Instruction == H.WillBeDestroy && !Inventory.IsItemOnInv(prod))
        {
            DestroydHiddenBuild();
        }
    }

    #endregion

    private BuildersManager _buildersManager;
    void InitWheelBarrow()
    {
        if (IsLoadingFromFile)
        {
            return;
        }

        if (HType == H.HeavyLoad || HType == H.Masonry)
        {
            _dispatch = new Dispatch();

            if (HType == H.Masonry)
            {
                _buildersManager = new BuildersManager(this);
            }
        }
    }

    #region Job Related

    /// <summary>
    /// Will say if this building can hire or fire a new person
    /// </summary>
    /// <param name="newEmploys">This will be decresead by one towards 0 if was able to hire or fire
    /// If was able to hire. For ex a 3 was passed and a 2 is return
    /// If was able to fire. For ex a -3 is passed and a -2 is return</param>
    /// <returns></returns>
    internal int CanYouChangeOne(int newEmploys)
    {
        //need to fire people
        if (newEmploys < 0 && _maxPeople > 0)
        {
            ChangeMaxAmoutOfWorkers("Less");
            return newEmploys + 1;//one less that needs to be fire 
        }
        else if(newEmploys > 0 && _maxPeople < AbsMaxPeople)
        {
            ChangeMaxAmoutOfWorkers("More");
            return newEmploys - 1;//one less that needs to be hire
        }
        //else return the same amount
        return newEmploys;
    }


    private int _maxPeople;//max people this builging can hold. workers this one can change
    private int _absMaxPeople;//this one doesnt change 

    public int AbsMaxPeople
    {
        get { return _absMaxPeople; }
        set { _absMaxPeople = value; }
    }

    public int MaxPeople
    {
        get { return _maxPeople; }
        set { _maxPeople = value; }
    }

    internal string ChangeMaxAmoutOfWorkers(string action)
    {
        if (action == "Less")
        {
            _maxPeople--;
            ManagerReport.AddInput("Less workers on: " + transform.name + ". now:" + _maxPeople);

        }
        else if (action == "More" && MyText.Lazy() > 0)
        {
            _maxPeople++;
            ManagerReport.AddInput("More workers on: " + transform.name + ". now:" + _maxPeople);
            Quest();
        }

        if (_maxPeople < 0)
        {
            _maxPeople = 0;
        }
        if (_maxPeople >= AbsMaxPeople)
        {
            _maxPeople = AbsMaxPeople;
        }

        UpdateWorkersRoutine();
        return _maxPeople + "";
    }



    void UpdateWorkersRoutine()
    {
        //fire people
        FirePeopleIfNeeded();

        CheckIfNoOpenPosLeftThenRemoveFromList();
        CheckIfNeedsToBeAddedToList();

        
    }

    private void FirePeopleIfNeeded()
    {
        var peopleToBeFired = PeopleDict.Count - _maxPeople;

        for (int i = 0; i < peopleToBeFired; i++)
        {
            var index = PeopleDict.Count - (1 + i);//starting from the last towards the first
            var person = Family.FindPerson(PeopleDict[index]);
            person.WasFired = true;
            person.ShowEmotion("Fired");
            PersonPot.Control.RestartControllerForPerson(person.MyId);
        }

        //if not people was to fired then make sure all are hired that are less than MaxPeople and PeopleDict.Count
        if (peopleToBeFired == 0 && Program.gameScene.GameFullyLoaded())
        {
            for (int i = 0; i < MaxPeople && i < PeopleDict.Count; i++)
            {
                var index = i;
                var person = Family.FindPerson(PeopleDict[index]);
                person.WasFired = false;
                //in case had a Input Work Order
                person.Inventory.Delete();

                PersonPot.Control.RestartControllerForPerson(person.MyId);
            }
        }
    }


    private void InitJobRelated()
    {
        AbsMaxPeople = Book.GiveMeStat(HType).MaxPeople;
        MaxPeople = PeopleDict.Count;

        UpdateWorkersRoutine();
    }

    /// <summary>
    /// Called to fill one position in the job place
    /// </summary>
    public void FillPosition()
    {
        CheckIfNoOpenPosLeftThenRemoveFromList();
    }

    /// <summary>
    /// Will check if positions are still open on this job site if not then 
    /// remove from List 
    /// </summary>
    void CheckIfNoOpenPosLeftThenRemoveFromList()
    {
        if (PeopleDict.Count >= _maxPeople)
        {
            BuildingPot.Control.WorkOpenPos.Remove(MyId);
//           //Debug.Log(MyId+" removed from curr Jobs");
        }
    }

    /// <summary>
    /// Called when a person leave this job to find a better one or dies
    /// </summary>
    public void RemovePosition(bool removeMaxAmtWorkers = false)
    {
        if (Instruction == H.WillBeDestroy)
        {
            return;
        }

        if (removeMaxAmtWorkers && MyText.Lazy() == 0)//bz if there are lazy people then we need to hire
        {
            ChangeMaxAmoutOfWorkers("Less");
        }
        CheckIfNeedsToBeAddedToList();
    }

    /// <summary>
    /// Checks if building can be added to the list 
    /// </summary>
    void CheckIfNeedsToBeAddedToList()
    {
        if (!HasOpenPositions())
        {
            return;   
        }

        if (BuildingPot.Control.WorkOpenPos.Contains(MyId))
        {
            return;
        }

        //add to list 
        BuildingPot.Control.WorkOpenPos.Add(MyId);
//       //Debug.Log(MyId + " Added to curr Jobs");
    }

    /// <summary>
    /// Will tell if this Buildign still has Open Positions 
    /// </summary>
    /// <returns></returns>
    internal bool HasOpenPositions()
    {
        return PeopleDict.Count < _maxPeople;
    }




#endregion


    #region Booking

    /// <summary>
    /// Once a person is booked will be moved to family with its family ID
    /// </summary>
    /// <param name="newP"></param>
    /// <param name="familyID"></param>
    public void MovePersonToFamilySpot(Person newP, Structure newHome)
    {
        var familyID = newHome.BookedHome1.Family.FamilyId;
        Family toBeFill = ReturnFamilyByID(familyID);

        //means he is a teen.
        //if he booked here is bz he either fits in an exisitng family
        //or a emptyVirign family exist here 

        //if this is null is bz that family doesnt exist in that building so
        //u can select a virgin family in that building then 
        if (toBeFill == null)
        {
            //toBeFill = newHome.BookedHome1.Family;
            //throw new Exception(newP.MyId + " . " +newHome.MyId);
        }

        //newP.IsBooked = false;
        AssignBookedRole(newP, toBeFill, familyID);
        newP.transform.SetParent(transform);

        //so families are resaved 
        BuildingPot.Control.Registro.ResaveOnRegistro(MyId);
    }

    void AssignBookedRole(Person newP, Family toBeFill, string familyID)
    {
        if (toBeFill == null)
        {
            toBeFill = ReturnEmptyFamily();
            if (toBeFill == null)
            {
                print("toBeFill == nul :" + newP.MyId);
                throw new Exception("At least a family should be Virgin");
            }
        }

        for (int i = 0; i < BookedHome1.Family.Kids.Count; i++)
        {
            if (newP.MyId == BookedHome1.Family.Kids[i])
            {
                toBeFill.AddKids(newP.MyId);
            }
        }
        if (newP.MyId == BookedHome1.Family.Father)
        {
            toBeFill.Father = newP.MyId;
        }
        if (newP.MyId == BookedHome1.Family.Mother)
        {
            toBeFill.Mother = newP.MyId;
        }
    }



    Role FindRoleOnBooking(Person newP)
    {
        for (int i = 0; i < BookedHome1.Family.Kids.Count; i++)
        {
            if (newP.MyId == BookedHome1.Family.Kids[i])
            {
                return Role.Kid;
            }
        }

        if (newP.MyId == BookedHome1.Family.Father)
        {
            return Role.Father;
        }
        if (newP.MyId == BookedHome1.Family.Mother)
        {
            return Role.Mother;
        }
        return Role.None;
    }

    Family ReturnFamilyByID(string familyID)
    {
        for (int i = 0; i < Families.Length; i++)
        {
            if (Families[i].FamilyId == familyID)
            {
                return Families[i];
            }
        }
        return null;
    }

    /// <summary>
    /// Will return a family that is empty and has not ID set yet 
    /// </summary>
    /// <returns></returns>
    //internal Family FindVirginFamily()
    //{
    //    if (Families == null)
    //    {
            
    //        return null;
    //    }

    //    for (int i = 0; i < Families.Length; i++)
    //    {
    //        if (Families[i].IsFamilyEmpty() && string.IsNullOrEmpty(Families[i].FamilyId))
    //        {
    //            return Families[i];
    //        }
    //    }
    //    return null;
    //}

#endregion




    #region AnimalFarm

    PlantSave _plantSave;
    /// <summary>
    /// If is not null a plant was save it in here 
    /// </summary>
    public PlantSave PlantSave1
    {
        get { return _plantSave; }
        set { _plantSave = value; }
    }

    List<Animal> _animals = new List<Animal>();//the animals in a AnimalFarm 
    private bool wasFarmInited;

    public void RemoveAnimal(Animal animal)
    {
        var noti =_animals.Remove(animal);
        Debug.Log("animal removed on:"+MyId);
    }

    private void InitFarm()
    {
        if (wasFarmInited || !PositionFixed)
        {
            return;
        }
        wasFarmInited = true;

        if (HType.ToString().Contains(H.AnimalFarm + "") || HType == H.HeavyLoad)
        {
            InitAnimalFarm();
        }
    }

    void InitAnimalFarm()
    {
        if (HType==H.AnimalFarmSmall)
        {
            SpawnFarmAnimals(H.Small);
        }
        else if (HType == H.AnimalFarmMed)
        {
            SpawnFarmAnimals(H.Med);
        }
        else if (HType == H.AnimalFarmLarge)
        {
            SpawnFarmAnimals(H.Large );
        }
        else if (HType == H.AnimalFarmXLarge || HType == H.HeavyLoad)
        {
            SpawnFarmAnimals(H.XLarge);
        } 
    }

    void RedoAnimalFarmIfNeeded()
    {
        if (_oldProd != CurrentProd)
        {
            for (int i = 0; i < _animals.Count; i++)
            {
                //THEY WONT yield anything bz in theory
                //that meat used to create the new Farm is from the old
                _animals[i].Destroy();
            }
            _animals.Clear();
            InitAnimalFarm();
        }
    }

    private void SpawnFarmAnimals(H size)
    {
        var animalFactor = AmountOfAnimalFactor();
        if (size == H.Small)
        {
            SpawnAnimalNow(1 * animalFactor);
        }
        else if (size == H.Med)
        {
            SpawnAnimalNow(2 * animalFactor);
        }    
        else if (size == H.Large)
        {
            SpawnAnimalNow(4 * animalFactor);
        }  
        else if (size == H.XLarge)
        {
            SpawnAnimalNow(6 * animalFactor);
        }
    }

    /// <summary>
    /// The action of spawining each animal on the game in the farms 
    /// </summary>
    /// <param name="amt"></param>
    private void SpawnAnimalNow(int amt)
    {
        var iniPos = ReturnGroundMiddleOfInGameObjectZone(H.FarmZone);

        for (int i = 0; i < amt; i++)
        {
            Animal t = SpawnSpecificAnimal(iniPos);
            _animals.Add(t);
        }
    }

    Animal SpawnSpecificAnimal(Vector3 iniPos)
    {
        Animal t = null;

        if (HType == H.HeavyLoad)
        {
            t = Horse.Create(iniPos, this);
        }
        else if (CurrentProd.Product == P.Beef)
        {
            t = Beef.CreateBeef(iniPos, this);
        }  
        else if (CurrentProd.Product == P.Chicken)
        {
            t = Beef.CreateBeef(iniPos, this);
        } 
        else if (CurrentProd.Product == P.Pork)
        {
            t = Beef.CreateBeef(iniPos, this);
        }

        return t;
    }

    /// <summary>
    /// Will return the zone of a game object. Use to return the FarmZone of a farm 
    /// </summary>
    /// <param name="zone"></param>
    /// <returns></returns>
    public Rect ReturnInGameObjectZone(H zone)
    {
        var child = GetChildThatContains(zone);
        var min = child.transform.GetComponent<Collider>().bounds.min;
        var max = child.transform.GetComponent<Collider>().bounds.max;
        var childBounds = FindBounds(min, max);

        return Registro.FromALotOfVertexToRect(childBounds);
    }

    public Vector3 ReturnGroundMiddleOfInGameObjectZone(H zone)
    {
        var child = GetChildThatContains(zone);
        var min = child.transform.GetComponent<Collider>().bounds.min;
        var max = child.transform.GetComponent<Collider>().bounds.max;

        var mid = (min + max)/2;

        return m.Vertex.BuildVertexWithXandZ( mid.x, mid.z);
    }

    /// <summary>
    /// Will tell u if animal pass will overlap anyother existing aniumal
    /// </summary>
    /// <param name="pass"></param>
    /// <param name="animalDim"></param>
    /// <returns></returns>
    public bool CollideWithExistingAnimal(Vector3 newPos, int newID, float animalDim  )
    {
        var passAnimalRect = ReturnBoundsRect(newPos, animalDim);

        for (int i = 0; i < _animals.Count; i++)
        {
            var evalRect = ReturnBoundsRect(_animals[i].transform.position, animalDim);
            if (passAnimalRect.Overlaps(evalRect) && _animals[i].Id != newID)//so its not asking to  himself 
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Will create a recty with the 'dim;
    /// </summary>
    /// <param name="position"></param>
    /// <param name="dim"></param>
    /// <returns></returns>
    Rect ReturnBoundsRect(Vector3 position, float dim)
    {
        var poly= UPoly.CreatePolyFromVector3(position, dim, dim);
        return Registro.FromALotOfVertexToRect(poly);
    }


        /// <summary>
    /// Will give a list of vector 3 that is a division of amt int rows and col in the rect 
    /// </summary>
    /// <param name="zone"></param>
    /// <param name="amt"></param>
    /// <returns></returns>
    List<Vector3> ReturnPositionsFromInGameObjectZone(H zone, int amt)
    {
        List<Vector3>res = new List<Vector3>();
        var child = GetChildThatContains(zone);
        var min = child.transform.GetComponent<Collider>().bounds.min;
        var max = child.transform.GetComponent<Collider>().bounds.max;

        var mid = (min + max) / 2;
        var zonePoly = Registro.FromALotOfVertexToPoly(new List<Vector3>() {min, max});

        return DivideIntoPositions(zonePoly, amt);
    }

    List<Vector3> DivideIntoPositions(List<Vector3> zonePoly, int amt)
    {
        List<Vector3> res = new List<Vector3>();
        amt = MakeIntAEvenNumber(amt);

        var wide = Vector3.Distance(zonePoly[0], zonePoly[1]);
        var height = Vector3.Distance(zonePoly[1], zonePoly[2]);

        int rows = amt/2;
        int col = amt/rows;

        var addX = wide/rows;
        var addZ = height/col;

        var initVector = new Vector3(zonePoly[0].x + addX/2, m.IniTerr.MathCenter.y, zonePoly[0].z + addZ/2);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < col; j++)
            {
                res.Add(new Vector3(initVector.x + addX*i, m.IniTerr.MathCenter.y, initVector.z +addZ*j));
            }
        }

        //UVisHelp.CreateHelpers(res, Root.yellowCube);
        return res;
    }

    int MakeIntAEvenNumber(int amt)
    {
        var evenNumb = Bridge.isAEvenNumb(amt);
        if (!evenNumb)
        {
            amt += 1;
        }
        return amt;
    }


    /// <summary>
    /// Will return the factor of animals to put in an animal farm .
    /// </summary>
    /// <param name="animalType"></param>
    /// <returns></returns>
    int AmountOfAnimalFactor()
    {
        var animalType = CurrentProd.Product;

        //so HeavyLoad spawns cows at start 
        if (animalType == P.None && 
            (HType == H.HeavyLoad))
        {
            animalType = P.Horse;
        }

        if (animalType == P.Chicken)
        {
            return 5;
        }
        if (animalType == P.Pork)
        {
            return 3;
        }
        if (animalType == P.Beef || animalType == P.Horse)
        {
            return 1;
        }
        return -1;
    }

#endregion



    #region HeavyLoad

    /// <summary>
    /// When a HeavyLoader worker needs an animal
    /// </summary>
    public void GiveMeAnimal()
    {
        for (int i = 0; i < _animals.Count; i++)
        {
            if (!_animals[i].OnATrip())
            {
                _animals[i].Hide();
                return;
            }
        }
    }

    /// <summary>
    /// When a heavyLoader workers is done with an animal and its returning it to 
    /// this 
    /// </summary>
    public void ReturningBackAnimal()
    {
        for (int i = 0; i < _animals.Count; i++)
        {
            if (_animals[i].OnATrip())
            {
                _animals[i].BackFromTrip();
                return;
            }
        }
    }


#endregion



    #region Militar

    private Militar _militar;
    void InitMilitar()
    {
        if (IsMilitar())
        {
            _militar=new Militar(this);
        }
    }

    bool IsMilitar()
    {
        if (HType == H.PostGuard //|| HType == H.Tower 
            || HType == H.Fort || HType == H.Morro)
        {
            return true;
        }
        return false;
    }


#endregion


    /// <summary>
    /// Check if contain Bohio or House
    /// </summary>
    /// <param name="passID"></param>
    /// <returns></returns>
    static public bool IsHouseType(string passID)
    {
        return (passID.Contains("House") || passID.Contains("Bohio") || passID.Contains("Shack"))
        && !passID.Contains("LightHouse");
    }

    #region Dock  DryDock and Supplier

    Dock _dock;
    private Dispatch _dispatch;//dock will have a Dispatch


    public bool IsNaval()
    {
        if (HType == H.Shipyard || HType == H.Supplier || HType == H.Dock)
        {
            return true;
        }
        return false;
    }



    private void InitDockDryDockAndSupplier()
    {
        if (IsLoadingFromFile && _dock != null)
        {
            _dock.AssignBuild(this);

            return;
        }

        if (HType == H.Shipyard || HType == H.Supplier || HType == H.Dock)
        {
            _dock = new Dock(this);
            _dispatch = new Dispatch();
        }
    }

    public Dispatch Dispatch1
    {
        get { return _dispatch; }
        set { _dispatch = value; }
    }

    public Dock Dock1
    {
        get { return _dock; }
        set { _dock = value; }
    }

    public BuildersManager BuildersManager1
    {
        get { return _buildersManager; }
        set { _buildersManager = value; }
    }



    /// <summary>
    /// Will return a family that is empty and has not ID set yet 
    /// </summary>
    /// <returns></returns>
//internal Family FindVirginFamily()
//{
//    if (Families == null)
//    {

//        return null;
//    }

//    for (int i = 0; i < Families.Length; i++)
//    {
//        if (Families[i].IsFamilyEmpty() && string.IsNullOrEmpty(Families[i].FamilyId))
//        {
//            return Families[i];
//        }
//    }
//    return null;
//}

    #endregion

    #region AnimalFarm


    #endregion








    /// <summary>
    /// Will say if one empty family spot is marked already with param 'p'
    /// </summary>
    /// <param name="p"></param>
    /// <returns></returns>
    internal bool ThereIsAFamilyMarkedAlreadyWithMyId(string p)
    {
        if (Families == null)
        {
            return false;
        }

        for (int i = 0; i < Families.Length; i++)
        {
            if (Families[i].FamilyId == p)
            {
                return true;
            }
        }
        return false;
    }









    #region Upgrade Building Material

    /// <summary>
    /// The routine call to update mat to next 
    /// </summary>
    public void UpgradeMatToNext()
    {
        var current = ReturnMatUpgradeStatus();
        var next = ReturnNextUpgrade(current);


        PayUpgradeFee(FirstUpgradeAmt());

        UpgradeBuildMat(next);
    }

    /// <summary>
    /// Will return the material of current building. Only the last portion of the Key tht 
    /// is wht says wht status is at 
    /// </summary>
    /// <returns></returns>
    string ReturnMatUpgradeStatus()
    {
        var matKey = BuildingPot.Control.Registro.SelectBuilding.MaterialKey;
        var alone = matKey.Split('.')[1];
        return alone;
    }

    /// <summary>
    /// Will tell u if the material of currnet bulding is the best
    /// 
    /// Needed bz if is the best then the upgrade mat btn must be hide
    /// </summary>
    /// <returns></returns>
    public bool IsBuildingMaterialBest()
    {
        return ReturnMatUpgradeStatus() == "matBuildUpg2";
    }

    /// <summary>
    /// Will return which one is the next update coming after the current 
    /// </summary>
    /// <param name="current"></param>
    /// <returns></returns>
    string ReturnNextUpgrade(string current)
    {
        if (current == "matBuildBase")
        {
            return "matBuildUpg1";
        }
        if (current == "matBuildUpg1")
        {
            return "matBuildUpg2";
        }

        return "";
    }

    /// <summary>
    /// Will upgrade building material then will update it on registro 
    /// </summary>
    /// <param name="which"></param>
    void UpgradeBuildMat(string which)
    {
        Building b = BuildingPot.Control.Registro.SelectBuilding as Building;
        b.MaterialKey = b.HType + "." + which;

        string root = Root.RetMaterialRoot(b.MaterialKey);
        Material mat = (Material)Resources.Load(root);

        if (BuildingPot.Control.Registro.SelectBuilding.HType == H.Trail)
        {
            Trail t = BuildingPot.Control.Registro.SelectBuilding as Trail;
            t.AssignNewMaterialToPlanes(mat);
        }
        else if (BuildingPot.Control.Registro.SelectBuilding.Category == Ca.Structure
            || BuildingPot.Control.Registro.SelectBuilding.Category == Ca.Shore)
        {
            Structure t = BuildingPot.Control.Registro.SelectBuilding as Structure;
            t.AssignNewMaterial(mat);
        }
        else if (BuildingPot.Control.Registro.SelectBuilding.Category == Ca.DraggableSquare)
        {
            DragSquare t = BuildingPot.Control.Registro.SelectBuilding as DragSquare;
            t.AssignNewMaterialToPlanes(mat);
        }

        //will update the Item in Registro so if user saves will be saved on disk
        BuildingPot.Control.Registro.UpdateItemMaterial(BuildingPot.Control.Registro.SelectBuilding.Category, BuildingPot.Control.Registro.SelectBuilding.MyId,
            b.MaterialKey);
    }



    #endregion




    #region Upgrade Building Storage Capacity

    /// <summary>
    /// Will return true if all Storages capacities upgraces were reached
    /// </summary>
    /// <returns></returns>
    internal bool IsBuildingCapAtMax()
    {
        //for ways 
        if (Inventory == null)
        {
            //true so the btn for addding more capacity hides 
            return true;
        }

        var baseCap = Book.GiveMeStat(HType).Capacity;

        return Inventory.CapacityVol == baseCap + FirstUpgradeAmt() + SecondUpgradeAmt();
    }

    float FirstUpgradeAmt()
    {
        var baseCap = Book.GiveMeStat(HType).Capacity;
        return baseCap /3;
    }

    float SecondUpgradeAmt()
    {
        return FirstUpgradeAmt()/2;
    }

    /// <summary>
    /// Upgrades capacity of Storage to next level 
    /// </summary>
    internal void UpgradeCapToNext()
    {
        var baseCap = Book.GiveMeStat(HType).Capacity;

        if (Inventory.CapacityVol == baseCap)
        {
            Inventory.CapacityVol += FirstUpgradeAmt();
            PayUpgradeFee(FirstUpgradeAmt());
        }
        else if (Inventory.CapacityVol == baseCap + FirstUpgradeAmt())
        {
            Inventory.CapacityVol += SecondUpgradeAmt();
            PayUpgradeFee(SecondUpgradeAmt());
        }

        //will update the Item in Registro so if user saves will be saved on disk
        BuildingPot.Control.Registro.ResaveOnRegistro(MyId);
    }




    /// <summary>
    /// Everytime the next stage is used a fee must be paid 
    /// </summary>
    private void PayUpgradeFee(float fee)
    {
        Program.gameScene.GameController1.Dollars -= fee;
    }



    #endregion





    /// <summary>
    /// Will add a 1000 if a adult will find love in this building 
    /// </summary>
    /// <param name="person"></param>
    /// <returns></returns>
    internal int WouldFindLoveInThisBuilding(Person person)
    {
        for (int i = 0; i < Families.Length; i++)
        {
            if (Families[i].WouldIFoundLoveHere(person))
            {
                return 1000;
            }
        }
        return 0;
    }





    #region Customers

    //Customers . For school the kids, church people going there, tavern is the customer
    private int _customerCap = 20;
    private int _currentCustomers;

    void InitCustomersCap()
    {
        
    }

    public bool CanAddOneMoreCustomer()
    {
        return _currentCustomers < _customerCap;
    }

    public void AddOneCustomer()
    {
        _currentCustomers++;
    }

    public void RemoveOneCustomer()
    {
        _currentCustomers--;
    }

    public bool IsBuildingCustomerType(Person pers)
    {
        var tavChu = (HType == H.Tavern || HType == H.Church);
        var scholar = (HType == H.School || HType == H.TradesSchool) && !UPerson.IsMajor(pers.Age)
            ;
        return (tavChu || scholar) && PeopleDict.Count >= MaxPeople;
    }

#endregion









    Vector3 _middlePoint = new Vector3();
    /// <summary>
    /// Must be called only if Anchors were defined already. Otherwise returns transform.position
    /// </summary>
    /// <returns></returns>
    internal Vector3 MiddlePoint()
    {
        if (_anchors.Count == 0)
	    {
	        return transform.position;
	    }

        if (_middlePoint == new Vector3())
        {
            for (int i = 0; i < _anchors.Count; i++)
			{
			    _middlePoint+= _anchors[i];  
			}
            _middlePoint /= 4;
        }
        return _middlePoint;
    }












    #region Hover All Objects. All objects that have a collider will be hoverable

    protected void OnMouseEnter()
    {
        base.OnMouseEnter();
    }

    protected void OnMouseExit()
    {
        //if (transform.name.Contains("Preview"))
        //{
        //    return;
        //}

        //base.OnMouseExit();
    }

    #endregion




    StageManager _stageManager;
    List<ParticleSystem> _pSystem;
    public void SmokePlay(bool isToPlayNow)
    {
        if (_pSystem == null || _pSystem.Count == 0)
        {
            return;
        }

        for (int i = 0; i < _pSystem.Count; i++)
        {
            PlayThisSystemPart(isToPlayNow, _pSystem[i]);
        }
    }

    void PlayThisSystemPart(bool isToPlayNow, ParticleSystem pSystem)
    {
        if (isToPlayNow && pSystem.isStopped)
        {
            pSystem.Play();
            pSystem.Clear(true);

        }
        else if(!isToPlayNow && !pSystem.isStopped)
        {
            pSystem.Stop();
        }
    }

    void CheckIfNightSmoke()
    {
        var isEmit = _pSystem!=null && _pSystem.Count > 0 && _pSystem[0].isEmitting;

        if (isEmit && ( _stageManager.IsSunsetOrLater() || PeopleDict.Count == 0) )
        {
            SmokePlay(false);
        }

    }

    internal void HomeSmokePlay()
    {
        if (!_stageManager.IsSunsetOrLater())
        {
            SmokePlay(true);
        }
    }

}












/// <summary>
/// Created to booked homes to familyes so they are kept toghether 
/// </summary>
public class BookedHome
{
    public string Building;//the key of the building there are booked to
    
    public Family Family = new Family();//the family tht booked this building

    public BookedHome() { }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="building"></param>
    /// <param name="family">It doesnt hold the ref. cretes a copy object of family</param>
    public BookedHome(string building, Family family)
    {
        Building = building;
        Family =  new Family(family);
    }

    /// <summary>
    /// Clears all the information of the bopoking so is unbooked 
    /// </summary>
    public void ClearBooking()
    {
        Family.State = H.None;
        Building = "";
        Family.DeleteFamily();
    }

    /// <summary>
    /// Will return true if param 'person' belong to the field 'Family'
    /// </summary>
    /// <param name="person"></param>
    /// <returns></returns>
    public bool IAmBookedHere(Person person)
    {
        return Family.DoIBelongToThisFamilyChecksFamID(person) && 
            !string.IsNullOrEmpty(Building) //if is "" or null is not booked here. Spent a whole day trying to find where the 
                                            //person write in another house BookedHome1 without being able too and with people on it
                                            //i found do that everytime tht doesnt doesnt put a Building. So thts it
                                            //if doesnt have Building is not booked 
            ;
    }

    /// <summary>
    /// Will return true if this Building is booked
    /// </summary>
    /// <returns></returns>
    public bool IsBooked()
    {
        if (Family == null)
        {
            return false;
        }

        return !Family.IsFamilyEmpty();
    }

    /// <summary>
    /// As person are assigned to  the booked place are remove from this family in the booking and 
    /// added to the Family in the building , and removed from the Familoy var in their old Home too
    /// </summary>
    public void RemovePersonFromBooking(Person personToRemove)
    {
        Family.RemovePersonFromFamily(personToRemove);
        //everyone was added to the new place and the boking is clear 
        if (Family.IsFamilyEmpty())
        {
            //so is not there anymore as a house with space to get new people. This hose is already
            //ocuppied by a family... and the family is formed then can be removed
            var building = Brain.GetBuildingFromKey(Building);

            if (building != null && building.AllFamiliesFull())
            {
                BuildingPot.Control.RemoveFromHousesWithSpace(Building);
            }
            
            //just addressingn a bugg tht book can happen 
           //Debug.Log("Book to clear:"+personToRemove.MyId+ " famId b4:"+personToRemove.FamilyId);
            ClearBooking();
           //Debug.Log("Book Cleared:" + personToRemove.MyId + " famId b4:" + personToRemove.FamilyId);


            //so Individuals tht asked and where denied get a chancee to see this building unbooked
            PersonPot.Control.RestartController();
        }
    }

    ///// <summary>
    ///// Will make the old Home Family var virign so can be booked properly on realtor 
    ///// </summary>
    //private void MakeOldHomeFamilyVarVirgin(Person toRemove)
    //{
    //    var oldHome = Brain.GetBuildingFromKey(toRemove.Brain.MoveToNewHome.OldHomeKey);
    //    //Debug.Log("Make virgin on");

    //    if (oldHome!= null)
    //    {
    //        //Debug.Log("Make virgin on oldHome!= null");
    //        //is good enoguh bz as long as the first perso moving out do this 
    //        var fam = oldHome.FindOldFamilyById(toRemove);

    //        if (fam == null)
    //        {
    //            fam = oldHome.FindMyFamily(toRemove);
    //        }

    //        if (fam != null)
    //        {
    //            fam.DeleteFamily();
    //           //Debug.Log("deleted family on:");
    //        }
    //    }
    //}



    internal bool MySpouseBooked(string Spouse)
    {
        if (Family.Mother == Spouse || Family.Father == Spouse)
        {
            return true;
        }
        return false;
    }
















}
