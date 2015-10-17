using UnityEngine;
using System.Collections.Generic;

public class URayCast : MonoBehaviour {

    static SMe m = new SMe();

    /// <summary>
    /// Will find the Obj on its way btw ini and end param
    /// 
    /// Suitable for Persons
    /// 
    /// This would cast rays only against colliders in layer passed as Param 'layer'
    /// </summary>
    public static RaycastHit FindObjOnMyWay(Vector3 ini, Vector3 end, int layer, Color color)
    {
        ini.y += 0.2f;//so we are not in flloor raso
        end.y += 0.2f;

        General dummyRayCaster = General.Create(Root.redCube, ini);
        dummyRayCaster.transform.LookAt(end);

        int layerMask = 1 << layer;

        Debug.DrawRay(dummyRayCaster.transform.position, dummyRayCaster.transform.forward * 33, Color.yellow, 5f);
        RaycastHit hit;
        if (Physics.Raycast(dummyRayCaster.transform.position,
            dummyRayCaster.transform.TransformDirection(Vector3.forward) * 100, out hit, Mathf.Infinity, layerMask))
        {

        }
        else
        {
            dummyRayCaster.Destroy();
            return hit;
        }
        dummyRayCaster.Destroy();
        return hit;
    }

    /// <summary>
    /// Will find all obj are colliding with this as an sphere and will 
    /// look to the parent and granpa 
    /// </summary>
    /// <returns></returns>
    public static List<string> CastSphere(Vector3 castingFromTransform, float castRadius)
    {
        List<string> res = new List<string>();
        Collider[] hitColliders = Physics.OverlapSphere(castingFromTransform, castRadius);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            res.AddRange(UString.ExtractNamesUntilGranpa(hitColliders[i].transform));
        }
        return res;
    }
}
