using UnityEngine;

public class Profile : MonoBehaviour
{
    private SaveLoad saveLoad = new SaveLoad();

    private float creationTime;
    private float _timePlayed;
    private string _userName;

    private bool _isSoundOn;
    private bool _isMusicOn;
    private bool _isToAskB4Exit;

    private int _playerLevel;
    private int _playerXP;

    private H _difficulty;

    public float TimePlayed
    {
        get { return _timePlayed; }
        set { _timePlayed = value; }
    }

    public string UserName
    {
        get { return _userName; }
        set { _userName = value; }
    }

    public bool IsSoundOn
    {
        get { return _isSoundOn; }
        set { _isSoundOn = value; }
    }

    public bool IsMusicOn
    {
        get { return _isMusicOn; }
        set { _isMusicOn = value; }
    }

    public bool IsToAskB4Exit
    {
        get { return _isToAskB4Exit; }
        set { _isToAskB4Exit = value; }
    }

    public int PlayerLevel
    {
        get { return _playerLevel; }
        set { _playerLevel = value; }
    }

    public int PlayerXP
    {
        get { return _playerXP; }
        set { _playerXP = value; }
    }

    public H Difficulty
    {
        get { return _difficulty; }
        set { _difficulty = value; }
    }

    // Use this for initialization
    private void Start()
    {
        creationTime = Time.time;
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void LoadProfile(string currentUser)
    {
        saveLoad.PlayerPrefLoad(currentUser);
        //print("TimePlayed Load:" + TimePlayed);
    }

    public void UpdateProfile(string currentUser)
    {
        float timeElapsedSinceLastUpdate = 0;
        FindTimePlayed(out creationTime, out timeElapsedSinceLastUpdate);
        TimePlayed += timeElapsedSinceLastUpdate;

        int xpElapsed = 0;
        Program.THEPlayer.PlayerXP = FetchReset((Program.THEPlayer.PlayerXP), out xpElapsed);
        PlayerXP += xpElapsed;

        saveLoad.PlayerPrefSave(currentUser);
        //print("TimePlayed:" + TimePlayed);
    }

    private void FindTimePlayed(out float creationTime, out float timeElapsedSinceLastUpdate)
    {
        creationTime = this.creationTime;
        timeElapsedSinceLastUpdate = Time.time - creationTime;
        creationTime = Time.time;
    }

    //FindOutElapseAmountThenReset
    private double FetchReset(double propertyInPlayer, out double elapsedAmount)
    {
        elapsedAmount = propertyInPlayer;
        propertyInPlayer = 0;
        return propertyInPlayer;
    }

    //FindOutElapseAmountThenReset
    private int FetchReset(int propertyInPlayer, out int elapsedAmount)
    {
        elapsedAmount = propertyInPlayer;
        propertyInPlayer = 0;
        return propertyInPlayer;
    }
}