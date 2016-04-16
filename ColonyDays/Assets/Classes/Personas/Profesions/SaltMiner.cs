using System;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class SaltMiner : Profession
{
    public SaltMiner(Person person, PersonFile pF)
    {
        if (pF == null)
        {
            CreatingNew(person);
        }
        else LoadingFromFile(person, pF);
    }

    void CreatingNew(Person person)
    {
        //in case was a Wheelbarrow the prevProfession and when home route back gives problem 
        person.PrevOrder = null;

        IsRouterBackUsed = false;
        MyAnimation = "isSummon";
        _person = person;
        HandleNewProfDescrpSavedAndPrevJob(Job.SaltMiner);

        Init();
    }

    void LoadingFromFile(Person person, PersonFile pF)
    {
        _person = person;
        LoadAttributes(pF.ProfessionProp);
    }

    private void Init()
    {
        if (ShouldITakeBreakInit())
        {
            return;
        }

        //when get a number here is defined by wht worker is this on the building 
        //workers will be numbered on buildingsB
        FinRoutePoint = ReturnRandomFinalPoint();

        InitRoute();
    }

    /// <summary>
    /// Bz the Fisher site has varius endings 
    /// </summary>
    /// <returns></returns>
    Vector3 ReturnRandomFinalPoint()
    {
        List<Vector3> list = new List<Vector3>()
        {
            _person.Work.InBuildWorkPoint01,
            _person.Work.InBuildWorkPoint02,
            _person.Work.InBuildWorkPoint03
        };

        var ind = Random.Range(0, list.Count);
        return list[ind];
    }

    void InitRoute()
    {
        RouterActive = true;
        ConformInBuildRoute();

        RouteBackForNewProfThatUseHomer();
    }

    void ConformInBuildRoute()
    {
        Router1 = new CryRouteManager();

        if (PersonPot.Control.RoutesCache1.ContainANewerOrSameRoute(_person.Work.MyId + ".OFish", _person.Work.MyId + ".DFish",
                   new DateTime()))
        {
            Router1.TheRoute = PersonPot.Control.RoutesCache1.GiveMeTheNewerRoute();
            Router1.IsRouteReady = true;
            return;
        }



        var inBuildPoints = DefineInBuildPoint();
        //UVisHelp.CreateHelpers(inBuildPoints, Root.yellowCube);
        var TheRoute = ReachBean.RouteVector3s(inBuildPoints);

        //so they go trhu on Profession 
        TheRoute.OriginKey = _person.Work.MyId + ".OFish";
        TheRoute.DestinyKey = _person.Work.MyId + ".DFish";

        Router1.TheRoute = TheRoute;
        Router1.IsRouteReady = true;


    }

    List<Vector3> DefineInBuildPoint()
    {
        List<Vector3> points = new List<Vector3>();

        points.Add(_person.Work.BehindMainDoorPoint);
        points.Add(_person.Work.InBuildIniPoint);
        points.Add(_person.Work.InBuildMidPointA);
        points.Add(FinRoutePoint);
        return points;
    }

    public override void Update()
    {
        if (_reInitNow)
        {
            _reInitNow = false;
            Init();
            return;
        }

        base.Update();

        if (_breakInitNow)
        {
            return;
        }

        Execute();
    }

    /// <summary>
    /// The specific action of a Proffession 
    /// Ex: Forester add lumber to its inventory and removed the amt from tree invetory
    /// </summary>
    void Execute()
    {
        if (ExecuteNow && ReadyToWork)
        {
            ExecuteNow = false;
            //do stuff
            base.Execute();
        }
        else if (ExecuteNow && !ReadyToWork)
        {
            //so we leave it for next time to see if is ready
            //othwrwise gives buggg bz wherever is will do stuff as is will be inside the work place
            ExecuteNow = false;
        }
    }
}
