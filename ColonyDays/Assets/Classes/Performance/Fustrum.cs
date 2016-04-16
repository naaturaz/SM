using UnityEngine;
using System.Collections;

public class Fustrum
{
    private Camera cam;
    //private Plane[] planes;

    private Rect screenRect;

    public Fustrum()
    {
        cam = Camera.main;
        //planes = GeometryUtility.CalculateFrustumPlanes(cam);

        RedoRect();
    }

    //public bool OnFustrum(Collider anObjCollider)
    //{
    //    if (GeometryUtility.TestPlanesAABB(planes, anObjCollider.bounds))
    //    {
    //        //Debug.Log(" has been detected!");
    //        return true;
    //    }
    //    //Debug.Log("Nothing has been detected");

    //    return false;
    //}

    public bool OnScreen(Vector3 objPosition)
    {
        //todo change this cam.WorldToScreenPoint so its not asked by everyone all the time 
        var convertedPos = cam.WorldToScreenPoint(objPosition);
        if (screenRect.Contains(convertedPos))
        {
            //Debug.Log(" has been detected!");
            return true;
        }
        //Debug.Log("Nothing has been detected");
        return false;
    }

    private Vector3 _oldCamPos;
    public void Update()
    {
        //if (_oldCamPos!=cam.transform.position)
        //{
        //    planes = GeometryUtility.CalculateFrustumPlanes(cam);
        //    _oldCamPos = cam.transform.position;
        //}
    }


    //the pad outside the screen so things are seeing slighly before getting into screen
    private float screenPad = 50;
    /// <summary>
    /// Needed to init and if ScreenGame resolution is changed 
    /// </summary>
    public void RedoRect()
    {
        screenRect = new Rect(0 - screenPad, 0 - screenPad , Screen.width + screenPad, Screen.height + screenPad);
    }
}
