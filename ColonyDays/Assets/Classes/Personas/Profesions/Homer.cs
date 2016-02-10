using System;
using UnityEngine;

/*
 * Created with the Simple purpose of From current point go get food and go home, drop food
 * And thats it
 * 
 */
public class Homer : Profession
{
    private Structure MyFoodSrc;//the food src of the homer

    public Homer(Person person, PersonFile pF)
    {
        if (pF == null)
        {
            CreatingNew(person);
        }
        else LoadingFromFile(person, pF);
    }

    void CreatingNew(Person person)
    {
        _person = person;
        Init();
    }

    void LoadingFromFile(Person person, PersonFile pF)
    {
        _person = person;
        LoadAttributes(pF.ProfessionProp);
        Init();
    }

    private void Init()
    {
        MyFoodSrc = _person.FoodSource;
//       //Debug.Log(_person.MyId + " new Homer");

        FinRoutePoint = DefineFinRoute();
        ProfDescription = Job.Homer;

        InitRoute();
    }

    Vector3 DefineFinRoute()
    {
        if (_person.Work.HType.ToString().Contains("Dock"))
        {
            return _person.Work.SpawnPoint.transform.position;
        }
        else
        {
            return MyFoodSrc.SpawnPoint.transform.position;
        }
    }

    void InitRoute()
    {
        _routerActive = true;
        dummy = CreateDummy();
        dummy.transform.position = FinRoutePoint;
        dummy.HandleLandZoning();

        if (MyFoodSrc != null)
        {
            InitRouteWithFoodSrc();
        }
        else
        {
            throw new Exception("MyFoodSrc cant be null");
            InitRouteWithOutFoodSrc();
        }
    }

    Structure CreateDummy()
    {

        return Program.gameScene.GimeMeUnusedDummy(ProfDescription + ".Dummy." + _person.PrevJob+"." + _person.Home);
    }

    /// <summary>
    /// Init a route that person will go to FoodSrc and then to Home 
    /// </summary>
    void InitRouteWithFoodSrc()
    {
//       //Debug.Log(_person.MyId+ ".Prev job:" + _person.PrevJob);
        IsRouterBackUsed = true;

        Structure building = Brain.GetStructureFromKey(_person.PrevOrder.DestinyBuild);

        if (building == null //&& ProfDescription == Job.Docker
            )
        {
            //exporting will finish at destiny build is null(not sure)
            building = _person.Work;
        }

        Router1 = new CryRouteManager(building, MyFoodSrc, _person);
        //Router1 = new RouterManager(building, MyFoodSrc, _person, HPers.InWork);
        RouterBack = new CryRouteManager(MyFoodSrc, _person.Home, _person,  HPers.InWork);
    }

    /// <summary>
    /// Init a Route where from current point poerson goes directly to home
    /// </summary>
    void InitRouteWithOutFoodSrc()
    {
        if (_person.PrevJob == Job.WheelBarrow)
        {
            Structure building = Brain.GetStructureFromKey(_person.PrevOrder.DestinyBuild);
            Router1 = new CryRouteManager(building, _person.Home, _person);
          //  Router1 = new RouterManager(building, _person.Home, _person, HPers.InWork);
        }
        else
        {
            Router1 = new CryRouteManager(dummy, _person.Home, _person );
         //   Router1 = new RouterManager(dummy, _person.Home, _person, HPers.InWork, false, true);
        }
    }

    public override void Update()
    {
        base.Update();
        WorkAction(HPers.None);
        Execute();
        CheckWhenDone();
    }

    void Execute()
    {
        if (ExecuteNow)
        {
            ExecuteNow = false;
            _person.GetFood(MyFoodSrc);
        }
    }

    private void CheckWhenDone()
    {
        //person its at home
        if (_person.Body.Location == HPers.Home && _person.Body.GoingTo == HPers.Home)
        {
            _person.HomeActivities();
            //UVisHelp.CreateText(_person.transform.position, "Home Now");

//         //Debug.Log(_person.MyId + " not homer anymore now will be a: " + _person.PrevJob);

            if (_person.PrevJob == Job.WheelBarrow || _person.PrevJob == Job.Builder)
            {
                ConvertToWheelBarrOrBuilder();
            }
            else
            {
                _person.CreateProfession(_person.PrevJob);
            }
        }
    }

    void ConvertToWheelBarrOrBuilder()
    {
        
        if (CheckIfCanBeWheelBar())
        {
            _person.CreateProfession(Job.WheelBarrow);
        }
        else
        {
            _person.CreateProfession(Job.Builder);
        }        
    }

    /// <summary>
    /// Checks need to do when done 
    /// </summary>
    public static bool CheckIfCanBeWheelBar()
    {
        //will check only when is done 
        //if one building is up. This person will convert into a Builder 
        if (PersonPot.Control.BuildersManager1.IsAtLeastOneBuildUp())
        {
            //
            return false;
        }
        return true;
    }
}
