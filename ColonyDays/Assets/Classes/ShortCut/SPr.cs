//Will containt properties that will reference directyl to Terreno.MeshController.variable

//ShortCut to Program... This is the general Shorcuts
public class SPr
{
    public MeshController MeshController
    {
        get { return Program.gameScene.controllerMain.MeshController; }
        set { Program.gameScene.controllerMain.MeshController = value; }
    }

    public TerrainSpawnerController TerraSpawnController
    {
        get { return Program.gameScene.controllerMain.TerraSpawnController; }
        set { Program.gameScene.controllerMain.TerraSpawnController = value; }
    }
}