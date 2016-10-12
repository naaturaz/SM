using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class OptionsWindow : GUIElement
{
    private Toggle _fullToggle;
    private Toggle _musicToggle;
    private Toggle _soundToggle;


    private GameObject _autoSaveBtn;
    private GameObject _unitBtn;
    private GameObject _langBtn;


    private GameObject _resBtn;
    private GameObject _qualityBtn;  
    
    private Text _autoSaveBtnTxt;
    private Text _unitBtnTxt;
    private Text _langBtnTxt;

    private Text _resBtnTxt;
    private Text _qualityBtnTxt;

    private Slider _musicSlider;
    private Slider _soundSlider;

    void Start()
    {
        iniPos = transform.position;

        _fullToggle = GetGrandChildCalled("FullScreen_Toggle").GetComponent<Toggle>();
        _musicToggle = GetGrandChildCalled("Music_Toggle").GetComponent<Toggle>();
        _soundToggle = GetGrandChildCalled("Sound_Toggle").GetComponent<Toggle>();
        
        
        _musicSlider = GetGrandChildCalled("Music_Slider").GetComponent<Slider>();
        _soundSlider = GetGrandChildCalled("Sound_Slider").GetComponent<Slider>();




        var autoSavePanel = GetGrandChildCalled("Panel_AutoSave");
        _autoSaveBtn = GetChildCalledOnThis("AutoSave_Btn", autoSavePanel);
        _autoSaveBtnTxt = GetChildCalledOnThis("Text", _autoSaveBtn).GetComponent<Text>();

        var langPanel = GetGrandChildCalled("Panel_Lang");
        _langBtn = GetChildCalledOnThis("Lang_Btn", langPanel);
        _langBtnTxt = GetChildCalledOnThis("Text", _langBtn).GetComponent<Text>();
        
        var unitPanel = GetGrandChildCalled("Panel_Unit");
        _unitBtn = GetChildCalledOnThis("Unit_Btn", unitPanel);
        _unitBtnTxt = GetChildCalledOnThis("Text", _unitBtn).GetComponent<Text>();
        

        var resPanel = GetGrandChildCalled("Panel_Res");
        _resBtn = GetChildCalledOnThis("Res_Btn", resPanel);
        _resBtnTxt = GetChildCalledOnThis("Text", _resBtn).GetComponent<Text>();
        
        var qualityPanel = GetGrandChildCalled("Panel_Quality");
        _qualityBtn = GetChildCalledOnThis("Quality_Btn", qualityPanel);
        _qualityBtnTxt = GetChildCalledOnThis("Text", _qualityBtn).GetComponent<Text>();




        SetAllControls();
        RefreshAllDropDowns();

        Hide();

        //means was hidden by a Res Change 
        if (resTimeChanged!=0)
        {
            resTimeChanged = 0;
            Show();
        }

        LoadSlidersValues();

    }

    private void SetAllControls()
    {
        _fullToggle.isOn = Screen.fullScreen;


        _musicToggle.isOn = Settings.ISMusicOn;
        _soundToggle.isOn = Settings.ISSoundOn;


        //so they dont trigger event 
        _fullToggle.onValueChanged.AddListener((value) => Program.MouseClickListenerSt("MainMenu.Options.Full"));
        _musicToggle.onValueChanged.AddListener((value) => Program.MouseClickListenerSt("MainMenu.Options.Music"));
        _soundToggle.onValueChanged.AddListener((value) => Program.MouseClickListenerSt("MainMenu.Options.Sound"));
    }

    public void RefreshAllDropDowns()
    {
        _autoSaveBtnTxt.text = (Settings.AutoSaveFrec / 60) + " min";
        _unitBtnTxt.text = "Imperial";
        if (Unit.Units == 'm')
        {
            _unitBtnTxt.text = "Metric";
        }
        _resBtnTxt.text = CurrentResolution();
        _langBtnTxt.text = Languages.CurrentLang();

        var names = QualitySettings.names;
        var curr = QualitySettings.GetQualityLevel();
        for (int i = 0; i < names.Length; i++)
        {
            if (i == curr)
            {
                _qualityBtnTxt.text = names[i];
            }
        }   
    }

    /// <summary>
    /// bz if is not full screen then is not accurate since will return the size
    /// of the screen and not the window
    /// </summary>
    /// <returns></returns>
    string CurrentResolution()
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
    void Update()
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



    string HandleAction(string sub)
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

        if(sub == "OKBtn")
        {
            Hide();
        }
        else if(sub == "CancelBtn")
        {
            Hide();
        }
        else if (sub == "Music" || sub == "Sound")
        {
            ChangeAudioSettings(sub);
        }
        //screen
        else if (sub == "Full")
        {
            Screen.fullScreen = Settings.MecanicSwitcher(Screen.fullScreen);
            ChangeResNow();
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
        List<string> names = new List<string>() { "English", "Español" };

        for (int i = 0; i < _buttonsName.Count; i++)
        {
            if (i < names.Count)
            {
                SetEachButton(_buttonsName[i], names[i].ToString(), "Lang");
            }
            else
            {
                //so dont show empty btns in the drop down 
                Destroy(_buttonsName[i]);
            }
        }
    }


    public void ClickAutoSaveDropDown()
    {
        SetButtonsList(_autoSaveBtn);
        List<string> names = new List<string>() {"5 min", "10 min", "15 min", "20 min"};

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
        List<string> names = new List<string>() {"Metric", "Imperial"};

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

    void SetBtnEvent(UnityEngine.UI.Button button, string name, string type)
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

#endregion


#region Sliders Sound and Music

    void LoadSlidersValues()
    {
        _soundSlider.value = AudioCollector.SoundLevel;
        _musicSlider.value = AudioCollector.MusicLevel;

    }

    public void NewSoundLevel()
    {
        AudioCollector.SetNewSoundLevelTo(_soundSlider.value);
    }

    public void NewMusicLevel()
    {
        AudioCollector.SetNewMusicLevelTo(_musicSlider.value);
    }

#endregion
}

