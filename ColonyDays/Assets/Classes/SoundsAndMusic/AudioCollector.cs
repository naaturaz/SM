using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Classes.SoundsAndMusic;
using Random = UnityEngine.Random;


public class AudioCollector
{

    //determined by user on Interface. Saved and loaded too
    private static float _soundLevel = 1;
    private static float _musicLevel = 1;


    static Dictionary<string, string> _languages = new Dictionary<string, string>();

    //TO ADD A SOUND ====>
    //Add first the type of HType is initiating the Sound Ex: "Person" then the sound
    //as named in Prefab/Audio/Sound/Other/
    //place the sound in that folder
    //done 
    static Dictionary<string, string> _roots = new Dictionary<string, string>()
    {

    };


    //TO ADD A SOUND in person added below pls the other steps ====>
    //Keep in mind that Animations sounds must be played from Body.cs
    static Dictionary<string, string> _personRoots = new Dictionary<string, string>()
    {
        {"Person", ""},//so persons are spawned
        //Animations
        {"isHoe", ""},
        {"isWheelBarrow", ""},
        {"isAxe", ""},
        {"isHammer", ""},

        

    };

    //this roots sounds get spawned anywas. Like BabyBorn sound
    private static Dictionary<string, string> _rootsToSpawn = new Dictionary<string, string>()
    {
        //one shots 
        {"BabyBorn", ""},
        {"Emigrate", ""},
        {"FallingTree", ""},



        {"PirateUp", ""},
        {"PirateDown", ""},
        {"PortUp", ""},
        {"PortDown", ""},
        {"BoughtLand", ""},
        {"ShipPayed", ""},
        {"ShipArrived", ""},

        {"ClickMetal1", ""},
        {"ClickWood1", ""},
        {"ClickWood4", ""},
        {"ClickWood7", ""},
        {"ClickWoodSubtle", ""},
    };


    //this roots sounds get spawned anywas. Like BabyBorn sound
    private static Dictionary<string, string> _ambience = new Dictionary<string, string>()
    {
        //one shots 
        {"FullOcean", ""},
        {"InLand", ""},
        {"OceanShore", ""},
        {"Jungle", ""},
        {"River", ""},
        {"OutOfTerrain", ""},
        {"Mountain", ""},
    };






    static Dictionary<string, AudioContainer> _audioContainers = new Dictionary<string, AudioContainer>();

    static Dictionary<string, AudioReport> _report = new Dictionary<string, AudioReport>();

    static float _timeLastReport;

    /// <summary>
    /// The roots that are on file for sounds
    /// </summary>
    public static Dictionary<string, string> Roots
    {
        get
        {
            StartRoots();
            return _roots;
        }
        set { _roots = value; }
    }

    public static Dictionary<string, string> PersonRoots
    {
        get { return _personRoots; }
        set { _personRoots = value; }
    }

    public static Dictionary<string, string> RootsToSpawn
    {
        get { return _rootsToSpawn; }
        set { _rootsToSpawn = value; }
    }

    public static Dictionary<string, string> Ambience
    {
        get { return _ambience; }
        set { _ambience = value; }
    }

    public static Dictionary<string, AudioContainer> AudioContainers
    {
        get { return _audioContainers; }
        set { _audioContainers = value; }
    }

    public static float MusicLevel
    {
        get { return _musicLevel; }
        set { _musicLevel = value; }
    }

    public static float SoundLevel
    {
        get { return _soundLevel; }
        set { _soundLevel = value; }
    }

    public static Dictionary<string, string> Languages1
    {
        get { return _languages; }
        set { _languages = value; }
    }

    static void StartRoots()
    {
        if (_roots.Count > 0)
        {
            return;
        }

        foreach (var personRoot in _personRoots)
        {
            _roots.Add(personRoot.Key, personRoot.Value);
        }

        foreach (var amb in _ambience)
        {
            _roots.Add(amb.Key, amb.Value);
        }

        foreach (var amb in _rootsToSpawn)
        {
            _roots.Add(amb.Key, amb.Value);
        }
    }

    public static void SpawnSounds()
    {

        for (int i = 0; i < _roots.Count; i++)
        {
            var root = DefineRoot(_roots.ElementAt(i).Key);


            var audCont = AudioContainer.Create(_roots.ElementAt(i).Key, root, 0,
                container: AudioPlayer.SoundsCointaner.transform);

            LevelChanged += audCont.LevelChanged;


            _audioContainers.Add(_roots.ElementAt(i).Key, audCont);
        }
    }



    /// <summary>
    /// Reporting how far an GameObj is 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="dist"></param>
    internal static void Reporting(string key, Vector3 v3)
    {
        Reporting(key, Vector3.Distance(v3, Camera.main.transform.position));
    }


    /// <summary>
    /// Reporting how far an GameObj is 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="dist"></param>
    internal static void Reporting(string key, float dist)
    {
        //if is not on Roots is useless to add on report
        if (!_roots.ContainsKey(key))
        {
            return;
        }

        //Debug.Log("Reported: "+key +"|| dist: "+dist);
        if (_report.ContainsKey(key))
        {
            _report[key].NewReport(dist);
        }
        else
        {
            _report.Add(key, new AudioReport(key, dist));
        }
        _timeLastReport = Time.time;
    }

    static IEnumerable<KeyValuePair<string, AudioReport>> reportOrdered;
    /// <summary>
    /// Executing the report. It ordered it and then acts in the first 10
    /// </summary>
    static void ExecuteReport()
    {
        //take is limit
        //is Descending bz the closest one is the one that reported less
        //desitance 
        reportOrdered = _report.OrderByDescending(a => a.Value.AverageDistance()).Take(40);

        foreach (var item in reportOrdered)
        {
            OnTheFirst40Items(item);
        }

        //we need to clean the report after
        _report.Clear();

    }

    /// <summary>
    /// The first 10 items will be pplayed by its Audio Container 
    /// </summary>
    /// <param name="item"></param>
    private static void OnTheFirst40Items(KeyValuePair<string, AudioReport> item)
    {
        //if is there will be set to  a NewLevel()
        if (_audioContainers.ContainsKey(item.Key))
        {
            _audioContainers[item.Key].Play(item.Value.AverageDistance());
        }
        //Other wise will be spawned 
        else
        {
            if (!_roots.ContainsKey(item.Key))
            {
                return;
            }

            if (string.IsNullOrEmpty(_roots[item.Key]))
            {
                _roots[item.Key] = DefineRoot(item.Key);
            }
            var root = _roots[item.Key];

            _audioContainers.Add(item.Key, AudioContainer.Create(item.Key, root, item.Value.AverageDistance(),
                container: AudioPlayer.SoundsCointaner.transform));
        }
    }

    private static string DefineRoot(string key)
    {
        var plsFolders = "Prefab/Audio/Sound/Other/";
        return plsFolders + key;
    }

    public static void Update()
    {
        StartRoots();

        //if half second had pass since last element reported 
        if (_timeLastReport != 0 && Time.time + 0.5f > _timeLastReport)
        {
            ExecuteReport();
        }
    }


    /// <summary>
    /// Mainly for Persons animations 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="pos"></param>
    public static void PlayOneShot(string key, float dist)
    {
        if (_audioContainers.ContainsKey(key))
        {
            _audioContainers[key].PlayAShot(dist);
        }
    }

    public static void PlayOneShot(string key, Vector3 urPos)
    {
        var dist = Vector3.Distance(Camera.main.transform.position, urPos);

        if (dist < AudioContainer.DistanceThatVolIsZeroAt)
        {
            PlayOneShot(key, dist);
        }
    }

    internal static void PlayAmbience(string playThis, Vector3 vector3)
    {
        var dist = Vector3.Distance(Camera.main.transform.position, vector3);
        if (dist < AudioContainer.DistanceThatVolIsZeroAt)
        {
            StopCurrAmbienceThatIsNotNewSound(playThis);

            if (_audioContainers[playThis].IsPlayingNow())
            {
                //do nothing
            }
            else
            {
                _audioContainers[playThis].PlayAmbience(dist);
            }
        }
    }

    private static void StopCurrAmbienceThatIsNotNewSound(string playThis)
    {
        for (int i = 0; i < _ambience.Count; i++)
        {
            var keyHere = _ambience.ElementAt(i).Key;
            if (_audioContainers[keyHere].IsPlayingNow() && keyHere != playThis)
            {
                _audioContainers[keyHere].StopAmbience();
            }
        }
    }

    internal static void SetNewSoundLevelTo(float p)
    {
        SoundLevel = p;
        OnLevelChanged(EventArgs.Empty);
    }

    internal static void SetNewMusicLevelTo(float p)
    {
        MusicLevel = p;
        MusicManager.OnLevelChanged(EventArgs.Empty);
    }




    static public EventHandler<EventArgs> LevelChanged;

    static void OnLevelChanged(EventArgs e)
    {
        if (LevelChanged != null)
        {
            LevelChanged("", e);
        }
    }


    public static void SoundTurnedOff()
    {
        //wills stop all ambiences noises 
        StopCurrAmbienceThatIsNotNewSound("");
    }

    internal static void SoundIsSwitchNow()
    {
        if (Settings.ISSoundOn)
        {

        }
        else
        {
            SoundTurnedOff();
        }
    }




#region Person Voice

    public static void PlayPersonVoice(Person p)
    {
        if (p.Gender == H.Male)
        {
            if (p.Age > 18 && p.Age < 70)
            {
                PlayPerson("Man");
            }
        }

    }


    static List<string> info = new List<string>();
    private static int infoIndex;
    static void PlayPerson(string typeOfPerson)
    {
        //the root is determined by the languages and the type of person
        //English/Man/ is an ex
        var buildRoot = "Prefab/Audio/Sound/" + Languages.CurrentLang() + "/" + typeOfPerson + "/";

        InitInfoList(typeOfPerson);

        var key = info[Random.Range(0, info.Count)];
        key = key.Substring(infoIndex, key.Length - (4 + infoIndex));

        //Debug.Log("Key: " + key);
        //Debug.Log("buildRoot: " + buildRoot);

        if (!_audioContainers.ContainsKey(key))
        {
            var audioConta = AudioContainer.Create(key, key, 0,
            container: AudioPlayer.SoundsCointaner.transform);
            _audioContainers.Add(key, audioConta);
            _languages.Add(key, buildRoot + key);
        }
        else
        {
            _audioContainers[key].PlayAShot(0);
        }
    }

    /// <summary>
    /// Be aware if new sounds were added to a Language it needs
    /// to be run at least once in EDITOR b4 works in Standalone.
    /// So in editor play the game and click a person so it talks 
    /// </summary>
    /// <param name="typeOfPerson"></param>
    static void InitInfoList(string typeOfPerson)
    {
        if (info.Count > 0)
        {
            return;
        }

        infoIndex = 0;

#if UNITY_EDITOR
        info = GetFilesInEditor(typeOfPerson);
        SaveOnProgramData(info);
#endif
#if UNITY_STANDALONE
        info = GetFilesInStandAlone(typeOfPerson);
        infoIndex = 17;//bz needs to remove 'Assets/Resources/'
#endif
    }

    private static void SaveOnProgramData(List<string> waves)
    {
        var pData = XMLSerie.ReadXMLProgram();
        pData.Voices = waves;
        XMLSerie.WriteXMLProgram(pData);
    }

    static List<string> GetFilesInStandAlone(string typeOfPerson)
    {
        var pData = XMLSerie.ReadXMLProgram();

        //needs to contain the current lang and the type of person
        var res = pData.Voices.Where(a => a.Contains(Languages.CurrentLang()) &&
            a.Contains(typeOfPerson)).ToList();

        return res;
    }

    static List<string> GetFilesInEditor(string typeOfPerson)
    {
        var root = "Assets/Resources/Prefab/Audio/Sound/" + Languages.CurrentLang() + "/"
            + typeOfPerson + "/";

        //Debug.Log("sound root:" + root);
        var waves = Directory.GetFiles(root, "*.wav").ToList();
        return waves;
    }

#endregion

}
