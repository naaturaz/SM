﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class AudioPlayer
{
    private static bool isToPlayOneTimePlayed = true;

    /// <summary>
    /// use to contain the sounds of all buildings
    /// </summary>
    private static Dictionary<string, Sound> _soundsLib = new Dictionary<string, Sound>();

    private static Dictionary<string, Sound> _sounds = new Dictionary<string, Sound>();
    private static Dictionary<string, Music> _musics = new Dictionary<string, Music>();

    static private General soundsCointaner;

    private Camera cam;

    public AudioPlayer()
    {
        //so AudioPlayer is not null
        cam = Camera.main;

        LoadAllAudios();
    }

    public static General SoundsCointaner
    {
        get { return soundsCointaner; }
        set { soundsCointaner = value; }
    }

    static private void LoadAllAudios()
    {
        if (_soundsLib.Count > 0) return;

        soundsCointaner = General.Create(Root.classesContainer, CamControl.CurrentCamera().transform.position,
            "SoundContainer", CamControl.CurrentCamera().transform);

        //starts the music
        MusicManager.Start();
    }

    public static void CameraWasChanged()
    {
        soundsCointaner.gameObject.transform.SetParent(CamControl.CurrentCamera().transform);
        soundsCointaner.gameObject.transform.position = CamControl.CurrentCamera().transform.position;
    }

    public static void InitSoundsLib()
    {
        if (_soundsLib.Count > 0)
        {
            return;
        }

        //LoadAllAudios();

        var root = "C:/GitHub/SM/ColonyDays/Assets/Resources/Prefab/Audio/Sound/";
        var waves = new List<string>();

#if UNITY_EDITOR
        waves = GetFilesInEditor(root);
        SaveOnProgramData(waves);
#endif
#if UNITY_STANDALONE
        waves = GetFilesInStandAlone();
#endif

        Debug.Log("waves.count :" + waves.Count);

        foreach (var item in waves)
        {
            var newName = ChangeCasingRemoveUnderscores(item);
            _soundsLib.Add(newName, CreatePrefabAndAddAudioSource(item, root));
        }
    }

    private static void SaveOnProgramData(List<string> waves)
    {
        var pData = XMLSerie.ReadXMLProgram();
        pData.Waves = waves;
        XMLSerie.WriteXMLProgram(pData);
    }

    private static List<string> GetFilesInStandAlone()
    {
        var pData = XMLSerie.ReadXMLProgram();
        return pData.Waves;
    }

    private static List<string> GetFilesInEditor(string root)
    {
        //Debug.Log("sound root:" + root);
        var waves = Directory.GetFiles(root, "*.wav").ToList();
        return waves;
    }

    /// <summary>
    /// This is needed so later when a building is looking for its key will be easy to find a string.contain
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    private static string ChangeCasingRemoveUnderscores(string item)
    {
        item = item.ToLower();
        var splt = item.Split('_');
        item = "";

        //removing all underscores
        for (int i = 0; i < splt.Length; i++)
        {
            item += splt[i];
        }

        return item;
    }

    private static Sound CreatePrefabAndAddAudioSource(string audioSourceRoot, string root)
    {
        var sound = (Sound)General.Create("Prefab/Audio/Sound/Template",
            Camera.main.transform.position, container: soundsCointaner.transform);
        audioSourceRoot = CleanTheFileNameThenAddPrefabRoot(audioSourceRoot, root);
        sound.name = audioSourceRoot;

        AudioSource audioSource = sound.gameObject.GetComponent<AudioSource>();
        var clip = Resources.Load(audioSourceRoot) as AudioClip;
        audioSource.clip = clip;
        //Play sound on creation
        //audioSource.Play();

        return sound;
    }

    private static string CleanTheFileNameThenAddPrefabRoot(string audioSourceRoot, string root)
    {
        //removing the first part
        audioSourceRoot = audioSourceRoot.Substring(root.Length);
        //remove the extension of the file
        audioSourceRoot = audioSourceRoot.Remove(audioSourceRoot.Length - 4);
        //add the prefab root
        audioSourceRoot = "Prefab/Audio/Sound/" + audioSourceRoot;
        return audioSourceRoot;
    }

    public static void PlayThisSound1Time(string hTypeP, string currProd)
    {
        InitSoundsLib();

        if (!Settings.ISSoundOn)
        {
            return;
        }

        var type = TreatType(hTypeP, currProd);
        if (_sounds.ContainsKey(type))
        {
            //play sound
            //Debug.Log("play sound " + type);
            _sounds[type].Play();
        }
        else
        {
            //if was found then play it
            if (FindItsSoundAndDefineKey(type))
            {
                PlayThisSound1Time(type, currProd);
            }
        }
    }

    private static string TreatType(string hType, string currProd)
    {
        var res = ChangeCasingRemoveUnderscores(hType + "");

        if (res.Contains("house"))
        {
            return "house";
        }
        if (res.Contains("fieldfarm"))
        {
            return "fieldfarm";
        }
        if (res.Contains("animalfarm"))
        {
            //for animalFarmPigs
            return "animalfarm" + currProd.ToLower();
        }
        return res;
    }

    /// <summary>
    /// will fidn change the casing in the keyP
    /// will try to find a containing one
    /// </summary>
    /// <param name="p">Retrurn true if was defined </param>
    private static bool FindItsSoundAndDefineKey(string keyP)
    {
        var treatedKey = ChangeCasingRemoveUnderscores(keyP);
        for (int i = 0; i < _soundsLib.Count; i++)
        {
            if (_soundsLib.ElementAt(i).Key.Contains(treatedKey))
            {
                _sounds.Add(keyP, _soundsLib.ElementAt(i).Value);
                _soundsLib.Remove(_soundsLib.ElementAt(i).Key);
                return true;
            }
        }
        return false;
    }

    public static Audio PlayAudio(string soundToPlayRoot, H musicOrSound, Vector3 iniPos = new Vector3(), bool playOneTime = false)
    {
        Audio temp = null;
        if (iniPos == Vector3.zero && Camera.main != null)
        {
            iniPos = Camera.main.transform.position;
        }

        if (musicOrSound == H.Sound && Settings.ISSoundOn)
        {
            temp = (Sound)General.Create(soundToPlayRoot, iniPos);
        }
        else if (musicOrSound == H.Music && Settings.ISMusicOn)
        {
            //wont play music now
            return null;
            temp = (Music)General.Create(soundToPlayRoot, iniPos);
        }
        return temp;
    }
}