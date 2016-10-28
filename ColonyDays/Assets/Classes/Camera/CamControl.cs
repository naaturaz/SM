using UnityEngine;
using System.Collections;

//THIS IS A STATIC FUNCTION CLASS
public class CamControl : MonoBehaviour {

    public static SmoothFollow CAMFollow;
    //RTS cam object and is a CamRTSController reference obj
    public static CamRTSController CAMRTS;
    //public static CamFPS CAMFPS;

    private static Camera rtsCamera;
    private static Camera mainMenuCamera;

	// Use this for initialization
    void Start()
    {
        mainMenuCamera = FindObjectOfType<Camera>();
        
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
            //none here
            //mainMenuCamera = FindObjectOfType<Camera>();

            CAMRTS = (CamRTSController)Create(Root.cameraRTS, Vector3.zero);
            rtsCamera = CAMRTS.GetComponent<Camera>();

            if (mainMenuCamera != null)
            {
                mainMenuCamera.enabled = false;
            }

            //so when is loaded goes to mainMenu cam
            //ChangeTo("Main");

        }
    }

    public static void ChangeTo(string newVal)
    {
        if (newVal == "Main")
        {
            mainMenuCamera.enabled = true;

            if (rtsCamera != null)
            {
                rtsCamera.enabled = false;
                
            }

        }
        else
        {
            mainMenuCamera.enabled = false;
            if (rtsCamera != null)
            {
                rtsCamera.enabled = true;

            }
        }
    }
}