using UnityEngine;

public class MenuHandler : General
{
    public static bool CREATEMENU = true;//use to decide if create new menu

    internal Button currentBtn;
    private Btn3D tipMenu = null;
    internal Btn3D textMessage;
    private bool tipHover = true;

    //sounds
    internal AudioPlayer audioPlayer;

    //temporary
    private Vector3 moveToPosTemp;

    internal Btn lastAction;

    // Use this for initialization
    internal void Start()
    {
        //audioPlayer = new AudioPlayer();
    }

    /// <summary>
    /// Will act for each btn set actiion .. this method is implemented in all child classes too
    /// </summary>
    /// <param name="setOfBtns"></param>
    internal void ActionableBtnClick(Button[] setOfBtns)//actionable buttons
    {
        //make sure there is not two obj in the array with the same name or contains partial part of the same name
        currentBtn = USearch.FindBtnInColl(setOfBtns, Program.MOUSEOVERTHIS.name);

        if (currentBtn == null) return;
        else if (currentBtn.btnAction == Btn.SelectMale1)
        {
            SelectPlayerRoutine(setOfBtns, Root.boardMale1);
        }
        else if (currentBtn.btnAction == Btn.SelectMale2)
        {
            SelectPlayerRoutine(setOfBtns, Root.boardMale2);
        }
        else if (currentBtn.btnAction == Btn.PlayNewGame)
        {
            //needed here other wise would only create menu in next Scene after first left click
            MenuHandler.CREATEMENU = true;
            Application.LoadLevel("Test3");
        }
        else if (currentBtn.btnAction == Btn.ExitGame)
        {
            Application.Quit();
        }
    }

    /// <summary>
    /// It finds the single board obj and replace it with the one for the selected character
    /// </summary>
    /// <param name="setOfBtns">The set of buttons passed</param>
    /// <param name="rootP">The obj root will be spawn</param>
    private void SelectPlayerRoutine(Button[] setOfBtns, string rootP)
    {
        int indexOfSingleB = -1;
        Btn3D singleBoard = (Btn3D)USearch.FindBtnInColl(setOfBtns, "SingleBoard", out indexOfSingleB);
        string storeTemp = singleBoard.gameObject.name;
        moveToPosTemp = singleBoard.MovingToPosition;

        Destroy(singleBoard.gameObject);
        MenuHandler.CREATEMENU = true;
        //Single Board object handlgins
        singleBoard = (Btn3D)CreateBtn(singleBoard, rootP, moveToPosTemp);
        singleBoard.gameObject.name = storeTemp;
        singleBoard.MovingToPosition = moveToPosTemp;
        //needs to be assign otherwise after being hovered lose movetoPos
        singleBoard.InitialPosition = moveToPosTemp;

        MenuHandler.CREATEMENU = false;
        //asign this back to the array so as is reference the array will hold this new obj now
        setOfBtns[indexOfSingleB] = singleBoard;
    }

    public void ChangeAudioSettings(string typeP)
    {
        //if (typeP == "Sound")
        //{
        //    Settings.Switch(H.Sound);
        //    return;
        //}
        //else if (typeP == "Music")
        //{
        //    Settings.music = Settings.Switch(H.Music, Settings.music);
        //    return;
        //}
    }

    // Update is called once per frame
    internal void Update()
    {
    }

    /// will return object type Menus based on action
    public static Button CreateBtn(Button menuPass, string rootAction, Vector3 iniPosPass = new Vector3()
        , float speedPass = 0, string newName = "")
    {
        if (MenuHandler.CREATEMENU)
        {
            //Actions
            menuPass = (Button)General.Create(rootAction, iniPosPass);
            menuPass.FadeDirection = "FadeIn";
            menuPass.MoveSpeed = speedPass;
            if (menuPass.GetComponent<GUIText>() != null)
            {
                menuPass.GetComponent<GUIText>().text = newName;
            }
        }
        return menuPass;
    }

    internal void SpawnText(Vector3 iniPosPass, string msgP = "")
    {
        //spwan a message
        if (msgP != "")
        {
            textMessage = (Btn3D)General.Create(Root.menusTextMiddle, iniPosPass);
            textMessage.MoveSpeed = 40f; //so fade happens
            textMessage.FadeDirection = "FadeIn";
            textMessage.GetComponent<GUIText>().text = msgP;
        }
        //spwans a tip
        else if (Program.MOUSEOVERTHIS != null)
        {
            if (Tips.ReturnTip(Program.MOUSEOVERTHIS.name) != "No tips")
            {
                if (tipHover)
                {
                    tipMenu = (Btn3D)General.Create(Root.menusTextLeft, iniPosPass);
                    tipMenu.MoveSpeed = 40f; //so fade happens
                    tipMenu.FadeDirection = "FadeIn";
                    tipMenu.GetComponent<GUIText>().text = Tips.ReturnTip(Program.MOUSEOVERTHIS.name);
                    tipHover = false;
                }
                else if (tipMenu.GetComponent<GUIText>().text != Tips.ReturnTip(Program.MOUSEOVERTHIS.name))
                {
                    tipMenu.GetComponent<GUIText>().text = Tips.ReturnTip(Program.MOUSEOVERTHIS.name);
                    tipMenu.transform.position = iniPosPass;
                }
            }
            else if (Tips.ReturnTip(Program.MOUSEOVERTHIS.name) == "No tips")
            {
                DestroyTipMenu();
            }
        }
    }

    internal void DestroyTipMenu()
    {
        if (tipMenu != null && !tipHover)
        {
            tipMenu.Destroyer();
            tipMenu = null;
            tipHover = true;
        }
    }
}