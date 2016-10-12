using System;
using UnityEngine;

/// <summary>
/// This class along wiht AudioPlayer are the only ones that play audio sounds
/// </summary>
public class AudioContainer: MonoBehaviour
{
    private string _key;
    private string _root;
    private float _newLevel;//newLevel this audio is trying to reach as the value pass from report
    //is a average of distances 
    private AudioSource _audioSource;



    private bool volUp;//volume needs to go up 
    private bool volDown;//down
    private float target;//target vol

    private float speed = 0.1f;

    private float _lastReport;

    private AudioClip _audioClip;

    public string Root1
    {
        get { return _root; }
        set { _root = value; }
    }

    public string Key
    {
        get { return _key; }
        set { _key = value; }
    }

    public float NewLevel
    {
        get { return _newLevel; }
        set { _newLevel = value; }
    }


    /// <summary>
    /// At this distance the Volume will be zero or passed this
    /// </summary>
    static int _distanceThatVolIsZeroAt = 30;//200
    /// <summary>
    /// At this distance the Volume will be zero or passed this
    /// </summary>
    public static int DistanceThatVolIsZeroAt
    {
        get { return _distanceThatVolIsZeroAt; }
        set { _distanceThatVolIsZeroAt = value; }
    }

    static public AudioContainer Create(string key, string root, float newLevel,
        Vector3 origen = new Vector3(), Transform container = null)
    {
        AudioContainer obj = null;
        obj = (AudioContainer)Resources.Load("Prefab/Audio/Sound/TemplateAudioContainer",
            typeof(AudioContainer));
        obj = (AudioContainer)Instantiate(obj, origen, Quaternion.identity);
        obj.transform.name = "AudioContaner: "+key;

        obj.Key = key;
        obj.Root1 = root;
        obj.NewLevel = newLevel;

        if (container != null) { obj.transform.parent = container; }
        return obj;
    }

  

    void Start()
    {
        Debug.Log("newAudioContainer: "+Root1);
        AddSpecificAudioSource();

        if (IsAmbience())
        {
            return;
        }
        
        Play(_newLevel);
    }

    private void AddSpecificAudioSource()
    {
        //todo
        _audioSource = gameObject.GetComponent<AudioSource>();

        _audioClip = Resources.Load(Root1) as AudioClip;
        _audioSource.clip = _audioClip;
        _audioSource.volume = 0;
    }


    internal void Play(float newLevel)
    {
        if (!Settings.ISSoundOn)
        {
            return;
        }

        FadesTo(newLevel);
        _lastReport = Time.time;
    }

    public void Stop()
    {
        FadesTo(_distanceThatVolIsZeroAt);//1000 is zero
    }

    void FadesTo(float newDist)
    {
        var realVal = ConvertLevel(newDist);
      
        if (_audioSource.volume > realVal)
        {
            volDown = true;
        }
        else if (_audioSource.volume < realVal)
        {
            volUp = true;

            //if was below or zero I will play it 
            if (_audioSource.volume <= 0 || !_audioSource.isPlaying)
            {
                PlayAudioSource();
            }
        }
        target = realVal;
    }

    /// <summary>
    /// Wont function with person sounds
    /// </summary>
    void StopAudioSource()
    {
        if (IsThisAPersonSound() || IsASpawnSound() || IsAMusic() || IsLanguage()
            //|| IsAmbience()
            )
        {
            return;
        }
        _audioSource.volume = 0;
        _audioSource.Stop();
    }

    /// <summary>
    /// Wont function with person sounds
    /// </summary>
    void PlayAudioSource()
    {
        if (!Settings.ISSoundOn)
        {
            return;
        }

        if (IsThisAPersonSound() || IsASpawnSound() || IsAMusic()
            //|| IsAmbience()
            )
        {
            return;
        }

        _audioSource.Play();
    }



    /// <summary>
    /// bz the newLevel is a Distance report it has to be removed from 1000
    /// bz the bigger is the lower should the Volumen be
    /// 
    /// Then divide by 1000 so is ready for AudioSource Volume (0-1f)
    /// </summary>
    /// <param name="newVal"></param>
    /// <returns></returns>
    float ConvertLevel(float newVal)
    {
        var newReal = _distanceThatVolIsZeroAt - newVal;
        return newReal / _distanceThatVolIsZeroAt;//so is ready for AudioSource Volume (0-1f)
    }

    private float ConvertVolToDist(float p)
    {
        var dist = (1 - p) * _distanceThatVolIsZeroAt;
        return dist;
    }


    void Update()
    {
        if (coolDownUntil > 0 && Time.time > coolDownUntil)
        {
            PlayMusicAShot();
        }

        //3 seconds before finishes 
        if (Time.time > timeToPlayNextSong - 3 && timeToPlayNextSong > 0)
        {
            //todo play another music 
            timeToPlayNextSong = 0;//so it doesnt keep trying to play new songs 
            MusicManager.PlayANewSong(_key);
        }

        //if is not changign the vol and time has passed since las report and is playing
        if (!IsThisAPersonSound() && 
            !IsASpawnSound() && !IsAMusic() && !IsAmbience() &&
            !IsLanguage() &&
            _audioSource.isPlaying && !volUp && !volDown 
            && Time.time + 2.6f > _lastReport)
        {
            Stop();
        }
        UpAndDown();

        if (speedJustChanged)
        {
            speedJustChanged = false;
        }
    }

    private bool IsLanguage()
    {
        return AudioCollector.Languages1.ContainsKey(_key);
    }

    private bool IsASpawnSound()
    {
        return AudioCollector.RootsToSpawn.ContainsKey(_key);
    }

    bool IsThisAPersonSound()
    {
        return AudioCollector.PersonRoots.ContainsKey(_key);
    }   
    
    bool IsAMusic()
    {
        return MusicManager.IsMusic(_key);
    }

    bool IsAmbience()
    {
        return AudioCollector.Ambience.ContainsKey(_key);
    }

    private static bool speedJustChanged;
    public static void SpeedChanged()
    {
        speedJustChanged = true;
    }

    void UpAndDown()
    {
        if (volUp)
        {
            //still less 
            if (_audioSource.volume < target)
            {
                _audioSource.volume += speed;
            }
            else
            {
                volUp = false;
                target = -1;
            }
        }
        else if (volDown)
        {
            //still more 
            if (_audioSource.volume > target)
            {
                _audioSource.volume -= speed;
            }
            else
            {
                if (target == 0)
                {
                    StopAudioSource();
                }

                volDown = false;
                target = -1;
            }
        }
    }

    internal void PlayAmbience(float dist)
    {
        var volHere = ConvertLevel(dist);
        _audioSource.volume = volHere * AudioCollector.SoundLevel; 
        _audioSource.loop = true;
        PlayAShot(dist);
       // FadesTo(dist);
    }

    private float lastShotPlayed;
    /// <summary>
    /// Main created for people play animtaions sounds
    /// </summary>
    internal void PlayAShot(float dist)
    {
        if (!Settings.ISSoundOn)
        {
            return;
        }

        //so 12 wheelBarrowers dont sound aweful
        //we need at least 0.15f sec since last played 
        if (lastShotPlayed + .11f > Time.time)
        {
            return;
        }

        var volHere = ConvertLevel(dist);
        _audioSource.PlayOneShot(_audioClip, volHere * AudioCollector.SoundLevel);

        lastShotPlayed = Time.time;
    }

#region Music

    private float timeToPlayNextSong;
    private float coolDownUntil;//used when play was hit before Start() happened
    /// <summary>
    /// Created to play music a shot 
    /// </summary>
    /// <param name="dist"></param>
    internal void PlayMusicAShot()
    {
        if (!Settings.ISMusicOn)
        {
            return;
        }

        //means is being played before it had time to Start()
        if (_audioSource == null || _audioClip == null)
        {
            coolDownUntil = Time.time + 3;
            return;
        }
        coolDownUntil = 0;
        
        _audioSource.PlayOneShot(_audioClip, .15f * AudioCollector.MusicLevel);
        timeToPlayNextSong = Time.time + _audioClip.length;
    }

    private bool wasPaused;
    internal void Pause()
    {
        if (_audioSource.isPlaying)
        {
            _audioSource.Pause();
            wasPaused = true;
        }
    }   
    
    internal void UnPause()
    {
        if (wasPaused)
        {
            _audioSource.UnPause();
            wasPaused = false;
        }
        else
        {
            PlayMusicAShot();
        }
    }


#endregion


    public void LevelChanged(object sender, EventArgs e)
    {
        _audioSource.volume = AudioCollector.SoundLevel;

        if (IsAMusic())
        {
            _audioSource.volume = AudioCollector.MusicLevel;
        }
        //Debug.Log("Sound Event :"+_key);
    }



    internal bool IsPlayingNow()
    {
        return _audioSource.isPlaying;
    }

    internal void StopAmbience()
    {
        speed = 0.01f;
        Stop();
    }
}
