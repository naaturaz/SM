﻿using System.Collections.Generic;

public class ProgramData
{
    public string GameVersion;

    public List<string> Waves = new List<string>();

    public ProgramData()
    {
    }

    public ProgramData(string gameVersion)
    {
        GameVersion = gameVersion;
    }

    public bool SoundIsOn;
    public bool MusicIsOn;

    public float SoundLevel;
    public float MusicLevel;
    public char Units;
    public int AutoSaveFrec;
    public int QualityLevel;
    public bool isFullScreen;
    public string Lang;

    public List<string> Voices { get; set; }
}