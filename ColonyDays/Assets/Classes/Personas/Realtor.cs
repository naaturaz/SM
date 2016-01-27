using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Realtor
{
    private static int confortWeight = 100;


    public static Structure GiveMeTheBetterHome(Person person)
    {
        var key = "";
        if (!string.IsNullOrEmpty(person.IsBooked))
        {
            //if they are booked somewhere need to handle that first
            var bookedHome = Brain.GetStructureFromKey(person.IsBooked);
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
    static string LoopThruAllBetterHomes(Person person)
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
    static Structure HandleBooking(Person person, Structure newhome)
    {
        if (newhome == null)
        {
            return null;
        }

        //if is booked
        if (newhome.BookedHome1 != null && newhome.BookedHome1.IsBooked())
        {
            //and im booked on it 
            if (newhome.BookedHome1.IAmBookedHere(person))
            {
                //so its added to the family
                newhome.MovePersonToFamilySpot(person, newhome);
                //Debug.Log(person.MyId + " added to: " + newhome.MyId + " bz was booked");
                return newhome;
            }
            //if is booked and im not on it will return null bz that building is booked already
            return null;
        }

        BookMyFamilyToNewBuild(person, newhome);
        //if is not booked
        return newhome;
    }

    /// <summary>
    /// Has the logic that defined if is a bbeter home
    /// </summary>
    /// <param name="person"></param>
    /// <returns></returns>
    private static string DefineIfIsABetterHouse(Person person, string homeToEval, List<BuildRank> list)
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

    static string DefineBetterHome4Adult(Person person, string homeToEval)
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


    static string DefineBetterHome4Child(Person person, string homeToEval)
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
    static bool DoesFamilyFit(Person person, Building newHome)
    {
        var personHasFamily = person.HasFamily();
        //Debug.Log(person.MyId);

        if (personHasFamily)
        {
            var canI = CanIMoveFamilyToNewHome(newHome);
            if (canI)
            {
                BookMyFamilyToNewBuild(person, newHome);
                return true;
            }
        }
        //person does't have family
        else
        {
            var canI = CanIMoveFamilyToNewHome(newHome);
            if (canI)
            {
                BookMyFamilyToNewBuild(person, newHome);
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Will tell u if the 'person' is booked in the 'newHome'
    /// </summary>
    static bool AmIBookedInThatBuild(Person person, Building newHome)
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
    static bool DoesPersonFit(Person person, Building newHome)
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
            Structure s = (Structure)newHome;
            if (s.ThisPersonFitInThisHouse(person))
            {
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

    static Family FindCurrentFamily(Person person)
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
    public static void BookMyFamilyToNewBuild(Person person, Building newHome)
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
            famIDInBookedHome = curFamily.FamilyId;
        }
        else
        {
            var familyToBeTransferTo = SetFirstPersonInNewFamily(curFamily, newHome, person);
            IdEveryOneOnTheFamily(familyToBeTransferTo.FamilyId ,curFamily);
            famIDInBookedHome = familyToBeTransferTo.FamilyId;
        }
        BookMyFamilyToNewBuildTail(person, newHome, curFamily, famIDInBookedHome);
    }

    static void BookMyFamilyToNewBuildTail(Person person, Building newHome, Family myFamily, string famIDInBookedHome)
    {
        //seeting person as the first person in the family
        myFamily.CanGetAnotherAdult(person);
        newHome.BookedHome1 = new BookedHome(newHome.MyId, myFamily);
        
        //the booking is a copy dull of the Family but the ID must match the family we are going into
        newHome.BookedHome1.Family.FamilyId = famIDInBookedHome;

        BuildingPot.Control.Registro.ResaveOnRegistro(newHome.MyId);
        MarkTheFamilyBooking(newHome.MyId, myFamily);
        RestartControllerForMyFamily(myFamily, person);
    }

    private static Family SetFirstPersonInNewFamily(Family curFamily, Building newHome, Person newPerson)
    {
        var fam = newHome.ReturnEmptyFamily();
        //seeting person as the first person in the family
        fam.CanGetAnotherAdult(newPerson);

        return fam;
    }

    /// <summary>
    /// Books a a Person that doesnt have any family into a new place
    /// 
    /// so far used by Teens moving out of home 
    /// and now for just spwnred people
    /// </summary>
    /// <param name="person"></param>
    /// <param name="newHome"></param>
    public static void BookNewPersonInNewHome(Person person, Building newHome, string familyIDP)
    {
        var myFamily = newHome.FindFamilyById(familyIDP);
        person.FamilyId = myFamily.FamilyId;
        myFamily.CanGetAnotherAdult(person);

        //so i dont pull the existing family memebers and book them . whn they are actually already in the house
        Family famToBook = new Family();
        famToBook.FamilyId = person.FamilyId;
        famToBook.Home = newHome.MyId;
        famToBook.CanGetAnotherAdult(person);
        newHome.BookedHome1 = new BookedHome(newHome.MyId, famToBook);

        BuildingPot.Control.Registro.ResaveOnRegistro(newHome.MyId);
        MarkTheFamilyBooking(newHome.MyId, myFamily);
        RestartControllerForMyFamily(myFamily, person);
    }

    /// <summary>
    /// Will uncheck them from the controller so they can see their new Home Booked so theu can move there
    /// 
    /// PersonB is the one asking for this. Is theone tht booked and this person doesnt need to check again  
    /// </summary>
    static void RestartControllerForMyFamily(Family myFamily, Person personB)
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
    static bool CanIMoveFamilyToNewHome(Building newHome)
    {
        bool isBooked = IsBuildBooked(newHome);

        return newHome.ReturnEmptyFamily()!=null && !isBooked;
    }

    /// <summary>
    /// Will return true if building is booked 
    /// </summary>
    /// <returns></returns>
    static bool IsBuildBooked(Building newHome)
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
    static float ScoreABuild(Building building, Vector3 comparePoint)
    {
        var distToNewHome = Vector3.Distance(building.transform.position, comparePoint);
        var confort = building.Confort * confortWeight;

        return confort - distToNewHome;
    }

    /// <summary>
    /// Return a list with the Home rank ordered descending 
    /// 
    /// Above: that are above this score
    /// </summary>
    /// <param name="person"></param>
    /// <returns></returns>
    static List<BuildRank> ScoreAllAvailBuilds(Person person, Vector3 comparePoint, float above)
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

            var score = ScoreABuild(struc, comparePoint);

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
    static bool ThereIsABetterHome(Person person, List<BuildRank> list)
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

    public static bool PublicIsABetterHome(Person person)
    {
        var tempList = DefineBetterHomesList(person);

        return ThereIsABetterHome(person, tempList);
    }

    static List<BuildRank> DefineBetterHomesList(Person person)
    {
        var myCurrentHomeScore = ScoreCurrentHome(person);

        //will return list with all avail building tht have better score than 'myCurrentHomeScore'
        return ScoreAllAvailBuilds(person, person.transform.position, myCurrentHomeScore);
    }

    static float ScoreCurrentHome(Person person)
    {
        //to address the case if there is not home currently for the person
        //if will be destroy score will be -10000
        var myCurrentHomeScore = -10000f;
        if (person.Home != null && person.Home.Instruction != H.WillBeDestroy)
        {
            myCurrentHomeScore = ScoreABuild(person.Home, person.Home.transform.position);
        }
        return myCurrentHomeScore;
    }

    /// <summary>
    /// Mark everyone on the Family as booked 
    /// </summary>
    /// <param name="makeIt"></param>
    /// <param name="family"></param>
    static void MarkTheFamilyBooking(string booking, Family family)
    {
        for (int i = 0; i < family.Kids.Count; i++)
        {
            PersonPot.Control.SetIsBookedToPerson(family.Kids[i], booking);
        }
        PersonPot.Control.SetIsBookedToPerson(family.Father, booking);
        PersonPot.Control.SetIsBookedToPerson(family.Mother, booking);
    }

    static void IdEveryOneOnTheFamily(string newFamId, Family family)
    {
        for (int i = 0; i < family.Kids.Count; i++)
        {
            PersonPot.Control.SetFamIDToPerson(family.Kids[i], newFamId);
        }
        PersonPot.Control.SetFamIDToPerson(family.Father, newFamId);
        PersonPot.Control.SetFamIDToPerson(family.Mother, newFamId);
    }
}
