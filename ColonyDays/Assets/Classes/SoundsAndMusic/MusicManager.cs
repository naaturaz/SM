﻿using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public class MusicManager
{
    private static Dictionary<string, string> _musicRoots = new Dictionary<string, string>()
    {
        {"THEME 2_simple", ""},
        {"THEME 3", ""},
        {"THEME 4_simple", ""},
        {"THEME 5", ""},
        {"THEME 7_rough (1)", ""},
        {"THEME 8", ""},
        {"THEME 9", ""},
        {"THEME 10", ""},
        {"MODERN_CUBA", ""},
        {"THEME 6_new", ""},
        {"THEME 2_simple_CUBAN_BONGO", ""},
    };

    private static Dictionary<string, string> _musicCivRoots = new Dictionary<string, string>()
    {
        {"PIRATES_10%", ""},
        {"PIRATES_20%", ""},
    };

    private static List<string> _playedSongs = new List<string>();

    private static Dictionary<string, AudioContainer> _musics = new Dictionary<string, AudioContainer>();
    private static Dictionary<string, AudioContainer> _musicsCiv = new Dictionary<string, AudioContainer>();
    private static AudioContainer _currMusic;

    public static bool IsMusic(string val)
    {
        return _musicRoots.ContainsKey(val) || _musicCivRoots.ContainsKey(val);
    }

    public static void Start()
    {
        //bz gets called again when level is started
        if (_currMusic != null)
        {
            return;
        }
        //Debug.Log("MusicManager.Start()");

        PlayRandom();
    }

    private static int secCount;

    private static void PlayRandom()
    {
        //Debug.Log("MusicManager.PlayRandom()");

        _currMusic = PlayMaracasFirst();

        secCount++;

        if (secCount > 1000)
        {
            throw new Exception("Trying to reach Random music over 1000 times");
        }

        //if is contained we need a new one
        if (_playedSongs.Contains(_currMusic.Key))
        {
            PlayRandom();
            return;
        }

        secCount = 0;
        _currMusic.PlayMusicAShot();
    }

    /// <summary>
    /// Will play maracas first if has not being played once already
    /// </summary>
    /// <returns></returns>
    private static AudioContainer PlayMaracasFirst()
    {
        AudioContainer res = null;

        //never a song was played
        if (_playedSongs.Count == 0)
        {
            res = ReturnOrCreate(_musicRoots.ElementAt(5));//las maracas
        }
        else
        {
            res = ReturnOrCreate(_musicRoots.ElementAt(Random.Range(0, _musicRoots.Count)));
        }
        return res;
    }

    /// <summary>
    /// Will return if exist or will create and return
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    private static AudioContainer ReturnOrCreate(KeyValuePair<string, string> item)
    {
        if (!_musics.ContainsKey(item.Key))
        {
            LoadAMusic(item);
        }
        return _musics[item.Key];
    }

    private static void LoadAMusic(KeyValuePair<string, string> item)
    {
        var root = DefineRoot(item.Key);

        var audCont = AudioContainer.Create(item.Key, root, 0,
            container: AudioPlayer.SoundsCointaner.transform);

        LevelChanged += audCont.LevelChanged;

        _musics.Add(item.Key, audCont);
    }

    //private static void LoadMusics()
    //{
    //    for (int i = 0; i < _musicRoots.Count; i++)
    //    {
    //        var item = _musicRoots.ElementAt(i);
    //        var root = DefineRoot(item.Key);

    //        var audCont = AudioContainer.Create(_musicRoots.ElementAt(i).Key, root, 0,
    //            container: AudioPlayer.SoundsCointaner.transform);

    //        LevelChanged += audCont.LevelChanged;

    //        _musics.Add(item.Key, audCont);
    //    }

    //}

    //private static void LoadMusicCiv()
    //{
    //    for (int i = 0; i < _musicCivRoots.Count; i++)
    //    {
    //        var item = _musicCivRoots.ElementAt(i);
    //        var root = DefineRoot(item.Key);

    //        _musicsCiv.Add(item.Key, AudioContainer.Create(item.Key, root, 0,
    //            container: AudioPlayer.SoundsCointaner.transform));
    //    }
    //}

    private static string DefineRoot(string key)
    {
        var plsFolders = "Prefab/Audio/Music/";

        return plsFolders + key;
    }

    public static void MusicIsTurnedOFF()
    {
        _currMusic.Pause();
    }

    public static void MusicIsTurnedON()
    {
        _currMusic.UnPause();
    }

    internal static void PlayANewSong(string key)
    {
        _playedSongs.Add(key);

        if (_playedSongs.Count == _musicRoots.Count)
        {
            _playedSongs.Clear();
        }

        PlayRandom();
    }

    internal static void MusicIsSwitchNow()
    {
        if (Settings.ISMusicOn)
        {
            MusicIsTurnedON();
        }
        else
        {
            MusicIsTurnedOFF();
        }
    }

    static public EventHandler<EventArgs> LevelChanged;

    public static void OnLevelChanged(EventArgs e)
    {
        if (LevelChanged != null)
        {
            LevelChanged("", e);
        }
    }
}