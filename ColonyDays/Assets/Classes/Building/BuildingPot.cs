/*  This class is the pot is the container for all the main elements of buildings
 *  Contains the controller that controls the input, the control, and the saveLoad functions
 */

using UnityEngine;
using System.Collections;

//This class is instantiatedon InputMain and has a real obj on scene so the start and update works

public class BuildingPot : Pot 
{
    private static InputBuilding _input;
    private static BuildingController _control;
    private static BuildingSaveLoad _saveLoad = new BuildingSaveLoad();
    private static bool isToLoadBuildings;

    private static UnlockBuilds _unlockBuilds;

    public static InputBuilding InputU
    {
        get { return _input; }
        set { _input = value; }
    }

    public static BuildingController Control
    {
        get { return _control; }
        set { _control = value; }
    }

    public static BuildingSaveLoad SaveLoad
    {
        get { return _saveLoad; }
        set { _saveLoad = value; }
    }

    /// <summary>
    /// None, or Building or Placing 
    /// </summary>
    public static Mode InputMode { get; set; }
   
    /// <summary>
    /// Current building that is being built. The one is being hovered right now
    /// </summary>
    public static H DoingNow { get; set; }

    public static UnlockBuilds UnlockBuilds1
    {
        get { return _unlockBuilds; }
        set { _unlockBuilds = value; }
    }

    private void Start()
    {
        _input = (InputBuilding)Create(Root.inputBuilder, container: Program.ClassContainer.transform);
        StartCoroutine("OneMinUpdate");
    }


    private IEnumerator OneMinUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(60); // wait
            //UnlockBuilds1.UpdateBuildsStatuses();
        }
    }


    private void Update()
    {
        if (Control == null)
        {
            _control = (BuildingController)Create(Root.builderController, container: Program.ClassContainer.transform);
        }

        //load the buildings

        //PersonController.CrystalManager1.IsFullyLoaded() needs to be here so when loading the buildings
        //they can get theyir landingZone

        if (Control != null && isToLoadBuildings && MeshController.CrystalManager1.IsFullyLoaded())
        {
            isToLoadBuildings = false;
            _saveLoad.Load();
        }

        _saveLoad.Update();
    }

    public static void CreateUnlockBuilds()
    {
        _unlockBuilds = new UnlockBuilds();
    }

    /// <summary>
    /// will make load buildins 
    /// </summary>
    public static void LoadBuildingsNow()
    {
        isToLoadBuildings = true;
    }

    public static bool FullyLoaded()
    {
        if (Control == null || Control.Registro == null)
        {
            return false;
        }
        return Control.Registro.IsFullyLoaded;
    }
}
