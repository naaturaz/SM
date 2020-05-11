using UnityEngine;

public class Sound : Audio
{
    public int autoDestroyInSec = 3;
    public bool isAutoDestroy = true;
    private AudioSource _audioSource;

    private float creationTime;

    // Use this for initialization
    private void Start()
    {
        _audioSource = gameObject.GetComponent<AudioSource>();
        creationTime = Time.time;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Time.time > creationTime + autoDestroyInSec && isAutoDestroy)
        {
            Destroy();
        }

        base.Update();
    }

    internal void Play()
    {
        if (_audioSource == null)
        {
            Start();
        }

        _audioSource.volume = AudioCollector.SoundLevel;
        _audioSource.Play();
    }
}