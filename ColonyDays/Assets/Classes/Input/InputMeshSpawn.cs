using UnityEngine;
using System.Collections.Generic;

public class InputMeshSpawn : InputParent {

    Vector2 firstCorner = new Vector2();
    Vector2 secondCorner = new Vector2();

    private Rect selectionDimRect;
    public Texture2D tex;

    //contains the elements that are selected to be mined
    private List<TerrainRamdonSpawner> toMineSelectList = new List<TerrainRamdonSpawner>();
    List<Selection> currentMineVisHelp = new List<Selection>();

    public List<TerrainRamdonSpawner> ToMineSelectList
    {
        get { return toMineSelectList; }
        set { toMineSelectList = value; }
    }

    // Use this for initialization
    void Start()    {    }

    // Update is called once per frame
    void Update()
    {
        //CuttingSwitch();
        //Cutter();
    }

    #region Mesh Controller
    /// <summary>
    /// Changes the current object to be cut
    /// </summary>
    void CuttingSwitch()
    {
        if (BuildingPot.InputMode == Mode.Cutting)
        {
            if (Input.GetKeyUp(KeyCode.T)) { BuildingPot.DoingNow = H.Tree; }
            if (Input.GetKeyUp(KeyCode.S)) { BuildingPot.DoingNow = H.Stone; }
            if (Input.GetKeyUp(KeyCode.I)) { BuildingPot.DoingNow = H.Iron; }
            if (Input.GetKeyUp(KeyCode.R)) { BuildingPot.DoingNow = H.RemoveSelection; }
        }
    }

    /// <summary>
    /// this is the cutter action will fill up the toMineSelectList depending if we adding or removing items to cut/mine
    /// </summary>
    void Cutter()
    {
        if (BuildingPot.InputMode == Mode.Cutting)
        {
            print("Ready to Mine Stuff");
            if (Input.GetMouseButtonDown(0) && firstCorner == new Vector2())
            {
                ToMineSelectList.Clear();
                firstCorner = new Vector2(UInput.TransformedMPos.x, UInput.TransformedMPos.y);
            }
            else if (Input.GetMouseButton(0) && firstCorner != new Vector2())
            {
                secondCorner = new Vector2(UInput.TransformedMPos.x, UInput.TransformedMPos.y);
                selectionDimRect = UDir.ReturnDragRect(firstCorner, secondCorner);
            }
            else if (Input.GetMouseButtonUp(0) && firstCorner != new Vector2())
            {
                ToMineSelectList = ReturnListOfSpecificObj(BuildingPot.DoingNow);
                firstCorner = new Vector2();
                secondCorner = new Vector2();
                selectionDimRect = new Rect();
            }
        }
    }

    /// <summary>
    /// Returns a list of specific type element (trees, stones, iron)
    /// </summary>
    /// <param name="typePass">type of object expected (trees, stones, iron)</param>
    /// <returns>a list with the specific type of element passed (trees, stones, iron)</returns>
    List<TerrainRamdonSpawner> ReturnListOfSpecificObj(H typePass)
    {
        StillElement still = null;
        List<TerrainRamdonSpawner> res = new List<TerrainRamdonSpawner>();

        for (int i = 0; i < p.TerraSpawnController.AllRandomObjList.Count; i++)
        {
            still = (StillElement)p.TerraSpawnController.AllRandomObjList[i];
            bool isSpawnObjOnSelRect = selectionDimRect.Contains(
                CamControl.CAMRTS.GetComponent<Camera>().WorldToScreenPoint(
                    p.TerraSpawnController.AllRandomObjList[i].transform.position));

            bool isRightType = CheckIfTypeOfObj(typePass, p.TerraSpawnController.AllRandomObjList[i]);

            if (isSpawnObjOnSelRect && isRightType)
            {
                res.Add(still);
                AddVisHelpList(true, still);
            }
            else if (isSpawnObjOnSelRect && typePass == H.RemoveSelection)
            {
                AddVisHelpList(false, still);
            }
        }
        return res;
    }

    /// <summary>
    /// Adds or remove items from the currentMineVisHelp list
    /// </summary>
    /// <param name="isToAdd">is is true we add otherwise remove</param>
    /// <param name="obj">the new obj</param>
    public void AddVisHelpList(bool isToAdd, TerrainRamdonSpawner obj)
    {
        if (isToAdd)
        {
            //print(obj.name + " added to selection");
            currentMineVisHelp.Add(Selection.CreateSelection(Root.selectMine1, obj.transform.position, obj.IndexAllVertex,
               obj.name + " Selection", container: transform));
        }
        else if (!isToAdd && currentMineVisHelp.Count > 0)
        {
            for (int i = 0; i < currentMineVisHelp.Count; i++)
            {
                if (currentMineVisHelp[i].IndexAllVertex == obj.IndexAllVertex)
                {
                    //print(currentMineVisHelp[i].IndexAllVertex + " currentMineVisHelp[i].IndexAllVertex");
                    //print(obj.IndexAllVertex + " obj.IndexAllVertex");
                    currentMineVisHelp[i].Destroy();
                    currentMineVisHelp.RemoveAt(i);
                    i--;
                }
            }
        }
        //updates the needed list on TerraSpawnController
        p.TerraSpawnController.UpdateLocalLists(obj);
    }

    /// <summary>
    /// Evaluates if objpass is same as typepass expected correspondent value
    /// </summary>
    /// <param name="typePass">H enum type pass</param>
    /// <param name="objPass">obj with will be evaluated to see if is that type of class</param>
    /// <returns>true if is the same, false otherwise</returns>
    bool CheckIfTypeOfObj(H typePass, General objPass)
    {
        bool res = false;
        if (typePass == H.Tree)
        {
            if (objPass is TreeVeget) res = true;
        }
        else if (typePass == H.Stone)
        {
            if (objPass is StoneRock) res = true;
        }
        else if (typePass == H.Iron)
        {
            if (objPass is IronRock) res = true;
        }
        return res;
    }

    #endregion

    void OnGUI()
    {
        Rect mapDimRectDraw = U2D.ReturnDrawRectYInverted(selectionDimRect);
        GUI.DrawTexture(mapDimRectDraw, tex);
    }
}
