using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CUIAssignWorkersWindow : MonoBehaviour
{
    public WorkerTile WorkerTile;
    private bool reDoneSalaries;//for users  that changed salaries already
    private List<string> _newList;
    private List<WorkerTile> _tiles = new List<WorkerTile>();
    private int _count;

    private void Start()
    {
        ShowWorkers();

        if (WorkerTile == null)
            throw new Exception("Please set Worker Tile");
    }

    private void Update()
    {
        if (_count > 30)
        {
            _count = 0;
            var a = BuildingPot.Control.Registro.StringOfAllBuildingsThatAreAWork().Distinct().ToList();
            if (a.Count != _newList.Count)
            {
                ShowWorkers();
            }
        }
        _count++;
    }

    public void ShowWorkers()
    {
        if (!reDoneSalaries)
        {
            reDoneSalaries = true;
            BuildingPot.Control.Registro.DoEquealPaymentForAllWorks();
        }

        _newList = BuildingPot.Control.Registro.StringOfAllBuildingsThatAreAWork().Distinct().ToList();
        ShowWorkers(_newList);
    }

    private void ShowWorkers(List<string> list)
    {
        //create if not there
        for (int i = 0; i < list.Count; i++)
        {
            var found = _tiles.Find(a => a.WorkType == list[i]);
            if (found == null)
            {
                var tile = WorkerTile.CreateTile(gameObject.transform, list[i], new Vector3(), WorkerTile);
                _tiles.Add(tile);
            }
        }

        //delete if job doesnt exist anymore
        for (int i = 0; i < _tiles.Count; i++)
        {
            var found = list.Find(a => a == _tiles[i].WorkType);
            if (found == null)
            {
                Destroy(_tiles[i].gameObject);
                _tiles.RemoveAt(i);
                i--;
            }
        }
    }
}