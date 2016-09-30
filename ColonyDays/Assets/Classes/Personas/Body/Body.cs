using System;
using UnityEngine;
using System.Collections.Generic;

public class Body //: MonoBehaviour //: General
{
    private bool _movingNow;//says if body is moving 
    private int _currentRoutePoint;
    private int iniRoutePoint;//so i can access the first point of a route
    private int lastRoutePoint;

    TheRoute _currTheRoute = new TheRoute();
    private List<CheckPoint> _routePoins = new List<CheckPoint>();
    
    private int sign = 1;
    //the speed local of this player they should be all at the same but can be used to slow down 
    //or speed up temporarily
    private float _speed = .5f;
 //   private Person _person;
    Person _person;
    private HPers _location = HPers.None;//curr loc
    private HPers _goingTo=HPers.None;//going to location
    bool _inverse;//inverse route

    private Router dummyRouter;

    HPers _whichRoute;//which route we are using currentlu only iddle is using this 

    Vector3 _loadedPosition = new Vector3();//the position was saved the GameObj was at
    Quaternion _loadedRotation = new Quaternion();

    //for save Load current animation of body
    private string _currentAni;
    private string _loadedAni;

    private PersonalObject _personalObject;

    private Vector3 _currentPosition;

    private bool _imARidingAnAnimal;

    public HPers Location
    {
        get { return _location; }
        set
        {
            if (_location == HPers.MovingToNewHome && value != HPers.Restarting)
            {
//                //Debug.Log("Ret Body Location: "+_person.MyId);
            }

            _location = value;
            //_person.Brain.MindState();
        }
    }

    public HPers GoingTo
    {
        get { return _goingTo; }
        set
        {
            if (_person != null && _person.Brain !=null &&
                (_person.Brain.CurrentTask == HPers.MovingToNewHome || !string.IsNullOrEmpty(_person.IsBooked))
                && _goingTo == HPers.Home &&
                value == HPers.None)
            {
                Debug.Log(string.Format("!Body.GoingTo being screw for {0} on person {1}", value, _person.MyId));
                //return;
            }
            _goingTo = value;
        }
    }

    public bool MovingNow
    {
        get { return _movingNow; }
    }

    public int CurrentRoutePoint
    {
        get { return _currentRoutePoint; }
        set { _currentRoutePoint = value; }
    }

    public List<CheckPoint> CurrRoute
    {
        get { return _routePoins; }
        set { _routePoins = value; }
    }

    public bool Inverse
    {
        get { return _inverse; }
        set { _inverse = value; }
    }

    public HPers WhichRoute
    {
        get { return _whichRoute; }
        set { _whichRoute = value; }
    }

    public float Speed
    {
        get { return _speed; }
        set { _speed = value; }
    }

    public string CurrentAni
    {
        get { return _currentAni; }
        set { _currentAni = value; }
    }

    public TheRoute CurrTheRoute
    {
        get { return _currTheRoute; }
        set { _currTheRoute = value; }
    }

    /// <summary>
    /// The current position of the _personGameObject.
    /// 
    /// </summary>
    public Vector3 CurrentPosition
    {
        get
        {
            //if (_currentPosition == new Vector3())
            //{
            //    _currentPosition = _person.gameObject.transform.position;
            //}
            return _currentPosition;
        }
        set { _currentPosition = value; }
    }

    public Body() { }

    public Body(Person person)
    {
        Init(person);

    }

    private PersonFile _pFile;
    /// <summary>
    /// Intended to be used when loading from file
    /// </summary>
    public Body(Person person, PersonFile pF)
    {
        _pFile = pF;
        Init(person);
        LoadPosition();


        Location = pF._body.Location;
        GoingTo = pF._body.GoingTo;
        _movingNow = pF._body.MovingNow;
        
        _routePoins = pF._body.CurrRoute;
        _inverse = pF._body.Inverse;
        _whichRoute = pF._body.WhichRoute;

        _loadedPosition = pF.Position;
        _loadedRotation = pF.Rotation;

        //_person.transform.position = _loadedPosition;
        AssignNewPositionNoQuesition(_loadedPosition);
        _person.transform.rotation = _loadedRotation;

        _loadedAni = pF._body.CurrentAni;


        //if is zero is that is idling in a house 
        if (pF._body.CurrTheRoute.CheckPoints.Count > 0)
        {
            WalkRoutineLoad(pF._body.CurrTheRoute, GoingTo, pF._body.CurrentRoutePoint, _inverse, _whichRoute); 
        }

    }

    public void Init(Person person)
    {
        _person = person;
        _person.StartLOD();
        
        myAnimator = _person.gameObject.GetComponent<Animator>();
        myAnimator.speed = Program.gameScene.GameSpeed;
        SetCurrentAni("isIdle", "isIdle");
        dummyRouter = new Router();
        oldGameSpeed = Program.gameScene.GameSpeed;

        SetScaleByAge();

        renderer = _person.Geometry.GetComponent<Renderer>(); 
    }

	//the yearly grow for each Gender. For this be effective the GameObj scale must
	// be initiated at 0.26f in all axis
    private float maleGrow = 0.01333f;
    private float femaleGrow = 0.01111f;
    /// <summary>
    /// Will set the body scale by Gender to this be effective the GameObj scale must
	/// be initiated at 0.26f in all axis
    /// </summary>
    void SetScaleByAge()
    {
        UnparentPerson();

        var toAdd = 0f;
        var addAmnt = maleGrow;
        if (_person.Gender == H.Female)
        {addAmnt = femaleGrow;}

        int ageHere = _person.Age;
        if (ageHere > 20)
        {ageHere = 20;}

        //starting age is always 2 .. bz thas the calculus i was based on 
        for (int i = 2; i < ageHere + 1; i++)
        {toAdd += addAmnt;}

        AddToBodyScale(toAdd);

        ParentBack();
    }


    private Transform savedParenTransform;
    void UnparentPerson()
    {
        savedParenTransform = _person.transform.parent;
        _person.transform.parent = null;
    }

    void ParentBack()
    {
        _person.transform.parent = savedParenTransform;
        savedParenTransform = null;
    }

    /// <summary>
    /// Will grow the scale of the body by the years pass as param in 'howMany'
    /// </summary>
    /// <param name="howMany"></param>
    public void GrowScaleByYears(int howMany = 1)
    {
        if (_person.Age > 21)
        {
            return;
        }

        float toAdd = 0;

        if (_person.Gender == H.Male)
        {
            toAdd = maleGrow;
        }
        else if (_person.Gender == H.Female)
        {
            toAdd = femaleGrow;
        }

        AddToBodyScale(toAdd);
    }

    /// <summary>
    /// Will add the scale phisically to the body
    /// </summary>
    /// <param name="toAdd"></param>
    void AddToBodyScale(float toAdd)
    {
        var localScale = _person.transform.localScale;
        var singleS = localScale.x + toAdd;
        var newScale = new Vector3(singleS, singleS, singleS);
        _person.transform.localScale = newScale;
        //print(singleS + ".singleS");
    }

    private Animator myAnimator;
    private string savedAnimation = "";//in case an animation was passed and the animators was disabled will be stored
    //until is enabled
    /// <summary>
    /// Here is when u set the new Animation
    /// </summary>
    /// <param name="animationPass"></param>
    /// <param name="oldAnimation"></param>
    public void SetCurrentAni(string animationPass, string oldAnimation)
    {
        if (!myAnimator.enabled)
        {
            savedAnimation = animationPass;
            return;
        }

        if (string.IsNullOrEmpty(animationPass))
        {
            return;
        }


        //CheckIfNeedsToActivateAniCheck(animationPass, oldAnimation);
        //CleanIfNewAni(animationPass);


        savedAnimation = "";

        
        
        _currentAni = animationPass;
        myAnimator.SetBool(animationPass, true);

        //otherwise will stop the one intended to be playing now 
        if (_currentAni != oldAnimation)
        {
            myAnimator.SetBool(oldAnimation, false);
        }

        //Debug.Log(_person.Name + " " + animationPass);

        if (_personalObject == null)
        {
            _personalObject = new PersonalObject(_person);
        }
        _personalObject.AddressNewAni(animationPass, ShouldHide());
        AddressNewAniSound();
    }








    public void TurnCurrentAniAndStartNew(string animationPass)
    {
        ////Debug.Log("TurnCurrent nw:" + animationPass + ".old:" + _currentAni);
        SetCurrentAni(animationPass, _currentAni);
    }
    
    /// <summary>
    /// Walkable animations so far:
    /// Walk, Carry
    /// Carry is way slower thatn walk
    /// </summary>
    private void DefineSpeed()
    {
        var aniToEval = FindAnimationToEvalSpeed();
        if (aniToEval == "isCarry")
        {
            _speed = UMath.GiveRandom(0.09f, 0.12f);
        }
        else if (aniToEval == "isWheelBarrow")
        {
            _speed = UMath.GiveRandom(0.49f, 0.59f);
        } 
        else if (aniToEval == "isCartRide")
        {
            _speed = UMath.GiveRandom(0.1f, 0.11f);
        }
        else
        {
            _speed = UMath.GiveRandom(0.45f, 0.55f);
        }
        _speed = _speed*CorrectSpeedPeopleAge();
        //bz the speed changes and then looks bad 
        ReCalculateWalkStep();
    }

    /// <summary>
    /// Will correct speed based on age
    /// </summary>
    /// <returns></returns>
    float CorrectSpeedPeopleAge()
    {
        if (_person.Age > 0 && _person.Age <= 45)
        {
            return 1;
        }
        else if (_person.Age > 45 && _person.Age <= 65)
        {
            return .8f;
        }
        else if (_person.Age > 65 && _person.Age <= 75)
        {
            return .7f;
        } 
        return .6f;
    }

    /// <summary>
    /// Bz if saved need to put that Speed
    /// </summary>
    /// <returns></returns>
    string FindAnimationToEvalSpeed()
    {
        if (!string.IsNullOrEmpty(savedAnimation))
        {
            return savedAnimation;
        }
        return _currentAni;
    }


    private void DefineAnimation()
    {
        if (!string.IsNullOrEmpty(_loadedAni))
        {
            return;
        }

        if (!_person.Inventory.IsEmpty() && _loadedAni != "isCarry")
        {
            //defines _loadedAni so will be taken care of in InitWalk
            _loadedAni = "isCarry";
        }
    }

    /// <summary>
    /// Init the Variables to walk
	/// 
	/// If loading from file loadCurrentPoint will be specified 
    /// </summary>
    /// <param name="route">The route to be walked</param>
    /// <param name="inverse">If we are coming back from the route</param>
    /// <param name="loadCurrentPoint">Use to load person last aprox position </param>
    void InitWalk(TheRoute route, bool inverse, int loadCurrentPoint = -1 )
    {
        DefineAnimation();
        FindIfAAniIsSaved();

        DefineSpeed();

   

        _inverse = inverse;
        _currTheRoute = route;
        _routePoins = route.CheckPoints;

        if (inverse)
        {
            sign = -1;
            lastRoutePoint = 0;
            DefineCurrentRoutePoint(loadCurrentPoint, true);

            SetInitRoutePoint();
            DefineInversedRouteRot();
            _person.transform.rotation = _routePoins[iniRoutePoint].QuaterniRotationInv;
        }
        else
        {
            sign = 1;
            lastRoutePoint = _routePoins.Count - 1;
            DefineCurrentRoutePoint(loadCurrentPoint, false);

            SetInitRoutePoint();
            //GameScene.print("iniRoutePoint:" + iniRoutePoint + ".Count:" + _currRoute.Count );
            _person.transform.rotation = _routePoins[iniRoutePoint].QuaterniRotation;
        }
        SetNextPoint();
    }



    /// <summary>
    /// If is a person using a Cart will need to ask for an animal at work
    /// </summary>
    private void AskForAnimalIfNeeded()
    {
        if (_person.Work != null && _person.Work.HType == H.HeavyLoad 
            && _person.ProfessionProp.ProfDescription == Job.WheelBarrow  
            && Location == HPers.Work 
            && GoingTo == HPers.InWork
            && !_imARidingAnAnimal)
        {
            _person.Work.GiveMeAnimal();
            _imARidingAnAnimal = true;
        }
    }

    /// <summary>
    /// If is a person using a Cart will need to ask for an animal at work
    /// </summary>
    private void ReturnAnimalIfNeeded()
    {
        if (_person.Work != null && _person.Work.HType == H.HeavyLoad
            && _person.ProfessionProp.ProfDescription == Job.Homer
            && Location == HPers.InWork
            && GoingTo == HPers.Home
            && _imARidingAnAnimal)
        {
            _person.Work.ReturningBackAnimal();
            _imARidingAnAnimal = false;
        }
    }

    /// <summary>
    /// Address the time where the body is being loaded and wants to load an animation that 
    /// was saved . Other wise will use walk ani
    /// </summary>
    void FindIfAAniIsSaved()
    {
        //_loadedAni == "isIdle" for people that were saved idling.
        //for debug puporses or people that was in the dock waiting to get into 
        //the city as new immigrant 
        if (string.IsNullOrEmpty(_loadedAni) 
            //|| _loadedAni == "isIdle"
            )
        {
            SetCurrentAni("isWalk", "isIdle");
        }
        else
        {
            TurnCurrentAniAndStartNew(_loadedAni);
         //   SetCurrentAni(_loadedAni, "isIdle");
            _loadedAni = "";//so its used only once 
        }
    }

    /// <summary>
    /// Is gonnabe ethier 0 or currentROute.count -1 
    /// 
    /// I created this so is not related to the loading process 
    /// </summary>
    void SetInitRoutePoint()
    {
        //used when is not being loaded from file 
        if (_inverse)
        {
            iniRoutePoint = _routePoins.Count - 1;
        }
        else
        {
            iniRoutePoint = 0;
        }
    }

    /// <summary>
    /// Creates to correct the Load when in Idle ot of range excp
    /// </summary>
    private void CorrectLoadedPoint(int loadCurrentPoint)
    {
        //I need to remove one from the loaded bz when initiating always add a new one 
        if (_whichRoute == HPers.IdleSpot)
        {
            if (_inverse)
            {
                _currentRoutePoint = loadCurrentPoint - sign * 2;
            }
            else
            {
                _currentRoutePoint = loadCurrentPoint - sign;
            }
        }
        else
        {
            _currentRoutePoint = loadCurrentPoint - sign;
        }
    }

    void DefineCurrentRoutePoint(int loadCurrentPoint, bool inverse)
    {
        //if 'loadCurrentPoint' is not -1 means that is being loaded from File 
        if (loadCurrentPoint != -1)
        {
            CorrectLoadedPoint(loadCurrentPoint);
            return;
        }
		//used when is not being loaded from file 
        if (inverse)
        {
            _currentRoutePoint = _routePoins.Count - 1;
        }
        else
        {
            _currentRoutePoint = 0;
        }
    }

    int CorrectBounds(int current, int min, int max)
    {
        if (current < min)
        {
            return min;
        }
        if (_currentRoutePoint > max)
        {
            return max;
        }
        return current;
    }

    /// <summary>
    /// Is here bz the rotation on the reverse Route needs to be corrected again
    /// </summary>
    void DefineInversedRouteRot()
    {
        if (_routePoins[0].InverseWasSet)//means was setup already once
        { return; }
    
        GameScene.dummyBlue.transform.position = _routePoins[_currentRoutePoint].Point;
        _routePoins[0].InverseWasSet = true;//only the first one is marked as bool

        for (int i = _currentRoutePoint; i > 0; i--)
        {
            //so it doesnt tilt when going up or down the brdige hill 
            //im putting in the same height on Y as the next point 
            var nexPos = new Vector3(_routePoins[i].Point.x, _routePoins[i - 1].Point.y, _routePoins[i].Point.z);
            GameScene. dummyBlue.transform.position = nexPos;

            GameScene.dummyBlue.transform.LookAt(_routePoins[i - 1].Point);
            _routePoins[i].QuaterniRotationInv = GameScene.dummyBlue.transform.rotation;
        }

        GameScene.ResetDummyBlue();
        PersonPot.Control.RoutesCache1.AddReplaceRoute(_currTheRoute);
    }

	///Set the next point on the route
    void SetNextPoint()
    {
        //GameScene.print("_currentRoutePoint:" + _currentRoutePoint + ".Count:" + _currRoute.Count +
        //    ".Loaded" + _pFile._body.CurrentRoutePoint);
	    _currentRoutePoint = CorrectBounds(_currentRoutePoint, 0, _routePoins.Count - 1);



        //_person.transform.position = _routePoins[_currentRoutePoint].Point;

        //calling this one bz if is not Render doesnt matter has to be set where it goes in case
        //then gets on Screen and its there doing nothing 
        AssignNewPositionNoQuesition(_routePoins[_currentRoutePoint].Point);



        SetNextPointOverFive();
        _currentRoutePoint += sign;
    }

    /// <summary>
    /// needs to rotate body in first and second point sharply bz since is too fast
    /// dont ave time to pass before is created the gameobj
    /// 
    /// this method in seems that doesnt help when they are coming back... however the workaround
    /// is that since they are inside a building who cares how they caome out from the building for the 
    /// 2nd point on the route,, since all that time was inside the building... another thing is to add
    /// a small idle time in that place and that will make it too... since the problem now is only on the church 
    /// bz doesnt have an idle time over there 
    /// </summary>
    void SetNextPointOverFive()
    {
        if (speedOver5)
        {
            if (!_inverse && (_currentRoutePoint == 0 || _currentRoutePoint == 1))
            {
                _person.transform.rotation = _routePoins[_currentRoutePoint].QuaterniRotation;
            }
            else if (_inverse && (_currentRoutePoint == iniRoutePoint || _currentRoutePoint == iniRoutePoint - 1))
            {
                _person.transform.rotation = _routePoins[_currentRoutePoint].QuaterniRotationInv;
            }
        }
    }

    public void WalkRoutine(TheRoute route, HPers goingTo ,bool inverse = false, HPers whichRouteP = HPers.None)
    {
        InitWalk(route, inverse);
        WalkRoutineTail(goingTo, whichRouteP);

        AskForAnimalIfNeeded();
        ReturnAnimalIfNeeded();
    }



    public void WalkRoutineLoad(TheRoute route, HPers goingTo, int loadInitCurrentPoint,
        bool inverse, HPers whichRouteP)
    {
        InitWalk(route, inverse, loadInitCurrentPoint);
        WalkRoutineTail(goingTo, whichRouteP);

        AskForAnimalIfNeeded();
        ReturnAnimalIfNeeded();
    }

    void WalkRoutineTail(HPers goingTo, HPers whichRouteP = HPers.None)
    {
        GoingTo = goingTo;
        _movingNow = true;
        _whichRoute = whichRouteP;

        AddressWheelBarrowingAni();
    }



    private void AddressWheelBarrowingAni()
    {
        if (CanSpawnWheelBarrow())
        {
            TurnCurrentAniAndStartNew("isWheelBarrow");
            DefineSpeed();
        }
        if (CanSpawnCart())
        {
            TurnCurrentAniAndStartNew("isCartRide");
            DefineSpeed();
        }
    }

    public bool CanSpawnCart()
    {
        if (_person.ProfessionProp == null || _person.Brain == null || _person.Work == null)
        {
            return false;
        }

        var fromWorkToBuildingToPickAmt = Location == HPers.Work && GoingTo == HPers.InWork
            && _person.Brain.CurrentTask == HPers.Working;

        var fromPickingPlaceToDestiny = Location == HPers.InWork && GoingTo == HPers.WheelBarrow
            && _person.Brain.CurrentTask == HPers.Working;

        var fromDestinyBackToWork = Location == HPers.Work && GoingTo == HPers.InWork;


        if (!GameController.AreThereCartsOnStorage)
        {
            return false;
        }

        //so only spawns the WheelBarrow from FoodSrc to dropplace and in its way back 
        if (!fromWorkToBuildingToPickAmt && !fromPickingPlaceToDestiny && !fromDestinyBackToWork)
        {
            return false;
        }

        bool profesion = (_person.ProfessionProp.ProfDescription == Job.WheelBarrow ||
                          _person.PrevJob == Job.WheelBarrow) &&
                         _person.ProfessionProp.ProfDescription != Job.Builder;
        //so prevJob being wheelBarrow and working on a Farm Spawns wheelbarrow
        bool isCurrentCart = profesion &&
            (_person.Work.HType == H.HeavyLoad);

        if (isCurrentCart)
        {
            return true;
        }
        return false;
    }

    public bool CanSpawnWheelBarrow()
    {
        if (_person.ProfessionProp == null || _person.Brain == null || _person.Work == null)
        {
            return false;
        }

        var isNavalWorker = _person.Work.IsNaval();

        var fromWorkToBuildingToPickAmt = Location == HPers.Work && GoingTo == HPers.InWork
            && _person.Brain.CurrentTask == HPers.Working ;

        var fromPickingPlaceToDestiny = Location == HPers.InWork && GoingTo == HPers.WheelBarrow
            && _person.Brain.CurrentTask == HPers.Working;

        var fromDestinyBackToWork = Location == HPers.Work && GoingTo == HPers.InWork
            &&
            ((_person.Brain.CurrentTask == HPers.WheelBarrow && !isNavalWorker) || 
                                                          //so docker doesnt come back with
                                                          //WheelBarow and leave it on Storage
             (_person.Brain.CurrentTask == HPers.None && !isNavalWorker));


        if (!GameController.AreThereWheelBarrowsOnStorage)
        {
            return false;
        }

        if (isNavalWorker)
        {
            return CanNavalWorkerSpawnWheelBarrow();
        }

        //so only spawns the WheelBarrow from FoodSrc to dropplace and in its way back 
        if (!fromWorkToBuildingToPickAmt && !fromPickingPlaceToDestiny && !fromDestinyBackToWork)
        {
            return false;
        }

        bool profesion = (_person.ProfessionProp.ProfDescription == Job.WheelBarrow ||
                          _person.PrevJob == Job.WheelBarrow) &&
                         _person.ProfessionProp.ProfDescription != Job.Builder;
        //so prevJob being wheelBarrow and working on a Farm Spawns wheelbarrow
        bool isCurrentWheelBarrow = profesion &&
            (_person.Work.HType == H.Masonry);

        if (isNavalWorker || isCurrentWheelBarrow)
        {
            return true;
        }
        return false;
    }





    /// <summary>
    /// They will take it from home to everywhere 
    /// 
    /// bz when importinh
    /// </summary>
    /// <returns></returns>
    bool CanNavalWorkerSpawnWheelBarrow()
    {
        if (_person.Brain.PreviousTask == HPers.IdleSpot || GoingTo == HPers.FoodSource || GoingTo == HPers.IdleSpot
            || Location == HPers.FoodSource)
        {
            return false;
        }
        
        return true;
    }


    void InitRotaVars()
    {
        double angle = 0;

        if (_currentRoutePoint + sign < _routePoins.Count && _currentRoutePoint + sign > 0)
        {
            angle = dummyRouter.AngleFrom3PointsInDegrees(_routePoins[_currentRoutePoint - sign].Point,
                _routePoins[_currentRoutePoint].Point, _routePoins[_currentRoutePoint + sign].Point);
        }

        //if is firsr ot last 2,, and angle on the 3 points is more than 80
        if (_currentRoutePoint == iniRoutePoint || _currentRoutePoint == iniRoutePoint + sign ||
            _currentRoutePoint == lastRoutePoint || _currentRoutePoint == lastRoutePoint - sign
             || Mathf.Abs((float)angle) > 80)
        {   //sharp turn values
            distToChangeRot = 0.1f;
            //smoothDivider = 1;
        }
        else
        {
            distToChangeRot = 0.275f;
            //smoothDivider = 4;
        }
    }

    private bool speedOver5;
    //multiplies the step for rotation on ChangeRotation().. is speed is over 5 i make it 10
    //to correct the bugg where would not rotate completly if speed was high 
    private int speedCorrection = 10;
    /// <summary>
    /// Needs to be here to address speeds bigger than 1
    /// </summary>
    void InitRotaVarsOnSpeed()
    {
        speedOver5 = false;
        if (Program.gameScene.GameSpeed > 1 && Program.gameScene.GameSpeed <= 5)
        {
            distToChangeRot = 0.1f;
            speedCorrection = 1;
            //smoothDivider = 1;
        }
        else if (Program.gameScene.GameSpeed > 5)
        {
            speedOver5 = true;
            distToChangeRot = 0.29f;
            speedCorrection = 10;
        }
    }

    private int oldCurrent;//the previus route point
	/// <summary>
    /// The walk handler is being called on Update() if MovingNow = true
    /// </summary>
    void WalkHandler()
    {
        if (oldCurrent != _currentRoutePoint)
        {
            InitRotaVars();
            oldCurrent = _currentRoutePoint;
            InitRotaVarsOnSpeed();
        }

        //CheckRotation();
        if (_routePoins.Count == 0)
        {
            return;
        }

        //correction needed when loading the _idle route inverse.. to avoid out of range excp
        _currentRoutePoint = CorrectBounds(_currentRoutePoint, 0, _routePoins.Count - 1);
        MoveAction();

        Vector3 next = _routePoins[_currentRoutePoint].Point;
        if (UMath.nearEqualByDistance(_currentPosition, next, 0.01f))// 0.001f
        {
            CheckRotation();

            if (next == _routePoins[lastRoutePoint].Point)
            {WalkDone();}
            SetNextPoint();
        }
    }

    /// <summary>
    /// The action of moving the GameObj 
    /// </summary>
    void MoveAction()
    {
        LoadPosition();

        var newPos = Vector3.MoveTowards(_currentPosition, _routePoins[_currentRoutePoint].Point,
            _walkStep);

        AssignNewPosition(newPos);
    }

#region CPU

    private bool isPersonOnScreenRenderNow;
    /// <summary>
    /// For CPU reasons 
    /// </summary>
    /// <param name="newPos"></param>
    void AssignNewPosition(Vector3 newPos)
    {
        _currentPosition = newPos;

        if (_person.LevelOfDetail1.OutOfScreen1.OnScreenRenderNow)
        {
            //will onl;y assign if on screen now 
            _person.transform.position = newPos;
        }
    }

    public void A64msUpdate()
    {


    }




#endregion




    /// <summary>
    /// Created for Loading instances 
    /// </summary>
    /// <param name="newPos"></param>
    private void AssignNewPositionNoQuesition(Vector3 newPos)
    {
        _currentPosition = newPos;
       _person.transform.position = newPos;
    }


    private float _walkStep;
    /// <summary>
    /// for CPU ussage . so this calculation is only done if a param in there changed
    /// </summary>
    void ReCalculateWalkStep()
    {
        CheckOnGameSpeed();
        _walkStep = _speed*Program.gameScene.GameSpeed * 0.02f * FPSCorrection();
    }

    /// <summary>
    /// This corrects the current FPS
    /// 
    /// if is a 60FPS will return a 1, if FPS is at 30 will return 2, FPS:15 ret 4...and so on
    /// </summary>
    /// <returns></returns>
    private float FPSCorrection()
    {
        //avoiding math issues
        if (HUDFPS.FPS() > 60 || HUDFPS.FPS() == 0)//if over 60 then is good to lock it at one bz sometimes happens
            //when game is paused or something then people will go really slow bz in a small portion the 
            //fps was really high ex 120fps when saving 
        {
            //returning 1 doesnt affect the normal step of them 
            return 1;
        }

        //ex 30/60 = 0.5
        var w60 = HUDFPS.FPS()/60;
        //ex 1/0.5 = 2
        return 1/w60;
    }



    public void ChangedSpeedHandler(object sender, EventArgs e)
    {
        //bz somehow will call the dead people already
        if (_person == null)
        {
            return;
        }
        ReCalculateWalkStep();
    }

	/// <summary>
    /// If the _loadedPosition != new Vector3() will load the saved position and rotation
    /// </summary>
    void LoadPosition()
    {
        //if _loadedPosition has a value means that we are loading from file so will move the person to there 
        if (_loadedPosition != new Vector3())
        {
            //GameScene.print(_loadedPosition + "._loadedPosition");

            AssignNewPositionNoQuesition(_loadedPosition);
            //_person.transform.position = _loadedPosition;
            _person.transform.rotation = _loadedRotation;
            _loadedPosition = new Vector3();
        }
    }



    //if dist btw Person and neext point is less than 'param':distToChangeRot we fire ChangeRot()
    private float distToChangeRot = 0.275f;//.299 is the max can be 
    private int smoothDivider = 4;//use to make smooth transition on route points the higher the smoother
    void CheckRotation()
    {
        //correction needed when loading the _idle route inverse.. to avoid out of range excp

        //correction needed when loading the _idle route inverse.. to avoid out of range excp
        _currentRoutePoint = CorrectBounds(_currentRoutePoint, 0, _routePoins.Count - 1);

        //print(currentRoutePoint + ".currentRoutePoint." + currRoute.Count + ".currRoute.Count");
        var currDist = Vector3.Distance(_currentPosition, _routePoins[_currentRoutePoint].Point);
        if (currDist < distToChangeRot)
        {
            ChangeRotation(currDist);
        }

    }

	/// <summary>
    /// Change the rotatation on the GameObj
    /// </summary>
    void ChangeRotation(float currDist)
    {
        var nextPoint = _routePoins[_currentRoutePoint];
        var pastPoint = _routePoins[_currentRoutePoint - sign];
        float diffRotY = 0;

        if (_inverse)
        {
            diffRotY = nextPoint.QuaterniRotationInv.eulerAngles.y - pastPoint.QuaterniRotationInv.eulerAngles.y;//dif on Y rotation btz next and past point
        }
        else
        {
            diffRotY = nextPoint.QuaterniRotation.eulerAngles.y - pastPoint.QuaterniRotation.eulerAngles.y;//dif on Y rotation btz next and past point
        }
        
        var perUnitChange = diffRotY/distToChangeRot;//how much has to change per unit
        var units = distToChangeRot - currDist;//how far had come to the next point
        var finRot = Mathf.Abs(units*perUnitChange) * speedCorrection * Program.gameScene.GameSpeed;

        if (_inverse)
        {
            _person.transform.rotation = Quaternion.RotateTowards(_person.transform.rotation,
         _routePoins[_currentRoutePoint].QuaterniRotationInv, finRot );
        }
        else
        {
            _person.transform.rotation = Quaternion.RotateTowards(_person.transform.rotation,
                // currRoute[currentRoutePoint].QuaterniRotation, (finRot  / smoothDivider)* Program.gameScene.GameSpeed);
            _routePoins[_currentRoutePoint].QuaterniRotation, finRot );
        }
    }

    private float _walkDoneAt;
	//Called when the last point of a route was reached
    void WalkDone()
    {
        Location = GoingTo;
        _movingNow = false;
        SetCurrentAni("isIdle",_currentAni);//_current ani could be walk or carry
        _walkDoneAt = Time.time;

    }
    
    public void DestroyAllPersonalObj()
    {
        _personalObject.DestroyAllGameObjs();
    }


    #region Hide Show
    public void Show()
    {
        renderer.enabled = true;

        if (_personalObject!=null)
        {
            _personalObject.Show();
        }
    }


    public void HideNoQuestion()
    {
        renderer.enabled = false;

        if (_personalObject != null)
        {
            _personalObject.Hide();
        }
    }

    private Renderer renderer;
    public void Hide()
    {
        if (ShouldHide())
        {
            renderer.enabled = false;

            if (_personalObject != null)
            {
                _personalObject.Hide();
            }
        }
    }

    bool ShouldHide()
    {
        //return false;

        if (CurrTheRoute==null )
        {
            return false;
        }

        if (!Inverse && !ContainsOpenAirJob(CurrTheRoute.DestinyKey))
        {
            return true;
        }
        if (Inverse && !ContainsOpenAirJob(CurrTheRoute.OriginKey))
        {
            return true;
        }
        return false;
    }

    bool ContainsOpenAirJob(string key)
    {
        if (key==null)
        {
            return true;
        }

        var foresterAtStillElement = _person.ProfessionProp.ProfDescription == Job.Forester;
        var builderAtConstruction = _person.ProfessionProp.ProfDescription == Job.Builder;

        if (foresterAtStillElement || builderAtConstruction)
        {
            return true;
        }

        if (key.Contains("Dummy") || key.Contains("Fish"))
        {
            return true;
        }
        if (_person.ProfessionProp!=null && _person.ProfessionProp.ProfDescription==Job.Farmer)
        {
            return true;
        }

        return false;
    }


#endregion
	
	// Update is called once per frame
	public void Update ()
    {
	    if (_movingNow)
	    {
            //ThreadPool.RunThis(_person);
	        WalkHandler();
	    }
	    //CheckOnGameSpeed();
        CheckIfGoingIntoBuild();

	CheckSound();
    }



    /// <summary>
    /// If is inside a building will hide or show Geometry
    /// </summary>
    private void CheckIfGoingIntoBuild()
    {
        if (  _person == null || _routePoins == null || _routePoins.Count == 0){ return; }

        var dist = 0.9f;//.2 //.25
        var currDist = Vector3.Distance(_currentPosition, _routePoins[lastRoutePoint].Point);
        //getting close to last point
        if (currDist < dist && CurrentRoutePointIsTheOneBeforeLast()) 
        {
            Hide();
            //not when gonna idle .. other wise will just hide body on miuddle of iddle
            //when is coming back ca be hidden not problem bz will be in the house again 
            if (_whichRoute == HPers.IdleSpot && !_inverse)
            { Show(); }
        }

        //geting close to 2nd point on route
        var index = iniRoutePoint;
        if (!_inverse)
        {
            index += 1;
        }
        else
        {
            index -= 1;
        }

        currDist = Vector3.Distance(_currentPosition, _routePoins[index].Point);
        if (currDist < 0.01f  ){Show();}
    }

    /// <summary>
    /// Needed so when passed close to house and is close to last point doesnt desappear.
    /// so only dispappers in the point before the last 
    /// </summary>
    /// <returns></returns>
    private bool CurrentRoutePointIsTheOneBeforeLast()
    {
        if (_currentRoutePoint  == lastRoutePoint)
        {
            return true;
        }  
        return false;
    }

    /// <summary>
    /// When person get close to Last Point of route is getting into a building and should most
    /// of the time hides 
    /// </summary>
    void GettingCloseToLastPoint()
    {
        var dist = 0.9f;//.2 //.25
        var currDist = Vector3.Distance(_currentPosition, _routePoins[lastRoutePoint].Point);
        //getting close to last point
        if (currDist < dist)
        {
            Hide();
            //not when gonna idle .. other wise will just hide body on miuddle of iddle
            //when is coming back ca be hidden not problem bz will be in the house again 
            if (_whichRoute == HPers.IdleSpot && !_inverse)
            { Show(); }
        }
    }

    /// <summary>
    /// When person is getting out of buiding aprochin the 2nd point on route should show up
    /// </summary>
    void GettingCloseToSecondPoint()
    {
        //geting close to 2nd point on route
        var index = iniRoutePoint;
        if (!_inverse)
        {
            index += 1;
        }
        else
        {
            index -= 1;
        }

        var currDist = Vector3.Distance(_currentPosition, _routePoins[index].Point);
        if (currDist < 0.01f) { Show(); }
    }




    private int oldGameSpeed;//same speed the game is always started at
    /// <summary>
    /// Will change the speed of the animator 
    /// </summary>
    private void CheckOnGameSpeed()
    {
        if (oldGameSpeed != Program.gameScene.GameSpeed)
        {
            myAnimator.speed = Program.gameScene.GameSpeed;
            oldGameSpeed = Program.gameScene.GameSpeed;
        }
    }

    /// <summary>
    /// Used to slow down or speed up speed of the person if person is on the same 
	/// spot that other person	
    /// </summary>
    public void ChangeSpeed(float amt)
    {
        myAnimator.speed += amt;
        _speed += amt;
    }

    internal void UpdatePersonalForWheelBa()
    {
        //need to put isCarry if dont have wheel barrow
        if (!_person.Inventory.IsEmpty() && !GameController.ThereIsAtLeastOneOfThisOnStorage(P.WheelBarrow))
        {
            TurnCurrentAniAndStartNew("isCarry");
            DefineSpeed();
            return;
        }

        _personalObject.AddressNewAni(_currentAni, true);
    }

    public void UpdatePersonalObjAniSpeed()
    {
        DefineIfCarryAni();
    }

    private void DefineIfCarryAni()
    {
        if (_currentAni == "isWheelBarrow")
        {
            return;
        }

        if (!_person.Inventory.IsEmpty() && _currentAni != "isCarry")
        {
            TurnCurrentAniAndStartNew("isCarry");
            DefineSpeed();

            //so its gets show 
            _personalObject.Show();
        }
    }

    /// <summary>
    /// bz when homer droping . and picking new load might always have the same animation 
    /// but the prod he is loading noew is different 
    /// 
    /// Then calls UpdatePersonalObjAniSpeed();
    /// </summary>
    internal void ResetPersonalObject()
    {
        //return;

        //_currentAni = "";
        _personalObject.Reset();

        //to address when person is changing from Wood to Crate
        if (!_person.Inventory.IsEmpty())
        {
            _personalObject.AddressNewAni(_currentAni, false);
        }
    }






    /// <summary>
    /// Created so onces is enable activate the current Ani. Since the Animator might be running another animation
    /// bz was disabled until now 
    /// </summary>
    internal void EnableAnimator()
    {
        myAnimator.enabled = true;
        TurnCurrentAniAndStartNew(savedAnimation);

        //needs to know if is Shown, other wise will show Personal Object without person
        //
        if (_personalObject != null && IAmShown())
        {
            _personalObject.Show();
        }
        DefineSpeed();
    }

    private bool IAmShown()
    {
        return renderer.enabled;
    }




    /// <summary>
    /// Syays if is in first or last of a route 
    /// </summary>
    /// <returns></returns>
    bool AmIOnTheStartOrEnd()
    {
        return _currentRoutePoint == iniRoutePoint || _currentRoutePoint == lastRoutePoint;
    }







    void CheckIfNeedsToActivateAniCheck(string pass, string old)
    {
        var currentBaseState = myAnimator.GetCurrentAnimatorStateInfo(0);

        //if saved animation has a value 
        if (!string.IsNullOrEmpty(pass) && !currentBaseState.IsName(pass) && pass != old)
        {
            isSaveAniCheck = true;
            saveAniToCheck = pass;
        }
    }

    private string saveAniToCheck = "";
    private bool isSaveAniCheck = false;
    /// <summary>
    /// This is gonig to be call every 64ms as long saveAniCheck is true
    /// 
    /// bz sometimes an animation that needs to be played does not get transioned to.
    /// </summary>
    void CheckIfCurrentAnimationIsTheSaved()
    {
        var currentBaseState = myAnimator.GetCurrentAnimatorStateInfo(0);

        //if is the same is all good
        if (string.IsNullOrEmpty(saveAniToCheck) || currentBaseState.IsName(saveAniToCheck))
        {
            var a = 1;
            saveAniToCheck = "";
            isSaveAniCheck = false;
        }
        //if is not needs to be reCall again
        else
        {
            TurnCurrentAniAndStartNew(saveAniToCheck);
        }
    }

    /// <summary>
    /// bz needs to be clean in case a new ani was passed 
    /// </summary>
    /// <param name="newPass"></param>
    void CleanIfNewAni(string newPass)
    {
        if (isSaveAniCheck && newPass != saveAniToCheck)
        {
            isSaveAniCheck = false;
            saveAniToCheck = "";
        }
    }










    public void DisAbleAnimator()
    {
        //if animator is disabled they need to be turn off
        saveAniToCheck = "";
        isSaveAniCheck = false;

        myAnimator.enabled = false;
    }



#region Sounds

    //Time to wait in normal speed to play the animation from the beggining of the animation  
    Dictionary<string, int> _aniDelayToPlaySound = new Dictionary<string, int>()
    {
        //so isHoe has 50 frames. in the frame 19 the animation hits the ground
        //so i will play 
        {"isHoe", 19},
        {"isWheelBarrow", 10},//2 or any point before full
        {"isHammer", 7},
        {"isAxe", 16},
    };   
    
    //total timeof animation
    //must run once again before i can play sound again
    Dictionary<string, int> _aniWholeTime = new Dictionary<string, int>()
    {
        {"isHoe", 50},//the total time of this animation
        {"isWheelBarrow", 37},//37 it is
        {"isHammer", 18},//the total time of this animation
        {"isAxe", 40},//the total time of this animation
    };

    private float timeToPlaySound = -1;
    private void AddressNewAniSound()
    {
        if (!_aniDelayToPlaySound.ContainsKey(CurrentAni))
        {
            timeToPlaySound = -1;
            return;
        }

        var frames = _aniDelayToPlaySound[CurrentAni];
        timeToPlaySound = Time.time + ConvertFramesIntoSeconds(frames);
    }

    float TimeInSecToNextAnimation()
    {
        //if person not on screen now 
        if (!_person.LevelOfDetail1.OutOfScreen1.OnScreenRenderNow || IsHidden())
        {
            return -1;
        }

        var framesToPlayWholeAni = _aniWholeTime[CurrentAni];
        return Time.time + ConvertFramesIntoSeconds(framesToPlayWholeAni);
    }

    private bool IsHidden()
    {
        return renderer.enabled == false;
    }

    float ConvertFramesIntoSeconds(int frames)
    {
        //30FPS becasue animations are played at tht speed
        var time = (float)frames/(float)30;
        //bz if is 2x the speed is going to be half of the time to play
        return time/Program.gameScene.GameSpeed;
    }

    /// <summary>
    /// Called in Update 
    /// </summary>
    void CheckSound()
    {
        if (Time.time > timeToPlaySound && timeToPlaySound != -1)
        {
            var dist = Vector3.Distance(Camera.main.transform.position, _person.transform.position);

            if (dist > 200)
            {
                timeToPlaySound = -1;
                return;
            }

            timeToPlaySound = TimeInSecToNextAnimation();
            AudioCollector.PlayOneShot(CurrentAni, dist);
        }
    }

#endregion
}
