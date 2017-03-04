using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// Will find right spot to spawn the enemies
/// </summary>
public class EnemiesAgent
{
    int _howMany;
    List<Person> _enemies = new List<Person>();
    Vector3 _iniPoint;

    public EnemiesAgent(int howMany)
    {
        _howMany = howMany;
        FindSpawningPoint();
        SpawnEnemies();
    }

    private void FindSpawningPoint()
    {
        //find iniPoint
        _iniPoint = Program.gameScene.controllerMain.TerraSpawnController.ReturnCenterPosOfLockedNearbyRegion();
    }

    private void SpawnEnemies()
    {
        for (int i = 0; i < _howMany; i++)
        {
            _enemies.Add(Person.CreatePersonEnemy(_iniPoint));
        }
    }



    public void Update()
    {

    }
}

