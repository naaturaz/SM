using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InputMouse : InputParent
{
    private Building oldSelection;

	// Use this for initialization
    public void Start()
    {
        
    }
	
	// Update is called once per frame
    public void Update()
    {
        DebugFullConstructedBuilding();
    }

    public void UnSelectRoutine(List<string> names, Transform clicked)
    {
        bool ifAnyIsTheSame = names.Any(a => a == clicked.name);

        if (Program.MouseListener.CurrForm != null && !Program.MouseListener.CurrForm.name.Contains("Initial_Form"))
        {
            if (!ifAnyIsTheSame && !Program.MouseListener.CurrForm.IsOverLapingPanel(Input.mousePosition))
            {
                UnSelectCurrent();
                Program.MouseListener.DestroyForm();
            }
        }
    }

    //Will get fully built
    void DebugFullConstructedBuilding()
    {
        if (BuildingPot.Control == null)
        {
            return;
        }

        if (BuildingPot.Control.Registro.SelectBuilding != null && Input.GetKeyUp(KeyCode.Z))
        {
            BuildingPot.Control.Registro.SelectBuilding.AddToConstruction(10000);
        }
    }

    /// <summary>
    /// Given the name of an object wil the format 'haha | House | 222'  will tell u the type
    /// </summary>
    public H FindType(string nameP)
    {
        string sep = " | ";//separator
        H res = H.None;

        foreach (H item in Program.gameScene.Book.AllStructures)
        {
            if (nameP.Contains(sep + item + sep))
            {
                res = item;
            }
        }
        return res;
    }

    /// <summary>
    /// Will select at obj based on its key
    /// </summary>
    public void Select(Ca cat, string keyName)
    {
        print("Select() " + cat + ".keyName:" + keyName);
        if (cat == Ca.Structure || cat == Ca.Shore)
        {
            if (BuildingPot.Control.Registro.Structures.Count > 0)
            {
                if (BuildingPot.Control.Registro.Structures.ContainsKey(keyName))
                {
                    BuildingPot.Control.Registro.SelectBuilding = BuildingPot.Control.Registro.Structures[keyName];

                   
                }
            }
        }
        else if (cat == Ca.Way)
        {
            //print("Select().keyName:" + keyName);
            //print("1st MyId:" + BuilderPot.Control.Registro.Ways[BuilderPot.Control.Registro.Ways.Count-1]);
            if (BuildingPot.Control.Registro.Ways.Count > 0)
            {
                if (BuildingPot.Control.Registro.Ways.ContainsKey(keyName))
                {
                    //print("Way Way Selected");
                    BuildingPot.Control.Registro.SelectBuilding = BuildingPot.Control.Registro.Ways[keyName];
                }
            }
        }
        else if (cat == Ca.Spawn)
        {
            if (p.TerraSpawnController.AllRandomObjList.Count > 0)
            {
                if (UKeyColl.CheckIfKeyInColl<TerrainRamdonSpawnerKey, string>(p.TerraSpawnController.AllRandomObjList, keyName))
                {
                    General t = p.TerraSpawnController.AllRandomObjList[keyName];
                    BuildingPot.Control.Registro.SelectBuilding = (Building) t;
                }
            }
        }
        else if (cat == Ca.DraggableSquare)
        {
            //print("Famrs count:" + BuilderPot.Control.Registro.Farms.Count);
            if (BuildingPot.Control.Registro.Farms.Count > 0)
            {
                //print("keyName:" + keyName);
                //print("1st MyId:" + BuilderPot.Control.Registro.Farms[0]);
                if (BuildingPot.Control.Registro.Farms.ContainsKey(keyName))
                {
                    //print("Famrs Famrs Selected");
                    BuildingPot.Control.Registro.SelectBuilding = BuildingPot.Control.Registro.Farms[keyName];
                }
            }
        }


        if (BuildingPot.Control.Registro.SelectBuilding == null)
        {
            BuildingPot.Control.Registro.SelectBuilding =
                BuildingPot.Control.Registro.FindFromToDestroyBuildings(keyName);
        }

        SelectDecisions();
    }

    void SelectDecisions()
    {
        if (BuildingPot.Control.Registro.SelectBuilding == null) { return; }

        if (oldSelection != null)
        {
            //so the old one is unselected 
            if (oldSelection != BuildingPot.Control.Registro.SelectBuilding)
            {
                oldSelection.UnSelectBuilding();
            }
        }

        if (BuildingPot.Control.Registro.SelectBuilding.PositionFixed 
            && BuildingPot.Control.Registro.SelectBuilding.Category != Ca.None
            //&& BuildingPot.InputMode == Mode.None
            )
        {
            SelectCurrent();
            Program.MouseListener.CreateNewForm(H.Selection);
        }
    }

    /// <summary>
    /// Select current object
    /// </summary>
    void SelectCurrent()
    {
        if (BuildingPot.Control.Registro.SelectBuilding == null || BuildingPot.Control.Registro.SelectBuilding == new General())
        { return; }

        if (BuildingPot.Control.Registro.SelectBuilding.Category == Ca.Way ||
            BuildingPot.Control.Registro.SelectBuilding.Category == Ca.DraggableSquare ||
            BuildingPot.Control.Registro.SelectBuilding.Category == Ca.None)
        { return;}

        BuildingPot.Control.Registro.SelectBuilding.CreateProjectorConditionals();
        oldSelection = BuildingPot.Control.Registro.SelectBuilding;
    }
    /// <summary>
    /// Un Select what was the current object
    /// </summary>
    public void UnSelectCurrent()
    {
        if (BuildingPot.Control.Registro.SelectBuilding == null || BuildingPot.Control.Registro.SelectBuilding == new General())
        { return; }

        if (BuildingPot.Control.Registro.SelectBuilding.Category == Ca.Way ||
            BuildingPot.Control.Registro.SelectBuilding.Category == Ca.DraggableSquare ||
            BuildingPot.Control.Registro.SelectBuilding.Category == Ca.None)
        { return; }

        BuildingPot.Control.Registro.SelectBuilding.UnSelectBuilding();
        BuildingPot.Control.Registro.SelectBuilding = null;
    }
}
