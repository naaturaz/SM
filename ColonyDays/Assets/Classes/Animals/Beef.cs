using UnityEngine;

public class Beef : Animal
{
    private int _lastYearYielded;//says the last year Yielded for the Farm some Goods

	// Use this for initialization
	void Start ()
    {
	    base.Start();

	    MoveToRandomSpot();
	    RotateRandomly();
	    //SetRandomIdleStart();
    }




    /// <summary>
    /// Intended to be used For the first load of people spawned
    /// </summary>
    static public Beef CreateBeef(Vector3 iniPos, Building spawner)
    {
        Beef obj = null;

        obj = (Beef)Resources.Load(Root.beefMale1, typeof(Beef));

        obj = (Beef)Instantiate(obj, iniPos, Quaternion.identity);
        obj.Geometry.GetComponent<Renderer>().sharedMaterial = Resources.Load(Root.beefMat1) as Material;
        obj.gameObject.transform.parent = spawner.transform;
        obj.Spawner = spawner;

        return obj;
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
	    base.FixedUpdate();

	    CheckIfYield();
    }

    private void CheckIfYield()
    {
        //yields on Dec
        if (_lastYearYielded != Program.gameScene.GameTime1.Year && Program.gameScene.GameTime1.Month1 == 12)
        {
            _lastYearYielded = Program.gameScene.GameTime1.Year;

            //if farm Inventory is not full
            if (!Spawner.Inventory.IsFull())
            {
                YieldGoods();
            }
            else
            {
                //todo Notify
                Debug.Log("Not Producing meat(beef) bz Inv is full:"+Spawner.MyId);
            }
        }
    }

    private void YieldGoods()
    {
        Spawner.Inventory.Add(P.Beef, 100);
        Spawner.Inventory.Add(P.Leather, 2);
//        Debug.Log("Yielded Goods");
    }
}
