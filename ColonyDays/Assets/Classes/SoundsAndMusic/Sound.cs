using UnityEngine;
using System.Collections;

public class Sound : Audio {

    public int autoDestroyInSec = 3;
    public bool isAutoDestroy = true;
    private AudioSource _audioSource;


    float creationTime;

	// Use this for initialization
	void Start ()
	{
	    _audioSource = gameObject.GetComponent<AudioSource>();
        creationTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Time.time > creationTime + autoDestroyInSec && isAutoDestroy)
        {
            Destroy();
        }

        base.Update();
	}

    internal void Play()
    {
        _audioSource.volume = AudioCollector.SoundLevel;
        _audioSource.Play();
    }
}
