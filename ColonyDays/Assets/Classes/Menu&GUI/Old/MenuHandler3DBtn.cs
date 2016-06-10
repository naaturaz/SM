using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* SEP 14 2014 - THIS CLASS PRETTY MUCH IS ONLY TO TAKE CARE OF THE MAIN MENU NOW 
 * SEP 16 2014 - THERE IS A LOT OF OLD CODE IN THIS CLASS I FOUND CODE TO OPEN AND SPAWN RAW AND ELEMENTS
 *              AND SPWAN MENUS ON SCREEN WHILE PLAYING ... THAT HAS TO BE DELETED
 */

public class MenuHandler3DBtn : MenuHandler
{
    //Current Menu Set Positions
    public Transform[] menuSetSpawn = new Transform[2];
    public Transform[] menuSetShow = new Transform[9];
    Transform menuSetTip = null;
    private Transform[] menuSetCameraGuide = new Transform[8];
    public Transform menuSetCamGuideNoSelection = null;

    private Btn3D[] menusArrayOne = new Btn3D[10];//will hold menus realtime

    //private bool toScale = true;
    private bool toBringForward = true;
    private Btn3D hoveredTemp = null;//use to store temporarly a hovered Menu

    private Material[] menuSetMat = new Material[5];

    //will flag is camera is moving btw menus
    public bool isCameraMoving = false;

	// Use this for initialization
	void Start ()
    {
        base.Start();
        LobbyHandleMenu("Select_Main_Btn_Main_3dMenu");//for main manu
	}

    new void ActionableBtnClick(Button[] setOfBtns)//actionable buttons 
    {
        base.ActionableBtnClick(setOfBtns);


        currentBtn = null;
    }

    //Lobby Find Origin And Camera Guide Points Apply Cam Target And SpawnMenu
    void LobbyHandleMenu(string whichMenu)
    {
        //if is not a clickable btn we exit this method
        if (currentBtn != null)
        {
            if (!currentBtn.isClickAble)
            {
                return;
            }
        }

        //only works in the Lobby Scene
        if (Application.loadedLevelName == "Lobby")
        {
            //format must be always like ths : Select_Main_Btn_Main_3dMenu
            string[] split = whichMenu.Split('_');
            string menuToPull = split[1] + "_" + "Menu";
            //if the first word is "Actionable" will come here ...
            if (split[0] == "Actionable")
            {
                //this method() is in the base class
                ActionableBtnClick(menusArrayOne);
            }
            else if (split[1] == "Music" || split[1] == "Sound")
            {
                ChangeAudioSettings(split[1]);
                return;
            }
            else
            {
                CloseCurrentMenu();
                SubRoutineToFindAllPointsAndPull3dMenu(menuToPull);
            }
        }
    }

    public void SubRoutineToFindAllPointsAndPull3dMenu(string menuToPull)
    {
        isCameraMoving = true;//camera is moving
        CamControl.CAMFollow.smoothTime = 0.125f;//makes the cam move fast
        //will find the point for this menu

        menuSetShow = USearch.GetAllTransforms(menuToPull + "_Origin_Points", H.Spawn, H.Tips);
        menuSetSpawn = USearch.IncludeOnlyTransforms(menuToPull + "_Origin_Points", H.Spawn);
        menuSetTip = USearch.IncludeOnly1Transform(menuToPull + "_Origin_Points", H.Tips);

        menuSetCameraGuide = USearch.GetAllTransforms(menuToPull + "_Camera_Guide_Points", H.NoSelection);
        menuSetCamGuideNoSelection =
            USearch.IncludeOnly1Transform(menuToPull + "_Camera_Guide_Points", H.NoSelection);

        //Assing TARGET in camera
        CamControl.CAMFollow.target = menuSetCamGuideNoSelection;
        //will spawn menu 
        SpawnMenu(menuToPull + "_Spawner", menuSetSpawn, menuSetShow, speedPass: 15f);
    }

    /// <summary>
    /// Trys to find the camera guide gameobj for the tPass object passed as parameter
    /// </summary>
    /// <param name="tPass">Object we want to find the Camera Guide for</param>
    /// <returns>The camera guide correspoding obj if found</returns>
    public Transform ReturnGuideCameraPoint(Transform tPass)
    {
        for (int i = 0; i < menuSetCameraGuide.Length; i++)
        {
            if (menuSetCameraGuide[i] != null)
            {
                if (tPass.name.Contains(menuSetCameraGuide[i].name))
                {
                    tPass = menuSetCameraGuide[i];
                }
            }
        }
        return tPass;
    }

	// Update is called once per frame
	void Update () 
    {
        //if some menu is on and  the CREATEMENU flag is off ...
        if (!MenuHandler.CREATEMENU  /*Program.WHATMENUISON != MenusType.None.*/)
        {
            for (int i = 0; i < menusArrayOne.Length; i++)
            {
                if (menusArrayOne[i] != null)
                {
                    if (menusArrayOne[i].Type != null)
                    {
                        if (menusArrayOne[i].Type == "TopUnEvenRow")
                        {
                            menusArrayOne[i].MovingToPosition = new Vector3();
                        }
                    }
                }
            }
        }

        LeftClick();
        HoveringOver();
        CheckIfCameraStillMoving();
	}

    //will check if camera is moving and when the position is same as NoSelection Camera_Guide point will set
    //flag to false ... will set the speed to slow again
    void CheckIfCameraStillMoving()
    {
        if (isCameraMoving)
        {
            if (CamControl.CAMFollow.transform.position == menuSetCamGuideNoSelection.position)
            {
                isCameraMoving = false;
                CamControl.CAMFollow.smoothTime = 1f;
            }
        }
    }

    void LeftClick()
    {
        currentBtn = USearch.FindBtnInColl(menusArrayOne, Program.MOUSEOVERTHIS);

        if (Input.GetMouseButtonUp(0))
        {
            //if MOUSEOVERTHIS not null
            if (Program.MOUSEOVERTHIS != null)
            {
                LobbyHandleMenu(Program.MOUSEOVERTHIS.name);
                //audioPlayer.PlayAudio(RootSound.clickMenuSound, H.Sound);

                //will pop up new menus
                //will pop up new Raws and Elements
                if (Program.MOUSEOVERTHIS.name.Contains("Select_") && Application.loadedLevelName != "Lobby")
                {
                    UponLeftClickOnMenu(Program.MOUSEOVERTHIS.name);
                }
                else if(Application.loadedLevelName != "Lobby")
                {
                    CloseCurrentMenu();
                }
            }
            //click outside the buttons bascially MOUSEOVERTHIS == null...
            else if (Application.loadedLevelName != "Lobby")
            {
                CloseCurrentMenu();
            }
        }
    }

    //Based ont the paramenter will create geometry... the parameter format always have to be the same 
    //used for Raw and Elements
    void UponLeftClickOnMenu(string menuClicked)
    {
        General temp;
        //will split the string in '_', 
        //word we are looking for is the second one: "Cone", "Cube", etc
        //the 4th word : "Raw", "Element"
        string[] split = menuClicked.Split('_');

        Vector3 ini = new Vector3();

        //used to store the initial pos of new models if Player is exisitng 
        if (Program.THEPlayer != null)
        {
            ini = Vector3.MoveTowards(Program.THEPlayer.transform.position,
                -Camera.main.transform.position, 1f);
        }

        if (split[3] == "Raw")
        {
            temp = (Raw)General.Create(Root.ReturnFullPath(split[1]), ini);
        }
        else if (split[3] == "Element")
        {
            temp = (Element)General.Create(Root.ReturnFullPath(split[1]), ini);
        }
        else if (split[3] == "Main" || split[3] == "NewMenu" || menuClicked == "RightClicked") 
        {
            CloseCurrentMenu();
            SpawnMenu(menuClicked, null, null, true);
        }
        else
        {
            print("Error in naming should be: Select_Bomb_Btn_Element_3dMenu");
        }
    }

    //contains the actions for each button


    //spawn menus base on string passed 
    public void SpawnMenu(string whichMenu, Transform[] spawnPoints, Transform[] showPoints, bool isUnevenRow = false, 
        float speedPass = 30f)
    {   
        //we are looking for the a set of obj tht are stored in the list myMenuSets as a list as well
        int listIndex = Root.ReturnListNumber(whichMenu);
        int count = Root.myMenuSets[listIndex].Count - 1; /*bz last one is spawner*/
        int max = Root.myMenuSets[listIndex].Count-2;//use it for find the middle one 

        for (int i = 0; i < count; i++)
        {
            print(listIndex + ".listIndex");
            print(count + ".count");
            print(spawnPoints[0].name+".name");

            menusArrayOne[i] = (Btn3D)CreateBtn(menusArrayOne[i], Root.myMenuSets[listIndex][i],
                spawnPoints[GiveRandom(0, spawnPoints.Length)].position, speedPass);

            if (isUnevenRow)
            {
                if (count == 3)//if is a 3 objects menu
                {
                    menusArrayOne[i].MovingToIndex = i + 1;
                    menusArrayOne[i].MovingToPosition = showPoints[i + 1].position;
                    menusArrayOne[i].Type = "TopUnEvenRow";
                }
                else if (count == 5)//if is a 5 objects menu
                {
                    menusArrayOne[i].MovingToIndex = i;
                    menusArrayOne[i].MovingToPosition = showPoints[i].position;
                    menusArrayOne[i].Type = "TopUnEvenRow";
                }
            }
            else if (!isUnevenRow)
            {
                menusArrayOne[i].MovingToIndex = i;
                menusArrayOne[i].MovingToPosition = showPoints[i].position;
                menusArrayOne[i].Type = "Main";
            }
        }
        if (isUnevenRow)
        {
            menusArrayOne[GiveMiddle(0, max)].name = menusArrayOne[GiveMiddle(0, max)].name + ".Middle3dMenu";
        }
        AssignInitialPos(showPoints);
        MenuHandler.CREATEMENU = false;
    }

    public int GiveMiddle(int first, int last)//mus be an odd number max
    {
        double d = (double)last / 2;
        string s = d.ToString("n0");
        first = int.Parse(s);
        return first;
    }

    void CloseCurrentMenu(bool forceP = false)
    {
        if (!Settings.ISPAUSED || forceP)
        {
            for (int i = 0; i < menusArrayOne.Length; i++)
            {
                if (menusArrayOne[i] != null)
                {
                    //will assing this InitialPosition, in Menus.Update() we use it to move to there as fadeout
                    menusArrayOne[i].InitialPosition = new Vector3();
                    menusArrayOne[i].Destroyer();
                    menusArrayOne[i] = null;
                }
            }
            MenuHandler.CREATEMENU = true;
        }
    }

    //will assign InitialPos to Menus 
    void AssignInitialPos(Transform[] passed)
    {
        for (int i = 0; i < menusArrayOne.Length; i++)
        {
            if (menusArrayOne[i] != null)
            {
                menusArrayOne[i].InitialPosition = passed[i].position;
            }
        }
    }

    //will do stuff as we  hover over...
     void HoveringOver()
     {
         if (Program.MOUSEOVERTHIS != null)
         {
             //if we are in lobby 
             if (Application.loadedLevelName == "Lobby" && !isCameraMoving)
             {
                 //Loaidng All Effect of hovering...
                 SpawnText(Camera.main.WorldToViewportPoint(menuSetTip.position));
                 BringForward(Program.MOUSEOVERTHIS);
                 CamControl.CAMFollow.target = ReturnGuideCameraPoint(Program.MOUSEOVERTHIS);
                 //audioPlayer.PlaySoundOneTime(RootSound.hoverMenuSound, H.Sound);

                 if (hoveredTemp != null)
                 {
                     SwitchMaterial(0);
                     //if Program.MOUSEOVERTHIS is not the same as hoveredTemp.transform...
                     if (Program.MOUSEOVERTHIS != hoveredTemp.transform)
                     {
                         BringForward(action: "BackToOrigin");
                         CamControl.CAMFollow.target = menuSetCamGuideNoSelection;
                         SwitchMaterial(1);//Materials
                         //audioPlayer.PlaySoundOneTime(RootSound.hoverMenuSound, H.Sound, reset: true);
                     }
                 }
             }
             else if (Application.loadedLevelName != "Lobby")
             {
                 SpawnText(Program.VIEWPOS);
             }
         }
         else if (Program.MOUSEOVERTHIS == null)
         {
             DestroyTipMenu();
             BringForward(action: "BackToOrigin");
             CamControl.CAMFollow.target = menuSetCamGuideNoSelection;
             SwitchMaterial(1);//Materials
             //audioPlayer.PlaySoundOneTime(RootSound.hoverMenuSound, H.Sound, reset: true);
         }
     }

    //matIndex is the index of the material in the inspector slot 
     void SwitchMaterial(int matIndex)
     {
         if (hoveredTemp != null)
         {
             Material[] mats = hoveredTemp.GetComponent<Renderer>().materials;
             //if is more than one mat in the object 
             if (mats.Length > 1)
             {
                 mats[1] = hoveredTemp.materiales[matIndex];
                 hoveredTemp.GetComponent<Renderer>().materials = mats;
             }
         }
     }

    //will bribg foward an Menu Obj Towards the camera
    void BringForward(Transform transfPass = null, string action = "")
    {
        float step = 0.15f;//setp to brong fprward

        if (toBringForward && transfPass != null)
        {
            for (int i = 0; i < menusArrayOne.Length; i++)
            {
                if (menusArrayOne[i] != null)
                {
                    //and that object in the array was positionedFixed already then 
                    if(menusArrayOne[i].PositionFixed)
                    {
                        //if the  transform object passed contains the name of the menusArrayOne[i]... 
                        if (transfPass.name.Contains(menusArrayOne[i].transform.name))
                        {   //we make hovered temp this...
                            hoveredTemp = menusArrayOne[i];
                        }
                    }
                }
            }
            //if hoevered was assign then 
            if (hoveredTemp != null)
            {   //we will birng it a step tpwards the screen 
                hoveredTemp.MovingToPosition = Vector3.MoveTowards(transfPass.position, Camera.main.transform.position, step);
                toBringForward = false;
            }
        }
        //bring it back to origin
        else if (!toBringForward && action == "BackToOrigin")
        {
            hoveredTemp.MovingToPosition = hoveredTemp.InitialPosition;
            toBringForward = true;
         }
    }
}