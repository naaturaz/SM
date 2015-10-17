using UnityEngine;
using System.Collections;

public class Settings : MonoBehaviour {

    public static bool ISPAUSED;

    static bool _isSoundOn = true;
    static bool _isMusicOn = true;

    //this is the music variable use for the whole game 
    public static Music music = null;

    public static bool ISSoundOn
    {
        get { return _isSoundOn; }
        set { _isSoundOn = value; }
    }

    public static bool ISMusicOn
    {
        get { return _isMusicOn; }
        set { _isMusicOn = value; }
    }

    public static bool ISToAskB4Exit = true;

    public static Music Switch(H what, Music current = null)
    {
        if(what == H.Sound)
        {
            _isSoundOn = MecanicSwitcher(_isSoundOn);
        }
        else if(what == H.Music)
        {
            _isMusicOn = MecanicSwitcher(_isMusicOn);
            if (current != null || _isMusicOn)
            {
                current = KillOrRestart(current, _isMusicOn);
            }
        }
        return current;
    }

    static Music KillOrRestart(Music current, bool isMusicOnPass)
    {
        AudioPlayer ap = new AudioPlayer();
        if (isMusicOnPass) 
        { 
            if(Application.loadedLevelName == "Lobby")
            {
                current = (Music)ap.PlayAudio(RootSound.musicLobby, H.Music);
            }
            else current = (Music)ap.PlayAudio(RootSound.musicLvl1Start, H.Music);
        }
        else
        {   
            Destroy(current.gameObject);
        }
        return current;
    }

    public static bool MecanicSwitcher(bool currentState)
    {
        if (currentState)
        {
            currentState = false;
        }
        else
        {
            currentState = true;
        }
        return currentState;
    }

    public static void PlayMusic()
    {
        AudioPlayer ap = new AudioPlayer();
        if(Application.loadedLevelName != "Lobby")
            music = (Music)ap.PlayAudio(RootSound.musicLvl1Start, H.Music);
        else if(Application.loadedLevelName == "Lobby")
            music = (Music)ap.PlayAudio(RootSound.musicLobby, H.Music);
    }
}