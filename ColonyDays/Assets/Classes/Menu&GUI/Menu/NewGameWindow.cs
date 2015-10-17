using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

public class NewGameWindow : GUIElement
{
    private string _townName;
    private string _size;
    private string _terraName;
    private string _terraRoot;
    private string _difficulty;

    private Text _sizeTxt;//the btn tht contains the size 
    private Text _terraNameTxt;//the btn tht contains the size 
    private Text _diffTxt;//the btn tht contains the size 
    private InputField _inputTownName;


    private List<string> _terraNames = new List<string>();//the names will be displayed on the terra name drop down  

    //this is the options in the drop down names 
    private List<GameObject> _buttonsName = new List<GameObject>();// 

    private GameObject Terra_Name_Btn;


	// Use this for initialization
	void Start ()
	{
	    iniPos = transform.position;
        Hide();

	    var Terra_Size_Btn = GetGrandChildCalled("Terra_Size_Btn");
	    _sizeTxt = Terra_Size_Btn.GetComponentInChildren<Text>();

        Terra_Name_Btn = GetGrandChildCalled("Terra_Name_Btn");
        _terraNameTxt = Terra_Name_Btn.GetComponentInChildren<Text>();

        var Diff_Btn = GetGrandChildCalled("Diff_Btn");
        _diffTxt = Diff_Btn.GetComponentInChildren<Text>();

        _inputTownName = GetChildCalled("Input_Name").GetComponent<InputField>();






        LoadDefaultForNewGame();
	}

    /// <summary>
    /// Load default conditions for a game
    /// </summary>
    void LoadDefaultForNewGame()
    {
        _townName = "Toronto";
        _size = "Small";
        _difficulty = "Easy";



        Display();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void MouseListen(string action)
    {
        var sub = action.Substring(4);

        if (sub == "Small" || sub == "Med" || sub == "Big")
        {
            _size = sub;
            DefineTerrainNames();
        }
        else if (sub == "Easy" || sub == "Moderate" || sub == "Hard")
        {
            _difficulty = sub;
        }
        //create new game 
        else if (sub == "OKBtn")
        {
            DefineTownName();
            Program.MyScreen1.NewGameCreated(_terraRoot, _difficulty, _townName);
        }
        //Reloadd main menu
        else if (sub == "CancelBtn")
        {
            Program.MyScreen1.HideWindowShowMain(this);
        }

        Display();
    }

    /// <summary>
    /// Once the size is selected in the TerraName the terrain names must be loaded there 
    /// </summary>
    private void DefineTerrainNames()
    {
        if (_size == "Big")
        {
            DefineEachTerraName(Root.BigTerrains);
        }
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

        _sizeTxt.text = _size;
        _terraNameTxt.text = _terraName;
        _diffTxt.text = _difficulty;
    }

    void DefineTownName()
    {
        _townName = _inputTownName.text;
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
            GetGrandChildCalledFromThis("Btn_3", Terra_Name_Btn)
        };
    }

    /// <summary>
    /// Once a specicifc terra name was seelected 
    /// </summary>
    public void ClickTerraNameSelection(string terraName)
    {
        _terraName = terraName;
        if (_size == "Big")
        {

            _terraRoot = ReturnTerrainOnList(terraName, Root.BigTerrains);
        }

        Display();
    }
}
