using UnityEngine;
using System.Collections.Generic;
using System.Linq;

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
    private Dictionary<string, DragSquare> _farms = new Dictionary<string, DragSquare>();

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

    public Dictionary<string, DragSquare> Farms
    {
        get { return _farms; }
        set { _farms = value; }
    }

    public Building SelectBuilding
    {
        get { return _selectBuilding; }
        set { _selectBuilding = value; }
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
                a.Value.MyId.Contains("Church") == false).ToList();

        if (works.Count==0)
        {
            return 0;
        }

        var avg = works.Average(a => a.Value.DollarsPay);
        var workers = works.Sum(a => a.Value.PeopleDict.Count);

        return (float)avg*workers;
    }


#region ToDestroyBuilding
    public void AddToDestroyBuilding(Building build)
    {
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
        
        Debug.Log("Registro RemoveItem");
        PersonPot.Control.BuildersManager1.RemoveConstruction(myId);//so its removed from the BuilderManager

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
            _farms.Remove(myId);
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
            Farms[myId].MaterialKey = newMatKey;
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
        Vector3 NW = m.Vertex.BuildVertexWithXandZ( minX, maxZ);
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
    public void AddBuildToAll(string myId, H type, List<Vector3> poly, Ca categ, Vector3 iniPosition,
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
        List<Vector3> anchors = null 
        )
    {
        // 12 hours to find this OMG!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        // I was creating the recatblgele and not inverting Y.. then I invert Y but didint inverted in 
        //IsColliding() chet !!!! And i knew it bz i inverted the big rectangle...
        //with the rectangles inverted works like a charm... we have to do it bz im using North as bigger
        //and south as less... in the Rect cordinates is the other way around

        //this is the usual poly will be filled for eg regular structures only use this one.
        //For ways is the vertic bound
        Rect to  = U2D.FromPolyToRect(poly);
        to = U2D.ReturnRectYInverted(to);

        Rect toHoriz = new Rect();
        if (polyHoriz != null)
        {
            toHoriz = U2D.FromPolyToRect(polyHoriz);
            toHoriz = U2D.ReturnRectYInverted(toHoriz);
        }

        //ading to All
        RegFile regFile = new RegFile(myId, type, to, categ,iniPosition,
            inventory, 
            PeopleDict, LandZone1,
            toHoriz, tilePosVert: tilePosVert, tilePosHor: tilePosHor,
            planesOnAirPos: planesOnAirPos, tileScale: tileScale, partsOnAir: parts, dominantSide: dominantSide, startingStage: startingStage, rotationFacerIndex: rotationFacerIndex, 
            materialKey: materialKey, planesOnSoilPos: planesOnSoilPos, partsOnSoil: partsOnSoil, min: min, max: max,
            instructionP: instructionP,  bookedHome: BookedHome1, dispatch: dispatch, familes: Families,
            dollarsPay: dollarsPay, 
            anchors: anchors);

        AddToAll(regFile);
        BuildingPot.Control.DockManager1.AddToDockStructure(myId, type);


        AddToBuilderManager(myId);




        AddSpecToList(categ);
        if (_locHoverVert.Count > 0){UpdateCurrentVertexRect(_locHoverVert);}
        //use on the drawing debug functionalitie only:
        //toDraw.Add(to);
        //toDraw.Add(toHoriz);
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
        if (build==null)
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
            f = (DragSquare)CheckIfOnDict(Farms, f);
            Farms.Add(f.MyId, f);
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
        AllRegFile[index].Anchors = build.Anchors;
        AllRegFile[index].DollarsPay = build.DollarsPay;
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
        }

        //if all the obj are not colliding then...
        if (count == _allRegFile.Count)     
        {       
            res = false;   
        }
        return res;
    }

    internal List<Family> AllFamilies()
    {
        List<Family>  res = new List<Family>();

        for (int i = 0; i < AllRegFile.Count; i++)
        {
            res.AddRange(AllRegFile[i].Familes);
        }
        return res;
    }
}
