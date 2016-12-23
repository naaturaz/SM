public class PersonPot : Pot
{
    private static PersonController _control;
    private static PersonSaveLoad _saveLoad;



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


    // Use this for initialization
	void Start ()
	{
	    _control = PersonController.CreatePersonController(Root.personController,
            Program.MyScreen1.HoldDifficulty,
	        Program.ClassContainer.transform);
	    _saveLoad = (PersonSaveLoad) Create(Root.personSaveLoad, container: Program.ClassContainer.transform);


	}
	
    //// Update is called once per frame
    //void Update()
    //{
    //   // PersonController.Update();
    //}
}
