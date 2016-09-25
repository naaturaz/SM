using UnityEngine;
using System.Collections;

public class Settings
{
    public static bool ISPAUSED;//not used in this proj

    static bool _isSoundOn = true;
    static bool _isMusicOn = true;

    //this is the music variable use for the whole game 
    public static Music music = null;


    private static int _autoSaveFrec = 300;//5min
    public static int AutoSaveFrec
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

#region Save Load Settings

    /// <summary>
    /// Loads all Saved settings in PlayerPref
    /// </summary>
    public static void Load()
    {
        //general
        var unitsSaved = PlayerPrefs.GetString("Unit").ToCharArray();
        if (unitsSaved.Length>0)
        {
            Unit.Units = unitsSaved[0];
        }
        //means never is being saved before     
        else if (unitsSaved.Length == 0)
        {
            return;
        }
        _autoSaveFrec = PlayerPrefs.GetInt("AutoSave");

        //screen
        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("Quality"));
        //LoadAndApplyResolution();
        Screen.fullScreen = bool.Parse(PlayerPrefs.GetString("FullScreen"));

        //audio
        ISMusicOn = bool.Parse(PlayerPrefs.GetString("Music"));
        ISSoundOn = bool.Parse(PlayerPrefs.GetString("Sound"));

        Debug.Log("Loading Settings");
    }

    static void LoadAndApplyResolution()
    {
        var width = PlayerPrefs.GetInt("Res.Width");
        var height = PlayerPrefs.GetInt("Res.Height");

        Screen.SetResolution(width, height, Screen.fullScreen);

    }

    /// <summary>
    /// Saves all setting in PlayerPref 
    /// </summary>
    public static void Save()
    {
        //general
        PlayerPrefs.SetString("Unit", Unit.CurrentSystem());
        PlayerPrefs.SetInt("AutoSave", AutoSaveFrec);
        
        //screen
        PlayerPrefs.SetInt("Quality", QualitySettings.GetQualityLevel());
        PlayerPrefs.SetString("FullScreen", Screen.fullScreen.ToString());

        //del call 
        //SaveResolution("1920 x 1080");

        //audio
        PlayerPrefs.SetString("Music", ISMusicOn.ToString());
        PlayerPrefs.SetString("Sound", ISSoundOn.ToString());

        PlayerPrefs.Save();
    }

    /// <summary>
    /// Will be called from OptionsWindow
    /// </summary>
    /// <param name="newResolution"></param>
    public static void SaveResolution(string newResolution)
    {
        Debug.Log(Screen.currentResolution.ToString());

        var splt = newResolution.Split(' ');
        PlayerPrefs.SetInt("Res.Width", int.Parse(splt[0]));
        PlayerPrefs.SetInt("Res.Height", int.Parse(splt[2]));
    }



#endregion

    #region Audio

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
        if (isMusicOnPass) 
        { 
            if(Application.loadedLevelName == "Lobby")
            {
                current = (Music)AudioPlayer.PlayAudio(RootSound.musicLobby, H.Music);
            }
            else current = (Music)AudioPlayer.PlayAudio(RootSound.musicLvl1Start, H.Music);
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
        if(Application.loadedLevelName != "Lobby")
            music = (Music)AudioPlayer.PlayAudio(RootSound.musicLvl1Start, H.Music);
        else if(Application.loadedLevelName == "Lobby")
            music = (Music)AudioPlayer.PlayAudio(RootSound.musicLobby, H.Music);
    }
#endregion

#region Change Params 
    internal static void SetQuality(string qual)
    {
        string[] names = QualitySettings.names;

        for (int i = 0; i < names.Length; i++)
        {
            if (qual == names[i])
            {
                QualitySettings.SetQualityLevel(i);
            }
        }
        Program.MyScreen1.OptionsWindow1.RefreshAllDropDowns();

    }

    internal static void SetResolution(string name)
    {
        SaveResolution(name);
        LoadAndApplyResolution();            
     
        Program.MyScreen1.OptionsWindow1.ChangeResNow();
        U2D.RedoScreenRect();
    }

    internal static void SetUnit(string sub)
    {
        if (sub == "Metric")
        {
            Unit.Units = 'm';
        }
        else if (sub == "Imperial")
        {
            Unit.Units = 'i';
        }
        //so options menu is not gone
        Program.MyScreen1.OptionsWindow1.ChangeResNow();
    }

    internal static void SetAutoSave(string name)
    {
        var spl = name.Split(' ');
        AutoSaveFrec = int.Parse(spl[0]) * 60;
        Program.MyScreen1.OptionsWindow1.RefreshAllDropDowns();
    }  
    
    internal static void SetLanguage(string name)
    {
        Languages.SetCurrentLang(name);
        Program.MyScreen1.OptionsWindow1.RefreshAllDropDowns();

        //So all LangUpdateScripts gets Started again
        Program.MouseListener.ApplyChangeScreenResolution();
    }
#endregion






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


    /// <summary>
    /// Weight converted to proper current Unit
    /// </summary>
    /// <param name="p"></param>
    /// <returns></returns>
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

    internal static string CurrentSystem()
    {
        return _units.ToString();
    }
}