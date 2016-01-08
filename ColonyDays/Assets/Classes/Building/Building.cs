using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Random = UnityEngine.Random;

public class Building : General, Iinfo
{
    #region Fields and Prop

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
        get { return _anchors; }
        set { _anchors = value; }
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
        if (container != null){obj.transform.parent = container;}

        if (materialKey == "")
        {materialKey = hType + "." + Ma.matBuildBase;}

        obj.MaterialKey = materialKey;
        //obj.Geometry.GetComponent<Renderer>().sharedMaterial = Resources.Load(Root.RetMaterialRoot(materialKey)) as Material;

        return obj;
    }

    /// <summary>
    /// Checks if Terrain below the build _isEven or _isColliding, and is tall enough
    /// </summary>
    /// <returns>True if terrain is even, not colliding and tall enough</returns>
    public virtual bool CheckEvenTerraCollWater()
    {
        //if is fake obj will be terminated 
        if (_isFakeObj) { return false; }

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

        bool res = _isEven && !_isColliding && _isGoodWaterHeight
            && isScaledOnFloor
            ;
        if (res)
        {
            //Geometry.renderer.material.color = _initialColor;
        }
        return res;
    }

    /// <summary>
    /// Tells u if the Anchors out of the anchors are on the Floor
    /// 
    /// 
    /// </summary>
    /// <returns></returns>
    bool IsScaledAnchorsOnFloor()
    {
        var scale = 1.25f;//0.2f was choose arbitrary. How far I gonna check
        //I scaled to address the problem when building is too close to water or cliff or moutain.
        //I make Anchors a bit bigger so checks for a bit bigger that Anchor area 
        var scaledAnchors = UPoly.ScalePolyNewList(Anchors, scale);
        //bz needs to find the real Y values
        scaledAnchors = RectifyOnY(scaledAnchors);
        //checks if is on the floor
        return IsOnTheFloor(scaledAnchors, 0.25f);
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
        //if they were set already 
        if (!forced && ValidVector3List(_anchors, 4))//4: bz anchors are 4 
        {return _anchors;}

        //will find anchors
        UpdateMinAndMaxVar();
        _bounds = FindBounds(_min, _max);
        _anchors = FindAnchors(_bounds);

        return _anchors;
    }

    /// <summary>
    /// Will tell u if the list has the amount of elements and if all are different that 
    /// default value, then will be true
    /// </summary>
    bool ValidVector3List(List<Vector3> list, int amt)
    {
        int count = 0;
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] != new Vector3())
            {
                count++;
            }
        }

        if (count == list.Count && list.Count == amt)
        {
            return true;
        }
        return false;
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
        //UVisHelp.CreateHelpers(NW, Root.redSphereHelp);
        //UVisHelp.CreateHelpers(SE, Root.redSphereHelp);
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
        //UVisHelp.CreateHelpers(res, Root.redSphereHelp);
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
            res.Add(m.Vertex.BuildVertexWithXandZ(list[i].x, list[i].z));
        }
        //UVisHelp.CreateHelpers(res, Root.blueCube);
        return res;
    }

    /// <summary>
    /// Update _min, _max, _bounds, _anchors and then will call CheckIfIsEven(_anchors, maxDiffAllowOnTerrain)
    /// </summary>
    /// <returns></returns>
    public virtual bool CheckIfIsEvenRoutine()
    {
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
  
            IfShackResaveInventoryOnRegistro();
        }

        InitDock();
        InitDryDockAndSupplier();
        InitWheelBarrow();

        //if gives a null ex here ussually is that u forgot a prefab on scene
        _minHeightToSpawn = Program.gameScene.WaterBody.transform.position.y + minHeightAboveSeaLevel;

        if(!IsLoadingFromFile){CreateProjector();}

        //this is for init builds propertes such as Families, inventory
        Init();
        LayerRoutine("init");

        //loads the defualt
        CurrentProd = BuildingPot.Control.ProductionProp.ReturnDefaultProd(HType);
        InitFarm();
        
        InitJobRelated();

        StartCoroutine("ThirtySecUpdate");
    }

    #region Current Product

    /// <summary>
    /// Will show all the products this Building can produce 
    /// </summary>
    /// <returns></returns>
    public List<string> ShowProductsOfBuild()
    {
        return BuildingPot.Control.ProductionProp.ReturnProducts(HType);
    }

    /// <summary>
    /// Will set CurrentProd to 'newProd'
    /// </summary>
    /// <param name="newProd"></param>
    public void SetProductToProduce(string newProd)
    {
        //the newProd comes with the name of the prod a . and a number that is the index of the 
        //list of where this prod is 
        //ex Corn.0 
        //0 is the corresponding item in the list 
        var split = newProd.Split('.');
        int index = int.Parse(split[1]);
        var foundProd = BuildingPot.Control.ProductionProp.ReturnExactProduct(HType, index);
        CurrentProd = foundProd;
        Debug.Log("now Prod curr: "+CurrentProd +" on:"+MyId);
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
        if (!IsLoadingFromFile || PersonPot.Control == null || PersonPot.Control.BuildersManager1 == null)
        {
            return;
        }

        if (IsLoadingFromFile && Instruction == H.WillBeDestroy)
        {
            AssignLayer(0);//Default
            RemovePeople();

            if (Category != Ca.Way)
            {
                //so the action of demolish happens again 
                var b = (Structure)this;
                b.Demolish();
            }
            else
            {
                var b = (Trail)this;
                b.Demolish();
            }


            //so dont call this method anymore
            IsLoadingFromFile = false;
        }
    }


    /// <summary>
    /// Needs to be save bz shack is added by the builder to the registro before the inventory is instantiated
    /// </summary>
    void IfShackResaveInventoryOnRegistro()
    {
        if (HType != H.Shack)
        {
            return;
        }

        int index = BuildingPot.Control.Registro.AllRegFile.FindIndex(a => a.MyId == MyId);

        if (index == -1)
        {
            return;
        }

        BuildingPot.Control.Registro.AllRegFile[index].Inventory = Inventory;
    }


    #region Bad Ass

    private static MyProjector _projector;
    private static General _light;

    /// <summary>
    /// this is the projector that hover when creating a nw building, or the current selected building
    /// </summary>
    public MyProjector Projector
    {
        get { return _projector; }
        set { _projector = value; }
    }

    public void CreateProjector()
    {
        if (Category != Ca.None && Projector == null && !MyId.Contains("Dummy") &&
            !MyId.Contains("Shack"))
        {
            Projector = (MyProjector) Create(Root.projector, container: transform);
            _light = Create(Root.lightCil, transform.position, container: transform);
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

   

    //this need to be called in derived classes 
    protected new void Update()
    {
        //if is way not need to know this.
        //bz we will be going btw buildings 
        if (Category != Ca.Way)
        {
            LandZoneLoader(); 
        }
        

        LoadingWillBeDestroyBuild();

        if (!PositionFixed)
        {
            base.Update();

            //will updtae if is not beinfg ordered to destroy and the instrucion not = = H.WillBeDestroy
            //added the last part to avoid Exception when class was checking on Building that was marked for be destroyed
            if(!_isOrderToDestroy && Instruction != H.WillBeDestroy)
            {UpdateBuild();}
        }

        if (_debugShip != null)
        {
            _debugShip.Update();    
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
    /// </summary>
    public virtual void UpdateClosestVertexAndOld()
    {
        if (!UMath.nearEqualByDistance(ClosestVertOld, ClosestSubMeshVert, 0.01f))
        {
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
        else if (!IsEven) { GameScene.ScreenPrint("Can't place, uneven terrain.Building.cs"); }
        else if (_isColliding) { GameScene.ScreenPrint("Is colliiding.Building.cs"); }
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

    /// <summary>
    /// This is call when we finish placing a building 
    /// </summary>
    public virtual void FinishPlacingMode(H action)
    {
        //bz this action needs to be immediate 
        if (action == H.Cancel)
        {
            DestroyCool();
            return;
        }

        LayerRoutine("done");
        PositionFixed = true;

        if (!HType.ToString().Contains("Unit") && !IsLoadingFromFile)
        {
            PrivHandleZoningAddCrystals(); ;
        }

        if (IsLoadingFromFile)
        {
            return;
        }

        BuildingPot.Control.AddToQueuesRestartPersonControl(MyId);
    }

    /// <summary>
    /// Checks if is colliding with another building 
    /// </summary>
    /// <returns>True if collides</returns>
    public virtual bool CheckIfColliding()
    {
        var tBounds = _bounds;
        tBounds = UPoly.ScalePoly(tBounds, 0.05f);

        return BuildingPot.Control.Registro.IsCollidingWithExisting(tBounds);
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

            BuildingPot.Control.Registro.RemoveItem(Category, MyId);
            BuildingPot.Control.Registro.AllBuilding.Remove(MyId);
            MeshController.CrystalManager1.Delete(this);

            DestroyProjector();
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
            MarkTerraSpawn(p.TerraSpawnController.AllRandomObjList[collidWith[i]]);
        }
        if (listFrom == null && from == new Vector3())
        { print("Error: Both obj passed were null. Building.MarkTerraSpawnRoutine"); }
    }

    /// <summary>
    /// Will mark the TerrainRamdonSpawner pass. Will added to the InputMain.InputMeshSpawnObj.ToMineSelectList list
    /// and will create visual help so can be seen that was mark to be mined
    /// </summary>
    /// <param name="obj">TerrainRamdonSpawner obj to be mark</param>
    protected void MarkTerraSpawn(TerrainRamdonSpawner obj)
    {
        StillElement still = (StillElement)obj;
        
        //still.IsMarkToMine = true;
        //InputMain.InputMeshSpawnObj.ToMineSelectList.Add(still);
        //InputMain.InputMeshSpawnObj.AddVisHelpList(true, still);

        //so they disappear, remove Crystals and Routing can work properly
        still.DestroyCool();
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

    List<H> doubleBounds = new List<H>(){H.FishRegular, H.FishSmall, 
        H.Dock, H.DryDock, H.Supplier,
        H.MountainMine, H.SaltMine};
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
           return RoutineToFindIfAnchorsAreGood(_terraBound, _underTerraBound, H.TerraUnderBound);
        }
        else
        {
            DefineBoundsGameObj(H.MaritimeBound);
            return RoutineToFindIfAnchorsAreGood(_terraBound, _maritimeBound, H.MaritimeBound);
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
            if (anchorsListP[i].y > m.SubMesh.mostCommonYValue + variance
                || anchorsListP[i].y < m.SubMesh.mostCommonYValue - variance)
            {
                return false;
            }
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

    private P _currentProd = P.None;//product currently is being created on this building 
    private int _rationsPay = 2;//the pay in rations to the workers of a building 
    private int _dollarsPay = 5;//in dollars 

    private int _confort;//the confort of a house
    private BookedHome _bookedHome;//will hold the information if a

    public P CurrentProd
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
        set { _peopleDict = value; }
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

    public int Confort
    {
        get { return _confort; }
        set { _confort = value; }
    }

    public BookedHome BookedHome1
    {
        get { return _bookedHome; }
        set { _bookedHome = value; }
    }

    void Init()
    {
        InitHouseProp();
        FirstFamilyWasMarked();

        SetHouseConfort();

        InitBasePays();
    }

    #region Salary

    /// <summary>
    /// this is the base pay for each building. From here will move to change salary
    /// </summary>
    private void InitBasePays()
    {
        _dollarsPay = BasePay();
    }

    /// <summary>
    /// the base pay for each job
    /// </summary>
    /// <returns></returns>
    int BasePay()
    {
        if (HType == H.Ceramic)
        {
            //return  10;
        }
        if (HType == H.Church)
        {
            //return 20;
        }
        return 5;
    }

    /// <summary>
    /// The user wil click on the checkbox and tht will change the salary
    /// 
    /// checkBox val : 1-5
    /// </summary>
    /// <param name="which"></param>
    public void ChangeSalary(string which)
    {
        if (which == "Sal_Toggle_1")
        {
            DollarsPay = BasePay() - 2;
        }
        else if (which == "Sal_Toggle_2")
        {
            DollarsPay = BasePay() - 1;
        }
        else if (which == "Sal_Toggle_3")
        {
            DollarsPay = BasePay();
        }
        else if (which == "Sal_Toggle_4")
        {
            DollarsPay = BasePay() + 1;
        }
        else if (which == "Sal_Toggle_5")
        {
            DollarsPay = BasePay() + 2;
        }
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
            Inventory.Add(P.Pork, amtFood);


            Program.gameScene.GameController1.SetInitialLote();


            UpdateInfo();
        }
        
    }

    public void UpdateInfo(string v = "")
    {
        info = Inventory.Info();
    }

    void InitHouseProp()
    {
        if (IsLoadingFromFile)
        {
            return;
        }

        //can hhave 1 famili with 3 kids
        if (HType == H.HouseA || HType == H.HouseB)
        {
            Families = new Family[1];
            Families[0] = new Family(3, MyId);
        }
        //can hhave 2 famili with 3 kids each
        else if (HType == H.HouseAWithTwoFloor)
        {
            Families = new Family[2];
            Families[0] = new Family(3, MyId);
            Families[1] = new Family(3, MyId);
        }
        //can hhave 1 famili with 5 kids
        else if (HType == H.HouseMedA || HType == H.HouseMedB || HType == H.HouseC || HType == H.HouseD)
        {
            Families = new Family[1];
            Families[0] = new Family(5, MyId);
        }
        else if (HType == H.Shack)
        {
            Families = new Family[1];
            Families[0] = new Family(3, MyId);
        }
    }

    /// <summary>
    /// If the 1st family was marked to be booked before the 'Familes' were created then will do so
    /// with the first one 
    /// </summary>
    void FirstFamilyWasMarked()
    {
        if (!string.IsNullOrEmpty( markTheFirstFamily))
        {
            Families[0].FamilyId = markTheFirstFamily;
            markTheFirstFamily = "";
        }
    }

    /// <summary>
    /// Will find the family of the person asking .
    /// If not found will return null
    /// </summary>
    /// <param name="person"></param>
    /// <returns></returns>
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
    /// </summary>
    /// <param name="pers"></param>
    /// <returns></returns>
    internal Family FindFamilyById(Person pers)
    {
        for (int i = 0; i < Families.Length; i++)
        {
            if (Families[i].FamilyId == pers.FamilyId)
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
    public bool WouldAdultFitInThisHouseInAFamily(Person asker)
    {
        for (int i = 0; i < Families.Length; i++)
        {
            if (Families[i].WouldAdultFitInThisFamily(asker))
            {
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

    public bool AtLeastHasOneVirginFamily()
    {
        var vFam = FindVirginFamily();

        if (vFam == null)
        {
            return false;
        }
        return true;
    }

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

    void SetHouseConfort()
    {
        if (HType == H.Shack)
        {
            _confort = 1;
        }
        else if (HType == H.HouseAWithTwoFloor)
        {
            _confort = 3;
        }
        else if (HType == H.HouseA || HType == H.HouseB)
        {
            _confort = 4;
        }
        else if (HType == H.HouseMedA || HType == H.HouseMedB || HType == H.HouseC)
        {
            _confort = 6;
        }
        else if ( HType == H.HouseD)
        {
            _confort = 7;
        }
    }

    public bool ThisPersonFitInThisHouse(Person newPerson)
    {
        //means another person is asking for this buiding before hit the Start()
        //is addressing the case when a lot of people is getting out of a house to a Shack
        if (Families == null)
        {
            return false;
        }

        if(!UPerson.IsMajor(newPerson.Age))
        {
            return ANewKidFitsInThisHouse(newPerson);
        }
        return ANewAdultFitsInThisHouse(newPerson);
    }






    bool ANewKidFitsInThisHouse(Person newP)
    {
        for (int i = 0; i < Families.Length; i++)
        {
            if (Families[i].CanGetAnotherKid(newP))
            {
                return true;
            }
        }
        return false;
    }

    bool ANewAdultFitsInThisHouse(Person newP)
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
            AssignLayer(0);//Default
            RemovePeople();
        }
        BuildingPot.Control.EditBuildRoutine(MyId, action, HType);
    }

    void RemovePeople()
    {
        Instruction = H.WillBeDestroy;
        if (PeopleDict.Count == 0)//no one is registered on the build
        {
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
            AssignLayer(0);//default
        }
        else if (command == "done")
        {
            AssignLayer(prefabLayer);//restore layer to initial one
        }
    }

    void GrabPrefabLayer()
    {
        prefabLayer = gameObject.layer;
    }

    protected void AssignLayer(int layer)
    {
        //just bz shack starts with layer 0 .. i dont know why 
        if (HType == H.Shack)
        {
            gameObject.layer = 10;
        }
        else gameObject.layer = layer;
    }
    #endregion

    public void DestroydHiddenBuild()
    {
        //the invetory needs to be empty to be destroyed  
        if (Inventory != null && !Inventory.IsEmpty() )
        {
            return;
        }

        if (BuildingPot.Control.DispatchManager1.DoIHaveAnyOrderOnAnyDispatch(this))
        {
            return;
        }

        BuildingPot.Control.Registro.RemoveFromAllRegFile(MyId);

        BuildingPot.Control.EditBuildRoutine(MyId, H.Remove, HType);
        PositionFixed = false;

        _isOrderToDestroy = true;
        DestroyOrdered();

        //so people can Reroutes if new build fell in the midle of one
        PersonPot.Control.Queues.AddToDestroyBuildsQueue(Anchors, MyId);
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

    private int constructionAmt;
    private int amtNeeded;
    Book book = new Book();
    BuildStat buildStat = new BuildStat();

    public void AddToConstruction(float amt)
    {
        DefineAmtNeeded();
        constructionAmt += (int)amt;
        CheckIfNewStageOrDone();
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
    private void CheckIfNewStageOrDone()
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
            HandleLastStage();
        }
    }

    /// <summary>
    /// Crated for modularity. And reutnr the indicate Structure Parent . if is a bridge will return the first piece
    /// </summary>
    /// <returns></returns>
    StructureParent ReturnCurrentStructureParent()
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

    /// <summary>
    /// Created for modularity. Handles all things related onces the building is fully built 
    /// </summary>
    protected void HandleLastStage()
    {
        PersonPot.Control.BuildersManager1.RemoveConstruction(MyId);

        //if is a Unit from a bridge doesnt need to be added there 
        //Bridge bz needs to be called when all bridge elements are spanwed
        if (HType.ToString().Contains(H.Unit.ToString()))
        {
            return;
        }
        UpdateOnBuildControl(H.Add);
        
        //needs to be called here other wise Dormant Orders will not become active
        InitStorage();

        //bz trhu this way is the only way brdige can call it 
        if (MyId.Contains("Bridge"))
        {
            PrivHandleZoningAddCrystals();
        }
    }

    #endregion

    #region LandZoning

    private bool landZoneLoaded;
    /// <summary>
    /// Loads the land Zone and adds the Crystals of this Building to Crystal Manager 
    /// </summary>
    void LandZoneLoader()
    {
        if (landZoneLoaded || !PositionFixed ||!IsLoadingFromFile)
        {
            return;
        }

        //so it can add the corners on CrystalManager
        Anchors = GetAnchors();
        //UVisHelp.CreateHelpers(Anchors, Root.largeBlueCube);
        MeshController.CrystalManager1.Add(this);
        landZoneLoaded = true;
    }

    protected void PrivHandleZoningAddCrystals()
    {
        HandleLandZoning();
        //UVisHelp.CreateHelpers(Anchors, Root.yellowCube);
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
            var landZonName = MeshController.CrystalManager1.ReturnLandingZone(sp.SpawnPoint.transform.position);

            LandZone1.Add(new VectorLand(landZonName, sp.SpawnPoint.transform.position));
        }
        else
        {
            LandZoningBridge();
        }
    }

    /// <summary>
    /// Is made public so when is loding is called 
    /// </summary>
    public void LandZoningBridge()
    {
        Bridge br = (Bridge)this;
        var ends = br.GiveTwoRoughEnds();

        var zone1 = MeshController.CrystalManager1.ReturnLandingZone(ends[0]);
        var zone2 = MeshController.CrystalManager1.ReturnLandingZone(ends[1]);
        
        BuildingPot.Control.BridgeManager1.AddBridge(zone1, zone2, br);

        LandZone1.Add(new VectorLand(zone1, ends[0]));
        LandZone1.Add(new VectorLand(zone2, ends[1]));
    }
    #endregion



    #region Production

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
        return CurrentProd;
    }
    
    /// <summary>
    /// Produce what this Building is set to. ex Fisherman produce fish
    /// 
    /// 'amt' the amount the person calling this can produce in a shift 
    /// </summary>
    internal void Produce(float amt, Person person, bool addToBuildInv = true, P prod = P.None)
    {
        P prodHere = DefineProdHere(prod);

        var doIHaveInput = DoBuildHaveRawResources();
        var hasStorageRoom = DoesStorageHaveCapacity(person);
        var hasThisBuildRoom = DoWeHaveCapacityInThisBuilding();

        if (doIHaveInput && (hasStorageRoom || hasThisBuildRoom))
        {
            //if is a farm
            if (MyId.Contains("Farm"))
            {
                var farm = (Structure)this;
                farm.AddWorkToFarm();
            }
            else if (addToBuildInv && !MyId.Contains("Farm"))
            {
                Inventory.Add(prodHere, amt);
            }
            else if (!addToBuildInv && !MyId.Contains("Farm"))
            {
                person.Inventory.Add(prodHere, amt);
            }
        }
        else if (!hasStorageRoom && !hasThisBuildRoom && person.FoodSource != null)
        {
            AddEvacuationOrder();
            Debug.Log("Both full" + person.FoodSource.MyId + ".and." + MyId + " AddEvacuationOrder() called");
        }
        else if (!doIHaveInput)
        {
            Debug.Log(MyId+" doesnt have input");
        }
    }

    /// <summary>
    /// Use for plants lie Corn to add produced ammt
    /// </summary>
    /// <param name="amt"></param>
    internal void Produce(int amt)
    {
        var doIHaveInput = DoBuildHaveRawResources();
        var hasThisBuildRoom = DoWeHaveCapacityInThisBuilding();

        if (doIHaveInput && hasThisBuildRoom)
        {
            Inventory.Add(CurrentProd, amt);
        }
        else if (!hasThisBuildRoom)
        {
            AddEvacuationOrder();
            //            Debug.Log("Both full" + person.FoodSource.MyId + ".and." + MyId);
        }
        else if (!doIHaveInput)
        {
            Debug.Log(MyId + " doesnt have input");
        }
    }

    /// <summary>
    /// If is all full an evacuation order is add to Dispatch so at least this 
    /// building room will be clear .
    /// 
    /// When this starts to happen over and over again is when u want to start to Export this Product
    /// In the Port should be a Dispatch to Export and Import 
    /// </summary>
    void AddEvacuationOrder()
    {
        Order t = new Order(CurrentProd, "", MyId);
        //BuildingPot.Control.Dispatch1.AddEvacuationOrder(t);
        AddToClosestWheelBarrowAsOrder(t, H.Evacuation);
    }

    /// <summary>
    /// Custom product. use so far by Forester:
    /// 
    /// Will find the product it has the most amount of units and will add the evacuation orders 
    /// with that Product
    /// </summary>
    /// <param name="prod"></param>
    public void AddEvacuationOrderMost()
    {
        //order the Products by amout
        var prods = Inventory.InventItems.OrderBy(a => a.Amount).ToList();

        //uses the one prod has the most to be added on the Evac Order
        Order t = new Order(prods[0].Key, "", MyId);
        AddToClosestWheelBarrowAsOrder(t, H.Evacuation);
    }

    /// <summary>
    /// Will tell worker if can take products out of the biulding
    /// 
    /// Used to express if a person can take goods out of building to a Storage or should leave it here in this building 
    /// </summary>
    /// <returns></returns>
    public bool CanTakeItOut(Person person)
    {
        return (person.FoodSource != null && DoesStorageHaveCapacity(person));
    }

    /// <summary>
    /// For the buildings that need raw products as an input for the output will will tell u if 
    /// has input enough or not 
    /// </summary>
    /// <returns></returns>
    bool DoBuildHaveRawResources()
    {
        return BuildingPot.Control.ProductionProp.DoIHaveEnoughOnInvToProdThis(this);
    }

    /// <summary>
    /// Will tell u if a Storage has enoguh capacity to hold this new amt of goods
    /// </summary>
    /// <returns></returns>
    bool DoesStorageHaveCapacity(Person person)
    {
        return person.FoodSource != null && !person.FoodSource.Inventory.IsFull();
    }

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
        var ingredientsNeeded = BuildingPot.Control.ProductionProp.ReturnIngredients(CurrentProd);

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
        var rawsOnNeed = BuildingPot.Control.ProductionProp.ReturnIngredients(CurrentProd);

        if (rawsOnNeed == null)
        {
            return;
        }

        for (int i = 0; i < rawsOnNeed.Count; i++)
        {
            P prod = rawsOnNeed[i].Element;

            if (!HaveThisProdOnInv(prod))
            {
                //use 10000 to put a large number of units needed
                Order prodNeed = new Order(prod, MyId, 10000);

                //BuildingPot.Control.Dispatch1.AddToOrders(prodNeed);

                AddToClosestWheelBarrowAsOrder(prodNeed, H.None);
             
            }
        }
    }


    /// <summary>
    /// Will find closest WheelBarrow office from here and will add the order 
    /// </summary>
    /// <param name="order"></param>
    public void AddToClosestWheelBarrowAsOrder(Order order, H typeOfOrder)
    {
        var closWheelBarr = FindClosestWheelBarrowerOffice();

        //only for debug bz a WheelBarrow always should be up
        if (closWheelBarr == null)
        {
            return;
        }

        if (typeOfOrder == H.None)
        {

            closWheelBarr.Dispatch1.AddToOrders(order);            
        }
        else if (typeOfOrder == H.Evacuation)
        {
            closWheelBarr.Dispatch1.AddEvacuationOrder(order);
        }
    }








    Structure FindClosestWheelBarrowerOffice()
    {
        return BuildingController.FindTheClosestOfThisType(H.BuildersOffice, transform.position);
    }










    private IEnumerator ThirtySecUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1, 2)); // wait
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

    /// <summary>
    /// Function tht user cam use to delete the Building inventory in case finds he doesnt need it 
    /// </summary>
    public void DeleteBuildingInventory()
    {
        Inventory.Delete();
    }
    #endregion

    void InitWheelBarrow()
    {
        if (HType != H.BuildersOffice)
        {
            return;
        }

        _dispatch = new Dispatch();
    }

    #region Job Related

    private int _maxPeople;//max people this builging can hold. workers 
    private int _positionsFilled;//positions filled in this building as a job 
    public int PositionsFilled
    {
        get { return _positionsFilled; }
        set { _positionsFilled = value; }
    }



    private void InitJobRelated()
    {
        _maxPeople = Book.GiveMeStat(HType).MaxPeople;
    }

    /// <summary>
    /// Called to fill one position in the job place
    /// </summary>
    public void FillPosition()
    {
        _positionsFilled++;
        CheckIfNoOpenPosLeftThenRemoveFromList();
    }

    /// <summary>
    /// Will check if positions are still open on this job site if not then 
    /// remove from List 
    /// </summary>
    void CheckIfNoOpenPosLeftThenRemoveFromList()
    {
        if (_positionsFilled == _maxPeople)
        {
            BuildingPot.Control.WorkOpenPos.Remove(MyId);
//            Debug.Log(MyId+" removed from curr Jobs");
        }
    }

    /// <summary>
    /// Called when a person leave this job to find a better one 
    /// </summary>
    public void RemovePosition()
    {
        _positionsFilled--;

        if (Instruction == H.WillBeDestroy)
        {
            return;
        }

        CheckIfNeedsToBeAddedToList();
    }

    /// <summary>
    /// Checks if building can be added to the list 
    /// </summary>
    void CheckIfNeedsToBeAddedToList()
    {
        if (_positionsFilled >= _maxPeople)
        {
            return;   
        }

        if (BuildingPot.Control.WorkOpenPos.Contains(MyId))
        {
            return;
        }

        //add to list 
        BuildingPot.Control.WorkOpenPos.Add(MyId);
//        Debug.Log(MyId + " Added to curr Jobs");
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

        if (toBeFill == null)
        {
            throw new Exception(newP.MyId + " . " +newHome.MyId);
        }

        newP.IsBooked = false;
        AssignBookedRole(newP, toBeFill);
        toBeFill.Home = MyId;//bz the id if is booked is not saved 

        newP.transform.parent = transform;

        //so families are resaved 
        BuildingPot.Control.Registro.ResaveOnRegistro(MyId);
    }

    void AssignBookedRole(Person newP, Family toBeFill)
    {
//        print("BookedRole:" + newP.MyId);

        if (toBeFill == null)
        {
            print("toBeFill == nul :" + newP.MyId);
            //return;
        }


        for (int i = 0; i < BookedHome1.Family.Kids.Count; i++)
        {
            if (newP.MyId == BookedHome1.Family.Kids[i])
            {
                toBeFill.Kids.Add(newP.MyId);
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

    private string markTheFirstFamily;

    /// <summary>
    /// Use to mark the first family , is done bz when was asked the first time the 'Familes' were
    /// not created yet
    /// </summary>
    public void MarkFirstFamily(string famID)
    {
        markTheFirstFamily = famID;
    }

    /// <summary>
    /// Will return a family that is empty and has not ID set yet 
    /// </summary>
    /// <returns></returns>
    internal Family FindVirginFamily()
    {
        if (Families == null)
        {
            
            return null;
        }

        for (int i = 0; i < Families.Length; i++)
        {
            if (Families[i].IsFamilyEmpty() && string.IsNullOrEmpty(Families[i].FamilyId))
            {
                return Families[i];
            }
        }
        return null;
    }

#endregion




    #region AnimalFarm


    List<Animal> _animals = new List<Animal>();//the animals in a AnimalFarm 

    private void InitFarm()
    {
        if (HType.ToString().Contains(H.Farm + ""))
        {
            if (HType.ToString().Contains(H.AnimalFarm + ""))
            {
                InitAnimalFarm();
            }
        
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
            SpawnFarmAnimals(H.Large);
        }
        else if (HType == H.AnimalFarmXLarge)
        {
            SpawnFarmAnimals(H.XLarge);
        } 
    }

    private void SpawnFarmAnimals(H size)
    {
        var animalFactor = AmountOfAnimalFactor();
        if (size == H.Small)
        {
            SpawnAnimalNow(2 * animalFactor);
        }
        else if (size == H.Med)
        {
            SpawnAnimalNow(3 * animalFactor);
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
            Animal t = Beef.CreateBeef(iniPos, this);
            _animals.Add(t);
        }
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
    /// Will return the factor of animals to put in an animal farm .
    /// </summary>
    /// <param name="animalType"></param>
    /// <returns></returns>
    int AmountOfAnimalFactor()
    {
        var animalType = CurrentProd;

        if (animalType == P.Chicken)
        {
            return 5;
        }
        if (animalType == P.Pork)
        {
            return 3;
        }
        if (animalType == P.Beef)
        {
            return 2;
        }
        return -1;
    }

#endregion


#region DryDock and Supplier 

    DryDock _dryDock;

    private void InitDryDockAndSupplier()
    {
        if (HType == H.DryDock || HType == H.Supplier)
        {
            _dryDock = new DryDock(this);
            _dispatch = new Dispatch();
        }
    }

    #endregion


    #region Dock

    private Dispatch _dispatch;//dock will have a Dispatch

    public Dispatch Dispatch1
    {
        get { return _dispatch; }
        set { _dispatch = value; }
    }



    private Ship _debugShip;

    private void InitDock()
    {
        if (HType!=H.Dock)
        {
            return;
        }

        _dispatch = new Dispatch();
        _debugShip = new Ship(this);
    }

    /// <summary>
    /// ACtion from the user tht need an 'item' to be import 
    /// </summary>
    /// <param name="item"></param>
    public void Import(Order order)
    {
        _dispatch.AddToExpImpOrders(order);
    }

    /// <summary>
    /// ACtion from the User when needs Export and order 
    /// </summary>
    /// <param name="order"></param>
    public void Export(Order order)
    {
        Order exp = new Order();
        exp = order;
        _dispatch.AddToExpImpOrders(exp);


        //so Dockers starts looking for this in the Storage Buildings 
        Order local = new Order();
        local = order;
        _dispatch.AddToOrders(local);
    }




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

    int FirstUpgradeAmt()
    {
        var baseCap = Book.GiveMeStat(HType).Capacity;
        return baseCap /3;
    }

    int SecondUpgradeAmt()
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
    private void PayUpgradeFee(int fee)
    {
        Program.gameScene.GameController1.Dollars -= fee;
    }



    #endregion




    
}

/// <summary>
/// The ship calss is the one that ask for Import and Exports on the Dock ExportImportDispath
/// </summary>
public class Ship
{
    private Building _dock;
    Inventory _inventory = new Inventory();
    private float _size = 20f;

    public Building Dock
    {
        get { return _dock; }
        set { _dock = value; }
    }

    public Inventory Inventory1
    {
        get { return _inventory; }
        set { _inventory = value; }
    }

    public float Size
    {
        get { return _size; }
        set { _size = value; }
    }

    public Ship(Building dock)
    {
        _dock = dock;
        DebugInit();
    }

    /// <summary>
    /// To emulate the user entering orders 
    /// </summary>
    void DebugInit()
    {
        //_dock.Export(new Order(P.Gold, "Ship", 1));




        Order order = new Order(P.Axe, "", "Ship");
        order.Amount = 1;
        _dock.Import(order);
    }

    void CheckIfImportOrders()
    {
        if (_dock.Dispatch1.HasImportOrders())
        {
            _dock.Dispatch1.Import(_dock);
        }    
    }
    
    private void CheckIfExportOrders()
    {
        if (_dock.Dispatch1.HasExportOrders())
        {
            _dock.Dispatch1.Export(_dock);
        }
    }



    private float lastCheck;
    public void Update()
    {
        if (Time.time > lastCheck + 10f)
        {
            CheckIfImportOrders();
            CheckIfExportOrders();

            lastCheck = Time.time;
        }
    }







}











/// <summary>
/// Created to booked homes to familyes so they are kept toghether 
/// </summary>
public class BookedHome
{
    public string Building;//the key of the building there are booked to
    public Family Family;//the family tht booked this building

    public BookedHome() { }

    public BookedHome(string building, Family family)
    {
        Building = building;
        Family = family;
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
        return Family.DoIBelongToThisFamily(person) && 
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
        personToRemove.IsBooked = false;

        //everyone was added to the new place and the boking is clear 
        if (Family.IsFamilyEmpty())
        {
            //so is not there anymore as a house with space to get new people. This hose is already
            //ocuppied by a family... and the family is formed then can be removed

            var building = Brain.GetBuildingFromKey(Building);

            //if (!building.IsALeastOneFamilyEmpty() && building.AllFamiliesFormed())
            if (building != null && building.AllFamiliesFull())
            {
                BuildingPot.Control.RemoveFromHousesWithSpace(Building);
            }
            
            //just addressingn a bugg tht book can happen 
            ClearBooking();
            MakeOldHomeFamilyVarVirgin(personToRemove);

            //so Individuals tht asked and where denied get a chancee to see this building unbooked
            PersonPot.Control.RestartController();
        }

        
    }

    /// <summary>
    /// Will make the old Home Family var virign so can be booked properly on realtor 
    /// </summary>
    private void MakeOldHomeFamilyVarVirgin(Person toRemove)
    {
        var oldHome = Brain.GetBuildingFromKey(toRemove.Brain.MoveToNewHome.OldHomeKey);
        //Debug.Log("Make virgin on");

        if (oldHome!= null)
        {
            //Debug.Log("Make virgin on oldHome!= null");
            var fam = oldHome.FindFamilyById(toRemove);

            if (fam != null)
            {
                fam.DeleteFamily();
                fam.MakeVirgin();

                Debug.Log("Made family virgin on");

                //so families are resaved 
                //BuildingPot.Control.Registro.UpdateOnRegistro(oldHome.MyId);
            }
        }
    }
}
