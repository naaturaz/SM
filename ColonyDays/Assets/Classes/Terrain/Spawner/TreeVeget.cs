using System.Collections;
using UnityEngine;

public class TreeVeget : StillElement
{
    // Use this for initialization
    private void Start()
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
    private void Update()
    {
        base.Update();
    }
}