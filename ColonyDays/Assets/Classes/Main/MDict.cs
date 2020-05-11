using System;
using System.Collections.Generic;

//My Generic Dict... do it for XML serialization
public class MDict<TKey, TValue> : IComparable
{
    private List<TKey> _keys = new List<TKey>();
    private List<TValue> _values = new List<TValue>();

    public MDict()
    {
    }

    public MDict(TKey key, TValue value)
    {
    }

    public List<TKey> Keys
    {
        get { return _keys; }
        set { _keys = value; }
    }

    public List<TValue> Values
    {
        get { return _values; }
        set { _values = value; }
    }

    internal bool ContainsKey(TKey inv)
    {
        if (_keys.Contains(inv))
        {
            return true;
        }
        return false;
    }

    public void Add(TKey key, TValue value)
    {
        _keys.Add(key);
        _values.Add(value);
    }

    public void Remove(TKey key)
    {
        for (int i = 0; i < _keys.Count; i++)
        {
            if (CompareTo(_keys[i], key) == 0)
            {
                _keys.RemoveAt(i);
                _values.RemoveAt(i);
            }
        }
    }

    public int Count()
    {
        return _keys.Count;
    }

    public void SetValueOfKey(TKey key, TValue value)
    {
        for (int i = 0; i < _keys.Count; i++)
        {
            if (CompareTo(_keys[i], key) == 0)
            {
                _values[i] = value;
            }
        }
    }

    public int CompareTo(TKey a, TKey b)
    {
        if (a.Equals(b))
        {
            return 0;
        }
        return -1;
    }

    public int CompareTo(object obj)
    {
        throw new NotImplementedException();
    }

    internal bool IsEmpty()
    {
        if (_keys.Count == 0)
        {
            return true;
        }
        return false;
    }
}