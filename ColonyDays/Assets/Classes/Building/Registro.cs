﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

//mono only use to print. can be remove
//this class used to be Rectangle.cs
public class Registro : MonoBehaviour
{
    //all bulidings
    private List<RegFile> _allRegFile = new List<RegFile>();
    //hovered buildings
    private List<RegFile> _hover = new List<RegFile>();

    private List<Vector3> _locHoverVert = new List<Vector3>(); //poly of the area hovered 
    private Rect _hoverVertexRect = new Rect(); //poly of the area hovered , the rect
    public static List<Rect> toDraw = new List<Rect>();
    public static Rect curr = new Rect();
    private SMe m = new SMe();

    private Dictionary<string, Structure> _structures = new Dictionary<string, Structure>();
    private Dictionary<string, Way> _ways = new Dictionary<string, Way>();
    private Dictionary<string, DragSquare> _dragSquare = new Dictionary<string, DragSquare>();

    private Dictionary<string, Building> _allBuilding = new Dictionary<string, Building>();
    private Building _selectBuilding = new Building();


    private List<Building> _toDestroyBuilding = new List<Building>();

    /// <summary>
    /// All Buildings are here collected 
    /// </summary>
    public Dictionary<string, Building> AllBuilding
    {
        get { return _allBuilding; }
        set { _allBuilding = value; }
    }

    public List<RegFile> AllRegFile
    {
        get { return _allRegFile; }
        set { _allRegFile = value; }
    }

    public Dictionary<string, Structure> Structures
    {
        get { return _structures; }
        set { _structures = value; }
    }

    public Dictionary<string, Way> Ways
    {
        get { return _ways; }
        set { _ways = value; }
    }

    public Dictionary<string, DragSquare> DragSquares
    {
        get { return _dragSquare; }
        set { _dragSquare = value; }
    }

    public Building SelectBuilding
    {
        get { return _selectBuilding; }
        set { _selectBuilding = value; }
    }

    private bool _isFullyLoaded;

    public bool IsFullyLoaded
    {
        get { return _isFullyLoaded; }
        set { _isFullyLoaded = value; }
    }


    public Registro() { }

    /// <summary>
    /// Will return the first Structure that cointains param
    /// 
    /// So if u pass as param 'dock' will try to find the first dock
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public Structure ReturnFirstThatContains(string param)
    {
        var list = AllBuilding.ToList();
        return (Structure)list.Find(a => a.Value.MyId.Contains(param)).Value;
    }

    public float ReturnYearSalary()
    {
        var works = AllBuilding.Where(
            a => a.Value.MyId.Contains("House") == false &&
                a.Value.MyId.Contains("Storage") == false &&
                a.Value.MyId.Contains("Church") == false &&
                a.Value.MyId.Contains("Tavern") == false).ToList();

        if (works.Count == 0)
        {
            return 0;
        }

        var avg = works.Average(a => a.Value.DollarsPay);
        var workers = works.Sum(a => a.Value.PeopleDict.Count);

        return (float)avg * workers;
    }


    #region ToDestroyBuilding
    public void AddToDestroyBuilding(Building build)
    {
        //was added already
        if (FindFromToDestroyBuildings(build.MyId) != null)
        {
            return;
        }

        _toDestroyBuilding.Add(build);
    }

    public void RemoveFromDestroyBuildings(Building build)
    {
        _toDestroyBuilding.Remove(build);
    }

    public Building FindFromToDestroyBuildings(string myIdP)
    {
        return _toDestroyBuilding.Find(a => a.MyId == myIdP);
    }

    internal bool AreUDemolishingOneAlready()
    {
        return _toDestroyBuilding.Count > 0;
    }
    #endregion





    /// <summary>
    /// Remove item from All, and its spefic list
    /// </summary>
    /// <param name="cat">item category</param>
    /// <param name="myId">item myId</param>
    public void RemoveItem(Ca cat, string myId)
    {
        var build = Brain.GetBuildingFromKey(myId);
        AddToDestroyBuilding(Brain.GetBuildingFromKey(myId));

        //Debug.Log("Registro RemoveItem");
        //PersonPot.Control.BuildersManager1.RemoveConstruction(myId);//so its removed from the BuilderManager

        BuildingPot.Control.DockManager1.RemoveFromDockStructure(myId, build.HType);


        ////so its save to AllRegFiles
        AllBuilding[myId].Instruction = H.WillBeDestroy;
        ResaveOnRegistro(myId);

        AllBuilding[myId].UpdateOnBuildControl(H.Remove);//so its remove from build control.
        RemoveRegularOrders(myId);
        AddEvacutationOrder(myId);

        UpdateCurrentVertexRect(m.CurrentHoverVertices);
        if (cat == Ca.Way)
        {
            _ways.Remove(myId);
        }
        else if (cat == Ca.Structure || cat == Ca.Shore)
        {
            _structures.Remove(myId);
        }
        else if (cat == Ca.DraggableSquare)
        {
            _dragSquare.Remove(myId);
        }
    }

    /// <summary>
    /// Onces is set to be destroy all the Regular orders in Dispatch need to be removed 
    /// </summary>
    /// <param name="myId"></param>
    private void RemoveRegularOrders(string myId)
    {
        BuildingPot.Control.DispatchManager1.RemoveRegularOrders(myId);
    }

    /// <summary>
    /// If building has any remaining inventory will add the evacuation order to Dispatch
    /// So the inventory is not lost 
    /// </summary>
    /// <param name="myIdP"></param>
    private void AddEvacutationOrder(string myIdP)
    {
        var build = Brain.GetBuildingFromKey(myIdP);

        //the building was destroy before was fully built 
        if (build == null || build.Category == Ca.Way)
        {
            return;
        }

        build.AddToClosestWheelBarrowAsOrderEvacuateAllInv();
    }

    /// <summary>
    /// So its removed at the end so no new buildings are created on top of building that will be removed soon
    /// </summary>
    /// <param name="myId"></param>
    public void RemoveFromAllRegFile(string myId)
    {
        int index = AllRegFile.FindIndex(a => a.MyId == myId);

        //bz when destroying Way this method is called at least two times. 
        //the 2nd time doesnt find the index bz was removed already
        if (index == -1)
        {
            return;
        }

        AllRegFile.RemoveAt(index);
    }

    /// <summary>
    /// This is intended to save the new material assigned to the building on disk 
    /// </summary>
    public void UpdateItemMaterial(Ca cat, string myId, string newMatKey)
    {
        int index = AllRegFile.FindIndex(a => a.MyId == myId);
        AllRegFile[index].MaterialKey = newMatKey;

        if (cat == Ca.Way)
        {
            Ways[myId].MaterialKey = newMatKey;
        }
        else if (cat == Ca.Structure || cat == Ca.Shore)
        {
            Structures[myId].MaterialKey = newMatKey;
        }
        else if (cat == Ca.DraggableSquare)
        {
            DragSquares[myId].MaterialKey = newMatKey;
        }
    }

    public static List<Vector3> FromALotOfVertexToPoly(List<Vector3> list)
    {
        List<float> xS = UList.ReturnAxisList(list, H.X);
        List<float> zS = UList.ReturnAxisList(list, H.Z);

        float minX = UMath.ReturnMinimum(xS);
        float maxX = UMath.ReturnMax(xS);

        float minZ = UMath.ReturnMinimum(zS);
        float maxZ = UMath.ReturnMax(zS);

        //Poly List that only need a valid NW, NE, and SW
        SMe m = new SMe();
        //throwing rays so we keep the exact Y values 
        Vector3 NW = m.Vertex.BuildVertexWithXandZ(minX, maxZ);
        Vector3 NE = m.Vertex.BuildVertexWithXandZ(maxX, maxZ);
        Vector3 SE = m.Vertex.BuildVertexWithXandZ(maxX, minZ);
        Vector3 SW = m.Vertex.BuildVertexWithXandZ(minX, minZ);

        return new List<Vector3>() { NW, NE, SE, SW };
    }

    /// <summary>
    /// This one use Y as MathCenter
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public static List<Vector3> FromALotOfVertexToPolyMathCenterY(List<Vector3> list)
    {
        List<float> xS = UList.ReturnAxisList(list, H.X);
        List<float> zS = UList.ReturnAxisList(list, H.Z);

        float minX = UMath.ReturnMinimum(xS);
        float maxX = UMath.ReturnMax(xS);

        float minZ = UMath.ReturnMinimum(zS);
        float maxZ = UMath.ReturnMax(zS);

        //Poly List that only need a valid NW, NE, and SW
        SMe m = new SMe();
        //throwing rays so we keep the exact Y values 
        Vector3 NW = new Vector3(minX, m.IniTerr.MathCenter.y, maxZ);
        Vector3 NE = new Vector3(maxX, m.IniTerr.MathCenter.y, maxZ);
        Vector3 SE = new Vector3(maxX, m.IniTerr.MathCenter.y, minZ);
        Vector3 SW = new Vector3(minX, m.IniTerr.MathCenter.y, minZ);

        return new List<Vector3>() { NW, NE, SE, SW };
    }

    /// <summary>
    /// Taken a list of vectors 3 will find NW, NE, and SW and from there will create a new rectangle
    /// Returns a rectangle in our system where North is on the higher Y value always
    /// 
    /// Y val flipped at the end 
    /// </summary>
    public static Rect FromALotOfVertexToRect(List<Vector3> list)
    {
        List<float> xS = UList.ReturnAxisList(list, H.X);
        List<float> zS = UList.ReturnAxisList(list, H.Z);

        float minX = UMath.ReturnMinimum(xS);
        float maxX = UMath.ReturnMax(xS);

        float minZ = UMath.ReturnMinimum(zS);
        float maxZ = UMath.ReturnMax(zS);

        //Poly List that only need a valid NW, NE, and SW
        Vector3 NW = new Vector3(minX, 0, maxZ);
        Vector3 NE = new Vector3(maxX, 0, maxZ);
        Vector3 SE = new Vector3(maxX, 0, minZ);
        Vector3 SW = new Vector3(minX, 0, minZ);

        List<Vector3> poly = new List<Vector3>() { NW, NE, SE, SW };

        //here i find the Rect from this poly and then 
        // I invert the Y of the recatangle... other wise this big rectangle
        //is not overlapping anything will be far off in the Cordinates...
        //Due to North(up) is bigger here say 100,, and South(down) less say 0 all this on World Z axis
        //As long as MeshManager Hover Current Vertices is big as is its now 9 Lots (each lot 5x5 real polys)
        //the Rect of the buildings will work flawlessly
        return U2D.ReturnRectYInverted(U2D.FromPolyToRect(poly));
    }

    /// <summary>
    /// Will add a poly with the seq NW, NE, SE, SW to the _allBuilding List. then will call UpdateCurrentVertexRect()
    /// Adds the file to Registro All that is the save list to file of buildings
    /// </summary>
    public void AddBuildToAll(Building build, List<Vector3> poly, Ca categ, Vector3 iniPosition,
        Inventory inventory,
        List<string> PeopleDict,
        List<VectorLand> LandZone1,
        List<Vector3> polyHoriz = null,
        List<Vector3> tilePosVert = null, List<Vector3> tilePosHor = null, List<Vector3> planesOnAirPos = null,
        Vector3 tileScale = new Vector3(), List<int> parts = null,
        H dominantSide = H.None, H startingStage = H.None, int rotationFacerIndex = -1, string materialKey = "",
        List<Vector3> planesOnSoilPos = null, List<int> partsOnSoil = null, Vector3 min = new Vector3(),
        Vector3 max = new Vector3(), H instructionP = H.None, BookedHome BookedHome1 = null,
        Dispatch dispatch = null, Family[] Families = null,
        int dollarsPay = 0,
        List<Vector3> anchors = null, Dock dock = null, string root = ""
        )
    {
        // 12 hours to find this OMG!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        // I was creating the recatblgele and not inverting Y.. then I invert Y but didint inverted in 
        //IsColliding() chet !!!! And i knew it bz i inverted the big rectangle...
        //with the rectangles inverted works like a charm... we have to do it bz im using North as bigger
        //and south as less... in the Rect cordinates is the other way around

        //this is the usual poly will be filled for eg regular structures only use this one.
        //For ways is the vertic bound
        var to = ReturnDimOnMap(poly);

        Rect toHoriz = new Rect();
        if (polyHoriz != null)
        {
            toHoriz = U2D.FromPolyToRect(polyHoriz);
            toHoriz = U2D.ReturnRectYInverted(toHoriz);
        }

        //ading to All
        RegFile regFile = new RegFile(build, to, categ, iniPosition,
            inventory,
            PeopleDict, LandZone1,
            toHoriz, tilePosVert: tilePosVert, tilePosHor: tilePosHor,
            planesOnAirPos: planesOnAirPos, tileScale: tileScale, partsOnAir: parts, dominantSide: dominantSide, startingStage: startingStage, rotationFacerIndex: rotationFacerIndex,
            materialKey: materialKey, planesOnSoilPos: planesOnSoilPos, partsOnSoil: partsOnSoil, min: min, max: max,
            instructionP: instructionP, bookedHome: BookedHome1, dispatch: dispatch, familes: Families,
            dollarsPay: dollarsPay,
            anchors: anchors, dock: dock, root: root);

        //UVisHelp.CreateHelpers(anchors, Root.blueCube);
        AddToAll(regFile);
        AddToBuilderManager(build.MyId);

        AddSpecToList(categ);
        if (_locHoverVert.Count > 0) { UpdateCurrentVertexRect(_locHoverVert); }
        //use on the drawing debug functionalitie only:
        //toDraw.Add(to);
        //toDraw.Add(toHoriz);
    }

    public static Rect ReturnDimOnMap(List<Vector3> poly)
    {
        Rect to = U2D.FromPolyToRect(poly);
        to = U2D.ReturnRectYInverted(to);
        return to;
    }

    /// <summary>
    /// Will add to the list of buildings needed to be built 
    /// </summary>
    /// <param name="b"></param>
    void AddToBuilderManager(string myId)
    {
        Building st = Brain.GetBuildingFromKey(myId);
        if (st.IsLoadingFromFile)
        {
            return;
        }

        PersonPot.Control.BuildersManager1.AddNewConstruction(st.MyId, st.HType, 2, st.transform.position);
    }

    //used to hold the current birdge until all Pieces are spawned 
    //needed bz we clear BuilderPot.Control.CurrentSpawnBuild
    public static Building oldBridge;
    /// <summary>
    /// Created to avoid exception that key exist already
    /// </summary>
    void AddToAll(RegFile regFile)
    {
        var key = regFile.MyId;

        //if key exsit will add this number at the end .
        //This can happen when spawing a building and an old building has the same name and ID #
        if (_allBuilding.ContainsKey(key))
        {
            key = key + "1983";
        }

        regFile.MyId = key;

        var build = BuildingPot.Control.CurrentSpawnBuild;
        //means is a CancelDemolish
        if (build == null)
        {
            build = SelectBuilding;
            BuildingPot.Control.CurrentSpawnBuild = SelectBuilding;
        }
        build.MyId = key;
        build.transform.name = key;

        AllRegFile.Add(regFile);
        _allBuilding.Add(key, build);
    }

    //will add an new building to it list dependeing on catefory
    void AddSpecToList(Ca cat)
    {
        if (cat == Ca.Way)
        {
            Way f = BuildingPot.Control.CurrentSpawnBuild as Way;
            f = (Way)CheckIfOnDict(_ways, f);
            Ways.Add(f.MyId, f);
        }
        else if (cat == Ca.Structure || cat == Ca.Shore)
        {
            Structure f = BuildingPot.Control.CurrentSpawnBuild as Structure;
            f = (Structure)CheckIfOnDict(Structures, f);
            Structures.Add(f.MyId, f);
        }
        else if (cat == Ca.DraggableSquare)
        {
            DragSquare f = BuildingPot.Control.CurrentSpawnBuild as DragSquare;
            f = (DragSquare)CheckIfOnDict(DragSquares, f);
            DragSquares.Add(f.MyId, f);
        }
    }

    /// <summary>
    /// Will update Propersties on AllRegFile so when is saved is there to be loaded 
    /// 
    /// Prop that Update so far:
    /// BookedHome1
    /// Instruction
    /// Families
    /// Invetory
    /// PeopleDic
    /// PositionFilled
    /// Anchors
    /// DollarsPay
    /// Dock1
    /// PlantSave1
    /// Dispatch
    /// BuildersManager1
    /// CurrentProd
    /// </summary>
    public void ResaveOnRegistro(string myIdP)
    {
        //for when Building is loading and writing PeopleDict 
        if (!AllBuilding.ContainsKey(myIdP))
        {
            return;
        }

        var build = AllBuilding[myIdP];
        int index = AllRegFile.FindIndex(a => a.MyId == myIdP);

        //bz when destroying Way this method is called
        if (index == -1)
        {
            return;
        }

        AllRegFile[index].BookedHome1 = build.BookedHome1;
        AllRegFile[index].Instruction = build.Instruction;
        AllRegFile[index].Familes = build.Families;
        AllRegFile[index].Inventory = build.Inventory;
        AllRegFile[index].PeopleDict = build.PeopleDict;
        AllRegFile[index].Anchors = build.Anchors.ToArray();

        //UVisHelp.CreateHelpers(build.Anchors, Root.yellowCube);



        AllRegFile[index].DollarsPay = build.DollarsPay;
        AllRegFile[index].Dock1 = build.Dock1;
        AllRegFile[index].Dispatch1 = build.Dispatch1;
        AllRegFile[index].BuildersManager1 = build.BuildersManager1;
        AllRegFile[index].PlantSave1 = build.PlantSave1;
        AllRegFile[index].CurrentProd = build.CurrentProd;
        AllRegFile[index].Name = build.NameBuilding();
    }

    /// <summary>
    /// Will update Propersties on AllRegFile so when is saved is there to be loaded 
    /// 
    /// Prop that Update so far:
    /// BookedHome1
    /// Instruction
    /// Families
    /// Invetory
    /// PeopleDic
    /// PositionFilled
    /// Anchors
    /// DollarsPay
    /// Dock1
    /// PlantSave1
    ///     /// Dispatch
    /// BuildersManager1
    /// CurrentProd
    /// </summary>
    public void ResaveOnRegistro(RegFile regFile, Building build, bool anchorsIsOn)
    {
        regFile.BookedHome1 = build.BookedHome1;
        regFile.Instruction = build.Instruction;
        regFile.Familes = build.Families;
        regFile.Inventory = build.Inventory;
        regFile.PeopleDict = build.PeopleDict;

        //only need to be resave when Loaded Town spanws 
        if (anchorsIsOn && !build.MyId.Contains("Bridge"))
        {
            regFile.Anchors = build.Anchors.ToArray();
        }



        regFile.DollarsPay = build.DollarsPay;
        regFile.Dock1 = build.Dock1;
        regFile.Dispatch1 = build.Dispatch1;
        regFile.BuildersManager1 = build.BuildersManager1;

        regFile.PlantSave1 = build.PlantSave1;
        regFile.CurrentProd = build.CurrentProd;

        regFile.ProductionReport = build.ProductionReport;
        regFile.MaxPeople = build.MaxPeople;
        regFile.Name = build.NameBuilding();

    }

    /// <summary>
    /// If the key exist on Dict will add a Zero to the end of the MyId, will check again until it doesnt exist 
    /// </summary>
    General CheckIfOnDict<T>(Dictionary<string, T> onDictionary, General checkP)
    {
        //is a CancelDemolish
        //if (checkP==null)
        //{
        //    checkP = SelectBuilding;
        //}

        if (onDictionary.ContainsKey(checkP.MyId))
        {
            checkP.AddZeroToMyID();
            //Recursive call so if still is contained will add a new zero until is not contained on the Dictiornay
            CheckIfOnDict(onDictionary, checkP);
        }
        return checkP;
    }

    /// <summary>
    /// Will update the rect that covers the allHoverVertex and the currentBuilding List Rect too
    /// </summary>
    /// <param name="allHoverVertex">All Current Vertex from MeshManager</param>
    public void UpdateCurrentVertexRect(List<Vector3> allHoverVertex)
    {
        _locHoverVert = allHoverVertex;
        _hoverVertexRect = FromALotOfVertexToRect(allHoverVertex);
        _hover.Clear();

        for (int i = 0; i < AllRegFile.Count; i++)
        {
            if (_hoverVertexRect.Overlaps(AllRegFile[i].DimOnMap))
            {
                _hover.Add(AllRegFile[i]);
            }
        }
    }

    /// <summary>
    /// If the object pass is colliding with any rectangle of the currentBuilding List returns true
    /// </summary>
    /// <param name="poly">Object being place on terrain</param>
    /// <returns></returns>
    public bool IsCollidingWithExisting(List<Vector3> poly)
    {
        ///////////////////////////////////////////////////////////**********************************************
        // 12 hours to find this OMG!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        // I was creating the recatblgele and not inverting Y.. then I invert Y but didint inverted in 
        //IsColliding() cheat !!!! And i knew it bz i inverted the big rectangle...
        //with the rectangles inverted works like a charm... we have to do it bz im using North as bigger
        //and south as leess... in the Rect cordinates is the other way arounds
        Rect polyPass = U2D.FromPolyToRect(poly);
        polyPass = U2D.ReturnRectYInverted(polyPass);
        curr = polyPass;

        //make hover bigger  4 hours  BUGGGGGGGGGGGGGGGGGGGGGGGGGG
        //for (int i = 0; i < _hover.Count; i++)
        //{
        //    if (_hover[i].IsCollidingWithMe(polyPass))
        //    {
        //        return true;
        //    }
        //}

        //have to changed to be always colliding instead we can check that all are not colliding
        //this is to correct a bugg where u can click really fast and structures will build on top
        //of wach others

        bool res = true;
        int count = 0;

        for (int i = 0; i < _allRegFile.Count; i++)
        {
            if (!_allRegFile[i].IsCollidingWithMe(polyPass))
            {
                count++;
            }
            else
            {
                var a = 1;
            }
        }

        //if all the obj are not colliding then...
        if (count == _allRegFile.Count)
        {
            res = false;
        }
        return res;
    }

    /// <summary>
    /// The poly pass shoud be really really small like a point. to this be effcective
    /// Created to find what is coliiding with a Tile 
    /// </summary>
    /// <param name="poly"></param>
    /// <returns></returns>
    public H IsCollidingWithWhat(List<Vector3> poly)
    {
        ///////////////////////////////////////////////////////////**********************************************
        // 12 hours to find this OMG!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        // I was creating the recatblgele and not inverting Y.. then I invert Y but didint inverted in 
        //IsColliding() cheat !!!! And i knew it bz i inverted the big rectangle...
        //with the rectangles inverted works like a charm... we have to do it bz im using North as bigger
        //and south as leess... in the Rect cordinates is the other way arounds
        Rect polyPass = U2D.FromPolyToRect(poly);
        polyPass = U2D.ReturnRectYInverted(polyPass);
        curr = polyPass;

        for (int i = 0; i < _allRegFile.Count; i++)
        {
            if (_allRegFile[i].IsCollidingWithMe(polyPass))
            {
                return _allRegFile[i].HType;
            }
        }

        return H.None;
    }

    internal List<Family> AllFamilies()
    {
        List<Family> res = new List<Family>();

        for (int i = 0; i < AllRegFile.Count; i++)
        {
            res.AddRange(AllRegFile[i].Familes);
        }
        return res;
    }

    /// <summary>
    /// Created for GC reasons Im not updating the invetory in buildings as changes anymore
    /// Call when saving a game 
    /// 
    /// Will get the info from all buildings and will update it into AllRegFiles only Prop specified in ResaveOnRegistro()
    /// are being resaved 
    /// </summary>
    internal void ResaveAllBuildings()
    {
        var error = 0;
        for (int i = 0; i < AllBuilding.Count; i++)
        {
            if (AllBuilding.ElementAt(i).Value.MyId == AllRegFile[i].MyId)
            {
                ResaveOnRegistro(AllRegFile[i], AllBuilding.ElementAt(i).Value, false);
            }
            //means that somehow are not in sync
            //this happens only when destroyed a bulding befofre finished built
            //still on game and save
            else
            {
                error++;
                var build = AllBuilding.First(a => a.Value.MyId == AllRegFile[i].MyId);
                ResaveOnRegistro(AllRegFile[i], build.Value, false);
            }
        }
        Debug.Log("Margin error ReSaving: " + error);
    }



    /// <summary>
    /// This is used only when loading a town and needs to redo DimOnMap
    /// </summary>
    internal void RedoDimAndResaveAllBuildings()
    {
        for (int i = 0; i < AllBuilding.Count; i++)
        {
            AllRegFile[i].DimOnMap = ReturnDimOnMap(AllBuilding.ElementAt(i).Value.Anchors);
            ResaveOnRegistro(AllRegFile[i], AllBuilding.ElementAt(i).Value, true);
        }
    }

    public void DoLastStepOfTownLoaded()
    {
        for (int i = 0; i < AllBuilding.Count; i++)
        {
            AllBuilding.ElementAt(i).Value.TownBuildingLastStep();
        }
    }

    public List<Vector3> ReturnMySavedAnchors(string MyIdP)
    {
        var miSave = AllRegFile.Find(a => a.MyId == MyIdP);

        if (miSave != null)
        {
            return miSave.Anchors.ToList();
        }
        return new List<Vector3>();
    }

    internal Vector3 AverageOfAllBuildingsNow()
    {
        Vector3 sum = new Vector3();
        for (int i = 0; i < AllBuilding.Count; i++)
        {
            sum += AllBuilding.ElementAt(i).Value.transform.position;
        }
        return sum / AllBuilding.Count;
    }

    internal List<string> StringOfAllBuildings()
    {
        List<string> res = new List<string>();
        for (int i = 0; i < AllBuilding.Count; i++)
        {
            res.Add(AllBuilding.ElementAt(i).Value.MyId);
        }
        return res;
    }   
    
    internal List<string> StringOfAllBuildingsHType()
    {
        List<string> res = new List<string>();
        for (int i = 0; i < AllBuilding.Count; i++)
        {
            res.Add(AllBuilding.ElementAt(i).Value.HType+"");
        }
        return res;
    }

    /// <summary>
    /// All the positions in all work buildings 
    /// </summary>
    /// <returns></returns>
    internal int MaxPositions()
    {
        int res = 0;
        for (int i = 0; i < AllBuilding.Count; i++)
        {
            if (BuildingWindow.isAWorkBuild(AllBuilding.ElementAt(i).Value))
            {
                res += AllBuilding.ElementAt(i).Value.MaxPeople;
            }
        }
        return res;
    }

    /// <summary>
    /// a list string of all the building types that are a work 
    /// </summary>
    /// <returns></returns>
    internal List<string> StringOfAllBuildingsThatAreAWork()
    {
        List<string> res = new List<string>();
        for (int i = 0; i < AllBuilding.Count; i++)
        {
            if (BuildingWindow.isAWorkBuild(AllBuilding.ElementAt(i).Value))
            {
                res.Add(AllBuilding.ElementAt(i).Value.HType + "");
            }
        }
        return res;
    }

    internal void DoEquealPaymentForAllWorks()
    {
        List<string> res = new List<string>();
        for (int i = 0; i < AllBuilding.Count; i++)
        {
            if (BuildingWindow.isAWorkBuild(AllBuilding.ElementAt(i).Value))
            {
               AllBuilding.ElementAt(i).Value.DollarsPay = 5;
            }
        }
    }


    /// <summary>
    /// todo... 
    /// to be used only when loading a brand new Terra Spawner file 
    /// 
    /// if spawnedData is in the closest9 regions will check if needs to remove itself bz
    /// could fall into a building 
    /// </summary>
    /// <param name="spawnedData"></param>
    /// <param name="closest9"></param>
    internal void MarkTerraIfNeeded(SpawnedData spawnedData, List<RegionD> closest9)
    {
        var index = closest9.FindIndex(a => a.Region == spawnedData.Region);

        if (index == -1)
        {
            return;
        }

        for (int i = 0; i < AllBuilding.Count; i++)
        {
            AllBuilding.ElementAt(i).Value.CheckOnMarkTerra();
        }

    }
}
