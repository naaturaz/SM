using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Options
{
    private static float _autoSaveFrec = 300;//5min
    public static float AutoSaveFrec
    {
        get { return _autoSaveFrec; }
        set { _autoSaveFrec = value; }
    }

    public Options()
    {

        Load();
    }

    private void Load()
    {

    }

    public void Save()
    {
        
    }
}

