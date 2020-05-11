//Shpourcut to Builder Functionalietes... Input and Contoller
public class SBu
{
    private InputBuilding _inputBuilder;

    public BuildingController BuilderController
    {
        get { return BuildingPot.Control; }
        set { BuildingPot.Control = value; }
    }

    //public List<Building> CurrBuildBasePlanes
    //{
    //    get { return Program.InputMain.BuilderPot.Control.BuildsList; }
    //    set { Program.InputMain.BuilderPot.Control.BuildsList = value; }
    //}

    //public CreatePlane Plane
    //{
    //    get { return Program.InputMain.BuilderPot.Control.Plane; }
    //    set { Program.InputMain.BuilderPot.Control.Plane = value; }
    //}

    public InputBuilding InputBuilder
    {
        get { return BuildingPot.InputU; }
        set { BuildingPot.InputU = value; }
    }

    public H DoingNow
    {
        get { return BuildingPot.DoingNow; }
        set { BuildingPot.DoingNow = value; }
    }

    public Mode InputMode
    {
        get { return BuildingPot.InputMode; }
        set { BuildingPot.InputMode = value; }
    }

    public Building CurrentSpawnBuild
    {
        get { return BuildingPot.Control.CurrentSpawnBuild; }
        set { BuildingPot.Control.CurrentSpawnBuild = value; }
    }

    //public General Arrow
    //{
    //    get { return Program.InputMain.BuilderPot.Control.CurrentSpawnBuild.Arrow; }
    //    set { Program.InputMain.BuilderPot.Control.CurrentSpawnBuild.Arrow = value; }
    //}
}