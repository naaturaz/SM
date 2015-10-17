using UnityEngine;
using System.Collections;

//THIS IS A STATIC FUNCTION CLASS
public class CamControl : MonoBehaviour {

    public static SmoothFollow CAMFollow;
    //RTS cam object and is a CamRTSController reference obj
    public static CamRTSController CAMRTS;
    public static CamFPS CAMFPS;

	// Use this for initialization
	void Start (){}

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
        }
    }
}