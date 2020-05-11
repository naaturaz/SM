using UnityEngine;

public class KeepHeight : MonoBehaviour
{
    public bool isKeepingCamHeight = true;
    public bool isToKeepDistanceFromCamAndRotateLikeChild;

    public bool isKeepingParentHeight;

    public bool isKeepingInitPos;
    public bool isKeepingIniRot;

    public bool isKeepingCamYRotX;
    public bool isKeepingCamYRotY;
    public bool isKeepingCamYRotZ;

    private CamControl mainCam;
    private Transform iniTransf;
    private Vector3 difference;

    // Use this for initialization
    private void Start()
    {
        iniTransf = transform;
        mainCam = USearch.FindCurrentCamera();

        if (isToKeepDistanceFromCamAndRotateLikeChild)
        {
            difference = mainCam.transform.position - transform.position;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        //this line is the one that makes this obj go togther with the camera
        //every where they go... keeping a distance and keeping the
        //same rotation as camera
        difference = mainCam.transform.position - transform.position;
        Keeps();
    }

    private void Keeps()
    {
        if (isKeepingCamHeight)
        {
            Vector3 t = transform.position;
            t.y = mainCam.transform.position.y;
            transform.position = t;
        }
        //will keep always the initial height
        else if (!isKeepingCamHeight)
        {
            Vector3 t = transform.position;
            t.y = iniTransf.position.y;
            transform.position = t;
        }

        if (isKeepingParentHeight)
        {
            Vector3 t = transform.position;
            t.y = transform.parent.transform.position.y;
            transform.position = t;
        }

        if (isToKeepDistanceFromCamAndRotateLikeChild)
        {
            transform.position = mainCam.transform.position - difference;
        }

        if (isKeepingCamYRotX)
        {
            var rot = transform.rotation;
            rot.x = mainCam.transform.position.x;
            transform.rotation = rot;
        }

        if (isKeepingCamYRotY)
        {
            var rot = transform.rotation;
            rot.y = mainCam.transform.position.y;
            transform.rotation = rot;
        }

        if (isKeepingCamYRotZ)
        {
            var rot = transform.rotation;
            rot.z = mainCam.transform.position.z;
            transform.rotation = rot;
        }

        if (isKeepingInitPos)
        {
            transform.position = iniTransf.position;
        }
        if (isKeepingIniRot)
        {
            transform.rotation = iniTransf.rotation;
        }
    }
}