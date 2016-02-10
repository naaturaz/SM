
using System.Xml;

public class Insider : Profession {

    public Insider(Person person, PersonFile pF)
    {
        if (pF == null)
        {
            CreatingNew(person);
        }
        else LoadingFromFile(person, pF);
    }

    void CreatingNew(Person person)
    {
        ProfDescription = Job.Insider;
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
        //  to adress the problem when creating a new profession and builder spawned
        // Dummy somewhere, and the Dummy doesnt get destroyed 
        ResetDummy();

        FinRoutePoint = _person.Work.BehindMainDoorPoint;
        ReadyToWork = true;
    }

    public override void Update()
    {
        base.Update();
        Execute();
    }

    public override void WorkAction(HPers p)
    {
        _person.Brain.CurrentTask = p;
        ExecuteNow = true;
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
            //do stuff

            base.Execute();


            ////GameScene.print("excuting on Insider ");
        }
    }
}
