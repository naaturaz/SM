using UnityEngine;

public class CollideWith : MonoBehaviour
{

    private bool _isCollidingNow;

    public bool IsCollidingNow
    {
        get { return _isCollidingNow; }
        set { _isCollidingNow = value; }
    }

    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        var p = other.transform.GetComponent<CollideWith>();

        print("colliding with stuff");

        if (p != null)
        {
            print("colliding iwth person now");
            _isCollidingNow = true;
        }
        else _isCollidingNow = false;
    }
}
