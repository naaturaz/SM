public class PersonPot : Pot
{
    private static PersonController _control;
    private static PersonSaveLoad _saveLoad;

    //game start brand new  dificulty
    private int _gameDifficulty = 0;

    public static PersonController Control
    {
        get { return _control; }
        set { _control = value; }
    }

    public static PersonSaveLoad SaveLoad
    {
        get { return _saveLoad; }
        set { _saveLoad = value; }
    }
////////////////////
    public int IniAmount
    {
        get { return _gameDifficulty; }
        set { _gameDifficulty = value; }
    }

    // Use this for initialization
	void Start ()
	{
	    _control = PersonController.CreatePersonController(Root.personController, 0,
	        Program.ClassContainer.transform);
	    _saveLoad = (PersonSaveLoad) Create(Root.personSaveLoad, container: Program.ClassContainer.transform);


	}
	
    //// Update is called once per frame
    //void Update()
    //{
    //   // PersonController.Update();
    //}
}
