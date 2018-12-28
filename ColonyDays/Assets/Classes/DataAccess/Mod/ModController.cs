using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

//Attached to Mod GameObject
public class ModController: MonoBehaviour
{
    //GO
    public Text Text;

    //private
    float _shown;

    public static PeopleModData PeopleModData;

    public void ReloadMods()
    {
        PeopleModData = XMLSerie.ReadXMLPeopleModData();
        if (PeopleModData != null)
        {
            Text.text = "xml ok";
        }
        else
        {
            Text.text = "xml error";
        }
        _shown = Time.time;
    }

    private void Update()
    {
        if(Text.text.Length>0 && Time.time > _shown + 5)
        {
            Text.text = "";
        }
    }

    public static int AllowedAgeGapOnMarry()
    {
        int age = 0;
        if (PeopleModData != null)
        {
            int.TryParse(PeopleModData.AllowedAgeGapOnMarry + "", out age);
        }

        if (age > 0)
            return age;

        return 20;
    }

    public static int AgeKidStartSchool()
    {
        int age = 0;
        if (PeopleModData != null)
        {
            int.TryParse(PeopleModData.AgeKidStartSchool + "", out age);
        }

        if (age > 0)
            return age;

        return 5;
    }

    public static int AgeKidStartTradeSchool()
    {
        int age = 0;
        if (PeopleModData != null)
        {
            int.TryParse(PeopleModData.AgeKidStartTradeSchool + "", out age);
        }

        if (age > 0)
            return age;

        return 11;
    }

    public static int AgeLimit()
    {
        int age = UnityEngine.Random.Range(75, 85);
        int age2 = 0;

        if (PeopleModData != null)
        {
            int.TryParse(UnityEngine.Random.Range(PeopleModData.DieAgeStart, PeopleModData.DieAgeEnd)+"", out age2);
        }

        if (age2 > 0)
            return age2;

        return age;
    }
}
