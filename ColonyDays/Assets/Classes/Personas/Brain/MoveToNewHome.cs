/*
 * All actions related to moving to a new home 
 * this was a section in Brain . Decided to make it a class for readability etc
 * 
 */ 

using UnityEngine;
using System.Collections.Generic;

public class MoveToNewHome  {

    private Brain _brain;
    private string _oldHomeKey = "";
    private List<string> _homeOldKeysList = new List<string>();
    private bool buildRouteToNewHome;//main bool here to build route to new Home 
    private bool newHomeRouteStart;
    private RouterManager _newHomeRouter = new RouterManager();
    private int searchedNewHome;//counter of new search for a home 
    private Structure old;
    private TheRoute _routeToNewHome = new TheRoute();

    public MoveToNewHome(Brain brain)
    {
        _brain = brain;
    }

    public string OldHomeKey
    {
        get { return _oldHomeKey; }
        set { _oldHomeKey = value; }
    }

    public List<string> HomeOldKeysList
    {
        get { return _homeOldKeysList; }
        set { _homeOldKeysList = value; }
    }

    public TheRoute RouteToNewHome
    {
        get { return _routeToNewHome; }
        set { _routeToNewHome = value; }
    }

    /// <summary>
    /// Cleans and remove people from the lists in each building listed in 'list' param
    /// </summary>
    private void CleanOldKeyList(List<string> list)
    {
        if (HasOldKeyOfCurrentPlaceAndPlaceWillBeDestroyed(list))
        { return; }

        while (list.Count > 0)
        {
            var oldBuild = list[0];
            RemovePeopleList(oldBuild);
            _brain.DestroyOldBuildIfEmptyOrShack(oldBuild);
            list.RemoveAt(0);
        }
        list.Clear();
    }

    /// <summary>
    /// Will tell u if current person still belown to one of the buildins passed in the list 
    /// 
    /// To avoid people to destroy a building they are in 
    /// 
    /// Address to the case where a person keeps the current building old key but that buildign 
    /// wont be destroy 
    /// </summary>
    private bool HasOldKeyOfCurrentPlaceAndPlaceWillBeDestroyed(List<string> list)
    {
        for (int i = 0; i < _brain.AllPlaces.Length; i++)
        {
            _brain.SetCurrents(_brain.AllPlaces[i]);

            for (int j = 0; j < list.Count; j++)
            {
                if (_brain.CurrStructure == null)
                {
                    return false;
                }

                if (_brain.CurrStructure.MyId == list[j] && _brain.CurrStructure.Instruction == H.WillBeDestroy)
                {
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Removing people from the 'oldBuild' Building object  PeopleDict Dictionary
    /// So we can destroy that building 
    /// </summary>
    private void RemovePeopleList(string oldBuild)
    {
        if (!BuildingPot.Control.Registro.AllBuilding.ContainsKey(oldBuild))
        {
            return;
        }

        //if the person was in the PeopleDict of the oldbuilding gets removed 
        if (BuildingPot.Control.Registro.AllBuilding[oldBuild].PeopleDict.Contains(_brain.Person1.MyId))
        {
            BuildingPot.Control.Registro.AllBuilding[oldBuild].PeopleDict.Remove(_brain.Person1.MyId);
        }
    }

    /// <summary>
    /// MAIN METHOD for moving to nw Home 
    /// 
    /// It has the logic to build the route and everytjhing while moving to new house
    /// </summary>
    public void BuildRouteToNewHomeRoutine()
    {
        if (!buildRouteToNewHome) { return; }
        _newHomeRouter.Update();

        SearchForNewHome();
        if (!newHomeRouteStart)
        {
            SearchForNewHomeAgain();

            //Person1.Home == null person is creating shack 
            //if (Person1.Home == null)
            //{
            //    return;
            //}

            _newHomeRouter = new RouterManager(old, _brain.Person1.Home, _brain.Person1, HPers.NewHome);
            newHomeRouteStart = true;
        }
        //person getting ready to move to new home 
        if (_newHomeRouter.IsRouteReady && _routeToNewHome.CheckPoints.Count == 0
            && _brain.IAmHomeNow())
        {
            //    Debug.Log(Person1.MyId + " setting to new home");

            _routeToNewHome = _newHomeRouter.TheRoute;
            _brain.CurrentTask = HPers.MovingToNewHome;
            GoMindTrue();
            _brain.RoutesWereStarted = false;
            _brain.Person1.Body.Location = HPers.Home;
            _brain.RealeaseIdle(HPers.MovingToNewHome);
        }
    }

    /// <summary>
    /// This is the method that starts the process to moving to new home
    /// </summary>
    private void InitValForNewHome()
    {
        _brain.GoMindState = false;
        //        Debug.Log(Person1.MyId + " InitValForNewHome()");

        _oldHomeKey = _brain.PullOldHome().MyId;

        var firstKeyOnList = _homeOldKeysList[0];
        old = BuildingPot.Control.Registro.AllBuilding[firstKeyOnList] as Structure;
        buildRouteToNewHome = true;
    }

    public void GoMindTrue()
    {
        _brain.GoMindState = true;
        //if not wauting and cant reroute now then im done 
        //PersonPot.Control.DoneReRoute(Person1.MyId);
    }

    /// <summary>
    /// Will keep searching for new home until old is not = to Person.Home
    /// 
    /// This is here to address the case in where a persons home is destroyed twice or more
    /// </summary>
    private void SearchForNewHomeAgain()
    {
        while (old == _brain.Person1.Home)
        {
            SearchForNewHome();
        }
    }

    /// <summary>
    /// Search for new home 
    /// </summary>
    private void SearchForNewHome()
    {
        //childs dont look for new homes here 
        if (newHomeRouteStart)
        { return; }

        _brain.Who = HPers.Home;
        _brain.SearchAgain(true);
        searchedNewHome++;
        
        //then search again next year 
        if (searchedNewHome > 10)
        {
            //searchedNewHome = 0;
            buildRouteToNewHome = false;
            Debug.Log(_brain.Person1.MyId + " searched over 10 times buildRouteToNewHome = false");

            //AddToHomeOldKeysList();
            //Person1.Home = null;

            //BuildShacks();
        }
    }

    /// <summary>
    /// Will create shack if is full and current person the last one doesnt have a house 
    /// 
    /// Every time people come from emigration or somehow are without house I have to marked 
    /// PersonController.UnivCounter=0 and BuilderPot.Control.IsNewHouseSpace=true
    /// </summary>
    private void CheckIfShacksAreNeed()
    {
        var oneHomeLess = ShacksManager.IsAtLeast1HomeLess();
        //GameScene.print("CheckIfShacksAreNeed() called ");

        if (oneHomeLess)
        {
            //GameScene.print("oneHomeLess true called ");
            //BuildShacks();
        }
    }

    public void CleanUpRouteToNewHome()
    {
        newHomeRouteStart = false;
        _newHomeRouter.IsRouteReady = true;
        _routeToNewHome.CheckPoints.Clear();

        //Debug.Log("CleanUpRouteToNewHome goMindState");
        searchedNewHome = 0;
        buildRouteToNewHome = false;
    }

    public void GetMyNameOutOfOldHomePeopleList()
    {
        var t = new List<string>() { _homeOldKeysList[0] };
        _homeOldKeysList.RemoveAt(0);

        CleanOldKeyList(t);
        _oldHomeKey = "";
    }

    /// <summary>
    /// Will add currrent home to _homeOldKeysList so the InitValForNewHome() gets initiated 
    /// </summary>
    public void AddToHomeOldKeysList(string oldHomeP = "")
    {
        if (_brain.Person1.Home == null && oldHomeP != "")
        {
            if (!_homeOldKeysList.Contains(oldHomeP))
            {
                _homeOldKeysList.Add(oldHomeP);
            }
            return;
        }

        if (_brain.Person1.Home == null)
        {
            return;
        }

        if (!_homeOldKeysList.Contains(_brain.Person1.Home.MyId))
        {
            _homeOldKeysList.Add(_brain.Person1.Home.MyId);
        }
    }

    /// <summary>
    /// Aaddress the case where we have some old keys on the list waiting to be proccessed
    /// </summary>
    public void CheckOnOldKeysList(bool inHomeForce = false)
    {
        //if the oldHomeKey was cleared and homeOldKeysList has more than one means that we have
        //olds key to address 
        if (_oldHomeKey == "" && _homeOldKeysList.Count > 0 && _routeToNewHome.CheckPoints.Count == 0 && _brain.IAmHomeNow())
        {
            InitValForNewHome();
        }
    }

    //so i dont overwrite old keys 

    ///// <summary>
    ///// Will create the shck manager. If is not being initiated yet 
    ///// </summary>
    //private void BuildShacks()
    //{
    //    if (ShacksManager.State == H.None)
    //    {
    //        GameScene.print("State == H.None true called ");
    //        //ShacksManager.Start();
    //    }
    //}

    //void CheckOnGenOldKeys()
    //{
    //    if (_generalOldKeysList.Count > 0 && IAmHomeNow())
    //    {
    //        CleanOldKeyList(_generalOldKeysList);
    //    }
    //}

}
