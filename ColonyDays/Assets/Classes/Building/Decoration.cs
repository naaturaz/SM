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

    List<string> allOrna = new List<string>()
    { 
        Root.orna1, Root.orna2, Root.orna3, Root.orna4 , Root.orna5, Root.orna6,
         Root.orna7, Root.orna8
    };

    List<string> allGrass = new List<string>() 
    { 
        //Root.grass1, 
        Root.grass2, Root.grass3  ,
        Root.grass4, Root.grass5, Root.grass6,
        Root.grass7, 
        //Root.grass8,
        //Root.grass9,
        Root.grass11, Root.grass12,
    };

    private Building _building;
    List<Line> _lines = new List<Line>(); 

    List<Vector3> _positionsToSpawnDecor=new List<Vector3>(); 
    List<General> _spwnedObj = new List<General>(); 

    public Decoration(Building build)
    {
        _building = build;
        _roots.AddRange(allOrna);
        _roots.AddRange(allGrass);
        Init();
    }

    private void Init()
    {
        _lines = U2D.FromPolyToLines(_building.Anchors);
        RemoveSpwnPointLine();
        FindPositionToSpwnDecor();
        SpawnDecorObj();
    }




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
        spwnObj.transform.Rotate(new Vector3(0, UMath.GiveRandom(0, 360), 0));

        //they are well scaled 
        if (root.Contains("Prefab/Terrain/Spawner/Decora"))
        {
            return;
        }

        //ScaleDownTerrainSpawners
        var actScale = spwnObj.transform.localScale;
        spwnObj.transform.localScale = actScale/2.5;
    }


}
