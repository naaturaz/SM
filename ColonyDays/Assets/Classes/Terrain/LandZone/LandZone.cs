using System.Collections.Generic;
using UnityEngine;

public class LandZone : Crystal
{
    private SMe m = new SMe();
    private string _id = "";

    private List<LinkRect> _linkRects = new List<LinkRect>();

    private LinkRect _cuRect;

    private string _landZoneName;

    public LandZone()
    {
    }

    public LandZone(List<Vector3> grid)
    {
        StartLinking(grid);
    }

    public LandZone(List<LinkRect> list)
    {
        _linkRects = list;
    }

    public LandZone(string name)
    {
        LandZoneName = name;
        SetCrystalProps();
    }

    public List<LinkRect> LinkRects
    {
        get { return _linkRects; }
        set { _linkRects = value; }
    }

    public string LandZoneName
    {
        get { return _landZoneName; }
        set { _landZoneName = value; }
    }

    /// <summary>
    /// This is when creating the first pass of LinkRects
    /// </summary>
    /// <param name="grid"></param>
    public void StartLinking(List<Vector3> grid)
    {
        AddCurrent();

        //Debug.Log("GridCount:" + grid.Count);

        if (grid.Count > 0)
        {
            _cuRect = (new LinkRect(grid));
        }
        else
        {
            RectLinkingDone();
        }
    }

    private void AddCurrent()
    {
        if (IsCurrentValid() && BelongToARegion())
        {
            _linkRects.Add(_cuRect);
        }
        else
        {
            if (_cuRect == null)
            {
                return;
            }
            //so i can see its not added
            _cuRect.MakeDebugRed();
        }
    }

    /// <summary>
    /// Current LinkRect must below to a region to be added here otherwise is not needed
    /// </summary>
    /// <returns></returns>
    private bool BelongToARegion()
    {
        if (_cuRect == null)
        {
            return false;
        }

        if (MeshController.CrystalManager1.ReturnMyRegion(_cuRect.Position) == -1)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// Will tell u if current LinkRect is valid
    ///
    /// If is on top or intersect  of a line from Terra is not valid
    /// </summary>
    /// <returns></returns>
    private bool IsCurrentValid()
    {
        if (_cuRect == null)
        {
            return false;
        }

        if (MeshController.CrystalManager1.DoIContainAnyCrystal(_cuRect.Rect1, _cuRect.Position))
        {
            return false;
        }
        if (MeshController.CrystalManager1.DoIIntersectAnyLine(_cuRect.Rect1, _cuRect.Position))
        {
            return false;
        }
        return true;
    }

    private void RectLinkingDone()
    {
        for (int i = 0; i < _linkRects.Count; i++)
        {
            MeshController.CrystalManager1.AddCrystal(_linkRects[i]);
        }

        MeshController.CrystalManager1.LinkCrystals();
    }

    public void AddLinkRect(LinkRect linkRect)
    {
        if (_id == "")
        {
            _id = linkRect.Id;
        }
        else if (_id != "" && _id != linkRect.Id)
        {
            ANewZoneMustBeCreated();
        }

        _linkRects.Add(linkRect);

        DecideIfContinueLinking();
    }

    public void ANewZoneMustBeCreated()
    {
        throw new System.NotImplementedException();
    }

    private void DecideIfContinueLinking()
    {
        if (m.MeshController.LandZoneManager1.CommomLayer.Count > 0)
        {
            StartLinking(m.MeshController.LandZoneManager1.CommomLayer);
        }
    }

    public void Update()
    {
        if (_cuRect != null)
        {
            _cuRect.Update();
        }
    }

    /// <summary>
    /// This will be the middle of the LandZone
    ///
    /// This is created bz landzones at beggining always are split in the same physycal land.
    ///
    /// So I will try to connect them
    /// </summary>
    /// <returns></returns>
    public Vector2 CalcPosition()
    {
        Vector2 pos = new Vector3();
        for (int i = 0; i < _linkRects.Count; i++)
        {
            pos = pos + _linkRects[i].Position;
        }

        return pos / _linkRects.Count;
    }

    /// <summary>
    /// Use to remane all its linkRects when a LandZone was connected to another
    /// </summary>
    /// <param name="p"></param>
    internal void RenameAllMyLinkRects(string p)
    {
        for (int i = 0; i < _linkRects.Count; i++)
        {
            _linkRects[i].Name = p;
        }
    }

    internal void SetCrystalProps()
    {
        Id = Person.GiveRandomID();
        Type1 = H.LandZone;
        Position = CalcPosition();
        Name = LandZoneName;
    }
}