using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// Enemies will only be shown once they will go to work that is
/// to attack a town 
/// </summary>
public class EnemyManager
{
    float _firtSpawnsAt = 1;
    float _othersSpawnsAt = 121;
    float _lastSpawn = -1;
    //every 8 adult will spawn 1 enemy
    int _adultPer1Enemy = 8;

    EnemiesAgent _currentEnemy;

    public EnemyManager()
    {

    }


    public void Update()
    {
        CheckIfSpawnEnemy();

        if (_currentEnemy!=null)
        {
            _currentEnemy.Update();
        }
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

    private void SpawnThisManyEnemies()
    {
        _lastSpawn = Time.time;

        int howMany = HowManyEnemiesSpawnNow();
        if (howMany < 1)
        {
            return;
        }

        //Spawns enemy
        _currentEnemy = new EnemiesAgent(howMany);
    }

    private int HowManyEnemiesSpawnNow()
    {
        var adult = PersonPot.Control.All.Count(a => a.Age >= JobManager.majorityAge);
        return adult / _adultPer1Enemy;
    }
}

