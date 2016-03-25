using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
 * The rule for this class to work is that always that we spawn people more adults need to be spawn that kids 
 */

public static class ShacksManager
{
    static H _state = H.None;
    static List<Person> _homeless = new List<Person>();
    static List<Person> _adult = new List<Person>();
    static List<Building> _shacksDone = new List<Building>();
    static int _adultTtl = 0;//the total combined amt of adults. man and women

    private static int _sameHomeless;
    static List<Person> _oldHomeless = new List<Person>();

    private static bool _isNewYear;//will say if a new year has arrived
    private static int _lastYearChecked;//last year was checked 


    private static bool _newAdult;//if a new adult . Called by a person tht reached Majority of age

    private static bool _lastChecked;//flag if all ShackdDone.Count == adultTtl
    private static float _time;//use to go ahead and pass to female action or not 

    private static List<H> newYearChecks = new List<H>();//How many times has used the newYear to return false in below ()

    static public H State
    {
        get { return _state; }
        set { _state = value; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="clearReport">Will clear report of _shacksDone. If is called from Start in this class
    /// when actually is restarting . _shacksDone should not be cleared </param>
    public static void Start(bool clearReport = true)
    {
        //GameScene.print("ShackManager started.Clear:"+ clearReport);
        _state = H.Male;

        if (clearReport)
        {
            _shacksDone.Clear();
            _adultTtl = 0;
        }

        Init();
        ManAction();
    }

    static void ManAction()
    {
        _adult = FindAdult(H.Male, _homeless);
        _adultTtl += _adult.Count;

        MakeAdultsShackBuilders();
        CheckIfALeast1Man();
    }


    static bool OldHomeLessAreSame(List<Person> thisHomeLess)
    {
        if (thisHomeLess.Count != _oldHomeless.Count)
        {
            return false;
        }

        int count = 0;
        for (int i = 0; i < _oldHomeless.Count; i++)
        {
            if (_oldHomeless[i].MyId == thisHomeLess[i].MyId)
            {
                count++;
            }
        }

        return thisHomeLess.Count == count;
    }

    

    /// <summary>
    /// Clear old vars, locks PersonPot and define _homeless 
    /// </summary>
    static void Init()
    {
        ClearOldVar();
        PersonPot.Control.Locked = true;
        _homeless = FindAllHomeless();
    }

    static void CheckIfALeast1Man()
    {
        if (_adult.Count == 0)
        {
            ConditionsLastCheck();
            FemaleAction();
        }
    }

    static void ClearOldVar()
    {
        //WaveReseter();

        _homeless.Clear();
        _adult.Clear();
        _time = 0;

        _lastChecked = false;
    }

    /// <summary>
    /// Will return all people that home = null
    /// </summary>
    /// <returns></returns>
    static private List<Person> FindAllHomeless()
    {
        List<Person> res = new List<Person>();
        for (int i = 0; i < PersonPot.Control.All.Count; i++)
        {
            //var person = PersonPot.Control.All[i];
            //if (person.Home == null && !person.IsBooked)
            //{
                //res.Add(person);
            //}
        }
        return res;
    }

    /// <summary>
    /// Will find adults of specific Gender from the homeless list 
    /// </summary>
    /// <param name="gender">Adult gender </param>
    /// <summary>
    /// Will find adults of specific Gender from the homeless list 
    /// </summary>
    /// <param name="gender">Adult gender </param>
    static private List<Person> FindAdult(H gender, List<Person> homeLessP)
    {
        List<Person> res = new List<Person>();
        for (int i = 0; i < homeLessP.Count; i++)
        {
            var person = homeLessP[i];
            if (person.Gender == gender && UPerson.IsMajor(person.Age))
            {
                res.Add(person);
            }
        }
        return res;
    }

    static private void MakeAdultsShackBuilders()
    {
        for (int i = 0; i < _adult.Count; i++)
        {
            //to addres a restart where we found again a homeless person that is already building a shack
            //but still doesnt have the shackl assigned as home 
            if (_adult[i].ProfessionProp.ProfDescription != Job.ShackBuilder)
            {
                _adult[i].CreateProfession(Job.ShackBuilder);
            }
        }
    }

    /// <summary>
    /// When the pperson finish the shack need to report it done so we can move on 
    /// </summary>
    /// <param name="newShack"></param>
    static public void ReportShackDone(Building newShack)
    {
        _shacksDone.Add(newShack);
    }

    static private void Finish()
    {
       //GameScene.print("Shack Manger Finished State:" + State + ".locked:" + PersonPot.Control.Locked);

        if (DidCourseWasChange())
        {
            return;
        }

        _state = H.Kids;
    }

    static public void Update()
    {
        CheckIfNewYear();


        CheckForHomeLessKids();
        CheckIfNewAdult();

        if (_state == H.None || _adultTtl == 0 || _shacksDone.Count == 0)
        {
            return;
        }

        ConditionsLastCheck();
        RestartForAdultFemales();
    }


    static void CheckIfNewYear()
    {
        if (Program.gameScene.GameTime1.Year != _lastYearChecked && Program.gameScene.GameTime1.Month1 == 1)
        {
           //Debug.Log("New Year:" + Program.gameScene.GameTime1.Year);

            _isNewYear = true;
            _lastYearChecked = Program.gameScene.GameTime1.Year;
            newYearChecks.Clear();
        }
    }



    /// <summary>
    /// If new adult was mark then Start will be called 
    /// </summary>
    static void CheckIfNewAdult()
    {
        if (_newAdult && !AtLeastBuilding1ShackNow() && _adultTtl != _shacksDone.Count)
        {
            _newAdult = false;
            Start();
        }
    }

    /// <summary>
    /// Will be called everytime a Person Reach Majority of age. 
    /// 
    /// This one is to address the problem where the TimeChecks make people grow and there is
    /// more adults thant shacksDone
    /// </summary>
    static public void NewAdultIsUp()
    {
        _newAdult = true;
    }

    /// <summary>
    /// Will search for homeless kids and will kill them if this class is not working on State=Kids
    /// 
    /// Will pass only once after the shackmanager is finished ... then will make state = None. so will be ready
    /// to start working again whenever is needed
    /// </summary>
    private static void CheckForHomeLessKids()
    {
        if (State != H.Kids)
        {
            return;
        }

        var homeLess = FindAllHomeless();
        var adult = FindAdult(H.Male, homeLess).Count;
        adult += FindAdult(H.Female, homeLess).Count;

        AddToNewYearChecks();
        CleanOldHomeLess();

        if (adult == 0)
        {
            KillAllHomeLessKids();

            //PersonPot.Control.Locked = false;
            //State=H.None;
            //PersonPot.Control.RestartController();
        }
        else
        {
            RestartManager();
        }
    }

    /// <summary>
    /// Will kill all homeless kids 
    /// </summary>
    static void KillAllHomeLessKids()
    {
        var homeLess = FindAllHomeless();
       
        for (int i = 0; i < homeLess.Count; i++)
        {
            //bz is has a father or mother they had been booked in another place 
            if (homeLess[i].IsOrphan())
            {
                homeLess[i].Kill();    
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private static void CheckIfAllGotAPlace()
    {
        _homeless.Clear();
        _homeless = FindAllHomeless();

        var male = FindAdult(H.Male, _homeless).Count;
        var female = FindAdult(H.Female, _homeless).Count;

        if (male + female == 0)
        {
            Finish();
        }
        else
        {
            RestartManager();
        }
    }

    static void RestartManager()
    {
        //throw new Exception("Implement to address if people created while ShackManger was going ");
       //GameScene.print("Shack Manger Restarted,State:" + State);
        Start(false);
    }




    static void ConditionsLastCheck()
    {
        if (_lastChecked)
        { return; }

        if (_shacksDone.Count == _adultTtl)
        {
            PersonPot.Control.Locked = false;
            _lastChecked = true;

            if (_state == H.Male)
            {
                _time = Time.time;
            }
            else if (_state == H.Female)
            {
                CheckIfAllGotAPlace();
            }
        }
    }

    static void RestartForAdultFemales()
    {
        if (_time != 0 && _lastChecked && _state == H.Male)
        {
            FemaleAction();
        }
    }

    static void FemaleAction()
    {
        if (DidCourseWasChange())
        {
            return;
        }

       //GameScene.print("FemaleAction()");
        
        Init();
        _adult = FindAdult(H.Female, _homeless);
        _state = H.Female;//so it doesnt call this() ever again 
        _lastChecked = false;//so the update checks again for the same ConditionsLastCheckMale()

        _adultTtl += _adult.Count;//so it keeps the amout of the man pls the women

        if (_adult.Count == 0)
        {
            PersonPot.Control.Locked = false;
            Finish();
        }
        else
        {
            MakeAdultsShackBuilders();
        }
    }

    /// <summary>
    /// Will return true if at least one person is homeless 
    /// </summary>
    /// <returns></returns>
    static public bool IsAtLeast1HomeLess()
    {
        var homeless = FindAllHomeless();
        if (homeless.Count == 0)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// Created to check at the end of MaleAction and FemaleAction to see if at least one Shack with space
    /// if so will stop the execution of this class and will allow people to find their own Shacks 
    /// 
    /// Created to fix women creating Shacks when men living alone in theirs and the killing of children
    /// when man and women with empty Shacks
    /// </summary>
    static bool AtLeastOneShackWithSpace()
    {
        if (HowManyShacksWithSpace() > 0)
        {
            return true;
        }
        return false;
    }

    static void PassControlToPersonController()
    {
       //Debug.Log("Control passed to PersonController.cs");

        PersonPot.Control.Locked = false;
        State = H.None;
        //so checks for houses again 
        BuildingPot.Control.IsNewHouseSpace = true;
        //so everyone looks for homes again, so women and child wil potentially find new Shack Homes
        PersonPot.Control.RestartController();        
    }

    /// <summary>
    /// Will let u know how many shacks with space are 
    /// 
    /// Needed bz knowing tht are houses with space its not enough , ex:  houses with space could be looking for
    /// males and only females are homeless. 
    /// </summary>
    /// <returns></returns>
    static int HowManyShacksWithSpace()
    {
        int res = 0;
        for (int i = 0; i < BuildingPot.Control.HousesWithSpace.Count; i++)
        {
            var key = BuildingPot.Control.HousesWithSpace[i];
            var build = Brain.GetBuildingFromKey(key);

            //if (build.HType == H.Shack)
            //{
            //    res++;
            //}
        }
        return res;
    }




    /// <summary>
    /// If contdion are met will pass the control to PersonController
    /// </summary>
    static bool DidCourseWasChange()
    {
        if ((!AtLeastOneBuildWillBeDestroy() && AtLeastOneShackWithSpace()) || AtLeastBuilding1ShackNow())
        {
            if (!_isNewYear)
            {
                PassControlToPersonController();
                return true;    
            }
            //so if is true will return false so all can be checked regardless if one person is building a shack 
            //or anything else. Is creatred so at least once a year all gets checked 

            AddToNewYearChecks();

            //so it checks at least thru all states , so each State of the Manager can use it at least once 
            if (newYearChecks.Count == 3)
            {
               //Debug.Log("2 States Checked");
                _isNewYear = false;
                newYearChecks.Clear();

                //so at least will have this year as check so people building new Shacks can see each other 
                //bz having new year after new year was just putting everyone on shack and not marriying them 
                _lastYearChecked = Program.gameScene.GameTime1.Year;
                
                //bz here means all States were Checked and can be restarted and for now control can be pass to 
                //PersonController so people try to look for marriages
                return true;
            }
            return false;
        }
        return false;
    }

    /// <summary>
    /// Check in on newYearChecks if is not there already
    /// </summary>
    static void AddToNewYearChecks()
    {
        if (!newYearChecks.Contains(State))
        {
            newYearChecks.Add(State);
        }
    }

    /// <summary>
    /// Will tell u if everytime for the last 3 checks had the same Homeless,
    /// that means that are open spaces in houses but they dont fit. Therefore they have to create new shacks
    /// </summary>
    /// <returns></returns>
    private static bool SameHomeLess1Times()
    {
        var newHomeLess = FindAllHomeless();

        if (OldHomeLessAreSame(newHomeLess))
        {
            _sameHomeless++;
        }
        _oldHomeless = newHomeLess;

        if (_sameHomeless >= 1)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Neeed to be called when kids Killing is done. so i know the cycle went trhu
    /// and was not restart it on the middle
    /// </summary>
    static void CleanOldHomeLess()
    {
        _sameHomeless = 0;
        _oldHomeless = new List<Person>();
    }

    /// <summary>
    /// Will return true if at least one person is building a shack now 
    /// </summary>
    /// <returns></returns>
    public static bool AtLeastBuilding1ShackNow()
    {
        var homeless = FindAllHomeless();

        for (int i = 0; i < homeless.Count; i++)
        {
            if (homeless[i].ProfessionProp.ProfDescription == Job.ShackBuilder)
            {
                return true;
            }
        }
        return false;
    }

    static bool AtLeastOneBuildWillBeDestroy()
    {
        for (int i = 0; i < BuildingPot.Control.Registro.AllBuilding.Count; i++)
        {
            if (BuildingPot.Control.Registro.AllBuilding.ElementAt(i).Value.Instruction == H.WillBeDestroy)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Used to address when Shack spaces are up but still a couple tht is homeless the man will create shack 
    /// </summary>
    static void MakeHomelessMarriedManShackBuilders()
    {
        var homelessH = FindAllHomeless();
        var malesH = FindAdult(H.Male, homelessH);

        for (int i = 0; i < malesH.Count; i++)
        {
            if (malesH[i].Spouse != "")
            {
                if (malesH[i].ProfessionProp.ProfDescription != Job.ShackBuilder)
                {
                    malesH[i].CreateProfession(Job.ShackBuilder);
                }
            }
        }
    }

    

}
