﻿using UnityEngine;
using System.Collections;

public class ControllerMain : General {

    public MeshController MeshController;
    public TerrainSpawnerController TerraSpawnController;

    void Start()
    {
        TerraSpawnController = FindObjectOfType<TerrainSpawnerController>();

        if (TerraSpawnController == null)
        {
            TerraSpawnController =
                (TerrainSpawnerController)
                    General.Create(Root.terrainSpawnerController);
        }
        else
        {
            TerraSpawnController.RehuseStart();
        }

    }

    void Update()
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
        //TerraSpawnController.Destroy();
        TerraSpawnController.PrepareToRehuse();

        base.Destroy();
    }

}
