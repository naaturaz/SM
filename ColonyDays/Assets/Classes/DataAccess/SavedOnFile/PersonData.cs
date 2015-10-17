using System.Collections.Generic;

public class PersonData  {

    public  PersonControllerSaveLoad PersonControllerSaveLoad = new PersonControllerSaveLoad();

    private List<PersonFile> _all = new List<PersonFile>();

    public List<PersonFile> All
    {
        get { return _all; }
        set { _all = value; }
    }

    public PersonData(List<PersonFile> all, PersonControllerSaveLoad personControllerSaveLoad)
    {
        _all = all;
        PersonControllerSaveLoad = personControllerSaveLoad;
    }

    public PersonData() { }
}
