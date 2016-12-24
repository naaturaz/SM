using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class BuyRegionManager
{
    //regions that are being unlock
    List<int> _unlockRegions = new List<int>();

    //regions that are not being unlock. they are up for sale 
    List<int> _forSaleRegions = new List<int>();
 
    List<ForSaleRegionGO> _forSaleRegionGoes = new List<ForSaleRegionGO>();

    //the region we might be able to unlock now
    private int _currentRegion;


    public BuyRegionManager()
    {
        if (TownLoader.IsTemplate)
        {
            return;
        }

        LoadUnlockRegions();
        AssignUnlockRegionsToNewGame();

        FindAllRegionsAndClean();
        SpawnForSaleOnTerrain();
    }

    public List<int> UnlockRegions
    {
        get { return _unlockRegions; }
        set { _unlockRegions = value; }
    }

    private void FindAllRegionsAndClean()
    {
        var all = MeshController.CrystalManager1.CrystalRegions;

        //find all regions
        for (int i = 0; i < all.Count; i++)
        {
            _forSaleRegions.Add(all[i].Index);
        }

        //removes the unlock regions from the _forSaleRegions
        for (int i = 0; i < _unlockRegions.Count; i++)
        {
            _forSaleRegions.Remove(_unlockRegions[i]);
        }
    }

    private void SpawnForSaleOnTerrain()
    {
        for (int i = 0; i < _forSaleRegions.Count; i++)
        {
            var index = _forSaleRegions[i];
            var reg = MeshController.CrystalManager1.CrystalRegions[index].Region;

            //if is not inland will remove it from Sale will added to unlock 
            if (MeshController.CrystalManager1.CrystalRegions[index].WhatAudioIReport != "InLand")
            {
                _unlockRegions.Add(i);
                continue;
            }

            _forSaleRegionGoes.Add(ForSaleRegionGO.CreateForSaleRegionGO(Root.forSaleRegion, index, 
                reg, container: Program.gameScene.Terreno.transform));
        }

        //removes the unlock regions from the _forSaleRegions
        for (int i = 0; i < _unlockRegions.Count; i++)
        {
            _forSaleRegions.Remove(_unlockRegions[i]);
        }
    }

    /// <summary>
    /// Assign the first lot of regions 
    /// </summary>
    private void AssignUnlockRegionsToNewGame()
    {
        if (_unlockRegions.Count > 0)
        {
            return;
        }

        //var firstBuild = BuildingPot.Control.Registro.AllBuilding.ElementAt(0).Value.transform.position;
        var middleOfTown = BuildingPot.Control.Registro.AverageOfAllBuildingsNow();
        var first1x1Regions = MeshController.CrystalManager1.ReturnMySurroundRegions(U2D.FromV3ToV2(middleOfTown), 1);

        _unlockRegions.AddRange(first1x1Regions);
    }

   
    private void LoadUnlockRegions()
    {
        PersonData pData = XMLSerie.ReadXMLPerson();

        //if new game
        if (pData == null)
        {
            return;
        }

        //it saves at PersonSaveLoad
        _unlockRegions = pData.PersonControllerSaveLoad.UnlockRegions;
    }

    private int moneyMul = 100;
    public bool HasEnoughResourcesToBuy()
    {
        var moneyNeeded = MoneyNeeded();

        var hasMoney = Program.gameScene.GameController1.Dollars > moneyNeeded;

        return hasMoney;
    }

    public string Cost()
    {
        return "Money: " + MoneyNeeded();
    }

    float MoneyNeeded()
    {
        if (Developer.IsDev && Input.GetKey(KeyCode.F9))
        {
            return 0;
        }

        return moneyMul * _unlockRegions.Count ;
    }

    private void RemoveCost()
    {
        Program.gameScene.GameController1.Dollars -= MoneyNeeded();
    }


    internal void SetCurrentRegion(int indexP)
    {
        _currentRegion = indexP;
    }

    public void CurrentRegionBuy()
    {
        //play sound
        Program.gameScene.GameController1.NotificationsManager1.Notify("BoughtLand");

        RemoveCost();

        _unlockRegions.Add(_currentRegion);
        _forSaleRegions.Remove(_currentRegion);

        var gO = _forSaleRegionGoes.Find(a => a.Index == _currentRegion);
        gO.Destroy();
    }

    /// <summary>
    /// bz FullOceans dont need to be spanwed 
    /// </summary>
    /// <param name="indexP"></param>
    public void DestroyForSaleGO(int indexP)
    {
        var gO = _forSaleRegionGoes.Find(a => a.Index == indexP);
        gO.Destroy();
    }



    internal bool AreAnchorsOnUnlockRegions(List<UnityEngine.Vector3> anchors)
    {
        var anchorRegions = new List<int>();
        for (int i = 0; i < anchors.Count; i++)
        {
            anchorRegions.Add(MeshController.CrystalManager1.ReturnMyRegion(U2D.FromV3ToV2(anchors[i])));
        }
        anchorRegions = anchorRegions.Distinct().ToList();

        for (int i = 0; i < anchorRegions.Count; i++)
        {
            //if doest contain one is false
            if (!_unlockRegions.Contains(anchorRegions[i]))
            {
                return false;
            }
        }
        return true;
    }
}
