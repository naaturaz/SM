using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/*
 * There is more Job Managment on Brain.RemoveAndAddPositionsToJob()
 * 
 */

public class JobManager
{
    public   int startSchool = 5;
    public   int startTrade = 11;
    public static  int majorityAge = 16;

    #region Give Initial Work

    public Structure GiveWork(Person person)
    {
        ////bz need to finish wht is doing to find new work 
        //if (PersonPot.Control.IAmOnSystemNow(person.MyId))
        //{
        //    return person.Work;
        //}

        var key = DecideBasedOnAge(person);

        if (UPerson.IsWorkingAtSchool(person) && !UPerson.IsMajor(person.Age))
        {
            person.IsStudent = true;
        }
        return Brain.GetStructureFromKey(key);
    }

    /// <summary>
    /// Will look for specific School or Work depending Age 
    /// </summary>
    /// <param name="person"></param>
    /// <returns></returns>
      string DecideBasedOnAge(Person person)
    {
        if (person.Age >= majorityAge)
        {
            //find work
            return DefineClosestBuild(person);
        }
        else if (person.Age < majorityAge && person.Age >= startTrade)
        {
            //try find trade
            var res = FindBestSchool(H.TradesSchool, person);
            //if cant find, try find school
            if (res=="")
            {
                res = FindBestSchool(H.School, person);
            }
            return res;
        }
        else if (person.Age < startTrade && person.Age >= startSchool)
        {
            //try find school
            return FindBestSchool(H.School, person);
        }
        return "";
    }

      bool OneMoreKidFitOnTheSchool(Building building)
    {
        return true;
    }

      string FindBestSchool(H hTypeP, Person person)
    {
        var trades = ReturnListType(hTypeP, person);

        for (int i = 0; i < trades.Count; i++)
        {
            if (OneMoreKidFitOnTheSchool(trades[i]))
            {
                return trades[i].MyId;
            }
        }
        return "";
    }

      private string DefineClosestBuild(Person person)
    {
        var currListOfBuild =  BuildingPot.Control.WorkOpenPos;
        int size = currListOfBuild.Count;
        List<VectorM> loc = new List<VectorM>();

        for (int i = 0; i < size; i++)
        {
            //to address if building was deleted and not updated on the list 
            string key = currListOfBuild[i];
            var build = Brain.GetBuildingFromKey(key);

            if (!person.Brain.BlackList.Contains(key) && BuildingPot.Control.Registro.AllBuilding.ContainsKey(key)
                && build != null && build.HasOpenPositions())//to avoid checking on deleted building 
            {
                Vector3 pos = BuildingPot.Control.Registro.AllBuilding[key].transform.position;
                loc.Add(new VectorM(pos, person.Home.transform.position, key));
            }
        }
        loc = Brain.ReturnOrderedByDistance(person.Home.transform.position, loc);

        int index = 0;

        if (loc.Count==0)
        {return "";}

        while (BuildingPot.Control.Registro.AllBuilding[loc[index].LocMyId].Instruction == H.WillBeDestroy)
        {
            index++;
            //this is to avoid exception when many buildings are on the blacklist
            if (index > loc.Count - 1)
            {
                index = -1;
                break;
            }
        }
        return DefineClosestBuildTail(loc, index);
    }

    /// <summary>
    /// Created for modularity
    /// </summary>
      string DefineClosestBuildTail(List<VectorM> loc, int index)
    {
        string closestKey = "";

        if (index != -1)
        {
            closestKey = loc[index].LocMyId;
        }
        return closestKey;
    }

    /// <summary>
    /// Will return a list of the type pass 
    /// 
    /// Used to find out shcools 
    /// </summary>
    /// <param name="hTypeP"></param>
    /// <returns></returns>
      List<Building> ReturnListType(H hTypeP, Person person)
    {
        List<Building> Re = new List<Building>();
        for (int i = 0; i < BuildingPot.Control.Registro.AllRegFile.Count; i++)
        {
            var key = BuildingPot.Control.Registro.AllRegFile[i].MyId;
            var struc = Brain.GetStructureFromKey(key);

            //brdige 
            if (struc==null)
            {
                continue;
            }

            if (struc.HType == hTypeP &&
                struc.Instruction != H.WillBeDestroy && !person.Brain.BlackList.Contains(key))
            {
                Re.Add(BuildingPot.Control.Registro.AllBuilding.ElementAt(i).Value);
            }
        }
        return Re;
    }  
    
    //List<Building> ReturnListType(H hTypeP, Person person)
    //{
    //    List<Building> Re = new List<Building>();
    //    for (int i = 0; i < BuildingPot.Control.Registro.AllBuilding.Count; i++)
    //    {
    //        var key = BuildingPot.Control.Registro.AllBuilding.ElementAt(i).Key;
    //        var struc = BuildingPot.Control.Registro.AllBuilding.ElementAt(i).Value;

    //        if (struc.HType == hTypeP &&
    //            struc.Instruction != H.WillBeDestroy && !person.Brain.BlackList.Contains(key))
    //        {
    //            Re.Add(BuildingPot.Control.Registro.AllBuilding.ElementAt(i).Value);
    //        }
    //    }
    //    return Re;
    //}

#endregion

    public Structure ThereIsABetterJob(Person person)
    {
        ////bz need to finish wht is doing to find new work 
        //if (PersonPot.Control.IAmOnSystemNow(person.MyId))
        //{
        //    return person.Work;
        //}

        if (person.Home == null)
        {return null;}

        //if new positions are up will return the same so it doesnt affect the flow of finding job
        //this method is only intended to work once no new positions are up 
        //if (person.Work != null 
        //    && BuildingPot.Control.AreNewWorkPos
        //    )
        //{return person.Work;}

        if (!UPerson.IsMajor(person.Age))
        {
            return BetterSchool(person);
        }
        return BetterWork(person);
    }

    /// <summary>
    /// Will return a beetter job if is one 
    /// </summary>
    /// <param name="person"></param>
    /// <returns></returns>
      Structure BetterWork(Person person)
    {
        if (BuildingPot.Control.WorkOpenPos.Count == 0 ||
            (person.Work != null && person.Work.Instruction == H.WillBeDestroy))
        {
            return person.Work;
        }

        var betterPlaceKey = DefineIfIsABetterJob(person);
        var res = Brain.GetStructureFromKey(betterPlaceKey);

        if (res != null)
        {
            return res;
        }
        return person.Work;
    }

    /// <summary>
    /// Created to address the case when a teen need to go to a Trades school if exist 
    /// </summary>
    /// <returns></returns>
      Structure BetterSchool(Person person)
    {
        return GiveWork(person);
    }

    /// <summary>
    /// Has the logic that defined if is a bbeter job
    /// </summary>
    /// <param name="person"></param>
    /// <returns></returns>
    private   string DefineIfIsABetterJob(Person person)
    {
        UpdateAllJobAvail(person);
        
        var myCurrentJobScore = ScoreABuild(person.Work, person.Home.transform.position);

        bool isABetterJob = false;
        if (_allJobAvailGC.Count>0)
        {
            isABetterJob = _allJobAvailGC[0].Score > myCurrentJobScore;
        }

        if (isABetterJob)
        {
            return _allJobAvailGC[0].Key;
        }
        return "";
    }


    List<string> _oldJobs = new List<string>();
    List<BuildRank> _allJobAvailGC = new List<BuildRank>();
    public   void UpdateAllJobAvail(Person person)
    {
        if (_oldJobs != BuildingPot.Control.WorkOpenPos)
        {
            _oldJobs.Clear();
            _oldJobs.AddRange(BuildingPot.Control.WorkOpenPos);

            _allJobAvailGC = ScoreAllAvailBuilds(person, person.Home.transform.position);
        }
    }

      private int rationsWeight = 50;
      private int dollarsWeight = 100;
    /// <summary>
    /// Score one building 
    /// </summary>
    /// <param name="building"></param>
    /// <param name="person"></param>
    /// <returns></returns>
      float ScoreABuild(Building building, Vector3 comparePoint)
    {
        if (building==null)
        {
            return -1000;
        }

        var distToWork = Vector3.Distance(building.transform.position, comparePoint);
        var rations = building.RationsPay*rationsWeight;
        var dollars = building.DollarsPay*dollarsWeight;

        return rations + dollars - distToWork;
    }

    /// <summary>
    /// Return a list with the Job rank ordered descending 
    /// </summary>
    /// <param name="person"></param>
    /// <returns></returns>
      List<BuildRank> ScoreAllAvailBuilds(Person person, Vector3 comparePoint)
    {
        List<BuildRank> res = new List<BuildRank>();

        for (int i = 0; i < BuildingPot.Control.WorkOpenPos.Count; i++)
        {
            var key = BuildingPot.Control.WorkOpenPos[i];
            var struc = Brain.GetStructureFromKey(key);

            //to avoid struct tht weere recentrly deleted 
            if (struc == null)
            {
                continue;
            }

            var score = ScoreABuild(struc, comparePoint);

            if (struc.Instruction != H.WillBeDestroy && !person.Brain.BlackList.Contains(key))
            {
                res.Add(new BuildRank(key, score));
            }
        }

        return res.OrderByDescending(a=>a.Score).ToList();
    }
}

public class BuildRank
{
    public string Key;
    public float Score;

    public BuildRank(string key, float score)
    {
        Key = key;
        Score = score;
    }
}
