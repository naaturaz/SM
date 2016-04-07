using UnityEngine;
using System.Collections;

public class Fustrum
{
    private Camera cam;
    private Plane[] planes;

    public Fustrum()
    {
        cam = Camera.main;
        planes = GeometryUtility.CalculateFrustumPlanes(cam);
    }

    public bool OnFustrum(Collider anObjCollider)
    {
        if (GeometryUtility.TestPlanesAABB(planes, anObjCollider.bounds))
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
        if (_oldCamPos!=cam.transform.position)
        {
            planes = GeometryUtility.CalculateFrustumPlanes(cam);
            _oldCamPos = cam.transform.position;
        }
    }
}
