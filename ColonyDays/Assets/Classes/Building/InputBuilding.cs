using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

/// <summary>
/// Contains majority of input actions for the buildings
/// </summary>

public class InputBuilding : BuildingPot {

    Vector2 oldMousePos;
    private float timeStart;

    bool _isDraggingWay;
    Dictionary<KeyCode, H> _fInputKeys = new Dictionary<KeyCode, H>();

    //this one has a list with dictionaries.. This dictiories have mapped
    //which key is for each H elemenet. So all buildings are here 
    //Each dictionary will be for the category
    //for ex: List[0] is the dictionary for Roads
    List<Dictionary<KeyCode, H>> _inputListDict = new List<Dictionary<KeyCode, H>>();

    //this will defined the type of building will be biult F1-F...
    //Infra, Prod, House, etc
    H selection = H.None;

    public bool IsDraggingWay
    {
        get { return _isDraggingWay; }
        set { _isDraggingWay = value; }
    }

    public List<Dictionary<KeyCode, H>> InputListDict
    {
        get { return _inputListDict; }
        set { _inputListDict = value; }
    }

    void Start ()
	{
	    oldMousePos = Input.mousePosition;
	    timeStart = Time.time;

        InputKeyMapping();
    }

    void InputKeyMapping()
    {
        List<KeyCode> numbers = new List<KeyCode>()
        {
            KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4,
            KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Alpha0
        };

        List<KeyCode> fKeys = new List<KeyCode>()
        {
            KeyCode.F1, KeyCode.F2, KeyCode.F3, KeyCode.F4, KeyCode.F5, KeyCode.F6, KeyCode.F7, 
            KeyCode.F8, KeyCode.F9, KeyCode.F10
        };

        int count = 0;
        //mapping all buildings Structures Categories  to F keys... for eg:
        //KeyCode.F1, Infrastructure
        //KeyCode.F2, Housing
        foreach (KeyValuePair<H, List<H>> entry in Program.gameScene.Book.StructuresDict)
        {
            //entry cant have more items than FKeys
            // do something with entry.Value or entry.Key
            _fInputKeys.Add(fKeys[count], entry.Key);
            count++;
        }

        //mapping all buildings to numbers keys... for eg:
        //KeyCode.Alpha1, Trail
        //KeyCode.Alpha2, Road
        foreach (KeyValuePair<H, List<H>> entry in Program.gameScene.Book.StructuresDict)
        {
            Dictionary<KeyCode, H> _inputKeys = new Dictionary<KeyCode, H>();
            // do something with entry.Value or entry.Key
            //value cant have more items than numbers
            for (int i = 0; i < entry.Value.Count; i++)
            {
                _inputKeys.Add(numbers[i], entry.Value[i]);
            }
            _inputListDict.Add(_inputKeys);
        }
    }
	
	void Update () 
    {
	    BuildingSwitch();
        if (Time.time > timeStart + 0.01)
        {
            oldMousePos = Input.mousePosition;
            timeStart = Time.time;
        }
    }

    /// <summary>
    /// Depending on the current InputMode will direct the code to  BuildingMode(); or
    /// Structure.Place, DrawWay(), DragFarm(
    /// </summary>
    public void BuildingSwitch(H val = H.None)
    {
        //Building mode
        if (InputMode == Mode.Building)
        {
            //with keyboard
            if (val == H.None)
            {
                BuildingMode();
            }
            //with GUI buttons
            else 
            {
                BuildNowNew(val);
            }
        }
        //Placing mode
        else if (InputMode == Mode.Placing)
        {
            //Vector3 iniPos = m.HitMouseOnTerrain.point;
            //var onMap = CamControl.CAMRTS.MiniMapRts.IsOnMapConstraints(iniPos);

            //if (!onMap)
            //{
            //    return;
            //}


            //Screen.showCursor = false;
            //Structures
            if (Control.CurrentSpawnBuild.Category == Ca.Structure || Control.CurrentSpawnBuild.Category == Ca.Shore)
            {
                Structure str = Control.CurrentSpawnBuild as Structure;
                str.UpdateClosestVertexAndOld();
            }

            //Ways and Farm
            if (Control.CurrentSpawnBuild.Category == Ca.Way || Control.CurrentSpawnBuild.Category == Ca.DraggableSquare)
            {
                UpdateWayFarmCursor();
                if (Control.CurrentSpawnBuild.Category == Ca.Way)
                {
                    DrawWay();
                }
                else if (DefineCategory(DoingNow) == Ca.DraggableSquare)
                {
                    DragFarm();
                }
            }

            //Structures
            if (Input.GetKeyUp(KeyCode.R) && (DefineCategory(DoingNow) == Ca.Structure || (DefineCategory(DoingNow) == Ca.Shore)))
            {
                Control.CurrentSpawnBuild.RotationAction();
                _audioPlayer.PlaySoundOneTime(RootSound.hoverMenuSound);
            }
            else if (Input.GetMouseButtonUp(0) && !_isDraggingWay && (DefineCategory(DoingNow) == Ca.Structure || (DefineCategory(DoingNow) == Ca.Shore)))
            {
                MouseUp();
            }
        }
    }

    /// <summary>
    /// Updates the cursor of a farm
    /// </summary>
    void UpdateWayFarmCursor()
    {
        //updating current vertex which is used in Way.cs
        if (Control.BuildWayCursor != null) 
        {
            Control.BuildWayCursor.UpdateCursor();
        }
    }

    /// <summary>
    /// This gets call when user is dreaggin a Farm object. 
    /// </summary>
    void DragFarm()
    {
        DragSquare farm = Control.CurrentSpawnBuild as DragSquare;

        if (Input.GetMouseButtonUp(0) && !_isDraggingWay)
        {
            _isDraggingWay = true;
        }
        
        if (_isDraggingWay)
        {
            farm.Drag(); 
        }
        
        if (_isDraggingWay && Input.GetMouseButtonUp(0) && farm.IsBuildOk && farm.IsFarmOk)
        {
            //print("DragFarm() MouseUp() farm");
            IsDraggingWay = false;
            MouseUp();
        }
        //else if (!farm.IsFarmOk && _isDraggingWay && Input.GetMouseButtonUp(0))
        else if (!farm.IsFarmOk)
        {
            print("cant place farm here ");
        }
    }

    /// <summary>
    /// Draggin a Way , all below HTypes
    /// DoingNow == H.Trail  || DoingNow == H.BridgeTrail || DoingNow == H.BridgeRoad)
    /// </summary>
    void DrawWay()
    {
        Trail way = Control.CurrentSpawnBuild as Trail;
        
        if (Input.GetMouseButtonUp(0) && !_isDraggingWay)
        {
            _isDraggingWay = true;
        }
        if (_isDraggingWay)
        {
            if (way.CurrentLoop == H.None) { way.Drag(); }
        }
        if (_isDraggingWay && Input.GetMouseButtonUp(0) && way.IsWayOk)
        {
            MouseUp();
            IsDraggingWay = false;
        }
    }

    /// <summary>
    /// Routine when mouse is clicked 
    /// </summary>
    public void MouseUp()
    {
        if (Control.CurrentSpawnBuild.Category == Ca.Structure || Control.CurrentSpawnBuild.Category == Ca.Shore)
        {
            Structure h = Control.CurrentSpawnBuild as Structure;
            h.DonePlace();
            if (h.PositionFixed)
            {
                BuildNowNew(DoingNow);
            }
        }
        else if (Control.CurrentSpawnBuild.Category == Ca.Way)
        {
            DoneWayRoutine();
        }
        else if (Control.CurrentSpawnBuild.Category == Ca.DraggableSquare)
        {
            DoneFarmRoutine();
        }
        _audioPlayer.PlaySoundOneTime(RootSound.hoverMenuSound);
    }

    /// <summary>
    /// Is being called from this class on  public void MouseUp() and from the Farm instance 
    /// bz we need to prevent from building a new one before the old obj was added to registro.
    /// So the 1st call is expected to do the first if... the second call is expected to comply with the 2nd if
    /// </summary>
    public void DoneFarmRoutine()
    {
        DragSquare f = Control.CurrentSpawnBuild as DragSquare;
        //if the farm is ok and farm is not being added to refistro
        if (f.IsFarmOk && !Control.Registro.Farms.ContainsKey(Control.CurrentSpawnBuild.MyId))
        {
            f.FinishPlacingFarm();
        }
        //is here so we prevent from building a new one before the old obj was added to registro
        else if (f.IsFarmOk && Control.Registro.Farms.ContainsKey(Control.CurrentSpawnBuild.MyId))
        {
            BuildNowNew(DoingNow);
        }
    }

    /// <summary>
    /// Is called from MouseUp() and if all is good will get the way done 
    /// </summary>
    public void DoneWayRoutine()
    {
        Trail h = Control.CurrentSpawnBuild as Trail;
        if (!h.PositionFixed && h.CurrentLoop == H.None)
        {
            h.DonePlace();
        }
        else if (!h.PositionFixed && h.CurrentLoop != H.None)
        {
            GameScene.ScreenPrint("Still building coop pls");
        }
        //print("doingnow H.trail cLoop:" + h.CurrentLoop + "PFix: " + h.PositionFixed);
        if (h.PositionFixed && h.CurrentLoop == H.Done)
        {
            //tgis need to be called here before the new bridge is creted
            if (h.HType.ToString().Contains("Bridge"))
            {
                Bridge b = Control.CurrentSpawnBuild as Bridge;
                b.CreatePartsRoutine();
            }

            _isDraggingWay = false;
            GameScene.ScreenPrint("new Way auto");
            BuildNowNew(DoingNow);
        }
    }


   /// <summary>
   /// This is where the selection of a new building to be built happen
   /// </summary>
    void BuildingMode()
    {
        foreach (var item in _fInputKeys)
        {
            if (Input.GetKeyUp(item.Key))
            {
                //selection of the type of Building is goonna be build 
                selection = item.Value;
                //print(selection + ".");
            }
        }

        //if something was selected 
        if (selection != H.None)
        {
            //will find what numbers is that selection in Book.MenuGroupsList
            int indexSelection = -1;
            if (indexSelection == -1)
            {
                for (int i = 0; i < Program.gameScene.Book.MenuGroupsList.Count; i++)
                {
                    if (selection == Program.gameScene.Book.MenuGroupsList[i])
                    {
                        indexSelection = i;
                    }
                }
            }
            
            if (indexSelection != -1)
            {
                //then in that list whaterever key is pressed will be pass for buildNow()
                foreach (var item in _inputListDict[indexSelection])
                {
                    if (Input.GetKeyUp(item.Key))
                    {
                        //print(indexSelection + ".indexSelection");
                        BuildNowNew(item.Value);
                        //print(selection + "."+item.Value );
                    }
                }
            }
        }
    }

    /// <summary>
    /// Builds all the new Buildings .. The buildinga are loaded thue this method 
    /// </summary>
    public void BuildNowNew(H buildWhat)
    {
        CleanCurrentSpawnBuildIfNewBuildIsADiffType(buildWhat);
        Vector3 iniPos = m.HitMouseOnTerrain.point;

        var onMap = CamControl.CAMRTS.MiniMapRts.IsOnMapConstraints(iniPos);
        iniPos = CamControl.CAMRTS.MiniMapRts.ConstrainLimits(iniPos);


        if (DefineCategory(buildWhat) == Ca.Structure || DefineCategory(buildWhat) == Ca.Shore)
        {
            BuildStructure( buildWhat,  iniPos);
        }
        else if (DefineCategory(buildWhat) == Ca.Way)
        {
            BuildWay(buildWhat, iniPos);
            CreateOrKillCursor();
        }
        else if (DefineCategory(buildWhat) == Ca.DraggableSquare)
        {
            BuildDragSquare(buildWhat, iniPos);
            CreateOrKillCursor();
        }

        DoingNow = buildWhat;
        _audioPlayer.PlaySoundOneTime(RootSound.clickMenuSound);
        GameScene.ScreenPrint("Ready to build a " + buildWhat);
        InputMode = Mode.Placing;
    }

    /// <summary>
    /// Cleans CurrentSpawnBuild If buildWhat is a Is A Diff Type of what is in CurrentSpawnBuild.
    /// This is creeated to fixing bugg where new obj will be hainging there bz was never destroyed
    /// </summary>
    void CleanCurrentSpawnBuildIfNewBuildIsADiffType(H buildWhat)
    {
        if(Control.CurrentSpawnBuild == null){return;}
        if (buildWhat != Control.CurrentSpawnBuild.HType)
        {
            Control.CurrentSpawnBuild.Destroy();
        }
    }

    #region Building the 3 types of posible buildins
    private void BuildDragSquare(H buildWhat, Vector3 iniPos)
    {
        Control.CurrentSpawnBuild = Way.CreateWayObj(Root.farm, iniPos,
            previewObjRoot: Root.previewTrail, hType: buildWhat, materialKey: buildWhat + "." + Ma.matBuildBase
            , container: Program.BuildsContainer.transform);
    }

    void BuildStructure(H buildWhat, Vector3 iniPos)
    {
        Control.CurrentSpawnBuild = Building.CreateBuild(Root.RetBuildingRoot(buildWhat), iniPos, buildWhat,
            materialKey: buildWhat + "." + Ma.matBuildBase
            , container: Program.BuildsContainer.transform);

        AssignSharedMaterial();
    }

    void BuildWay(H buildWhat, Vector3 iniPos)
    {
        if (buildWhat == H.Trail)
        {
            Control.CurrentSpawnBuild = (Trail)Way.CreateWayObj(Root.trail, iniPos,
                previewObjRoot: Root.previewTrail, hType: H.Trail, materialKey: H.Trail + "." + Ma.matBuildBase
                , container: Program.BuildsContainer.transform);
        }
        //else if (buildWhat == H.Road)
        //{
        //    Control.CurrentSpawnBuild = (Trail)
        //            Way.CreateWayObj(Root.trail, iniPos, previewObjRoot: Root.previewRoad,
        //                hType: H.Road, wideSquare: 5, radius: 5f, planeScale: 0.11f, maxStepsWay: 20,
        //                materialKey: H.Road + "." + Ma.matBuildBase);
        //}
        else if (buildWhat == H.BridgeTrail)
        {
            Control.CurrentSpawnBuild = (Bridge)
                    Way.CreateWayObj(Root.bridge, iniPos, previewObjRoot: Root.previewTrail,
                        hType: H.BridgeTrail, materialKey: H.BridgeTrail + "." + Ma.matBuildBase
                        , container: Program.BuildsContainer.transform);
        }
        else if (buildWhat == H.BridgeRoad)
        {
            Control.CurrentSpawnBuild = (Bridge)
                    Way.CreateWayObj(Root.bridge, iniPos, previewObjRoot: Root.previewRoad,
                        hType: H.BridgeRoad, wideSquare: 5, radius: 5f, planeScale: 0.11f, maxStepsWay: 20,
                        materialKey: H.BridgeRoad + "." + Ma.matBuildBase
                        , container: Program.BuildsContainer.transform);
        }

        //AssignSharedMaterial();
    }

    /// <summary>
    /// Will asign a shared material to the Control.CurrentSpawnBuild
    /// </summary>
    void AssignSharedMaterial()
    {
        Material n = Resources.Load<Material>(Root.RetMaterialRoot(Control.CurrentSpawnBuild.MaterialKey));
        n.name = "BaseOri";

        //Control.CurrentSpawnBuild.Geometry.GetComponent<Renderer>().sharedMaterial = n;
    }
    #endregion

    /// <summary>
    /// This create or destroy the create plane that the user can see where the new Way is gonna
    /// be created
    /// </summary>
    public void CreateOrKillCursor()
    {
        KillCursor();
      
        Control.BuildWayCursor = (BigBoxPrev)CreatePlane.CreatePlan(Root.bigBoxPrev, Root.matGreenSel2);
        Control.BuildWayCursor.transform.name = "Cursor for:" + Control.CurrentSpawnBuild.MyId;
    }

    public void KillCursor()
    {
        if (Control.BuildWayCursor != null)
        {
            Control.BuildWayCursor.Destroy();
            Control.BuildWayCursor = null;
        }
    }
}
