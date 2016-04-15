using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/*
 * For deal with Trees (SemiOpaque) material will do regions that are 4 CrystalRegions.
 * Will incluse all Decora, Rocks, bz they are in the same Material
 * The roof of a Bohio should come into this too. Since the Material is SemiOpaque
 * 
 * For Opaque, Buildings etc, will do BatchRegions until they the Vertices count is 30K
 * 
 */

public class BatchRegion
{
    private string _id;
    private General _batchMaster;
    GameObject [] _all = new GameObject[500];

    //the object ID and the INT in the array
    Dictionary<string, int> _keymap = new Dictionary<string, int>();

    private int _totalVertices;
    private int _emptySpot;

    public BatchRegion(string id)
    {
        this._id = id;
    }

    void AddToAll(GameObject go, string id)
    {
        //todo Label Correctly object u want to put in here when Opaque
        //now Mansory has C Main as main and has a lot of subOjects . wont pass bz : meshF == null
        var meshF = go.GetComponent<MeshFilter>();
        if (go == null || go.transform == null || meshF == null)//if meshF == null is a 'Main' with subOjects in it 
        {
            return;
        }
        _totalVertices += meshF.mesh.vertexCount;


        if (_totalVertices > 62000)
        {
            Debug.Log("vert cnt over 62K "+_id);
            return;
        }

        _all[_emptySpot] = go;
        _keymap.Add(id, _emptySpot);

        DefineNextEmptySpot();
    }

    string ReturnProperID(General go)
    {
        if (go.Category == Ca.Spawn)
        {
            return go.MyId + "+";
        }
        return go.MyId;
    }

    void RemoveFromAll(General go)
    {
        var myID = ReturnProperID(go);
        var indexes = _keymap.Where(a => a.Key.Contains(myID)).OrderBy(a => a.Value).ToList();

        //setting the lowest int value to be the emptySpot
        if (indexes.Count>0)
        {
            _emptySpot = indexes[0].Value;
        }

        for (int i = 0; i < indexes.Count; i++)
        {
            var index = indexes[i].Value;
            var key = indexes[i].Key;

            //so it seen 
            _all[index].SetActive(true);
            //asign it back to original gamebject
            _all[index].transform.parent = go.transform;
            _totalVertices -= _all[index].GetComponent<MeshFilter>().mesh.vertexCount;
            _all[index] = null;
            _keymap.Remove(key);
        }
    }

    /// <summary>
    /// Will find the next element tht is null in the _all array
    /// </summary>
    private void DefineNextEmptySpot()
    {
        for (int i = _emptySpot; i < _all.Length; i++)
        {
            if (_all[i] == null)
            {
                _emptySpot = i;
                return;
            }
        }
    }

    internal void AddToRegion(General go)
    {
        go.BatchRegionId = _id;

        if (go.Category == Ca.Structure || go.Category == Ca.Shore)
        {
            //bz Strcuture calls twice 
            if (_keymap.ContainsKey(go.MyId))
            {
                return;
            }

            AddToAll(go.Geometry.gameObject, go.MyId);
            DecideIfRedoBatch();
        }
        else if (go.Category == Ca.Spawn)
        {
            //find all subGameObjects and add
            FindAllChildObjectsAndAddThem(go);
        }
    }

    private void FindAllChildObjectsAndAddThem(General go)
    {
        var subs = General.FindAllChildsGameObjectInHierarchy(go.gameObject);

        for (int i = 0; i < subs.Length; i++)
        {
            AddToAll(subs[i], go.MyId+"+"+i);
        }
    }






    internal void Remove(General go)
    {
        RemoveFromAll(go);

        DecideIfRedoBatch();
    }

    internal bool IsClose()
    {
        if (_totalVertices > 30000 && _id.Contains("Opaque"))
        {
            return true;
        }
        return false;
    }


    private SPr p = new SPr();
    public void DecideIfRedoBatch()
    {
        //soi buildings dont do anything while they are being aded
        if (p.TerraSpawnController.IsToLoadFromFile || !BuildingPot.Control.Registro.IsFullyLoaded)
        {
            return;
        }

        if (_batchMaster!=null)
        {
            var child = General.FindAllChildsGameObjectInHierarchy(_batchMaster.gameObject);

            for (int i = 0; i < child.Length; i++)
            {
                //so doesnt get wiped when destoryed 
                child[i].transform.parent = null;
            }

            _batchMaster.Destroy();
            _batchMaster = null;
        }

        //to address Regions tht have 0
        if (_keymap.Count == 0)
        {
            return;
        }
        CreateBatchMesh();
    }


    private static int highestInd;
    private static int highestVert;
    /// <summary>
    /// Kepps the highes array used
    /// </summary>
    void StatKeeper(int newVal)
    {
        if (newVal > highestInd)
        {
            highestInd = newVal;
            Debug.Log("Highes array:"+highestInd);
        }
        if (_totalVertices > highestVert)
        {
            highestVert = _totalVertices;
            Debug.Log("highest Vert :" + _totalVertices);
        }
    }

    void CreateBatchMesh()
    {
        if (_batchMaster == null)
        {
            _batchMaster = General.Create(Root.classesContainer, new Vector3(),
                _id, Program.MeshBatchContainer.transform);
        }

        int nullCt=0;
        int eleNtNull = 0;
        for (int i = 0; i < _all.Length; i++)
        {
            if (_all[i] == null && nullCt > 9)
            {
                //at 10 cts in a round of null means the last of the Array was reached 
                StatKeeper(i-10);
                break;
            }
            if (_all[i] == null)
            {
                nullCt++;
                continue;
            }

            eleNtNull = i;
            nullCt = 0;
            _all[i].SetActive(true);//in case this is now redoing a Region
            _all[i].transform.parent = _batchMaster.transform;
        }
        CombineMeshes(_batchMaster.gameObject, _all[eleNtNull].GetComponent<Renderer>().material);
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
        onGO.GetComponent<Renderer>().sharedMaterial = mat;

        onGO.transform.GetComponent<MeshFilter>().mesh = new Mesh();
        onGO.transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
        onGO.transform.gameObject.active = true;

        //Reset position
        onGO.transform.position = position;
    }
}

