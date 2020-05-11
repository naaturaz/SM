public class Emigrate
{
    private MDate _deathDate;
    private string _myID;

    public Emigrate()
    {
    }

    public Emigrate(Person person)
    {
        _myID = person.MyId;
        DetermineDeathDate(person.LifeLimit - person.Age);
    }

    public string MyId
    {
        get { return _myID; }
        set { _myID = value; }
    }

    public MDate DeathDate
    {
        get { return _deathDate; }
        set { _deathDate = value; }
    }

    public void DetermineDeathDate(int yearsToAddToToday)
    {
        _deathDate = Program.gameScene.GameTime1.ReturnCurrentDatePlsAdded(yearsToAddToToday * 360);
    }

    /// <summary>
    /// Year check that will find if person pass away or is adding POrtReputation
    /// </summary>
    internal void YearCheck()
    {
        if (GameTime.IsPastOrNow(_deathDate))
        {
            PersonPot.Control.EmigrateController1.RemoveEmigrate(this);
        }
        else
        {
            //add to PortReputation
            BuildingPot.Control.DockManager1.AddSurvey(0.01f);
        }
    }
}