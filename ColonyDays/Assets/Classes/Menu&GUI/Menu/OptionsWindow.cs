using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class OptionsWindow : GUIElement
{
    private Toggle _fullToggle;
    private Toggle _dayToggle;
    private StageManager _stageManager;

    private Toggle _musicToggle;
    private Toggle _soundToggle;

    private Toggle _babyBornToggle;
    private Toggle _buildCompletedToggle;
    private Toggle _citizensVoiceToggle;

    private Toggle _halloToggle;
    private Toggle _xmasToggle;

    private GameObject _autoSaveBtn;
    private GameObject _unitBtn;
    private GameObject _langBtn;

    private GameObject _resBtn;
    private GameObject _qualityBtn;

    private Text _autoSaveBtnTxt;
    private LangUpdateScript _unitBtnLang;
    private Text _langBtnTxt;

    private Text _resBtnTxt;
    private LangUpdateScript _qualityBtnLang;

    private Slider _musicSlider;
    private Slider _soundSlider;
    private Slider _cameraSlider;

    private void Start()
    {
        IniPos = transform.position;

        _fullToggle = GetGrandChildCalled("FullScreen_Toggle").GetComponent<Toggle>();
        _dayToggle = GetGrandChildCalled("DayCycle_Toggle").GetComponent<Toggle>();

        _musicToggle = GetGrandChildCalled("Music_Toggle").GetComponent<Toggle>();
        _soundToggle = GetGrandChildCalled("Sound_Toggle").GetComponent<Toggle>();

        _babyBornToggle = GetGrandChildCalled("Baby_Born_Toggle").GetComponent<Toggle>();
        _buildCompletedToggle = GetGrandChildCalled("Building_Completed_Toggle").GetComponent<Toggle>();
        _citizensVoiceToggle = GetGrandChildCalled("Citizens_Voice_Toggle").GetComponent<Toggle>();

        _halloToggle = GetGrandChildCalled("Halloween_Toggle").GetComponent<Toggle>();
        _xmasToggle = GetGrandChildCalled("Xmas_Toggle").GetComponent<Toggle>();

        _musicSlider = GetGrandChildCalled("Music_Slider").GetComponent<Slider>();
        _soundSlider = GetGrandChildCalled("Sound_Slider").GetComponent<Slider>();
        _cameraSlider = GetGrandChildCalled("Camera_Slider").GetComponent<Slider>();

        var autoSavePanel = GetGrandChildCalled("Panel_AutoSave");
        _autoSaveBtn = GetChildCalledOnThis("AutoSave_Btn", autoSavePanel);
        _autoSaveBtnTxt = GetChildCalledOnThis("Text", _autoSaveBtn).GetComponent<Text>();

        var langPanel = GetGrandChildCalled("Panel_Lang");
        _langBtn = GetChildCalledOnThis("Lang_Btn", langPanel);
        _langBtnTxt = GetChildCalledOnThis("Text", _langBtn).GetComponent<Text>();

        var unitPanel = GetGrandChildCalled("Panel_Unit");
        _unitBtn = GetChildCalledOnThis("Unit_Btn", unitPanel);
        _unitBtnLang = GetChildCalledOnThis("Text", _unitBtn).GetComponent<LangUpdateScript>();

        var resPanel = GetGrandChildCalled("Panel_Res");
        _resBtn = GetChildCalledOnThis("Res_Btn", resPanel);
        _resBtnTxt = GetChildCalledOnThis("Text", _resBtn).GetComponent<Text>();

        var qualityPanel = GetGrandChildCalled("Panel_Quality");
        _qualityBtn = GetChildCalledOnThis("Quality_Btn", qualityPanel);
        _qualityBtnLang = GetChildCalledOnThis("Text", _qualityBtn).GetComponent<LangUpdateScript>();

        SetAllControls();
        RefreshAllDropDowns();

        Hide();

        //means was hidden by a Res Change
        if (resTimeChanged != 0)
        {
            resTimeChanged = 0;
            Show();
        }

        //LoadSlidersValues();
    }

    private void SetAllControls()
    {
        _fullToggle.isOn = Screen.fullScreen;
        _dayToggle.isOn = Settings.ISDay;

        _musicToggle.isOn = Settings.ISMusicOn;
        _soundToggle.isOn = Settings.ISSoundOn;

        //the very first time will be on, and if not touch will be on
        _babyBornToggle.isOn = PlayerPrefs.GetInt("BabyBorn") > -1;
        _buildCompletedToggle.isOn = PlayerPrefs.GetInt("Built") > -1;
        _citizensVoiceToggle.isOn = PlayerPrefs.GetInt("Voice") > -1;

        _halloToggle.isOn = Settings.IsHalloweenTheme;
        _xmasToggle.isOn = Settings.IsXmas;

        //so they dont trigger event
        _fullToggle.onValueChanged.AddListener((value) => Program.MouseClickListenerSt("MainMenu.Options.Full"));
        _dayToggle.onValueChanged.AddListener((value) => Program.MouseClickListenerSt("MainMenu.Options.Day"));

        _musicToggle.onValueChanged.AddListener((value) => Program.MouseClickListenerSt("MainMenu.Options.Music"));
        _soundToggle.onValueChanged.AddListener((value) => Program.MouseClickListenerSt("MainMenu.Options.Sound"));

        _babyBornToggle.onValueChanged.AddListener((value) => Program.MouseClickListenerSt("MainMenu.Options.BabyBorn"));
        _buildCompletedToggle.onValueChanged.AddListener((value) => Program.MouseClickListenerSt("MainMenu.Options.Built"));
        _citizensVoiceToggle.onValueChanged.AddListener((value) => Program.MouseClickListenerSt("MainMenu.Options.Voice"));

        _halloToggle.onValueChanged.AddListener((value) => Program.MouseClickListenerSt("MainMenu.Options.Hallo"));
        _xmasToggle.onValueChanged.AddListener((value) => Program.MouseClickListenerSt("MainMenu.Options.Xmas"));
    }

    public void RefreshAllDropDowns()
    {
        _autoSaveBtnTxt.text = (Settings.AutoSaveFrec / 60) + " min";
        _unitBtnLang.SetKey("Imperial");
        if (Unit.Units == 'm')
        {
            _unitBtnLang.SetKey("Metric");
        }
        _resBtnTxt.text = CurrentResolution();
        _langBtnTxt.text = Languages.CurrentLang();

        var names = QualitySettings.names;
        var curr = QualitySettings.GetQualityLevel();
        for (int i = 0; i < names.Length; i++)
        {
            if (i == curr)
            {
                _qualityBtnLang.SetKey(names[i]);
            }
        }
    }

    /// <summary>
    /// bz if is not full screen then is not accurate since will return the size
    /// of the screen and not the window
    /// </summary>
    /// <returns></returns>
    private string CurrentResolution()
    {
        if (Screen.fullScreen)
        {
            return Screen.currentResolution.ToString();
        }
        var splt = Screen.currentResolution.ToString().Split(' ');
        return Screen.width + " x " + Screen.height + " @ " + splt[4];//the Hz
    }

    private bool resChanged;
    private static float resTimeChanged;

    private void Update()
    {
        if (resChanged && Time.time > resTimeChanged + .5f)
        {
            resChanged = false;
            Program.MouseListener.ApplyChangeScreenResolution();
        }
    }

    /// <summary>
    /// So it leaves a  while(.5s) before can redo MainGUI and MainMenu
    ///
    /// is called too when switched to Full Screen or back
    ///
    /// is called too when switch from Imperial to Metric and vice versa
    /// </summary>
    public void ChangeResNow()
    {
        resChanged = true;
        resTimeChanged = Time.time;
    }

    private string HandleAction(string sub)
    {
        if (sub.Length > 8)
        {
            return sub.Substring(8);
        }
        return sub;
    }

    internal void Listen(string sub)
    {
        ReFreshDropsThenShow();
        sub = HandleAction(sub);

        if (sub == "OKBtn")
        {
            Hide();
        }
        else if (sub == "CancelBtn")
        {
            Hide();
        }
        //audio
        else if (sub == "Music" || sub == "Sound")
        {
            ChangeAudioSettings(sub);
        }
        //
        else if (sub == "BabyBorn")
        {
            SetPlayerPrefInt(sub, _babyBornToggle.isOn);
        }
        else if (sub == "Built")
        {
            SetPlayerPrefInt(sub, _buildCompletedToggle.isOn);
        }
        else if (sub == "Voice")
        {
            SetPlayerPrefInt(sub, _citizensVoiceToggle.isOn);
        }
        else if (sub == "Hallo")
        {
            Settings.ToggleHalloween();
        }
        else if (sub == "Xmas")
        {
            Settings.ToggleXmas();
        }
        //screen
        else if (sub == "Full")
        {
            Screen.fullScreen = Settings.MecanicSwitcher(Screen.fullScreen);
            ChangeResNow();
        }
        else if (sub == "Day")
        {
            SetPlayerPrefInt(sub, _dayToggle.isOn);
            Settings.ISDay = _dayToggle.isOn;

            if (_stageManager == null)
                _stageManager = FindObjectOfType<StageManager>();
            _stageManager.OptionsDayCycleWasToggled();

            Debug.Log("Day");
        }
    }

    private void SetPlayerPrefInt(string which, bool onOrOff)
    {
        if (onOrOff)
        {
            PlayerPrefs.SetInt(which, 1);
        }
        else
        {
            PlayerPrefs.SetInt(which, -1);
        }
    }

    public void ReFreshDropsThenShow()
    {
        RefreshAllDropDowns();
        Show();
        LoadSlidersValues();
    }

    public void ChangeAudioSettings(string typeP)
    {
        if (typeP == "Sound")
        {
            Settings.Switch(H.Sound);
        }
        else if (typeP == "Music")
        {
            Settings.music = Settings.Switch(H.Music, Settings.music);
        }
    }

    #region DropDowns

    private List<GameObject> _buttonsName = new List<GameObject>();

    public void ClickQualityDropDown()
    {
        SetButtonsList(_qualityBtn);
        string[] names = QualitySettings.names;

        for (int i = 0; i < _buttonsName.Count; i++)
        {
            if (i < names.Length)
            {
                SetEachButton(_buttonsName[i], names[i], "Quality");
            }
            else
            {
                //so dont show empty btns in the drop down
                Destroy(_buttonsName[i]);
            }
        }
    }

    public void ClickResDropDown()
    {
        SetButtonsList(_resBtn);
        var names = Screen.resolutions.ToList();
        names.Reverse();

        for (int i = 0; i < _buttonsName.Count; i++)
        {
            if (i < names.Count)
            {
                SetEachButton(_buttonsName[i], names[i].ToString(), "Res");
            }
            else
            {
                //so dont show empty btns in the drop down
                Destroy(_buttonsName[i]);
            }
        }
    }

    public void ClickLanguagesDropDown()
    {
        SetButtonsList(_langBtn);
        List<string> names = new List<string>()
        {
            "English",
            "Español(Beta)",
            //"Français(Beta)",
            "Deutsch",
            //"Português(Beta)"
        };

        for (int i = 0; i < _buttonsName.Count; i++)
        {
            if (i < names.Count)
                SetEachButton(_buttonsName[i], names[i].ToString(), "Lang");
            else
                //so dont show empty btns in the drop down
                Destroy(_buttonsName[i]);
        }
    }

    public void ClickAutoSaveDropDown()
    {
        SetButtonsList(_autoSaveBtn);
        List<string> names = new List<string>() { "5 min", "10 min", "15 min", "20 min" };

        for (int i = 0; i < _buttonsName.Count; i++)
        {
            if (i < names.Count)
            {
                SetEachButton(_buttonsName[i], names[i].ToString(), "AutoSave");
            }
            else
            {
                //so dont show empty btns in the drop down
                Destroy(_buttonsName[i]);
            }
        }
    }

    public void ClickUnitDropDown()
    {
        SetButtonsList(_unitBtn);
        List<string> names = new List<string>() { "Metric", "Imperial" };

        for (int i = 0; i < _buttonsName.Count; i++)
        {
            if (i < names.Count)
            {
                SetEachButton(_buttonsName[i], names[i].ToString(), "Unit");
            }
            else
            {
                //so dont show empty btns in the drop down
                Destroy(_buttonsName[i]);
            }
        }
    }

    /// <summary>
    /// Each button on the tera name drop down wil be set here
    ///
    /// Will add the event and change the name
    /// </summary>
    private void SetEachButton(GameObject b, string click, string type)
    {
        var button = b.GetComponent<UnityEngine.UI.Button>();
        SetBtnEvent(button, click, type);

        var child = GetChildCalledOnThis("Text", b);
        child.GetComponent<Text>().text = click;
    }

    private void SetBtnEvent(UnityEngine.UI.Button button, string name, string type)
    {
        if (type == "Quality")
        {
            button.onClick.AddListener(() => Settings.SetQuality(name));
        }
        else if (type == "Res")
        {
            button.onClick.AddListener(() => Settings.SetResolution(name));
        }
        else if (type == "Unit")
        {
            button.onClick.AddListener(() => Settings.SetUnit(name));
        }
        else if (type == "AutoSave")
        {
            button.onClick.AddListener(() => Settings.SetAutoSave(name));
        }
        else if (type == "Lang")
        {
            button.onClick.AddListener(() => Settings.SetLanguage(name));
        }
    }

    /// <summary>
    /// bz the buttons are inactive this must be set when is cliced
    /// </summary>
    private void SetButtonsList(GameObject Terra_Name_Btn)
    {
        _buttonsName.Clear();

        _buttonsName = new List<GameObject>()
        {
            GetGrandChildCalledFromThis("Btn_1", Terra_Name_Btn),
            GetGrandChildCalledFromThis("Btn_2", Terra_Name_Btn),
            GetGrandChildCalledFromThis("Btn_3", Terra_Name_Btn),
            GetGrandChildCalledFromThis("Btn_4", Terra_Name_Btn),
            GetGrandChildCalledFromThis("Btn_5", Terra_Name_Btn),
            GetGrandChildCalledFromThis("Btn_6", Terra_Name_Btn),

            GetGrandChildCalledFromThis("Btn_7", Terra_Name_Btn),
            GetGrandChildCalledFromThis("Btn_8", Terra_Name_Btn),
            GetGrandChildCalledFromThis("Btn_9", Terra_Name_Btn),
            GetGrandChildCalledFromThis("Btn_10", Terra_Name_Btn),
            GetGrandChildCalledFromThis("Btn_11", Terra_Name_Btn),
            GetGrandChildCalledFromThis("Btn_12", Terra_Name_Btn),

            GetGrandChildCalledFromThis("Btn_13", Terra_Name_Btn),
            GetGrandChildCalledFromThis("Btn_14", Terra_Name_Btn),
            GetGrandChildCalledFromThis("Btn_15", Terra_Name_Btn),
            GetGrandChildCalledFromThis("Btn_16", Terra_Name_Btn),
            GetGrandChildCalledFromThis("Btn_17", Terra_Name_Btn),
            GetGrandChildCalledFromThis("Btn_18", Terra_Name_Btn),
        };
    }

    #endregion DropDowns

    private void OnEnable()
    {
        Debug.Log("PrintOnEnable: OptionsWindow was enabled");
        LoadSlidersValues();
    }

    #region Sliders Sound and Music , Camera

    private void LoadSlidersValues()
    {
        if (!_soundSlider) return;

        _soundSlider.value = AudioCollector.SoundLevel;
        _musicSlider.value = AudioCollector.MusicLevel;

        //if(!CamControl.CAMRTS)
        //    _cameraSlider.value = 0.5f;
        //else
        //    _cameraSlider.value = CamControl.CAMRTS.CamSensivity/factorSens;

        if (Settings.CamSliderVal > 0.0f)
            _cameraSlider.value = Settings.CamSliderVal;
        else
            _cameraSlider.value = 0.5f;

        NewCamSensitivity();
    }

    public void NewSoundLevel()
    {
        AudioCollector.SetNewSoundLevelTo(_soundSlider.value);
    }

    public void NewMusicLevel()
    {
        AudioCollector.SetNewMusicLevelTo(_musicSlider.value);
    }

    private float factorSens = 12f;//bz .5 in the slider is 6 for the cam senstivity
    private float factorDesi = 2f;//bz .5 in the slider is 1

    //Called from GUI
    public void NewCamSensitivity()
    {
        if (CamControl.CAMRTS != null)
        {
            CamControl.CAMRTS.CamSensivity = _cameraSlider.value * factorSens;
            CamControl.CAMRTS.DesiredSpeed = _cameraSlider.value * factorDesi;
        }
        Settings.SetCamSliderVal(_cameraSlider.value);
    }

    #endregion Sliders Sound and Music , Camera

    //Called from GUI
    public void ResetUI()
    {
        PlayerPrefs.SetInt("ResetUI", 1);
    }

    public void DeletePlayerPref()
    {
        PlayerPrefs.DeleteAll();
    }
}