﻿using System;
using UnityEngine;

/*
 * Created with the Simple purpose of From current point go get food and go home, drop food
 * And thats it
 *
 */

public class Homer : Profession
{
    private Structure BuildToGoBackTo;//the food src of the homer

    public Homer(Person person, PersonFile pF)
    {
        if (pF == null)
        {
            CreatingNew(person);
        }
        else LoadingFromFile(person, pF);
    }

    private void CreatingNew(Person person)
    {
        _person = person;
        Init();
    }

    private void LoadingFromFile(Person person, PersonFile pF)
    {
        _person = person;

        LoadAttributes(pF.ProfessionProp);

        //so drops stuff in there
        BuildToGoBackTo = BuildToGoBackToDefine();

        var router1 = Router1 == null && Router1.TheRoute == null;
        var routerBack = RouterBack == null && RouterBack.TheRoute == null;

        if (_person.PrevJob == Job.Farmer && _person.Work != null && _person.Work.MyId.Contains("Farm")
            && WorkerTask == HPers.None//standing in field
            )
        {
            FinRoutePoint = DefineFinRoute();
            InitRoute();
        }
    }

    private void Init()
    {
        //Debug.Log(_person.MyId + " new Homer");
        HandleNewProfDescrpSavedAndPrevJob(Job.Homer);
        BuildToGoBackTo = BuildToGoBackToDefine();
        FinRoutePoint = DefineFinRoute();

        _routingStarted = null;
        _routingStartPos = new Vector3();

        InitRoute();
    }

    private Structure BuildToGoBackToDefine()
    {
        //if is a wheelbarrow working from its DestinyBuild will go back to Work. and from there to home.
        //this is to make it neat with the PersonalObject spawned
        //Heavy load too should drop Cow and Cart at work place
        if (_person.PrevJob == Job.WheelBarrow && _person.Work != null
            && (_person.Work.HType == H.Masonry || _person.Work.HType == H.HeavyLoad))
        {
            return _person.Work;
        }

        //they just need to go there and then their houuse
        if (_person.PrevJob == Job.Docker)
        {
            return _person.FoodSource;
        }

        if (!_person.Work.DoIHaveInput())
        {
            return _person.FoodSource;
        }

        //so now they will take it to the closest storage from the production point
        return _person.Work.PreferedStorage;
    }

    private bool IsWorkNaval()
    {
        return _person.Work != null && _person.Work.IsNaval();
    }

    private Vector3 DefineFinRoute()
    {
        if (IsWorkNaval())
        {
            return _person.Work.SpawnPoint.transform.position;
        }
        else
        {
            return BuildToGoBackTo.SpawnPoint.transform.position;
        }
    }

    private void InitRoute()
    {
        RouterActive = true;

        if (BuildToGoBackTo != null)
        {
            InitRouteWithFoodSrc();
        }
        else
        {
            throw new Exception("MyFoodSrc cant be null");
        }
    }

    /// <summary>
    /// Init a route that person will go to FoodSrc and then to Home
    /// </summary>
    private void InitRouteWithFoodSrc()
    {
        IsRouterBackUsed = true;

        Structure building = null;
        if (_person.PrevOrder != null)
        {
            building = Brain.GetStructureFromKey(_person.PrevOrder.DestinyBuild);
        }

        //bz docker coming back after import has the same building == BuildToGoBackTo
        var dockerComingBack = _person.PrevJob == Job.Docker && building == BuildToGoBackTo;
        if (building == null || dockerComingBack)
        {
            //exporting will finish at destiny build is null(not sure)
            building = _person.Work;
        }

        Router1 = new CryRouteManager(building, BuildToGoBackTo, _person);
        RouterBack = new CryRouteManager(BuildToGoBackTo, _person.Home, _person, HPers.InWork);

        _routingStarted = Program.gameScene.GameTime1.CurrentDate();
        _routingStartPos = _person.transform.position;
    }

    public override void Update()
    {
        if (_takeABreakNow)
        {
            TakeABreak();
            return;
        }

        base.Update();

        WorkAction(HPers.None);
        Execute();
        CheckWhenDone();
    }

    private void Execute()
    {
        if (ExecuteNow)
        {
            ExecuteNow = false;
            DropAllMyGoods(BuildToGoBackTo);

            CheckOnWorkInputOrders();

            //if is not empty means got something for inputs...
            if (_person.Inventory.IsEmpty())
            {
                if (ExDockerIsGettingFood())
                {
                    _person.GetFood(BuildToGoBackTo);
                }
                else if (_person.PrevJob != Job.Docker)
                {
                    _person.GetFood(BuildToGoBackTo);
                }
            }

            _person.Body.ResetPersonalObject();
            ComingBackToOffice();
        }
    }

    /// <summary>
    /// Once on Storage will attempt to pick Inputs for work
    /// </summary>
    private void CheckOnWorkInputOrders()
    {
        if (_person.DoesHasInputOrders())
        {
            var ord = _person.ReturnFirstOrder();
            if (ord != null)
            {
                //getting the input item
                var amt = _person.HowMuchICanCarry() * 2;
                _person.ExchangeInvetoryItem(BuildToGoBackTo, _person, ord.Product, amt, BuildToGoBackTo);
            }
        }
    }

    /// <summary>
    /// As exDocker will have wheelbarrow will get food once every 4 times random
    /// </summary>
    /// <returns></returns>
    private bool ExDockerIsGettingFood()
    {
        if (_person.PrevJob != Job.Docker)
        {
            return false;
        }
        return UMath.GiveRandom(0, 4) == 0;
    }

    private void CheckWhenDone()
    {
        //person its at home
        if (_person.Body.Location == HPers.Home && _person.Body.GoingTo == HPers.Home)
        {
            _person.HomeActivities();

            //UVisHelp.CreateText(_person.transform.position, "Home Now");

            //         //Debug.Log(_person.MyId + " not homer anymore now will be a: " + _person.PrevJob);

            if (_person.PrevJob == Job.WheelBarrow || _person.PrevJob == Job.Builder// || _person.PrevJob == Job.None
                )
            {
                ConvertToWheelBarrOrBuilder();
            }
            else
            {
                _person.CreateProfession(_person.PrevJob);
            }
        }
    }

    private void ConvertToWheelBarrOrBuilder()
    {
        if (CheckIfCanBeWheelBar(_person))
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
    public static bool CheckIfCanBeWheelBar(Person per)
    {
        if (per == null || per.Work == null)
        {
            return false;
        }
        //his work is not a BUliding office
        if (per.Work.BuildersManager1 == null)
        {
            return true;
        }

        //for heavyLoad. They only transport load so they are always ready to wheelbarrow
        if (per.Work.HType == H.HeavyLoad)
        {
            return true;
        }

        //will check only when is done
        //if one building is up. This person will convert into a Builder
        if (per.Work.BuildersManager1.IsAtLeastOneBuildUp())
        {
            //
            return false;
        }
        return true;
    }

    private bool _takeABreakNow;
    private float _breakDuration = 1f;
    private float startIdleTime;
    private bool _reInit;
    private MDate _routingStarted;
    private Vector3 _routingStartPos;

    /// <summary>
    /// Used so a person is asking for bridges anchors takes a break and let brdige anchors complete then can
    /// work on it
    /// </summary>
    private void TakeABreak()
    {
        if (startIdleTime == 0)
        { startIdleTime = Time.time; }

        if (Time.time > startIdleTime + _breakDuration)
        {
            _takeABreakNow = false;
            startIdleTime = 0;

            //so it restarted
            Init();
        }
    }
}