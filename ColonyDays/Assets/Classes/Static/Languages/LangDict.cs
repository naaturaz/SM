using System.Collections.Generic;
using UnityEngine;

internal class LangDict
{
    private Dictionary<string, string> _dict = new Dictionary<string, string>();

    public int Count { get { return _dict.Count; } }
    public Dictionary<string, string> Dictionary { get { return _dict; } }

    public void Add(string key, string value)
    {
        if (_dict.ContainsKey(key))
        {
            Debug.Log("'" + key + "' is already is dict. Not added");
            return;
        }

        _dict.Add(key, value);
    }

    internal bool ContainsKey(string key)
    {
        return _dict.ContainsKey(key);
    }

    internal string ReturnValueWithKey(string key)
    {
        return _dict[key];
    }

    internal void Clear()
    {
        _dict.Clear();
    }
}