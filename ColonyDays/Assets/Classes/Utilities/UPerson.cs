using UnityEngine;

public class UPerson
{
    static public bool IsMajor(int currAge)
    {
        if (currAge >= ModController.AgeMajorityReached())
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

    internal static bool IsThisPersonTheSelectedOne(Person person)
    {
        if (!Developer.IsDev) return false;
        if (person == null) return false;

        var win = GameObject.FindObjectOfType<PersonWindow>();
        return Program.MouseListener.PersonSelect != null && Program.MouseListener.PersonSelect.MyId == person.MyId && win.IsShownNow() ||
             Program.Debugger.IsThisOneTarget(person);
    }
}