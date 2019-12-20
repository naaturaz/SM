using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewGameWindow : GUIElement
{
    private string _townName;
    //private string _size;
    private string _terraName;
    private string _terraRoot;
    private string _difficulty;

    //private Text _sizeTxt;//the btn tht contains the size 
    private Text _terraNameTxt;//the btn tht contains the size 
    private LangUpdateScript _diffLang;//the btn tht contains the size 
    private LangUpdateScript _typeLang; 
    private InputField _inputTownName;

    private List<string> _terraNames = new List<string>();//the names will be displayed on the terra name drop down  

    //this is the options in the drop down names 
    private List<GameObject> _buttonsName = new List<GameObject>();// 

    private GameObject Terra_Name_Btn;
    private GameObject Diff_Btn;
    private Toggle _pirateToggle;
    private Toggle _foodToggle;

	// Use this for initialization
	void Start ()
	{
	    iniPos = transform.position;
        Hide();

        //var Terra_Size_Btn = GetGrandChildCalled("Terra_Size_Btn");
        //_sizeTxt = Terra_Size_Btn.GetComponentInChildren<Text>();

        Terra_Name_Btn = GetGrandChildCalled("Terra_Name_Btn");
        _terraNameTxt = Terra_Name_Btn.GetComponentInChildren<Text>();

        Diff_Btn = GetGrandChildCalled("Diff_Btn");
        _diffLang = Diff_Btn.GetComponentInChildren<LangUpdateScript>();
        
        var t_Btn = GetGrandChildCalled("Type_Btn");
        _typeLang = t_Btn.GetComponentInChildren<LangUpdateScript>();
        _typeLang.SetKey("Freewill");

        _inputTownName = GetChildCalled("Input_Name").GetComponent<InputField>();

        _pirateToggle = GetChildCalled("Pirate_Toggle").GetComponent<Toggle>();
        _foodToggle = GetChildCalled("Food_Toggle").GetComponent<Toggle>();

        LoadDefaultForNewGame();
	    AddressDevVer();
    }

    private void AddressDevVer()
    {
        if (!Developer.IsDev)
        {
            GetChildCalled("Panel_Terra_Name").SetActive(false);
            GetChildCalled("Terra_Name_Lbl").SetActive(false);
        }
    }

    string[] array = new string[] {
        "Matanzas", "Havana", "Baracoa", "Varadero", "Santiago de Cuba", "Pinar del Rio", "Versalles", "Colon", "Las Villas",
        "Topes de Collante", "La Cumbre", "Cumbre Alta", "Santa Cristina", "Aranguren", "Ancon", "Vinales", "Havana Libre",
        "San Severino",
        "Sacti Spiritus", "Trinidad",
        "Sevilla", 
        "San Hipolito", "Paseo Marti",
        "La Pinta", "La Nina", "La Santa Maria",
        "Sedano", "Hialeah", "Key West", "Orlando", "San Agustine",
        "Boca Raton", "Florida", "Palm Beach", "Calusa",
    };
    /// <summary>
    /// Load default conditions for a game
    /// </summary>
    void LoadDefaultForNewGame()
    {

        _townName = array[UMath.GiveRandom(0, array.Length)];
#if UNITY_EDITOR
        _townName = "Editor";
#endif
        _difficulty = "Easy";

        DefineTerrainNames();
        Display();
    }
	
	// Update is called once per frame
	void Update () {
	}

    public void MouseListen(string action)
    {
        var sub = action.Substring(4);

        //create new game 
        if (sub == "OKBtn")
        {
            DefineTownName();
            Program.IsPirate = _pirateToggle.isOn;
            Program.IsFood = _foodToggle.isOn;
            Program.MyScreen1.NewGameCreated(_terraRoot, _difficulty, _townName);

            //Defines type of game here only when OK is cliked
            if (_typeLang.Key == "Traditional")
            {
                Program.TypeOfGame = H.Lock;
            }
            else if (_typeLang.Key == "Freewill")
            {
                Program.TypeOfGame = H.Unlock;
            }
        }
        //Reloadd main menu
        else if (sub == "CancelBtn")
        {
            Program.MyScreen1.HideWindowShowMain(this);
        }
        else if (action == "Tutorial")
        {
            Tutorial();
            return;
        }

        Display();
    }

    /// <summary>
    /// The call of the Tutorial from the Main Menu
    /// </summary>
    private void Tutorial()
    {
        Program.WasTutoPassed = false;
        PlayerPrefs.SetString("Tuto", "");

        _townName = "Tutorial";
        //save load window set
        DataController.LoadGameTuto();
    }

    /// <summary>
    /// Once the size is selected in the TerraName the terrain names must be loaded there 
    /// </summary>
    private void DefineTerrainNames()
    {
        //if (_size == "Big")
        //{
            DefineEachTerraName(Root.BigTerrains);
        //}
    }

    /// <summary>
    /// Will make __terraNames the last part of each element on the list 
    /// </summary>
    /// <param name="list"></param>
    void DefineEachTerraName(List<string> list)
    {
        _terraNames.Clear();

        for (int i = 0; i < list.Count; i++)
        {
            _terraNames.Add(list[i].Split('/')[2]);
        }
    }

    private void Display()
    {
        _inputTownName.text = _townName;

        //_sizeTxt.text = _size;
        _terraNameTxt.text = _terraName;
        _diffLang.SetKey(_difficulty);
    }

    /// <summary>
    /// Is also called from GUI
    /// </summary>
    public void DefineTownName()
    {
        _townName = _inputTownName.text;
        
        //so that gets define 
        ClickOnTypeOfGame(_typeLang.Key);
    }

    /// <summary>
    /// The last part of the name of the terrain which is wht is added on the Button when they are loaded
    /// with a specific terrain set of names 
    /// </summary>
    /// <param name="last"></param>
    /// <param name="roots"></param>
    /// <returns></returns>
    string ReturnTerrainOnList(string last, List<string> roots )
    {
        for (int i = 0; i < roots.Count; i++)
        {
            if (roots[i].Contains(last))
            {
                return roots[i];
            }
        }
        return "";
    }

    /// <summary>
    /// Is called when the terraName drop down Btn is clicked 
    /// </summary>
    public void ClickTerraNameDropDown()
    {
        SetButtonsList();

        for (int i = 0; i < _buttonsName.Count; i++)
        {
            SetEachButton(_buttonsName[i], _terraNames[i]);
        }
    }

    /// <summary>
    /// Each button on the tera name drop down wil be set here 
    /// 
    /// Will add the event and change the name 
    /// </summary>
    private void SetEachButton(GameObject b, string terraName)
    {
        var button = b.GetComponent<UnityEngine.UI.Button>();
        button.onClick.AddListener(() => Program.MyScreen1.NewGameWindow1.ClickTerraNameSelection(terraName));

        var child = GetChildCalledOnThis("Text", b);
        child.GetComponent<Text>().text = terraName;
    }

    /// <summary>
    /// bz the buttons are inactive this must be set when is cliced
    /// </summary>
    private void SetButtonsList()
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
            //GetGrandChildCalledFromThis("Btn_15", Terra_Name_Btn),
        };
    }

    /// <summary>
    /// Once a specicifc terra name was seelected 
    /// </summary>
    public void ClickTerraNameSelection(string terraName)
    {
        _terraName = terraName;
        //if (_size == "Big")
        //{
            _terraRoot = ReturnTerrainOnList(terraName, Root.BigTerrains);
        //}
        Display();
    }

    //Dificulty

    //
    /// <summary>
    /// Is called when the terraName drop down Btn is clicked 
    /// </summary>
    public void ClickDifficultyDropDown()
    {
        //var newbie = GetGrandChildCalledFromThis("Btn_Newbie", Diff_Btn);
        var easy = GetGrandChildCalledFromThis("Btn_Easy", Diff_Btn);
        var moderate = GetGrandChildCalledFromThis("Btn_Moderate", Diff_Btn);
        var hard = GetGrandChildCalledFromThis("Btn_Hard", Diff_Btn);
        //var insane = GetGrandChildCalledFromThis("Btn_Insane", Diff_Btn);

        //SetDiffButton(newbie, "Newbie");
        SetDiffButton(easy, "Easy");
        SetDiffButton(moderate, "Moderate");
        SetDiffButton(hard, "Hard");
        //SetDiffButton(insane, "Insane");
    }

    private void SetDiffButton(GameObject b, string diff)
    {
        var button = b.GetComponent<UnityEngine.UI.Button>();
        button.onClick.AddListener(() => Program.MyScreen1.NewGameWindow1.ClickDifficultySelection(diff));

        var child = GetChildCalledOnThis("Text", b);
        child.GetComponent<Text>().text = diff;
    }

    public void ClickDifficultySelection(string difficulty)
    {
        _difficulty = difficulty;
        Display();
    }

    /// <summary>
    /// And is called from GUI also
    /// </summary>
    /// <param name="pass"></param>
    public void ClickOnTypeOfGame(string pass)
    {
        if (pass == "Traditional")
        {
            _typeLang.SetKey("Traditional");
        }
        else if (pass == "Freewill")
        {
            _typeLang.SetKey("Freewill");
        }
        if (TownLoader.IsTemplate)
        {
            Program.TypeOfGame = H.None;
            _typeLang.SetKey("None");
        }
    }

    /// <summary>
    /// Called from GUI
    /// </summary>
    public void LockInput()
    {
        Program.LockInputSt();
    }

    /// <summary>
    /// Called from GUI
    /// </summary>
    public void UnLockInput()
    {
        Program.UnLockInputSt();
    }

    public void PirateChange()
    {
    }

    public void FoodChange()
    {
    }
}
