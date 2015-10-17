using UnityEngine;
using System.Collections;

public class UPerson
{
    static public bool IsMajor(int currAge)
    {
        if (currAge >= JobManager.majorityAge)
        {
            return true;
        }
        return false;
    }

    static public bool IsWorkingAtSchool(Person person)
    {
        if (person.Work == null)
        {
            return false;
        }
        if (person.Work.HType == H.School || person.Work.HType == H.TradesSchool)
        {
            return true;
        }
        return false;
    }

}
