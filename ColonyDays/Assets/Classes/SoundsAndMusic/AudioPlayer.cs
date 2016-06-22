using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class AudioPlayer : MonoBehaviour {

    private static bool isToPlayOneTimePlayed = true;

    static Dictionary<string, Sound> _soundsLib = new Dictionary<string, Sound>();
    static Dictionary<string, Sound> _sounds = new Dictionary<string, Sound>();
    static Dictionary<string, Music> _musics = new Dictionary<string, Music>();

    static private General soundsCointaner;

    public AudioPlayer()
    {
        LoadAllAudios();
        
    }

    // Use this for initialization
    void Start()
    {
    }

    static private void LoadAllAudios()
    {
        return;


        if (_soundsLib.Count > 0)
        {
            return;
        }

        soundsCointaner = General.Create(Root.classesContainer, Camera.main.transform.position,
            "SoundContainer", Camera.main.transform);
        var root = Application.dataPath + "/Resources/Prefab/Audio/Sound/";
        //Debug.Log("r:"+ root );
        var waves = Directory.GetFiles(root, "*.wav").ToList();



        foreach (var item in waves)
        {
            var newName = ChangeCasingRemoveUnderscores(item);
            _soundsLib.Add(newName, CreatePrefabAndAddAudioSource(item, root));
        }
    }

    /// <summary>
    /// This is needed so later when a building is looking for its key will be easy to find a string.contain
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    static string ChangeCasingRemoveUnderscores(string item)
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

    static Sound CreatePrefabAndAddAudioSource(string audioSourceRoot, string root)
    {
        var sound = (Sound)General.Create("Prefab/Audio/Sound/Template", Camera.main.transform.position, container: soundsCointaner.transform);
        audioSourceRoot = CleanTheFileNameThenAddPrefabRoot(audioSourceRoot, root);
        sound.name = audioSourceRoot;

        AudioSource audioSource = sound.gameObject.GetComponent<AudioSource>();
        var clip = Resources.Load(audioSourceRoot) as AudioClip;
        audioSource.clip = clip;
        audioSource.Play();

        return sound;
    }

    static string CleanTheFileNameThenAddPrefabRoot(string audioSourceRoot, string root)
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
        var type = TreatType(hTypeP, currProd);
        if (_sounds.ContainsKey(type))
        {
            //play sound
            Debug.Log("play sound " + type);
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

    static string TreatType(string hType, string currProd)
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
    

       





    // Update is called once per frame
    void Update()
    {

    }


    static Audio Spawn(string soundToPlayRoot, H musicOrSound)
    {
        Audio temp = null;
        if (musicOrSound == H.Sound)
        {
            temp = (Sound)General.Create(soundToPlayRoot, Camera.main.transform.position, container: Camera.main.transform);
        }
        else if (musicOrSound == H.Music)
        {
            temp = (Music)General.Create(soundToPlayRoot, Camera.main.transform.position, container: Camera.main.transform);
        }
        return temp;
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

    public static Sound PlaySoundOneTime(string soundToPlayRoot, H musicOrSound, Vector3 iniPos = new Vector3(), bool reset = false)
    {
        return null;

        Sound temp = null;
        if (Settings.ISSoundOn && musicOrSound == H.Sound)
        {
            if (!reset)
            {
                if (iniPos == Vector3.zero)
                {
                    iniPos = Camera.main.transform.position;
                }
                if (!isToPlayOneTimePlayed)
                {
                    temp = (Sound)General.Create(soundToPlayRoot, iniPos);
                    isToPlayOneTimePlayed = true;
                }
            }
            else if (reset && isToPlayOneTimePlayed)
            {
                isToPlayOneTimePlayed = false;
            }
        }
        return temp;
    }

    public static Sound PlaySoundOneTime(string soundToPlayRoot, Vector3 iniPos = new Vector3(), Transform container = null)
    {
        return null;

        CamControl mainCam = USearch.FindCurrentCamera();
        Sound temp = null;
        if (Settings.ISSoundOn)
        {
            if (iniPos == Vector3.zero)
            {
                iniPos = mainCam.transform.GetComponent<Camera>().transform.position;
            }
            temp = (Sound)General.Create(soundToPlayRoot, iniPos);
        }

        if (container == null)
        {
            temp.transform.parent = mainCam.transform.GetComponent<Camera>().transform;
        }
        else temp.transform.parent = container;

        return temp;
    }

  


}
