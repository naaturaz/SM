using UnityEngine;
using System.Collections.Generic;

public class MouseListener : InputMain
{
    private MyForm _currForm = new MyForm();
    public MyForm CurrForm
    {
        get { return _currForm; }
        set { _currForm = value; }
    }

    public BuildingWindow BuildingWindow1
    {
        get { return _buildingWindow; }
        set { _buildingWindow = value; }
    }

    public PersonWindow PersonWindow1
    {
        get { return _personWindow; }
        set { _personWindow = value; }
    }

    public BuildingsMenu BuildingsMenu1
    {
        get { return _buildingsMenu; }
        set { _buildingsMenu = value; }
    }


    // Use this for initialization
    public void Start()
    {
       
    }

    /// <summary>
    /// Loading  and Reloading Main Form 
    /// </summary>
    private MyForm main;//the main GUI 
    public void LoadMainGUI()
    {
        if (main == null)
        {
            main = (MyForm)Create(Root.mainGUI, new Vector2());
        }

        //can only be one on scene to work 
        _buildingsMenu = FindObjectOfType<BuildingsMenu>();
        _descriptionWindow = FindObjectOfType<DescriptionWindow>();
        _personWindow = FindObjectOfType<PersonWindow>();
        _buildingWindow = FindObjectOfType<BuildingWindow>();
        _addOrderWindow = FindObjectOfType<AddOrderWindow>();
    }

    private Vector3 mainTempIniPos;
    public void HideMainGUI()
    {
        mainTempIniPos = main.transform.position;

        Vector3 t = mainTempIniPos;
        t.y += 400f;

        main.transform.position = t;
    }

    public void ShowMainGUI()
    {
        main.transform.position = mainTempIniPos;
    }

    public void ApplyChangeScreenResolution()
    {
        Program.gameScene.Fustrum1.RedoRect();
        Program.MyScreen1.ReLoadMainMenuIfActive();
        
        //being called before a game is loaded 
        if (_personWindow == null)
        {
            return;
        }

        HideAllWindows();

        main.Destroy();
        main = null;

        LoadMainGUI();
        //in case PersonWindow was not null. So main menu is last 
        Program.MyScreen1.ReLoadMainMenuIfActive();
    }

    /// <summary>
    /// Mian  input method
    /// </summary>
    /// <param name="action"></param>
    public void DetectMouseClick(string action)
    {
        //print("DetectMouseClick() :" + type);
        if (action == "Outside")
        {
            _addOrderWindow.Hide();
            _buildingWindow.Hide();

            //try to select person first
            if (!SelectPerson())
            {
                //if coulndt then try to select build
                if (!SelectClickedBuild())
                {
                    //if was not posible to seelct a building 
                    HideAllWindows();       
                }
            }
        }
        else if (action != "")
        {
            ActionFromForm(action);
        }
    }

    /// <summary>
    /// Will try to select a person. Person selection has more importantce than 
    /// building selection and priority 
    /// </summary>
    /// <returns>Will retrun true if a person was selected</returns>
    bool SelectPerson()
    {
        Transform clicked = UPoly.RayCastLayer(Input.mousePosition, 11).transform;

        if (clicked != null)
        {
            print("Clicked:" + clicked.name);

            _personSelect = clicked.GetComponent<Person>();
            _personWindow.Show(_personSelect);

            UnselectingBuild();

            _buildingWindow.Hide();
            return true;
        }
        _personWindow.Hide();
        return false;
    }

    /// <summary>
    /// Will select clicked building and ret true if one was seelected 
    /// </summary>
    /// <returns></returns>
    bool SelectClickedBuild()
    {
        if (!Input.GetMouseButtonUp(0))
        {
            return false;
        }
     
        List<string> names = new List<string>();
        var clicked = ReturnBuildinHit();

        //unselect if was click outise 
        if (clicked != null)
        {
            names = UString.ExtractNamesUntilGranpa(clicked);
            Program.InputMain.InputMouse.UnSelectRoutine(names, clicked);
        }
        //select new Build
        if (names.Count > 0 )
        {
            for (int i = 0; i < names.Count; i++)
            {
                H typeL = Program.InputMain.InputMouse.FindType(names[i]);
                Ca cat = DefineCategory(typeL);
                Program.InputMain.InputMouse.Select(cat, names[i]);

                if (BuildingPot.Control.Registro.SelectBuilding != null)
                {
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Method tht address when a building is unseletced
    /// </summary>
    void UnselectingBuild()
    {
        _buildingsMenu.Hide();
        _dictSelection = -1;

        Program.InputMain.InputMouse.UnSelectCurrent();
    }

    /// <summary>
    /// Will return the build or way hitt by mouse position 
    /// </summary>
    /// <returns></returns>
    Transform ReturnBuildinHit()
    {
        //used to be UPoly.RayCastAll()
        Transform clicked = UPoly.RayCastLayer(Input.mousePosition, 10).transform;//10: personBlock   

        //try ways then
        if (clicked == null)
        {
            clicked = UPoly.RayCastLayer(Input.mousePosition, 12).transform;//12: way  
        }

        return clicked;
    }

    /// <summary>
    /// Actions to perform from form
    /// </summary>
    /// <param name="action"></param>
    public void ActionFromForm(string action)
    {
        //btn from main menu
        if (action.Contains("MainMenu."))
        {
            Program.MyScreen1.MouseListenAction(action);
        }
        else if (action == "upgBuildMat")
        {
            _buildingWindow.ClickedUpdMatBtn();
        }
        else if (action == "upgBuildCap")
        {
            _buildingWindow.ClickedUpdCapBtn();
        }
        else if (action == "Close_Btn")
        {
            _personWindow.Hide();

            _buildingWindow.Hide();
            UnselectingBuild();
        }
        else if (action == "Demolish_Btn")
        {
            DemolishAction();

            _buildingWindow.Reload();
        }
        else if (action == "Cancel_Demolish_Btn")
        {
            CancelDemolishAction();
            _buildingWindow.Reload();

        }    
        else if (action.Contains("Dialog."))
        {
            Dialog.Listen(action);
        }     
        else if (action.Contains("GUIBtn."))
        {
            GUIBtnHandlers(action);
        }
        else if (action == H.Next_Stage_Btn.ToString())
        {
            if (BuildingPot.Control.Registro.SelectBuilding.HType.ToString().Contains("Bridge"))
            {
                Bridge b = BuildingPot.Control.Registro.SelectBuilding as Bridge;
                b.ShowNextStageOfParts();
            }
            else
            {
                Structure b = BuildingPot.Control.Registro.SelectBuilding as Structure;
                b.ShowNextStage();
            }
        }
        //Handling GUI : Aug 2015
        else
        {
            HandleGUIClicks(action);
        }
    }

    /// <summary>
    /// Handle actions from buttons in GUI 
    /// </summary>
    /// <param name="action"></param>
    private void GUIBtnHandlers(string action)
    {
        action = action.Substring(7);

        if (action == "Menu")
        {
            Program.InputMain.EscapeKey();
        }
        else if (action == "QuickSave")
        {
            Program.InputMain.QuickSaveNow();
        }
        else if (action == "Share")
        {
            
        } 
        else if (action == "MoreSpeed")
        {
            Program.InputMain.ChangeGameSpeedBy(1);
        }
        else if (action == "LessSpeed")
        {
            Program.InputMain.ChangeGameSpeedBy(-1);
        }  
        else if (action == "Feedback")
        {
            
        }
        else if (action == "BugReport")
        {
            
        }
    }















    #region GUI. Aug 2015

    //this is holding the index of _inputListDict on InputBuilding
    private int _dictSelection = -1;

    private BuildingsMenu _buildingsMenu;
    private DescriptionWindow _descriptionWindow;

    private PersonWindow _personWindow;
    private BuildingWindow _buildingWindow;
    private AddOrderWindow _addOrderWindow;

    /// <summary>
    /// Will handle all the inputs from the buttons on the GUI 
    /// </summary>
    /// <param name="action"></param>
    void HandleGUIClicks(string action)
    {
        //when adding a nw import or export . clicked on Dock orders tab
        if (action == "Add_Export_Btn" || action == "Add_Import_Btn")
        {
           AddExportImport(action);
        }
            //when selecting a prod on _addOrderForm
        else if (action.Contains("AddOrder."))
        {
            _addOrderWindow.FeedFromForm(action);
        }
        else if (action.Contains("BuildingForm."))
        {
            _buildingWindow.FeedFromForm(action);
        }



        //building new buildins
        else if (action.Contains("Slot"))
        {
            HandleSlot(action);
        }
            //clicking on the categories of buildings . ex Road
        else
        {
            HandleBuildCat(action);
        }
    }



    void AddExportImport(string action)
    {
        if (action == "Add_Export_Btn" )
        {
            _addOrderWindow.Show("Export");
        }
        else if( action == "Add_Import_Btn")
        {
            _addOrderWindow.Show("Import");
        }
    }





    /// <summary>
    /// Will hide the Person, Building, and AddOrder Window
    /// </summary>
    public void HideAllWindows()
    {


        _personWindow.Hide();
        UnselectingBuild();
        _addOrderWindow.Hide();
    }

    /// <summary>
    /// This is when click on , Road, or House 
    /// </summary>
    /// <param name="action"></param>
    private void HandleBuildCat(string action)
    {
        HideAllWindows();

        _dictSelection = ReturnDictSelection(action);

        LoadIconsOnMenu();
    }

    /// <summary>
    /// Will load the Icons on the Menu 
    /// </summary>
    private void LoadIconsOnMenu()
    {
        var dict = BuildingPot.InputU.InputListDict[_dictSelection];
        var list = ReturnHList(dict);
        _buildingsMenu.Show(list);
    }

    /// <summary>
    /// Will return the H values of the Dict 
    /// </summary>
    /// <param name="dict"></param>
    /// <returns></returns>
    List<H> ReturnHList(Dictionary<KeyCode, H> dict)
    {
        List<H> res = new List<H>();

        foreach (var item in dict)
        {
            res.Add(item.Value);
        }

        return res;
    }

    /// <summary>
    /// Depending on the Category of building selected will
    ///  return wichi index is for '_inputListDict' on InputBuilding.cs
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    int ReturnDictSelection(string action)
    {
        var res = -1;
        if (action == "Infrastructure")
        {
            res = 0;
        }
        else if (action == "House")
        {
            res = 1;
        }
        else if (action == "Food")
        {
            res = 2;
        }
        else if (action == "Raw")
        {
            res = 3;
        }
        else if (action == "Prod")
        {
            res = 4;
        }
        else if (action == "Ind")
        {
            res = 5;
        }
        else if (action == "Trade")
        {
            res = 6;
        }
        else if (action == "Gov")
        {
            res = 7;
        }
        else if (action == "Other")
        {
            res = 8;
        }
        else if (action == "Militar")
        {
            res = 9;
        }
        return res;
    }

    /// <summary>
    /// Handles a Slot. This is where 1 would be HouseA, and 2 HouseB
    /// 
    /// This is handlign the click action 
    /// </summary>
    /// <param name="action"></param>
    void HandleSlot(string action)
    {
        var dict = BuildingPot.InputU.InputListDict[_dictSelection];
        InputBuilding.InputMode = Mode.Building;
        var key = ReturnKeyCode(action);

        //means is click an empty slot 
        if (!dict.ContainsKey(key))
        {
            return;
        }

        var val = dict[key];
        BuildingPot.InputU.BuildingSwitch(val);
        
        _buildingsMenu.Hide();
        _descriptionWindow.Hide();

        _dictSelection = -1;
    }

    /// <summary>
    /// With the Slot1 as 'action' will return wht keyCode is 
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    KeyCode ReturnKeyCode(string action)
    {
        if (action == "Slot1")
        {
            return KeyCode.Alpha1;
        }
        if (action == "Slot2")
        {
            return KeyCode.Alpha2;
        }
        if (action == "Slot3")
        {
            return KeyCode.Alpha3;
        }
        if (action == "Slot4")
        {
            return KeyCode.Alpha4;
        }
        if (action == "Slot5")
        {
            return KeyCode.Alpha5;
        }
        if (action == "Slot6")
        {
            return KeyCode.Alpha6;
        }
        if (action == "Slot7")
        {
            return KeyCode.Alpha7;
        }
        if (action == "Slot8")
        {
            return KeyCode.Alpha8;
        }
        if (action == "Slot9")
        {
            return KeyCode.Alpha9;
        }
        if (action == "Slot10")
        {
            return KeyCode.Alpha0;
        }
        return KeyCode.None;
    }




    /// <summary>
    /// This is to know which H is the one is clicked on slot
    /// 
    /// Knowing already wht category was touched is easy
    /// 
    /// 
    /// </summary>
    /// <param name="slot"></param>
    /// <returns></returns>
    public H ReturnThisSlotVal(string slot)
    {
        if (_dictSelection == -1)
        {
            return H.None;
        }

        var dict = BuildingPot.InputU.InputListDict[_dictSelection];

        for (int i = 0; i < dict.Count; i++)
        {
            KeyCode code = ReturnKeyCode(slot);

            if (!dict.ContainsKey(code))
            {
                //to avoid when Buildins category dont have the whole 10 alpha codes. which is majority 
                return H.None;
            }

            return dict[code];
        }
        return H.None;
    }





#endregion




    #region Person Selection Form

    private Person _personSelect;



#endregion



    private void CancelDemolishAction()
    {
        if (BuildingPot.Control.Registro.SelectBuilding.Category == Ca.Structure)
        {
            Structure b = BuildingPot.Control.Registro.SelectBuilding as Structure;
            b.CancelDemolish();
        }
    }

    void DemolishAction()
    {
        //print("Selected name:" + b.name);
        if (BuildingPot.Control.Registro.SelectBuilding.Category == Ca.Way)
        {
            Trail b = BuildingPot.Control.Registro.SelectBuilding as Trail;
            b.Demolish();
        }
        else if (BuildingPot.Control.Registro.SelectBuilding.Category == Ca.Structure
           || BuildingPot.Control.Registro.SelectBuilding.Category == Ca.Shore)
        {
            Structure b = BuildingPot.Control.Registro.SelectBuilding as Structure;

            if (BuildingPot.Control.IsThisTheLastFoodSrc(b))
            {
                GameScene.ScreenPrint("Cant destroy last Food Src ");
                return;
            }

            if (BuildingPot.Control.IsThisTheLastOfThisType(H.Masonry, b))
            {
                GameScene.ScreenPrint("Cant destroy last Builders Office ");
                return;
            }

            b.Demolish();
        }
        else if (BuildingPot.Control.Registro.SelectBuilding.Category == Ca.DraggableSquare)
        {
            DragSquare b = BuildingPot.Control.Registro.SelectBuilding as DragSquare;
            b.Demolish();
        }

        //Program.InputMain.InputMouse.UnSelectCurrent();
        //DestroyForm();
    }





    #region Update Building Material

   
    void UpgradeBuildMatRoutine()
    {







    }





   


#endregion




	// Update is called once per frame
    public void Update()
    {

	}

    public void CreateNewForm(H type)
    {
        DestroyForm();

        if (type == H.Selection)
        {
            _buildingWindow.Show(BuildingPot.Control.Registro.SelectBuilding);
        }
    }

    public void DestroyForm()
    {
        if (_currForm != null)
        {
            _currForm.Destroy();
            _currForm = null;
        }
    }
}
