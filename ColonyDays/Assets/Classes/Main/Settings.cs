using UnityEngine;
using System.Collections;

public class Settings : MonoBehaviour
{

    //imperial or metric
    private static char _unitSystem = 'm';//'i'

    public static bool ISPAUSED;

    static bool _isSoundOn = true;
    static bool _isMusicOn = true;

    //this is the music variable use for the whole game 
    public static Music music = null;

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

    public static char UnitSystem
    {
        get { return _unitSystem; }
        set { _unitSystem = value; }
    }

    public static string ReturnStringInSystem(string param)
    {
        if (param == "kg")
        {
            if (UnitSystem == 'm')
            {
                return param;
            }
            return "lbs";
        }
        return "nothing from Settings.cs";
    }

    public static string WeightUnit()
    {
        if (UnitSystem == 'm')
        {
            return "kg";
        }
        return "lb";
    }  
    
    public static string DensityUnit()
    {
        if (UnitSystem == 'm')
        {
            return "kg/m3";
        }
        return "lb/ft3";
    }



    public static bool ISToAskB4Exit = true;

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
            Destroy(current.gameObject);
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