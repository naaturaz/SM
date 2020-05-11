using UnityEngine;

public class LandZoneLinker
{
    private string _zone1;
    private string _zone2;
    private Vector3 _pos;
    private string _buildMyId;

    public string Zone1
    {
        get { return _zone1; }
        set { _zone1 = value; }
    }

    public string Zone2
    {
        get { return _zone2; }
        set { _zone2 = value; }
    }

    public Vector3 Pos
    {
        get { return _pos; }
        set { _pos = value; }
    }

    public string BuildMyId
    {
        get { return _buildMyId; }
        set { _buildMyId = value; }
    }

    public LandZoneLinker(string zone1, string zone2, Vector3 pos = new Vector3(), string MyId = "")
    {
        this._zone1 = zone1;
        this._zone2 = zone2;
        this._pos = pos;
        this._buildMyId = MyId;
    }

    /// <summary>
    /// Will return true if both ends are the same disregardin order
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool IsSameType(LandZoneLinker other)
    {
        int same = 0;
        if (_zone1 == other.Zone1 || _zone1 == other.Zone2)
        {
            same++;
        }
        if (_zone2 == other.Zone1 || _zone2 == other.Zone2)
        {
            same++;
        }

        if (same == 2)
        {
            return true;
        }
        return false;
    }

    internal bool IsSameType(Connection conn)
    {
        int same = 0;
        if (_zone1 == conn.Zone1 || _zone1 == conn.Zone2)
        {
            same++;
        }
        if (_zone2 == conn.Zone1 || _zone2 == conn.Zone2)
        {
            same++;
        }

        if (same == 2)
        {
            return true;
        }
        return false;
    }

    internal bool Has1OfSameType(string zone)
    {
        int same = 0;
        if (_zone1 == zone)
        {
            same++;
        }
        if (_zone2 == zone)
        {
            same++;
        }

        if (same == 1)
        {
            return true;
        }
        return false;
    }
}