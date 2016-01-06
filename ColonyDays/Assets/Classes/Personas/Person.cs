using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Person : General
{
    //Debug
    private List<General> _debugList = new List<General>();

    private int _age;

    private bool _isMajor; //says if a person is major age 

    //Birthday Related
    private int _lastBDYear; //the year I had the last birthday. So i dont have more thn 1 BD a Year 
    private int _birthMonth; //the birthday of the person
    private int _unHappyYears; //if is over X amount will emmigrate

    private int _lifeLimit;
    private string _name;
    private EducationLevel _educationLevel;
    private double _happinnes = 5, _prosperity = 5;
    private H _gender;

    //only for females
    private bool _isPregnant;
    private int _lastNewBornYear = -10; //the last pregnancy she had, when was the birth year 
    //once pregnant will tell u the due date 
    private int _dueMonth;
    private int _dueYear;

    //how well feed is a person. 100 is max. 
    //if person eat adds to this. This will be removed by one every : checkFoodElapsed
    private float _nutritionLevel = 50;

    ///Variables to allow the class be independet 
    //how often will check if obj has eaten
    private float checkFoodElapsed = 7.5f;

    private float startFoodTime;

    //Places
    private Structure _home;
    private Structure _work;
    private Structure _foodSource;
    private Structure _religion;
    private Structure _chill;
    private Vector3 _idlePlace;

    //Techincal
    private List<Vector3> _personBounds = new List<Vector3>();
    private float _personDim = 0.0925f; //used to naviagte btw buildigns 

    //Person Own Objects and fields
    private Brain _brain;
    private Body _body;
    private string spouse = "";
    private bool isWidow; //if is wont get married again

    private bool _isStudent; //true if found a school as being a student bz is in student age 

    //mother and father of a person
    private string _familyId = ""; //the id of a Family will be used to ease the process of moving 
    private string _father;
    private string _mother;

    private bool _isBooked; //says if the person is Booked in a building to be his new home
    private string _homerFoodSrc; //where the Homer will grab the food  

    private bool _isLoading; //use to know if person is being loaded from file 

    public bool IsBooked
    {
        get { return _isBooked; }
        set { _isBooked = value; }
    }

    public string HomerFoodSrc
    {
        get { return _homerFoodSrc; }
        set { _homerFoodSrc = value; }
    }

    #region Initializing Obj

    public Person()
    {
    }

    public Structure Home
    {
        get { return _home; }
        set { _home = value; }
    }

    public Structure Work
    {
        get { return _work; }
        set { _work = value; }
    }

    public Structure FoodSource
    {
        get { return _foodSource; }
        set { _foodSource = value; }
    }

    public List<General> DebugList
    {
        get { return _debugList; }
        set { _debugList = value; }
    }

    public Body Body
    {
        get { return _body; }
        set { _body = value; }
    }

    public float PersonDim
    {
        get { return _personDim; }
    }

    public int Age
    {
        get { return _age; }
        set { _age = value; }
    }


    public Brain Brain
    {
        get { return _brain; }
        set { _brain = value; }
    }

    public H Gender
    {
        get { return _gender; }
        set { _gender = value; }
    }

    public Structure Religion
    {
        get { return _religion; }
        set { _religion = value; }
    }

    public Structure Chill
    {
        get { return _chill; }
        set { _chill = value; }
    }

    public int LifeLimit
    {
        get { return _lifeLimit; }
        set { _lifeLimit = value; }
    }

    public EducationLevel EducationLevel
    {
        get { return _educationLevel; }
        set { _educationLevel = value; }
    }

    public double Happinnes
    {
        get { return _happinnes; }
        set { _happinnes = value; }
    }

    public double Prosperity
    {
        get { return _prosperity; }
        set { _prosperity = value; }
    }

    public bool IsPregnant
    {
        get { return _isPregnant; }
        set { _isPregnant = value; }
    }

    public float NutritionLevel
    {
        get { return _nutritionLevel; }
        set { _nutritionLevel = value; }
    }

    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    public string Spouse
    {
        get { return spouse; }
        set { spouse = value; }
    }

    public bool IsWidow
    {
        get { return isWidow; }
        set { isWidow = value; }
    }




    public bool IsMajor
    {
        get { return _isMajor; }
        set { _isMajor = value; }
    }

    public int LastBdYear
    {
        get { return _lastBDYear; }
        set { _lastBDYear = value; }
    }

    public int BirthMonth
    {
        get { return _birthMonth; }
        set { _birthMonth = value; }
    }

    public int LastNewBornYear
    {
        get { return _lastNewBornYear; }
        set { _lastNewBornYear = value; }
    }

    public int DueMonth
    {
        get { return _dueMonth; }
        set { _dueMonth = value; }
    }

    public int DueYear
    {
        get { return _dueYear; }
        set { _dueYear = value; }
    }

    public string FamilyId
    {
        get { return _familyId; }
        set { _familyId = value; }
    }

    public int UnHappyYears
    {
        get { return _unHappyYears; }
        set { _unHappyYears = value; }
    }

    public bool IsLoading
    {
        get { return _isLoading; }
        set { _isLoading = value; }
    }

    /// <summary>
    /// Intended to be used For the first load of people spawned
    /// </summary>
    public static Person CreatePerson(Vector3 iniPos = new Vector3())
    {
        Person obj = null;

        if (PersonController.GenderLast == H.Male)
        {
            obj = (Person) Resources.Load(Root.personaFeMale1, typeof (Person));
        }
        else if (PersonController.GenderLast == H.Female)
        {
            obj = (Person) Resources.Load(Root.personaMale1, typeof (Person));
        }

        int iniAge = General.GiveRandom(28, 29); //5, 29

        //will assign ramdom pos if has none 
        if (iniPos == new Vector3())
        {
            iniPos = obj.AssignRandomIniPosition();
        }


        obj = (Person) Instantiate(obj, iniPos, Quaternion.identity);
        obj.Gender = obj.OtherGender();
        obj.InitObj(iniAge); //5,29
        obj.Geometry.GetComponent<Renderer>().sharedMaterial = Resources.Load(Root.personGuy1) as Material;

        //this to when Person dont have where to leave and then they find a place the teletranport effect
        //wont be seeable bz there are spawneed hidden. 
        //obj.Body.Hide();

        return obj;
    }

    /// <summary>
    /// Intended to be used while loading persons from file 
    /// </summary>
    public static Person CreatePersonFromFile(PersonFile pF)
    {
        Person obj = null;

        if (pF._gender == H.Male)
        {
            obj = (Person) Resources.Load(Root.personaMale1, typeof (Person));
        }
        else if (pF._gender == H.Female)
        {
            obj = (Person) Resources.Load(Root.personaFeMale1, typeof (Person));
        }

        obj = (Person) Instantiate(obj, obj.AssignRandomIniPosition(), Quaternion.identity);

        obj.IsLoading = true;
        obj.InitLoadedPerson(pF);
        obj.Geometry.GetComponent<Renderer>().sharedMaterial = Resources.Load(Root.personGuy1) as Material;

        return obj;
    }

    /// <summary>
    /// Returns the gennder other than the last used , keeps that saved on PersonController.GenderLast
    /// </summary>	
    public H OtherGender()
    {
        if (PersonController.GenderLast == H.Male)
        {
            PersonController.GenderLast = H.Female;
        }
        else if (PersonController.GenderLast == H.Female)
        {
            PersonController.GenderLast = H.Male;
        }
        return PersonController.GenderLast;
    }

    /// <summary>
    /// Intented to create  kids
    /// </summary>
    /// <param name="typePerson"></param>
    public static Person CreatePersonKid(Vector3 iniPos)
    {
        Person obj = null;

        if (PersonController.GenderLast == H.Male)
        {
            obj = (Person) Resources.Load(Root.personaFeMale1, typeof (Person));
        }
        else if (PersonController.GenderLast == H.Female)
        {
            obj = (Person) Resources.Load(Root.personaMale1, typeof (Person));
        }

        obj = (Person) Instantiate(obj, obj.AssignRandomIniPosition(), Quaternion.identity);
        obj.Gender = obj.OtherGender();
        obj.InitObj(1);
        obj.Geometry.GetComponent<Renderer>().sharedMaterial = Resources.Load(Root.personGuy1) as Material;

        //this to when Person dont have where to leave and then they find a place the teletranport effect
        //wont be seeable bz there are spawneed hidden. 
        //obj.Body.Hide();

        return obj;
    }

    /// <summary>
    /// Init for a loaded person
    /// </summary>
    public void InitLoadedPerson(PersonFile pF)
    {
        Age = pF._age;

        _isMajor = pF.IsMajor;

        //Birthday
        _lastBDYear = pF.LastBDYear; //the year I had the last birthday. So i dont have more thn 1 BD a Year 
        _birthMonth = pF.BirthMonth; //the birthday of the person
        UnHappyYears = pF.UnHappyYears;

        _name = pF._name;
        _lifeLimit = pF._lifeLimit;
        MyId = pF.MyId;
        Gender = pF._gender;


        _lastNewBornYear = pF.LastNewBornYear; //the last pregnancy she had, when was the birth year 
        //once pregnant will tell u the due date 
        _dueMonth = pF.DueMonth;
        _dueYear = pF.DueYear;

        IsStudent = pF.IsStudent;
        Father = pF.Father;
        Mother = pF.Mother;

        IsBooked = pF.IsBooked;
        HomerFoodSrc = pF.HomerFoodSrc;

        Inventory = pF.Inventory;

        FamilyId = pF.FamilyId;
        Spouse = pF._spouse;

        PrevOrder = pF.PrevOrder;

        _body = new Body(this, pF);
        Brain = new Brain(this, pF);

        InitGeneralStuff();

        RecreateProfession(pF);

        //bz loading ends here 
        IsLoading = false;
    }

    private void RecreateProfession(PersonFile pF)
    {
        if (string.IsNullOrEmpty(pF._work))
        {
            return;
        }

        //will create the type of class we need
        CreateProfession(pF.ProfessionProp.ProfDescription, pF);
    }

    public void InitObj(int iniAge)
    {
        Age = iniAge;
        _name = BuildRandomName();

        _lifeLimit = GiveRandom(75, 85);
        MyId = _name + "." + Id;

        Brain = new Brain(this);
        _body = new Body(this);

        InitGeneralStuff();
    }

    private void InitGeneralStuff()
    {
        NameTransform();
        DefineColliders();
        DefineIfIsMajor();
        DefineBirthMonth();
    }

    /// <summary>
    /// Will define a ramdom Birth Month if person doesnt have one yet 
    /// </summary>
    private void DefineBirthMonth()
    {
        if (_birthMonth != 0)
        {
            return;
        }

        _birthMonth = Random.Range(1, 13);
    }

    private void DefineIfIsMajor()
    {
        if (UPerson.IsMajor(Age))
        {
            _isMajor = true;
        }
    }

    private void NameTransform()
    {
        transform.name = MyId + "...|" + Age + "|" + Gender;
    }

    private string BuildRandomName()
    {
        Naming n = new Naming(Gender);
        return n.NewName();
    }

    public static string GiveRandomID()
    {
        Naming n = new Naming(H.Female);
        int numb = GiveRandom(0, 10000);
        return n.NewName() + "." + numb;
    }

    public static string GiveRandomName()
    {
        int numb = GiveRandom(0, 10);
        return GiveRandomID() + "." + numb;
    }


    private Vector3 originalPoint;
    /// <summary>
    /// Returns Random position from origin. If fell inside a building will find another spot
    /// until is in a clear zone
    /// If origin is not specified will assume is CamControl.CAMRTS.hitFront.point the center of terrain
    /// </summary>
    /// <param name="howFar">How far will go</param>
    public Vector3 AssignRandomIniPosition(Vector3 origin = new Vector3(), int howFar = 8)
    {
        if (origin == new Vector3())
        {
            //origin = m.IniTerr.MathCenter;
            origin = CamControl.CAMRTS.hitFront.point;
        }

        //so origin is not changed in every recursive
        if (originalPoint == new Vector3())
        {
            originalPoint = origin;
            origin = ReturnRandomPos(origin, howFar);
        }
        else
        {
            origin = ReturnRandomPos(originalPoint, howFar);
        }

        //_personBounds.Clear();
        ////*4 so Dummy with SwpanPoint has room to spawn 
        //_personBounds = UPoly.CreatePolyFromVector3(origin, PersonDim*4, PersonDim*4);
        //UVisHelp.CreateHelpers(_personBounds, Root.blueCube);
        //if bound collide will recurse

        //if (BuildingPot.Control.Registro.IsCollidingWithExisting(_personBounds) || !IsOnTerrain(origin))
        if (MeshController.CrystalManager1.IntersectAnyLine(ReturnIniPos(), origin)
            || !IsOnTerrain(origin))
        {
            origin = AssignRandomIniPosition(origin);
        }
        originalPoint = new Vector3();
        return origin;
    }

    private Vector3 ReturnRandomPos(Vector3 origin, float howFar)
    {
        float x = UMath.Random(-howFar, howFar);
        float z = UMath.Random(-howFar, howFar);
        origin = new Vector3(origin.x + x, origin.y, origin.z + z);
        return origin;
    }

    /// <summary>
    /// Will return house position if not null. otherwise person current pos
    /// </summary>
    /// <returns></returns>
    Vector3 ReturnIniPos()
    {
        if (Home != null)
        {
            return Home.SpawnPoint.transform.position;
        }

        if (IsOnTerrain(transform.position))
        {
            return transform.position;
        }
        return CamControl.CAMRTS.hitFront.point;

    }

    /// <summary>
    /// Will say if origin is on terrain 
    /// </summary>
    /// <param name="origin"></param>
    /// <returns></returns>
    public bool IsOnTerrain(Vector3 origin)
    {
        var hit = m.SubDivide.FindYValueOnTerrain(origin.x, origin.z);
        var terrCenter = m.IniTerr.MathCenter;

        //UnityEngine.Debug.Log("dist:"+Mathf.Abs(terrCenter.y - hit));

        if (Mathf.Abs(terrCenter.y - hit) < 0.1f )
        {
            return true;
        }
        
        return  false;
    }

    #endregion

    #region Person Nutrition

    void ChangeNutritionLvl(int change)
    {
        _nutritionLevel += change;
    }

    void CheckOnNutrition()
    {
        ChangeNutritionLvl(-1);
        //KillStarve();
    }

    /// <summary>
    /// The action of getting some nutrition, sets the _nutritionLevel
    /// </summary>
    /// <param name="amt"></param>
    /// <param name="item"></param>
    void Nutrive(int amt, P item)
    {
        var nutriValue = BuildingPot.Control.ProductionProp.Food1.NValues.Find(a => a.Nutrient == item).NutritionVal;
        _nutritionLevel += (amt * nutriValue);

        //UnityEngine.Debug.Log(MyId + " nutrived nutriVal:" + amt * nutriValue + ". curr:" + _nutritionLevel);
    }

    /// <summary>
    /// Of _nutrition level is so low will kill person
    /// </summary>
    void KillStarve()
    {
        if (_nutritionLevel < 0)
        {
            ChangeHappinesBy(-0.1);
            //UpdateInfo("Starving");
        }
        if (_nutritionLevel < -45)
        {
            ActionOfDisappear();
            print("Too hungry and died:"+MyId);
        }
    }

    #endregion

    #region Age Related Methods

    private int month = 0;

    /// <summary>
    /// Things related to time 
    /// 
    /// Will check if due date is now and if Birthdays is reached 
    /// </summary>
    void TimeChecks()
    {
        Birthday();
        CheckIfReadyForGiveBirth();
    }

    /// <summary>
    /// Called when a person has its birthday
    /// </summary>
    void Birthday()
    {
        if (!IsMyBD())
        {
            return;
        }

        AgeAction();

        if (UPerson.IsMajor(_age) && !_isMajor)
        {
            ReachAgeMajority();
        }

        CheckHappiness();
        //DidIDie();
        CheckIfEmmigrate();
    }

    /// <summary>
    /// The actions of of age in a BD
    /// </summary>
    void AgeAction()
    {
        _age++;
        Body.GrowScaleByYears();
        NameTransform();//so it reflects the new Age
        
    }

    void CheckHappiness()
    {
        if (Happinnes < 0.5f)
        {
            _unHappyYears++;
            return;
        }

        _unHappyYears = 0;
    }

    /// <summary>
    /// Will tell u if is your year will remember the last time u had a BD 
    /// </summary>
    /// <returns></returns>
    bool IsMyBD()
    {
        var currYear = Program.gameScene.GameTime1.Year;

        if (_lastBDYear != currYear && _birthMonth == Program.gameScene.GameTime1.Month1)
        {
            _lastBDYear = currYear;
            return true;
        }
        return false;
    }

    static int debug = 0;
    public PersonReport PersonReport = new PersonReport();

    /// <summary>
    /// Will contain all the functions to execute for a person when reach the Age Majority 
    /// 
    /// Call once a birthday if is old enough until, _isMajor = true
    /// </summary>
    void ReachAgeMajority()
    {
        if (debug >= 3)
        {
            //Program.gameScene.GameSpeed = 0;
            var t = this;
        }

        //will only will mark as majority age reached if he could fit a house 
        if (WouldIFindAPlaceToLive())
        {
            //print("Age Major: " + MyId);
            RemoveMeFromOldHome();
            _isMajor = true;
            Brain.MajorAge.MarkMajorityAgeReached();
            PersonPot.Control.IsAPersonHomeLessNow = MyId;
            debug++;
        }
    }

    /// <summary>
    /// Will retrun true if a house was find that can fit a Adult 
    /// </summary>
    /// <returns></returns>
    private bool WouldIFindAPlaceToLive()
    {
        //means that anotehr person is ins the process of finding a new home 
        if (!string.IsNullOrEmpty(PersonPot.Control.IsAPersonHomeLessNow))
        {
            return false;
        }

        var buildings = UBuilding.ReturnBuildings(BuildingPot.Control.HousesWithSpace);

        for (int i = 0; i < buildings.Count; i++)
        {
            if (buildings[i].WouldAdultFitInThisHouseInAFamily(this))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// bz when new adult needs to get out of old home .
    /// 
    /// 
    /// </summary>
    void RemoveMeFromOldHome()
    {
        if (Home != null)
        {
            var family = Home.FindMyFamily(this);

            if (family != null)
            {
                family.RemovePersonFromFamily(this);    
            }

            Brain.MoveToNewHome.OldHomeKey = "";//so he doesnt pull that family as its old family when creating Shack or moving to new home 
            BuildingPot.Control.AddToHousesWithSpace(Home.MyId);

            //so families are resaved 
            BuildingPot.Control.Registro.ResaveOnRegistro(Home.MyId);
        }
    }

    /// <summary>
    /// Will check if person passed its _lifeLimit
    /// </summary>
    private void DidIDie()
    {
        if (Age > _lifeLimit)
        {
          print(MyId + " gone , se partio");
          ActionOfDisappear();
        }
    }

    /// <summary>
    /// Will order to the brain to disapper 
    /// </summary>
    void ActionOfDisappear()
    {
        Brain.Partido = true;

        //if is widow dont need to check below means parter ya se partio
        if (!string.IsNullOrEmpty(Spouse) && !IsWidow)
        {
            Person sp = Family.FindPerson(Spouse);

            //to address when a person die in btw this one, assign true to Brain and still waiting to be process 
            if (sp == null)
            {
                return;
            }

            sp.IsWidow = true;
        }
    }

    #endregion

    public void Marriage(string spouseMyId)
    {
        spouse = spouseMyId;
    }

	/// <summary>
    /// Returns true if the other person is suitable for marriage with currnet obj
    /// </summary>
    public bool WouldUMarryMe(Person other)
    {
        if (spouse != "") { return false;}
        if (isWidow) { return false;}
        if (other.Gender == Gender) { return false;}
        if (Mathf.Abs(other.Age - _age) > 15) { return false;}

        return true;
    }

    // Use this for initialization
	void Start () 
    {
        base.Start();
        
        StartCoroutine("FiveSecUpdate");
        StartCoroutine("RandomUpdate1020");
        StartCoroutine("QuickUpdate");
        //StartCoroutine("QuickUpdate2");

        //means is loading from file
        if (Inventory != null && Inventory.InventItems.Count > 0)
	    {
	        return;
	    }

        Inventory = new Inventory(MyId, HType);


        _animator = GetComponent<Animator>();
        _bip = GetChildCalled("Bip001");
	}

    float RestartTimes(float a, float b){return Random.Range(a, b);}


    private float _quickTime;
    public float QuickTime
    {
        get { return _quickTime; }
        set { _quickTime = value; }
    }

    private IEnumerator QuickUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(_quickTime); // wait 
            _quickTime = RestartTimes(0f, 1f);
        }
    }


    private float quickTime2;
    /// <summary>
    /// For Person dont walk on top of each other 
    /// </summary>
    /// <returns></returns>
    private IEnumerator QuickUpdate2()
    {
        while (true)
        {
            yield return new WaitForSeconds(quickTime2); // wait 
            UpdateCollBools();
            CheckPersonColl();
            quickTime2 = RestartTimes(4f, 5f);
        }
    }

    private IEnumerator FiveSecUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(checkFoodElapsed); // wait
            CheckOnNutrition();
        }
    }

    private float random1020Time;
    private IEnumerator RandomUpdate1020()
    {
        while (true)
        {
            yield return new WaitForSeconds(random1020Time); // wait
            _brain.SlowCheckUp();
            random1020Time = RestartTimes(5f, 10f);

            TryHaveKids();
        }
    }

	// Update is called once per frame
	void FixedUpdate()
	{
        if (!PersonPot.Control.Locked)
        {
            _brain.CheckConditions();
        }

        Brain.Update();
        _body.Update();
        //UpdateInfo();

	    if (_profession != null)
	    {
            _profession.Update();     
	    }
	    
        TimeChecks();
        Program.gameScene.GameTime1.FixedUpdate();
	    LODCheck();
	}

    public void UpdateInfo(string add = "")
    {
        string homeId = "none";
        if (Home != null){homeId = Home.MyId;}

        string foodId = "none";
        if (FoodSource != null) { foodId = FoodSource.MyId; }

        string workId = "none";
        if (Work != null) { workId = Work.MyId; }

        string religionId = "none";
        if (Religion != null) { religionId = Religion.MyId; }

        string chillId = "none";
        if (Chill != null) { chillId = Chill.MyId; }

        info = "Home:" + homeId + "\n" +
               "FoodSrc:" + foodId + "\n" +
               "Work:" + workId + "\n" +
               "Religion:" + religionId + "\n" +
               "Chill:" + chillId + "\n" +
               "NutriLevel:" + _nutritionLevel + "\n" +
               "add:" + add + "\n" +
               "Current Task:" + Brain.CurrentTask + "\n" +
               "Location:" + Body.Location + "\n" +
               "GoingTo:" + Body.GoingTo + "\n" 
               ;
    }

    public bool IsVisible()
    {
        Renderer r = Geometry.GetComponent<Renderer>();
        //print("is render visible."+r.isVisible);
        return r.isVisible;
    }
    
    public bool I_Can_See()
    {
        Camera rts = CamControl.CAMRTS.GetComponent<Camera>();
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(rts);

        Collider c = gameObject.GetComponent<Collider>();

        if (GeometryUtility.TestPlanesAABB(planes, c.bounds))
            return true;
        else
            return false;
    }

    #region Section that detect and handles if is colliding with another person while moving 

    private GameObject _frontCollider;
    private GameObject _rearCollider;
    void DefineColliders()
    {
        _frontCollider = GetChildLastWordIs(H.FrontCollider);
        _rearCollider = GetChildLastWordIs(H.RearCollider);
    }

    private bool frontCollision;
    private bool rearCollision;
    void UpdateCollBools()
    {
        frontCollision = _frontCollider.GetComponent<CollideWith>().IsCollidingNow;
        rearCollision = _rearCollider.GetComponent<CollideWith>().IsCollidingNow;
    }

    void CheckPersonColl()
    {
        if (!Body.MovingNow){return;}

        if (rearCollision && frontCollision)
        { //do nothing
            print("do nothing");
        }
        else if (frontCollision)
        {
            ChangeBodySpeed(-0.1f);
        }
        else if(rearCollision)
        {
            ChangeBodySpeed(0.1f);  
        }
        else if(!rearCollision && !frontCollision)
        {
            SetBodySpeedToDefault();
        }
    }

    private bool speedChanged;//acuumulates the changes of speed 
    void ChangeBodySpeed(float amt)
    {
        if (Body.Speed != 0.5f){return;}//will only change it if is at is initial value

        speedChanged =true;
        Body.ChangeSpeed(amt);
        print("speed chng");
    }

    void SetBodySpeedToDefault()
    {
        if (speedChanged)
        {
            Body.Speed = 0.5f;
            speedChanged = false;
        }
    }

    #endregion








    #region Profession

    private Profession _profession = new Profession();

    Job _prevJob;//wht was the prev Job
    Order _prevOrder;//previous order, this so far is only of use of Wheel Barrowers

    public Profession ProfessionProp
    {
        get { return _profession; }
        set { _profession = value; }
    }

    public bool IsStudent
    {
        get { return _isStudent; }
        set { _isStudent = value; }
    }

    public string Father
    {
        get { return _father; }
        set { _father = value; }
    }

    public string Mother
    {
        get { return _mother; }
        set { _mother = value; }
    }

    public Job PrevJob
    {
        get { return _prevJob; }
        set { _prevJob = value; }
    }

    public Order PrevOrder
    {
        get { return _prevOrder; }
        set { _prevOrder = value; }
    }

   


    /// <summary>
    /// Will find the type of job based on type of building we are currently working on only if jType=None
    /// 
    /// if jType has a value diff than None will return tht 
    /// </summary>
    /// <param name="jType"></param>
    /// <returns></returns>
    Job DefineJobType(Job jType)
    {
        if (jType == Job.None)
        {
            return ReturnJobType();
        }
        return jType;
    }

    /// <summary>
    /// This is called when person finds a new job site 
    /// </summary>
    /// <param name="jType"></param>
    public void CreateProfession(Job jType = Job.None, PersonFile pF = null)
    {
        _profession.CleanOldProf();
        jType = DefineJobType(jType);

        if (jType == Job.Forester)
        {
            _profession = new Forester(this, pF);
            
        }
        else if (jType == Job.Docker)
        {
            _profession = new Docker(this, pF);
        }
        else if (jType == Job.Builder)
        {
            _profession = new Builder(this, pF);
            //print("new Builder:"+MyId);
        }
        else if (jType == Job.FisherMan)
        {
            _profession = new FisherMan(this, pF);
        }
        else if (jType == Job.Insider)
        {
            _profession = new Insider(this, pF);
            //GameScene.print("new Insider :" + MyId);
        }
        else if (jType == Job.ShackBuilder)
        {
            _profession = new ShackBuilder(this, pF);
        }
        else if (jType == Job.WheelBarrow)
        {
            _profession = new WheelBarrow(this, pF);
            //print("new WheelBarrow:"+MyId);
        }
        else if(jType == Job.Homer)
        {
            _profession = new Homer(this, pF);
            //print("new Homer:" + MyId);
        }
        else if(jType == Job.Farmer)
        {
            _profession = new Farmer(this, pF);
        }
    }

    /// <summary>
    /// Based on the current Work Htype will tell u wihc is the person Job Type
    /// </summary>
    private Job ReturnJobType()
    {
        if (Work.HType == H.Wood)
        {
            return Job.Forester;
        }
        else if (Work.HType == H.Dock)
        {
            return Job.Docker;
        }
        else if (Work.HType == H.BuildersOffice)
        {
            return Job.Builder;
        }
        else if (Work.HType == H.FishSmall || Work.HType == H.FishRegular)
        {
            return Job.FisherMan;
        }
        else if ( Work.HType.ToString().Contains(H.Farm.ToString()))
        {
            return Job.Farmer;
        }
        return Job.Insider;
    }

    #endregion

    internal void Kill()
    {
        var t = PersonPot.Control.All.Find(a => a.MyId == MyId);
        GameScene.print(MyId+".Killed");
        PersonPot.Control.All.Remove(t);
        t.Destroy();
    }

    public void HomeActivities()
    {
        if(Home == null || Home.Inventory == null)
        { return;}

        DropFoodAtHome();
        Eat();
    }

    private void Eat()
    {
        P item = Home.Inventory.GiveBestFood();
        if (item == P.None)
        {
            return;
        }

        int amt = (int)HowMuchINeedToBe100PointsFeed(item);
        int gotAmt = Home.Inventory.Remove(item, amt);

        ChangeHappinesBy(0.1);
        Nutrive(gotAmt, item);
    }

    void ChangeHappinesBy(double by)
    {
        Happinnes += by;

        if (Happinnes > 5)
        {
            Happinnes = 5;
        }
        else if (Happinnes < 0)
        {
            Happinnes = 0;
        }
    }

    public void DropFoodAtHome()
    {
        if (Home == null || Home.Instruction == H.WillBeDestroy)
        {
            return;
        }

        TransferInvetoryCat(this, Home, PCat.Food);
    }

    /// <summary>
    /// Will get food from the 'theFoodSrc'
    /// </summary>
    /// <param name="theFoodSource"></param>
    public void GetFood(Structure theFoodSrc)
    {
        //wont get anymore food is his house is full
        if (Home != null && Home.Inventory != null && Home.Inventory.IsFull())
        {
            //UnityEngine.Debug.Log(MyId+" my house inv is full");
            return;
        }

        //had error on log sept.21.2015
        if (theFoodSrc == null)
        {
            return;
        }

        P item = theFoodSrc.Inventory.GiveBestFood();

        if (item == P.None)
        {
            return;
        }

        int amt = HowMuchICanCarry(item);
        
        ExchangeInvetoryItem(FoodSource, this, item, amt);
        //UnityEngine.Debug.Log(MyId+" took food:"+item);
    }

    public void ExchangeInvetoryItem(General takenFrom, General givenTo, P product, int amt)
    {
        //to address when food Src is destroyed when person on its way 
        if (takenFrom == null)
        { return; }

        int amtTook = takenFrom.Inventory.Remove(product, amt);
        givenTo.Inventory.Add(product, amtTook);
    }

    /// <summary>
    /// Will be used for person drop food at home 
    /// </summary>
    void TransferInvetoryCat(General from, General to, PCat pCat)
    {
        var items = from.Inventory.ReturnAllItemsCat(pCat);

        if (items == null || to == null || to.Inventory == null)
        {
            return;
        }

        to.Inventory.AddItems(items);
        from.Inventory.RemoveItems(items);
    }

    /// <summary>
    /// Will tell u if this person at least has one family member,
    ///  and is not on the middle of moving out to new home with fmaily
    /// </summary>
    /// <returns></returns>
    public bool HasFamily()
    {
        if (Home == null)
        {
            return false;
        }
        Family myFamily = Home.FindMyFamily(this);

        if (myFamily == null)
        {
            return false;
        }

        //if State is not None means is on the middle of moving out to new place 
        return myFamily.DoesFamilyHasMoreThan1Member() && myFamily.State == H.None;
    }

    /// <summary>
    /// Will tell u if a person doesnt have mom and dad yet
    /// </summary>
    /// <returns></returns>
    internal bool IsOrphan()
    {
        if (string.IsNullOrEmpty(_father ) && string.IsNullOrEmpty(_mother ))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// How much phisically a person can carry. Like goods .lIke Rice
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    private int HowMuchICanCarry(P item)
    {
        if (Age < 21)
        {
            return 8 * Age;
        }

        return 200;
    }

    /// <summary>
    /// Will tell u how much u will need of a tpye of food to be fed to 100 %
    /// </summary>
    /// <param name="item">the type of food</param>
    /// <returns></returns>
    private float HowMuchINeedToBe100PointsFeed(P item)
    {
        var nutriValue = BuildingPot.Control.ProductionProp.Food1.NValues.Find(a => a.Nutrient == item).NutritionVal;
        var nutriNeed = 100 - _nutritionLevel;

        return  nutriNeed / nutriValue + 1;//+1 is to round up
    }
    
    #region Reproduction Having Kids & Stuff

    /// <summary>
    /// The decition of a having a new kid 
    /// </summary>
    /// <returns></returns>
    bool CanIHaveANewKid()
    {
        if (IsPregnant || IsWidow || Gender == H.Male || string.IsNullOrEmpty(Spouse) || Home == null)
        {
            return false;
        }

        if (Home != null && Home.BookedHome1 != null && Home.BookedHome1.IsBooked())
        {
            return false;
        }

        var family = Home.FindMyFamily(this);
        if (family == null)
        {
            return false;
        }

        bool hasSpace = family.QuickQuuestionCanGetAnotherKid();

        bool nutrida = AmINutrida();
        bool happy = AmIHappy();
        bool bodyReady = Mathf.Abs( _lastNewBornYear - Program.gameScene.GameTime1.Year ) > 1;

        return nutrida && hasSpace && happy && bodyReady && Age < 45;
    }

    /// <summary>
    /// happy enough to do something wuth my life. like having new kids
    /// </summary>
    /// <returns></returns>
    private bool AmIHappy()
    {
        return Happinnes > 3f;
    }

    public void TryHaveKids()
    {
        if (CanIHaveANewKid())
        {
            GetPregnant();
//            UnityEngine.Debug.Log(MyId+" got pregnant due m:" + _dueMonth+" y:" + _dueYear);
        }
    }

    private void GetPregnant()
    {
        IsPregnant = true;
        CalculateDueDate();
    }

    void CheckIfReadyForGiveBirth()
    {
        if (IsMyDueDate())
        {
            GiveBirth();
        }
    }

    void GiveBirth()
    {
        PersonPot.Control.HaveNewKid(Home.transform.position);
        Person kid = PersonPot.Control.All[PersonPot.Control.All.Count - 1];
        kid.Mother = this.MyId;
        kid.Father = Spouse;
        kid.FamilyId = FamilyId;
        kid.IsBooked = true;

        MoveNewBornToHome(kid);

        Debug.Log(MyId + " give birth to:" + kid.MyId);

        _lastNewBornYear = _dueYear;
        _dueMonth = 0;
        _dueYear = 0;
        IsPregnant = false;

        Happinnes = 5;
        var spouse = Family.FindPerson(Spouse);
        spouse.Happinnes = 5;

        PersonPot.Control.RestartControllerForPerson(kid.MyId);
    }

    /// <summary>
    /// Takes care of the actions needed to move the newBorn to Home 
    /// </summary>
    /// <param name="newBorn"></param>
    void MoveNewBornToHome(Person newBorn)
    {
        var family = Home.FindMyFamily(this);
        family.Kids.Add(newBorn.MyId);

        newBorn.transform.parent = Home.transform;
        newBorn.Home = Home;
        newBorn.Brain.SetNewHouseFound();
    }

    /// <summary>
    /// Will return true if is due date 
    /// </summary>
    /// <returns></returns>
    bool IsMyDueDate()
    {
        return _dueMonth == Program.gameScene.GameTime1.Month1 
            && _dueYear == Program.gameScene.GameTime1.Year;
    }

    /// <summary>
    /// Will calcultae the due date of a woman 
    /// 
    /// Will set _dueMonth and _dueYear
    /// </summary>
    private void CalculateDueDate()
    {
        var currMonth = Program.gameScene.GameTime1.Month1;
        _dueMonth = UMath.GoAround(9, currMonth, 1, 12);

        if (_dueMonth < currMonth)
        {
            _dueYear = Program.gameScene.GameTime1.Year + 1;
        }
        else
        {
            _dueYear = Program.gameScene.GameTime1.Year;     
        }
    }

    /// <summary>
    /// Will tell u if person is well fed. Nutrition wise
    /// </summary>
    /// <returns></returns>
    private bool AmINutrida()
    {
        return NutritionLevel > 10;
    }

#endregion

    /// <summary>
    /// Every BD will ask if wnt to emmigrate 
    /// </summary>
    void CheckIfEmmigrate()
    {
        if (_unHappyYears > 9 && UPerson.IsMajor(Age))
        {
            Emmigrate();
        }
    }

    public void Emmigrate()
    {
        EmmigrateWithFamily();
        ActionOfDisappear();
        print(MyId+" emmigrated");
    }

    /// <summary>
    /// Will make all ur family emmigrate.
    /// 
    /// Bz if u dont do so will bring other kind of bugs 
    /// </summary>
    void EmmigrateWithFamily()
    {
        if (Home == null)
        {
            return;
        }

        var fam = Home.FindMyFamily(this);
        if (fam == null)
        {
            return;
        }

        var members = fam.ReturnFamilyPersonObj();
        for (int i = 0; i < members.Count; i++)
        {
            members[i].Emmigrate();
        }
    }

    /// <summary>
    /// Will restart the Person Controller only for the family members tht are in that 'home'
    /// </summary>
    /// <param name="home">Where the family members are at now </param>
    public void RestartPersonControllerForFamilyMembers(string home)
    {
        var homeH = Brain.GetBuildingFromKey(home);
        if (homeH == null)
        {
            return;
        }

        var fam = homeH.FindMyFamily(this);
        if (fam == null)
        {
            return;
        }

        var members = fam.ReturnFamilyPersonObj();

        for (int i = 0; i < members.Count; i++)
        {
            PersonPot.Control.RestartControllerForPerson(members[i].MyId);
        }
    }













    #region LOD




    private Animator _animator;
    private GameObject _bip;


    void LODCheck()
    {
        if (_bip == null)
        {
            return;
        }

        var dist = Vector3.Distance(transform.position, Camera.main.transform.position);

        if (dist > 50)
        {
            LOD1();
        }
        else if (dist > 75)
        {
            LOD2();
        }
        else
        {
            LODBest();
        }
    }

    void LOD1()
    {
        _bip.SetActive(false);
        _animator.enabled = false;
    }

    void LOD2()
    {
        Geometry.SetActive(false);
    }

    void LODBest()
    {
        _bip.SetActive(true);
        _animator.enabled = true;
        Geometry.SetActive(true);
    }

#endregion






    #region Bad Ass

    internal void SelectPerson()
    {
        CreateProjector();
    }

    /// <summary>
    /// This is here bz when a building is unselected the proj has to be destroyed
    /// </summary>
    public static void UnselectPerson()
    {
        DestroyProjector();
    }

    private static General _projector;
    private static General _light;

    /// <summary>
    /// this is the projector that hover when creating a nw building, or the current selected building
    /// </summary>
    public General Projector
    {
        get { return _projector; }
        set { _projector = value; }
    }

    public void CreateProjector()
    {
        if (_light == null)
        {
            Vector3 projNewP = new Vector3(0,6,0) + transform.position;
            Projector = Create(Root.projectorPerson, projNewP, container: transform);
            Projector.transform.Rotate(new Vector3(90, 0, 0));

            _light = Create(Root.lightCilPerson, transform.position, container: transform);
        }
    }

    static void DestroyProjector()
    {
        if (_light != null)
        {
            _projector.Destroy();
            _projector = null;

            _light.Destroy();
            _light = null;
        }
    }
    #endregion
}

public class PersonReport
{
    public string whoGreenMeToBecomeMajor;
    public string whoDeniedMeAHouse;
}
