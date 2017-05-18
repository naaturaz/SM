using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    float _firtSpawnsAt = 1;
    float _othersSpawnsAt = 10;//120
    float _lastSpawn = -1;

    //every 8 adult will spawn 1 enemy
    int _personPer1Enemy = 4;//8


    private Vector3 _iniPoint;

    public EnemyManager()
    {

    }


    public void LateUpdate()
    {
        if (MeshController.BuyRegionManager1 == null)
        {
            return;
        }

        CheckIfSpawnEnemy();
    }

    private void CheckIfSpawnEnemy()
    {
        if (_lastSpawn == -1 && Time.time > _firtSpawnsAt)
        {
            SpawnThisManyEnemies();
        }
        else if (_lastSpawn != -1 && Time.time > _lastSpawn + _othersSpawnsAt)
        {
            SpawnThisManyEnemies();
        }
    }

    private void SpawnEnemies(int many)
    {
        for (int i = 0; i < many; i++)
        {
            SpawnEnemy();
        }
    }

    private void SpawnThisManyEnemies()
    {
        if (_iniPoint == new Vector3())
        {
            _iniPoint = MeshController.BuyRegionManager1.ReturnCenterPosOfLockedNearbyRegion();
        }


        _lastSpawn = Time.time;

        int howMany = HowManyEnemiesSpawnNow();
        if (howMany < 1)
        {
            return;
        }

        SpawnEnemies(howMany);
    }

    private int HowManyEnemiesSpawnNow()
    {
        var adult = PersonPot.Control.All.Count;
        return adult / _personPer1Enemy;
    }

























    float _nextWaveAt;
    int _nextWaveEnemies;

    List<UnitT> _enemies = new List<UnitT>();
    List<Transform> _enemiesTransform = new List<Transform>();

    Vector3 _spawnPos;
    int _xSing;
    int _zSign;

    int _waveNumb = 1;
    int _kills;

    float _thisGameStartedAt;

    Building _base;

    // Use this for initialization
    void Start()
    {
        _thisGameStartedAt = Time.time;
        _xSing = UMath.RandomSign();
        _zSign = UMath.RandomSign();

        var currLevel = PlayerPrefs.GetInt("Current");
        _nextWaveAt = Time.time + 9 + currLevel;
    }




    List<string> _addBuilds = new List<string>() {"Prefab/Building/Militar/Bunker",
        "Prefab/Building/Militar/Defense_Tower"
    };
    string ReturnRandomAddBuilding()
    {
        return _addBuilds[UMath.GiveRandom(0, _addBuilds.Count)];
    }

    internal bool ThereIsAnAttackNow()
    {
        return _enemies.Count > 0;
    }

    // Update is called once per frame
    void Update()
    {
        ////player should not be on air other wise nav weird message 
        //if (Time.time > _nextWaveAt)
        //{
        //    count = 0;
        //    var levlDif = 1;

        //    SetNextWave();
        //    _nextWaveEnemies = _waveNumb * (UMath.GiveRandom(1 + levlDif, 6 + levlDif));
        //    Program.gameScene.CameraK.Attack();

        //    _waveNumb++;
        //}
        //SpawnEnemies();

    }

    void SetNextWave()
    {
        _nextWaveAt = Time.time + 90;
    }


    int count = -1;
    void SpawnEnemies()
    {
        if (count > -1 && count < _nextWaveEnemies)
        {
            SpawnEnemy();
            count++;
        }
        else if (count >= _nextWaveEnemies)
        {
            count = -1;
        }
    }


    void SpawnEnemy()
    {
        if (Program.gameScene == null)
        {
            return;
        }
        var root = ReturnRandomAttackUnit();
        SpawnEnemy(root, _iniPoint);
    }


    void SpawnEnemy(string rootP, Vector3 spawnPos)
    {
        var ene = UnitT.CreateU(rootP, spawnPos, "Enemy." + rootP, null);
        _enemies.Add(ene);
        _enemiesTransform.Add(ene.transform);
    }

    public void SpawnInfantry(Vector3 pos)
    {
        SpawnEnemy("Prefab/TA/Units/Infantry", pos);
    }



    List<string> _attackUnit = new List<string>() {
        "Prefab/TA/Units/Infantry",
    };
    string ReturnRandomAttackUnit()
    {
        return _attackUnit[UMath.GiveRandom(0, _attackUnit.Count)];
    }



    public Transform GiveMeClosestEnemy(Vector3 from)
    {
        return GetClosestEnemy(_enemiesTransform, from);
    }

    Transform GetClosestEnemy(List<Transform> enemies, Vector3 from)
    {
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        foreach (Transform t in enemies)
        {
            if (t == null)
            {
                continue;
            }

            float dist = Vector3.Distance(t.position, from);
            if (dist < minDist)
            {
                tMin = t;
                minDist = dist;
            }
        }
        return tMin;
    }

    internal void RemoveMeFromEnemiesList(UnitT enemyGO)
    {
        var index = _enemies.FindIndex(a => a == enemyGO);

        if (index < 0)
        {
            return;
        }

        Program.gameScene.UnitsManager.HonorRank();

        _enemies.RemoveAt(index);
        _enemiesTransform.RemoveAt(index);

        _kills++;

        if (_enemies.Count == 0)
        {
            Program.gameScene.EnemyManager.Peace();
            //Program.gameScene.CameraK.Peace();
        }
    }

    private void Peace()
    {
        throw new NotImplementedException();
    }

    internal float TtlTimeOfCurrentGame()
    {
        return Time.time - _thisGameStartedAt;
    }

    internal int Kills()
    {
        return _kills;
    }


    public void DestroyAllUnits()
    {

        for (int i = 0; i < _enemies.Count; i++)
        {
            Destroy(_enemies[i].gameObject);
        }
    }
}
