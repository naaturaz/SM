﻿public class Insider : Profession
{
    public Insider(Person person, PersonFile pF)
    {
        if (pF == null)
        {
            CreatingNew(person);
        }
        else LoadingFromFile(person, pF);
    }

    private void CreatingNew(Person person)
    {
        MyAnimation = "isSummon";
        _person = person;

        HandleNewProfDescrpSavedAndPrevJob(Job.Insider);

        Init();
    }

    private void LoadingFromFile(Person person, PersonFile pF)
    {
        _person = person;
        LoadAttributes(pF.ProfessionProp);
    }

    private void Init()
    {
        if (ShouldITakeBreakInit())
        {
            return;
        }

        //in case was a Wheelbarrow the prevProfession and when home route back gives problem
        _person.PrevOrder = null;

        //  to adress the problem when creating a new profession and builder spawned
        // Dummy somewhere, and the Dummy doesnt get destroyed
        ResetDummy();

        FinRoutePoint = _person.Work.BehindMainDoorPoint;
        FakeRouter1ForNewProfThatUseHomer();
        RouteBackForNewProfThatUseHomer();
    }

    public override void WorkAction(HPers p)
    {
        //        Debug.Log("WorkAction called insider:" + _person.MyId);

        _person.Brain.CurrentTask = p;
        ExecuteNow = true;
    }

    /// <summary>
    /// The specific action of a Proffession
    /// Ex: Forester add lumber to its inventory and removed the amt from tree invetory
    /// </summary>
    private void Execute()
    {
        //need to check if is Ready To work bz might be routing still
        //and if execute once wont do it again
        if (ExecuteNow && ReadyToWork)
        {
            ExecuteNow = false;
            //do stuff

            FakeWheelBarrowToRouteBack();
            base.Execute();
        }
        else if (ExecuteNow && !ReadyToWork)
        {
            //so we leave it for next time to see if is ready
            //othwrwise gives buggg bz wherever is will do stuff as is will be inside the work place
            ExecuteNow = false;
        }
    }

    public override void Update()
    {
        if (_reInitNow)
        {
            _reInitNow = false;
            Init();
            return;
        }

        base.Update();

        if (_breakInitNow)
        {
            return;
        }

        Execute();
    }
}