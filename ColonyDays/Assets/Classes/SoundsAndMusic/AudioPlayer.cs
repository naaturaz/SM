using UnityEngine;

public class AudioPlayer : MonoBehaviour {

    private bool isToPlayOneTimePlayed = true;

    public AudioPlayer(){}

    public Audio PlayAudio(string soundToPlayRoot, H musicOrSound, Vector3 iniPos = new Vector3(), bool playOneTime = false)
    {
        Audio temp = null;
        if (iniPos == Vector3.zero)
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

    public Sound PlaySoundOneTime(string soundToPlayRoot, H musicOrSound, Vector3 iniPos = new Vector3(), bool reset = false)
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

    public Sound PlaySoundOneTime(string soundToPlayRoot, Vector3 iniPos = new Vector3(), Transform container = null)
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

  

	// Use this for initialization
	void Start ()
    {}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
