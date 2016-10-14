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
    private bool _majorityAgeRecentReached;
    private Person _person;
    private MoveToNewHome _moveToNewHome;


    public bool MajorityAgeRecentReached
    {
        get { return _majorityAgeRecentReached; }
        set { _majorityAgeRecentReached = value; }
    }


    public MajorityAgeReached() { }

    public MajorityAgeReached(Brain brain, Person person , MoveToNewHome moveToNewHome)
    {
        _person = person;
        _brain = brain;
        _moveToNewHome = moveToNewHome;
    }

    /// <summary>
    /// Loading
    /// </summary>
    /// <param name="brain"></param>
    /// <param name="person"></param>
    /// <param name="moveToNewHome"></param>
    /// <param name="pF"></param>
    public MajorityAgeReached(Brain brain, Person person, MoveToNewHome moveToNewHome, PersonFile pF)
    {
        _person = person;
        _brain = brain;
        _moveToNewHome = moveToNewHome;
        
        MajorityAgeRecentReached = pF._brain.MajorAge.MajorityAgeRecentReached;
    }


    public void MarkMajorityAgeReached()
    {
        _majorityAgeRecentReached = true;
    }

    /// <summary>
    /// Actions to execute on Brain once the majority of age was reached
    /// </summary>
    private void PersonReachMajorityAgeAction()
    {
        _moveToNewHome.AddToHomeOldKeysList();
        _person.transform.SetParent( null);
        _person.Home = null;

       //Debug.Log(_person.MyId + " reached majority");
        PersonPot.Control.RestartControllerForPerson(_person.MyId);
    }

    public void CheckIfMajorityRecentlyReached()
    {
        if (_majorityAgeRecentReached && _brain.IAmHomeNow())
        {
            _majorityAgeRecentReached = false;
            PersonReachMajorityAgeAction();
        }
    }

    internal void RollBackMoajority()
    {
        _majorityAgeRecentReached = false;
    }
}
