using System.Collections.Generic;
using UnityEngine;

public class Animal : General
{

    private Building _spawner;//the building spawn me. where I belong to

    public Building Spawner
    {
        get { return _spawner; }
        set { _spawner = value; }
    }


    // Use this for initialization
	protected void Start ()
    {
	    base.Start();
	}
	
	// Update is called once per frame
	protected void FixedUpdate ()
    {
	    
	}

    /// <summary>
    /// Returns Random position from origin. If fell inside a building will find another spot
    /// until is in a clear zone
    /// If origin is not specified will assume is CamControl.CAMRTS.hitFront.point the center of terrain
    /// </summary>
    /// <param name="howFar">How far will go</param>
    public static Vector3 AssignRandomIniPosition(Vector3 origin, Rect area , float howFar = 0.75f, float animalDim = 0.15f)
    {
        float x = UMath.Random(-howFar, howFar);
        float z = UMath.Random(-howFar, howFar);
        origin = new Vector3(origin.x + x, origin.y, origin.z + z);

        var _bounds = UPoly.CreatePolyFromVector3(origin, animalDim, animalDim);
  
        if (!area.Contains(new Vector2(origin.x, origin.z)))
        {
            origin = AssignRandomIniPosition(origin, area);
        }
        return origin;
    }


    protected void RotateRandomly()
    {
        var rand = Random.Range(0, 361);
        gameObject.transform.Rotate(0, rand, 0);
    }

    protected void MoveToRandomSpot()
    {
        Rect area = Spawner.ReturnInGameObjectZone(H.FarmZone);

        transform.position = AssignRandomIniPosition(transform.position, area);
    }

    protected void SetRandomIdleStart()
    {
        var myAnimator = gameObject.GetComponent<Animator>();
        myAnimator.Play("Idle", 0, Random.Range(0,2));
    }

    public void YieldGoods()
    {
        Spawner.Inventory.Add(P.Beef, 100);
        Spawner.Inventory.Add(P.Leather, 2);
        //        Debug.Log("Yielded Goods");
    }
}
