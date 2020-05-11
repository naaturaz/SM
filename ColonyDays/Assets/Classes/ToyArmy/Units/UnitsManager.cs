using System.Collections.Generic;
using UnityEngine;

public class UnitsManager : General
{
    private List<UnitT> _units = new List<UnitT>();

    public List<UnitT> Units
    {
        get
        {
            return _units;
        }

        set
        {
            _units = value;
        }
    }

    // Use this for initialization
    private void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void Create(string buildingPath)
    {
        var howMany = 1;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            howMany = 5;
        }

        for (int i = 0; i < howMany; i++)
        {
            SpawnPlayerUnit(buildingPath);
        }
    }

    private void SpawnPlayerUnit(string buildingPath)
    {
        //if (!Building.DoWeHavePowerToBuildThis(buildingPath))
        //{
        //    return;
        //}

        var randomNessPos = new Vector3(UMath.RandomSign() * UMath.GiveRandom(.2f, .3f), 0,
        UMath.RandomSign() * UMath.GiveRandom(.1f, .2f));

        //var pos = Rocket.transform.position + new Vector3(0, 0, -5) + randomNessPos;
        //var u = UnitT.CreateU("Prefab/Units/" + buildingPath, pos, buildingPath, transform);
        //u.SetToSoil();

        ////add to cell
        //Units.Add(u);
    }

    public void CreateFixed(string buildingPath, Vector3 pos)
    {
        var u = UnitT.CreateU("Prefab/Units/" + buildingPath, pos, buildingPath, transform);
        u.WasFixed = true;
        //add to cell
        Units.Add(u);
    }

    internal void AddToAll(UnitT unit)
    {
        _units.Add(unit);
    }

    public void DestroyAllUnits()
    {
        for (int i = 0; i < _units.Count; i++)
        {
            Destroy(_units[i].gameObject);
        }
    }

    internal void RemoveUnit(UnitT u)
    {
        _units.Remove(u);
    }

    public Transform GiveMeClosestUnit(Vector3 from)
    {
        var tMin = GetClosestEnemy(Units, from);
        if (tMin == null)
        {
            //tMin = Program.gameScene.BuildingManager.GetClosestBuild(from);
            tMin = BuildingController.FindTheClosestOfContainTypeFullyBuilt("Storage", from).transform;
        }

        return tMin;
    }

    private Transform GetClosestEnemy(List<UnitT> units, Vector3 from)
    {
        Transform tMin = null;
        float minDist = Mathf.Infinity;

        for (int i = 0; i < units.Count; i++)
        {
            if (units[i] == null)
            {
                continue;
            }

            float dist = Vector3.Distance(units[i].transform.position, from);
            if (dist < minDist)
            {
                tMin = units[i].transform;
                minDist = dist;
            }
        }

        return tMin;
    }

    public void HonorRank()
    {
        if (Units.Count == 0)
        {
            return;
        }

        var u = Units[UMath.GiveRandom(0, Units.Count)];

        if (u != null)
        {
            u.RankUp();
        }
    }
}