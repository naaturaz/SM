using UnityEngine;
using System.Collections.Generic;

public class FisherMan : Profession {

    public FisherMan(Person person, PersonFile pF)
    {
        if (pF == null)
        {
            CreatingNew(person);
        }
        else LoadingFromFile(person, pF);
    }

    void CreatingNew(Person person)
    {
        ProfDescription = Job.FisherMan;
        IsRouterBackUsed = false;
        MyAnimation = "isSummon";
        _person = person;
        Init();
    }

    void LoadingFromFile(Person person, PersonFile pF)
    {
        _person = person;
        LoadAttributes(pF.ProfessionProp);
    }

    private void Init()
    {
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
        _routerActive = true;
        ConformInBuildRoute();
    }

    void ConformInBuildRoute()
    {
        Router1 = new CryRouteManager();

        var inBuildPoints = DefineInBuildPoint();
        //UVisHelp.CreateHelpers(inBuildPoints, Root.yellowCube);
        var TheRoute = ReachBean.RouteVector3s(inBuildPoints);

        //so they go trhu on Profession 
        TheRoute.OriginKey = "PointIniFish";
        TheRoute.DestinyKey = "PointFinFish";

        Router1.TheRoute=TheRoute;
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
        base.Update();
        Execute();
    }

    /// <summary>
    /// The specific action of a Proffession 
    /// Ex: Forester add lumber to its inventory and removed the amt from tree invetory
    /// </summary>
    void Execute()
    {
        if (ExecuteNow)
        {
            ExecuteNow = false;
            base.Execute();
            //do stuff
        }
    }
}
