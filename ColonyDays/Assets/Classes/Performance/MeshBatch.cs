using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MeshBatch
{
    public MeshBatch()
    {
        Debug.Log("Mesh Batch");
        House();
        //SemiOpaque();
    }

    private void SemiOpaque()
    {
        // find the corresponding objects e.g. with loading into cache
        General batchmaster = General.Create(Root.classesContainer, new Vector3(),
            "SemiOpaque", Program.MeshBatchContainer.transform);

        GameObject[] array = GameObject.FindGameObjectsWithTag("SemiOpaque");

        for (int i = 0; i < array.Length; i++)
        {
            array[i].transform.parent = batchmaster.transform;
        }

        CombineMeshes(batchmaster.gameObject, array[0].GetComponent<Renderer>().material);
    }

    void House()
    {
        // find the corresponding objects e.g. with loading into cache
        General batchmaster = General.Create(Root.classesContainer, new Vector3(),
            "Opaque", Program.MeshBatchContainer.transform);

        GameObject[] array = GameObject.FindGameObjectsWithTag("HouseMed");

        for (int i = 0; i < array.Length; i++)
        {
            array[i].transform.parent = batchmaster.transform;
        }

        CombineMeshes(batchmaster.gameObject, array[0].GetComponent<Renderer>().material);
    }

    void CombineMeshes(GameObject onGO, Material mat)
    {
        //Zero transformation is needed because of localToWorldMatrix transform
        Vector3 position = onGO.transform.position;
        onGO.transform.position = Vector3.zero;
        onGO.AddComponent<MeshFilter>();
        onGO.AddComponent<MeshRenderer>();

        MeshFilter[] meshFilters = onGO.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.active = false;
            i++;
        }

        //onGO.GetComponent<Renderer>().sharedMaterial = (Material)Resources.Load(Root.matTavernBase); 
        onGO.GetComponent<Renderer>().sharedMaterial = mat; 

        onGO.transform.GetComponent<MeshFilter>().mesh = new Mesh();
        onGO.transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
        onGO.transform.gameObject.active = true;

        //Reset position
        onGO.transform.position = position;
    }



    //  https://www.reddit.com/r/gamedev/comments/22mlem/reducing_draw_calls_in_unity_by_combining_meshes/
    /// <summary>
    ///  Assign the cluster of GameObjects with an identical mesh to a parent object, and run this
    ///  script on that parent object. This will reduce the amount of drawcalls of the object from 'n' 
    /// (for each sub-object) to 1 if you do it correctly.
    /// </summary>
    /// <param name="obj"></param>
    //void CombineMeshes2(GameObject obj)
    //{
    //    //Zero transformation is needed because of localToWorldMatrix transform
    //    Vector3 position = obj.transform.position;
    //    obj.transform.position = Vector3.zero;

    //    //whatever man
    //    MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
    //    CombineInstance[] combine = new CombineInstance[meshFilters.Length];
    //    int i = 0;
    //    while (i < meshFilters.Length)
    //    {
    //        combine[i].mesh = meshFilters[i].sharedMesh;
    //        combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
    //        meshFilters[i].gameObject.SetActive(false);
    //        i++;
    //    }
    //    obj.transform.GetComponent<MeshFilter>().mesh = new Mesh();
    //    obj.transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine, true, true);
    //    obj.transform.gameObject.SetActive(true);

    //    //Reset position
    //    obj.transform.position = position;

    //    //Adds collider to mesh
    //    obj.AddComponent<MeshCollider>();
    //}
}
