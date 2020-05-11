using UnityEngine;

public class Passive : MonoBehaviour
{
    private float destroyStartTime;
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
    private void Start()
    {
        destroyStartTime = Time.time;
    }

    // Update is called once per frame
    private void Update()
    {
        if (isTimeDestroy) { AutoDestroy(); }
    }

    private void AutoDestroy()
    {
        if (Time.time > destroyStartTime + timedDestroyInSec)
        {
            Destroy(gameObject);
        }
    }
}