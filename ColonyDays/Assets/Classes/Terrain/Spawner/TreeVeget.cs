using UnityEngine;
using System.Collections;

public class TreeVeget : StillElement
{

    // Use this for initialization
    void Start()
    {
        base.Start();
        StartCoroutine("FiveSecUpdate");
        UpdateTreeEvery5Sec();
    }

    private IEnumerator FiveSecUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(5); // wait
            base.UpdateTreeEvery5Sec();
        }
    }


    // Update is called once per frame
    void Update()
    {
        base.Update();


    }


}
