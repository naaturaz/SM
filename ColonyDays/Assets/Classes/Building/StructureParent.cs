using System;
using System.Collections.Generic;
using UnityEngine;

//Created so Bridge Units can instantiate this class.
//Structure.cs class is to specific to structures like houses

public class StructureParent : Building {

    //the plane is shown in the botton of the builiding
    protected CreatePlane basePlane;

    //construtction stages of a building... Stage 1 is just one plane on the bottom
    private GameObject _stage2;
    private GameObject _stage3;

    private GameObject _spawnPoint;

    private Vector3 _behindMainDoorPoint = new Vector3();//calculated in base of the _spawnPoint and anchors

    //stages that a building goes trhuewhile is being buiild
    //1 Plane in the ground, 2 fundition, 3 paredes palos, 4 all geometry
    protected int _currentStage;


    private Material _stagesMaterial;

    //Wheel
    private GameObject wheel;
    private bool rotateWheel;

    public GameObject Stage2
    {
        get
        {
            if (_stage2 == null)
            {
                _stage2 = GetChildCalled(H.Stage2);
                if (_stage2 == null)
                {
                    //here im doing this bz I have a lot of objects that their Stage2 3d obj name
                    //is far from Stage2 is the Scene Name + s2
                    _stage2 = GetChildLastWordIs(H.s2);
                }
            }
            return _stage2;
        }
        set
        {
            if (_stage2 == null)
            {
                _stage2 = GetChildCalled(H.Stage2);
                if (_stage2 == null)
                {
                    _stage2 = GetChildLastWordIs(H.s2);
                }
            }
            _stage2 = value;
        }
    }

    public GameObject Stage3
    {
        get
        {
            if (_stage3 == null)
            {
                _stage3 = GetChildCalled(H.Stage3);
                if (_stage3 == null)
                {
                    _stage3 = GetChildLastWordIs(H.s3);
                }
            }
            return _stage3;
        }
        set
        {
            if (_stage3 == null)
            {
                _stage3 = GetChildCalled(H.Stage3);
                if (_stage3 == null)
                {
                    _stage3 = GetChildLastWordIs(H.s3);
                }
            }
            _stage3 = value;
        }
    }

    /// <summary>
    /// Will refind the SpawnPoint pos
    /// </summary>
    /// <returns></returns>
    public GameObject ResetedSpawnPoint()
    {
        _spawnPoint = null;
        return SpawnPoint;
    }

    public GameObject SpawnPoint
    {
        get
        {
            if (_spawnPoint == null)
            {
                _spawnPoint = GetChildThatContains(H.SpawnPoint);
                Vector3 t = _spawnPoint.transform.position;
                t.y = m.IniTerr.MathCenter.y;//so all spawnpoints are on the right Y
                _spawnPoint.transform.position = t;
            }
            return _spawnPoint;
        }
        set
        {
            if (_spawnPoint == null)
            {
                _spawnPoint = GetChildThatContains(H.SpawnPoint);
                Vector3 t = _spawnPoint.transform.position;
                t.y = m.IniTerr.MathCenter.y;//so all spawnpoints are on the right Y
                _spawnPoint.transform.position = t;
            }
            _spawnPoint = value;
        }
    }


    public Vector3 BehindMainDoorPoint
    {
        get
        {
            if (_behindMainDoorPoint == new Vector3())
            {
                _behindMainDoorPoint = FindPointBehindDoor();
            }
            return _behindMainDoorPoint;
        }
        set
        {
            if (_behindMainDoorPoint == new Vector3())
            {
                _behindMainDoorPoint = FindPointBehindDoor();
            }
            _behindMainDoorPoint = value;
        }
    }
    
    /// <summary>
    /// Bz the prop doesnt have a real setter 
    /// </summary>
    /// <param name="newVa"></param>
    public void SetBehindMainDoorPoint(Vector3 newVa)
    {
        _behindMainDoorPoint = newVa;
    }



    public Material StagesMaterial
    {
        get { return _stagesMaterial; }
        set { _stagesMaterial = value; }
    }

    public int CurrentStage
    {
        get { return _currentStage; }
        set { _currentStage = value; }
    }

   

    #region Working Places Prop

    //For works places that have in building routes. Like fishing site 
    private Vector3 _inBuildIniPoint;
    private Vector3 _inBuildMidPointA;

    private Vector3 _inBuildWorkPoint01;
    private Vector3 _inBuildWorkPoint02;
    private Vector3 _inBuildWorkPoint03;


    public Vector3 InBuildIniPoint
    {
        get
        {
            return FindVector3IfNull(_inBuildIniPoint, H.InBuildIniPoint);
        }
    }

    public Vector3 InBuildMidPointA
    {
        get
        {
            return FindVector3IfNull(_inBuildMidPointA, H.InBuildMidPointA);
        }
    }

    public Vector3 InBuildWorkPoint01
    {
        get { return FindVector3IfNull(_inBuildWorkPoint01, H.InBuildWorkPoint01); }
    }

    public Vector3 InBuildWorkPoint02
    {
        get { return FindVector3IfNull(_inBuildWorkPoint02, H.InBuildWorkPoint02); }
    }

    public Vector3 InBuildWorkPoint03
    {
        get { return FindVector3IfNull(_inBuildWorkPoint03, H.InBuildWorkPoint03); }
    }

    #endregion

    /// <summary>
    /// Will Determine if I need to find the position other wise wil return 'current'
    /// </summary>
    /// <param name="current"></param>
    /// <param name="byGameObjName">The child gameObj name pos Im looking for </param>
    /// <returns></returns>
    Vector3 FindVector3IfNull(Vector3 current, H byGameObjName)
    {
        if (current == null || current == new Vector3())
        {
            return GetVector3OnChild(byGameObjName);
        }
        return current;
    }

    Vector3 GetVector3OnChild(H byGameObjName)
    {
        return GetChildCalled(byGameObjName).transform.position;
    }

    static public StructureParent CreateStructureParent(string root, Vector3 origen, H hType, string name = "", Transform container = null,
       bool isLoadingFromFile = false, string materialKey = "", H startingStage = H.None)
    {
        WAKEUP = true;
        StructureParent obj = null;
        obj = (StructureParent)Resources.Load(root, typeof(Building));
        obj = (StructureParent)Instantiate(obj, origen, Quaternion.identity);
        obj.HType = hType;
        obj.transform.name = obj.MyId = obj.Rename(obj.transform.name, obj.Id, obj.HType, name);
        obj.IsLoadingFromFile = isLoadingFromFile;

        obj.ClosestSubMeshVert = origen;
        if (name != "") { obj.name = name; }
        if (container != null) { obj.transform.SetParent( container); }

        if (materialKey == "")
        { materialKey = hType + "." + Ma.matBuildBase; }

        obj.StartingStage = startingStage;
        //obj.MaterialKey = materialKey;
        //obj.Geometry.GetComponent<Renderer>().sharedMaterial = Resources.Load(Root.RetMaterialRoot(materialKey)) as Material;

        return obj;
    }

    /// <summary>
    /// Return the point inside the building opositing SpawnPoint
    /// Needed to be the very first and last points on routes 
    /// </summary>
    Vector3 FindPointBehindDoor()
    {
        float dist = 1f;

        //the shores one have the pivot al reves
        if (Category == Ca.Shore)
        {
            dist *= -1;
        }

        // RotationFacerIndex is: 0 is up, 1 is right, 2 is down, 3 is left
        var add = new[]
        {new Vector3(0, 0, -dist), new Vector3(-dist, 0, 0), new Vector3(0, 0, dist), new Vector3(dist, 0, 0)};

        Vector3 t = SpawnPoint.transform.position + add[RotationFacerIndex];
        return t;
    }

    /// <summary>
    /// This will recreate the building satgae(1 only the base plane,2,3,4 for fully) builded .
    /// This on is used to Load the building 
    /// </summary>
    protected void RecreateStage()
    {
        Geometry.gameObject.SetActive(false);
        UpdateBuild();

        //the bridges parts are called Units... u dont want planes on those...
        if (!HType.ToString().Contains("Unit") && Category != Ca.Shore && HType != H.MountainMine
            )
        {
            CreateBasePlane();
        }
        HandleMeshChild(_startingStage);

        FinishPlacingMode(H.Done);
        //so I defined the current stage so when we use the NextStage is ok
        _currentStage = ReturnCurrentStageInt(_startingStage);
        _startingStage = H.None;//has to be set to none so in Update() now goes to ShowNextStage();
    }

    //Hide the wheel of the mill and the mine
    protected void ShowWheel(bool show)
    {
        
        if (HType == H.Mill && Instruction != H.WillBeDestroy)
        {
            GameObject wheel = GetChildLastWordIs(H.Wheel);
            if (wheel == null) { throw new Exception("Obj doenst have obj attached called |...Wheel|");}

            //print(HType + ".HType.Show" + show);

            wheel.SetActive(show);
        }
    }

    protected void ToggleWheelRotate()
    {
        if (HType == H.Mill// || HType == H.Mine
            )
        {
            rotateWheel = !rotateWheel;
        }
    }

    protected void RotateWheel()
    {
        float speed = 0.5f;
        if (wheel == null){wheel = GetChildLastWordIs(H.Wheel);}
        if (wheel != null)
        {
            wheel.transform.Rotate(new Vector3(0, 0, speed));
        }
    }




    int ReturnCurrentStageInt(H stage)
    {
        if (H.Stage1 == stage)
        {
            return 1;
        }
        if (H.Stage2 == stage)
        {
            return 2;
        }
        if (H.Stage3 == stage)
        {
            return 3;
        }
        if (H.Done == stage)
        {
            return 4;
        }
        return -1;
    }

    /// <summary>
    /// Shwo next stange of a building from stage1 to done 
    /// </summary>
    public virtual void ShowNextStage()
    {
        //Debug.Log("ShowNext on StruPar");

        if (_currentStage < 4) { _currentStage++; }
        if (_currentStage == 1)
        {
            if (!HType.ToString().Contains("Unit") && !IsThisADoubleBoundedStructure())
            {
                CreateBasePlane();
            }
            HandleMeshChild(H.Stage1);
        }
        else if (_currentStage == 2)
        {
            //HandleMeshChild(H.Stage2);
        }
        else if (_currentStage == 3)
        {
            //HandleMeshChild(H.Stage3);
        }
        else if (_currentStage == 4)
        {
            HandleMeshChild(H.Done);
            HandleLastStage();
        }
    }

    /// <summary>
    /// Creates a Plane for the base of the Strcuture 
    /// </summary>
    void CreateBasePlane()
    {
        var locPoly = UPoly.ScalePoly(Anchors, 0.04f);
        basePlane = CreatePlane.CreatePlan(Root.createPlane, ReturnMatBase(), raiseFromFloor: 0.08f, container: transform);
    }

    string ReturnMatBase()
    {
        if (MyId.Contains("Farm"))
        {
            return Root.matBuildingBase2;
        }

        return Root.matBuildingBase3;
    }

    //Resave the .StartingStage on Control.Registro.All
    public void ResaveOnRegistro(H stage, string myIdP)
    {
        //Savign the Srating Stage on All everytime is changed here 
        int index = BuildingPot.Control.Registro.AllRegFile.FindIndex(a => a.MyId == myIdP);
        BuildingPot.Control.Registro.AllRegFile[index].StartingStage = stage;
    }

    /// <summary>
    /// Handles the geomtry subobject of a structure will do the sequence of building from stage1 to done
    /// it hides and shows geomtries
    /// </summary>
    void HandleMeshChild(H name)
    {
        //if its not done pay fee for using next stage 
        if (name != H.Done)
        {
            //no fees will be payed for now. Removed bz on Stage1 a fee is payed and throws Ledger off
            //this fees are intended as a pay now to get a building finish

            //NextStageFee(); 
        }



        _startingStage = name;
        Geometry.gameObject.SetActive(false);
        if (Stage2 != null) Stage2.gameObject.SetActive(false);
        if (Stage3 != null) Stage3.gameObject.SetActive(false);

        //so UnderTerra mines show something onces there are built
        if (name == H.Stage1 && IsThisADoubleBoundedStructure())
        {
            if (Stage2 != null)
            {
                Stage2.gameObject.SetActive(true);
                AssignMaterialToStage(Stage2);
            }
        }
        else if (name == H.Stage2)
        {
            //basePlane.Geometry.renderer.material =  (Material)Resources.Load(Root.matBuildingBase2) ;
            if (Stage2 != null)
            {
               Stage2.gameObject.SetActive(true);
               AssignMaterialToStage(Stage2);
            }
        }
        else if (name == H.Stage3)
        {
            if (Stage3 != null)
            {
                Stage3.gameObject.SetActive(true);
                AssignMaterialToStage(Stage3);
            }
        }
        else if (name == H.Done)
        {
            Geometry.gameObject.SetActive(true);
            ShowWheel(true);
        }

        //this is here bz units stuff must be saved with the bridge
        if (!HType.ToString().Contains("Unit"))
        {ResaveOnRegistro(name, MyId);}
    }

    /// <summary>
    /// Everytime the next stage is used a fee must be paid 
    /// </summary>
    private void NextStageFee()
    {
        Program.gameScene.GameController1.Dollars -= ReturnFee(HType);
    }

    int ReturnFee(H typeP)
    {
        return 500;
    }


    //assign the material Stage to all Stage 2 or 3 passed but a few ex
    //this is here to address the exepctions
    void AssignMaterialToStage(GameObject passP)
    {
        if (!HType.ToString().Contains(H.Bridge.ToString()) && HType != H.Dock && HType != H.Shipyard
            && HType != H.FishingHut// && HType != H.FishRegular
            )
        {
            passP.GetComponent<Renderer>().sharedMaterial = Resources.Load(Root.RetMaterialRoot(H.Stages.ToString())) as Material;
        }
        if (Category == Ca.Shore)
        {
            passP.GetComponent<Renderer>().sharedMaterial = Resources.Load(Root.blue_Semi_T) as Material;
        }
    }


	// Use this for initialization
	protected void Start ()
	{
        //just i know it works 
        ToggleWheelRotate();

        //this is here bz gave me a null ref ex
	    if (!HType.ToString().Contains(H.Bridge.ToString()))
	    {
	        Stage2.SetActive(false);
	        Stage3.SetActive(false);
	    }

	    if (HType.ToString().Contains("Unit"))
	    {
	        PositionFixed = true;
            if (_startingStage == H.None)
            {
                _startingStage = H.Stage2;
            }
	    }
	    
        base.Start();

        //this is for BridgeUnits in here 
        if (PositionFixed && _currentStage == 0)
        {
            if (_startingStage != H.None && _currentStage == 0)
            {
                RecreateStage();
            }
        }
	}


    bool wasBasePlaneUpdated;
	// Update is called once per frame
    protected void Update()
    {
	    base.Update();

        if (rotateWheel) { RotateWheel();}


        if (wasBasePlaneUpdated || basePlane==null)
        {
            return;
        }
        wasBasePlaneUpdated = true;
        basePlane.UpdatePos(Anchors);
	}


    #region Tops and Bottom of Piece 12

    private List<GameObject> _bottoms = new List<GameObject>();
    private List<GameObject> _tops = new List<GameObject>();

    System.Random rand = new System.Random();
    private int picked;

    /// <summary>
    /// The Botton Middle point of a Piece 12 Bridge
    /// </summary>
    /// <returns></returns>
    public GameObject BottonMiddle()
    {
        LoadBottomsAndTops();

        return _bottoms[1];
    }   
    
    public Vector3 BottonIn()
    {
        LoadBottomsAndTops();

        return _bottoms[2].transform.position;
    }   
    
    public Vector3 BottonOut()
    {
        LoadBottomsAndTops();

        return _bottoms[0].transform.position;
    }

    internal Vector3 TopIn()
    {
        LoadBottomsAndTops();

        return _tops[2].transform.position;
    }

    internal Vector3 TopOut()
    {
        LoadBottomsAndTops();

        return _tops[0].transform.position;
    }


    internal GameObject TopMiddle()
    {
        LoadBottomsAndTops();

        return _tops[1];
    }

    void LoadBottomsAndTops()
    {
        if (_bottoms.Count == 0)
        {
            LoadChildGameObj(_bottoms, H.Bottom01);
            LoadChildGameObj(_bottoms, H.Bottom02);
            LoadChildGameObj(_bottoms, H.Bottom03);
        }
        if (_tops.Count == 0)
        {
            LoadChildGameObj(_tops, H.Top01);
            LoadChildGameObj(_tops, H.Top02);
            LoadChildGameObj(_tops, H.Top03);
        }
    }


    GameObject Bottom()
    {
        if (_bottoms.Count == 0)
        {
            LoadChildGameObj(_bottoms, H.Bottom01);
            LoadChildGameObj(_bottoms, H.Bottom02);
            LoadChildGameObj(_bottoms, H.Bottom03);
        }
        picked = rand.Next(0, _bottoms.Count);
        return _bottoms[picked];
    }

    GameObject Top()
    {
        if (_tops.Count == 0)
        {
            LoadChildGameObj(_tops, H.Top01);
            LoadChildGameObj(_tops, H.Top02);
            LoadChildGameObj(_tops, H.Top03);
        }
        //bz must be the same Top and Down
        
        return _tops[picked];
    }

    /// <summary>
    /// Will return random row in the brdige 
    /// </summary>
    /// <returns></returns>
    public GameObject[] BottomTop()
    {
        return new GameObject[] { Bottom(), Top() };
    }

    /// <summary>
    /// If index is -1 iwll return bot and top randomly, wil assing val to 'index' as weell
    /// if index is diff that than will return specific value passed
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public GameObject[] BottomTop(CanIReach canIReach)
    {
        if (canIReach.PairUsed == -1)
        {
            var t = new GameObject[] { Bottom(), Top() };
            canIReach.PairUsed = picked;
            return t;
        }
        else
        {
            //so they load
            Bottom();
            Top();
            return new GameObject[] { _bottoms[canIReach.PairUsed], _tops[canIReach.PairUsed] };
        }
    }

    void LoadChildGameObj(List<GameObject> list, H typPass)
    {
        list.Add(GetChildCalled(typPass));
    }

    #endregion







}
