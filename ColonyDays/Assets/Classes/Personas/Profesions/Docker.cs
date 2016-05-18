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
        _destinyBuild = GetStructureSrcAndDestiny(Order1.DestinyBuild, _person);
        _sourceBuild = GetStructureSrcAndDestiny(Order1.SourceBuild, _person);

        DestinyBuildKey = Order1.DestinyBuild;
        SourceBuildKey = Order1.SourceBuild;        
    }

    void InitRoute()
    {
        RouterActive = true;
        Router1 = new CryRouteManager(_person.Work, _sourceBuild, _person , HPers.InWork);

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

    void Execute()
    {
        if (ExecuteNow)
        {
            ExecuteNow = false;

            if (_sourceBuild==null)
            {
                _sourceBuild = GetStructureSrcAndDestiny(SourceBuildKey, _person);
            }

            if (_sourceBuild.HasEnoughToCoverOrder(Order1))
            {
               //Debug.Log(_person.MyId + " Docker got from:" + Order1.SourceBuild +" : " + Order1.Product + ".amt:" + Order1.Amount);

                Order1.Amount = _person.HowMuchICanCarry();
                _person.ExchangeInvetoryItem(_sourceBuild, _person, Order1.Product, Order1.Amount );
                
                
                _sourceBuild.CheckIfCanBeDestroyNow(Order1.Product);
                _person.Body.UpdatePersonalForWheelBa();



                //will remove the import order(evacuation) from diispatch if is completed already
                _person.Work.Dispatch1.RemoveImportOrder(_person.Work, Order1);
            }
        }
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
}
