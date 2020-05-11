using UnityEngine;

public class WheelBarrow : Profession
{
    private Structure _destinyBuild;
    private Structure _sourceBuild;

    public WheelBarrow(Person person, PersonFile pF)
    {
        if (pF == null)
        {
            _person = person;
            Init();
        }
        else LoadingFromFile(person, pF);
    }

    private void LoadingFromFile(Person person, PersonFile pF)
    {
        _wasLoaded = true;

        _person = person;
        LoadAttributes(pF.ProfessionProp);
        InitForLoading();
    }

    /// <summary>
    /// This Init is for Loading since the other was ReWriting loaded Values such as Order1
    /// </summary>
    private void InitForLoading()
    {
        if (UPerson.IsThisPersonTheSelectedOne(_person))
        {
            var a = 1;
        }

        //if did not load a order will return, and take a break now
        if (Order1 == null// || _destinyBuild == null
            )
        {
            _takeABreakNow = true;
            return;
        }

        _destinyBuild = Brain.GetStructureFromKey(Order1.DestinyBuild);
        _sourceBuild = Brain.GetStructureFromKey(Order1.SourceBuild);
        _person.PrevOrder = Order1;

        InitRoute();
    }

    private void Init()
    {
        //so loads
        if (_wasLoaded)
        {
            InitForLoading();

            //watch NEW
            _wasLoaded = false;
            return;
        }

        //so its not using the same order over and over again in case the Dispatch is finding nothing
        CleanOldVars();
        //Debug.Log(_person.MyId+" Init WheelB");

        HandleNewProfDescrpSavedAndPrevJob(Job.WheelBarrow);

        MyAnimation = "isWheelBarrow";

        if (_person.Work != null &&
            (_person.Work.HType == H.HeavyLoad))
        {
            MyAnimation = "isCartRide";
        }

        //if did not fouind a order will return, and take a break now
        if (!DidPickUpOrder())
        {
            _takeABreakNow = true;
            return;
        }

        //means no Orders avail
        if (_destinyBuild == null)
        {
            _takeABreakNow = true;
            return;
        }

        InitRoute();
    }

    private void CleanOldVars()
    {
        _sourceBuild = null;//from where taking the load
        _destinyBuild = null;//where taking load
        Order1 = null;
    }

    private bool DidPickUpOrder()
    {
        if (_person == null)
            return false;
        if (_person.Work == null)
            return false;
        if (_person.Work.Dispatch1 == null)
            return false;

        Order1 = _person.Work.Dispatch1.GiveMeOrder(_person);

        _person.PrevOrder = Order1;

        if (Order1 == null)
            return false;

        _destinyBuild = Brain.GetStructureFromKey(Order1.DestinyBuild);
        _sourceBuild = Brain.GetStructureFromKey(Order1.SourceBuild);

        DestinyBuildKey = Order1.DestinyBuild;
        SourceBuildKey = Order1.SourceBuild;

        return true;
    }

    private void InitRoute()
    {
        if (_sourceBuild == null)
        {
            _sourceBuild = Brain.GetStructureFromKey(Order1.SourceBuild);
        }
        //if still null after this
        if (_sourceBuild == null)
        {
            //Debug.Log("srcBuild null whelbarr:" + _person.MyId + ".orderSrcBld:" + Order1.SourceBuild);
            _takeABreakNow = true;
            return;
        }

        RouterActive = true;
        Router1 = new CryRouteManager(_person.Work, _sourceBuild, _person, HPers.InWork);

        IsRouterBackUsed = true;
        RouterBack = new CryRouteManager(_sourceBuild, _destinyBuild, _person, HPers.InWorkBack);
    }

    public override void Update()
    {
        base.Update();

        if (_takeABreakNow)
        {
            TakeABreak();
            return;
        }

        Execute();
    }

    private void Execute()
    {
        if (ExecuteNow)
        {
            ExecuteNow = false;

            //Debug.Log(_person.MyId+ " Wheel Barr got from:"+Order1.SourceBuild +
            //" : " +Order1.Product+".amt:"+Order1.Amount);

            Order1.Amount = _person.HowMuchICanCarry();
            if (_person.Work.HType == H.HeavyLoad)
            {
                Order1.Amount = Order1.Amount < WhatIsLeft() ? Order1.Amount : WhatIsLeft();
            }

            _person.ExchangeInvetoryItem(_sourceBuild, _person, Order1.Product, Order1.Amount, _sourceBuild);

            //bz is putting car anim before has inventory loaded
            _person.Body.ResetPersonalObjectForHeavyLoader();

            _sourceBuild.CheckIfCanBeDestroyNow(Order1.Product);

            if (_person.Work.HType == H.HeavyLoad)
            {
                //update Order in Dock if is a dock
                if (import() || export())
                {
                    HandleInventoriesAndOrder();
                }
                Debug.Log("Order1.SourceBuild:" + Order1.SourceBuild);
                return;
            }

            _person.Body.UpdatePersonalForWheelBa();
        }
    }

    #region Heavy Loaders

    //import: Order1.SourceBuild.Contains("Dock")
    private bool import()
    {
        return Order1.SourceBuild.Contains("Dock");
    }

    //export: Order1.DestinyBuild.Contains("Dock")
    private bool export()
    {
        return Order1.DestinyBuild.Contains("Dock");
    }

    private Building Dock()
    {
        if (import())
            return Brain.GetBuildingFromKey(Order1.SourceBuild);
        if (export())
            return Brain.GetBuildingFromKey(Order1.DestinyBuild);
        return null;
    }

    private float WhatIsLeft()
    {
        if (import())
        {
            //bz is gets completed b4 hits this and was checked already for the amt
            return Order1.Amount;
        }
        if(Dock() != null)
            return Dock().Dispatch1.LeftOnThisOrder(Order1);
        return Order1.Left();
    }

    private void HandleInventoriesAndOrder()
    {
        //need to pull left from Dispatch bz Order1 is passed by Value not Ref
        var left = WhatIsLeft();
        var amt = Order1.ApproveThisAmt(left);

        if (_person.Inventory.ReturnAmtOfItemOnInv(Order1.Product) > 0 && export())
        {
            var amtTaken = _person.Inventory.ReturnAmtOfItemOnInv(Order1.Product);
            //and will report actually only was he physically took from it
            Dock().Dispatch1.AddToOrderAmtProcessed(Order1, amtTaken);
        }
        else if (import())
        {
            Dock().Dispatch1.AddToOrderAmtProcessed(Order1, amt);
        }
        Dock().Dispatch1.CleanOrdersIfNeeded();
    }

    #endregion Heavy Loaders

    private bool _takeABreakNow;
    private float _breakDuration = 1f;
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

            DecideOnNextIteration();
        }
    }

    private void DecideOnNextIteration()
    {
        if (_person == null)
        {
            _takeABreakNow = true;
            return;
        }

        if (Homer.CheckIfCanBeWheelBar(_person))
        {
            //so it restarted
            Init();
        }
        else
        {
            _person.CreateProfession(Job.Builder);
        }
    }
}