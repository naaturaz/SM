using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class InputReport
{
    private static string _report = "";
    private static int _counter;

    public static void Add(string add)
    {
        _report += add + "\n";
        _counter++;
    }

    public static void FinishReport(string addName = "")
    {
        Add("TTL Inputs: "+_counter+"");
        Dialog.CreateFile(addName+"Input", _report);
    }
}

