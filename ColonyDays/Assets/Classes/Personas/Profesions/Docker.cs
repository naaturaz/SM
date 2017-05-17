﻿using System;
using UnityEngine;

public class Docker : Profession
{


    public Docker(Person person, PersonFile pF)
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
        _wasLoaded = true;

        _person = person;
        LoadAttributes(pF.ProfessionProp);

        if (_destinyBuild == null || _sourceBuild == null)
        {
            _takeABreakNow = true;
            return;
        }

        InitRoute();
    }

    void Init()
    {
        if (_wasLoaded)
        {
            return;
        }

        //so its not using the same order over and over again in case the Dispatch is finding nothing 
        CleanOldVars();

        //Debug.Log(_person.MyId+" Init WheelB");
        HandleNewProfDescrpSavedAndPrevJob(Job.Docker);

        MyAnimation = "isWheelBarrow";

        PickUpOrder();

        ////means no Orders avail 
        //if (_destinyBuild == null)
        //{

        //    _takeABreakNow = true;
        //    return;
        //}

        InitRoute();
    }

    private void CleanOldVars()
    {
        _sourceBuild = null;//from where taking the load 
        _destinyBuild = null;//where taking load 
        Order1 = null;
    }

    private void PickUpOrder()
    {
        if (_person.Work == null || !_person.Work.IsNaval())//bz takes a cycle to person get its new job 
        {
            return;
        }

        Order1 = _person.Work.Dispatch1.GiveMeOrderDocker(_person);
        _person.PrevOrder = Order1;

        if (Order1 == null)
        {
            return;
        }

        SetSourceAndDestinyBuild();
    }

    void SetSourceAndDestinyBuild()
    {
        _destinyBuild = GetStructureSrcAndDestinyExpImp();
        _sourceBuild = GetStructureSrcAndDestinyExpImp();

        DestinyBuildKey = Order1.DestinyBuild;
        SourceBuildKey = Order1.SourceBuild;
    }

    void InitRoute()
    {
        RouterActive = true;
        Router1 = new CryRouteManager(_person.Work, _person.FoodSource, _person, HPers.InWork);

        //IsRouterBackUsed = true;
        //RouterBack = new CryRouteManager(_sourceBuild, _person.Work, _person, HPers.InWorkBack);
    }

    public override void Update()
    {
        if (_takeABreakNow)
        {
            TakeABreak();
            return;
        }

        if (!Router1.IsRouteReady)
        {
            Router1.Update();
        }

        DockerStates();
    }

    void CheckIfCanPickUoNewOrder()
    {
        if (Order1 == null || Order1.IsCompleted)
        {
            PickUpOrder();
        }
    }


    void DockerStates()
    {
        //at dock at first
        if (_person.Body.Location == HPers.Work && _person.Body.GoingTo != HPers.DockerSupply)
        {
            CheckIfCanPickUoNewOrder();

            ImportIfPossible();
            _person.Body.WalkRoutine(Router1.TheRoute, HPers.DockerSupply);
        }
        //at food source at first
        else if (_person.Body.Location == HPers.DockerSupply && _person.Body.GoingTo != HPers.DockerBackToDock)
        {
            DropAllMyGoods(_person.FoodSource);//so drop imports if any

            CheckIfCanPickUoNewOrder();

            ExportIfPossible();
            _person.Body.WalkRoutine(Router1.TheRoute, HPers.DockerBackToDock, true);
        }
        //back at dock
        else if (_person.Body.Location == HPers.DockerBackToDock && _person.Body.GoingTo != HPers.FoodSource)
        {
            DropAllMyGoods(_person.Work);//so drops exports if any 
            _person.Body.WalkRoutine(Router1.TheRoute, HPers.FoodSource);

            _person.Body.UpdatePersonalForWheelBa();
        }
    }

    private void ExportIfPossible()
    {
        Execute();
        if (_export)
        {
            HandleInventoriesAndOrder();
        }
    }

    private void ImportIfPossible()
    {
        Execute();
        if (_import)
        {
            HandleInventoriesAndOrder();
        }
    }

    bool _export;
    bool _import;
    void Execute()
    {
        _export = _order != null && _order.SourceBuild != "Ship";

        _import = _order != null && _order.SourceBuild == "Ship";
    }

    void HandleInventoriesAndOrder()
    {
        _sourceBuild = GetStructureSrcAndDestinyExpImp();

        if (_sourceBuild == null)
        {
            return;
        }

        if (_sourceBuild.HasEnoughToCoverOrder(Order1))
        {
            var amt = Order1.ApproveThisAmt(_person.HowMuchICanCarry(Order1.Amount));
            Order1.AddToFullFilled(amt);

            Debug.Log(_person.MyId + " Docker got from:" + Order1.SourceBuild + " : " + Order1.Product + ".amt:" + amt);

            _person.ExchangeInvetoryItem(_sourceBuild, _person, Order1.Product, amt);
            _sourceBuild.CheckIfCanBeDestroyNow(Order1.Product);
            _person.Body.UpdatePersonalForWheelBa();
        }
    }


    MDate _lastAct;
    string _act;
    bool NewAct(string act)
    {
        if (_lastAct != null && Program.gameScene.GameTime1.ElapsedDateInDaysToDate(_lastAct) < 15)
        {
            return false;
        }
        _lastAct = Program.gameScene.GameTime1.CurrentDate();
        _act = act;
        return true;
    }



    private bool _takeABreakNow;
    private float _breakDuration = 1f;
    private float startIdleTime;
    /// <summary>
    /// Used so a person is asking for bridges anchors takes a break and let brdige anchors complete then can 
    /// work on it
    /// </summary>
    void TakeABreak()
    {
        if (startIdleTime == 0)
        { startIdleTime = Time.time; }

        if (Time.time > startIdleTime + _breakDuration)
        {
            _takeABreakNow = false;
            startIdleTime = 0;
            Init();
        }
    }



    Structure GetStructureSrcAndDestinyExpImp()
    {
        if (_export)
        {
            return (Structure)_person.FoodSource;
        }
        if (_import)
        {
            return _person.Work;
        }
        return null;
    }
}
