﻿using System.Collections.Generic;

/*This calss contains how much the structures cost */

public class Book : General
{
    private static List<BuildStat> _build = new List<BuildStat>();

    //structures categories with the set of its structures
    private Dictionary<H, List<H>> _structuresDict = new Dictionary<H, List<H>>();

    //the list of all menuGroups of buildings 
    private List<H> _menuGroupsList = new List<H>();

    //the list of all structures
    private List<H> _allStructures = new List<H>();

    public static List<BuildStat> Build
    {
        get { return _build; }
    }

    public Dictionary<H, List<H>> StructuresDict
    {
        get { return _structuresDict; }
        set { _structuresDict = value; }
    }

    public List<H> AllStructures
    {
        get { return _allStructures; }
        set { _allStructures = value; }
    }

    public List<H> MenuGroupsList
    {
        get { return _menuGroupsList; }
        set { _menuGroupsList = value; }
    }

    /// <summary>
    /// Will give u the stats of a Specific type of building 
    /// </summary>
    /// <param name="hTypeP"></param>
    /// <returns></returns>
    static public BuildStat GiveMeStat(H hTypeP)
    {
        BuildStat res = new BuildStat();
        for (int i = 0; i < _build.Count; i++)
        {
            if (_build[i].HType == hTypeP)
            {
                res = _build[i];
            }
        }
        return res;
    }


    void LoadBuildDict()
    {
        //infr
        Build.Add(new BuildStat(H.BridgeTrail, 400, 80, 20, 0, 5, maxPeople: 0));
        Build.Add(new BuildStat(H.BridgeRoad, 1000, 80, 20, 0, 5, maxPeople: 0));

        Build.Add(new BuildStat(H.CoachMan, 800, 80, 20, 0, 5, maxPeople: 8));
        Build.Add(new BuildStat(H.BuildersOffice, 800, 80, 20, 0, 5, maxPeople: 8));
        Build.Add(new BuildStat(H.LightHouse, 800, 80, 20, 0, 5, maxPeople: 3));


        //houses 
        Build.Add(new BuildStat(H.HouseA, 400, 15, 5, 25, 5, maxPeople: 5, capacity: 5));
        Build.Add(new BuildStat(H.HouseB, 400, 15, 5, 25, 5, maxPeople: 5, capacity: 5));
        Build.Add(new BuildStat(H.HouseAWithTwoFloor, 800, 30, 5, 50, 5, maxPeople: 10, capacity: 10));
        Build.Add(new BuildStat(H.HouseMedA, 800, 30, 5, 50, 5, maxPeople: 7, capacity: 10));
        Build.Add(new BuildStat(H.HouseMedB, 800, 30, 5, 50, 5, maxPeople: 7, capacity: 10));
        Build.Add(new BuildStat(H.HouseC, 800, 30, 5, 50, 5, maxPeople: 7, capacity: 15));
        Build.Add(new BuildStat(H.HouseD, 800, 30, 5, 50, 5, maxPeople: 7, capacity: 20));

        Build.Add(new BuildStat(H.Shack, 50, maxPeople: 5, capacity: 500));

        //farming
        Build.Add(new BuildStat(H.AnimalFarmSmall, 400, 15, 5, 25, 5, maxPeople: 5));
        Build.Add(new BuildStat(H.AnimalFarmMed, 500, 15, 5, 25, 5, maxPeople: 7));
        Build.Add(new BuildStat(H.AnimalFarmLarge, 600, 15, 5, 25, 5, maxPeople: 9));
        Build.Add(new BuildStat(H.AnimalFarmXLarge, 800, 15, 5, 25, 5, maxPeople: 12));

        Build.Add(new BuildStat(H.FieldFarmSmall, 400, 15, 5, 25, 5, maxPeople: 2));
        Build.Add(new BuildStat(H.FieldFarmMed, 500, 15, 5, 25, 5, maxPeople: 4));
        Build.Add(new BuildStat(H.FieldFarmLarge, 600, 15, 5, 25, 5, maxPeople: 6));
        Build.Add(new BuildStat(H.FieldFarmXLarge, 800, 15, 5, 25, 5, maxPeople: 9));

        //Raw
        Build.Add(new BuildStat(H.Ceramic, 400, 15, 5, 25, 5, maxPeople: 5));
        Build.Add(new BuildStat(H.FishSmall, 400, 15, 5, 25, 5, maxPeople: 5));
        Build.Add(new BuildStat(H.FishRegular, 400, 15, 5, 25, 5, maxPeople: 5));
        Build.Add(new BuildStat(H.Mine, 400, 15, 5, 25, 5, maxPeople: 5));

        Build.Add(new BuildStat(H.MountainMine, 400, 15, 5, 25, 5, maxPeople: 5));
        Build.Add(new BuildStat(H.Resin, 400, 15, 5, 25, 5, maxPeople: 5));
        Build.Add(new BuildStat(H.Wood, 400, 15, 5, 25, 5, maxPeople: 5));
        Build.Add(new BuildStat(H.BlackSmith, 400, 15, 5, 25, 5, maxPeople: 5));

        Build.Add(new BuildStat(H.SaltMine, 400, 15, 5, 25, 5, maxPeople: 5));


        //Prod
        Build.Add(new BuildStat(H.Brick, 400, 15, 5, 25, 5, maxPeople: 5));
        Build.Add(new BuildStat(H.Carpintery, 400, 15, 5, 25, 5, maxPeople: 5));
        Build.Add(new BuildStat(H.Cigars, 400, 15, 5, 25, 5, maxPeople: 5));

        Build.Add(new BuildStat(H.Mill, 400, 15, 5, 25, 5, maxPeople: 5));
        Build.Add(new BuildStat(H.Slat, 400, 15, 5, 25, 5, maxPeople: 5));
        Build.Add(new BuildStat(H.Tilery, 400, 15, 5, 25, 5, maxPeople: 5));

        Build.Add(new BuildStat(H.CannonParts, 400, 15, 5, 25, 5, maxPeople: 5));
        Build.Add(new BuildStat(H.Rum, 400, 15, 5, 25, 5, maxPeople: 5));
        Build.Add(new BuildStat(H.Chocolate, 400, 15, 5, 25, 5, maxPeople: 5));
        Build.Add(new BuildStat(H.Ink, 400, 15, 5, 25, 5, maxPeople: 5));

        //Industry
        Build.Add(new BuildStat(H.Cloth, 400, 15, 5, 25, 5, maxPeople: 5));
        Build.Add(new BuildStat(H.GunPowder, 400, 15, 5, 25, 5, maxPeople: 5));
        Build.Add(new BuildStat(H.Paper, 400, 15, 5, 25, 5, maxPeople: 5));

        Build.Add(new BuildStat(H.Printer, 400, 15, 5, 25, 5, maxPeople: 5));
        Build.Add(new BuildStat(H.CoinStamp, 400, 15, 5, 25, 5, maxPeople: 5));
        Build.Add(new BuildStat(H.Silk, 400, 15, 5, 25, 5, maxPeople: 5));
        Build.Add(new BuildStat(H.SugarMill, 400, 15, 5, 25, 5, maxPeople: 5));

        Build.Add(new BuildStat(H.Foundry, 400, 15, 5, 25, 5, maxPeople: 5));
        Build.Add(new BuildStat(H.SteelFoundry, 400, 15, 5, 25, 5, maxPeople: 5));


        //Trade
        Build.Add(new BuildStat(H.Dock, 600, 80, 20, 0, 5, maxPeople: 10));
        Build.Add(new BuildStat(H.DryDock, 600, 80, 20, 0, 5, maxPeople: 10));
        Build.Add(new BuildStat(H.Supplier, 600, 80, 20, 0, 5, maxPeople: 10));

        Build.Add(new BuildStat(H.StorageSmall, 400, 80, 20, 0, 5, maxPeople: 0, capacity: 150));
        Build.Add(new BuildStat(H.StorageMed, 600, 80, 20, 0, 5, maxPeople: 0, capacity: 200));
        Build.Add(new BuildStat(H.StorageBig, 600, 80, 20, 0, 5, maxPeople: 0, capacity: 300));
        Build.Add(new BuildStat(H.StorageBigTwoDoors, 600, 80, 20, 0, 5, maxPeople: 0, capacity: 300));
        Build.Add(new BuildStat(H.StorageExtraBig, 600, 80, 20, 0, 5, maxPeople: 0, capacity: 420));

        //Gov
        Build.Add(new BuildStat(H.Clinic, 400, 15, 5, 25, 5, maxPeople: 5));
        Build.Add(new BuildStat(H.CommerceChamber, 400, 15, 5, 25, 5, maxPeople: 5));
        Build.Add(new BuildStat(H.Customs, 400, 15, 5, 25, 5, maxPeople: 5));

        Build.Add(new BuildStat(H.Library, 400, 15, 5, 25, 5, maxPeople: 5));
        Build.Add(new BuildStat(H.School, 400, 15, 5, 25, 5, maxPeople: 5));
        Build.Add(new BuildStat(H.TradesSchool, 400, 15, 5, 25, 5, maxPeople: 5));
        Build.Add(new BuildStat(H.TownHouse, 400, 15, 5, 25, 5, maxPeople: 5));

        //Other
        Build.Add(new BuildStat(H.Church, 1600, 80, 20, 0, 5, maxPeople: 3));
        Build.Add(new BuildStat(H.Tavern, 400, 30, 5, 50, 5, maxPeople: 2));
    }

    // Use this for initialization
    public void Start()
    {
        LoadBuildDict();
        MapStructuresCatego();
    }

    void MapStructuresCatego()
    {
        var infrastructure = StInfr.GetValues(typeof(StInfr));
        var housing = StHous.GetValues(typeof(StHous));
        var farming = StFarm.GetValues(typeof (StFarm));
        var raw = StRaw.GetValues(typeof(StRaw));
        var production = StProd.GetValues(typeof(StProd));
        var industry = StInd.GetValues(typeof (StInd));
        var trade = StTrade.GetValues(typeof(StTrade));
        var govServ = StGov.GetValues(typeof(StGov));
        var other = StOther.GetValues(typeof(StOther));
        var structCateg = StCat.GetValues(typeof(StCat));

        MenuGroupsList = UList.ConvertToList(structCateg);

        List<List<H>> listedArrays = new List<List<H>>();

        listedArrays.Add(UList. ConvertToList(infrastructure));
        listedArrays.Add(UList.ConvertToList(housing));
        listedArrays.Add(UList.ConvertToList(farming));
        listedArrays.Add(UList.ConvertToList(raw));
        listedArrays.Add(UList.ConvertToList(production));
        listedArrays.Add(UList.ConvertToList(industry));
        listedArrays.Add(UList.ConvertToList(trade));
        listedArrays.Add(UList.ConvertToList(govServ));
        listedArrays.Add(UList.ConvertToList(other));

        for (int i = 0; i < MenuGroupsList.Count; i++)
        {
            _structuresDict.Add(MenuGroupsList[i], listedArrays[i]);
        }

        foreach (KeyValuePair<H, List<H>> entry in StructuresDict)
        {
            for (int i = 0; i < entry.Value.Count; i++)
            {
                AllStructures.Add(entry.Value[i]);
            }
        }
    }


}

/// <summary>
/// The costs and specifics of each building unit
/// </summary>
public class BuildStat
{
    private int _amountOfLabour;//needed to finish the building
    //All Costs
    private int _wood;
    private int _stone;
    private int _brick;
    private int _iron;
    private int _gold;
    private int _dollar;//Money 

    //Cubic Meters of storage for a build
    private int _capacity;

    //In game prop
    private int _maxPeople;//max amt of peope can leave in a house or can work in a place

    //Technical
    private H _hType;
    private string _root;

    public BuildStat(H hType, int amountOfLabour = 0, int wood = 0, int stone = 0, int brick = 0, int iron = 0,
        int gold = 0, int colonyDollar = 0, int maxPeople = 0, int capacity = 50)
    {
        AmountOfLabour = amountOfLabour;
        HType = hType;
        Root = global::Root.RetBuildingRoot(hType);
        Wood = wood;
        Stone = stone;
        Brick = brick;
        Iron = iron;
        Gold = gold;
        Dollar = colonyDollar;
        _maxPeople = maxPeople;
        _capacity = capacity;
    }

    public BuildStat()
    {
        // TODO: Complete member initialization
    }

    public H HType
    {
        get { return _hType; }
        set { _hType = value; }
    }

    /// <summary>
    /// So far not used. but if will use in future . Double check bridge for posible bugg 
    /// </summary>
    public string Root
    {
        get { return _root; }
        set { _root = value; }
    }

    public int Wood
    {
        get { return _wood; }
        set { _wood = value; }
    }

    public int Stone
    {
        get { return _stone; }
        set { _stone = value; }
    }

    public int Brick
    {
        get { return _brick; }
        set { _brick = value; }
    }

    public int Iron
    {
        get { return _iron; }
        set { _iron = value; }
    }

    public int Gold
    {
        get { return _gold; }
        set { _gold = value; }
    }

    public int Dollar
    {
        get { return _dollar; }
        set { _dollar = value; }
    }

    public int AmountOfLabour
    {
        get { return _amountOfLabour; }
        set { _amountOfLabour = value; }
    }

    public int Capacity
    {
        get { return _capacity; }
        set { _capacity = value; }
    }

    public int MaxPeople
    {
        get { return _maxPeople; }
        set { _maxPeople = value; }
    }
}
