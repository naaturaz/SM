/* This is the camRTS class main class
 * 
 * This camera should always be spawned at 0.0.0 and from there move
 * 
 * Here is the input for the Keys for the Camera
 */

using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class CamRTSController : CamControl
{
    //Inital cam val
    Vector3 initialCamPos = new Vector3(0, 15f, 0);//(0, 20f, 0);
    Quaternion camIniRot = Quaternion.Euler(45f, 0, 0);
    private bool wasYAligned;

    float cameraXRotation;

    public float camSensivity = 6f;
    float camDiminigFactor = 0.4f;
    Transform myForward;
    Transform myBackward;
    Transform myRight;
    Transform myLeft;

    //Rotation Vars in Y axis
    public General centerTarget;
    General helpCam360MainY;
    General helpCam360GrabPosY;
    General helpCam360BalanceY;

    public static bool IsMouseMiddle;

     float MIN_FIELD_CAM = 41f;//25    5
     float MAX_FIELD_CAM = 41f;//42   48

    //Target
    public Transform target;
    public RaycastHit hitFront;
    public float speed = 15f;
    public float distance;

    //Smother cam
    float smoothTime = 0.3f;
    private Vector3 velocity = new Vector3();

    Dir mouseInBorderDir;
    //Classes references.. this clases only work for obj typeof CamRTS.cs 
    MouseInBorderRTS border;
    RotateRTS rotateRTS;
    InputRTS inputRTS;

    //the minimap fully functioning. 
    MiniMapRTS miniMapRTS;

    private Rotate _rotateScript;

    public MiniMapRTS MiniMapRts
    {
        get { return miniMapRTS; }
        set { miniMapRTS = value; }
    }

    public InputRTS InputRts
    {
        get { return inputRTS; }
        set { inputRTS = value; }
    }

    public float CamSensivity
    {
        get { return camSensivity; }
        set { camSensivity = value; }
    }



    /// <summary>
    /// Create the guides that will grab all the point to the camera move thru when doing the 360 
    /// Rotation
    /// </summary>
    void CreateRotCam360GuidesY()
    {
        if (helpCam360MainY != null)
        {
            return;
        }
        CreateAndUpdate360YGuidesPos();
        //CaptureAllPointsForCamRot();
    }

    /// <summary>
    /// This is the one that creates all the helpers that allow the camera rotation
    /// </summary>
    void CreateAndUpdate360YGuidesPos()
    {
        if (helpCam360MainY == null && centerTarget != null)
        {
            //create the initial pos for the main obj 
            Vector3 main = centerTarget.transform.position;
            main.y = transform.position.y;
            Vector3 difference = main - transform.position;

            helpCam360MainY = General.Create(Root.yellowSphereHelp_ZeroAlpha, main, "helpCam360MainY");
            helpCam360GrabPosY = General.Create(Root.yellowSphereHelp_ZeroAlpha, transform.position, "helpCam360GrabPosY");
            helpCam360BalanceY = General.Create(Root.yellowSphereHelp_ZeroAlpha, main + difference, "helpCam360BalanceY");
            helpCam360GrabPosY.transform.SetParent( helpCam360MainY.transform);
            helpCam360BalanceY.transform.SetParent( helpCam360MainY.transform);
        }
    }

    void RotateDealer()
    {
        CreateTargetAndUpdate();
        //if centerOfRotY was not created wont rotate
        if (centerTarget != null)
        {
            //Cursor.visible = false;
            CreateRotCam360GuidesY();
            rotateRTS.RotateCam(helpCam360GrabPosY, helpCam360MainY, target, camSensivity, smoothTime,
                ref velocity);
        }
    }

    void Start()
    {
        if (Developer.IsDev)
        {
            MIN_FIELD_CAM = 5f;// 48  5   21   45
            MAX_FIELD_CAM = 48f;//50   80  45  60
        }
#if UNITY_EDITOR
        MIN_FIELD_CAM = 5f;// 48  5   21   45
        MAX_FIELD_CAM = 48f;//50   80  45  60
#endif

    }


    void InitializeObjects()
    {
        if (transform.position != new Vector3())
        {
            return;
        }

        //initial pos and rot
        transform.position = initialCamPos;
        var rot = transform.rotation;
        transform.rotation = rot*camIniRot;

        CreateTargetAndUpdate();
        FindGuideChildObjs(centerTarget.transform);//if giving u a null exp here in new terrain is bz new terrain is not
        //l;ayer 8
        transform.SetParent( centerTarget.transform);


        miniMapRTS = (MiniMapRTS)MiniMapRTS.CreateCamComponent(Root.miniMapRTS, transform, classContainer: transform);
        border = (MouseInBorderRTS)MouseInBorderRTS.CreateCamComponent(Root.mouseInBorderRTS, transform,
             classContainer: transform);
        rotateRTS = (RotateRTS)RotateRTS.CreateCamComponent(Root.rotateRTS, transform,
            classContainer: transform);
        inputRTS = (InputRTS)InputRTS.CreateCamComponent(Root.inputRTS, transform, centerTarget: centerTarget.transform,
            classContainer: transform);

        _rotateScript = GetComponent<Rotate>();
    }

    #region Audio Reporting
    private bool _isAudioReport;
    public void ReportAudioNow()
    {
        _isAudioReport = true;
    }

    public void StopReportingAudioNow()
    {
        _isAudioReport = false;
    }


    void AudioAmbientPlay()
    {
        var playThis = MeshController.CrystalManager1.WhatAudioIPlay(transform.position);
        var pos = MeshController.CrystalManager1.CurrentRegionPos(transform.position);
        //if game tht has not started
        if (string.IsNullOrEmpty(playThis) || AudioCollector.AudioContainers.Count == 0 
            || !_isAudioReport
            )
        {
            return;
        }

        AudioCollector.PlayAmbience(playThis, pos);
        //Debug.Log(playThis);
    }

    void OnGUI()
    {
        if (Event.current.type == EventType.KeyUp || Event.current.type == EventType.MouseUp)
        {
        //    Debug.Log("Key/Mouse up event detected");
            AudioAmbientPlay();
        }
    }
#endregion

    // Update is called once per frame
    //void LateUpdate()
    void Update()
    {
        //initiales current obj pos and rot to...
        InitializeObjects();
        CreateTargetAndUpdate();

        if (!Program.IsInputLocked &&
            U2D.IsMouseOnScreen() &&
            !MiniMapRTS.isOnTheLimits && BuildingPot.Control != null && BuildingPot.Control.Registro.AllBuilding.Count>0)
        {
            ControlInput();
            MouseInBorderDealer();
        }

        AlignYInZero();

        RotateScript();
    }

    private void RotateScript()
    {
        if (!Developer.IsDev)
        {
            return;
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            _rotateScript.ToggleOn();
        }
        if (Input.GetKeyUp(KeyCode.X))
        {
            _rotateScript.ToggleOn('X');
        } 
        if (Input.GetKeyUp(KeyCode.Y))
        {
            _rotateScript.ToggleOn('Y');
        } 
        if (Input.GetKeyUp(KeyCode.Z))
        {
            _rotateScript.ToggleOn('Z');
        } 
        if (Input.GetKeyUp(KeyCode.KeypadPlus))
        {
            _rotateScript.ChangeSpeed(0.01f);
        } 
        if (Input.GetKeyUp(KeyCode.KeypadMinus))
        {
            _rotateScript.ChangeSpeed(-0.01f);

        }
    }

    /// <summary>
    /// Will always make the Y value whatever the hitFront value is plus the initial
    /// value stored in initialCamPos... so the terrain should be even around the 0,0,0
    /// area so the camera is even for all the terrain and looks the same for each terrain
    /// </summary>
    void AlignYInZero()
    {
        if (!wasYAligned)
        {
            transform.SetParent( null);
            float newYPos = initialCamPos.y + hitFront.point.y;
            transform.position =new Vector3(0, newYPos, 0);
            CreateTargetAndUpdate();//so the hitFront updates
            centerTarget.transform.position = hitFront.point;
            transform.SetParent( centerTarget.transform);
            wasYAligned = true;

            //CAMRTS.InputRts.SaveFirstCamPos();
        }
    }

    void MouseInBorderDealer()
    {
        mouseInBorderDir = Dir.None;
        mouseInBorderDir = border.ReturnMouseDirection();
        if (mouseInBorderDir != Dir.None && !InputRTS.IsFollowingPersonNow)
        {
            AssignPosTo(mouseInBorderDir);
        }
    }

    void FindGuideChildObjs(Transform toPickFrom)
    {
        for (int i = 0; i < toPickFrom.transform.childCount; i++)
        {
            if (toPickFrom.transform.GetChild(i).name == "Guide_Forward")
            {
                myForward = toPickFrom.transform.GetChild(i);
            }
            else if (toPickFrom.transform.GetChild(i).name == "Guide_Backward")
            {
                myBackward = toPickFrom.transform.GetChild(i);
            }
            else if (toPickFrom.transform.GetChild(i).name == "Guide_Right")
            {
                myRight = toPickFrom.transform.GetChild(i);
            }
            else if (toPickFrom.transform.GetChild(i).name == "Guide_Left")
            {
                myLeft = toPickFrom.transform.GetChild(i);
            }
        }
    }

    /// <summary>
    /// Creates obj in center of screen on the terrain that is used to 
    /// be the center of camera when rotates
    /// </summary>
    public void CreateTargetAndUpdate()
    {
        Vector3 startingRay = transform.position + transform.forward * 2;
        Debug.DrawRay(startingRay, transform.forward * 20, Color.green);

        // Bit shift the index of the layer (8) to get a bit mask
        // This would cast rays only against colliders in layer 8.
        int layerMask = 1 << 8;

        // Does the ray intersect any objects in the layer 8 "Terrain Layer"
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward),
            out hitFront, Mathf.Infinity, layerMask))
        {
            if (centerTarget == null)
            {
                centerTarget = General.Create(Root.centerTarget, hitFront.point, "centerTarget");
                target = centerTarget.transform;
            }
        }
        //else//Debug.Log("Did not Hit Layer 8: Terrain");
    }

    public void DestroyCenterTarget()
    {
        centerTarget.Destroy();
        centerTarget = null;
        target = null;
    }

    private void ControlInput()
    {
        QandEKeys();
        MiddleMouse();
        if (!IsMouseMiddle && mouseInBorderDir == Dir.None && !InputRTS.IsFollowingPersonNow)
        {
            AssignPosTo();
        }
        ScrollMouse();
    }

    private void QandEKeys()
    {
        

        if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E))
        {
            //CreateRotCam360GuidesY();
            //IsMouseMiddle = true;
            RotateDealer();

        }
        else if (Input.GetKeyUp(KeyCode.Q) || Input.GetKeyUp(KeyCode.E))
        {
            //IsMouseMiddle = false;
            CleanUpRotHelp();
        }
    }

    private void AssignPosTo(Dir dir = Dir.None)
    {
        dir = FilterNorthAndSouth(dir);

        Vector3 newPos = MoveLocalDealer(centerTarget.transform, dir);
        centerTarget.transform.position = miniMapRTS.ConstrainLimits(newPos);
    }

    /// <summary>
    /// Will filter North and South bz I have menu bars on there 
    /// </summary>
    /// <param name="dir"></param>
    /// <returns></returns>
    Dir FilterNorthAndSouth(Dir dir)
    {
        if (dir == Dir.N || dir == Dir.Up)
        {
            return Dir.None;
        }
        else if (dir == Dir.S || dir == Dir.Down)
        {
            return Dir.None;
        }
        return dir;
    }

    void ScrollMouse()
    {
        int localMultiplier = 900;
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            float fieldOfView = UMath.changeValSmooth(transform.GetComponent<Camera>().fieldOfView,
                -Input.GetAxis("Mouse ScrollWheel"), localMultiplier, 
                MIN_FIELD_CAM, MAX_FIELD_CAM,
                camSensivity);
            transform.GetComponent<Camera>().fieldOfView = fieldOfView;
        }
    }

    void MiddleMouse()
    {
        //if middle mouse btn is pressed
        if (Input.GetMouseButton(2) //&& !InputRTS.IsFollowingPersonNow
            )
        {
            IsMouseMiddle = true;
            RotateDealer();
        }
        //if is realeased
        else if (Input.GetMouseButtonUp(2)// && !InputRTS.IsFollowingPersonNow
            )
        {
            //transform.SetParent( null;
            CleanUpRotHelp();
            IsMouseMiddle = false;
            //Cursor.visible = true;
        }
        else if(Input.GetKeyUp(KeyCode.End)){}
    }

    /// <summary>
    /// Clean ups all the helper items
    /// </summary>
    public void CleanUpRotHelp()
    {
        //if (centerOfRotY != null)
        //{
        //    centerOfRotY.Destroy();
        //    centerOfRotY = null;
        //    target = null;
        //}
        if (helpCam360MainY != null)
        {
            helpCam360MainY.Destroy();
            helpCam360MainY = null;
            helpCam360GrabPosY.Destroy();
            helpCam360GrabPosY = null;
            helpCam360BalanceY.Destroy();
            helpCam360BalanceY = null;
        }
    }



    private float _desiredSpeed = 1f;
    public float DesiredSpeed
    {
        get { return _desiredSpeed; }
        set { _desiredSpeed = value; }
    }

    /// <summary>
    /// Will decompose a Compose Direction (ex: UpRight), into Up and Right so the 
    /// mouse direction will be valid if is in a corner, if is a simpel direction
    /// will be send in the first item of the 'List<D> localDirs' , 2nd item will be D.none.
    /// If composeDir == D.none , both items in the list will be equal to so. 
    /// </summary>
    /// <param name="composeDir">direction that MouseInBorder.cs obj is sending</param>
    /// <returns></returns>
    List<float> DecomposeDirectionRetValues(Dir composeDir)
    {
        List<Dir> localDirs = new List<Dir>();
        if(composeDir == Dir.DownRight)
        {
            //when decomposing always X axis 1st and then Y axis 
            localDirs.Add(Dir.Right);
            localDirs.Add(Dir.Down);
        }
        else if(composeDir == Dir.DownLeft)
        {
            localDirs.Add(Dir.Left);
            localDirs.Add(Dir.Down);
        }
        else if(composeDir == Dir.UpRight)
        {
            localDirs.Add(Dir.Right);
            localDirs.Add(Dir.Up);
        }
        else if (composeDir == Dir.UpLeft)
        {
            localDirs.Add(Dir.Left);
            localDirs.Add(Dir.Up);
        }
        //when is not really a composed direction
        else
        {
            //will assign both values to current direction in the below
            // ResponsiveInputAxisTo() will descriminate given the axis
            localDirs.Add(composeDir);
            localDirs.Add(composeDir);
        }

        float horChange = 0;
        float vertChange = 0;

        //if localDirections are none user is not moving the camera with mouse
        //therefore can use the keyboad to move 
        if(localDirs[0] == Dir.None)
        {
            horChange=Input.GetAxis("Horizontal");
            vertChange = Input.GetAxis("Vertical");
        }
        //if user is using mouse horChange and vertChange will remaing at 0 therefore
        //subsequente method will find where to go based on mouse direction 

        float horValue = UMath.ResponsiveInputAxisTo(_desiredSpeed, Dir.Horizontal,
        horChange, localDirs[0]);
        float vertValue = UMath.ResponsiveInputAxisTo(_desiredSpeed, Dir.Vertical,
        vertChange, localDirs[1]);

        return new List<float> {horValue, vertValue};
    }

    /// <summary>
    /// This is the move dealer will store in list the direction the user is moving to 
    /// and will decompose it, then will pass all tht as parameter to MoveInLocalDir()
    /// </summary>
    /// <param name="objToBeMoved">objToBeMoved</param>
    /// <param name="direction">direction that MouseInBorder.cs obj is sending</param>
    /// <returns></returns>
    
    Vector3 MoveLocalDealer(Transform objToBeMoved, Dir direction = Dir.None)
    {
        List<float> valDirection = DecomposeDirectionRetValues(direction);
        int shiftMultiplier = CheckIfShiftKeyIsPressed();
        return MoveInLocalDir(objToBeMoved, valDirection[0] * shiftMultiplier,
            valDirection[1] * shiftMultiplier, direction);
    }

    /// <summary>
    /// If shift key is pressed will return 5,. othrwise 1, so it will get multiplied
    /// the speed of the cam
    /// </summary>
    int CheckIfShiftKeyIsPressed()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            return 5;
        }
        return 1;
    }

    /// <summary>
    /// Based on first able the direction and then the input from keys will move the obj to there
    /// </summary>
    /// <param name="objToBeMoved">Ref obj tht Will be moved</param>
    /// <param name="horValue">horizontal value</param>
    /// <param name="vertValue">vertical value</param>
    /// <param name="direction">direction that MouseInBorder.cs obj is sending</param>
    /// <returns></returns>
    Vector3 MoveInLocalDir(Transform objToBeMoved, float horValue, float vertValue,
        Dir direction = Dir.None)
    {
        Vector3 result = objToBeMoved.transform.position;
        direction = ReturnAxisToMove(horValue, vertValue, direction);
        if (direction == Dir.VerticHorizo)
        {
            float compensate = 1.4f;
            Vector3 hor = MoveToWhere(myRight.position, horValue * compensate, objToBeMoved);
            Vector3 vert = MoveToWhere(myForward.position, vertValue * compensate, objToBeMoved);
            result = Vector3.Lerp(hor, vert, 0.5f);
        }
        else if (direction == Dir.Horizontal)
        {
            result = MoveToWhere(myRight.position, horValue, objToBeMoved);
        }
        else if (direction == Dir.Vertical)
        {
            result = MoveToWhere(myForward.position, vertValue, objToBeMoved);
        }
        return result;
    }

    /// <summary>
    /// Will return in which Axis the move is happening based on Direction first 
    /// pased from  MouseInBorder.cs obj, then if that is equal D.None will then
    /// look at the keyboard input 
    /// </summary>
    /// <param name="horVal">horizontal axis change value </param>
    /// <param name="vertVal">vertical axis change value</param>
    /// <param name="dir">direction pased from  MouseInBorder.cs obj</param>
    /// <returns></returns>
    Dir ReturnAxisToMove(float horVal, float vertVal, Dir dir = Dir.None)
    {
        //VerticHorizo
        //mouse 
        if((dir == Dir.UpLeft || dir == Dir.UpRight ||
             dir == Dir.DownLeft || dir == Dir.DownRight) && !IsMouseMiddle)
        {
            dir = Dir.VerticHorizo;
        }
        //keyboard
        else if((horVal != 0 && vertVal != 0 && UInput.IfCursorKeyIsPressed()) && dir == Dir.None)
        {
            dir = Dir.VerticHorizo;
        }

        //Horizontal
        //mouse
        if (((dir == Dir.Right || dir == Dir.Left) && !IsMouseMiddle))
        {
            dir = Dir.Horizontal;
        }
        //keyobard
        else if (horVal != 0 && dir == Dir.None)
        {
            dir = Dir.Horizontal;
        }

        //Vertical
        //mouse
        if ((dir == Dir.Up || dir == Dir.Down) && !IsMouseMiddle)
        {
            dir = Dir.Vertical;
        }
        //keyobard
        else if (vertVal != 0 && dir == Dir.None )
        {
            dir = Dir.Vertical;
        }
        return dir;
    }

    /// <summary>
    /// Move an objetc to a different position 
    /// </summary>
    /// <param name="moveTo"></param>
    /// <param name="multiplier"></param>
    /// <param name="objToBeMoved">Obj to be moved</param>
    Vector3 MoveToWhere(Vector3 moveTo, float multiplier, Transform objToBeMoved)
    {
        //this is created so an obj that is not the camera can use the same
        //local directions that guides the camera
        Vector3 moveToInternal = moveTo;
        moveToInternal.y = objToBeMoved.position.y;

        if (objToBeMoved != null)
        {
            moveTo = Vector3.MoveTowards(objToBeMoved.position, moveToInternal,
            multiplier * camDiminigFactor);
        }
        else { print("CamRTS.MoveToWhere() objToBeMoved was null"); };
        return moveTo;
    }


}