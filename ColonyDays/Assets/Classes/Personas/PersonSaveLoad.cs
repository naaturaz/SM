using System.Collections.Generic;

//cannot remove the start() or update() of a class that has clones on scene.
//Gives strange results
public class PersonSaveLoad : PersonPot {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	   
	}

    /// <summary>
    /// Save
    /// 
    /// The load is on : PersonControlller.LoadFromFile
    /// </summary>
    public void Save()
    {
        PersonData p = new PersonData(GetAllPerson(), GetAllFromPersonController());
        XMLSerie.WriteXMLPerson(p);
    }

    List<PersonFile> GetAllPerson()
    {
        List<PersonFile> res = new List<PersonFile>();
        for (int i = 0; i < Control.All.Count; i++)
        {
            res.Add(new PersonFile(Control.All[i]));
        }
        return res;
    }

    //saving
    PersonControllerSaveLoad GetAllFromPersonController()
    {
        PersonControllerSaveLoad res = new PersonControllerSaveLoad();

        res.Difficulty = Control.Difficulty;
        res.UnivCounter = PersonController.UnivCounter;
        res.Queues = Control.Queues;
        res.GenderLast = PersonController.GenderLast;
        res.Locked = Control.Locked;

        res.BuildersManager = PersonPot.Control.BuildersManager1;
        res.RoutesCache = Control.RoutesCache1;

        res.Waiting = Control.Waiting;
        res.OnSystemNow1 = Control.OnSystemNow1;
        res.IsAPersonHomeLessNow = Control.IsAPersonHomeLessNow;

        return res;
    }
}
