/*
 * All actions related a person reaching the majority of age.
 * this was a section in Brain . Decided to make it a class for readability etc
 * 
 */ 
using UnityEngine;

/// <summary>
/// Is inheritng from Brain just to access the field _person. 
/// It couldnt be public bz XML serializer will find redundancy
/// </summary>
public class MajorityAgeReached  {

    private Brain _brain;
    private bool majorityAgeRecentReached;
    private Person _person;
    private MoveToNewHome _moveToNewHome;


    public MajorityAgeReached() { }

    public MajorityAgeReached(Brain brain, Person person , MoveToNewHome moveToNewHome)
    {
        _person = person;
        _brain = brain;
        _moveToNewHome = moveToNewHome;
    }

    public void MarkMajorityAgeReached()
    {
        majorityAgeRecentReached = true;
    }

    /// <summary>
    /// Actions to execute on Brain once the majority of age was reached
    /// </summary>
    private void PersonReachMajorityAgeAction()
    {
        _person.IsBooked = false;

        _moveToNewHome.AddToHomeOldKeysList();
        _person.transform.parent = null;
        _person.FamilyId = "";
        _person.Home = null;

        if (_person.MyId.Contains("460"))
        {
            var t = this;
        }

        //ShacksManager.NewAdultIsUp();
        Debug.Log(_person.MyId + " reached majority");

        PersonPot.Control.RestartControllerForPerson(_person.MyId);
    }

    public void CheckIfMajorityRecentlyReached()
    {
        if (majorityAgeRecentReached && _brain.IAmHomeNow())
        {
            majorityAgeRecentReached = false;
            PersonReachMajorityAgeAction();
        }
    }
}
