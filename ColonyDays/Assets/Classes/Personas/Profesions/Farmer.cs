using UnityEngine;
using System.Collections.Generic;

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

    void CreatingNew(Person person)
    {
        if (person.PrevOrder!=null)
        {
            //other wise will malfuntion when creating backRouting 
            person.PrevOrder = null;
        }

        person.PrevJob = ProfDescription;
        ProfDescription = Job.Farmer;
        IsRouterBackUsed = false;
        MyAnimation = "isHoe";
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
        FinRoutePoint = DefineFinalPoint(); 

        InitRoute();
    }

    /// <summary>
    /// Will ramdomly assign an final point in the FarmZone
    /// </summary>
    /// <returns></returns>
    Vector3 DefineFinalPoint()
    {
        Rect area = _person.Work.ReturnInGameObjectZone(H.FarmZone);
        var middlePointOfArea  = _person.Work.ReturnGroundMiddleOfInGameObjectZone(H.FarmZone);

        return Animal.AssignRandomIniPosition(middlePointOfArea, area);
    }

    void InitRoute()
    {
        RouterActive = true;
        Router1 = new CryRouteManager();

        if (_person.Work.HType.ToString().Contains(H.AnimalFarm+""))
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
        List<Vector3> inBuildPoints = new List<Vector3>() 
        { _person.Work.BehindMainDoorPoint, FinRoutePoint};

        var TheRoute = ReachBean.RouteVector3s(inBuildPoints);
       
        //the .O is to pass the profession or brain reurn 
        TheRoute.OriginKey = _person.Work.MyId + ".O";
        TheRoute.DestinyKey = _person.Work.MyId + ".D";

        Router1.TheRoute = TheRoute;
        Router1.IsRouteReady = true;
    }

    /// <summary>
    /// For an animal farm 
    /// </summary>
    void ConformInBuildRouteAnimal()
    {
        var inBuildPoints = DefineInBuildPoint();
        var TheRoute = ReachBean.RouteVector3s(inBuildPoints);

        //the .O is to pass the profession or brain reurn 
        TheRoute.OriginKey = _person.Work.MyId + ".O";
        TheRoute.DestinyKey = _person.Work.MyId + ".D";

        Router1.TheRoute=TheRoute;
        Router1.IsRouteReady = true;
    }

    List<Vector3> DefineInBuildPoint()
    {
        List<Vector3> points = new List<Vector3>();

        points.Add(_person.Work.BehindMainDoorPoint);
        points.Add(_person.Work.InBuildIniPoint);
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
