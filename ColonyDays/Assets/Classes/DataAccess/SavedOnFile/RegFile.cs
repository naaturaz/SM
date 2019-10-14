using System.Collections.Generic;
using UnityEngine;

public class RegFile  {

    //Structures Propertie
    private string _myId;
    private H _hType;
    private Ca _category;
    Rect _dimOnMap = new Rect();
    private Vector3 _min;//min vector3 on a bound used only for stockpile
    private Vector3 _max;//max vector3 on a bound used only for stockpile
    private Vector3 _iniPos = new Vector3();
    private H _startingStage = H.None;
    private int _rotationFacerIndex;

    //Way properties 
    private Rect _dimOnMapHor = new Rect();//use to hold the second Rect of a way. The horiz

    private List<Vector3> _tilePosVert = new List<Vector3>();//the position for each tile and for a bridege the starting point of a part
    private List<Vector3> _tilePosHor = new List<Vector3>();//the position for each tile and for a bridege the starting point of a part

    private Vector3 _tileScale = new Vector3();
    string _materialKey;//the key of the material of an building

    //Bridge prop
    private List<int> _partsOnAir = new List<int>(); //the birdge part sequence
    private H _dominantSide;
    private List<Vector3> _planeOnAirPos = new List<Vector3>();//the planes position on the air of a bridge
    //this are the planes of the bridge that are not in the air
    private List<Vector3> _planesOnSoil = new List<Vector3>();

    //contains the shore elements of the bridge
    //in the seq /-- --\ in the middle of that goes the birdges pieces 
    // / represents an 11,,,     // - represents an 12
    List<int> _partsOnSoil = new List<int>();


    public List<VectorLand> LandZone1 = new List<VectorLand>();

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


    public string MaterialKey
    {
        get { return _materialKey; }
        set { _materialKey = value; }
    }

    public Rect DimOnMap
    {
        get { return _dimOnMap; }
        set { _dimOnMap = value; }
    }

    public Rect DimOnMapHor
    {
        get { return _dimOnMapHor; }
        set { _dimOnMapHor = value; }
    }

    public Vector3 TileScale
    {
        get { return _tileScale; }
        set { _tileScale = value; }
    }

    public string MyId
    {
        get { return _myId; }
        set { _myId = value; }
    }

    public H HType
    {
        get { return _hType; }
        set { _hType = value; }
    }

    public Ca Category
    {
        get { return _category; }
        set { _category = value; }
    }

    public Vector3 IniPos
    {
        get { return _iniPos; }
        set { _iniPos = value; }
    }

    public List<int> PartsOnAir
    {
        get { return _partsOnAir; }
        set { _partsOnAir = value; }
    }

    public H DominantSide
    {
        get { return _dominantSide; }
        set { _dominantSide = value; }
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

    public List<Vector3> PlaneOnAirPos
    {
        get { return _planeOnAirPos; }
        set { _planeOnAirPos = value; }
    }

    public H StartingStage
    {
        get { return _startingStage; }
        set { _startingStage = value; }
    }

    public int RotationFacerIndex
    {
        get { return _rotationFacerIndex; }
        set { _rotationFacerIndex = value; }
    }

    public Vector3 Min
    {
        get { return _min; }
        set { _min = value; }
    }

    public Vector3 Max
    {
        get { return _max; }
        set { _max = value; }
    }

    public List<string> PeopleDict;


    public Inventory Inventory;
    public H Instruction;
    public BookedHome BookedHome1;//only save and loaded for Structures and Shore Types.

    public Dispatch Dispatch1;
    public Family[] Familes;

    public int DollarsPay;

    //all ways load this but  is only save it by bridges. 
    public Vector3[] Anchors;
    public Dock Dock1;

    public PlantSave PlantSave1;

    //the root of the Building if used.
    //for now will be used only by HouseMed
    public string Root;

    public ProductionReport ProductionReport;
    public int MaxPeople;

    public BuildersManager BuildersManager;

    public RegFile(Building build, Rect dimOnMap, Ca category, Vector3 iniPosition, 
        Inventory InventoryP, 
        List<string> peopleDict,
        List<VectorLand> LandZone1,
        Rect dimOnMapHor = new Rect(),
        List<Vector3> tilePosVert = null, List<Vector3> tilePosHor = null, List<Vector3> planesOnAirPos = null,
        Vector3 tileScale = new Vector3(), List<int> partsOnAir = null,
        H dominantSide = H.None, H startingStage = H.None, int rotationFacerIndex = -1, string materialKey = "",
        List<Vector3> planesOnSoilPos = null, List<int> partsOnSoil = null, Vector3 min = new Vector3(), 
        Vector3 max = new Vector3(), H instructionP = H.None , BookedHome bookedHome = null, 
        Dispatch dispatch = null, Family[] familes = null ,
        int dollarsPay = 0,
        List<Vector3> anchors = null, Dock dock = null, string root = "",
        BuildersManager buildersManager = null
        )
    {
        MyId = build.MyId;
        HType = build.HType;
        _dimOnMap = dimOnMap;
        Category = category;
        _iniPos = iniPosition;
        _dimOnMapHor = dimOnMapHor;
        _tilePosVert = tilePosVert;
        _tilePosHor = tilePosHor;
        _tileScale = tileScale;
        //bridge stuff
        _partsOnAir = partsOnAir;
        _dominantSide = dominantSide;
        _planeOnAirPos = planesOnAirPos;
        _planesOnSoil = planesOnSoilPos;
        _partsOnSoil = partsOnSoil;

        _startingStage = startingStage;
        _rotationFacerIndex = rotationFacerIndex;
        _materialKey = materialKey;
        _min = min;
        _max = max;
        Instruction = instructionP;

        Inventory = InventoryP;
        PeopleDict = peopleDict;
        this.LandZone1 = LandZone1;
        BookedHome1 = bookedHome;

        Dispatch1 = dispatch;
        Familes = familes;
        DollarsPay = dollarsPay;
        Anchors = anchors.ToArray();
        Dock1 = dock;

        Root = root;

        ProductionReport = build.ProductionReport;
        MaxPeople = build.MaxPeople;
        Decoration = build.Decoration1;
        Name = build.NameBuilding();

        BuildersManager = buildersManager;
    }

    public RegFile() { }

    /// <summary>
    /// Returns true if rectPass is overlapping this obj on the _dimOnMap or  _dimOnMapHor
    /// returns true if _dimOnMap is not being setup
    /// </summary>
    /// <param name="rectPass"></param>
    /// <returns></returns>
    public bool IsCollidingWithMe(Rect rectPass)
    {
        //if rect pass overlaps the _dimOnMap or _dimOnMap is not being setup...
        if (rectPass.Overlaps(_dimOnMap) || _dimOnMap == new Rect())
        { return true;}

        if (_dimOnMapHor != new Rect())
        {
            if (rectPass.Overlaps(_dimOnMapHor))
            { return true; } 
        }
        return false;
    }

    public BuildersManager BuildersManager1 { get; set; }

    public ProductInfo CurrentProd { get; set; }

    public Decoration Decoration { get; set; }

    public string Name { get; set; }
}
