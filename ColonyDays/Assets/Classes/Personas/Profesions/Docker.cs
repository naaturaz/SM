﻿using System;
using UnityEngine;

public class Docker : Profession
{
    private Structure _source;

    public Docker(Person person, PersonFile pF)
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

    private void Init()
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

    private void SetSourceAndDestinyBuild()
    {
        //if(!_import && !_export)
        //Execute();

        _destinyBuild = GetStructureSrcAndDestinyExpImp();
        _sourceBuild = GetStructureSrcAndDestinyExpImp();

        DestinyBuildKey = Order1.DestinyBuild;
        SourceBuildKey = Order1.SourceBuild;
    }

    private void InitRoute()
    {
        _source = GiveMeSourceForThisProd();

        RouterActive = true;
        Router1 = new CryRouteManager(_person.Work, _source, _person, HPers.InWork);
    }

    private Structure GiveMeSourceForThisProd()
    {
        if (_order == null)
        {
            return _person.FoodSource;
        }

        //looking for a Source that has the product
        var sourceSt = Dispatch.FindFoodSrcWithProd(_person, _order.Product);
        if (string.IsNullOrEmpty(sourceSt))
        {
            return _person.FoodSource;
        }

        var source = Brain.GetStructureFromKey(sourceSt);
        //if not will default for the person FoodSource
        if (source == null)
        {
            source = _person.FoodSource;
        }

        return source;
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

    private void CheckIfCanPickUpNewOrder()
    {
        if (UPerson.IsThisPersonTheSelectedOne(_person))
        {
            var a = 1;
        }

        if (Order1 == null || Order1.IsCompleted)
        {
            PickUpOrder();
        }
    }

    private void DockerStates()
    {
        //at dock at first
        if (_person.Body.Location == HPers.Work && _person.Body.GoingTo != HPers.DockerSupply)
        {
            //CheckIfCanPickUpNewOrder();
            GetMeOrderIfAny(H.Import);
            ImportIfPossible();

            if (PersonPot.Control.RoutesCache1.ContainANewerOrSameRoute(_person.Work.MyId, _person.FoodSource.MyId,
           new DateTime()) || Router1.TheRoute.CheckPoints.Count == 0)
            {
                Router1.TheRoute = PersonPot.Control.RoutesCache1.GiveMeTheNewerRoute();
                Router1.IsRouteReady = true;
            }

            _person.Body.WalkRoutine(Router1.TheRoute, HPers.DockerSupply);
        }
        //at food source at first
        else if (_person.Body.Location == HPers.DockerSupply && _person.Body.GoingTo != HPers.DockerBackToDock)
        {
            DropAllMyGoods(_person.FoodSource);//so drop imports if any

            //CheckIfCanPickUpNewOrder();
            GetMeOrderIfAny(H.Export);

            ExportIfPossible();
            _person.Body.WalkRoutine(Router1.TheRoute, HPers.DockerBackToDock, true);
        }
        //back at dock
        else if (_person.Body.Location == HPers.DockerBackToDock && _person.Body.GoingTo != HPers.FoodSource)
        {
            DropAllMyGoods(_person.Work);//so drops exports if any

            //Then Homer Will handle the drop of those goods in the Storage
            GetMeOrderIfAny(H.Import);
            ImportIfPossible();

            //so homer works
            _person.Body.Location = HPers.Work;
            _workerTask = HPers.None;
            _person.CreateProfession(Job.Homer);
        }
    }

    private void GetMeOrderIfAny(H type)
    {
        if (UPerson.IsThisPersonTheSelectedOne(_person))
        {
            var a = 1;
        }

        if (_person.Work == null || _person.Work.HType != H.Dock)
            return;

        var ord = _person.Work.Dispatch1.GiveMeOrderIfAny(_person, type);
        if (ord != null)
        {
            Order1 = ord;
            _person.PrevOrder = Order1;
        }
    }

    private void ExportIfPossible()
    {
        if (UPerson.IsThisPersonTheSelectedOne(_person))
        {
            var a = 1;
        }

        Execute();
        if (_export)
        {
            HandleInventoriesAndOrder();
        }
    }

    private void ImportIfPossible()
    {
        if (UPerson.IsThisPersonTheSelectedOne(_person))
        {
            var a = 1;
        }

        Execute();
        if (_import)
        {
            HandleInventoriesAndOrder();
        }
    }

    private bool _export;
    private bool _import;

    private void Execute()
    {
        if (UPerson.IsThisPersonTheSelectedOne(_person))
        {
            var a = 1;
        }

        _export = _order != null && _order.SourceBuildInfo != "Ship";
        _import = _order != null && _order.SourceBuildInfo == "Ship";
    }

    private void HandleInventoriesAndOrder()
    {
        _sourceBuild = GetStructureSrcAndDestinyExpImp();
        if (_sourceBuild == null)
            return;

        //need to pull left from Dispatch bz Order1 is passed by Value not Ref
        var left = WhatIsLeft();
        var carryWeight = _person.HowMuchICanCarry() < left ? _person.HowMuchICanCarry() : left;
        var amt = Order1.ApproveThisAmt(carryWeight);

        _person.ExchangeInvetoryItem(_sourceBuild, _person, Order1.Product, amt, _sourceBuild);
        //will add to processed order only if actually took something...

        if (_person.Inventory.ReturnAmtOfItemOnInv(Order1.Product) > 0 && _export)
        {
            var amtTaken = _person.Inventory.ReturnAmtOfItemOnInv(Order1.Product);
            //and will report actually only was he physically took from it
            _person.Work.Dispatch1.AddToOrderAmtProcessed(Order1, amtTaken);
            //Debug.Log(_person.MyId + " Docker got from:" + Order1.SourceBuild + " : " + Order1.Product + ".amt:" + amt);
        }
        else if (_import)
        {
            _person.Work.Dispatch1.AddToOrderAmtProcessed(Order1, amt);
        }

        _sourceBuild.CheckIfCanBeDestroyNow(Order1.Product);
        _person.Body.UpdatePersonalForWheelBa();
    }

    private float WhatIsLeft()
    {
        if (_import)
        {
            //bz is gets completed b4 hits this and was checked already for the amt
            return Order1.Amount;
        }
        return _person.Work.Dispatch1.LeftOnThisOrder(Order1);
    }

    private MDate _lastAct;
    private string _act;

    private bool NewAct(string act)
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
    private float _breakDuration = 10f;
    private float startIdleTime;

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
            Init();
        }
    }

    private Structure GetStructureSrcAndDestinyExpImp()
    {
        if (_export)
        {
            return _source;
        }
        if (_import)
        {
            return _person.Work;
        }
        return null;
    }
}