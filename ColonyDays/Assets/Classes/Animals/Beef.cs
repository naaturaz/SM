using UnityEngine;

public class Beef : Animal
{

	// Use this for initialization
	void Start ()
    {
	    base.Start();

	    MoveToRandomSpot();
	    RotateRandomly();
	    SetRandomIdleStart();
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
        obj.gameObject.transform.SetParent(spawner.transform);
        obj.Spawner = spawner;

        return obj;
    }
	
	// Update is called once per frame
	void Update ()
    {
	    base.Update();

	    CheckIfYield();
    }


}
