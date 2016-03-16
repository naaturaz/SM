using UnityEngine;
using System.Collections;

public class FollowObject : General
{

    private GameObject _toFollow;

    public GameObject ToFollow
    {
        get { return _toFollow; }
        set { _toFollow = value; }
    }

    private Vector3 _objOriginalDimensions;

    static public FollowObject Create(string root, GameObject toFollow, Transform container, string owner)
    {
        WAKEUP = true;
        FollowObject obj = null;
        obj = (FollowObject)Resources.Load(root, typeof(FollowObject));
        obj = (FollowObject)Instantiate(obj, new Vector3(), Quaternion.identity);
        // obj.HType = hType;
        obj.transform.name = obj.MyId = obj.Rename(obj.transform.name, obj.Id, obj.HType) + " own:"+owner ;
        obj.ToFollow = toFollow;

        if (container != null) { obj.transform.parent = container; }
        return obj;
    }

    // Use this for initialization
	void Start () {
        _objOriginalDimensions = gameObject.transform.localScale;
	}
	
	// Update is called once per frame
	void Update ()
	{
	    transform.position = _toFollow.transform.position;
	    transform.rotation = _toFollow.transform.rotation;
	}

    /// <summary>
    /// So it gets then scaled down from here . to avoid bugg where was always small bz never came back to normal size
    /// </summary>
    internal void ReloadOriginalObjectDim()
    {
        gameObject.transform.localScale = _objOriginalDimensions;

    }
}
