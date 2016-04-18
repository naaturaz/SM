using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/*
 * SEALED CLASS AS JAN 28 2016
 */

public class Realtor
{
    //its not well implemented so Im dropping confomrt to people get better home 
    private   int confortWeight = 0;//100

    private   string familyID="";

    public   Structure GiveMeTheBetterHome(Person person)
    {
        var key = "";
        if (!string.IsNullOrEmpty(person.IsBooked))
        {
            //if they are booked somewhere need to handle that first
            var bookedHome = Brain.GetStructureFromKey(person.IsBooked);

            //so its added to the family and transform.parent
            bookedHome.MovePersonToFamilySpot(person, bookedHome);
            return bookedHome;
        }

        key = LoopThruAllBetterHomes(person);
        if (string.IsNullOrEmpty(key))
        {
            return null;
        }

        var newhome = BuildingPot.Control.Registro.AllBuilding[key] as Structure;

        return HandleBooking(person, newhome);
    }

    /// <summary>
    /// Will go thru all better homes to see if can find one. 
    /// </summary>
      string LoopThruAllBetterHomes(Person person)
    {
        //list of better hoomes
        var list = DefineBetterHomesList(person);
        // and if not none dont have to continue any deeper 
        if (!ThereIsABetterHome(person, list))
        {
            return "";
        }

        var key = "";

        for (int i = 0; i < list.Count; i++)
        {
            key = DefineIfIsABetterHouse(person, list[i].Key, list);

            if (!string.IsNullOrEmpty(key))
            {
                return key;
            }
        }
        return "";
    }

    /// <summary>
    /// It has the logic to deal with the booking of a home
    /// </summary>
    /// <returns></returns>
      Structure HandleBooking(Person person, Structure newhome)
    {
        if (newhome == null)
        {
            return null;
        }

        BookToNewBuild(person, newhome);
        //if is not booked
        return newhome;
    }

    /// <summary>
    /// Will handle if is to a Family or a individual
    /// </summary>
    /// <param name="person"></param>
    /// <param name="newHome"></param>
      void BookToNewBuild(Person person, Structure newHome)
    {
       //Debug.Log(person.MyId + " BookToNewBuild fID:"+familyID);

        if (familyID=="Empty")
        {
            BookMyFamilyToNewBuild(person, newHome);
        }
        else
        {
            BookNewPersonInNewHome(person, newHome, familyID);
            familyID = "";
        }
    }

    /// <summary>
    /// Has the logic that defined if is a bbeter home
    /// </summary>
    /// <param name="person"></param>
    /// <returns></returns>
    private   string DefineIfIsABetterHouse(Person person, string homeToEval, List<BuildRank> list)
    {
        if (ThereIsABetterHome(person, list))
        {
            var isAdult = UPerson.IsMajor(person.Age);

            if (isAdult)
            {
                return DefineBetterHome4Adult(person, homeToEval);
            }
            else
            {
                return DefineBetterHome4Child(person, homeToEval);
            }
        }
        return "";
    }

      string DefineBetterHome4Adult(Person person, string homeToEval)
    {
        var newHome = Brain.GetBuildingFromKey(homeToEval);

        if (DoesFamilyFit(person, newHome) || AmIBookedInThatBuild(person, newHome))
        {
            return homeToEval;
        }
        else if (DoesPersonFit(person, newHome))
        {
            return homeToEval;
        }
        return "";
    }


      string DefineBetterHome4Child(Person person, string homeToEval)
    {
        var newHome = Brain.GetBuildingFromKey(homeToEval);

        if (person.IsOrphan())
        {
            if (DoesPersonFit(person, newHome))
            {
                //Debug.Log("orphan fit:" + person.MyId);
                return homeToEval;
            }
        }
        else
        {
            if (AmIBookedInThatBuild(person, newHome))
            {
                //DoesPersonFit(person, newHome);//so its added there etc
                //Debug.Log("child booked:" + person.MyId);
                return homeToEval;
            }
        }
        return "";
    }

    /// <summary>
    /// Will tell u if the person family fits in the new home 
    /// </summary>
      bool DoesFamilyFit(Person person, Building newHome)
    {
        var personHasFamily = person.HasFamily();
        //Debug.Log(person.MyId);

        if (personHasFamily)
        {
            var canI = CanIMoveFamilyToNewHome(newHome);
            if (canI)
            {
                familyID = "Empty";
                return true;
            }
        }
        //person does't have family
        else
        {
            var canI = CanIMoveFamilyToNewHome(newHome);
            if (canI)
            {
                familyID = "Empty";
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Will tell u if the 'person' is booked in the 'newHome'
    /// </summary>
      bool AmIBookedInThatBuild(Person person, Building newHome)
    {
        if (newHome.BookedHome1 == null)
        {
            return false;
        }
        return newHome.BookedHome1.IAmBookedHere(person);
    }

    /// <summary>
    /// Will tell if a person fits in the 'newHome' will look first if the building is booked 
    /// </summary>
      bool DoesPersonFit(Person person, Building newHome)
    {
        //was removed from there then doesnt fit 
        if (!BuildingPot.Control.HousesWithSpace.Contains(newHome.MyId))
        {
            return false;
        }

        //and is not booked 
        var isBooked = IsBuildBooked(newHome);
        if (!isBooked)
        {
            string famID = "";
            Structure s = (Structure)newHome;
            if (s.ThisPersonFitInThisHouse(person, ref famID))
            {
                familyID = famID;
                //person.PersonGotMarriedAffairs(newHome);

                //so families are resaved 
                BuildingPot.Control.Registro.ResaveOnRegistro(newHome.MyId);

                //if not even have 1 spot avail for a new family and all families formed then    
                //home can be removed from list 
                if (newHome.AllFamiliesFull())
                {
                    BuildingPot.Control.RemoveFromHousesWithSpace(newHome.MyId);
                }
                return true;
            }
        }
        return false;
    }

      Family FindCurrentFamily(Person person)
    {
        if (person.Home==null)
        {
            return null;
        }
        else
        {
            return  person.Home.FindMyFamilyChecksFamID(person);
        }
    }

    /// <summary>
    /// Will book family to new building 
    /// </summary>
    public   void BookMyFamilyToNewBuild(Person person, Building newHome)
    {
        //if doesnt have at least 1 family empty.//means no booking is needed.
        if (newHome.ReturnEmptyFamily() == null)
        {return;}

        var famIDInBookedHome = "";

        Family curFamily = FindCurrentFamily(person);
        if (curFamily == null)
        {
            curFamily = newHome.ReturnEmptyFamily();
            person.FamilyId = curFamily.FamilyId;
            //seeting person as the first person in the family
            curFamily.CanGetAnotherAdult(person);
            BookMyFamilyToNewBuildTail(person, newHome, curFamily);
        }
        else
        {
            var familyToBeTransferTo = TransferInToNewFamily(curFamily, newHome, person);
            BookMyFamilyToNewBuildTail(person, newHome, familyToBeTransferTo);
        }
    }

      void BookMyFamilyToNewBuildTail(Person person, Building newHome, Family myFamily)
    {
        newHome.BookedHome1 = new BookedHome(newHome.MyId, myFamily);
        BuildingPot.Control.Registro.ResaveOnRegistro(newHome.MyId);
        MarkTheFamilyBooking(newHome.MyId, myFamily);
        RestartControllerForMyFamily(myFamily, person);
    }

    private   Family TransferInToNewFamily(Family curFamily, Building newHome, Person newPerson)
    {
        var fam = newHome.ReturnEmptyFamily();

        IdEveryOneOnTheFamily(fam.FamilyId, curFamily);

        //seeting person as the first person in the family
        fam.CanGetAnotherAdult(newPerson);
        curFamily.RemovePersonFromFamily(newPerson);

        TransferFromCurrToNewFam(curFamily, fam, newPerson, newHome);

        return fam;
    }

    /// <summary>
    /// Will set them into the family
    /// will set new transform.parent
    /// will remove from old family
    /// </summary>
    /// <param name="curFamily"></param>
    /// <param name="fam"></param>
    private   void TransferFromCurrToNewFam(Family curFamily, Family newFam, Person newPerson, Building newHome)
    {
        if (!string.IsNullOrEmpty( newPerson.Spouse))
        {
            var spouse = Family.FindPerson(newPerson.Spouse);
            if (spouse!= null)
            {
                newFam.CanGetAnotherAdult(spouse);
                curFamily.RemovePersonFromFamily(spouse);
            }
        }

        for (int i = 0; i < curFamily.Kids.Count; i++)
        {
            var kid = Family.FindPerson(curFamily.Kids[i]);
            kid.transform.parent = newHome.transform;

            var temp = kid.MyId;
            newFam.AddKids(temp);

            curFamily.RemovePersonFromFamily(kid);
            i--;
        }
    }

    /// <summary>
    /// Books a a Person that doesnt have any family into a new place
    /// 
    /// so far used by Teens moving out of home 
    /// </summary>
    /// <param name="person"></param>
    /// <param name="newHome"></param>
    public   void BookNewPersonInNewHome(Person person, Building newHome, string familyIDP)
    {
        RemoveFromOldHomeFamily(person);
        var myFamily = newHome.FindFamilyById(familyIDP);
        
        person.FamilyId = myFamily.FamilyId;
        myFamily.CanGetAnotherAdult(person);

        //so i dont pull the existing family memebers and book them . whn they are actually already in the house
        Family famToBook = new Family();
        famToBook.FamilyId = person.FamilyId;
        famToBook.Home = newHome.MyId;
        famToBook.CanGetAnotherAdult(person);
        newHome.BookedHome1 = new BookedHome(newHome.MyId, famToBook);

        person.IsBooked = newHome.MyId;

        BuildingPot.Control.Registro.ResaveOnRegistro(newHome.MyId);
        RestartControllerForMyFamily(myFamily, person);
    }

    /// <summary>
    /// In case is being called from In Here Realtor. 
    /// </summary>
    /// <param name="person"></param>
      void RemoveFromOldHomeFamily(Person person)
    {
        if (person.Home == null || person.Home.Families == null)
        {
            return;
        }

        for (int i = 0; i < person.Home.Families.Length; i++)
        {
            person.Home.Families[i].RemovePersonFromFamily(person);
        }
    }

    /// <summary>
    /// Will uncheck them from the controller so they can see their new Home Booked so theu can move there
    /// 
    /// PersonB is the one asking for this. Is theone tht booked and this person doesnt need to check again  
    /// </summary>
      void RestartControllerForMyFamily(Family myFamily, Person personB)
    {
        for (int i = 0; i < myFamily.Kids.Count; i++)
        {
            PersonPot.Control.RestartControllerForPerson(myFamily.Kids[i]);
        }

       
            PersonPot.Control.RestartControllerForPerson(myFamily.Mother);
      
            PersonPot.Control.RestartControllerForPerson(myFamily.Father);
        
    }

    /// <summary>
    /// Will return true if 'newHome' is unbooked and at least has one spot for a family
    /// </summary>
    /// <param name="newHome"></param>
    /// <returns></returns>
      bool CanIMoveFamilyToNewHome(Building newHome)
    {
        bool isBooked = IsBuildBooked(newHome);

        return newHome.ReturnEmptyFamily()!=null && !isBooked;
    }

    /// <summary>
    /// Will return true if building is booked 
    /// </summary>
    /// <returns></returns>
      bool IsBuildBooked(Building newHome)
    {
        bool isBooked = false;


        if (newHome.BookedHome1 != null)
        {
            isBooked = newHome.BookedHome1.IsBooked();
        }

        return isBooked;
    }

    /// <summary>
    /// Score one building 
    /// </summary>
    /// <param name="building"></param>
    /// <param name="person"></param>
    /// <returns></returns>
      float ScoreABuild(Building building, Vector3 comparePoint,Person person)
    {
        var distToNewHome = Vector3.Distance(building.transform.position, comparePoint);
        var confort = building.Confort * confortWeight;

        //so the marriage rate increase
        var love = building.WouldFindLoveInThisBuilding(person);

        return love + confort - ValidateDistanceToHome(distToNewHome);
    }

    private float MAXDISTANCETOHOME = 200;
    float ValidateDistanceToHome(float toEval)
    {
        if (toEval > MAXDISTANCETOHOME)
        {
            return 10000;
        }
        return toEval;
    }



    /// <summary>
    /// Return a list with the Home rank ordered descending 
    /// 
    /// Above: that are above this score
    /// </summary>
    /// <param name="person"></param>
    /// <returns></returns>
    List<BuildRank> ScoreAllAvailBuilds(Person person, Vector3 comparePoint, float above)
    {
        List<BuildRank> res = new List<BuildRank>();

        for (int i = 0; i < BuildingPot.Control.HousesWithSpace.Count; i++)
        {
            var key = BuildingPot.Control.HousesWithSpace[i];
            var struc = Brain.GetStructureFromKey(key);

            //to avoid struct tht weere recentrly deleted 
            if (struc == null)
            {
                continue;
            }

            var score = ScoreABuild(struc, comparePoint,person);
            //a house that is over the Max Distance
            if (score < -5000)
            {
                continue;
            }

            if (struc.Instruction != H.WillBeDestroy && !person.Brain.BlackList.Contains(key) && score > above
                //&& !IsBuildBooked(struc)
                )
            {
                res.Add(new BuildRank(key, score));
            }
        }

        return res.OrderByDescending(a => a.Score).ToList();
    }

    /// <summary>
    /// Will return true if is a better home than the current one 
    /// </summary>
      bool ThereIsABetterHome(Person person, List<BuildRank> list)
    {
        var myCurrentHomeScore = ScoreCurrentHome(person);

        //means not Houses are with empty Spaces 
        if (list.Count == 0)
        {
            return false;
        }

        bool isABetterHome = false;
        for (int i = 0; i < list.Count; i++)
        {
            isABetterHome = list[i].Score > myCurrentHomeScore;

            if (isABetterHome)
            {
                return true;
            }
        }
        return false;
    }

    public   bool PublicIsABetterHome(Person person)
    {
        var tempList = DefineBetterHomesList(person);

        return ThereIsABetterHome(person, tempList);
    }

    List<BuildRank> DefineBetterHomesList(Person person)
    {
        var myCurrentHomeScore = ScoreCurrentHome(person);

        UpdateAllAvalBuilds(person, myCurrentHomeScore);

        return _allAvailBuild;
    }

    private void UpdateAllAvalBuilds(Person person, float myCurrentHomeScore)
    {
        //if (_oldHomes != BuildingPot.Control.HousesWithSpace.Count)
        //{
            _oldHomes = BuildingPot.Control.HousesWithSpace.Count;

            //Debug.Log("Check if better home on Realtor");

            //will return list with all avail building tht have better score than 'myCurrentHomeScore'
            _allAvailBuild = ScoreAllAvailBuilds(person, person.transform.position, myCurrentHomeScore);
        //}
    }


    //List<string> _oldHomes = new List<string>();
    private int _oldHomes;
    List<BuildRank> _allAvailBuild = new List<BuildRank>(); 


    float ScoreCurrentHome(Person person)
    {
        //to address the case if there is not home currently for the person
        //if will be destroy score will be -10000
        var myCurrentHomeScore = -10000f;
        if (person.Home != null && person.Home.Instruction != H.WillBeDestroy)
        {
            myCurrentHomeScore = ScoreABuild(person.Home, person.Home.transform.position, person);
        }
        return myCurrentHomeScore;
    }

    /// <summary>
    /// Mark everyone on the Family as booked 
    /// </summary>
    /// <param name="makeIt"></param>
    /// <param name="family"></param>
      void MarkTheFamilyBooking(string booking, Family family)
    {
        for (int i = 0; i < family.Kids.Count; i++)
        {
            PersonPot.Control.SetIsBookedToPerson(family.Kids[i], booking);
        }
        PersonPot.Control.SetIsBookedToPerson(family.Father, booking);
        PersonPot.Control.SetIsBookedToPerson(family.Mother, booking);
    }

      void IdEveryOneOnTheFamily(string newFamId, Family family)
    {
        for (int i = 0; i < family.Kids.Count; i++)
        {
            PersonPot.Control.SetFamIDToPerson(family.Kids[i], newFamId);
        }
        PersonPot.Control.SetFamIDToPerson(family.Father, newFamId);
        PersonPot.Control.SetFamIDToPerson(family.Mother, newFamId);
    }
}
