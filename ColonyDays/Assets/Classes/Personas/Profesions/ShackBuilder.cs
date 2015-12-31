using System;
using UnityEngine;

public class ShackBuilder : Profession
{
    public ShackBuilder(Person person, PersonFile pF)
    {
        if (pF == null)
        {
            CreatingNew(person);
        }
        else LoadingFromFile(person, pF);
    }

    void CreatingNew(Person person)
    {
        ProfDescription = Job.ShackBuilder;
        MyAnimation = "isHammer";
        _person = person;
        //CreateShack();
        BookShack();

        Init();
    }

    void LoadingFromFile(Person person, PersonFile pF)
    {
        _person = person;
        LoadAttributes(pF.ProfessionProp);
        
        //needs to redo the Family on _constructing
        var oldBuild = Brain.GetBuildingFromKey(_person.Brain.MoveToNewHome.OldHomeKey);
        var myFamily = oldBuild.FindMyFamily(_person);

        var newFam = new Family(3, _constructing.MyId);
        newFam.FamilyId = person.FamilyId;
        _constructing.Families = new Family[]{newFam};
        //_constructing.Families[0] = myFamily;

      
        //Realtor.BookMyFamilyToNewBuild(_person, _constructing, myFamily);
    }

    private void Init()
    {
        //when get a number here is defined by wht worker is this on the building 
        //workers will be numbered on buildingsB
        FinRoutePoint = _constructing.transform.position;

        //UVisHelp.CreateHelpers(FinRoutePoint, Root.blueCubeBig);
        InitRoute();
    }

    void InitRoute()
    {
        _routerActive = true;

        dummy = (Structure)Building.CreateBuild(Root.dummyBuildWithSpawnPoint, new Vector3(), H.Dummy);
        dummy.transform.position = _person.transform.position;
        dummy.HandleLandZoning();

        Router1 = new CryRouteManager(dummy, (Structure)_constructing, _person, HPers.InWork, false, true);

        //Debug.Log("shck created dummy ");
    }

    //private void CreateShack()
    //{
    //    _constructing = (Structure)Building.CreateBuild(Root.RetBuildingRoot(H.Shack), _person.transform.position, 
    //        H.Shack, materialKey: H.Shack.ToString() + "." + Ma.matBuildBase);

    //    ConstructingKey = _constructing.MyId;

    //    RotateShack();

    //    AssignRandomIniPosition(_constructing.transform.position, _constructing);
    //    FixBuildingToGround();


    //    StructureParent sp = (StructureParent) _constructing;
    //    sp.ResetedSpawnPoint();

    //    _constructing.HandleZoningAddCrystals();
    //}

    void RotateShack()
    {
        //since i change 'UnivRotationFacer' when I rotate I keep it here so at the end will return back to its first value
        //in this way doestn affect the Building we have spawened rotation
        var RotUni = General.UnivRotationFacer;

        //so I hold the value of it and its not lost .. I will assign it again a few lines later so if
        //i had spawned a building doesnt suffer 
        var temp = BuildingPot.Control.CurrentSpawnBuild;
        BuildingPot.Control.CurrentSpawnBuild = _constructing;

        //random rotation
        UnityEngine.Random rand = new UnityEngine.Random();
        int thisRand = UnityEngine.Random.Range(0, 3);

        for (int i = 0; i < thisRand; i++)
        {
            BuildingPot.Control.CurrentSpawnBuild.RotationAction();
        }
        //asigning back the real CurrentSpawnBuild so user can keep working with it 
        BuildingPot.Control.CurrentSpawnBuild = temp;

        //assign its initial value 
        General.UnivRotationFacer = RotUni;
    }

    /// <summary>
    /// If person has family will book Shack
    /// </summary>
    void BookShack()
    {
        //this is for new Adults 
        if (string.IsNullOrEmpty(_person.Brain.MoveToNewHome.OldHomeKey))
        {
            return;
        }

        Family myFamily = null;
        var oldHome = Brain.GetStructureFromKey(_person.Brain.MoveToNewHome.OldHomeKey);
        myFamily = oldHome.FindMyFamily(_person);

        //is bz im a recently 16 years guy or moved from empty house 
        if (myFamily == null)
        {
            myFamily = CreateNewFamily(myFamily);
        }
        
        Realtor.BookMyFamilyToNewBuild(_person, _constructing, myFamily);

    }

    Family CreateNewFamily(Family myFamily = null)
    {
        myFamily = new Family(3, _constructing.MyId);

        if (string.IsNullOrEmpty(_person.FamilyId))
        {
            _person.FamilyId = "Family:" + _person.MyId;
        }
        myFamily.FamilyId = _person.FamilyId;
        return myFamily;
    }

    private int counter;
    /// <summary>
    /// Returns Random position from origin. If fell inside a building will find another spot
    /// until is in a clear zone
    /// 
    /// </summary>
    /// <param name="howFar">How far will go</param>
    public void AssignRandomIniPosition(Vector3 origin, Building building, float howFar = 8)
    {
        float x = UMath.Random(-howFar, howFar);
        float z = UMath.Random(-howFar, howFar);
        origin = new Vector3(origin.x + x, origin.y, origin.z + z);

        building.transform.position = origin;

        if (!IsBuildingGood(building) || !_person.IsOnTerrain(origin))
        {
            counter++;
            CheckIfCanRecurseAgain(origin, building, howFar);
        }
    }

    /// <summary>
    /// Created to address if recursion happens more than 100 times 
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="building"></param>
    /// <param name="howFar"></param>
    void CheckIfCanRecurseAgain(Vector3 origin, Building building, float howFar)
    {
        if (counter < 100)
        {
            //this is to avoid infinite loop. will keep extending how far will go from original pos
            howFar *= 1.2f;
            AssignRandomIniPosition(origin, building, howFar);
        }
        else
        {
            GameScene.print("ShackBilder cant find place to establish shack. will Emigrate");
            _person.Emmigrate();
        }
    }

    private void Emigrate()
    {
        throw new NotImplementedException();
    }

    bool IsBuildingGood(Building building)
    {
        building.UpdateBuildExternally();

        if (!building.IsBuildOk)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// Creates the the new building, category: structure
    /// </summary>
    void FixBuildingToGround()
    {
        //since i change 'UnivRotationFacer' when I rotate I keep it here so at the end will return back to its first value
        //in this way doestn affect the Building we have spawened rotation
        var RotUni = General.UnivRotationFacer;

        _constructing.PositionFixed = true;

        //so I hold the value of it and its not lost .. I will assign it again a few lines later so if
        //i had spawned a building doesnt suffer 
        var temp = BuildingPot.Control.CurrentSpawnBuild;

        //needed bz when I call FixBuildingToGround(); will save that CurrentSpawnBuild in Registro
        BuildingPot.Control.CurrentSpawnBuild = _constructing;
        _constructing.DonePlace();

        BuildingPot.Control.CurrentSpawnBuild = temp;
        
        //called here bz when trying to added the anchors to Queues so Router can see new shacks on way
        //the shacks are not added to the All in registro ,
        //so here I added to there so the router detects them 
        //_constructing.UpdateOnBuildControl(H.Add);

        General.UnivRotationFacer = RotUni;
    }

    public override void Update()
    {
        base.Update();
        Execute();
        DoneWorkNow();

        //Created to address the situation that ShackBuilder has to do all on his own
        InitForceAction();
    }

    private bool workActionCalled;
    /// <summary>
    /// Created to address the situation that ShackBuilder has to do all on his own. 
    /// Brain dosnt call him to start work
    /// </summary>
    private void InitForceAction()
    {
        if (!workActionCalled && ReadyToWork)
        {
            _person.Work = (Structure)_constructing;
            _person.Body.Location = HPers.Work;

            WorkAction(HPers.None);
            workActionCalled = true;
        }
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
            _constructing.AddToConstruction(ProdXShift);
        }
    }

    /// <summary>
    /// Action to take when complete his work all
    /// </summary>
    private void DoneWorkNow()
    {
        if (base.DoneWorkNow)
        {
            base.DoneWorkNow = false;

            //to avoid ShackBuilder infinite Loop on this method once was finished 
            _workerTask = HPers.None;
           
//            GameScene.print("Done work on ShackBulider:" + _person.MyId);
            _person.Brain.ShackBuilderDone(_constructing);
        }
    }
}
