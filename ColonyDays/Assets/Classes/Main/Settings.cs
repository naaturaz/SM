using UnityEngine;
using System.Collections;

public class Settings
{
    public static bool ISPAUSED;//not used in this proj



    static bool _isSoundOn = true;
    static bool _isMusicOn = true;

    //this is the music variable use for the whole game 
    public static Music music = null;


    private static float _autoSaveFrec = 300;//5min
    public static float AutoSaveFrec
    {
        get { return _autoSaveFrec; }
        set { _autoSaveFrec = value; }
    }

    public static bool ISSoundOn
    {
        get { return _isSoundOn; }
        set { _isSoundOn = value; }
    }

    public static bool ISMusicOn
    {
        get { return _isMusicOn; }
        set { _isMusicOn = value; }
    }









    public static Music Switch(H what, Music current = null)
    {
        if(what == H.Sound)
        {
            _isSoundOn = MecanicSwitcher(_isSoundOn);
        }
        else if(what == H.Music)
        {
            _isMusicOn = MecanicSwitcher(_isMusicOn);
            if (current != null || _isMusicOn)
            {
                current = KillOrRestart(current, _isMusicOn);
            }
        }
        return current;
    }

    static Music KillOrRestart(Music current, bool isMusicOnPass)
    {
        AudioPlayer ap = new AudioPlayer();
        if (isMusicOnPass) 
        { 
            if(Application.loadedLevelName == "Lobby")
            {
                current = (Music)ap.PlayAudio(RootSound.musicLobby, H.Music);
            }
            else current = (Music)ap.PlayAudio(RootSound.musicLvl1Start, H.Music);
        }
        else
        {   
            current.Destroy();
        }
        return current;
    }

    public static bool MecanicSwitcher(bool currentState)
    {
        if (currentState)
        {
            currentState = false;
        }
        else
        {
            currentState = true;
        }
        return currentState;
    }

    public static void PlayMusic()
    {
        AudioPlayer ap = new AudioPlayer();
        if(Application.loadedLevelName != "Lobby")
            music = (Music)ap.PlayAudio(RootSound.musicLvl1Start, H.Music);
        else if(Application.loadedLevelName == "Lobby")
            music = (Music)ap.PlayAudio(RootSound.musicLobby, H.Music);
    }
}

public class Unit
{
    //imperial or metric
    private static char _units = 'i';//'i'


    public static char Units
    {
        get { return _units; }
        set { _units = value; }
    }

    public static string WeightUnit()
    {
        if (Units == 'm')
        {
            return "kg";
        }
        return "lb";
    }

    public static string VolumeUnit()
    {
        if (Units == 'm')
        {
            return "m3";
        }
        return "ft3";
    }

    public static string DensityUnit()
    {
        if (Units == 'm')
        {
            return "kg/m3";
        }
        return "lb/ft3";
    }

    /// <summary>
    /// from Kg to lbs
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static float WeightFromMetricToImp(float input)
    {
        return input*2.2046f;
    }
    /// <summary>
    /// From m3 to ft3
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static float VolFromMetricToImp(float input)
    {
        return input*35.315f;
    }   
    
    /// <summary>
    /// from "kg/m3" to "lb/ft3"
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static float DensityFromMetricToImp(float input)
    {
        return input*0.062427974f;
    }



    internal static float  WeightConverted(float p)
    {
        if (_units == 'm')
        {
            return p;
        }
        return WeightFromMetricToImp(p);
    }

    internal static float VolConverted(float p)
    {
        if (_units == 'm')
        {
            return p;
        }
        return VolFromMetricToImp(p);
    }
}