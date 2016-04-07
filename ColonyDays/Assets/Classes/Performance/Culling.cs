using UnityEngine;
using System.Collections;

public class Culling {

    //CullingGroup _group = new CullingGroup();

    public Culling()
    {
        Layers();

        //CullingGroupsSet();
    }



    //void CullingGroupsSet()
    //{
    //    _group.targetCamera = Camera.main;
        
    //    BoundingSphere[] spheres = new BoundingSphere[1000];
    //    spheres[0] = new BoundingSphere(BuildingPot.Control.Registro.AllRegFile[0].IniPos, 1f);
    //    spheres[1] = new BoundingSphere(BuildingPot.Control.Registro.AllRegFile[1].IniPos, 100f);

    //    _group.SetBoundingSpheres(spheres);
    //    _group.SetBoundingSphereCount(2);

    //    _group.onStateChanged = StateChangedMethod;
    //}

    
    

    //private void StateChangedMethod(CullingGroupEvent evt)
    //{
    //    if(evt.hasBecomeVisible)
    //        Debug.LogFormat("Sphere {0} has become visible!", evt.index);
    //    if(evt.hasBecomeInvisible)
    //        Debug.LogFormat("Sphere {0} has become invisible!", evt.index);
    //}



    void Layers()
    {
        float[] distances = new float[32];
        Debug.Log("Oclussin layer");

        for (int i = 0; i<32; i++)
        {
            if (LayerMask.LayerToName(i) == "Ornament")
            {
                distances [i] = 50f;
            }
            if (LayerMask.LayerToName(i) == "Tree")
            {
                distances[i] = 55f;
            }
            //if (LayerMask.LayerToName(i) == "Person")
            //{
            //    distances[i] = 40f;
            //    Debug.Log("Person layer");
            //}
        }
        Camera.main.layerCullDistances = distances;
    }



}
