using UnityEngine;
using System.Collections;

public class Passive : MonoBehaviour {

    float destroyStartTime;
    public bool isTimeDestroy = true;
    public float timedDestroyInSec = 5f;

    static public Passive Create(string root, Vector3 origen = new Vector3())
    {
        Passive obj = null;
        obj = (Passive)Resources.Load(root, typeof(Passive));
        obj = (Passive)Instantiate(obj, origen, Quaternion.identity);
        return obj;
    }

	// Use this for initialization
	void Start () {
        destroyStartTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {

        if (isTimeDestroy) { AutoDestroy(); }
	}

    void AutoDestroy()
    {
        if (Time.time > destroyStartTime + timedDestroyInSec)
        {
            Destroy(gameObject);
        }
    }
}
