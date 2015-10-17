using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum MenusType
{
    None,
    SelectNewBuild,
    SelectNewRaw,
    SelectNewElement,
    SelectNewHelper,

    Help_Text,
    BasicMenu,
    RedSphereHelp,
    YellowSphereHelp
}

public class Btn3D : Button {

    //General variables

    private bool isRotating;//if is on will face the camera
    private bool isMoving;
    private float creationTime;//used to stop the isStarting bool
    private Vector3 midPointBtwOriginAndCamera;//use to find middle pount btw this and camera
    private Vector3 midPointBtwMenuAndOrigin;
    private Transform origin;//use to find origin of this 
    private bool areConnectionSpheresCreated;//use to create the link only once
    private int spheresLinkIndex;//use to keep track of amount of sphere that link origin and this
    private Btn3D[] link;//to hold the link
    private Vector3 distanceBtwCameraAndMenu;
    private float keepSpeed;//use to change speed in KeepRotationAndPosition()

    //static variables
    //this will reference the center menu transform so is fully aligend the menus 
    //to the camera horzontally and vertical
    public static Transform MIDDLEONE;

    #region Properties

    //for properties
    private Vector3 _movingToPosition;
    private int _movingToInxed;//will hold the index in the array tht this have to moveTo()
    private bool _isDocked;//if is undock will not keep the same position that camera has,stored in MovingToPosition

    /// Propertiees
    public Vector3 MovingToPosition
    {
        get { return _movingToPosition;}
        set { _movingToPosition = value; }
    }

    public int MovingToIndex
    {
        get { return _movingToInxed; }
        set { _movingToInxed = value; }
    }
    public bool IsDocked
    {
        get { return _isDocked; }
        set { _isDocked = value; }
    }
    #endregion Properties

    #region Constructors
    public Btn3D()
	{

	}
    #endregion

    #region General Functions
    ///Functions
    //public static Menus CreateMenu(MenusType type, Vector3 iniPosPass = new Vector3())
    
    //find all child and act in them 
    public void FindAllChild(bool pretendCloseBtnWasClicked = false)
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            if (Program.MOUSEOVERTHIS != null)
            {
                //if the mouse was over "CloseBtn"... and we have it in this obj as child... 
                //and name is == to the parent of wht we clicked ...
                if (this.transform.GetChild(i).name == "Close_Btn" && Program.MOUSEOVERTHIS.name == "Close_Btn"
                    && name == Program.MOUSEOVERTHIS.parent.name)
                {
                    this.Destroyer();//for fade to work
                    MenuHandler.CREATEMENU = true;
                    DestroyChilds();
                }
            }//if pretend flag was on...
            else if (pretendCloseBtnWasClicked)
            {
                this.Destroyer();//for fade to work
                MenuHandler.CREATEMENU = true;
                DestroyChilds();
            }
        }
    }

    ///Destroy Menu in fade mode if FadeState not equal null,
    ///if null we destroy it as is describe in base
    void DestroyChilds()
    {
        if (areConnectionSpheresCreated)
        {
            for (int k = 0; k < link.Length; k++)
            {
                if (link[k] != null)
                {
                    link[k].Destroyer();
                }
            }
        }
        areConnectionSpheresCreated = true;//so no connection is created after this was destroyed
    }
    #endregion

    #region 2D Functions

    #endregion
    #region 3dMenus
    //find menu position between two objects passed ... at speed
    //int division : //the bigger, the farest from secondPoint if is a 2 is half
    public Vector3 FindMiddlePoint(Vector3 currentPoint, Vector3 secondPoint, float where)
    {
        currentPoint = Vector3.Lerp(currentPoint, secondPoint, where);
        return currentPoint;
    }

    //will face camera at speedPass...
    public void FaceTo(Transform faceThis, float speedPass)
    {
        Vector3 orientation = new Vector3();

        if (Type != null)
        {
            if (Type.Contains("EvenRow"))
            {
                orientation = Vector3.down;
            }
            else if (!Type.Contains("EvenRow"))
            {
                orientation = Vector3.up;
            }
        }
        else
        {
            orientation = Vector3.up;
        }

        Quaternion currentRotation = transform.rotation;

        // Get the target rotation
        var newRotation = Quaternion.LookRotation(transform.position - faceThis.position, orientation);
        
        // Smoothly rotate towards the target //speedPass is the retardation, the higher the speedy is
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, speedPass * Time.deltaTime);

        //will stop rotation...//this is the way to allow a range, if is a bit one way or the other still will stop rotation
        if (transform.rotation.x > currentRotation.x - 0.0001f && transform.rotation.x  < currentRotation.x + 0.0001f)
        //if(transform.rotation.x == currentRotation.x)
        {
            isRotating = false;
        }
    }

    //will move to in 3d... pointB where to... 
    public void MoveTo(Vector3 pointB, float speedPass)
    {
        Vector3 currentPosition = transform.position;
        float step = speedPass * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, pointB, step);

        if (transform.position.x > currentPosition.x - 0.0001f && transform.position.x < currentPosition.x + 0.0001f)
        {
            isMoving = false;
        }
    }

    //handles 3d menu functions
    public void ThreeDMenuDealer()
    {
        //if  Program.MOUSEOVERTHIS not null
        if (Program.MOUSEOVERTHIS != null)
        {   //if we are hovering an obj that contains ".Hoverable" and is not rotating...
            if (Program.MOUSEOVERTHIS.name.Contains(".Hoverable") && !isRotating)
            {   
                isRotating = true;
            }
        }
        //if is rotating or starting
        if (isRotating || isStarting)
        {
            //Facing camera
            FaceTo(Camera.main.transform, 6f);
        }
        //If is moving or starting...
        if (isMoving || isStarting)
        {
            //if not MovingToPosition was set up then... will place in middle of screen
            if (MovingToPosition == null)
            {
                //will move to there.... middlePoint defined in Start(), at that speed
                MoveTo(midPointBtwOriginAndCamera, MoveSpeed);
                //will center obt to Camera//must be '.forward' if wanna center to Camera
                MoveTo(CenterToObject(Camera.main.transform, Vector3.forward, 25f), MoveSpeed);//25f is distance
            }
            //othewise
            else if(MovingToPosition != null)
            {
                MoveTo(MovingToPosition, MoveSpeed/10); // div by 10 bz if closer to the camera looks nice and smooth   
            }
        }
        //let ... seconds pass,... then isStarting = false
        if (Time.time > creationTime + 1f)
        {
            isStarting = false;
        }
        //if is not... and MovingToPosition was no set then...
        if (!isRotating && !isMoving && !isStarting && MovingToPosition == null)
        {
            //since is not moving anymore we find the midPointBtwMenuAndOrigin
            midPointBtwMenuAndOrigin = FindMiddlePoint(transform.position, origin.position, 0.5f);
            //creates conection btw origin and this 
            ThreeDConnectionCreator(origin.position, transform.position, 
                MenusType.RedSphereHelp, MenusType.YellowSphereHelp);
        }
    }

    //given the masterCenter will put aling the obj to it...
    public Vector3 CenterToObject(Transform masterCenter, Vector3 direction, float distance)
    {
        Vector3 position = Camera.main.transform.position + (masterCenter.rotation * direction * distance);
        return position;
    }

    //Creates physical connection btw two points
    public void ThreeDConnectionCreator(Vector3 originPass, Vector3 targetPass,
        MenusType middleModel, MenusType connectionModel)
    {
        Vector3[] spheresLink = new Vector3[8];
        //if flag is on... and is on Start()
        if (!areConnectionSpheresCreated)
        {
            //find all Vector3 point btw origin and this
            for (float where = 0.1f; where < 1f; where = where + 0.1f)
            {   //if is not the middle one
                if (where != 0.5f)
                {   //will assign Vector3 position.. will find distance btw points given 'where' (where 0-1, 1 is 100%)
                    spheresLink[spheresLinkIndex] = FindMiddlePoint(originPass, targetPass, where);
                    if (spheresLinkIndex < 7)
                    {   //will add a new number to index
                        spheresLinkIndex++;
                    }
                }
            }
            //creates the middle sphere 
            Btn3D sphereMiddle = (Btn3D)General.Create(Root.redSphereHelp, midPointBtwMenuAndOrigin);
            sphereMiddle.FadeDirection = "FadeIn";
            sphereMiddle.name = "RedSphereHelpMid";
            sphereMiddle.MoveSpeed = 100f;//needs speed cheat! otherwise doesnt do anything
            link[0] = sphereMiddle;
            //creates the link
            for (int i = 0; i < spheresLink.Length; i++)
            {
                Btn3D sphereLinkMenu = (Btn3D)General.Create(Root.yellowSphereHelp, spheresLink[i]);
                sphereLinkMenu.FadeDirection = "FadeIn";
                sphereLinkMenu.name = "RedSphereHelpLink";
                sphereLinkMenu.MoveSpeed = 20f + i * 30f;//speed diferent for each one.. so gives sensation of graduality
                link[i+1] = sphereLinkMenu;
            }
            //will set to false the flag
            areConnectionSpheresCreated = true;
        }
    }
    #endregion 3dMenus

    #region Fade Functions

 


  

    #endregion Fade Functions

    #region Position and Rotation

    //handles the position and rotation of entry , stay, and exit of menus 
    void PositionAndRotationDealer()
    {
        if (!isStarting && !isDestroying)
        {
            //will keep rotation and pos relative to camera
            KeepRotationAndPosition();
        }
        else if (isDestroying)
        {
            //will let it go to moveTo original point defined in Program.cs
            MoveTo(InitialPosition, MoveSpeed / 10f);
        }
    }

    void KeepRotationAndPosition()
    {
        if (Application.loadedLevelName == "Lobby")
        {
            MoveTo(MovingToPosition, 5f);
            MicroManageRotation(100f);
        }
        else
        {
            //if keepSpeed less than 45 and is not docked
            if (keepSpeed < 45f && !IsDocked)
            {
                //keep adding speed 
                keepSpeed = keepSpeed + 15f * Time.deltaTime;
                //move this to MovingToPosition
                MoveTo(MovingToPosition, keepSpeed);

                MicroManageRotation(keepSpeed);
            }
            //if keepSpeed is more than 44f
            else if (keepSpeed > 44f)
            {
                IsDocked = true;
                transform.position = MovingToPosition;
                MicroManageRotation(1000f);//really fast
            }
        }
    }

    /// <summary>
    /// Will micro manage the roation of this 
    /// </summary>
    /// <param name="speedPass">speed in which will roatate towards something</param>
    void MicroManageRotation(float speedPass)
    {
        //if is the middle menu...
        if (name.Contains(".Middle3dMenu"))
        {   //defines MIDDLEONE static that reference Middle Menu Transform
            MIDDLEONE = transform;
            //makes the middle menu faceTo(Camera)  
            FaceTo(Camera.main.transform, speedPass);
        }
        //if Type is not null...
        if (Type != null)
        {
            //if is not a Middle3dMenu and doest contain
            if ((!name.Contains(".Middle3dMenu")) && !Type.Contains("EvenRow"))
            {
                MIDDLEONE = Camera.main.transform;
            }
        }
        //if this is not null and is not the Middle3dMenu
        if (this != null && /*!name.Contains(".Middle3dMenu") &&*/ MIDDLEONE != null)
        {
            //if is not the middle menuu will .... 
            transform.rotation = Quaternion.Slerp(transform.rotation, MIDDLEONE.rotation, speedPass * Time.deltaTime);
        }
    }

    #endregion

    #region Checkers

    //check if current position == movingtoposition
    void CheckIfPositionFixed()
    {
        if (transform.position == MovingToPosition)
        {
            PositionFixed = true;
        }
    }
    #endregion

    #region Unity Voids

    void FixedUpdate()
    {
        //if (Type == "TopEvenRow")
        //{
        //    //reference position define and update in Program.cs
        //    //MovingToPosition = Program.TOPSHOW[MovingToIndex].position;
        //}
        if (name.Contains("3dMenu"))
        {
            ThreeDMenuDealer();
        }
        FadeDealer();

        PositionAndRotationDealer();
    }

    new void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            FindAllChild();
        }

        CheckIfPositionFixed();
    }

    void Start()//due to unity bug we have to initialize here in this level of class(Mono:General:This)
    {
        isStarting = true;
        isRotating = true;
        isMoving = false;
        creationTime = Time.time;
        if (Program.MOUSEOVERTHIS != null)
        {
            origin = Program.MOUSEOVERTHIS;//origin
            midPointBtwOriginAndCamera = FindMiddlePoint(origin.position,
                Camera.main.transform.position, 0.5f);//5 bz I tried other numbers and should be fine there with current camera and menus scale
        }
        spheresLinkIndex = 0;
        link = new Btn3D[9];
        distanceBtwCameraAndMenu = Camera.main.transform.position - transform.position;
        keepSpeed = 20f;

        base.Start();
    }
    #endregion Unity Voids
}