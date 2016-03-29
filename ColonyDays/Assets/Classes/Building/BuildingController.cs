using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildingController : BuildingPot
{
    Building _currentSpawnBuild;//current building we are spwaning
    private Registro _registro = new Registro();

    private List<int> _oldIndexesHover = new List<int>();
    private BigBoxPrev _buildWayCursor;

    Production _production = new Production();
    DispatchManager _dispatchManager = new DispatchManager();

    BridgeManager _bridgeManager = new BridgeManager();

    ShipManager _shipManager = new ShipManager();
    DockManager _dockManager = new DockManager();

    public Building CurrentSpawnBuild
    {
        get { return _currentSpawnBuild; }
        set { _currentSpawnBuild = value; }
    }

    public Registro Registro
    {
        get { return _registro; }
        set { _registro = value; }
    }

    public BigBoxPrev BuildWayCursor
    {
        get { return _buildWayCursor; }
        set { _buildWayCursor = value; }
    }

    void Start()
    {
    }
	
	void Update ()
	{
	    CheckHover();
        BridgeManager1.Update();
        ShipManager1.Update();
        DockManager1.Update();
	}

    /// <summary>
    /// Checks if the hovering of current had changed. If does will update on obj Rectangles
    /// and will update _oldIndexesHover
    /// </summary>
    void CheckHover()
    {
        if (_currentSpawnBuild == null) { return;}
        //if they are the same will not go in. So this Method will be use only when a building is
        //being use. Basically while the Building.Place() is being called
        if (_currentSpawnBuild.ClosestSubMeshVert == _currentSpawnBuild.ClosestVertOld) { return;}

        int amoutOfEqItems = 0;
        for (int i = 0; i < m.Vertex.IndexesHover.Count; i++)
        {
            if (i < _oldIndexesHover.Count)
            {
                if (m.Vertex.IndexesHover[i] == _oldIndexesHover[i])
                {
                    amoutOfEqItems++;
                }
            }
        }

        if (amoutOfEqItems != m.Vertex.IndexesHover.Count)
        {
            Registro.UpdateCurrentVertexRect(m.CurrentHoverVertices);
            _oldIndexesHover = m.Vertex.IndexesHover;
        }
    }

    #region Building Administrative. This is the section that will administer the buildings. Ex: a new house was built
    //all the places that have food currently
    private List<string> _foodSources = new List<string>();
    //all buildins that have open Positions of work ... I only hold the keys (MyId s)
    private List<string> _workOpenPos = new List<string>();
    //houses that currently have more space available
    private List<string> _housesWithSpace = new List<string>();
    //houses that currently have more space available
    private List<string> _religiousBuilds = new List<string>();
    //houses that currently have more space available
    private List<string> _chillBuilds = new List<string>();
    //new ways built
    private List<string> _wayBuilds = new List<string>();

    private bool _isfoodSourceChange;//if any one was removed or added from the list will be mark as true
    private bool _isWorkChanged;//new work pos opened
    private bool _isHouseSpaceChanged;//new space available in a house
    private bool _isReligionChanged;
    private bool _isChillChanged;//new tavern etc

    public bool IsfoodSourceChange
    {
        get { return _isfoodSourceChange; }
        set { _isfoodSourceChange = value; }
    }

    public bool AreNewWorkPos
    {
        get { return _isWorkChanged; }
        set { _isWorkChanged = value; }
    }

    public bool IsNewHouseSpace
    {
        get { return _isHouseSpaceChanged; }
        set { _isHouseSpaceChanged = value; }
    }

    public List<string> FoodSources
    {
        get { return _foodSources; }
        set { _foodSources = value; }
    }

    public List<string> WorkOpenPos
    {
        get { return _workOpenPos; }
        set { _workOpenPos = value; }
    }

    public List<string> HousesWithSpace
    {
        get { return _housesWithSpace; }
        set { _housesWithSpace = value; }
    }


    public List<string> ReligiousBuilds
    {
        get { return _religiousBuilds; }
        set { _religiousBuilds = value; }
    }

    public bool IsNewReligion
    {
        get { return _isReligionChanged; }
        set { _isReligionChanged = value; }
    }

    public bool IsNewChill
    {
        get { return _isChillChanged; }
        set { _isChillChanged = value; }
    }

    public List<string> ChillBuilds
    {
        get { return _chillBuilds; }
        set { _chillBuilds = value; }
    }

    public List<string> WayBuilds
    {
        get { return _wayBuilds; }
        set { _wayBuilds = value; }
    }

    public Production ProductionProp
    {
        get { return _production; }
        set { _production = value; }
    }

    public DispatchManager DispatchManager1
    {
        get { return _dispatchManager; }
        set { _dispatchManager = value; }
    }

    public BridgeManager BridgeManager1
    {
        get { return _bridgeManager; }
        set { _bridgeManager = value; }
    }

    public ShipManager ShipManager1
    {
        get { return _shipManager; }
        set { _shipManager = value; }
    }

    public DockManager DockManager1
    {
        get { return _dockManager; }
        set { _dockManager = value; }
    }


    private HPers buildFunc;//building function will determine what set of palces will be used wen editing building
    List<string> current = new List<string>();//current list of buildiings being edited
    /// <summary>
    /// Will start to routine to add or remove an Build from its list
    /// </summary>
    public void EditBuildRoutine(string MyIdP, H action, H hTypeP)
    {
        buildFunc = SelectCurrentList(hTypeP);
        EditBuildAction(MyIdP, action);
    }

    void EditBuildAction(string MyIdP, H action)
    {
        if (action == H.Remove)
        {
            for (int i = 0; i < current.Count; i++)
            {
                if (current[i] == MyIdP)
                {
                    current.RemoveAt(i);
                    break;
                }
            }
            PersonPot.Control.RestartController();
        }
        else
        {
            AddToCurrent(MyIdP);
            //UpdateOnPersonController(MyIdP);

            // So people can notice the new added building
            PersonPot.Control.RestartController();
        }
        SetFlag(buildFunc, true);
        UpdateCurrent(buildFunc);//updates the correspondent list
    }

    /// <summary>
    /// Will return all empty  family found in houses
    /// </summary>
    /// <returns></returns>
    public int HowManyEmptyFamilies()
    {
        int res = 0;
        for (int i = 0; i < _housesWithSpace.Count; i++)
        {
            var house = Brain.GetBuildingFromKey(_housesWithSpace[i]);
            res += house.EmptyFamilies();
        }
        return res;
    }

    /// <summary>
    /// Created so items are not duplicated in the current list 
    /// </summary>
    /// <param name="toAdd"></param>
    void AddToCurrent(string toAdd)
    {
        if (!current.Contains(toAdd))
        {
            current.Add(toAdd);
            //print(toAdd + " added");
        }
    }

    /// <summary>
    /// Updates variables on PersonController
    /// </summary>
    public void AddToQueuesRestartPersonControl(string MyIdP)
    {
        Building b = ReturnLocalCurrent(MyIdP);
        //needs to be restarted to people 
        PersonPot.Control.RestartController();

        if (PersonPot.Control == null || b==null)
        {return;}

        //so people can routes if new build fell in the midle of one
        if (b.HType.ToString().Contains(H.Bridge.ToString()))
        {
            //b.MyId so doesnt add units 
            PersonPot.Control.Queues.AddToNewBuildsQueue(b.Anchors, b.MyId);
        }
        else
        {
            PersonPot.Control.Queues.AddToNewBuildsQueue(b.GetAnchors(), MyIdP);
        }
    }

    /// <summary>
    /// Will return the passed key 'MyIdP' from 'Registro.AllBuilding' if is there
    /// other wise will return 'CurrentSpawnBuild'
    /// 
    /// I used to only use the 'CurrentSpawnBuild' but for brdige need to use the 'Registro.AllBuilding'
    /// since Brdige call this function only when all its parts are built
    /// </summary>
    Building ReturnLocalCurrent(string MyIdP)
    {
        if (Registro.AllBuilding.ContainsKey(MyIdP))
        {
            return Registro.AllBuilding[MyIdP];
        }
        return CurrentSpawnBuild;
    }

    #region This 2 Methods are doing the same juts that the last one doesnt select Anything if one is change the second must be changed too
    HPers SelectCurrentList(H hTypeP)
    {
        if (hTypeP == H.Bohio|| 
            hTypeP == H.HouseA || hTypeP == H.HouseB || hTypeP == H.HouseTwoFloor
            || hTypeP == H.HouseMed// || hTypeP == H.HouseMedB 
            || hTypeP == H.HouseLargeA || hTypeP == H.HouseLargeB || hTypeP == H.HouseLargeC
            )
        {
            current = HousesWithSpace;
            return HPers.Home;
        }
        else if (hTypeP == H.StorageSmall || hTypeP == H.StorageMed || hTypeP == H.StorageBig ||
            hTypeP == H.StorageBigTwoDoors || hTypeP == H.StorageExtraBig)
        {
            current = FoodSources;
            return HPers.FoodSource;
        }
        else if (hTypeP == H.Trail || hTypeP == H.Road || hTypeP == H.BridgeTrail || hTypeP == H.BridgeRoad)
        {
            /////////////////////
            current = WayBuilds;
            return HPers.Way;
        }
        else if (hTypeP == H.Church)
        {
           current = ReligiousBuilds;
           return HPers.Religion;
        }
        else if(hTypeP == H.Tavern)
        {
            current = ChillBuilds;
            return HPers.Chill;
        }
        current = WorkOpenPos;
        return HPers.Work;
    }

    public static HPers ReturnBuildingFunction(H hTypeP)
    {
        if (hTypeP == H.Bohio ||
           hTypeP == H.HouseA || hTypeP == H.HouseB || hTypeP == H.HouseTwoFloor
           || hTypeP == H.HouseMed// || hTypeP == H.HouseMedB 
           || hTypeP == H.HouseLargeA || hTypeP == H.HouseLargeB || hTypeP == H.HouseLargeC
           )
        {
            return HPers.Home;
        }
        else if (hTypeP == H.StorageSmall || hTypeP == H.StorageMed || hTypeP == H.StorageBig ||
            hTypeP == H.StorageBigTwoDoors || hTypeP == H.StorageExtraBig)
        {
            return HPers.FoodSource;
        }
        else if (hTypeP == H.Trail || hTypeP == H.Road || hTypeP == H.BridgeTrail || hTypeP == H.BridgeRoad)
        {
            return HPers.Way;
        }
        else if (hTypeP == H.Church)
        {
            return HPers.Religion;
        }
        else if (hTypeP == H.Tavern)
        {
            return HPers.Chill;
        }
        return HPers.Work;
    }
    #endregion




    void UpdateCurrent(HPers which)
    {
        if (which == HPers.Home)
        {
            HousesWithSpace = current;
            //print("HouseWithSpaceList Count Up:" + HousesWithSpace.Count);
        }
        else if (which == HPers.FoodSource)
        {
            FoodSources = current;
        }
        else if (which == HPers.Work)
        {
            WorkOpenPos = current;
        }
        else if (which == HPers.Religion)
        {
            ReligiousBuilds = current;
        }
        else if (which == HPers.Chill)
        {
            ChillBuilds = current;
        }
    }

    public void SetFlag(HPers which, bool val)
    {
        if (which == HPers.FoodSource)
        {
            _isfoodSourceChange = val;
        }
        else if (which == HPers.Work)
        {
            _isWorkChanged = val;
        }
        else if (which == HPers.Home)
        {
            _isHouseSpaceChanged = val;
        }
        else if (which == HPers.Religion)
        {
            IsNewReligion = val;
        }
        else if (which == HPers.Chill)
        {
            IsNewChill = val;
        }
    }
    #endregion

    internal void AddToHousesWithSpace(string newHomeWithSpace)
    {
        var build = Brain.GetBuildingFromKey(newHomeWithSpace);

        if (!_housesWithSpace.Contains(newHomeWithSpace) && build.Instruction != H.WillBeDestroy)
        {
            _housesWithSpace.Add(newHomeWithSpace);
        }
    }

    public void RemoveFromHousesWithSpace(string houseToRemove)
    {
//      print("Removed " + houseToRemove);
        HousesWithSpace.Remove(houseToRemove);
    }

    /// <summary>
    /// Will tell u if the last building is the one just passed ,
    /// If there is more thn 1 foodSrc then will return false 
    /// </summary>
    public bool IsThisTheLastFoodSrc(Building building)
    {
        if (_foodSources.Count > 1)
        {
            return false;
        }

        //needs to be remove with real game 
        if (_foodSources.Count == 0)
        {
            return false;
        }

        var key = _foodSources[0];
        var lastFood = Brain.GetBuildingFromKey(key);
        if (lastFood.MyId == building.MyId)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Will tell u if the 'hType' pass in all the buildings is the last one in all buildings .
    /// 
    /// If none is found will return false 
    /// </summary>
    /// <param name="hType"></param>
    /// <returns></returns>
    public bool IsThisTheLastOfThisType(H hType, Structure s)
    {
        int count = 0;
        for (int i = 0; i < BuildingPot.Control.Registro.AllBuilding.Count; i++)
        {
            if (BuildingPot.Control.Registro.AllBuilding.ElementAt(i).Value.HType == hType)
            {
                count++;
            }   
        }


        if (count == 1 && s.HType == H.Masonry)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Will tell u how many buildings of that type exist 
    /// </summary>
    /// <param name="hType"></param>
    /// <returns></returns>
    static public int HowManyOfThisTypeAre(H hType)
    {
        int count = 0;
        for (int i = 0; i < BuildingPot.Control.Registro.AllBuilding.Count; i++)
        {
            if (BuildingPot.Control.Registro.AllBuilding.ElementAt(i).Value.HType == hType)
            {
                count++;
            }
        }
        return count;
    }

    static public bool IsAtLeastOneOfThsType(H hType)
    {
        return HowManyOfThisTypeAre(hType) >= 1;
    }

    /// <summary>
    /// Will find the closest of the type form the point 'fromPos'
    /// </summary>
    /// <param name="hType"></param>
    /// <param name="fromPos"></param>
    /// <returns></returns>
    static public Structure FindTheClosestOfThisType(H hType, Vector3 fromPos)
    {
        List<VectorM> distances = new List<VectorM>();

        for (int i = 0; i < BuildingPot.Control.Registro.AllBuilding.Count; i++)
        {
            var build = BuildingPot.Control.Registro.AllBuilding.ElementAt(i).Value;

            if (build.HType == hType)
            {
                distances.Add(new VectorM(build.transform.position, fromPos, build.MyId));
            }
        }

        distances = distances.OrderBy(a => a.Distance).ToList();
        var clostKey = "";

        if (distances.Count > 0)
        {
            clostKey = distances[0].LocMyId;
        }


        return Brain.GetStructureFromKey(clostKey);
    }

    /// <summary>
    /// Will find the closest of the type form the point 'fromPos'
    /// </summary>
    /// <param name="hType"></param>
    /// <param name="fromPos"></param>
    /// <returns></returns>
    static public Structure FindTheClosestOfThisTypeFullyBuilt(H hType, Vector3 fromPos)
    {
        List<VectorM> distances = new List<VectorM>();

        for (int i = 0; i < BuildingPot.Control.Registro.AllBuilding.Count; i++)
        {
            var build = BuildingPot.Control.Registro.AllBuilding.ElementAt(i).Value;

            //bz cant cast a brdige 
            Structure st = null;
            if (!build.MyId.Contains("Bridge") && build.Category != Ca.Way && !build.MyId.Contains("Road"))
            {
                st = (Structure)build;
            }

            if (build.HType == hType && (build.StartingStage==H.Done || st.CurrentStage==4))
            {
                distances.Add(new VectorM(build.transform.position, fromPos, build.MyId));
            }
        }

        distances = distances.OrderBy(a => a.Distance).ToList();
        var clostKey = "";

        if (distances.Count > 0)
        {
            clostKey = distances[0].LocMyId;
        }

        return Brain.GetStructureFromKey(clostKey);
    }

    /// <summary>
    /// Will find the closest of the type form the point 'fromPos'
    /// </summary>
    /// <param name="hType"></param>
    /// <param name="fromPos"></param>
    /// <returns></returns>
    static public List<Structure> FindAllStructOfThisType(H hType)
    {
        List<Structure> distances = new List<Structure>();

        for (int i = 0; i < BuildingPot.Control.Registro.AllBuilding.Count; i++)
        {
            var build = BuildingPot.Control.Registro.AllBuilding.ElementAt(i).Value;

            if (build.HType == hType)
            {
                distances.Add((Structure)build);
            }
        }

        return distances;
    }

    /// <summary>
    /// Will find the closest of the type form the point 'fromPos'
    /// </summary>
    /// <param name="hType"></param>
    /// <param name="fromPos"></param>
    /// <returns></returns>
    static public List<Structure> FindAllStructOfThisTypeContain(H hType)
    {
        List<Structure> distances = new List<Structure>();

        for (int i = 0; i < BuildingPot.Control.Registro.AllBuilding.Count; i++)
        {
            var build = BuildingPot.Control.Registro.AllBuilding.ElementAt(i).Value;

            if (build.HType.ToString().Contains(hType.ToString()))
            {
                distances.Add((Structure)build);
            }
        }

        return distances;
    }

    /// <summary>
    /// Will find the first building of the type asked  
    /// </summary>
    /// <param name="hType"></param>
    /// <returns></returns>
    public Building FindRandomBuildingOfThisType(H hType)
    {
        List<Building> list = new List<Building>();

        for (int i = 0; i < BuildingPot.Control.Registro.AllBuilding.Count; i++)
        {
            if (BuildingPot.Control.Registro.AllBuilding.ElementAt(i).Value.HType == hType)
            {
                list.Add( BuildingPot.Control.Registro.AllBuilding.ElementAt(i).Value);
            }
        }

        if (list.Count == 0)
        {
            return null;    
        }

        var rand = Random.Range(0, list.Count);

        return list[rand];
    }
}
