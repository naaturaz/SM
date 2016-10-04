using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



public class ProgramData
{
    public string GameVersion;

    public List<string> Waves =new List<string>(); 

    public ProgramData() { }

    public ProgramData(string gameVersion)
    {
        GameVersion = gameVersion;
    }

    public bool SoundIsOn;
    public bool MusicIsOn;

    public float SoundLevel;
    public float MusicLevel;
}
