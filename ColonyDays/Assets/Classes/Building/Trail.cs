using UnityEngine;
using System.Collections.Generic;

//The Trail adnd the Road are obj instanses of this class
public class Trail : Way 
{
    private List<CreatePlane> _planesListVertic = new List<CreatePlane>();
    private List<CreatePlane> _planesListHor = new List<CreatePlane>();
  
    private int scaleCounter;

    private int _counter;
    protected bool _isToSetList;
    private bool _isBuildingWay;

    private H _currentLoop = H.None;//what the class is currently looping thru using the Update()

    //Bridges
    //to save on disk data
    private List<Vector3> _tilePosVert = new List<Vector3>();//the position for each tile and for a bridege the starting point of a part
    private List<Vector3> _tilePosHor = new List<Vector3>();//the position for each tile and for a bridege the starting point of a part
    private List<Vector3> _planesOnAirPos = new List<Vector3>();//the planes position on the air of a bridge 
    
    private Vector3 _tileScale = new Vector3();//the scale of a tile
    private List<int> _partsOnAir = new List<int>(); //the birdge part sequence

    private List<StructureParent> _pieces = new List<StructureParent>();//of a bridge

    //this are the planes of the bridge that are not in the air
    private List<Vector3> _planesOnSoil = new List<Vector3>();
    
    //contains the shore elements of the bridge
    //in the seq /-- --\ in the middle of that goes the birdges pieces 
    // / represents an 11,,,     // - represents an 12
    List<int> _partsOnSoil = new List<int>();

    //the starintg stage of each part of a  brdige
    protected H _startingStageForPieces;

    public List<int> PartsOnSoil
    {
        get { return _partsOnSoil; }
        set { _partsOnSoil = value; }
    }

    public List<Vector3> PlanesOnSoil
    {
        get { return _planesOnSoil; }
        set { _planesOnSoil = value; }
    }

    public List<StructureParent> Pieces
    {
        get { return _pieces; }
        set { _pieces = value; }
    }

    public List<CreatePlane> PlanesListVertic
    {
        get { return _planesListVertic; }
        set { _planesListVertic = value; }
    }

    public List<CreatePlane> PlanesListHor
    {
        get { return _planesListHor; }
        set { _planesListHor = value; }
    }

    public Vector3 TileScale
    {
        get { return _tileScale; }
        set { _tileScale = value; }
    }

    public List<int> PartsOnAir
    {
        get { return _partsOnAir; }
        set { _partsOnAir = value; }
    }

    public List<Vector3> TilePosVert
    {
        get { return _tilePosVert; }
        set { _tilePosVert = value; }
    }

    public List<Vector3> TilePosHor
    {
        get { return _tilePosHor; }
        set { _tilePosHor = value; }
    }

    public H CurrentLoop
    {
        get { return _currentLoop; }
        set { _currentLoop = value; }
    }

    public List<Vector3> PlanesOnAirPos
    {
        get { return _planesOnAirPos; }
        set { _planesOnAirPos = value; }
    }

    public H StartingStageForPieces
    {
        get { return _startingStageForPieces; }
        set { _startingStageForPieces = value; }
    }

    public void Demolish()
    {
        PositionFixed = false;

        if (_planesListVertic.Count > 0)
        {
            for (int i = 0; i < _planesListVertic.Count; i++)
            {
                _planesListVertic[i].Destroy();
            }
        }

        if (_planesListHor.Count > 0)
        {
            for (int i = 0; i < _planesListHor.Count; i++)
            {
                _planesListHor[i].Destroy();
            }
        }
        BuildingPot.Control.Registro.RemoveItem(Category, MyId);
        //_isOrderToDestroy = true;
        //DestroyOrdered();
    }

    public void AssignNewMaterialToPlanes(Material newMat)
    {
        for (int i = 0; i < _planesListVertic.Count; i++)
        {
            _planesListVertic[i].Geometry.GetComponent<Renderer>().sharedMaterial = newMat;
        }
        for (int i = 0; i < _planesListHor.Count; i++)
        {
            _planesListHor[i].Geometry.GetComponent<Renderer>().sharedMaterial = newMat;
        }
    }

    public void DonePlace()
    {
        //if good terrain heigt etc
        if (IsWayOk)
        {
            RemoveOverLapCorner(0.1f);

            if (BuildingPot.Control.BuildWayCursor != null)
            {
                BuildingPot.Control.BuildWayCursor.Destroy();
            }

            DestroyProjector();

            _isToSetList = true;
            _currentLoop = H.TerraSpawn;

            DestroyBigPrevBoxes();
        }
        else if(!IsWayOk)
        {
            if (!IsWayEven)
            {
                GameScene.ScreenPrint("Way not even");
            }
            else if (IsWayColliding)
            {
                GameScene.ScreenPrint("Way collid");
            }
            else if(!IsWayAboveWater)
            {
                GameScene.ScreenPrint("Way bellow water");
            }
        }
    }

    /// <summary>
    /// This is the routine that is looped with the Update() will change CurrentLoop
    /// thru all steps  
    /// </summary>
    void SetListsRoutine()
    {
        if (CurrentLoop == H.TerraSpawn)
        {
            if (_prevWayHor.Count > 0)
            {
                MarkTerraSpawnRoutine(_prevWayHor[0].Radius, UList.ReturnTheVector3List(_prevWayHor));
            }
            if (_prevWayVertic.Count > 0)
            {
                MarkTerraSpawnRoutine(_prevWayVertic[0].Radius, UList.ReturnTheVector3List(_prevWayVertic));
            }

            _currentLoop = H.Vertic;
        }
        else if(CurrentLoop == H.Vertic)
        {
            if (_counter < _verticPathNew.Count)
            {
                _subMeshPathVertic.Add(FindSubMeshVert(_verticPathNew[_counter]));
                _planesDimVertic.Add(ReturnPlanesDim(_subMeshPathVertic[_counter]));
                _counter++;
            }
            else
            {
                _counter = 0;
                _currentLoop = H.Horiz;
            }
        }
        else if (CurrentLoop == H.Horiz)
        {
            if (_counter < _horPathNew.Count)
            {
                _subMeshPathHor.Add(FindSubMeshVert(_horPathNew[_counter]));
                _planesDimHor.Add(ReturnPlanesDim(_subMeshPathHor[_counter]));
                _counter++;
            }
            else
            {
                _counter = 0;
                _currentLoop = H.PlanesVertic;
                _isToSetList = false;
                _isBuildingWay = true;
            }
        }
    }

    /// <summary>
    /// This is the last thing to do with the Object fixes position and addRectanges to Registro
    /// </summary>
    void FinishCurrent()
    {
        PositionFixed = true;
        for (int i = 0; i < PlanesListVertic.Count; i++)
        {
            PlanesListVertic[i].PositionFixed = true;
            _tilePosVert.Add(PlanesListVertic[i].transform.position);
        }
        for (int i = 0; i < PlanesListHor.Count; i++)
        {
            PlanesListHor[i].PositionFixed = true;
            _tilePosHor.Add(PlanesListHor[i].transform.position);
        }
        //only  AddRectangles if is Trail or Road. Bridges call AddRectangles() from that class
        if (HType == H.Trail /*|| HType == H.Road*/)
        {
            if (BoundsVertic.Count > 0 && !IsLoadingFromFile)
            {
                AddWayToRegistro();
            }
        }
        BuildingPot.InputU.MouseUp();
    }

    public void AddWayToRegistro()
    {
        BuildingPot.Control.Registro.AddBuildToAll(MyId, HType, BoundsVertic, Category,
            transform.position, 
            Inventory,  
            PeopleDict,
            polyHoriz: BoundsHoriz, tilePosVert: TilePosVert, tilePosHor: TilePosHor, planesOnAirPos: PlanesOnAirPos, tileScale: TileScale, parts: PartsOnAir, dominantSide: _dominantSide,
            startingStage: _startingStageForPieces,
            materialKey: MaterialKey, planesOnSoilPos: _planesOnSoil, partsOnSoil: _partsOnSoil,
            instructionP: Instruction);
    }

    List<Vector3> GetActiveBound()
    {
        List<Vector3> activeBound = new List<Vector3>();

        if (_dominantSide == H.Vertic)
        {
            activeBound = BoundsVertic;
        }
        else if (_dominantSide == H.Horiz)
        {
            activeBound = BoundsHoriz;
        }
        return activeBound;
    }

    public void AddBridgeToRegistro()
    {
        Debug.Log("AddBridgeToRegistro()");

        var activeBound = GetActiveBound();

        Registro.oldBridge = this;

        BuildingPot.Control.Registro.AddBuildToAll(MyId, HType,  activeBound, Category,
            transform.position,
            Inventory,  
            PeopleDict,
            polyHoriz: null , tilePosVert: TilePosVert, tilePosHor: TilePosHor, planesOnAirPos: PlanesOnAirPos, 
            tileScale: TileScale, parts: PartsOnAir, dominantSide: _dominantSide,
            startingStage: _startingStageForPieces, materialKey: MaterialKey, planesOnSoilPos: _planesOnSoil, partsOnSoil: _partsOnSoil
            , min: activeBound[1], max: activeBound[3] ,
            instructionP: Instruction);
    }

	// Use this for initialization
	protected void Start () 
    {
        base.Start();
        _isFakeObj = true;
	}
	
	// Update is called once per frame
	protected void Update ()
	{
        base.Update();

        if (_isOrderToDestroy) { DestroyOrdered();}

        if (!PositionFixed && CurrentLoop != H.Done)
        {
            
            if (_isToSetList) { SetListsRoutine(); }
            if (_isBuildingWay)
            {
                //Create planes the only diff that have is the box coliider attached to them
                if (HType == H.Trail)
                {
                    BuildWay(Root.createPlane);
                }
                //else if(HType == H.Road)
                //{
                //    BuildWay(Root.createPlaneRoad);
                //}
                else if (HType == H.BridgeTrail)
                {
                    //im adding here bz if u doit after will move the created planes positions
                    //so the collider has to be added first. Is added so is reconinzed on Router.cs
                    AddBoxCollider(GetActiveBound()[1], GetActiveBound()[3]);

                    BuildWay(Root.createPlane, makeWayInvisible: true);
                }
                else if (HType == H.BridgeRoad)
                {
                    AddBoxCollider(GetActiveBound()[1], GetActiveBound()[3]);
                    BuildWay(Root.createPlaneRoad, makeWayInvisible: true);
                }
            }
        }
	}



    /// <summary>
    ///  Creates the trail.
    /// </summary>
    void BuildWay(string rootPlane, bool makeWayInvisible = false)
    {
        Material baseMat = Resources.Load<Material>(Root.RetMaterialRoot(MaterialKey));

        if (CurrentLoop == H.PlanesVertic)
        {
            if (_counter < _planesDimVertic.Count && _prevWayVertic.Count > 0)
            {
                PlanesListVertic.Add(CreatePlane.CreatePlan(rootPlane, Root.matGravilla, m.HitMouseOnTerrain.point,
                    container: transform, mat: baseMat));

                //if is the last one or 1st I will show it even if is set to be not seen like I do in bridges
                if (_counter == _planesDimVertic.Count - 1 || _counter == 0)
                {
                    makeWayInvisible = false; 
                }
                PlanesListVertic[_counter].UpdatePos(_planesDimVertic[_counter], makeThisInvisible: makeWayInvisible);
                
                _counter++;
            }
            else
            {
                _counter = 0;
                _currentLoop = H.PlanesHor;
            }
        }
        else if(CurrentLoop == H.PlanesHor)
        {
            if (_counter < _planesDimHor.Count && _prevWayHor.Count > 0)
            {
                PlanesListHor.Add((CreatePlane.CreatePlan(rootPlane, Root.matGravilla, m.HitMouseOnTerrain.point, 
                    container: transform, mat: baseMat)));
                   
                //if is the last one I will show it even if is set to be not seen like I do in bridges
                if (_counter == _planesDimHor.Count - 1 || _counter == 0)
                {
                    makeWayInvisible = false;
                }
                PlanesListHor[_counter].UpdatePos(_planesDimHor[_counter], makeThisInvisible: makeWayInvisible);
                
                _counter++;
            }
            else
            {
                DoneWithLoop();
            }
        }
    }

    void DoneWithLoop()
    {
        //is called here bz if not will leave a few behind on game scene since we remove corners after we ever update those
        ClearPrevWay();

        _currentLoop = H.Done;
        _counter = 0;
        _isBuildingWay = false;
        FinishCurrent();

  
    }
}
