using System;
using UnityEngine;

public class Docker : Profession
{
    Structure _source;

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
        _source = GiveMeSourceForThisProd();

        RouterActive = true;
        Router1 = new CryRouteManager(_person.Work, _source, _person, HPers.InWork);
    }

    Structure GiveMeSourceForThisProd()
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
            //_person.Body.WalkRoutine(Router1.TheRoute, HPers.FoodSource);

            //_person.Body.UpdatePersonalForWheelBa();

            //so homer works
            _person.Body.Location = HPers.Work;
            _workerTask = HPers.None;
            _person.CreateProfession(Job.Homer);
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
            //need to pull left from Dispatch bz Order1 is passed by Value not Ref 
            var left = WhatIsLeft();
            var amt = Order1.ApproveThisAmt(left);

            //if (_export)//if import tht amt was added already to processed amounts 
            //{
                _person.Work.Dispatch1.AddToOrderAmtProcessed(Order1, amt);
            //}

            Debug.Log(_person.MyId + " Docker got from:" + Order1.SourceBuild + " : " + Order1.Product + ".amt:" + amt);

            _person.ExchangeInvetoryItem(_sourceBuild, _person, Order1.Product, amt);
            _sourceBuild.CheckIfCanBeDestroyNow(Order1.Product);
            _person.Body.UpdatePersonalForWheelBa();
        }
    }


    float WhatIsLeft()
    {
        if (_import)
        {
            //bz is gets completed b4 hits this and was checked already for the amt
            return Order1.Amount;
        }
        return _person.Work.Dispatch1.LeftOnThisOrder(Order1);
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
    private float _breakDuration = 10f;
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
            return _source;
        }
        if (_import)
        {
            return _person.Work;
        }
        return null;
    }
}
