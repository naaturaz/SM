using UnityEngine;

public class MyProjector : General
{
    public float height = 10f;
    public float waitTo2ndHeight = 0f;

    public float _initialXRot = 90;
    private Projector engineProjector;

    private float buildingHeight;

    private Vector3 heightCompound;

    //trying to address a Null ref exception on SwitchColor()
    private bool wasInit;

    private float wasCreated;

	// Use this for initialization
	void Start ()
	{
        wasCreated = Time.time;

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

        wasInit = true;
    }

	// Update is called once per frame
	void Update () 
    {
        //to address a Null Ref Excp
	    if (!wasInit)
	    {
	        return;
	    }

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

        if (BuildingPot.Control.CurrentSpawnBuild != null 
            || BuildingPot.Control.Registro.SelectBuilding != null)
        {
            var build = BuildingPot.Control.CurrentSpawnBuild;
            if (build == null)
            {
                build = BuildingPot.Control.Registro.SelectBuilding;
            }
            if (build == null)
            {
                return;
            }

            if (waitTo2ndHeight != 0)
            {
                if (Time.time > wasCreated + waitTo2ndHeight)
                {
                    MoveToThereBuilding(build.transform.position);
                }

            }
	    }

	    
    }

    void MoveToThere(Vector3 to)
    {
        if (transform.position != to)
        {
            transform.position = Vector3.Lerp(transform.position, to + heightCompound,
                0.5f);//0.1
        }
    }

    void MoveToThereBuilding(Vector3 to)
    {
        if (transform.position != to)
        {
            transform.position = Vector3.Lerp(transform.position, to, 1f);//0.2
        }
    }

    public void SwitchColorLight(bool isGood)
    {
        if (!wasInit)
        {
            Start();
        }

        if (engineProjector==null)
        {
            return;
        }

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
