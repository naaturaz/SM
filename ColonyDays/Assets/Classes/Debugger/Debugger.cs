using System;
using UnityEngine;

internal class Debugger : MonoBehaviour
{
    public string Target;
    public bool Save;

    internal bool IsThisOneTarget(Person person)
    {
        return person.MyId.Contains(Target);
    }

    private void Start()
    {
        Target = PlayerPrefs.GetString("Target");
    }

    private void Update()
    {
        if (Save)
        {
            Save = false;
            PlayerPrefs.SetString("Target", Target);
            Debug.Log("Saved:=> Target:" + Target);

        }
    }
}