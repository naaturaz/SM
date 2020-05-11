using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class RewardsWindow : GUIElement
{
    private bool _hasSeenRewards;
    private bool _isGoodPlayer;

    //assigned on Inspector
    public Text PlayedTime;

    public Text TargetTime;
    public Text TimeLeft;
    public GameObject Qualified;
    public InputField Email;
    public InputField EmailConfirm;
    public Text Display;

    private float _targetTime = 120;//60*60*2
    private float _playedPreviously;

    private DateTime _today = new DateTime();
    private int _week;

    // Use this for initialization
    private void Start()
    {
        _week = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear
    (DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

        if (PlayerPrefs.GetInt("Week") == _week)
        {
            Hide();
            return;
        }

        iniPos = transform.position;
        _today = DateTime.Today;

        _isGoodPlayer = PlayerPrefs.GetInt("Rate") > 0;
        _hasSeenRewards = PlayerPrefs.GetInt("Reward") > 0;
        _playedPreviously = PlayerPrefs.GetInt("Played");

        if (!_isGoodPlayer && !_hasSeenRewards)
        {
            Hide();
            return;
        }

        //bz this can change if he submits a negative OptionalFeedback
        if (_isGoodPlayer)
        {
            //so even if he submits negative feeedback he will continue to see Rewards window
            PlayerPrefs.SetInt("Reward", 1);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        TimeSpan t = new TimeSpan(ReturnTicks(Time.time + _playedPreviously));

        TimeSpan tar = new TimeSpan(ReturnTicks(_targetTime));

        TimeSpan left = new TimeSpan(ReturnTicks(_targetTime - Time.time));

        PlayedTime.text = t.Hours + ":" + AddZeroIfLess10(t.Minutes) + "." + AddZeroIfLess10(t.Seconds);
        TargetTime.text = tar.Hours + ":" + AddZeroIfLess10(tar.Minutes) + "." + AddZeroIfLess10(tar.Seconds);
        TimeLeft.text = left.Hours + ":" + AddZeroIfLess10(left.Minutes) + "." + AddZeroIfLess10(left.Seconds);

        CheckIfReward();
    }

    private void CheckIfReward()
    {
        if (Time.time + _playedPreviously > _targetTime)
        {
            Qualified.SetActive(true);
            if (Display.text == "")
            {
                Display.text = "If you haven't this week draw, you may enter now!!";
            }
        }
    }

    private long ReturnTicks(float seconds)
    {
        return 10000000 * long.Parse(Math.Truncate(seconds).ToString());
    }

    private string AddZeroIfLess10(int pass)
    {
        if (pass > 9)
        {
            return pass + "";
        }
        if (pass < 0)
        {
            return "00";
        }

        return "0" + pass;
    }

    private void OnApplicationQuit()
    {
        if (Program.WeekDraw)
        {
            return;
        }

        PlayerPrefs.SetInt("Played", int.Parse(Math.Truncate(Time.time).ToString()));
    }

    /// <summary>
    /// Called from GUI
    /// </summary>
    public void EnterDraw()
    {
        if (Email.text != EmailConfirm.text)
        {
            Display.text = "Emails are not the same";
            return;
        }
        else if (!DialogGO.IsValidEmail(Email.text))
        {
            Display.text = "Invalid email";
            return;
        }
        Display.text = "This week you are in! Only 1 application per week is needed";
        SendIt();

        ResetPlayer();
        Program.WeekDraw = true;

        PlayerPrefs.SetInt("Week", _week);
        Debug.Log("Week " + _week);

        Qualified.SetActive(false);
    }

    private void ResetPlayer()
    {
        PlayerPrefs.SetInt("Played", 0);
    }

    private void SendIt()
    {
        try
        {
            Dialog.CreateFile("Draw", EmailConfirm.text);
            print("mail Send");
        }
        catch (Exception ex)
        {
            Dialog.OKDialog(H.Info, "Something was incorrect");
            print(ex.ToString());
        }
    }
}