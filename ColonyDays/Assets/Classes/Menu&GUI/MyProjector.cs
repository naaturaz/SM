using UnityEngine;

public class MyProjector : General
{
    float height = 10f;

    private float _initialXRot = 90;
    private Projector engineProjector;

    private float buildingHeight;

    private Vector3 heightCompound;

	// Use this for initialization
	void Start ()
	{
	    engineProjector = GetComponent<Projector>();

	    //initialColor = engineProjector.material.color;
        transform.Rotate(new Vector3(_initialXRot, 0, 0));

        Initialize();
	}

    void Initialize()
    {
        if (BuildingPot.Control.CurrentSpawnBuild != null)
        {
            buildingHeight = BuildingPot.Control.CurrentSpawnBuild.Max.y - BuildingPot.Control.CurrentSpawnBuild.Min.y;
        }
        else if (BuildingPot.Control.Registro.SelectBuilding != null)
        {
            buildingHeight = BuildingPot.Control.Registro.SelectBuilding.Max.y -
                BuildingPot.Control.Registro.SelectBuilding.Min.y;
        }
        heightCompound = new Vector3(0, (height + buildingHeight) * 1.5f, 0);
    }

	// Update is called once per frame
	void Update () 
    {
        if (BuildingPot.Control.CurrentSpawnBuild != null) 
        {
            MoveToThere(BuildingPot.Control.CurrentSpawnBuild.ClosestSubMeshVert);
        }
	    else if (BuildingPot.Control.Registro.SelectBuilding != null)
	    {
            MoveToThere(BuildingPot.Control.Registro.SelectBuilding.transform.position);
	    }

	    if (BuildingPot.Control.CurrentSpawnBuild == null && BuildingPot.Control.Registro.SelectBuilding == null)
	    {
	        SwitchColorLight(true);
            Destroy();
	    }
    }

    void MoveToThere(Vector3 to)
    {
        if (transform.position != to)
        {
            transform.position = Vector3.Lerp(transform.position, to + heightCompound,
                0.1f);
        }
    }

    public void SwitchColorLight(bool isGood)
    {
        if (isGood)
        {
            engineProjector.material.color = Color.white;
        }
        else
        {
            engineProjector.material.color = Color.red;
        }
    }
}
