﻿using System.Collections.Generic;
using UnityEngine;

/*
 * This class is contained in Structure.cs and is the one that handles all with a Field Farm
 */

public class FieldFarm : Farm
{
    private SMe m = new SMe();

    private Structure _building; //the building structure contains the Field Farm
    private List<Plant> _plants = new List<Plant>();
    private List<Vector3> _seedLoc = new List<Vector3>(); //the location of the seeds in this Field Farm
    private P _plantType;

    private float _spaceBtwPlants; //this is the space btw plants will be different for each

    //of the FarmZone
    private Vector3 _NW;

    private Vector3 _SE;

    private Vector3 _min;
    private Vector3 _max;

    //the total produuced
    private int _kgProduced;

    private PlantSave _plantSave;

    public FieldFarm()
    {
    }

    public FieldFarm(Structure building)
    {
        _building = building;
        _plantType = building.CurrentProd.Product;
        Init();
    }

    public FieldFarm(Structure building, PlantSave plant)
    {
        _plantSave = plant;
        _building = building;
        _plantType = plant.Type;
        Init();
    }

    public MDate HarvestDate()
    {
        if (_plants.Count > 0)
        {
            return _plants[0].HarvestDate();
        }
        return null;
    }

    internal string PercentageDone()
    {
        if (_plants.Count > 0)
        {
            return _plants[0].PercentageDone();
        }
        return null;
    }

    public float SpaceBtwPlants
    {
        get { return _spaceBtwPlants; }
    }

    private void Init()
    {
        FindWherePlantSeeds();
        CreatePlants();
    }

    /// <summary>
    /// Here depeding on the type of plat will find the seed points
    /// </summary>
    private void DefineSpaceBtnPlants()
    {
        _spaceBtwPlants = Mathf.Abs(m.SubDivide.XSubStep) / 2.5f; //1.5

        if (_plantType == P.Banana)
        {
            _spaceBtwPlants *= 2;
        }
        else if (_plantType == P.Coconut)
        {
            _spaceBtwPlants *= 3;
        }
    }

    private bool createPlantNow;
    private int creaCount;

    /// <summary>
    /// After we got the Location of the seeds will procede to plant seeds
    /// </summary>
    private void CreatePlants()
    {
        createPlantNow = true;
    }

    private void CreatePlantsLoop()
    {
        if (creaCount < _seedLoc.Count)
        {
            var plantNew = Plant.Create(_plantType, _seedLoc[creaCount], _building, this);

            if (_plantSave != null)
            {
                plantNew.LoadPlant(_plantSave);
            }

            _plants.Add(plantNew);
            creaCount++;
        }
        else
        {
            createPlantNow = false;
            creaCount = 0;
            BatchInitial();
        }
    }

    private void FindWherePlantSeeds()
    {
        PullFarmZoneVars();
        DefineFarmNWandSE();
        LoopToFindSeeds();
    }

    private void DefineFarmNWandSE()
    {
        var poly = UPoly.RetTerrainPoly(_min, _max, Dir.SWtoNE);
        _NW = poly[0];
        _SE = poly[2];
    }

    /// <summary>
    /// Will loop considering _min, _max and will define seeding Vectors Locations
    /// </summary>
    private void LoopToFindSeeds()
    {
        if (_spaceBtwPlants == 0)
        {
            DefineSpaceBtnPlants();
        }
        _seedLoc = RetuFillPoly(_NW, _SE, _spaceBtwPlants, _spaceBtwPlants);
        //Debug.Log(_building.MyId + ". seeds: " + _seedLoc.Count);
        //UVisHelp.CreateHelpers(_seedLoc, Root.blueCube);
    }

    private List<Vector3> RetuFillPoly(Vector3 NW, Vector3 SE, float xStep, float zStep)
    {
        List<Vector3> res = new List<Vector3>();

        for (float x = NW.x; x < SE.x; x += xStep)
        {
            for (float z = NW.z; z > SE.z; z -= zStep)
            {
                res.Add(new Vector3(x, NW.y, z));
            }
        }
        return res;
    }

    private void PullFarmZoneVars()
    {
        var farmZone = _building.FarmZone();
        _min = farmZone.GetComponent<Collider>().bounds.min;
        _max = farmZone.GetComponent<Collider>().bounds.max;
    }

    /// <summary>
    /// This is call from the plant and will be given the amout of work
    /// they are entitled to. ex: in te small farm in Corn and Bean are 24 plants each will get a 1/24 of the
    /// total amount worked
    /// </summary>
    /// <returns></returns>
    public float GiveMeMyWorkedAmt()
    {
        //var amt = _workAdded/_plants.Count;
        //_workAdded -= amt;
        return _workAdded;
    }

    private int plantsCount;

    /// <summary>
    /// Use to keep track of plants grw once all of them grw will make _workAdded = 0
    /// </summary>
    internal void PlantGrew()
    {
        plantsCount++;

        if (plantsCount >= _plants.Count)
        {
            plantsCount = 0;
            _workAdded = 0;
        }
    }

    private int harvestCount;

    internal void HarvestCheck()
    {
        harvestCount++;

        if (harvestCount >= _plants.Count)
        {
            _isReadyToHarvest = true;
        }
    }

    public void Update()
    {
        if (_harvestNow)
        {
            HarvestNowTheField();
        }

        if (createPlantNow)
        {
            CreatePlantsLoop();
        }
    }

    private void HarvestNowTheField()
    {
        for (int i = 0; i < _plants.Count; i++)
        {
            _plants[i].Harvest();
        }

        _building.DestroyFarm();
    }

    /// <summary>
    /// Called from Building when a Product was changed
    /// </summary>
    public void ChangeProduct(P newProd)
    {
        if (newProd != _plantType)
        {
            //will make _farm null in Structure
            //Structure will handle the rest
            _building.DestroyFarm();
        }
    }

    #region Batching

    private BatchManager _batchManager;

    public void BatchAdd(General gen)
    {
        if (_batchManager == null)
        {
            _batchManager = new BatchManager(_building.MyId + ".Batch");
        }

        _batchManager.AddGen(gen);
    }

    internal void BatchRemove(General gen)
    {
        _batchManager.RemoveGen(gen);
    }

    private void BatchInitial()
    {
        _batchManager.BatchInitial();
    }

    public void BatchDestroy()
    {
        for (int i = 0; i < _plants.Count; i++)
        {
            if (_plants[i] != null)
            {
                _plants[i].Destroy();
            }
        }

        _batchManager.BatchDestroy();
    }

    /// <summary>
    /// Will scale all Batched GO
    /// </summary>
    /// <param name="add"></param>
    public void ScaleBatchedGO(Vector3 add)
    {
        _batchManager.ScaleAllRegionsBatchedGO(add);
    }

    #endregion Batching
}