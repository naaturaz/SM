using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class BatchManager
{



    //here with an Int that is the CrystalRegion you can find to which Batch Region you belong to
    //mostly to be use by semiOpaque 
    Dictionary<int, string> _semiIndeces = new Dictionary<int, string>();  

    //stirng is the ID
    Dictionary<string, BatchRegion> _batchRegions = new Dictionary<string, BatchRegion>();

    //the Opaque region is open to add more GameObj
    private string _openOpaque;

    public BatchManager()
    {
        SemiOpaqueSetUp();
        Debug.Log("BathMnanager init");
    }

    private void SemiOpaqueSetUp()
    {
        int loc = 0;
        string id = "Semi";
        List<int> regions = new List<int>();

        for (int i = 0; i < MeshController.CrystalManager1.CrystalRegions.Count; i++)
        {
            if (loc < 3)
            {
                id += "."+i;
                regions.Add(i);
            }
            else
            {
                Set3BatchRegions(id, regions);
                loc = 0;
                id = "Semi";
                regions.Clear();
                i--;//so the 3 is included
                continue;
            }
            loc++;
        }
        //the last 3 never get to the 'else'
        Set3BatchRegions(id, regions);
        Debug.Log("_semiIndeces ct:" + _semiIndeces.Count + ". Cryst.Regions ct:" +
            MeshController.CrystalManager1.CrystalRegions.Count);
    }

    private void Set3BatchRegions(string id, List<int> regions)
    {
        for (int i = 0; i < regions.Count; i++)
        {
            _semiIndeces.Add(regions[i], id);
        }
        _batchRegions.Add(id, new BatchRegion(id));
    }

    /// <summary>
    /// means we are working on the creation of a new terrain 
    /// </summary>
    /// <returns></returns>
    public bool IsABrandNewTerrain()
    {
        if (_semiIndeces.Count == 0)
        {
            return true;
        }
        return false;
    }

    public void AddGen(General go)
    {
        if (IsABrandNewTerrain())
        {
            return;
        }

        //todo address if Road, Bridge
        if (go.Category == Ca.Spawn)
        {
            var i = MeshController.CrystalManager1.ReturnMyRegion(U2D.FromV3ToV2(go.transform.position));
            var id = _semiIndeces[i];
            _batchRegions[id].AddToRegion(go);
        }
        else if (go.Category == Ca.Structure)
        {
            return;

            Structure st = (Structure)go;
            if (st.StartingStage != H.Done && st.CurrentStage != 4 )
            {
                //only will add fully finished buildings 
                return;
            }

            HandleCurrentOpaque();
            _batchRegions[_openOpaque].AddToRegion(go);
        }
    }

    internal void RemoveGen(General gen)
    {
        //a tree tht is not being added to Batch
        if (gen == null || string.IsNullOrEmpty(gen.BatchRegionId))
        {
            return;
        }

        _batchRegions[gen.BatchRegionId].Remove(gen);
    }


    private int lastNewOpaque;
    private void HandleCurrentOpaque()
    {
        if (string.IsNullOrEmpty(_openOpaque))
        {
            _openOpaque = "Opaque." + lastNewOpaque;
            CreateNewOpaqueRegion();
        }
        else
        {
            if (_batchRegions[_openOpaque].IsClose())
            {
                lastNewOpaque++;
                _openOpaque = "Opaque." + lastNewOpaque;
                CreateNewOpaqueRegion();
            }
        }
    }

    void CreateNewOpaqueRegion()
    {
        _batchRegions.Add(_openOpaque, new BatchRegion(_openOpaque));
    }







    internal void BatchInitial()
    {
        for (int i = 0; i < _batchRegions.Count; i++)
        {
            _batchRegions.ElementAt(i).Value.DecideIfRedoBatch();
        }
    }

 
}

