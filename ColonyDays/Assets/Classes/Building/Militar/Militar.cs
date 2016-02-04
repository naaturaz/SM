using UnityEngine;
using System.Collections;

public class Militar
{
    private Building _building;
    private MDate _nextDate;

    public Militar(Building building)
    {
        // TODO: Complete member initialization
        _building = building;
        _nextDate = Program.gameScene.GameTime1.CurrentDate();
        _nextDate = Program.gameScene.GameTime1.ReturnCurrentDatePlsAdded(360);
    }

    void YearEffectOnPirateThreat()
    {
        //add stuff for multiplier 

        var amtChange = _building.PeopleDict.Count*-.1f*EffectOfBuildingType();

        BuildingPot.Control.DockManager1.AddToPirateThreat(amtChange);
    }

    private int EffectOfBuildingType()
    {
        if (_building.HType==H.PostGuard)
        {
            return 1;
            
        }
        if (_building.HType == H.Tower)
        {
            return 3;

        }
        if (_building.HType == H.Fort)
        {
            return 10;

        }
        if (_building.HType == H.Morro)
        {
            return 50;
        }

        return 0;
    }

    public void Update()
    {
        if (GameTime.IsPastOrNow(_nextDate))
        {
            _nextDate = Program.gameScene.GameTime1.ReturnCurrentDatePlsAdded(360);
            YearEffectOnPirateThreat();
        }
    }

}
