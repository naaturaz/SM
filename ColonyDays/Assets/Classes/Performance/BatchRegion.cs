using System;
using System.Collections.Generic;
using System.Linq;
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
    private GameObject[] _all = new GameObject[1500];

    //the object ID and the INT in the array
    private Dictionary<string, int> _keymap = new Dictionary<string, int>();

    private int _totalVertices;
    private int _emptySpot;

    public BatchRegion(string id)
    {
        this._id = id;
    }

    private int ReturnVertices(GameObject go, string id)
    {
        var meshF = go.GetComponent<MeshFilter>();

        if (meshF != null)
        {
            //Debug.Log("Obj:" + go.transform.name + "|" + meshF.mesh.vertexCount);

            return meshF.mesh.vertexCount;
        }

        //for obj that have subobjects and not mesh attached
        Debug.Log("Obj:" + go.transform.name + "|" + 1000);
        throw new Exception("Obj:" + id + " has subObjects on it or doesnt have a meshFilter attched");
        return 1000;
    }

    private void AddToAll(GameObject go, string id)
    {
        //todo Label Correctly object u want to put in here when Opaque
        //now Mansory has C Main as main and has a lot of subOjects . wont pass bz : meshF == null
        if (go == null || go.transform == null)//if meshF == null is a 'Main' with subOjects in it
        {
            return;
        }
        _totalVertices += ReturnVertices(go, id);

        if (_totalVertices > 63500)
        {
            Debug.Log("vert cnt over 63.5K " + _id);
            return;
        }

        _all[_emptySpot] = go;
        _keymap.Add(id, _emptySpot);

        DefineNextEmptySpot();
    }

    private string ReturnProperID(General go)
    {
        if (go.Category == Ca.Spawn)
        {
            return go.MyId + "+";
        }
        return go.MyId;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="go"></param>
    /// <param name="destroyGameObj">If true will destroy the game object is going to be removed</param>
    private void RemoveFromAll(General go, bool destroyGameObj = false)
    {
        var myID = ReturnProperID(go);
        var indexes = _keymap.Where(a => a.Key.Contains(myID)).OrderBy(a => a.Value).ToList();

        //setting the lowest int value to be the emptySpot
        if (indexes.Count > 0)
        {
            _emptySpot = indexes[0].Value;
        }

        for (int i = 0; i < indexes.Count; i++)
        {
            var index = indexes[i].Value;
            var key = indexes[i].Key;

            //so it seen
            _all[index].SetActive(true);
            ActivateAllChildsObj(_all[index]);
            //asign it back to original gamebject
            _all[index].transform.SetParent(go.transform);
            _totalVertices -= ReturnVertices(_all[index], myID + "(not sure if Id Correct)");

            if (destroyGameObj)
            {
                GameObject.Destroy(_all[index].gameObject);
            }

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

        throw new Exception("Region Full : " + _id);
    }

    internal void AddToRegion(General go)
    {
        go.BatchRegionId = _id;

        if (go.Category == Ca.Structure || go.Category == Ca.Shore || go.Category == Ca.DraggableSquare)
        {
            //bz they update as user clicks right click ore new roads are added
            if (_keymap.ContainsKey(go.MyId) && go.Category == Ca.DraggableSquare)
            {
                RemoveFromAll(go, true);
                DecideIfRedoBatch();
            }
            //bz Strcuture calls twice
            else if (_keymap.ContainsKey(go.MyId))
            {
                return;
            }

            InspectGameObject(go.Geometry, go);
            AddToAll(ReturnObjectToMeshUp(go), go.MyId);
            DecideIfRedoBatch();
        }
        else if (go.Category == Ca.Spawn || go.HType == H.Plant)
        {
            if (go.HType == H.Tree)
            {
                //Debug.Log("Return tree");
                return;
            }

            //find all subGameObjects and add
            FindAllChildObjectsAndAddThem(go);
        }
    }

    private GameObject ReturnObjectToMeshUp(General go)
    {
        if (go.Category == Ca.DraggableSquare)
        {
            //returning the first son of the first son. bz on smartCreatePlanes is like that
            return go.transform.GetChild(1).transform.GetChild(0).gameObject;
        }

        return go.Geometry.gameObject;
    }

    private void FindAllChildObjectsAndAddThem(General go)
    {
        var subs = General.FindAllChildsGameObjectInHierarchy(go.gameObject);

        for (int i = 0; i < subs.Length; i++)
        {
            AddToAll(subs[i], go.MyId + "+" + i);
        }
    }

    /// <summary>
    /// bz techos de guano needs has alphaAtlas material
    /// </summary>
    /// <param name="geometry"></param>
    /// <param name="mainGen"></param>
    private void InspectGameObject(GameObject geometry, General mainGen)
    {
        var subs = General.FindAllChildsGameObjectInHierarchy(geometry);

        for (int i = 0; i < subs.Length; i++)
        {
            //if is a roof will take it back
            if (subs[i].name.Contains("Guano"))
            {
                subs[i].transform.SetParent(mainGen.transform);
            }
        }
    }

    internal void Remove(General go, bool redo = true)
    {
        RemoveFromAll(go);
        if (redo)
        {
            DecideIfRedoBatch();
        }
    }

    internal bool IsClose()
    {
        if (_totalVertices > 60000)
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

        if (_batchMaster != null)
        {
            var child = General.FindAllChildsGameObjectInHierarchy(_batchMaster.gameObject);
            for (int i = 0; i < child.Length; i++)
            {
                //if is child of batchMaster needs to get off him. need to ask tht in case a object has subobjects
                if (child[i].transform.parent == _batchMaster.transform)
                {
                    //so doesnt get wiped when destoryed
                    child[i].transform.SetParent(null);
                }
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
    private void StatKeeper(int newVal)
    {
        if (newVal > highestInd)
        {
            highestInd = newVal;
            Debug.Log("Highes array:" + highestInd);
        }
        if (_totalVertices > highestVert)
        {
            highestVert = _totalVertices;
            Debug.Log("highest Vert :" + _totalVertices + ", id:" + _id);
        }
    }

    private void CreateBatchMesh()
    {
        if (_batchMaster == null)
        {
            _batchMaster = General.Create(Root.classesContainer, new Vector3(),
                _id, Program.MeshBatchContainer.transform);
        }

        int nullCt = 0;
        for (int i = 0; i < _all.Length; i++)
        {
            if (_all[i] == null && nullCt > 99)
            {
                //at 10 cts in a round of null means the last of the Array was reached
                StatKeeper(i - 100);
                break;
            }
            if (_all[i] == null)
            {
                nullCt++;
                continue;
            }

            nullCt = 0;
            _all[i].SetActive(true);//in case this is now redoing a Region
            ActivateAllChildsObj(_all[i]);
            _all[i].transform.SetParent(_batchMaster.transform);
        }
        CombineMeshes(_batchMaster.gameObject, ReturnProperMaterial());
    }

    private Material ReturnProperMaterial()
    {
        if (_id.Contains("Opaque"))
        {
            return (Material)Resources.Load(Root.matTavernBase);
        }
        if (_id.Contains("Farm"))
        {
            return (Material)Resources.Load(Root.plantAtlas);
        }
        if (_id.Contains("Road"))
        {
            return (Material)Resources.Load(Root.roadAtlas);
        }
        return (Material)Resources.Load(Root.alphaAtlas);
    }

    /// <summary>
    /// In case an GameObj has child they need to be activate
    /// </summary>
    /// <param name="go"></param>
    private void ActivateAllChildsObj(GameObject go)
    {
        var child = General.FindAllChildsGameObjectInHierarchy(go);

        for (int i = 0; i < child.Length; i++)
        {
            child[i].SetActive(true);
        }
    }

    private void CombineMeshes(GameObject onGO, Material mat)
    {
        //Zero transformation is needed because of localToWorldMatrix transform
        Vector3 position = onGO.transform.position;
        onGO.transform.position = Vector3.zero;
        onGO.AddComponent<MeshFilter>();
        onGO.AddComponent<MeshRenderer>();

        MeshFilter[] meshFilters = onGO.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        //to avoid Combine mesh instance 0 is null.
        if (_totalVertices == 0 || meshFilters.Length == 0 || combine.Length == 0)
        {
            return;
        }

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

    internal void Destroy()
    {
        _batchMaster.Destroy();
        _batchMaster = null;

        if (_batchPivotContainer == null)
        {
            var a = _id;
            return;
        }

        _batchPivotContainer.Destroy();
        _batchPivotContainer = null;
    }

    internal bool IsAlive()
    {
        return _batchMaster != null;
    }

    #region Scale _batchMaster

    private float _scaleYCap = 2f;
    private General _batchPivotContainer;//created so the pivot point is in the middle of this

    /// <summary>
    /// Will scale the Combined Mesh GameObj
    /// </summary>
    /// <param name="newScale"></param>
    public void ScaleBatchedGO(Vector3 newScale)
    {
        if (_batchMaster == null)
        {
            return;
        }
        if (_batchPivotContainer == null)
        {
            CreateBatchPivot();
            //now nest the _batchMaster inside the pivot GO
            _batchMaster.transform.SetParent(_batchPivotContainer.transform);
        }
        if (_batchPivotContainer.gameObject.transform.localScale.y > _scaleYCap)
        {
            return;
        }

        //now I scale the Pivot GO that contains the _batchMaster inside
        var parent = _batchPivotContainer.gameObject.transform.parent;
        _batchPivotContainer.gameObject.transform.SetParent(null);

        var localScale = _batchPivotContainer.gameObject.transform.localScale;
        var final = localScale + newScale;
        _batchPivotContainer.gameObject.transform.localScale = final;

        _batchPivotContainer.gameObject.transform.SetParent(parent);
    }

    private void CreateBatchPivot()
    {
        _batchPivotContainer = General.Create(Root.classesContainer, _all[0].transform.position,
            _batchMaster.name + ".Pivot",
            _batchMaster.transform.parent.transform);
    }

    #endregion Scale _batchMaster
}