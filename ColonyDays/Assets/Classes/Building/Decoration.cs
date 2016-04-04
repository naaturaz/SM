using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Will decorate the surroundings of a building 
/// </summary>
public class Decoration  {

    List<string> _roots = new List<string>()
    {
        "Prefab/Terrain/Spawner/Decora/Crate 1",
        "Prefab/Terrain/Spawner/Decora/Crate 2",
        "Prefab/Terrain/Spawner/Decora/Crate 3",
        "Prefab/Terrain/Spawner/Decora/Crate 4",
        "Prefab/Terrain/Spawner/Decora/Crate 5",
        "Prefab/Terrain/Spawner/Decora/Barrel 1",
        "Prefab/Terrain/Spawner/Decora/Barrel 2",
        "Prefab/Terrain/Spawner/Decora/RusticChair",
    };

    

    

    private Building _building;
    List<Line> _lines = new List<Line>(); 

    List<Vector3> _positionsToSpawnDecor=new List<Vector3>(); 
    List<General> _spwnedObj = new List<General>(); 

    public Decoration(Building build)
    {
        return;

        _building = build;
        //_roots.AddRange(TerrainSpawnerController.allOrna);
        //_roots.AddRange(TerrainSpawnerController.allGrass);
        Init();
    }

    private void Init()
    {
        _lines = U2D.FromPolyToLines(_building.Anchors);
        RemoveSpwnPointLine();
        FindPositionToSpwnDecor();
        //SpawnDecorObj();
        IfHouseMedAssignRandomMat();
    }




    #region Romeo Bravo Pirate

    private Material _romeo;
    private Material _bravo;
    private Material _pirate;
    private GameObject _main;
    private void IfHouseMedAssignRandomMat()
    {
        if (_building.HType != H.HouseMed)
        {
            return;
        }
        DefineRamdonMat();

        _main = General.GetChildThatContains("Main", _building.gameObject);
        FindAllSubObjsAndAssignMat();
    }

    private void FindAllSubObjsAndAssignMat()
    {
        var romeos = General.GetChildsNameEqual(_main, "Romeo");
        var pirates = General.GetChildsNameEqual(_main, "Pirate");
        var bravos = General.GetChildsNameEqual(_main, "Bravo");

        AssignMat(romeos, _romeo);
        AssignMat(pirates, _pirate);
        AssignMat(bravos, _bravo);
    }

    void AssignMat(List<GameObject> list, Material mat)
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i].GetComponent<Renderer>().sharedMaterial = mat;
        }
    }

    private void DefineRamdonMat()
    {
        _romeo = (Material)Resources.Load("Prefab/Mats/Building/BuildsFactory/Romeo"+UMath.GiveRandom(1,4));
        _bravo = (Material)Resources.Load("Prefab/Mats/Building/BuildsFactory/Bravo"+UMath.GiveRandom(1,4));
        _pirate = (Material)Resources.Load("Prefab/Mats/Building/BuildsFactory/Pirate"+UMath.GiveRandom(1,4));
    }




#endregion

    /// <summary>
    /// The line facing spawnPoint wil be removed
    /// </summary>
    void RemoveSpwnPointLine()
    {
        var st = (Structure)_building;
        //moveing the spwn point away from building . assuimng is fallin inside building 
        var spwPoint = Vector3.MoveTowards(st.SpawnPoint.transform.position, _building.transform.position, -.75f);

        var spwnLine = new Line(U2D.FromV3ToV2(spwPoint), U2D.FromV3ToV2(_building.transform.position));
        for (int i = 0; i < _lines.Count; i++)
        {
            if (_lines[i].IsIntersecting(spwnLine))
            {
                _lines.RemoveAt(i);
                break;
            }
        }
    }

    private void FindPositionToSpwnDecor()
    {
        for (int i = 0; i < _lines.Count; i++)
        {
            _positionsToSpawnDecor.AddRange(_lines[i].ReturnRandomPointsInLine());
        }
    }

    private void SpawnDecorObj()
    {
        for (int i = 0; i < _positionsToSpawnDecor.Count; i++)
        {
            var root = _roots[UMath.GiveRandom(0, _roots.Count)];
            
            //moving a bit twrds buildings
            var iniPos = Vector3.MoveTowards(_positionsToSpawnDecor[i], _building.transform.position, .2f);

            var spwnObj = General.Create(root, iniPos, name:"Decora", container: _building.transform);
            RandomizeRotAndScale(spwnObj.gameObject, root);

            _spwnedObj.Add(spwnObj);
        }
    }

    void RandomizeRotAndScale(GameObject spwnObj, string root)
    {
        //return;


        spwnObj.transform.Rotate(new Vector3(0, UMath.GiveRandom(0, 360), 0));

        //they are well scaled 
        if (root.Contains("Prefab/Terrain/Spawner/Decora"))
        {
            return;
        }


        //ScaleDownTerrainSpawners
        var actScale = spwnObj.transform.localScale;
        spwnObj.transform.localScale = actScale/2.5f;//2
    }


}
