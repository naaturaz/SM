using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



public class FinalReportData
{
    public string GameVersion;

    public List<string> Waves = new List<string>();

    public FinalReportData() { }

    public FinalReportData(string gameVersion)
    {
        GameVersion = gameVersion;
    }

}
