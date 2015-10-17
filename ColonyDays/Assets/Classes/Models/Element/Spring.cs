using UnityEngine;
using System.Collections;

public class Spring : Element {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        base.Update();
        //SpringIt();
	}

    //void SpringIt()
    //{
    //    Vector3 originOffSet = new Vector3(0, 0.45f, 0);
    //    Vector3 jump = new Vector3(0, 10f, 0);
    //    Debug.DrawRay(transform.position + originOffSet, transform.up, Color.green, 20f);

    //    RaycastHit hitUp;
    //    Ray rayUp = new Ray(transform.position + originOffSet, transform.up);
    //    if (Physics.Raycast(rayUp, out hitUp))
    //    {

    //    }
    //    if (hitUp.transform != null)
    //    {
    //        print(hitUp.distance + "." + hitUp.transform.name);
    //        if (hitUp.distance < 0.05f)
    //        {
    //            if (hitUp.collider.transform.rigidbody != null)
    //            {
    //                hitUp.collider.transform.rigidbody.velocity = jump;
    //            }
    //            else print("Obj tht approached spring didnt have rigidbody attach ");
    //        }
    //    }
    //}

    void OnCollisionEnter(Collision collision)
    {
        print(collision.transform.name);
        Vector3 jump = new Vector3(0, 10f, 0);
        if (collision.collider.transform.GetComponent<Rigidbody>() != null /*&&
            (collision.collider.transform.name == "Bip001 L Foot" || collision.collider.transform.name == "Bip001 R Foot")*/)
        {
            collision.collider.transform.GetComponent<Rigidbody>().velocity = jump;
        }
        else print("Obj tht approached spring didnt have rigidbody attach ");
    }
}
