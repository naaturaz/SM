/*
 * All actions related to moving to a new home 
 * this was a section in Brain . Decided to make it a class for readability etc
 * 
 */

using System;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Is inheritng from Brain just to access the field:
///  _person 
///  currStructure 
/// 
/// It couldnt be public bz XML serializer will find redundancy
/// </summary>
public class MoveToNewHome
{
    private Brain _brain;
    private Person _person;

    private string _oldHomeKey = "";
    private List<string> _homeOldKeysList = new List<string>();
    private TheRoute _routeToNewHome = new TheRoute();

    private bool buildRouteToNewHome;//main bool here to build route to new Home 
    private bool newHomeRouteStart;
    private CryRouteManager _newHomeRouter = new CryRouteManager();
    private int searchedNewHome;//counter of new search for a home 
    private Structure old;

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



    public MoveToNewHome() { }

    public MoveToNewHome(Brain brain, Person person)
    {
        _brain = brain;
        _person = person;
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
            RemovePeopleDict(oldBuild);
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
            //_brain.SetCurrents(_brain.AllPlaces[i]);
            for (int j = 0; j < list.Count; j++)
            {
                var currSt = _brain.ReturnCurrStructure(_brain.AllPlaces[i]);
                if (currSt == null)
                {
                    return false;
                }

                if (currSt.MyId == list[j] && currSt.Instruction == H.WillBeDestroy)
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
    public void RemovePeopleDict(string oldBuild)
    {
        if (!BuildingPot.Control.Registro.AllBuilding.ContainsKey(oldBuild))
        {
            return;
        }

        //if the person was in the PeopleDict of the oldbuilding gets removed 
        if (BuildingPot.Control.Registro.AllBuilding[oldBuild].PeopleDict.Contains(_person.MyId))
        {
            BuildingPot.Control.Registro.AllBuilding[oldBuild].PeopleDict.Remove(_person.MyId);
            BuildingPot.Control.Registro.ResaveOnRegistro(oldBuild);

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
            //_person.Home == null person is in the proccess of getting a new house   
            if (_person.Home == null)
            {
                return;
            }

            _newHomeRouter = new CryRouteManager(ReturnCorrectInitStructure(), 
                _person.Home, _person, HPers.NewHome);
            newHomeRouteStart = true;
        }
        //person getting ready to move to new home 
        if (_newHomeRouter.IsRouteReady && _routeToNewHome.CheckPoints.Count == 0
            && _brain.IAmHomeNow())
        {
            //Debug.Log(_person.MyId + " setting to new home");
            _routeToNewHome = _newHomeRouter.TheRoute;
            GoMindTrue();
            _brain.RoutesWereStarted = false;
            //_person.Brain.MindState();

            //bz was overwritting that and people would be stuck on home after tey moved in
            if (_person.Body.Location != HPers.MovingToNewHome)
            {
                _person.Body.Location = HPers.Home;
            }
        }
    }

    Structure ReturnCorrectInitStructure()
    {
        Structure dummy = null;

        if (old != null)
        {
            return old;
        }
        //todo
        //The real solution is to find the closest building in MoveToNEwHome
        return Brain.GetStructureFromKey(_person.StartingBuild);
    }

    /// <summary>
    /// This is the method that starts the process to moving to new home
    /// </summary>
    private void InitValForNewHome()
    {
        //_routeToNewHome.CheckPoints.Clear();

        if (_brain.PullOldHome() != null && _brain.PullOldHome() == _person.Home)
        {   //means is moving towards the same house 
            //todo fix this keep being calling thru the game
            //Debug.Log(_person.MyId + " InitValForNewHome() Canceled");
            return;
        }

        _brain.GoMindState = false;
        Debug.Log(_person.MyId + " InitValForNewHome()");

        //_oldHomeKey = _brain.PullOldHome().MyId;
        var firstKeyOnList = _homeOldKeysList[0];
        _oldHomeKey = firstKeyOnList;

        old = BuildingPot.Control.Registro.AllBuilding[firstKeyOnList] as Structure;
        buildRouteToNewHome = true;

        //so that state happens 
        _brain.CurrentTask = HPers.MovingToNewHome;
        _brain.CheckMeOnSystemNow();
    }

    void InitValForNewHomeForNewSpawned()
    {
        //_routeToNewHome.CheckPoints.Clear();
        Debug.Log(_person.MyId + " InitValForNewHomeForNewSpawned()");

        _brain.GoMindState = false;
        buildRouteToNewHome = true;
        _brain.CurrentTask = HPers.MovingToNewHome;

        //so the states works
        _person.Body.GoingTo = HPers.Home;

        _brain.CheckMeOnSystemNow();
    }

    void GoMindTrue()
    {
        _brain.GoMindState = true;
        //if not wauting and cant reroute now then im done 
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

        //only will be added if is not moving bz in Brain.CheckHome() we wont allow the search if is moving 
        if (!_person.Body.MovingNow)
        {
            searchedNewHome++;
        }

        if (searchedNewHome > 50)
        {
           //Debug.Log("whoGreenMeToBecomeMajor: " +_person.PersonReport.whoGreenMeToBecomeMajor+
            //    "."+_person.MyId);
        }

        //at 11x , 100 was fine. So I leave it like this 
        //what happens is that ask too many times close to each other and breaks 
        if (searchedNewHome > 100)// * Program.gameScene.GameSpeed)
        {
            throw new Exception("House never should be searched more than 100 times since was initiated" +
                                "bz was confirmed that a house exist for this person to move in" +
                                "pls check in condintions that initatied : InitValForNewHome()");
        }
    }

    public void CleanUpRouteToNewHome()
    {
        newHomeRouteStart = false;
        //_newHomeRouter.IsRouteReady = true;

        //Debug.Log("CleanUpRouteToNewHome "+_person.MyId +" famID:"+_person.FamilyId);
        searchedNewHome = 0;
        buildRouteToNewHome = false;
        _routeToNewHome.CheckPoints.Clear();
    }

    public void GetMyNameOutOfOldHomePeopleList()
    {
        //addressing when is a brand new spwaned
        if (_homeOldKeysList.Count==0)
        {
            return;
        }

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
        if (_person.Home == null && oldHomeP != "")
        {
            if (!_homeOldKeysList.Contains(oldHomeP))
            {
                _homeOldKeysList.Add(oldHomeP);
            }
            return;
        }

        if (_person.Home == null)
        {
            return;
        }

        if (!_homeOldKeysList.Contains(_person.Home.MyId))
        {
            _homeOldKeysList.Add(_person.Home.MyId);
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

        if (_person.Brain.JustSpawned() && _person.Home != null)
        {
            InitValForNewHomeForNewSpawned();
        }
    }
}
