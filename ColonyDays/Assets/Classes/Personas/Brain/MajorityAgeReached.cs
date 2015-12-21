/*
 * All actions related a person reaching the majority of age.
 * this was a section in Brain . Decided to make it a class for readability etc
 * 
 */ 
using UnityEngine;

public class MajorityAgeReached : MonoBehaviour {

  private Brain _brain;
    private bool majorityAgeRecentReached;

    public MajorityAgeReached(Brain brain)
    {
        _brain = brain;
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
        _brain.Person1.IsBooked = false;

        _brain.MoveToNewHome.AddToHomeOldKeysList();
        _brain.Person1.transform.parent = null;
        _brain.Person1.FamilyId = "";

        ShacksManager.NewAdultIsUp();
        //Debug.Log(_person.MyId +" reached majority");

        PersonPot.Control.RestartControllerForPerson(_brain.Person1.MyId);
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
