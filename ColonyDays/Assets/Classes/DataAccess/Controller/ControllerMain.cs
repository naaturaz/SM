public class ControllerMain : General
{
    public MeshController MeshController;
    public TerrainSpawnerController TerraSpawnController;

    private void Start()
    {
        TerraSpawnController =
            (TerrainSpawnerController)
                General.Create(Root.terrainSpawnerController);
    }

    private void Update()
    {
        if (CamControl.CAMRTS != null)
        {
            if (MeshController == null)
            {
                MeshController = (MeshController)General.Create(Root.meshController, container: Program.ClassContainer.transform);
            }
        }
    }

    public void Destroy()
    {
        MeshController.Destroy();
        TerraSpawnController.Destroy();

        base.Destroy();
    }
}