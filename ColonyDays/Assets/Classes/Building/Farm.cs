using UnityEngine;
using System.Collections;

/*
 * This class is the parent of Field Farm
 */
public class Farm  {

    //how much work the farmers have done so far in this farm 
    protected float _workAdded;

    protected bool _isReadyToHarvest;

    protected bool _harvestNow;

    /// <summary>
    /// When a worker works in the farm 
    /// </summary>
    public void AddWorkToFarm()
    {
        if (_isReadyToHarvest)
        {
            Harvest();
            return;
        }

        //so its fair bz the amount of time pass is changed by the speed in the power 
        //+info@: GameTime.FixedUpdate()
        var add = 1000 * Program.gameScene.GameTime1.TimeFactorInclSpeed();
        _workAdded += add;
    }

    private void Harvest()
    {
        _harvestNow = true;
    }


}
