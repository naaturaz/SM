using System.Collections.Generic;

public class BuildingData
{
    //all buildings
    private List<RegFile> _all = new List<RegFile>();

    //building controller data to save and load 
    public BuildingControllerData BuildingControllerData = new BuildingControllerData();

    public List<RegFile> All
    {
        get { return _all; }
        set { _all = value; }
    }

    public BuildingData(List<RegFile> all, BuildingControllerData BuildingControllerDataP)
    {
        _all = all;
        BuildingControllerData = BuildingControllerDataP;
    }

    public BuildingData() { }
}

public class BuildingControllerData
{
    public List<string> _foodSources = new List<string>();
    //all buildins that have open Positions of work ... I only hold the keys (MyId s)
    public List<string> _workOpenPos = new List<string>();
    //houses that currently have more space available
    public List<string> _housesWithSpace = new List<string>();
    //houses that currently have more space available
    public List<string> _religiousBuilds = new List<string>();
    //houses that currently have more space available
    public List<string> _chillBuilds = new List<string>();
    //new ways built
    public List<string> _wayBuilds = new List<string>();


    public bool _isfoodSourceChange;//if any one was removed or added from the list will be mark as true
    public bool _isWorkChanged;//new work pos opened
    public bool _isHouseSpaceChanged;//new space available in a house
    public bool _isReligionChanged;
    bool _isChillChanged;//new tavern etc

    public bool IsChillChanged
    {
        get { return _isChillChanged; }
        set { _isChillChanged = value; }
    }

    public DispatchManager DispatchManager1;

    public BuildingControllerData() { }



    //They load on BuildingSaveLoad. LoadBuildingController()

    ///GameTime Data
    /// 
    public GameTime _GameTime;

    ///GameController Data
    public GameController _GameController;
}