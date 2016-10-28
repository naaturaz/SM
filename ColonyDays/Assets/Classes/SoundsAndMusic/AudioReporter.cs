using UnityEngine;

public class AudioReporter
{
    private General _general;

    public AudioReporter(General gen)
    {
        Debug.Log("AudioReporter created");
        _general = gen;
    }

    public void A2SecUpdate()
    {
        if (Camera.main == null)
        {
            return;
        }

        var dist = Vector3.Distance(Camera.main.transform.position, _general.transform.position);

        if (dist > 1000)
        {
            return;
        }

        AudioCollector.Reporting(WhatAmIReporting(), dist);
    }

    /// <summary>
    /// What Im a reporting. A person will report the current animation is playing for ex
    /// 
    /// 
    /// </summary>
    /// <returns></returns>
    string WhatAmIReporting()
    {
        if (_general.HType == H.Person)
        {
            Person p = (Person) _general;
            return p.Body.CurrentAni;
        }
        return _general.HType + "";
    }

}
