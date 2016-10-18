using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class FPSReport
{
    private static string _report = "";
    private static int _counter;

    public static void Add(string add)
    {
        _report += add + FinalReport.Separator;
        _counter++;
    }

    public static void FinishReport(string addName = "")
    {
        Add(FinalReport.Separator + "TTL FPS: " + _counter + "");
        Dialog.CreateFile(addName + "FPS", _report);
    }
}

