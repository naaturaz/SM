using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//for search utility
public class USearch : MonoBehaviour
{
    /// <summary>
    /// Only evaluates X and Z axis... only works for the type of 90 degrees abanico
    /// implementado aqui
    /// </summary>
    /// <param name="list"></param>
    /// <param name="stone"></param>
    /// <returns></returns>
    public static Vector3 FindClosestPos(List<Vector3> list, Vector3 stone)
    {
        List<float > distances = new List<float>();
        for (int i = 0; i < list.Count; i++)
        {
            float dist = Vector3.Distance(stone, list[i]);
            distances.Add(dist);
            //General cloneTexto = General.Create(Root.texto3d, list[i]);
            //cloneTexto.GetComponent<TextMesh>().text = i.ToString();
        }

        int indexRes = -1;
        float min = UMath.ReturnMinimum(distances);
        for (int i = 0; i < distances.Count; i++)
        {
            if (distances[i] == min)
            {
                indexRes = i;
            }
        }

        return list[indexRes];
    }

    /// <summary>
    /// only find the first occurrance of gameObjToFind
    /// </summary>
    /// <param name="collPass">coll to look trhu</param>
    /// <param name="gameObjToFind">gameObjToFind</param>
    /// <returns>first occurrance of gameObjToFind in collPass</returns>
    public static Button FindBtnInColl(Button[] collPass, string gameObjToFind, out int index)
    {
        Button t = null;
        for (int i = 0; i < collPass.Length; i++)
        {
            if (collPass[i].transform.name == gameObjToFind || collPass[i].transform.name.Contains(gameObjToFind))
            {
                print(collPass[i].transform.name);
                t = collPass[i];
                index = i;
                return t;
            }
        }
        index = -1;
        return t;
    }

    public static Button FindBtnInColl(Button[] collPass, string gameObjToFind)
    {
        Button t = null;
        for (int i = 0; i < collPass.Length; i++)
        {
            if (collPass[i].transform.name == gameObjToFind || collPass[i].transform.name.Contains(gameObjToFind))
            {
                print(collPass[i].transform.name);
                t = collPass[i];
                return t;
            }
        }
        return t;
    }

    public static Button FindBtnInColl(Button[] collPass, Transform transfToFind)
    {
        Button t = null;
        for (int i = 0; i < collPass.Length; i++)
        {
            if (collPass[i] != null && transfToFind != null)
            {
                if (collPass[i].transform == transfToFind)
                {
                    t = collPass[i];
                    return t;
                }
            }
        }
        return t;
    }

    public static Transform FindTransfInArray(Transform[] collPass, string gameObjToFind)
    {
        Transform t = null;
        for (int i = 0; i < collPass.Length; i++)
        {
            if (collPass[i].transform.name == gameObjToFind || collPass[i].transform.name.Contains(gameObjToFind))
            {
                t = collPass[i];
                return t;
            }
        }
        return t;
    }

    public static Model FindTransfInList(List<Model> collPass, Transform gameObjToFind)
    {
        Model t = null;
        for (int i = 0; i < collPass.Count; i++)
        {
            if (collPass[i].transform == gameObjToFind )
            {
                t = collPass[i];
                return t;
            }
        }
        return t;
    }

    public static int GiveMeIndexInList(List<Model> collPass, Transform gameObjToFind)
    {
        int t = 0;
        for (int i = 0; i < collPass.Count; i++)
        {
            if (collPass[i].transform == gameObjToFind)
            {
                t = i;
                return t;
            }
        }
        return t;
    }

    public static GameObject FindGObjInCollWitString(GameObject[] collPass, string gameObjToFind)
    {
        GameObject t = null;
        int counter = 0;
        for (int i = 0; i < collPass.Length; i++)
        {
            if(collPass[i].name == gameObjToFind)
            {
                t = collPass[i];
                counter++;
            }
        }
        
        if(counter > 1)
        {
            print("were more than one obj with the same name StringUtil.FindGameObjInCollectionWithString()");
        }
        return t;
    }

    /// <summary>
    ///  Will search trhu the scene and and return an array wit all the transforms childs of the parameter transform
    /// </summary>
    /// <param name="objWithChild"></param>
    /// <returns></returns>
    public static Transform[] GetAllTransforms(Transform objWithChild, H excludeA = H.None, H excludeB = H.None)
    {
        int indexToRest = 0;
        Transform[] objs = new Transform[objWithChild.childCount];
        for (int i = 0; i < objWithChild.childCount; i++)
        {
            //if the name of the child objs is not the same as wht was passed in excludeA and B then ...
            if (objWithChild.GetChild(i).name != excludeA.ToString() &&
                objWithChild.GetChild(i).name != excludeB.ToString())
            {
                objs[i] = objWithChild.GetChild(i);
            }
            else
            {
                indexToRest++;
            }
        }
        //this is the final array and is as big as the child count minus all the exclusions
        Transform[] final = new Transform[objWithChild.childCount - indexToRest];

        for (int i = 0; i < final.Length; i++)
		{
            final[i] = objs[i];
		}

        return final;
    }

    /// <summary>
    /// Will search trhu the scene and return an array wit all the transforms childs if we find a gameObj in the scene with the same name
    /// </summary>
    /// <param name="groupString"></param>
    /// <param name="excludeA"></param>
    /// <param name="excludeB"></param>
    /// <returns></returns>
    public static Transform[] GetAllTransforms(string groupString, H excludeA = H.None, H excludeB = H.None)
    {
        Transform objWithChild = GameObject.Find(groupString).gameObject.transform;
        List<Transform> objs = new List<Transform>();
        for (int i = 0; i < objWithChild.childCount; i++)
        {
            //if the name of the child objs is not the same as wht was passed in excludeA and B then ...
            if (objWithChild.GetChild(i).name != excludeA.ToString() &&
                objWithChild.GetChild(i).name != excludeB.ToString())
            {
                objs.Add(objWithChild.GetChild(i));
            }
        }
        Transform[] final = objs.ToArray();
        return final;
    }

    public static List<Transform> ReturnAllTransformsInAList(Transform group)
    {
        List<Transform> objs = new List<Transform>();
        for (int i = 0; i < group.childCount; i++)
        {
            objs.Add(group.GetChild(i));
        }
        return objs;
    }

    /// <summary>
    /// Will search trhu the scene and Will return an array wit all the transforms childs if the have the same name as include
    /// </summary>
    /// <param name="groupString"></param>
    /// <returns></returns>
    public static Transform[] IncludeOnlyTransforms(string groupString, H include)
    {
        Transform objWithChild = GameObject.Find(groupString).gameObject.transform;
        List<Transform> objs = new List<Transform>();

        for (int i = 0; i < objWithChild.childCount; i++)
        {
            //if the name of the child .....
            if (objWithChild.GetChild(i).name == include.ToString())
            {
                objs.Add(objWithChild.GetChild(i));
            }
        }
        Transform[] final = objs.ToArray();
        return final;
    }

    public static Transform[] IncludeOnlyTransforms(Transform group, H include)
    {
        List<Transform> objs = new List<Transform>();
        for (int i = 0; i < group.childCount; i++)
        {
            //if the name of the child .....
            if (group.GetChild(i).name == include.ToString())
            {
                objs.Add(group.GetChild(i));
            }
        }
        Transform[] final = objs.ToArray();
        return final;
    }


    /// <summary>
    /// Will search trhu the scene and Will return an Transform wit all the transforms childs if the have the same name as include
    /// </summary>
    /// <param name="groupString"></param>
    /// <returns></returns>
    public static Transform IncludeOnly1Transform(string groupString, H include)
    {
        Transform objWithChild = GameObject.Find(groupString).gameObject.transform;
        List<Transform> objs = new List<Transform>();
        Transform final = null;
        for (int i = 0; i < objWithChild.childCount; i++)
        {
            //if the name of the child .....
            if (objWithChild.GetChild(i).name == include.ToString())
            {
                final = objWithChild.GetChild(i);
            }
        }
        return final;
    }

    public static Transform IncludeOnly1Transform(Transform group, H include)
    {
        Transform objWithChild = group;
        List<Transform> objs = new List<Transform>();
        Transform final = null;
        for (int i = 0; i < objWithChild.childCount; i++)
        {
            //if the name of the child .....
            if (objWithChild.GetChild(i).name == include.ToString())
            {
                final = objWithChild.GetChild(i);
            }
        }
        return final;
    }

    static List<string> WildcardFiles()
    {
        List<string> listRange = new List<string>();
        listRange.Add("q");
        listRange.Add("s");

        return listRange;
    }

    public static CamControl FindCurrentCamera()
    {
        CamControl cc = new CamControl();
        if (CamControl.CAMFollow != null)
        {
            cc = CamControl.CAMFollow;
        }
        else if (CamControl.CAMFPS != null)
        {
            print("CAMFPS doesnt inherit from CAMControl");
        }
        else if (CamControl.CAMRTS != null)
        {
            cc  = CamControl.CAMRTS;
        }
        return cc;
    }
}
