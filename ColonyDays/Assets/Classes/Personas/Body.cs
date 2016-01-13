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

    //if is false will not hide when getting close to building ... use for instances when is not
    //really a building in the end or start of a route. ex routing to a tree
    private bool _hideThisTime = true;

    //for save Load current animation of body
    private string _currentAni;
    private string _loadedAni;

    public HPers Location
    {
        get { return _location; }
        set { _location = value; }
    }

    public HPers GoingTo
    {
        get { return _goingTo; }
        set { _goingTo = value; }
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

    public bool HideThisTime
    {
        get { return _hideThisTime; }
        set { _hideThisTime = value; }
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

        Location = pF._body.Location;
        GoingTo = pF._body.GoingTo;
        _movingNow = pF._body.MovingNow;
        
        _routePoins = pF._body.CurrRoute;
        _inverse = pF._body.Inverse;
        _whichRoute = pF._body.WhichRoute;

        _loadedPosition = pF.Position;
        _loadedRotation = pF.Rotation;
        _hideThisTime = pF._body.HideThisTime;

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
    public void SetCurrentAni(string animationPass, string oldAnimation)
    {
        _currentAni = animationPass;
        myAnimator.SetBool(animationPass, true);
        myAnimator.SetBool(oldAnimation, false);
    }

    public void TurnCurrentAniAndStartNew(string animationPass)
    {
        SetCurrentAni(animationPass, _currentAni);
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
        FindIfAAniIsSaved();
        
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
    /// Address the time where the body is being loaded and wants to load an animation that 
    /// was saved . Other wise will use walk ani
    /// </summary>
    void FindIfAAniIsSaved()
    {
        if (string.IsNullOrEmpty(_loadedAni))
        {
            SetCurrentAni("isWalk", "isIdle");
        }
        else
        {
            SetCurrentAni(_loadedAni, "isIdle");
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
        //PersonPot.Control.RoutesCache1.AddReplaceRoute(_currTheRoute);
    }

	///Set the next point on the route
    void SetNextPoint()
    {
        //GameScene.print("_currentRoutePoint:" + _currentRoutePoint + ".Count:" + _currRoute.Count +
        //    ".Loaded" + _pFile._body.CurrentRoutePoint);
	    _currentRoutePoint = CorrectBounds(_currentRoutePoint, 0, _routePoins.Count - 1);

        //if (_person.Work != null && _person.Work.HType == H.Dock)
        //{
        //    var t = this;
        //    Debug.Log("Moved "+ _person.Name + " to:"  + _routePoins[_currentRoutePoint].Point);
        //}

        _person.transform.position = _routePoins[_currentRoutePoint].Point;

        //if (!_inverse)
        //{
        //    _person.transform.rotation = _routePoins[_currentRoutePoint].QuaterniRotation;     
        //}
        //else
        //{
        //    _person.transform.rotation = _routePoins[_currentRoutePoint].QuaterniRotationInv;
        //}

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

    public void WalkRoutine(TheRoute route, HPers goingTo ,bool inverse = false, HPers whichRouteP = HPers.None,
        bool hideThisTime = true)
    {
        _hideThisTime = hideThisTime;

        if (!hideThisTime)
        {
            Show();//to show person whenh going from old home to shack to be built
        }

        InitWalk(route, inverse);
        WalkRoutineTail(goingTo, whichRouteP);
    }

    public void WalkRoutineLoad(TheRoute route, HPers goingTo, int loadInitCurrentPoint,
        bool inverse, HPers whichRouteP)
    {
        InitWalk(route, inverse, loadInitCurrentPoint);
        WalkRoutineTail(goingTo, whichRouteP);
    }

    void WalkRoutineTail(HPers goingTo, HPers whichRouteP = HPers.None)
    {
        GoingTo = goingTo;
        _movingNow = true;
        _whichRoute = whichRouteP;
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
        CheckRotation();

        MoveAction();

        Vector3 curr = _person.transform.position;
        Vector3 next = _routePoins[_currentRoutePoint].Point;

        if (UMath.nearEqualByDistance(curr, next, 0.01f))// 0.001f
        {
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

        if (_routePoins.Count == 0 || _currentRoutePoint > _routePoins.Count - 1)
        {
            var t = this;
        }

        _person.transform.position = Vector3.MoveTowards(_person.transform.position, _routePoins[_currentRoutePoint].Point,
            _speed * Program.gameScene.GameSpeed * Time.deltaTime * _routePoins[_currentRoutePoint].Speed);
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
            _person.transform.position = _loadedPosition;
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
        if (_routePoins.Count > 0)
        {
            //correction needed when loading the _idle route inverse.. to avoid out of range excp
            _currentRoutePoint = CorrectBounds(_currentRoutePoint, 0, _routePoins.Count - 1);

            //print(currentRoutePoint + ".currentRoutePoint." + currRoute.Count + ".currRoute.Count");
            var currDist = Vector3.Distance(_person.transform.position, _routePoins[_currentRoutePoint].Point);
            if (currDist < distToChangeRot)
            { ChangeRotation(currDist); }
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

	//Called when the last point of a route was reached
    void WalkDone()
    {
        _location = GoingTo;
        _movingNow = false;
        SetCurrentAni("isIdle","isWalk");
    }

    public void Show()
    {
        _person.Geometry.GetComponent<Renderer>().enabled = true;
    }


    private Renderer renderer;
    public void Hide()
    {
        if (_hideThisTime)
        {
            //renderer.enabled = false; 
        }
    }

    Child GiveBirth()
    {
        return new Child();
    }
	
	// Update is called once per frame
	public void Update ()
    {
	    if (_movingNow)
	    {WalkHandler();}
	    CheckOnGameSpeed();
        CheckIfGoingIntoBuild();
    }

    /// <summary>
    /// If is inside a building will hide or show Geometry
    /// </summary>
    private void CheckIfGoingIntoBuild()
    {
        if (  _person == null || _routePoins == null || _routePoins.Count == 0){ return; }

        var dist = 0.2f;
        var currDist = Vector3.Distance(_person.transform.position, _routePoins[lastRoutePoint].Point);
        //getting close to last point
        if (currDist < dist ) 
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

        currDist = Vector3.Distance(_person.transform.position, _routePoins[index].Point);
        if (currDist < dist  ){Show();}
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
}
