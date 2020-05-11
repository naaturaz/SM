using System.Collections.Generic;

//cannot remove the start() or update() of a class that has clones on scene.
//Gives strange results
public class PersonSaveLoad : PersonPot
{
    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
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

    /// <summary>
    /// Needed for When GUI is reloaded
    /// </summary>
    /// <returns></returns>
    public PersonData TempSave()
    {
        return new PersonData(GetAllPerson(), GetAllFromPersonController());
    }

    private List<PersonFile> GetAllPerson()
    {
        List<PersonFile> res = new List<PersonFile>();
        for (int i = 0; i < Control.All.Count; i++)
        {
            res.Add(new PersonFile(Control.All[i]));
        }
        return res;
    }

    /// <summary>
    /// Gathering data to savePerson Controller
    /// </summary>
    /// <returns></returns>
    private PersonControllerSaveLoad GetAllFromPersonController()
    {
        PersonControllerSaveLoad res = new PersonControllerSaveLoad();

        res.Difficulty = Control.Difficulty;
        res.UnivCounter = PersonController.UnivCounter;
        res.Queues = Control.Queues;
        res.GenderLast = PersonController.GenderLast;
        res.Locked = Control.Locked;

        res.BuildersManager = PersonPot.Control.BuildersManager1;

        Control.RoutesCache1.CreateSave();
        res.RoutesCache = Control.RoutesCache1;

        //res.OnSystemNow1 = Control.OnSystemNow1;
        res.EmigrateController1 = Control.EmigrateController1;
        res.IsAPersonHomeLessNow = Control.IsAPersonHomeLessNow;

        //used in DataController.cs.SetLoadedTerrainInTerraRoot()
        res.TerrainName = Program.gameScene.Terreno.name;
        //used in BuyRegionManager.cs
        res.UnlockRegions = MeshController.BuyRegionManager1.UnlockRegions;

        res.TownName = Program.MyScreen1.TownName;
        res.SubBulletinProduction = BulletinWindow.SubBulletinProduction1;
        res.SubBulletinFinance = BulletinWindow.SubBulletinFinance1;

        res.IsPirate = Program.IsPirate;
        res.IsFood = Program.IsFood;
        res.WasTutoPassed = Program.WasTutoPassed;

        res.QuestManager = Program.gameScene.QuestManager;

        return res;
    }
}