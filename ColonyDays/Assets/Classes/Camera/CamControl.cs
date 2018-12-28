using UnityEngine;
using System.Collections;
using System.Globalization;
using UnityStandardAssets.Characters.FirstPerson;

//THIS IS A STATIC FUNCTION CLASS
public class CamControl : MonoBehaviour
{

    public static SmoothFollow CAMFollow;
    //RTS cam object and is a CamRTSController reference obj
    public static CamRTSController CAMRTS;
    //public static CamFPS CAMFPS;

    private static Camera rtsCamera;
    private static Camera mainMenuCamera;
    private static Camera firstPersonCamera;

    private GameObject menuCam;

    public static Camera RTSCamera() { return rtsCamera; }

    // Use this for initialization
    void Start()
    {
        mainMenuCamera = GameObject.Find("MainMenuCamera").GetComponent<Camera>();
    }

    public static CamControl Create(string root, Vector3 origen = new Vector3())
    {
        CamControl obj = null;
        obj = (CamControl)Resources.Load(root, typeof(CamControl));
        obj = (CamControl)Instantiate(obj, origen, Quaternion.identity);
        return obj;
    }

    public static void CreateCam(H cameraType)
    {
        if (cameraType == H.CamFollow)
        {
            CAMFollow = (SmoothFollow)Create(Root.cameraFollowLobby, Vector3.zero);
            CAMFollow.smoothTime = 1f;
            CAMFollow.isToFollowPlayer = false;
        }
        else if (cameraType == H.CamRTS && CAMRTS == null)
        {
            CAMRTS = (CamRTSController)Create(Root.cameraRTS, Vector3.zero);
            rtsCamera = CAMRTS.GetComponent<Camera>();
            
            //mainMenuCamera.enabled = false;
        }
    }

    public static bool IsMainMenuOn()
    {
        return currentCam == "Main";
    }

    private static string currentCam = "Main";//bz is the first camera on
    public static void ChangeTo(string newVal)
    {
        if (currentCam != newVal)
        {
            currentCam = newVal;
        }
        else//if is reACtivating same cam will return 
        {
            return;
        }

        //so if is FPS active the cam wont go anywhere 

        if (newVal == "Main")
        {
            mainMenuCamera.enabled = true;
            if (rtsCamera != null)
            {
                rtsCamera.enabled = false;
            }
        }
        else if(newVal == "Game")
        {
            CAMRTS.CamSensivity = 6;

            Cursor.visible = true;

            mainMenuCamera.enabled = false;
            if (rtsCamera != null)
            {
                rtsCamera.enabled = true;
            }
            if (firstPersonCamera != null)
            {
                firstPersonCamera.enabled = false;
            }
        } 
        else if(newVal == "First")
        {
            CAMRTS.CamSensivity = 0;

            firstPersonCamera = GameObject.Find("FirstPersonCharacter").GetComponent<Camera>();
            mainMenuCamera.enabled = false;
            rtsCamera.enabled = false;

            if (firstPersonCamera != null)
            {
                firstPersonCamera.enabled = true;
            }
        }
    }
}