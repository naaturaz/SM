using UnityEngine;
using System.Collections;

public class TreeVeget : StillElement {

	// Use this for initialization
	void Start () {
	    base.Start();
        //StartCoroutine("FiveSecUpdate");

        //StartCoroutine("OneSecUpdate");

	}

    private IEnumerator FiveSecUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(2); // wait
            base.Update();
        }
    }

    private IEnumerator OneSecUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(1); // wait
            base.UpdateTree();
        }
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();


    }


}
