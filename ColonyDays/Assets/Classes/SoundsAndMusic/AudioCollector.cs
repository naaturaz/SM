using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Classes.SoundsAndMusic;


public class AudioCollector
{





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
        {"Emigrated", ""},
        {"FallingTree", ""},
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
        //var root = "C:/GitHub/SM/ColonyDays/Assets/Resources/Prefab/Audio/Sound/Other/";

        var plsFolders = "Prefab/Audio/Sound/Other/";
        var appData = Application.dataPath;

        Debug.Log("appData path: "+ appData);
        
        return plsFolders + key;
    }

    static bool isSpawnStarted;
    public static void Update()
    {
        StartRoots();

        //if half second had pass since last element reported 
        if (_timeLastReport != 0 && Time.time + 0.5f > _timeLastReport)
        {
            ExecuteReport();
        }

        if (!isSpawnStarted && _audioContainers.Count > 0)
        {
            isSpawnStarted = true;
            Spawn();
        }
    }

    static void Spawn()
    {
        foreach (var item in _rootsToSpawn)
        {
            var root = DefineRoot(item.Key);

            _audioContainers.Add(item.Key, AudioContainer.Create(item.Key, root, 0,
              container: AudioPlayer.SoundsCointaner.transform));  
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

        if (dist < 200)
        {
            PlayOneShot(key, dist);
        }
    }
}
