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
        FinRoutePoint = _person.Work.InBuildWorkPoint01;

        InitRoute();
    }

    void InitRoute()
    {
        _routerActive = true;
        ConformInBuildRoute();
    }

    void ConformInBuildRoute()
    {
        var inBuildPoints = DefineInBuildPoint();
        var TheRoute = ReachBean.RouteVector3s(inBuildPoints);

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
