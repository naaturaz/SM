using UnityEngine;
using System.Collections.Generic;

public class Family
{
    private int _kidsMax;//how many kids the house can have(they are always all younger than the couple in the house)
    private List<string> _kids = new List<string>();
    private string _mother = "";
    private string _father = "";
    private string _home = "";
    //will say wht family is up to. Iniitially created to keep family toghether , marking it as movingToNewHome
    H _state = H.None;

    private string _familyId;
    public string FamilyId
    {
        get { return _familyId; }
        set { _familyId = value; }
    }

    public Family() { }

    public Family(int kidsMax, string homeKey)
    {
        _kidsMax = kidsMax;
        _home = homeKey;
    }

    /// <summary>
    /// get right amt of kids
    /// create constructor that get right amt of kids and sets Id
    /// 
    /// This is useful when creating a brand new family and the new place is known too
    /// </summary>
    /// <param name="newHome">the new home of the new family</param>
    /// <param name="firstPerson">The person is initiaint this family</param>
    public Family(Building newHome, Person firstPerson)
    {
        _kidsMax = TellMeHowManyKidsThisHouseSupport(newHome.HType);
        FamilyId = "Family:" + firstPerson.MyId;
        _home = newHome.MyId;
    }

    int TellMeHowManyKidsThisHouseSupport(H HType)
    {
        //can hhave 1 famili with 3 kids
        if (HType == H.HouseA || HType == H.HouseB)
        {
            return 3;

        }
        //can hhave 2 famili with 3 kids each
        else if (HType == H.HouseAWithTwoFloor)
        {
            return 3;
        }
        //can hhave 1 famili with 5 kids
        else if (HType == H.HouseMedA || HType == H.HouseMedB || HType == H.HouseC || HType == H.HouseD)
        {
            return 5;
        }
        //else if (HType == H.Shack)
        //{
            return 3;
        //}
    }

    public List<string> Kids
    {
        get { return _kids; }
        set { _kids = value; }
    }

    public string Home
    {
        get { return _home; }
        set { _home = value; }
    }

    public H State
    {
        get { return _state; }
        set { _state = value; }
    }

    public string Mother
    {
        get { return _mother; }
        set { _mother = value; }
    }

    public string Father
    {
        get { return _father; }
        set { _father = value; }
    }

    public int KidsMax
    {
        get { return _kidsMax; }
        set { _kidsMax = value; }
    }

    /// <summary>
    /// Here will teel u if were successeful at getting a new kid i the famili and will 
    /// include it in the family and will take care of adoption and all, transform.parent, etc
    /// </summary>
    public bool CanGetAnotherKid(Person newP)
    {
        if (Kids.Count + 1 > _kidsMax)
        {
            return false;
        }

        if (!string.IsNullOrEmpty(_mother))
        {
            Person mom = FindPerson(_mother);
            //a pregnant woman wont welcome new kids 
            if (mom != null && mom.IsPregnant)
            {
                return false;
            }
        }

        //must have at least a mother or a father to be accepted in family 
        //unless is loading from file. Bz the order of the Saved list is random 
        if ((string.IsNullOrEmpty(_mother) && string.IsNullOrEmpty(_father)) && !newP.IsLoading) 
        {
            return false;
        }

        if (!string.IsNullOrEmpty(newP.FamilyId) && newP.FamilyId != FamilyId)
        {
            return false;
        }

        //if is not contained means the family is moving to a new home. So its not a good time
        //to get  anew kid .
        //Will restart the Kid on person controller so can check again in another place 
        if (!BuildingPot.Control.Registro.AllBuilding.ContainsKey(_home))
        {
            PersonPot.Control.RestartControllerForPerson(newP.MyId);
            return false;
        }

        AssignNewKidToThisFamily(newP);
        return true;
    }

    void AssignNewKidToThisFamily(Person newP)
    {
        Kids.Add(newP.MyId);
        newP.transform.parent = BuildingPot.Control.Registro.AllBuilding[_home].transform;

        if (string.IsNullOrEmpty(newP.FamilyId) && !string.IsNullOrEmpty(FamilyId))
        {
            newP.FamilyId = FamilyId;
        }

        if (string.IsNullOrEmpty(newP.Mother))
        {
            newP.Mother = _mother;
        }
        if (string.IsNullOrEmpty(newP.Father))
        {
            newP.Father = _father;
        } 
    }

    int Adults()
    {
        int res = 0;
        if (_mother != "") { res++; }
        if (_father != "") { res++; }
        return res;
    }

    void Set1stAdult(Person adult)
    {
        string debug = "M";//mother 
        if (adult.Gender == H.Male)
        {
            _father = adult.MyId;
            debug = "F";
        }
        else
        {
            _mother = adult.MyId;
        }

        if (!BuildingPot.Control.Registro.AllBuilding.ContainsKey(_home))
        {
            var t = this;
        }

        adult.transform.parent = BuildingPot.Control.Registro.AllBuilding[_home].transform;

        if (string.IsNullOrEmpty(adult.FamilyId))
        {
            FamilyId = "Family:" + adult.MyId;
            adult.FamilyId = FamilyId;    
        }
        Debug.Log(adult.MyId + " inscribed on " + FamilyId + " as " + debug);
    }

    //tis is if has already a adult we have to try to marry them
    //
    /// <summary>
    /// If dating is good will make param newPerson.transform a child of BuildingPot.Control.Registro.AllBuilding[_home]
    /// </summary>
    /// <param name="newPerson"></param>
    /// <returns></returns>
    bool WasDatingGood(Person newPerson)
    {
        Person inFamily = FindCurrentAdult();

        if (inFamily == null)
        {
            var t = this;
        }

        //2nd question is to check that other person is not Married already , etc
        if (inFamily.WouldUMarryMe(newPerson) && newPerson.WouldUMarryMe(inFamily))
        {
//            Debug.Log(inFamily.MyId + " accepted " + newPerson.MyId);

            inFamily.Marriage(newPerson.MyId);
            newPerson.Marriage(inFamily.MyId);

            AssignNewPersonToCurrentFamilyAndHome(newPerson);

            return true;
        }
        return false;
    }

    /// <summary>
    /// WIll assign the new person to its current role in the family and in the transform.parent 
    /// </summary>
    /// <param name="newPerson"></param>
    void AssignNewPersonToCurrentFamilyAndHome(Person newPerson)
    {
        if (_mother == "") { _mother = newPerson.MyId; }
        else if (_father == "") { _father = newPerson.MyId; }

        newPerson.transform.parent = BuildingPot.Control.Registro.AllBuilding[_home].transform;
        newPerson.FamilyId = FamilyId;
    }

    Person FindCurrentAdult()
    {
        Person res = null;
        if (_mother != "")
        {
            res =  PersonPot.Control.All.Find(a => a.MyId == _mother);
        }
        else if (_father != "")
        {
            res = PersonPot.Control.All.Find(a => a.MyId == _father);
        }
        return res;
    }

    public bool CanGetAnotherAdult(Person newPerson)
    {
        if (Adults() == 0)
        {
            Set1stAdult(newPerson);
            MakeAdultFatherOrMotherOfKids(newPerson);
            return true;
        }
        if (Adults() >= 2) { return false;}

        if (WasDatingGood(newPerson) || AreTheyMarriedAlready(newPerson))
        {
            MakeAdultFatherOrMotherOfKids(newPerson);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Will set the mother or father of current kids to 'newP'
    /// </summary>
    /// <param name="newP"></param>
    void MakeAdultFatherOrMotherOfKids(Person newP)
    {
        if (newP.Gender == H.Male)
        {
            MakeAdultFatherOfKids(newP);
        }
        else
        {
            MakeAdultMotherOfKids(newP);
        }
    }

    /// <summary>
    /// Will set the Adult as a Father or Mother in Family 
    /// field and it will set the adult family id to this family
    /// </summary>
    void SetAdultInFamily(Person adult, string momOrDad)
    {
        if (momOrDad == "M")
        {
            Mother = adult.MyId;
        }
        else if (momOrDad == "F")
        {
            Father = adult.MyId;
        }

        adult.FamilyId = FamilyId;

        adult.transform.parent = BuildingPot.Control.Registro.AllBuilding[_home].transform;
        Debug.Log(adult.MyId + " inscribed on " + FamilyId +" as " + momOrDad);
    }

    private void MakeAdultFatherOfKids(Person newP)
    {
        SetAdultInFamily(newP, "F");

        for (int i = 0; i < Kids.Count; i++)
        {
            var kid = FindPerson(Kids[i]);
            kid.Father = newP.MyId;
            kid.FamilyId = FamilyId;
        }
        
    }

    private void MakeAdultMotherOfKids(Person newP)
    {
        SetAdultInFamily(newP, "M");

        for (int i = 0; i < Kids.Count; i++)
        {
            var kid = FindPerson(Kids[i]);
            kid.Mother = newP.MyId;
            kid.FamilyId = FamilyId;
        }
    }


    public static Person FindPerson(string find)
    {
        return PersonPot.Control.All.Find(a => a.MyId == find);
    }

    /// <summary>
    /// Will tell u if person living in home is married to new person
    /// This is created to rejoin maried copuples while moving to new homes 
    /// </summary>
    /// <param name="newPerson"></param>
    /// <returns></returns>
    private bool AreTheyMarriedAlready(Person newPerson)
    {
        Person inFamily = FindCurrentAdult();

        if (inFamily.Spouse == newPerson.MyId)
        {
            AssignNewPersonToCurrentFamilyAndHome(newPerson);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Will tell u if the person asking belong to this family
    /// </summary>
    /// <param name="askPerson"></param>
    /// <returns></returns>
    public bool DoIBelongToThisFamily(Person askPerson)
    {
        for (int i = 0; i < _kids.Count; i++)
        {
            if (askPerson.MyId == _kids[i] && askPerson.FamilyId == FamilyId)
            {
                return true;
            }
        }

        if ((askPerson.MyId == _father || askPerson.MyId == _mother)  && askPerson.FamilyId == FamilyId)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Will tell if in the family has more than one member 
    /// </summary>
    /// <param name="askPerson"></param>
    /// <returns></returns>
    public bool DoesFamilyHasMoreThan1Member()
    {
        var members = MembersOfAFamily();

        if (members > 1)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Will tell u if a Family is empty. Has zero members 
    /// </summary>
    /// <returns></returns>
    public bool IsFamilyEmpty()
    {
        return 0 == MembersOfAFamily();
    }

    /// <summary>
    /// Will return how manny members a family has 
    /// </summary>
    /// <returns></returns>
    public int MembersOfAFamily()
    {
        int kids = 0;

        for (int i = 0; i < _kids.Count; i++)
        {
            if (!string.IsNullOrEmpty(_kids[i]))
            {
                kids++;
            }
        }

        return kids + Adults();
    }


    public void DeleteFamily()
    {
        Home = "";
        State = H.None;

        Kids.Clear();
        _father = "";
        _mother = "";
    }

    /// <summary>
    /// Used to move one family booked to the final destination 
    /// 
    /// Or to remove kid tht reach majority
    /// </summary>
    public void RemovePersonFromFamily(Person personToRemove)
    {
        for (int i = 0; i < Kids.Count; i++)
        {
            if (Kids[i] == personToRemove.MyId)
            {
                Kids.RemoveAt(i);
                break;
            }
        }

        if (_father == personToRemove.MyId)
        {
            _father = "";
        }
        else if (_mother == personToRemove.MyId)
        {
            _mother = "";
        }
    }

    /// <summary>
    /// If a family was form. Mom and Dad then will reutnr true
    /// </summary>
    /// <returns></returns>
    public bool AFamilyIsFormed()
    {
        if (!string.IsNullOrEmpty(_father) && !string.IsNullOrEmpty(_mother))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Will tell u when all members of a Family have been completed. Done so houses with people just married
    /// are open to accept kids 
    /// </summary>
    /// <returns></returns>
    internal bool AFamilyIsFull()
    {
        return Kids.Count == _kidsMax && AFamilyIsFormed();
    }

    /// <summary>
    /// Will tell u if can get a new Kid in the House 
    /// </summary>
    /// <returns></returns>
    public bool QuickQuuestionCanGetAnotherKid()
    {
        if (Kids.Count < _kidsMax)
        {
            return true;
        }

        return false;
    }

    internal void MakeVirgin()
    {
        FamilyId = "";
    }

    /// <summary>
    /// Will return a list of the active members as a person object each 
    /// </summary>
    /// <returns></returns>
    public List<Person> ReturnFamilyPersonObj()
    {
        List<Person> res = new List<Person>();
        for (int i = 0; i < Kids.Count; i++)
        {
            var kid = FindPerson(Kids[i]);

            if (kid != null)
            {
                res.Add(kid);
            }
        }

        var mom = FindPerson(_mother);
        var dad = FindPerson(_father);

        if (mom != null)
        {
            res.Add(mom);
        }
        if (dad!=null)
        {
            res.Add(dad);
        }

        return res;
    }

    /// <summary>
    /// Created so a person that could reach majority of age knows if can find a place where to 
    /// live or not 
    /// </summary>
    /// <param name="asker"></param>
    /// <returns></returns>
    public bool WouldAdultFitInThisFamily(Person newPerson)
    {
        if (Adults() >= 2)
        {
            return false;
        }

        if (Adults() == 0)
        {
            return true;
        }

        Person inFamily = FindCurrentAdult();
        //2nd question is to check that other person is not Married already , etc
        if (inFamily.WouldUMarryMe(newPerson) && newPerson.WouldUMarryMe(inFamily))
        {
            return true;
        }

        return false;
    }
}
