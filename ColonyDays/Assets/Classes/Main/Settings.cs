using System;
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


    

    static void LoadAndApplyResolution()
    {
        var width = PlayerPrefs.GetInt("Res.Width");
        var height = PlayerPrefs.GetInt("Res.Height");

        Screen.SetResolution(width, height, Screen.fullScreen);

        //var scnLoaded = PlayerPrefs.GetString("SceneSaved");
    }



    /// <summary>
    /// Will be called from OptionsWindow
    /// </summary>
    /// <param name="newResolution"></param>
    public static void SaveResolution(string newResolution)
    {
        Debug.Log(Screen.currentResolution.ToString());

        var splt = newResolution.Split(' ');
        PlayerPrefs.SetInt("Res.Width", Int32.Parse(splt[0]));
        PlayerPrefs.SetInt("Res.Height", Int32.Parse(splt[2]));

        //PlayerPrefs.SetString("SceneSaved", "Scn02");
    }



#endregion

    #region Audio
    

    public static Music Switch(H what, Music current = null)
    {
        if(what == H.Sound)
        {
            _isSoundOn = MecanicSwitcher(_isSoundOn);
            AudioCollector.SoundIsSwitchNow();
        }
        else if(what == H.Music)
        {
            _isMusicOn = MecanicSwitcher(_isMusicOn);
            MusicManager.MusicIsSwitchNow();
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



    static bool loadedOnce;
    /// <summary>
    /// Loads Audio Settings from file
    /// </summary>
    public static void LoadFromFile()
    {
        if (loadedOnce)
        {
            return;
        }
        loadedOnce = true;
        var pData = XMLSerie.ReadXMLProgram();

        Settings.ISSoundOn = pData.SoundIsOn;
        Settings.ISMusicOn = pData.MusicIsOn;
        AudioCollector.SoundLevel = pData.SoundLevel;
        AudioCollector.MusicLevel = pData.MusicLevel;
        Unit.Units = pData.Units;
        _autoSaveFrec = pData.AutoSaveFrec;
        //in case was deleted or somehting
        if (_autoSaveFrec < 300)
        {
            _autoSaveFrec = 300;
        }

        QualitySettings.SetQualityLevel(pData.QualityLevel);
        Screen.fullScreen = pData.isFullScreen;
        Languages.SetCurrentLang(pData.Lang);
        //in case was deleted or somehting
        if (string.IsNullOrEmpty(Languages.CurrentLang()))
        {
            Languages.SetCurrentLang("English");
        }

        Debug.Log("Loading Settings");

    }

    /// <summary>
    /// Saves to file audio settings 
    /// </summary>
    public static void SaveToFile()
    {
        var pData = XMLSerie.ReadXMLProgram();

        pData.SoundIsOn = Settings.ISSoundOn;
        pData.MusicIsOn = Settings.ISMusicOn;

        pData.SoundLevel = AudioCollector.SoundLevel;
        pData.MusicLevel = AudioCollector.MusicLevel;

        pData.Units = Unit.CurrentSystem();
        pData.AutoSaveFrec = AutoSaveFrec;

        //screen
        pData.QualityLevel = QualitySettings.GetQualityLevel();
        pData.isFullScreen = Screen.fullScreen;

        pData.Lang = Languages.CurrentLang();

        XMLSerie.WriteXMLProgram(pData);
        Debug.Log("Saving settings");

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
        AutoSaveFrec = Int32.Parse(spl[0]) * 60;
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

    private static float WeightFromImpToMetric(float input)
    {
        return input / 2.2046f;
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
    /// 
    /// 
    /// </summary>
    /// <param name="p">The metric amount</param>
    /// <returns></returns>
    internal static float  WeightConverted(float p)
    {
        if (_units == 'm')
        {
            return p;
        }
        return WeightFromMetricToImp(p);
    }

    /// <summary>
    /// used to The input amt in order Converted if needed
    /// </summary>
    /// <param name="p"></param>
    /// <returns></returns>
    public static float ConvertFromKGToCurrent(float p)
    {
        if (_units == 'm')
        {
            return p;
        }
        return WeightFromImpToMetric(p);

    }



    internal static float VolConverted(float p)
    {
        if (_units == 'm')
        {
            return p;
        }
        return VolFromMetricToImp(p);
    }

    internal static char CurrentSystem()
    {
        return _units;
    }

    internal static bool IsCurrentSystemMetric()
    {
        return _units == 'm';
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static float CelsiusToFarenheit(float input)
    {
        return (input * 1.8f) + 32;
    }


    internal static string CurrentWeightUnitsString()
    {
        if (_units == 'm')
        {
            return "KG";
        }
        return "Lbs";
    }
}