using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/*
 * This is the one that will Resume all Storage Inventories 
 * 
 */

public class ResumenInventory {

    Inventory _gameInventory = new Inventory("GameInventory", H.None);

    /// <summary>
    /// All Game items are resumed here. Is updated from inventory.cs everytime something is added or removed
    /// </summary>
    public Inventory GameInventory
    {
        get { return _gameInventory; }
        set { _gameInventory = value; }
    }

    /// <summary>
    /// Will retruen the amt of Category in all inventories.
    /// 
    /// Used for GUI
    /// </summary>
    /// <param name="pCat"></param>
    /// <returns></returns>
    public float ReturnAmountOnCategory(PCat pCat)
    {
        if (Program.gameScene.GameFullyLoaded() && GameInventory.InventItems.Count == 0)
        {
            SetInitialGameInventory();
        }

        return GameInventory.ReturnAmountOnCategory(pCat);
    }

    /// <summary>
    /// Will retruen the amt of item in all inventories.
    /// 
    /// Used for GUI
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public float ReturnAmtOfItemOnInv(P item)
    {
        return GameInventory.ReturnAmtOfItemOnInv(item);
    }

    void SetInitialGameInventory()
    {
        var storages = BuildingController.FindAllStructOfThisTypeContain(H.Storage);

        for (int i = 0; i < storages.Count; i++)
        {
            GameInventory.AddItems(storages[i].Inventory.InventItems);
        }
    }

    /// <summary>
    /// Will tell u if the item is on one of the inventorires 
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool IsItemOnInv(P item)
    {
        var storages = BuildingController.FindAllStructOfThisTypeContain(H.Storage);

        for (int i = 0; i < storages.Count; i++)
        {
            if (storages[i].Inventory.IsItemOnInv(item))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Will remove the item and the amount from the inventories.
    /// 
    /// This method is use for an building was built and 40 wood for ex needs to be removed 
    /// 
    /// Will loop thru the storages until remove the full amt
    /// </summary>
    /// <param name="item"></param>
    /// <param name="amt"></param>
    public void Remove(P item, float amt)
    {
        var storages = BuildingController.FindAllStructOfThisTypeContain(H.Storage);

        for (int i = 0; i < storages.Count; i++)
        {
            var left = LeftToRemove(item, amt, storages[i]);

            if (left == 0)
            {
                return;
            }
        }
    }


    /// <summary>
    /// Will remove from the 'building' and will tell u how much is left to be removed 
    /// </summary>
    float LeftToRemove(P item, float amt, Structure building)
    {
        if (building.Inventory.IsItemOnInv(item))
        {
            var removed = building.Inventory.RemoveByWeight(item, amt);
            var left = amt - removed;

            return left;
        }
        return amt;
    }

    /// <summary>
    /// Will tell u if all inventories are empty on the storages 
    /// </summary>
    /// <returns></returns>
    internal bool IsEmpty()
    {
        var storages = BuildingController.FindAllStructOfThisTypeContain(H.Storage);

        for (int i = 0; i < storages.Count; i++)
        {
            if (!storages[i].Inventory.IsEmpty())
            {
                return false;
            }
        }

        return true;
    }
}


public class Coverage
{
    private static float _edu;
    private static float _tradesEdu;
    private static float _rel;
    private static float _chill;

    private static float _lastEdu;
    private static float _lastTradeEdu;
    private static float _lastRel;
    private static float _lastChill;

    //so its not recalculated everytime is asked if is really frequent
    //for CPU reasons
    private static float _cool = 60f;

    public static float Edu(bool force = false)
    {

        if ((force) || (_lastEdu == 0 || float.IsNaN(_edu) || Time.time > _lastEdu + _cool))
        {
            _lastEdu = Time.time;
            _edu = CalculateCoverage(H.School);
        }
        return _edu;
    }

    public static float TradesEdu(bool force = false)
    {

            if ((force) ||(_lastTradeEdu == 0 || float.IsNaN(_tradesEdu) || Time.time > _lastTradeEdu + _cool))
            {
                _lastTradeEdu = Time.time;
                _tradesEdu = CalculateCoverage(H.TradesSchool);
            }
            return _tradesEdu;
        
    }

    public static float Rel(bool force = false)
    {

            if((force) || (_lastRel == 0 || float.IsNaN(_rel) || Time.time > _lastRel + _cool))
            {
                _lastRel = Time.time;
                _rel = CalculateCoverage(H.Church);
            }
            return _rel;
    }

    public static float Chill(bool force = false)
    {
   
            if ((force) ||(_lastChill == 0 || float.IsNaN(_chill) || Time.time > _lastChill + _cool))
            {
                _lastChill = Time.time;
                _chill = CalculateCoverage(H.Tavern);
            }
            return _chill;
        
    }




    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <param name="factor"></param>
    /// <param name="ageMin">not included</param>
    /// <param name="ageMax">not incl</param>
    /// <returns></returns>
    static float CalculateCoverage(H type)
    {
        if (_build.Count==0)
        {
            LoadBuildStats();
        }

        var factor = GiveMeStat(type).Factor;
        var ageMin = GiveMeStat(type).MinAge;
        var ageMax = GiveMeStat(type).MaxAge;

        var schools = BuildingController.FindAllStructOfThisType(type);
        var acumPercent = 0f;

        for (int i = 0; i < schools.Count; i++)
        {
            acumPercent += schools[i].CurrentCoverage();
        }
        //the people can cover this tyoes of buildings shcools for ex. with 2 schools working at full will cover
        //40 people bz the factor is 2 
        var peopleCanCover = acumPercent*factor;
        //amt of people that need the coverage 
        var amtPplTtl = PersonPot.Control.All.Count(a => a.Age > ageMin && a.Age < ageMax);
        //final coverage if can cover 20 people and 30 needed then the cover is gonna be .66
        var fin = peopleCanCover/amtPplTtl;

        if (fin>1)
        {
            return 1;
        }
        return fin;
    }


    internal static string PeopleICanServe(float amtOfPpl, H hType)
    {
        var stat = GiveMeStat(hType).Factor;
        return WholeNumbFormat(amtOfPpl, stat);
    }


    /// <summary>
    /// If is 
    /// </summary>
    /// <param name="HType"></param>
    /// <param name="forced">if is marked will be found out every 10s </param>
    /// <returns></returns>
    internal static string OverallMyType(H HType, bool forced = false)
    {
        if (forced)
        {
            _cool = 10;
        }
        else _cool = 60;

        if (HType==H.School)
        {
            return PercentFormat(Edu());
        }      
        if ( HType == H.TradesSchool)
        {
            return PercentFormat(TradesEdu());
        }   
        if (HType==H.Church)
        {
            return PercentFormat(Rel());
        }     
        if (HType==H.Tavern)
        {
            return PercentFormat(Chill());
        }

        return "-1";
    }

    static public int HowManyPeopleNeedThisService(H htype)
    {
        var stat = GiveMeStat(htype);
        return PersonPot.Control.All.Count(a => a.Age > stat.MinAge && a.Age < stat.MaxAge);
    }

    static string PercentFormat(float val)
    {
        return (val * 100).ToString("n1") + "%";
    }  
    
    static string WholeNumbFormat(float val, int factor)
    {
        return ((int)(val * factor)) + "";
    }


    static List<BuildStat> _build = new List<BuildStat>(); 
    static private void LoadBuildStats()
    {
        _build.Add(new BuildStat(H.School, 20, 4, 11));
        _build.Add(new BuildStat(H.TradesSchool, 5, 10, 16));
        _build.Add(new BuildStat(H.Church, 150, 0, 100));
        _build.Add(new BuildStat(H.Tavern, 40, 20, 80));
    }

    static BuildStat GiveMeStat(H hTypeP)
    {
        BuildStat res = new BuildStat();
        for (int i = 0; i < _build.Count; i++)
        {
            if (_build[i].HType == hTypeP)
            {
                res = _build[i];
            }
        }
        return res;
    }

}
