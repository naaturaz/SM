using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using System.Collections;
using System.Linq;

public class UString : MonoBehaviour {

    /// <summary>
    ///     extract names of transform until granpa 
    ///will check too if the have the format of a MyId property
    /// Good: kaka | hahah | 272
    /// Bad: assvs | as_da | 345645645
    /// Bad: House2 | House 2 | 82828   //numbers are only allowed on the last part 
    /// </summary>
    public static List<string> ExtractNamesUntilGranpa(Transform tra)
    {
        List<string> res = new List<string>();
        if (IsAValidMyId(tra.name))
        {
            res.Add(tra.name);
        }
        Transform par = tra.transform.parent;
        if (par != null)
        {
            if (IsAValidMyId(par.name))
            {
                res.Add(par.name);
            }
            Transform granpa = par.parent;
            if (granpa != null)
            {
                if (IsAValidMyId(granpa.name))
                {
                    res.Add(granpa.name);
                }
            }
        }
        return res;

    }

    //Will math this :
    //    kaka | hahah | 272
    //asda.asda | jaja | 33
    //lalal.llalal | asda | 32
    //kahsdkahskdkasd.as.asd | asda | 345645645
    //s | asda | 345645645

    //not this 
    //s | as_da | 345645645
    public static  bool IsAValidMyId(string strinP)
    {
        Regex regNot = new Regex(@"([a-zA-Z\.]+) \| (none) \| (\d+)");//if contains ' | None | ' will return false
        Regex regPass = new Regex(@"([a-zA-Z\.]+) \| ([a-zA-Z]+) \| (\d+)");

        if (regNot.IsMatch(strinP))
        {
            return false;
        }
        else if (regPass.IsMatch(strinP))
        {
            return true;
        }
        return false;
    }


    public static string ReturnMostCommonName(List<string> names)
    {
        var groupsWithCounts = from s in names
            group s by s into g
            select new
            {
                Item = g.Key,
                Count = g.Count()
            };

        var groupsSorted = groupsWithCounts.OrderByDescending(g => g.Count).ToList();
        string mostFrequest = groupsSorted[0].Item;

        return mostFrequest;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pass">pass The string pass</param>
    /// <param name="max">amount of spaces</param>
    /// <param name="riOrLeft">padding on the right or left</param>
    /// <returns> Spaces depending on the pass.lenght and max </returns>
    public static string PadThis(string pass, int max, char riOrLeft, string pad = " ")
    {
        int spaces = max - pass.Length;

        for (int i = 0; i < spaces; i++)
        {
            if (riOrLeft == 'r')
            {
                pass += pad;
            }
            else if (riOrLeft == 'l')
            {
                pass = pad + pass;
            }
        }
        return pass;
    }


    /// <summary>
    /// Returns only the first part of a String
    /// 
    /// ex in: 'Cuco.Perez' out 'Cuco'
    /// </summary>
    /// <param name="pass"></param>
    /// <returns></returns>
    public static string RemoveLastPart(string pass)
    {
        var arr = pass.Split('.');
        return arr[0];
    }

}
