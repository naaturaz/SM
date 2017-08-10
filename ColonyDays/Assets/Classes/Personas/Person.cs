using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Person : Hoverable
{





    public void MouseClickHandler(object sender, EventArgs e)
    {
        //Person v = (Person)sender;
        CheckMouseClicked();
    }




    //Debug
    private List<General> _debugList = new List<General>();

    private int _age;
    private float _weight;//kg
    private float _height;//cm
    private Nutrition _nutrition;

    private bool _isMajor; //says if a person is major age 

    //Birthday Related
    private int _lastBDYear; //the year I had the last birthday. So i dont have more thn 1 BD a Year 
    private int _birthMonth; //the birthday of the person
    private int _unHappyYears; //if is over X amount will emmigrate

    private int _lifeLimit;
    private EducationLevel _educationLevel;
    private double _happinnes = 5, _prosperity = 5;
    private H _gender;

    //only for females
    private bool _isPregnant;
    private int _lastNewBornYear = -10; //the last pregnancy she had, when was the birth year 
    //once pregnant will tell u the due date 
    private int _dueMonth;
    private int _dueYear;


    private string _nutritionLevel = "Normal";
    private string _thirst = "Quenched";



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
    private string _startingBuild;
    private Vector3 _idlePlace;

    //Techincal
    private List<Vector3> _personBounds = new List<Vector3>();
    private float _personDim = 0.0925f; //used to naviagte btw buildigns 

    //Person Own Objects and fields
    private Brain _brain;

    MilitarBrain _militarBrain;
    EnemyBrain _enemyBrain;

    private Body _body;
    private string spouse = "";
    private bool isWidow; //if is wont get married again

    private bool _isStudent; //true if found a school as being a student bz is in student age 
    private int _yearsOfSchool; //true if found a school as being a student bz is in student age 

    //mother and father of a person
    private string _familyId = ""; //the id of a Family will be used to ease the process of moving 
    private string _father;
    private string _mother;

    private string _isBooked = ""; //says if the person is Booked in a building to be his new home
    //private string _homerFoodSrc; //where the Homer will grab the food  

    private bool _isLoading; //use to know if person is being loaded from file 

    private PersonBank _personBank;
    private RandomUV _randomUV;



    private Structure _myDummy;
    private Structure _myDummyProf;



    /// <summary>
    /// eahc person has a dummy use to routing. here for GC 
    /// </summary>
    public Structure MyDummy
    {
        get { return _myDummy; }
        set { _myDummy = value; }
    }



    #region Reload Inventory

    bool _reloadInv = true;
    internal bool IsToReloadInventory()
    {
        return _reloadInv;
    }

    internal void InventoryReloaded()
    {
        _reloadInv = false;
    }

    void ShouldReloadInventory()
    {
        _reloadInv = true;
    }

    #endregion

    /// <summary>
    /// For profesions routing 
    /// </summary>
    public Structure MyDummyProf
    {
        get { return _myDummyProf; }
        set { _myDummyProf = value; }
    }
    public MilitarBrain MilitarBrain
    {
        get { return _militarBrain; }
        set { _militarBrain = value; }
    }

    public string Thirst
    {
        get { return _thirst; }
        set { _thirst = value; }
    }

    public PersonBank PersonBank1
    {
        get { return _personBank; }
        set { _personBank = value; }
    }

    public string IsBooked
    {
        get { return _isBooked; }
        set { _isBooked = value; }
    }

    //public string HomerFoodSrc
    //{
    //    get { return _homerFoodSrc; }
    //    set { _homerFoodSrc = value; }
    //}

    public float Weight
    {
        get { return _weight; }
        set { _weight = value; }
    }

    public float Height
    {
        get { return _height; }
        set { _height = value; }
    }

    public Nutrition Nutrition1
    {
        get { return _nutrition; }
        set { _nutrition = value; }
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
        set
        {
            if (_work != null && value == null)
            {
                //Debug.Log("I calling to make work null.."+MyId);
            }

            _work = value;
        }
    }

    public Structure FoodSource
    {
        get { return _foodSource; }
        set { _foodSource = value; }
    }

    /// <summary>
    /// If person was ramdon assig a position and if was set close 
    /// to a building like Storage or Dock . That building will be set here 
    /// 
    /// Not implement SaveLoad
    /// 
    /// Created to solve CryBridgeRoute.cs 125
    /// 
    /// The real solution is to find the closest building in MoveToNEwHome
    /// </summary>
    public string StartingBuild
    {
        get { return _startingBuild; }
        set
        {
            _startingBuild = value;
        }
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

    public int YearsOfSchool
    {
        get { return _yearsOfSchool; }
        set { _yearsOfSchool = value; }
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

    public string NutritionLevel
    {
        get { return _nutritionLevel; }
        set { _nutritionLevel = value; }
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
        set
        {
            if (Home != null && _familyId.Contains(Home.MyId) && !value.Contains(Home.MyId))
            {
                //               //Debug.Log(MyId + " Changing from:" + _familyId + " to:" + value + " while on:" + Home.MyId);
            }

            _familyId = value;
        }
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
    public static Person CreatePerson(Vector3 iniPos = new Vector3(), int age = 0)
    {
        Person obj = null;

        obj = (Person)Resources.Load(PersonPrefabRoot(PersonController.GenderLast), typeof(Person));


        //better like 17/29. since if they are less than 16 and have not parent is not addressed
        int iniAge = General.GiveRandom(17, 29); //5, 29

        if (age > 0)
        {
            iniAge = age;
        }

        obj = (Person)Instantiate(obj, iniPos, Quaternion.identity);

        //will assign ramdom pos if has none 
        if (iniPos == new Vector3())
        {
            iniPos = obj.AssignRandomIniPosition();
            obj.transform.position = iniPos;
        }

        obj.HType = H.Person;
        obj.Gender = obj.OtherGender();
        obj.InitObj(iniAge);

        //todo change 
        //obj.Geometry.GetComponent<Renderer>().sharedMaterial = ReturnRandoPersonMaterialRoot();

        //this to when Person dont have where to leave and then they find a place the teletranport effect
        //wont be seeable bz there are spawneed hidden. 
        obj.Body.HideNoQuestion();

        return obj;
    }



    static Material ReturnRandoPersonMaterialRoot()
    {
        var random = UMath.GiveRandom(1, 6);
        return Resources.Load("Prefab/Mats/Person/Guy1UV " + random) as Material;//random
    }

    /// <summary>
    /// Intended to be used while loading persons from file 
    /// </summary>
    public static Person CreatePersonFromFile(PersonFile pF)
    {
        Person obj = null;


        obj = (Person)Resources.Load(PersonPrefabRoot(pF._gender, true), typeof(Person));


        SMe me = new SMe();
        obj = (Person)Instantiate(obj, me.IniTerr.MathCenter, Quaternion.identity);

        obj.IsLoading = true;
        obj.InitLoadedPerson(pF);
        //obj.Geometry.GetComponent<Renderer>().sharedMaterial = ReturnRandoPersonMaterialRoot();
        obj.HType = H.Person;

        return obj;
    }


    static string PersonPrefabRoot(H gender, bool load = false)
    {
        if (load)
        {
            if (gender == H.Female)
            {
                return "Prefab/Personas/PersonaFeMale1";
            }
            else
            {
                return "Prefab/Personas/PersonaMale1";
            }
        }

        if (gender == H.Male)
        {
            return "Prefab/Personas/PersonaFeMale1";
        }
        else
        {
            return "Prefab/Personas/PersonaMale1";
        }
    }



    #region Enemy

    /// <summary>
    /// Intended to be used For the first load of people spawned
    /// </summary>
    public static Person CreatePersonEnemy(Vector3 iniPos = new Vector3())
    {
        Person obj = null;
        obj = (Person)Resources.Load(Root.personaMale1, typeof(Person));

        int iniAge = General.GiveRandom(19, 32);
        obj = (Person)Instantiate(obj, iniPos, Quaternion.identity);

        obj.HType = H.Enemy;
        obj.InitObj(iniAge);

        //this to when Person dont have where to leave and then they find a place the teletranport effect
        //wont be seeable bz there are spawneed hidden. 
        //obj.Body.HideNoQuestion();

        return obj;
    }




    #endregion 










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

        obj = (Person)Resources.Load(PersonPrefabRoot(PersonController.GenderLast), typeof(Person));


        obj = (Person)Instantiate(obj, new Vector3(iniPos.x, 0.06f, iniPos.z),
            Quaternion.identity);
        obj.Gender = obj.OtherGender();

        obj.HType = H.Person;

        obj.InitObj(0);//15    5
                       //obj.Geometry.GetComponent<Renderer>().sharedMaterial = ReturnRandoPersonMaterialRoot();


        //this to when Person dont have where to leave and then they find a place the teletranport effect
        //wont be seeable bz there are spawneed hidden. 
        obj.Body.HideNoQuestion();


        return obj;
    }

    /// <summary>
    /// Init for a loaded person
    /// </summary>
    public void InitLoadedPerson(PersonFile pF)
    {
        CreateTheTwoDummies();

        NutritionLevel = pF.NutritionLevel;
        Age = pF._age;

        _isMajor = pF.IsMajor;

        //Birthday
        _lastBDYear = pF.LastBDYear; //the year I had the last birthday. So i dont have more thn 1 BD a Year 
        _birthMonth = pF.BirthMonth; //the birthday of the person
        UnHappyYears = pF.UnHappyYears;

        Name = pF._name;
        _lifeLimit = pF._lifeLimit;
        MyId = pF.MyId;
        Gender = pF._gender;

        IsPregnant = pF._isPregnant;
        _lastNewBornYear = pF.LastNewBornYear; //the last pregnancy she had, when was the birth year 
        //once pregnant will tell u the due date 
        _dueMonth = pF.DueMonth;
        _dueYear = pF.DueYear;

        IsStudent = pF.IsStudent;
        Father = pF.Father;
        Mother = pF.Mother;

        IsBooked = pF.IsBooked;
        //HomerFoodSrc = pF.HomerFoodSrc;

        Inventory = pF.Inventory;

        FamilyId = pF.FamilyId;
        Spouse = pF._spouse;

        PrevOrder = pF.PrevOrder;
        YearsOfSchool = pF.YearsOfSchool;
        SavedJob = pF.SavedJob;
        PrevJob = pF.PrevJob;
        IsWidow = pF._isWidow;
        StartingBuild = pF.StartingBuild;

        Weight = pF.Weight;
        Height = pF.Height;

        Nutrition1 = pF.Nutrition1;
        Nutrition1.SetPerson(this);

        WasFired = pF.WasFired;
        PersonBank1 = pF.PersonBank;
        PersonBank1.SetPerson(this);

        _body = new Body(this, pF);

        InitEvents();

        Brain = new Brain(this, pF);

        InitGeneralStuff();

        RecreateProfession(pF);

        WorkInputOrders = pF.WorkInputOrders;

        //bz loading ends here 
        IsLoading = false;
    }

    internal void ResetBrainAndBody()
    {
        _brain = null;
        _brain = new Brain(this);

        _body = null;
        _body = new Body(this);
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
        Name = GiveMeMyName();

        _lifeLimit = GiveRandom(75, 85);//        40
        MyId = Name + "." + Id;

        if (HType == H.Person)
        {
            Brain = new Brain(this);
        }

        _body = new Body(this);

        InitEvents();

        DefineIfIsMajor();
        DefineBirthMonth();
        InitGeneralStuff();

        if (Age != 0)
        {
            return;
        }
        Program.gameScene.GameController1.NotificationsManager1.Notify("BabyBorn", Name);
    }

    void InitEvents()
    {
        Program.InputMain.ChangeSpeed += _body.ChangedSpeedHandler;
        Program.gameScene.ChangeSpeed += _body.ChangedSpeedHandler;
        GameController.War += WarHandler;
    }


    /// <summary>
    /// redoes brain and restartCOntroller for person
    /// </summary>
    /// <param name="blackList"></param>
    public void RedoBrain(List<string> blackList)
    {
        Brain = new Brain(this, blackList);

        PersonPot.Control.RestartControllerForPerson(MyId);
    }

    private void InitGeneralStuff()
    {
        NameTransform();

        //_randomUV = new RandomUV(Geometry.gameObject, H.Person);
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


    /// <summary>
    /// Static so they respect not repeating names 
    /// </summary>
    static Naming namingMale = new Naming(H.Male);
    static Naming namingFeMale = new Naming(H.Female);

    string GiveMeMyName()
    {
        if (Gender == H.Male)
        {
            return namingMale.NewName();
        }
        return namingFeMale.NewName();
    }


    /// <summary>
    /// Used by crystals and other to get an unique ID
    /// </summary>
    /// <returns></returns>
    public static string GiveRandomID()
    {
        Naming n = new Naming(H.Male);
        int numb = GiveRandom(0, 10000);
        return n.NewName() + "." + numb;
    }

    public static string GiveRandomName()
    {
        int numb = GiveRandom(0, 10);
        return GiveRandomID() + "." + numb;
    }


    private int secCount;
    private Vector3 originalPoint;
    /// <summary>
    /// Returns Random position from origin. If fell inside a building will find another spot
    /// until is in a clear zone
    /// If origin is not specified will assume is CamControl.CAMRTS.hitFront.point the center of terrain
    /// </summary>
    /// <param name="howFar">How far will go</param>
    public Vector3 AssignRandomIniPosition(Vector3 origin = new Vector3(), float howFar = 1)
    {
        if (origin == new Vector3())
        {
            origin = ReturnIniPos();
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
        //will add one unit to how far so can move further
        howFar += 0.1f;

        if (MeshController.CrystalManager1.IntersectAnyLine(ReturnIniPos(), origin) || !IsOnTerrain(origin))
        {
            secCount++;
            if (secCount > 1000)
            {
                throw new Exception("Infinite loop");
            }
            origin = AssignRandomIniPosition(origin, howFar);
        }

        originalPoint = new Vector3();
        secCount = 0;
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
        return ReturnFirstDockOrFirstStorage();
    }

    Vector3 ReturnFirstDockOrFirstStorage()
    {
        var build = BuildingPot.Control.Registro.ReturnFirstThatContains("Dock");

        if (build != null)
        {
            StartingBuild = build.MyId;
            return build.SpawnPoint.transform.position;
        }
        build = BuildingPot.Control.Registro.ReturnFirstThatContains("Storage");
        if (build != null)
        {
            StartingBuild = build.MyId;

            return build.SpawnPoint.transform.position;
        }
        //all houses shoud contain House 
        //if bug null ref
        //is bz the buildigns houses are all shacks
        //or still on dev 
        build = BuildingPot.Control.Registro.ReturnFirstThatContains("House");
        if (build != null)
        {
            StartingBuild = build.MyId;

            return build.SpawnPoint.transform.position;
        }
        //just here bz small town still not spwaning
        //todo remove after small town is implemented 
        return m.IniTerr.MathCenter;
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

        if (Mathf.Abs(terrCenter.y - hit) < 0.1f)
        {
            return true;
        }

        return false;
    }

    #endregion

    #region Person Nutrition

    MDate _lastWater;
    void CheckOnNutritionAndThirst()
    {
        KillStarve();

        if (_lastWater == null)
        {
            _thirst = "Low";
        }
        else if (Program.gameScene.GameTime1.ElapsedDateInDaysToDate(_lastWater) > 60)
        {
            _thirst = "Low";
        }
    }



    /// <summary>
    /// Of _nutrition level is so low will kill person
    /// </summary>
    void KillStarve()
    {
        if (_nutritionLevel == "Starve")
        {
            ChangeHappinesBy(-1);//   -5
            //UpdateInfo("Starving");
        }
        if (_nutritionLevel == "Dead")//-545   -345
        {
            if (string.IsNullOrEmpty(IsBooked))
            {
                print(MyId + " Too hungry and died:" + " major:" + IsMajor + " spouse:" + Spouse);
                ActionOfDisappear();
            }
            else
            {
                print(MyId + " Cant die bz booked " + " major:" + IsMajor + " spouse:" + Spouse);
            }
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

        GetPaid();
        AgeAction();

        ChangeHappinesBy(Home.Comfort);

        CheckHappiness();
        DidIDie();
        CheckIfEmmigrate();
        CheckIfInSchool();
        AddHappyForVarietyOfFoods();

        Program.MouseListener.MStatsAndAchievements.CheckOnManualAchievements(years: Age);
        UseMyFairAmt();

        if (ProfessionProp != null)
        {
            //so it relecfts new age 
            ProfessionProp.ProdXShift = 0;
        }
    }

    private void UseMyFairAmt()
    {
        GameController.ResumenInventory1.Remove(P.Cloth, .03f);
        GameController.ResumenInventory1.Remove(P.Utensil, .001f);
        GameController.ResumenInventory1.Remove(P.Crockery, .02f);
        GameController.ResumenInventory1.Remove(P.Furniture, .01f);
    }



    public override void DestroyCool()
    {
        MyDummy.Destroy();
        MyDummyProf.Destroy();
        base.DestroyCool();
    }

    private void AddHappyForVarietyOfFoods()
    {
        if (Home == null)
        {
            return;
        }

        var amtDiffFoods = Home.Inventory.FoodCatItems.Count;
        ChangeHappinesBy(amtDiffFoods * 0.3f);
    }

    /// <summary>
    /// If is in school will add to the Person Another year on school
    /// </summary>
    private void CheckIfInSchool()
    {
        if (Work != null && Work.HType.ToString().Contains("School") && !UPerson.IsMajor(Age))
        {
            YearsOfSchool++;
        }
    }

    /// <summary>
    /// The actions of of age in a BD
    /// </summary>
    void AgeAction()
    {
        _age++;
        Body.GrowScaleByYears();

#if UNITY_EDITOR
        //Debug.Log("Name trans");
        NameTransform();//so it reflects the new Age
#endif
    }

    void CheckHappiness()
    {
        if (Happinnes < 2f)//.5
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
        var currYear = Program.gameScene.GameTimePeople.Year;

        if (_lastBDYear != currYear && _birthMonth <= Program.gameScene.GameTimePeople.Month1)
        {
            _lastBDYear = currYear;
            return true;
        }
        return false;
    }

    //bool IsMyBD()
    //{
    //    var currYear = Program.gameScene.GameTime1.Year;

    //    if (_lastBDYear != currYear && _birthMonth == Program.gameScene.GameTime1.Month1)
    //    {
    //        _lastBDYear = currYear;
    //        return true;
    //    }
    //    return false;
    //}

    public PersonReport PersonReport = new PersonReport();
    /// <summary>
    /// Will contain all the functions to execute for a person when reach the Age Majority 
    /// 
    /// Call once a birthday if is old enough until, _isMajor = true
    /// </summary>
    void ReachAgeMajority()
    {
        //the family ID of the place he is gonna be booked
        //will be empty if is Virgin Family
        var newFamilyID = "";//will be returned below 
        //wnna keep it  here bz will be lost once past PlaceWhereIWillLiveToLive()
        //dont really needed bz the new one is stored in 'newFamilyID' the old one is needed
        //to be removed from it 
        var myOldFamID = FamilyId;

        var place = PlaceWhereIWillLiveToLive(ref newFamilyID);
        //will only will mark as majority age reached if he could fit a house 
        if (place != null)
        {
            FamilyId = myOldFamID;
            RemoveMeFromOldHome();
            PeopleDictMatters(place);

            Brain.Realtor1.BookNewPersonInNewHome(this, place, newFamilyID);

            _isMajor = true;
            Brain.MajorAge.MarkMajorityAgeReached();
            PersonPot.Control.IsAPersonHomeLessNow = MyId;

            //print("Age Major: " + MyId);

            //needed here so can addres the new stuff
            Brain.CheckConditions();
        }
    }

    void PeopleDictMatters(Building newPlace)
    {
        //dont need to remove if new place is same as current home 
        if (Home != null && newPlace == Home)
        {
            return;
        }

        //removing person from Home PeopleDict here 
        Home.PeopleDict.Remove(MyId);
        BuildingPot.Control.Registro.ResaveOnRegistro(Home.MyId);

    }

    /// <summary>
    /// Will retrun true if a house was find that can fit a Adult 
    /// </summary>
    /// <returns></returns>
    private Building PlaceWhereIWillLiveToLive(ref string familyID)
    {
        //means that anotehr person is ins the process of finding a new home 
        if (!string.IsNullOrEmpty(PersonPot.Control.IsAPersonHomeLessNow))
        {
            return null;
        }

        var buildings = UBuilding.ReturnBuildings(BuildingPot.Control.HousesWithSpace);

        for (int i = 0; i < buildings.Count; i++)
        {
            if ((buildings[i].BookedHome1 == null || buildings[i].BookedHome1.MySpouseBooked(Spouse) ||
                !buildings[i].BookedHome1.IsBooked())
                &&
                buildings[i].WouldAdultFitInThisHouseInAFamily(this, ref familyID)
                && !Brain.BlackList.Contains(buildings[i].MyId))
            {
                return buildings[i];
            }
        }

        return null;
    }

    /// <summary>
    /// bz when new adult needs to get out of old home .
    /// </summary>
    void RemoveMeFromOldHome()
    {
        if (Home != null)
        {
            var family = Home.FindMyFamilyChecksFamID(this);

            if (family == null)
            {
                family = Home.FindMyFamily(this);
            }

            if (family != null)
            {
                family.RemovePersonFromFamily(this);
            }
            else
            {
                FindMeInAllFamiliesAndRemoveMeFromMine();
                throw new Exception("family null:" + MyId);
            }

            Brain.MoveToNewHome.OldHomeKey = "";//so he doesnt pull that family as its old family when creating Shack or moving to new home 
            BuildingPot.Control.AddToHousesWithSpace(Home.MyId);

            //so families are resaved 
            BuildingPot.Control.Registro.ResaveOnRegistro(Home.MyId);
        }
    }

    /// <summary>
    /// Last resource when havent found the family of a person
    /// willlook trhu all buildings and see if can find this person in any family and if so will remove it from there 
    /// </summary>
    public Family FindMeInAllFamiliesAndRemoveMeFromMine()
    {
        var fams = BuildingPot.Control.Registro.AllFamilies();
        var myFam = fams.Find(a => a.Kids.Contains(MyId) || a.Father == MyId || a.Mother == MyId);

        if (myFam != null)
        {
            myFam.RemovePersonFromFamily(this);
            return myFam;
        }
        return null;
    }

    /// <summary>
    /// Will check if person passed its _lifeLimit
    /// </summary>
    private void DidIDie()
    {
        if (Age > _lifeLimit && !IsPregnant)
        {
            //print(MyId + " gone , se partio.To old" + " home:" + Home.MyId);

            var noti = "PersonDie";
            if (Work != null && MyText.Lazy() > 0)
            {
                noti = "DieReplacementFound";
            }
            else if (Work != null && MyText.Lazy() == 0)
            {
                noti = "DieReplacementNotFound";
            }

            Program.gameScene.GameController1.NotificationsManager1.Notify(noti, Name);
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
                Debug.Log("Need to die now but cant find family " + MyId + " Spouse:" + Spouse);
                return;
            }

            sp.IsWidow = true;
        }
        Brain.Die();
    }

    /// <summary>
    /// Only for a person that has Blacklisted a house and dont have where to go back to
    /// </summary>
    void ForcedDeath()
    {
        Debug.Log("ForcedDeath: " + MyId + " Spouse:" + Spouse);

        Brain.Partido = true;
        Brain.Die();
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
        //if is either is book no marriage
        if (!string.IsNullOrEmpty(IsBooked) || !string.IsNullOrEmpty(other.IsBooked))
        {
            return false;
        }

        if (spouse != "") { return false; }
        if (isWidow) { return false; }
        if (other.Gender == Gender) { return false; }
        if (Mathf.Abs(other.Age - _age) > 20) { return false; }

        if (Age < 16 || other.Age < 16)
        {
            return false;
        }

        return true;
    }

    Structure CreateDummy()
    {
        var dummyIdle = (Structure)Building.CreateBuild(Root.dummyBuildWithSpawnPointUnTimed, new Vector3(), H.Dummy,
            container: Program.PersonObjectContainer.transform, name: "DummyUntimed . " + MyId);

        return dummyIdle;
    }

    void CreateTheTwoDummies()
    {
        //mean was created already 
        if (MyDummy != null)
        {
            return;
        }

        MyDummy = CreateDummy();
        MyDummyProf = CreateDummy();
    }

    // Use this for initialization
    void Start()
    {
        if (_nutrition == null)
        {
            _nutrition = new Nutrition(this);
        }

        cam = Camera.main;
        base.Start();

        StartCoroutine("SixtySecUpdate");


        StartCoroutine("FiveSecUpdate");
        StartCoroutine("OneSecUpdate");

        StartCoroutine("EmoticonUpdate");


        //StartCoroutine("A45msUpdate");

        //for body
        //StartCoroutine("A32msUpdate");
        StartCoroutine("A64msUpdate");

        StartCoroutine("RandomUpdate1020");
        //StartCoroutine("QuickUpdate");

        CreateTheTwoDummies();

        CheckQuest();

        if (HType == H.Enemy)
        {
            _militarBrain = new EnemyBrain(this);
        }

        if (PersonBank1 == null)
        {
            PersonBank1 = new PersonBank();
            PersonBank1.SetPerson(this);
        }

        //means is loading from file
        if (Inventory != null && Inventory.InventItems.Count > 0)
        {
            return;
        }
        Inventory = new Inventory(MyId, HType);
    }

    float RestartTimes(float a, float b) { return Random.Range(a, b); }


    void CheckQuest()
    {
        if (PersonPot.Control.All.Count > 49)
        {
            Program.gameScene.QuestManager.QuestFinished("Population50");
        }
    }

    private IEnumerator SixtySecUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(60); // wait
            if (IsMajor && string.IsNullOrEmpty(Spouse) && PersonPot.Control.IsPeopleCheckFull())
            {
                //in a attempt to see if there is a house out there with a single person 
                PersonPot.Control.RestartControllerForPerson(MyId);
            }
        }
    }

    private IEnumerator FiveSecUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(checkFoodElapsed); // wait
            ParentPersonToHome();

            if (HType == H.Person)
            {
                _brain.SlowCheckUp();
            }

            TryHaveKids();
        }
    }


    private bool _wasPersonParented;
    /// <summary>
    /// bz if done before its all weird 
    /// This is useful for when it loads person and when newBorn 
    /// </summary>
    void ParentPersonToHome()
    {
        if (_wasPersonParented || Time.time < 5f || Home == null || transform.parent != null)
        {
            return;
        }
        _wasPersonParented = true;
        transform.SetParent(Home.transform);
    }


    private IEnumerator OneSecUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(3); // wait
            TimeChecks();
        }
    }




    //private IEnumerator A45msUpdate()
    //{
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(.045f); // wait
    //    }
    //}


    //for body
    //private IEnumerator A32msUpdate()
    //{
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(.032f); // wait
    //        _body.A32msUpdate();
    //    }
    //}

    private IEnumerator A64msUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(.064f); // wait
            _body.A64msUpdate();

            if (_showPathToHome != null)
            {
                _showPathToHome.Update();
            }
            if (_showPathToWork != null)
            {
                _showPathToWork.Update();
            }
            if (_showPathToFood != null)
            {
                _showPathToFood.Update();
            }
            if (_showPathToReligion != null)
            {
                _showPathToReligion.Update();
            }
            if (_showPathToChill != null)
            {
                _showPathToChill.Update();
            }
        }
    }





    private float random1020Time;
    private IEnumerator RandomUpdate1020()
    {
        while (true)
        {
            yield return new WaitForSeconds(random1020Time); // wait
            random1020Time = RestartTimes(0.5f, 3f);

#if UNITY_EDITOR
            random1020Time = .1f;
#endif
            if (HType == H.Person)
            {
                _brain.MindState();
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        _levelOfDetail.A45msUpdate();

        //was on update so new PeopleFind their new home 
        if (_home == null && !PersonPot.Control.Locked && HType == H.Person)
        {
            _brain.CheckConditions();
        }

        if (HType == H.Person)
        {
            _brain.Update();
        }

        //defenders
        if (HType == H.Defender)
        {

        }

        //enemy
        if (HType == H.Enemy && _enemyBrain == null)
        {
            _enemyBrain = (EnemyBrain)_militarBrain;
        }
        if (_enemyBrain != null)
        {
            _enemyBrain.Update();
        }


        _body.Update();
        //UpdateInfo();
        if (_profession != null)
        {
            _profession.Update();
        }

        //Majority used to be called in here 
        if (UPerson.IsMajor(_age) && !_isMajor && string.IsNullOrEmpty(IsBooked) //&& Brain.GoMindState 
          && Brain.IAmHomeNow())
        {
            ReachAgeMajority();
            NotifyAgeMajority();
        }
    }

    bool wasNotiAge;
    /// <summary>
    /// Notiftyng for the purpose thaty the personc an work
    /// </summary>
    private void NotifyAgeMajority()
    {
        if (wasNotiAge)
        {
            return;
        }

        //so when loads not notify a lot of people that are older
        if (!wasNotiAge && (IsMajor || Work != null || Age > JobManager.majorityAge + 1))
        {
            wasNotiAge = true;
            return;
        }

        Program.gameScene.GameController1.NotificationsManager1.Notify("AgeMajor", Name);
        wasNotiAge = true;
    }


    public void UpdateInfo(string add = "")
    {
        string homeId = "none";
        if (Home != null) { homeId = Home.MyId; }

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







    #region Bank Money

    private void GetPaid()
    {
        if (Work == null)
        {
            return;
        }
        //only dev can go past 10x,,, this and creates problems with money
        if (Program.gameScene.GameSpeed > 10)
        {
            return;
        }

        var sal = Work.DollarsPay;
        _personBank.Add(sal);
        Program.gameScene.GameController1.Dollars -= sal;
        BulletinWindow.SubBulletinFinance1.FinanceLogger.AddToAcct("Salary", sal);
    }



    private void PayProduct(float amt, P item)
    {
        var trans = Program.gameScene.ExportImport1.CalculateTransaction(item, amt);
        _personBank.WithDraw(trans);

        //if is in positive balance will pay to the Fisco. To the user 
        if (_personBank.CheckingAcct > 0)
        {
            Program.gameScene.GameController1.Dollars += trans;
        }
    }


    #endregion



    #region Profession

    private Profession _profession = new Profession();

    Job _savedJob = Job.None;//everytime a job wht to be found. wht was the last job it will be here
    //can be used to set Prev Job
    private Job _prevJob = Job.None;

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

    public Job SavedJob
    {
        get { return _savedJob; }
        set { _savedJob = value; }
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
            return ReturnJobType(Work);
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

            //caller, broadcaster           //client
            PersonPot.Control.BuildDone += _profession.OnBuildDoneHandler;
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
            //WheelBarrow wB = (WheelBarrow) _profession;
            //wB.DelayedCreatingNew();
            //print("new WheelBarrow:"+MyId);
        }
        else if (jType == Job.Homer)
        {
            _profession = new Homer(this, pF);
            //print("new Homer:" + MyId);
        }
        else if (jType == Job.Farmer)
        {
            _profession = new Farmer(this, pF);
        }
        else if (jType == Job.SaltMiner)
        {
            _profession = new SaltMiner(this, pF);
        }


        PersonPot.Control.RestartControllerForPerson(MyId);
    }

    /// <summary>
    /// Based on the current Work Htype will tell u wihc is the person Job Type
    /// </summary>
    public static Job ReturnJobType(Structure work)
    {
        if (work == null)
        {
            return Job.None;
        }

        if (work.HType == H.LumberMill)
        {
            return Job.Forester;
        }
        else if (work.HType == H.Dock || work.HType == H.Shipyard || work.HType == H.Supplier)
        {
            return Job.Docker;
        }
        else if (work.HType == H.Masonry || work.HType == H.HeavyLoad)
        {
            return Job.Builder;
        }
        else if (work.HType == H.FishingHut)
        {
            return Job.FisherMan;
        }
        else if (work.HType.ToString().Contains(H.Farm.ToString()))
        {
            return Job.Farmer;
        }
        else if (work.HType.ToString().Contains(H.ShoreMine.ToString()))
        {
            return Job.SaltMiner;
        }
        return Job.Insider;
    }

    #endregion

    internal void Kill()
    {
        var t = PersonPot.Control.All.Find(a => a.MyId == MyId);
        //GameScene.print(MyId+".Killed");
        PersonPot.Control.All.Remove(t);
        t.Destroy();
    }

    private MDate _lastTimeHome;
    public void HomeActivities()
    {
        if (Home == null || Home.Inventory == null
            //wont go in the same day
            )
        { return; }

        //needs to go back to work to drop it there 
        if (IsCarryingWorkInputOrder() && !WasFired)//if was fired need to deal with that 
        {
            Brain.ReadyToWork(true);
            return;
        }

        Home.HomeSmokePlay();

        DropFoodAtHome();

        if (_lastTimeHome == null)
        {
            _lastTimeHome = Program.gameScene.GameTime1.CurrentDate();
        }

        if (_lastTimeHome == Program.gameScene.GameTime1.CurrentDate()
            //had to pass at least 20 days in caledar to eat
            //other wise eats all the food really fast this should be called only once is gets home
            || Program.gameScene.GameTime1.ElapsedDateInDaysToDate(_lastTimeHome) < 20)
        {
            return;
        }

        _lastTimeHome = Program.gameScene.GameTime1.CurrentDate();
        EatDrink();

        CheckOnNutritionAndThirst();
    }

    float AdditionalFoodNeeds()
    {
        if (NutritionLevel == "Normal")
        {
            return 0;
        }
        return 6;
    }

    void EatDrink()
    {
        P item = ItemToGetAtHome();
        var kgNeeded = ReturnAmountToEat(item) + AdditionalFoodNeeds();//in case is below normal it needs to eat more 

        if (item == P.Water)
        {
            _thirst = "Quenched";
            kgNeeded = 2;
            _lastWater = Program.gameScene.GameTime1.CurrentDate();
        }

        float gotAmt = Home.Inventory.RemoveByWeight(item, kgNeeded);

        _nutrition.AteThisMuch(item, gotAmt);
        Home.AddConsumeThisYear(item, gotAmt);
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    float ReturnAmountToEat(P prod)
    {
        if (_nutrition == null)
        {
            _nutrition = new Nutrition(this);
        }

        return Math.Abs((_nutrition.HowManyKGINeedOfThisToSupplyMyNeed(prod)));
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
        if (Home == null)
        {
            return;
        }

        TransferInvetoryCat(this, Home, PCat.Food);
        ShouldReloadInventory();
    }

    /// <summary>
    /// Will get food from the 'theFoodSrc'
    /// </summary>
    /// <param name="theFoodSource"></param>
    public void GetFood(Structure theFoodSrc)
    {
        //if there are not crates then cant carry food
        if (!GameController.AreThereCratesOnStorage)
        {
            return;
        }

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

        P item = ItemToGetAtFoodSource(theFoodSrc);

        if (item == P.None)
        {
            return;
        }

        var amt = HowMuchICanCarry();
        PayProduct(amt, item);

        ExchangeInvetoryItem(FoodSource, this, item, amt);
    }


    P ItemToGetAtFoodSource(Structure theFoodSrc)
    {
        if (_thirst == "Quenched")
        {
            return theFoodSrc.Inventory.GiveRandomFood();
        }
        return P.Water;
    }

    P ItemToGetAtHome()
    {
        if (_thirst != "Quenched" && Home.Inventory.Contains(P.Water))
        {
            return P.Water;
        }
        return Home.Inventory.GiveRandomFood();
    }



    public void ExchangeInvetoryItem(General takenFrom, General givenTo, P product, float amt)
    {
        //to address when food Src is destroyed when person on its way 
        if (takenFrom == null)
        { return; }

        if (product.ToString().Contains("Random"))
        {
            DealWithRandom(takenFrom, givenTo, product, amt);
            return;
        }

        if (!Inventory.ThereIsContainerForThis(product))
        {
            Debug.Log("Not cont for:" + product);
            return;
        }

        float amtTook = takenFrom.Inventory.RemoveByWeight(product, amt);
        givenTo.Inventory.Add(product, amtTook);
        Inventory.RemoveContainerUsed(product);
        ShouldReloadInventory();
    }





    public void DropAllInvetoryItems(General takenFrom, General givenTo)
    {
        //to address when food Src is destroyed when person on its way 
        if (takenFrom == null)
        { return; }

        int expC = 0;
        while (takenFrom.Inventory.InventItems.Count > 0)
        {
            P product = takenFrom.Inventory.InventItems[0].Key;
            float amt = takenFrom.Inventory.InventItems[0].Amount;

            OneWayInvExchange(takenFrom, givenTo, product, amt);
            expC++;

            if (expC > 1000)
            {
                throw new Exception("over 100 iterations Droping all goods");
            }
        }
        ShouldReloadInventory();
    }

    void OneWayInvExchange(General takenFrom, General givenTo, P product, float amt)
    {
        if (product.ToString().Contains("Random"))
        {
            DealWithRandom(takenFrom, givenTo, product, amt);
            return;
        }

        float amtTook = takenFrom.Inventory.RemoveByWeight(product, amt);
        givenTo.Inventory.Add(product, amtTook);
        ShouldReloadInventory();
    }

    /// <summary>
    /// If the product is random has to actually decomposed and give that to him 
    /// </summary>
    /// <param name="takenFrom"></param>
    /// <param name="givenTo"></param>
    /// <param name="product"></param>
    /// <param name="amt"></param>
    private void DealWithRandom(General takenFrom, General givenTo, P product, float amt)
    {
        var prdInfo = BuildingPot.Control.ProductionProp.ReturnProdInfoWithOutput(product);
        var listItems = prdInfo.DecomposeRandomLoad(amt);

        for (int i = 0; i < listItems.Count; i++)
        {
            ExchangeInvetoryItem(takenFrom, givenTo, listItems[i].Key, listItems[i].Amount);
        }
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

        var b = (Building)to;
        if (b.Instruction==H.WillBeDestroy)
        {
            from.Inventory.Delete();
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
        Family myFamily = Home.FindMyFamilyChecksFamID(this);

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
        if (string.IsNullOrEmpty(_father) && string.IsNullOrEmpty(_mother))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// How much phisically a person can carry. Like goods .lIke Rice
    /// 
    /// Age + Genre values 
    /// </summary>
    /// <param name="item"></param>
    public float HowMuchICanCarry(float maxNeeded = 100f)
    {
        if (!GameController.AreThereCratesOnStorage)
        {
            //todo notify that people are not carrying bz there is not crates
            return 0;
        }

        var age = AgeFactor();
        var genre = ReturnGenreVal();

        var mul = 0.5f;

        var res = (age + genre) * mul * ProfessionMultiplierCarryWeight();

        if (maxNeeded < res)
        {
            return maxNeeded;
        }
        return res;
    }

    float ProfessionMultiplierCarryWeight()
    {
        var mul = 1;
        if (_body.CanSpawnWheelBarrow())
        {
            mul = 4;
        }
        else if (_body.CanSpawnCart())
        {
            mul = 16;
        }
        return mul;
    }


    int ReturnGenreVal()
    {
        if (Gender == H.Male)
        {
            return 15;
        }
        return 10;
    }

    int AgeFactor()
    {
        if (Age > 10 && Age <= 17)
        {
            return 3;
        }
        if (Age > 17 && Age <= 40)
        {
            return 15;
        }
        if (Age >= 41 && Age <= 60)
        {
            return 11;
        }
        if (Age >= 61 && Age <= 80)
        {
            return 8;
        }
        return 2;
    }

    ///// <summary>
    ///// Will tell u how much u will need of a tpye of food to be fed to 100 %
    ///// </summary>
    ///// <param name="item">the type of food</param>
    ///// <returns></returns>
    //private float HowMuchINeedToBe100PointsFeed(P item)
    //{
    //    var nutriValue = BuildingPot.Control.ProductionProp.Food1.FindNutritionValue(item).NutritionVal;
    //    var nutriNeed = 100 - _nutritionLevel;

    //    return nutriNeed / nutriValue + 1;//+1 is to round up
    //}

    #region Reproduction Having Kids & Stuff

    /// <summary>
    /// The decition of a having a new kid 
    /// </summary>
    /// <returns></returns>
    bool CanIHaveANewKid()
    {
        if (PersonPot.Control.All.Count > GameController.CapMaxPerson)
        {
            return false;
        }

        if (IsPregnant || IsWidow || Gender == H.Male || string.IsNullOrEmpty(Spouse) || Home == null
            || !string.IsNullOrEmpty(IsBooked) || !IsMajor)
        {
            return false;
        }

        if (Home != null && Home.BookedHome1 != null && Home.BookedHome1.IsBooked())
        {
            return false;
        }

        var family = Home.FindMyFamilyChecksFamID(this);
        if (family == null)
        {
            return false;
        }

        bool hasSpace = family.QuickQuuestionCanGetAnotherKid();

        bool nutrida = AmINutrida();
        bool happy = AmIHappy();
        bool bodyReady = Mathf.Abs(_lastNewBornYear - Program.gameScene.GameTimePeople.Year) > 2;

        //have to check Age>14 in case lost both parent really young and was made Family.HouseHeadPerson() on House 
        return nutrida && hasSpace && happy && bodyReady && Age > 14 && Age < 45 && IsSpouseAlive();
    }

    bool IsSpouseAlive()
    {
        var spouseFound = PersonPot.Control.All.Find(a => a.MyId == Spouse);
        return spouseFound != null;
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
        EmoticonManager.Show("Heart", Home.transform.position);

    }

    void CheckIfReadyForGiveBirth()
    {
        //bz is is moving should noy give birth
        if (IsPregnant && IsMyDueDateOrPast() && string.IsNullOrEmpty(IsBooked) && Brain.IAmHomeNow())
        {
            GiveBirth();
            EmoticonManager.Show("Stork", Home.transform.position);
        }
    }

    public string DebugBornInfo;

    void GiveBirth()
    {
        PersonPot.Control.HaveNewKid(Home.SpawnPoint.transform.position);
        Person kid = PersonPot.Control.All[PersonPot.Control.All.Count - 1];
        kid.Mother = MyId;
        kid.Father = Spouse;
        kid.FamilyId = FamilyId;
        kid.IsBooked = Home.MyId;

        MoveNewBornToHome(kid);

        //       //Debug.Log(MyId + " give birth to:" + kid.MyId+". inscribed on:"+FamilyId);
        kid.DebugBornInfo = FamilyId + ".home:" + Home.MyId + ".mom:" + MyId;

        _lastNewBornYear = _dueYear;
        _dueMonth = 0;
        _dueYear = 0;
        IsPregnant = false;

        Happinnes = 5;
        var spouseLo = Family.FindPerson(Spouse);

        //in case spoise die during preganancy
        if (spouseLo != null)
        {
            spouseLo.Happinnes = 5;
        }
        PersonPot.Control.RestartControllerForPerson(MyId);

    }

    /// <summary>
    /// Takes care of the actions needed to move the newBorn to Home 
    /// </summary>
    /// <param name="newBorn"></param>
    void MoveNewBornToHome(Person newBorn)
    {
        var family = Home.FindMyFamilyChecksFamID(this);
        family.AddKids(newBorn.MyId);

        newBorn.IsBooked = "";

        //will be addressed on Body.Update 
        //newBorn.transform.SetParent( Home.transform;

        newBorn.Home = Home;
        newBorn.Brain.SetNewHouseFound();
    }

    /// <summary>
    /// Will return true if is due date 
    /// </summary>
    /// <returns></returns>
    bool IsMyDueDateOrPast()
    {
        if (_dueMonth <= Program.gameScene.GameTimePeople.Month1
            && _dueYear <= Program.gameScene.GameTimePeople.Year)
        {
            return true;
        }
        if (_dueYear < Program.gameScene.GameTimePeople.Year)
        //so in case it missed bz moving to newer home took forever will deliver as soon get to new Place 
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Will calcultae the due date of a woman 
    /// 
    /// Will set _dueMonth and _dueYear
    /// </summary>
    private void CalculateDueDate()
    {
        var currMonth = Program.gameScene.GameTimePeople.Month1;
        _dueMonth = UMath.GoAround(9, currMonth, 1, 12);

        if (_dueMonth < currMonth)
        {
            _dueYear = Program.gameScene.GameTimePeople.Year + 1;
        }
        else
        {
            _dueYear = Program.gameScene.GameTimePeople.Year;
        }
    }

    /// <summary>
    /// Will tell u if person is well fed. Nutrition wise
    /// </summary>
    /// <returns></returns>
    private bool AmINutrida()
    {
        return NutritionLevel == "Normal";
    }

    #endregion

    /// <summary>
    /// Every BD will ask if wnt to emmigrate 
    /// </summary>
    void CheckIfEmmigrate()
    {
        if (_unHappyYears > 2 && !Brain.Partido
            && IsMajor && string.IsNullOrEmpty(IsBooked))
        {
            //Emmigrate();
        }
    }

    void Emmigrate()
    {
        //EmmigrateWithFamily();
        ActionOfDisappear();
        print(MyId + " emmigrated");
        // The peploe had emmigrated they will talk about your port wherever they are 
        PersonPot.Control.EmigrateController1.AddEmigrate(this);

        Program.gameScene.GameController1.NotificationsManager1.Notify("Emigrate");

        //AudioCollector.PlayOneShot("Emigrated", transform.position);
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

        var fam = homeH.FindMyFamilyChecksFamID(this);
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









    #region OutOfScreen






    #endregion
    #region LOD

    private LevelOfDetail _levelOfDetail;
    public LevelOfDetail LevelOfDetail1
    {
        get { return _levelOfDetail; }
    }

    public void StartLOD()
    {
        _levelOfDetail = new LevelOfDetail(this);
    }


    #endregion






    #region Bad Ass

    public bool ImITheSelectedPerson()
    {
        return _light != null;
    }


    float _oldFOV;
    internal void SelectPerson()
    {
        CreateProjector();
        AudioCollector.PlayPersonVoice(this);

        //follow citizen and camera FOV
        //InputRTS.IsFollowingPersonNow = true;
        //_oldFOV = CamControl.CAMRTS.GetComponent<Camera>().fieldOfView;
        //CamControl.CAMRTS.GetComponent<Camera>().fieldOfView = 30f;
    }

    /// <summary>
    /// This is here bz when a building is unselected the proj has to be destroyed
    /// </summary>
    public void UnselectPerson()
    {
        DestroyProjector();
        HidePaths();

        //InputRTS.IsFollowingPersonNow = false;
        //CamControl.CAMRTS.GetComponent<Camera>().fieldOfView = _oldFOV;


        //for militar brain
        if (Brain == null)
        {
            return;
        }

        //if is Dead not point to continue
        if (Brain.Partido)
        {
            return;
        }

        var index = Brain.BlackList.FindIndex(a => a.Contains("RedoBrain"));
        //then we have that instruction there . Placed by brain
        if (index != -1)
        {
            Brain.BlackList.RemoveAt(index);
            RedoBrain(Brain.BlackList);
        }
    }



    private General _projector;
    private General _light;
    private General _reachArea;

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
            Vector3 projNewP = new Vector3(0, 6, 0) + transform.position;
            Projector = Create(Root.projectorPerson, projNewP, container: transform);
            Projector.transform.Rotate(new Vector3(90, 0, 0));

            _light = Create(Root.lightCilPerson, transform.position, container: transform);

            _reachArea = Create(Root.reachAreaFilled, transform.position, container: transform);
            // *2 bz is from where the person is at so 'Brain.Maxdistance' is a  Radius
            _reachArea.transform.localScale = new Vector3(2, 0.1f, 2);

        }
    }

    void DestroyProjector()
    {
        if (_light != null)
        {
            _projector.Destroy();
            _projector = null;

            _light.Destroy();
            _light = null;

            _reachArea.Destroy();
            _reachArea = null;
        }
    }
    #endregion


    #region Events

    private Camera cam;
    internal void CheckMouseClicked()
    {
        return;

        if (!LevelOfDetail1.OutOfScreen1.OnScreenRenderNow || cam == null)
        {
            return;
        }

        //bz _body.CurrentPosition is on the bottom of person 
        var posCorrectedY = new Vector3(_body.CurrentPosition.x, _body.CurrentPosition.y + 0.1f, _body.CurrentPosition.z);
        var scrnPos = cam.WorldToViewportPoint(posCorrectedY);
        var ms = cam.ScreenToViewportPoint(Input.mousePosition);

        var posCorrectedY2 = new Vector3(_body.CurrentPosition.x, _body.CurrentPosition.y + 0.35f, _body.CurrentPosition.z);
        var scrnPos2 = cam.WorldToViewportPoint(posCorrectedY2);


        var dist = Vector2.Distance(scrnPos, ms);
        var dist2 = Vector2.Distance(scrnPos2, ms);

        if (dist + dist2 < 0.1f)
        {
            Debug.Log(MyId + " was this close to click:" + dist);
        }
    }
    #endregion

    /// <summary>
    /// When a person was moving out to a new Building as they reach their majority
    /// </summary>
    internal void RollBackMajority()
    {
        Debug.Log("RollingBack: " + MyId);

        //the one was blackListed
        Home.PeopleDict.Remove(MyId);
        Home.BookedHome1.ClearBooking();
        Home.BookedHome1 = null;

        var fam = Home.FindMyFamilyChecksFamID(this);
        fam.RemovePersonFromFamily(this);
        IsBooked = "";

        var homeIsGoingBackTo = Brain.GetBuildingFromKey(Brain.MoveToNewHome.OldHomeKey);

        //a person tht immigrate or spawned at initial game 
        if (homeIsGoingBackTo == null)
        {
            ForcedDeath();
            return;
        }

        FamilyId = homeIsGoingBackTo.Families[0].FamilyId;

        //even if doesnt fit. he needs to get back to his house of origin
        homeIsGoingBackTo.Families[0].AddKids(MyId);
        Body.GoingTo = HPers.None;

        Home = homeIsGoingBackTo as Structure;
        transform.SetParent(Home.transform);

        IsMajor = false;
        Brain.MajorAge.RollBackMoajority();

        RedoBrain(Brain.BlackList);
    }


    #region Showing Path

    /// <summary>
    /// They all need a function calling its Update() 
    /// </summary>
    private ShowPathTo _showPathToHome;
    private ShowPathTo _showPathToWork;
    private ShowPathTo _showPathToFood;
    private ShowPathTo _showPathToReligion;
    private ShowPathTo _showPathToChill;

    internal void ToggleShowPath(string which)
    {
        InitShowPath(which);

        if (_showPathToHome != null && which == "Home")
        {
            _showPathToHome.Toggle(which);
        }
        if (_showPathToWork != null && which == "Work")
        {
            _showPathToWork.Toggle(which);
        }
        if (_showPathToFood != null && which == "Food Source")
        {
            _showPathToFood.Toggle(which);
        }
        if (_showPathToReligion != null && which == "Religion")
        {
            _showPathToReligion.Toggle(which);
        }
        if (_showPathToChill != null && which == "Chill")
        {
            _showPathToChill.Toggle(which);
        }
    }

    void InitShowPath(string which)
    {
        if (_showPathToHome == null && which == "Home" && Home != null)
        {
            _showPathToHome = new ShowPathTo(this, "Home");
        }
        if (_showPathToWork == null && which == "Work" && Work != null)
        {
            _showPathToWork = new ShowPathTo(this, "Work");
        }
        if (_showPathToFood == null && which == "Food Source" && FoodSource != null)
        {
            _showPathToFood = new ShowPathTo(this, "Food Source");
        }
        if (_showPathToReligion == null && which == "Religion" && Religion != null)
        {
            _showPathToReligion = new ShowPathTo(this, "Religion");
        }
        if (_showPathToChill == null && which == "Chill" && Chill != null)
        {
            _showPathToChill = new ShowPathTo(this, "Chill");
        }
    }


    private void HidePaths()
    {
        if (_showPathToHome != null)
        {
            _showPathToHome.Hide();
        }
        if (_showPathToWork != null)
        {
            _showPathToWork.Hide();
        }
        if (_showPathToFood != null)
        {
            _showPathToFood.Hide();
        }
        if (_showPathToReligion != null)
        {
            _showPathToReligion.Hide();
        }
        if (_showPathToChill != null)
        {
            _showPathToChill.Hide();
        }
    }


    public void DestroyPaths()
    {
        if (_showPathToHome != null)
        {
            _showPathToHome.Destroy();
        }
        if (_showPathToWork != null)
        {
            _showPathToWork.Destroy();
        }
        if (_showPathToFood != null)
        {
            _showPathToFood.Destroy();
        }
        if (_showPathToReligion != null)
        {
            _showPathToReligion.Destroy();
        }
        if (_showPathToChill != null)
        {
            _showPathToChill.Destroy();
        }
    }

    internal void ShowLocationOf(string _key)
    {
        if (_key == "Home" && Home != null)
        {
            CamControl.CAMRTS.InputRts.CenterCamTo(Home.transform);
        }
        if (_key == "Work" && Work != null)
        {
            CamControl.CAMRTS.InputRts.CenterCamTo(Work.transform);
        }
        if (_key == "Food Source" && FoodSource != null)
        {
            CamControl.CAMRTS.InputRts.CenterCamTo(FoodSource.transform);
        }
        if (_key == "Religion" && Religion != null)
        {
            CamControl.CAMRTS.InputRts.CenterCamTo(Religion.transform);
        }
        if (_key == "Chill" && Chill != null)
        {
            CamControl.CAMRTS.InputRts.CenterCamTo(Chill.transform);
        }
    }

    #endregion



    internal void NewWeight(float kgChanged)
    {
        Weight += kgChanged;

        DefineNutritionLevel();
    }

    private void DefineNutritionLevel()
    {
        if (Weight >= NormalWeight())
        {
            NutritionLevel = "Normal";
        }
        else if (Weight >= LowWeight())
        {
            NutritionLevel = "Low";
        }
        else if (Weight >= StarvingWeight())
        {
            NutritionLevel = "Starve";
        }
        else if (Weight < StarvingWeight())
        {
            NutritionLevel = "Dead";
        }
    }

    float WeightBase()
    {
        if (Gender == H.Male)
        {
            return 56.2f;
        }
        return 53.1f;
    }

    float WeightFactor()
    {
        if (Gender == H.Male)
        {
            return 1.41f;
        }
        return 1.36f;
    }

    //D. R. Miller Formula (1983)
    //56.2 kg + 1.41 kg per 2.54cm over 152.4cm       (man)
    //53.1 kg + 1.36 kg per 2.54cm over 152.4cm       (woman)
    float NormalWeight()
    {
        var heightDiff = _height - 152.4f;
        var times = heightDiff / 2.54f;

        return WeightBase() + (WeightFactor() * times);
    }

    float LowWeight()
    {
        var normal = NormalWeight();
        var a15 = NormalWeight() * 0.15f;
        return NormalWeight() - a15;
    }

    float StarvingWeight()
    {
        var normal = NormalWeight();
        var a40 = NormalWeight() * 0.40f;
        return NormalWeight() - a40;
    }





    /// <summary>
    /// in game 0.5f is grown male, so that is 170cm in real life 
    /// </summary>
    internal void InitHeightAndWeight()
    {
        var fact = 170f / 0.5f;
        Height = transform.localScale.x * fact;

        if (Weight == 0)
        {
            Weight = NormalWeight();
        }
    }

    //todo saveload
    public bool WasFired { get; set; }













    #region Hover All Objects. All objects that have a collider will be hoverable

    protected void OnMouseEnter()
    {
        base.OnMouseEnter();
    }

    protected void OnMouseExit()
    {
        base.OnMouseExit();
    }

    #endregion

    #region Emoticon

    bool _toShowEmoticon;
    string _instructionEmoticon;
    float _rEmoTime = 1;
    private IEnumerator EmoticonUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(_rEmoTime); // wait

            if (_toShowEmoticon)
            {
                _rEmoTime = UMath.GiveRandom(0, 4f);
                _toShowEmoticon = false;

                //so they show less icons now only 25% will do so 
                if (UMath.GiveRandom(0, 4) == 0)
                {
                    EmoticonManager.Show(_instructionEmoticon, transform.position);
                }
            }
        }
    }


    internal void BuildPlacedHandler(object sender, EventArgs e)
    {
        _toShowEmoticon = true;
        _instructionEmoticon = "Built";
    }

    internal void BuildWasDemolished(object sender, EventArgs e)
    {
        _toShowEmoticon = true;
        _instructionEmoticon = "Demolish";
    }

    internal void ShowEmotion(string p)
    {
        EmoticonManager.Show(p, transform.position);
    }


    #endregion


    #region Militar
    internal bool IsMilitarNow()
    {
        return _militarBrain != null;
    }

    public void WarHandler(object sender, EventArgs e)
    {
        Debug.Log(Name + " war");
    }



    #endregion



    #region Work Input Orders

    List<Order> _workInputOrders;
    public List<Order> WorkInputOrders
    {
        get
        {
            return _workInputOrders;
        }

        set
        {
            _workInputOrders = value;
        }
    }

    /// <summary>
    /// This orders are input for the work place
    /// 
    /// Once they are back in Storage from work will pick item and store it at home
    /// then once going back to work will take it back 
    /// </summary>
    /// <param name="prodNeed"></param>
    internal void AddWorkInputOrder(Order prodNeed)
    {
        if (_workInputOrders==null)
        {
            _workInputOrders = new List<Order>();
        }

        if (DoIContainThatInputOrderAlready(prodNeed))
        {
            return;
        }
        _workInputOrders.Add(prodNeed);
    }

    /// <summary>
    /// If an order from the same place and same prod was placed before dont need to place it again 
    /// </summary>
    /// <param name="ord"></param>
    /// <returns></returns>
    bool DoIContainThatInputOrderAlready(Order ord)
    {
        var found = _workInputOrders.FindIndex(a => a.DestinyBuild == ord.DestinyBuild && a.Product == ord.Product);
        return found != -1;
    }

    public bool DoesHasInputOrders()
    {
        return _workInputOrders != null && _workInputOrders.Count > 0;
    }

    public Order ReturnFirstOrder()
    {
        if (Work == null || _workInputOrders == null)
        {
            return null;
        }

        if (DoesHasInputOrders())
        {
            if (_workInputOrders[0].DestinyBuild == Work.MyId)
            {
                return _workInputOrders[0];
            }
            else
            {
                _workInputOrders.RemoveAt(0);
            }
        }
        return null;
    }

    public void AddToOrdersCompleted(float amt)
    {
        var ord = ReturnFirstOrder();
        if (ord == null)
        {
            return;
        }

        ord.AddToFullFilled(amt);
        if (ord.IsCompleted)
        {
            _workInputOrders.RemoveAt(0);
        }
    }

    internal bool IsCarryingWorkInputOrder()
    {
        if (!Inventory.IsEmpty() && DoesHasInputOrders())
        {
            var ord = ReturnFirstOrder();
            if (ord!=null)
            {
                return Inventory.IsItemOnInv(ord.Product);
            }
        }
        return false;
    }

    #endregion
}

public class PersonReport
{
    public string whoGreenMeToBecomeMajor;
    public string whoDeniedMeAHouse;
}
