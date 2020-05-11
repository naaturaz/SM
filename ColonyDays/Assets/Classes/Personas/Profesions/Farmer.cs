﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class Farmer : Profession
{
    public Farmer(Person person, PersonFile pF)
    {
        if (pF == null)
        {
            CreatingNew(person);
        }
        else LoadingFromFile(person, pF);
    }

    private void CreatingNew(Person person)
    {
        if (person.PrevOrder != null)
        {
            //other wise will malfuntion when creating backRouting
            person.PrevOrder = null;
        }

        IsRouterBackUsed = false;
        MyAnimation = "isHoe";
        _person = person;
        HandleNewProfDescrpSavedAndPrevJob(Job.Farmer);

        Init();
    }

    private void LoadingFromFile(Person person, PersonFile pF)
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

        //will return cache 20 validated points in Farm and then no need of run this anymore
        FinRoutePoint = _person.Work.ReturnFarmWorkPoint();

        if (FinRoutePoint == new Vector3())
        {
            FinRoutePoint = DefineFinalPoint();
        }

        InitRoute();
    }

    /// <summary>
    /// Will ramdomly assign an final point in the FarmZone
    /// </summary>
    /// <returns></returns>
    private Vector3 DefineFinalPoint()
    {
        Rect area = _person.Work.ReturnInGameObjectZone(H.FarmZone);
        var middlePointOfArea = _person.Work.ReturnGroundMiddleOfInGameObjectZone(H.FarmZone);

        var newPoint = AssignRandomIniPosition(middlePointOfArea, area);
        if (newPoint == new Vector3())
        {
            return middlePointOfArea;
        }
        //since is validated will be added as cached point
        _person.Work.AddAsFarmWorkPoint(newPoint);
        return newPoint;
    }

    private int count;

    private Vector3 AssignRandomIniPosition(Vector3 origin, Rect area)
    {
        var howFar = HowFar();

        float x = UMath.Random(-howFar, howFar);
        float z = UMath.Random(-howFar, howFar);
        origin = new Vector3(origin.x + x, origin.y, origin.z + z);

        if (!area.Contains(new Vector2(origin.x, origin.z)))
        {
            count++;
            if (count > 1000)
            {
                Debug.Log("AssignRandomIniPosition() Farmer.cs over 1000");
                //throw new Exception("AssignRandomIniPosition() Farmer.cs");
                count = 0;
                return new Vector3();
            }
            origin = AssignRandomIniPosition(origin, area);
        }
        return origin;
    }

    private float HowFar()
    {
        if (_person.Work == null)
        {
            return 0.1f;
        }

        if (_person.Work.MyId.Contains("Small"))
        {
            return .5f;
        }
        if (_person.Work.MyId.Contains("Med"))
        {
            return 1f;
        }
        if (_person.Work.MyId.Contains("XLarge"))
        {
            return 2.5f;
        }
        if (_person.Work.MyId.Contains("Large"))
        {
            return 2f;
        }
        //unkown
        return .1f;
    }

    private void InitRoute()
    {
        RouterActive = true;
        Router1 = new CryRouteManager();

        if (_person.Work.HType.ToString().Contains(H.AnimalFarm + ""))
        {
            ConformInBuildRouteAnimal();
        }
        else
        {
            ConformInBuildRouteField();
        }

        RouteBackForNewProfThatUseHomer();
    }

    /// <summary>
    /// For a ffield farm
    /// </summary>
    private void ConformInBuildRouteField()
    {
        //if (PersonPot.Control.RoutesCache1.ContainANewerOrSameRoute(_person.Work.MyId + ".O", _person.Work.MyId + ".D",
        //        new DateTime()))
        //{
        //    Router1.TheRoute = PersonPot.Control.RoutesCache1.GiveMeTheNewerRoute();
        //    Router1.IsRouteReady = true;
        //    return;
        //}

        List<Vector3> inBuildPoints = new List<Vector3>()
        { _person.Work.BehindMainDoorPoint, FinRoutePoint};

        var TheRoute = ReachBean.RouteVector3s(inBuildPoints);

        //the .O is to pass the profession or brain reurn
        TheRoute.OriginKey = _person.Work.MyId + ".O";
        TheRoute.DestinyKey = _person.Work.MyId + ".D";

        Router1.TheRoute = TheRoute;
        Router1.IsRouteReady = true;

        //PersonPot.Control.RoutesCache1.AddReplaceRoute(TheRoute);
    }

    /// <summary>
    /// For an animal farm
    /// </summary>
    private void ConformInBuildRouteAnimal()
    {
        if (PersonPot.Control.RoutesCache1.ContainANewerOrSameRoute(_person.Work.MyId + ".O", _person.Work.MyId + ".D",
                   new DateTime()))
        {
            Router1.TheRoute = PersonPot.Control.RoutesCache1.GiveMeTheNewerRoute();
            Router1.IsRouteReady = true;
            return;
        }

        var inBuildPoints = DefineInBuildPoint();
        var TheRoute = ReachBean.RouteVector3s(inBuildPoints);

        //the .O is to pass the profession or brain reurn
        TheRoute.OriginKey = _person.Work.MyId + ".O";
        TheRoute.DestinyKey = _person.Work.MyId + ".D";

        Router1.TheRoute = TheRoute;
        Router1.IsRouteReady = true;

        PersonPot.Control.RoutesCache1.AddReplaceRoute(TheRoute);
    }

    private List<Vector3> DefineInBuildPoint()
    {
        List<Vector3> points = new List<Vector3>();

        points.Add(_person.Work.BehindMainDoorPoint);
        points.Add(_person.Work.InBuildIniPoint);
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
    private void Execute()
    {
        if (ExecuteNow && ReadyToWork)
        {
            ExecuteNow = false;
            //do stuff
            base.Execute();
        }
        else if (ExecuteNow && !ReadyToWork)
        {
            //so we leave it for next time to see if is ready ReadyToWork
            //othwrwise gives buggg bz wherever is will do stuff as is will be inside the work place
            ExecuteNow = false;
        }
    }
}