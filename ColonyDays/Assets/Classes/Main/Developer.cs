using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Sometimes developing mode is needed . to add new terrains for example.
/// </summary>
class Developer
{
    //if is dev true will be able to select terrain 
    private static bool _isDev = false;

    public static bool IsDev
    {
        get { return _isDev; }
        set { _isDev = value; }
    }
}

