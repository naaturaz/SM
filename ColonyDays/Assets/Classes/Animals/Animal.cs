using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Animal : General
{
    private Building _spawner;//the building spawn me. where I belong to


    private int _lastYearYielded;//says the last year Yielded for the Farm some Goods


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

    private static int count;
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
            count++;
            if (count>1000)
            {
                throw new Exception("AssignRandomIniPosition() animal.cs");
            }


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
        //myAnimator.Play("Idle", 0, Random.Range(0,2));
        myAnimator.Play("Idle");
    }

    /// <summary>
    /// Is multipliued by the amout of peopel working there so if non is working now will be 
    /// Yieled 
    /// </summary>
    public void YieldGoods()
    {
        if (_spawner.CurrentProd.Product == P.Beef)
        {
            AddToBuildingInvIfHasRawResourceToProduceIt(P.Beef, 10 * Spawner.PeopleDict.Count/ 4);
            AddToBuildingInvIfHasRawResourceToProduceIt(P.Leather, 1 * Spawner.PeopleDict.Count/ 4);
        }
        if (_spawner.CurrentProd.Product == P.Chicken)
        {
            AddToBuildingInvIfHasRawResourceToProduceIt(P.Chicken, 5 * Spawner.PeopleDict.Count/ 4);
            AddToBuildingInvIfHasRawResourceToProduceIt(P.Egg, 10 * Spawner.PeopleDict.Count/ 4);
        }
        if (_spawner.CurrentProd.Product == P.Pork)
        {
            AddToBuildingInvIfHasRawResourceToProduceIt(P.Pork, 15 * Spawner.PeopleDict.Count/ 4);
        }
    }

    /// <summary>
    /// Will added to the Building inventory if the building has the Raw Resources to produce this good
    /// 
    /// done it so Leather and Eggs for example can be Produced if no Raw inventory existed 
    /// </summary>
    /// <param name="prod"></param>
    /// <param name="amt"></param>
    void AddToBuildingInvIfHasRawResourceToProduceIt(P prod, float amt)
    {
        var itCan = BuildingPot.Control.ProductionProp.DoIHaveEnoughOnInvToProdThis(Spawner, prod);
        
        if (itCan)
        {
            Spawner.Inventory.Add(prod, amt);
        }
    }

    /// <summary>
    /// Yields on Dec of each year 
    /// </summary>
    protected void CheckIfYield()
    {
        //yields on Dec
        if (_lastYearYielded != Program.gameScene.GameTime1.Year && Program.gameScene.GameTime1.Month1 == 12)
        {
            _lastYearYielded = Program.gameScene.GameTime1.Year;
            var st = (Structure) Spawner;

            //if farm Inventory is not full
            //and Building is done
            //and I have input to produce this 
            if (!Spawner.Inventory.IsFull() && (st.CurrentStage == 4 || st.StartingStage == H.Done))
            {
                YieldGoods();
            }
            else
            {
                //todo Notify
                Debug.Log("Not Producing meat(beef) bz Inv is full:"+Spawner.MyId+". evac Orders added ");
                Spawner.AddEvacuationOrderOfProdThatAreNotInput();
            }
        }
    }

}
