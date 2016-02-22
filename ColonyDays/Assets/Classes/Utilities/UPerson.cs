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

    static public bool IsWorkingAtSchool(Person person, Structure newWork)
    {
        if (newWork == null)
        {
            return false;
        }
        if (newWork.HType == H.School || newWork.HType == H.TradesSchool)
        {
            return true;
        }
        return false;
    }

}
