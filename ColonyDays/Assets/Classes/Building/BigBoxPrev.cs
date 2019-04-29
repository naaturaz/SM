using UnityEngine;

//This calss Prefab is exactly the same as CreatePlane just with diff classes added 
//this class was initillly created only to see ifi can reduce createPlanes drw calls

public class BigBoxPrev : CreatePlane {
    
    private bool isToBeCoolDestroyed;
    private float moveTimeStap;
    private float moveForThisLong;
    private float moveSpeed;
    private H moveInThisAxis;

    private int prevWideSquares = 1;

    public bool IsToBeCoolDestroyed
    {
        get { return isToBeCoolDestroyed; }
        set { isToBeCoolDestroyed = value; }
    }

	// Use this for initialization
	void Start () 
    {
	    base.Start();

        //if (BuildingPot.Control.CurrentSpawnBuild.HType == H.BridgeRoad)
        //{
        //    prevWideSquares = 5;
        //}
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (IsToBeCoolDestroyed) { DestroyCoolMoveFirstRoutine(); }
	    CorrectPreviewSize();
    }

    /// <summary>
    /// Created so the build of a road is not alittle bit off wheen is to large
    /// </summary>
    void CorrectPreviewSize()
    {
        if (corretMinimuScaleOnBigBox)
        {
            corretMinimuScaleOnBigBox = false;
            PlaneGeometry.transform.localScale *= 1.04f;
        }
    }

    /// <summary>
    /// Check what is current color if is initial color or red and with condition will 
    /// switch current game obj color
    /// </summary>
    /// <param name="condition">condition true = initial_color, condition false = red</param>
    public void CheckAndSwitchColor(bool condition)
    {
        if (PlaneGeometry.GetComponent<Renderer>().material.color == InitialColor && !condition)
        {
            PlaneGeometry.GetComponent<Renderer>().material.color = Color.red;
        }
        else if (PlaneGeometry.GetComponent<Renderer>().material.color == Color.red && condition)
        {
            PlaneGeometry.GetComponent<Renderer>().material.color = InitialColor;
        }

        if (BuildingPot.Control == null) { return; }
        if (BuildingPot.Control.CurrentSpawnBuild == null) { return; }
        if (BuildingPot.Control.CurrentSpawnBuild.Projector == null) { return; }
        BuildingPot.Control.CurrentSpawnBuild.Projector.SwitchColorLight(condition);
    }

    /// <summary>
    /// DestroyCool ordered
    /// </summary>
    /// <param name="axis">The Axis the game obj will move </param>
    /// <param name="speed">The speed the game obj will move. Positive and Negative</param>
    /// <param name="timeToStay">Time the game obj will be alive</param>
    public void DestroyCoolMoveFirst(H axis, float speed, float timeToStay)
    {
        moveInThisAxis = axis;
        moveTimeStap = Time.time;
        moveSpeed = speed;
        moveForThisLong = timeToStay;
        IsToBeCoolDestroyed = true;
    }

    /// <summary>
    /// The routine for the Destroy Cool
    /// </summary>
    void DestroyCoolMoveFirstRoutine()
    {
        MoveThis();
        CheckOnTime();
    }

    /// <summary>
    /// Move this game object 
    /// </summary>
    void MoveThis()
    {
        Vector3 tempPos = transform.position;
        moveSpeed *= 1.5f;//so speeds up

        if (moveInThisAxis == H.X)
        {tempPos.x += moveSpeed * Time.deltaTime;}
        else if (moveInThisAxis == H.Y)
        { tempPos.y += moveSpeed * Time.deltaTime; }
        else if (moveInThisAxis == H.Z)
        { tempPos.z += moveSpeed * Time.deltaTime; }

        transform.position = Vector3.Lerp(transform.position, tempPos, 0.5f);
    }

    /// <summary>
    /// Checks on the time passed and will destroy gameobj if has passed
    /// </summary>
    void CheckOnTime()
    {
        if (Time.time > moveTimeStap + moveForThisLong)
        {
            IsToBeCoolDestroyed = false;
            Destroy();
        }
    }

    /// <summary>
    /// Updates the Big Box when is behaving as a Cursor 
    /// </summary>
    public void UpdateCursor()
    {
        //update submesh vert
        UpdateClosestSubMeshVert();
        if (UMath.nearEqualByDistance(ClosestVertOld, ClosestSubMeshVert, 0.01f)) { return; }
        UpdateClosestVertexAndOld();

        //creates poly
        var locPoly = UPoly.CreateSubMeshPoly(ClosestSubMeshVert, prevWideSquares);
        locPoly = UPoly.ScalePoly(locPoly, -0.04f);//a bit smaller is not colliding with every edge

        UpdatePos(locPoly, 0.75f);

        bool isEven = AreAllPointsEven(locPoly, 0.01f);
        bool isCollide = CheckIfColliding(locPoly);
        bool isOnFloor = IsOnTheFloor(locPoly);

        //if is even not coll and is on floor
        CheckAndSwitchColor(isEven && !isCollide && isOnFloor);
    }
}
