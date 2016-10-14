using UnityEngine;
using System.Collections;

public class GenericCameraComponent : MonoBehaviour {

    protected Transform TransformCam;
    protected Transform CenterTarget;

    public static GenericCameraComponent CreateCamComponent(string root, Transform cam, Vector3 origen = new Vector3(),
        Transform centerTarget = null, Transform classContainer = null)
    {
        GenericCameraComponent obj = null;
        obj = (GenericCameraComponent)Resources.Load(root, typeof(GenericCameraComponent));
        obj = (GenericCameraComponent)Instantiate(obj, origen, Quaternion.identity);
        obj.TransformCam = cam;
        obj.CenterTarget = centerTarget;

        if (classContainer != null)
        {
            obj.transform.SetParent( classContainer);
        }

        return obj;
    }
}
