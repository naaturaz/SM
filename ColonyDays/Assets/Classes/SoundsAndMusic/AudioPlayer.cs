using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour {

    private static bool isToPlayOneTimePlayed = true;

    Dictionary<So, Sound> _sounds = new Dictionary<So, Sound>();
    Dictionary<Mu, Music> _musics = new Dictionary<Mu, Music>(); 

    public AudioPlayer(){}

    // Use this for initialization
    void Start()
    {
        LoadAllAudios();
    }

    private void LoadAllAudios()
    {
        _sounds.Add(So.ClickMenuSound, (Sound)Spawn(RootSound.clickMenuSound, H.Sound));
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
