using UnityEngine;
using System.Collections;

public class LineUpHelper : General
{
    private float moveSpeed ;
    private H moveInThisAxis;
    private float moveForThisLong;

    private Vector3 _earthPosition;
    private Vector3 _upInSky;
    private Vector3 _goingTo;

    public Vector3 EarthPosition
    {
        get { return _earthPosition; }
        set { _earthPosition = value; }
    }


    static public LineUpHelper Create(string root, Vector3 origen = new Vector3(), string name = "", Transform container = null,
    H hType = H.None)
    {
        WAKEUP = true;
        LineUpHelper obj = null;
        obj = (LineUpHelper)Resources.Load(root, typeof(LineUpHelper));
        obj = (LineUpHelper)Instantiate(obj, origen, Quaternion.identity);
        obj.HType = hType;
        obj.transform.name = obj.MyId = obj.Rename(obj.transform.name, obj.Id, obj.HType, name);

        obj.EarthPosition = origen;

        if (container != null) { obj.transform.SetParent(container); }
        return obj;
    }

	// Use this for initialization
	void Start ()
	{
	    HideOnSky();
	}

    void HideOnSky()
    {
        _upInSky =new Vector3(transform.position.x, transform.position.y + 30, transform.position.z);
        transform.position = _upInSky;
    }

	// Update is called once per frame
	void Update () {
	
	    MoveThis();
    }

    public void BringToEarth()
    {
        _goingTo = _earthPosition;
        CoolMove(H.Y, .5f);
    }

    public void BackToSky()
    {
        _goingTo = _upInSky;
        CoolMove(H.Y, .5f);
    }

    /// <summary>
    /// DestroyCool ordered
    /// </summary>
    /// <param name="axis">The Axis the game obj will move </param>
    /// <param name="speed">The speed the game obj will move. Positive and Negative</param>
    /// <param name="timeToStay">Time the game obj will be alive</param>
    public void CoolMove(H axis, float speed)
    {
        moveInThisAxis = axis;
        moveSpeed = speed;
    }

    /// <summary>
    /// Move this game object 
    /// </summary>
    void MoveThis()
    {
        //Vector3 tempPos = transform.position;
        moveSpeed *= 1.1f;//so speeds up

        //if (moveInThisAxis == H.X)
        //{ tempPos.x += moveSpeed * Time.deltaTime; }
        //else if (moveInThisAxis == H.Y)
        //{ tempPos.y += moveSpeed * Time.deltaTime; }
        //else if (moveInThisAxis == H.Z)
        //{ tempPos.z += moveSpeed * Time.deltaTime; }

        transform.position = Vector3.MoveTowards(transform.position, _goingTo, moveSpeed);

        if (Vector3.Distance(transform.position, _goingTo) < 0.5)
        {
            Reset();
        }
    }

    private void Reset()
    {
        moveSpeed = 0;
    }
}
