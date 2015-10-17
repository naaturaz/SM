using UnityEngine;
using System.Collections;

//for render utility
public class URender : MonoBehaviour
{
    public static void AssignMaterialToGObj(GameObject goPass, Material mPass0, Material mPass1, 
        Material mPass2 = null)
    {
        Material[] pass = { mPass0, mPass1, mPass2 };
        Texture newTexture = mPass0.GetTexture(0);
        goPass.GetComponent<Renderer>().material.SetTexture(0, newTexture);
        Material[] mats = goPass.transform.GetComponent<Renderer>().materials;

        for (int i = 0; i < mats.Length; i++)
        {
            if (pass[i] != null)
            {
                mats[i] = pass[i];
            }
        }
        goPass.GetComponent<Renderer>().materials = mats;
    }

    public static Material GetMaterialFromGObj(GameObject goPass, int materialToExtractIndex)
    {
        Material t = null;
        Material[] mats = goPass.transform.GetComponent<Renderer>().materials;
        t = mats[materialToExtractIndex];
        return t;
    }
}
