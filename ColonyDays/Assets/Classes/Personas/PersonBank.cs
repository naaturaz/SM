public class PersonBank
{
    private Person _person;
    private float _checkingAcct;

    public float CheckingAcct
    {
        get { return _checkingAcct; }
        set { _checkingAcct = value; }
    }

    public PersonBank()
    {
    }

    internal void SetPerson(Person person)
    {
        _person = person;
    }

    public void Add(float amt, string acct = "Check")
    {
        if (acct == "Check")
        {
            _checkingAcct += amt;
        }
    }

    public void WithDraw(float amt, string acct = "Check")
    {
        if (acct == "Check")
        {
            _checkingAcct -= amt;
        }
    }
}