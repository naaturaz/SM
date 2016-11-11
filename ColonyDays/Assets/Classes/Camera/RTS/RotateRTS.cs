using UnityEngine;

public class RotateRTS : GenericCameraComponent {

    float MIN_Y = 10.8f;//14   20
    float MAX_Y = 120f;//60 50
    Vector3 oldPos;

    //move down and up thru Y
    public Vector3 MoveThruY(Transform current, float min, float max, float change)
    {
        Vector3 temp = current.position;
        temp.y += change;
        if (temp.y > max)
        {
            temp.y = max;
        }
        else if (temp.y < min)
        {
            temp.y = min;
        }
        return temp;
    }

    //Rotates camera hor and vert
    public void RotateCam(General helpCam360GrabPosY, General helpCam360MainY,
        Transform target, float camSensivity, float smoothTime, ref Vector3 velocity)
    {
            RotateCamHor(helpCam360GrabPosY.transform,
                helpCam360MainY.transform, target, camSensivity);
            RotateCamVert(camSensivity, target, smoothTime, ref velocity);

            CamControl.CAMRTS.centerTarget.transform.rotation = TransformCam.rotation;
            TransformCam.parent = CamControl.CAMRTS.centerTarget.transform;
    }

    void RotateCamVert(float camSensivity, Transform target, float smoothTimePass, ref Vector3 velocity)
    {
        var qOrE = Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E);
        TransformCam.parent = null;
        float changeValue = 0;
        if (Input.GetAxis("Mouse Y") != 0 && !qOrE)
        {
            changeValue = Input.GetAxis("Mouse Y") * camSensivity;
        }
        if (Input.GetAxis("Vertical") != 0 && !qOrE)
        {
            changeValue = Input.GetAxis("Vertical") * camSensivity;
        }
        if (changeValue != 0)
        {
            TransformCam.position = MoveThruY(TransformCam, MIN_Y, MAX_Y, changeValue);
            TransformCam.LookAt(target);
        }
    }

    public void RotateCamHor(Transform helpCam360GrabPosY, Transform helpCam360MainY,
        Transform target, float camSensivity)
    {
        TransformCam.parent = helpCam360GrabPosY.transform;
        float changeValue = 0;

        bool qOrE = false;
        if (Input.GetKey(KeyCode.Q))
        {
            qOrE = true;
            changeValue = .4f * camSensivity;
        }
        if (Input.GetKey(KeyCode.E))
        {
            qOrE = true;
            changeValue = -.4f * camSensivity;
        }
        
        //when Q or E this wont work
        if (Input.GetAxis("Mouse X") != 0 && !qOrE)
        {
            changeValue = Input.GetAxis("Mouse X") * camSensivity;
        }
        if (Input.GetAxis("Horizontal") != 0 && !qOrE)
        {
            changeValue = Input.GetAxis("Horizontal") * camSensivity;
        }

        if (changeValue != 0)
        {
            helpCam360MainY.transform.Rotate(new Vector3(0, changeValue, 0));
            TransformCam.LookAt(target);
        }
    }
}