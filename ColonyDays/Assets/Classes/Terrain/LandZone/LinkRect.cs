using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Will inherit from Crsytal so is easy to link the LinkRects.
/// They will be link using the Link() on Crystal
/// </summary>
public class LinkRect : Crystal
{
    private SMe m = new SMe();
    private string _id;

    private Rect _rect = new Rect();

    //he cant grow towards the one added here
    private List<int> _constraint = new List<int>();

    //what ever is left on the grid to grow. the grid in the mosy common Y but this
    //LinkRects will feed from there and will chew the _grid completely
    private List<Vector3> _grid;

    //N, E, S, W..... : 0,1,2,3
    private int _direction;

    //this is the current row looking to eat more land
    private List<Vector3> _currRow = new List<Vector3>();

    //how many tiles the currentRow List has
    private int _rowTiles;

    //the current Vector3 will be feed
    private List<Vector3> _currVector3s = new List<Vector3>();

    private const int STEP = 3;

    //will step further into the grid on 3s unless a constraint is found
    private int _stepFurther = 3;

    //the tiles size
    private float _xTile;

    private float _zTile;

    //the vector3 has eaten already this Rect
    private List<Vector3> _eaten = new List<Vector3>();

    //this rect a bit to be linked to others thru with .Contain
    private float _rectForcedGrow = 0.5f;

    public string Id
    {
        get { return _id; }
        set { _id = value; }
    }

    public Rect Rect1
    {
        get { return _rect; }
        set { _rect = value; }
    }

    public LinkRect()
    {
    }

    /// <summary>
    /// When creatig a Disconnected one
    ///
    /// like a brand new .
    /// </summary>
    /// <param name="grid"></param>
    public LinkRect(List<Vector3> grid) : base(new Vector3(), H.LinkRect, "")
    {
        _grid = grid;
        //_xTile = Mathf.Abs(m.SubDivide.XSubStep);
        //_zTile = Mathf.Abs( m.SubDivide.ZSubStep);

        _xTile = Mathf.Abs(m.IniTerr.StepX);
        _zTile = Mathf.Abs(m.IniTerr.StepZ);

        _eaten.Add(_grid[0]);//_grid.Count / 2

        _id = Person.GiveRandomID();

        Grow();
    }

    private void Grow()
    {
        if (StillOneDir())
        {
            EatLand();
            Feed();
            loop = true;
        }
        else if (!StillOneDir())
        {
            LastActions();
            //throw new Exception();
        }
    }

    /// <summary>
    /// will add vertext to current Verctor 3 List based on direction and current position
    /// </summary>
    private void EatLand()
    {
        DefineCurrentRow();
        DefineCurrentVector3s();
    }

    /// <summary>
    /// Define current vector3 list . that is the one we try to eat on terrain
    /// </summary>
    private void DefineCurrentVector3s()
    {
        //will find te sign where to go base on direction
        Vector3 sign = UDir.FromDirToVector3(_direction);
        var xSign = sign.x;
        var zSign = sign.z;

        for (int i = 0; i < _currRow.Count; i++)
        {
            var rowIniDot = _currRow[i];

            for (int j = 0; j < _stepFurther; j++)
            {
                //will move the initial row point towards the sign and times the size of the tile
                rowIniDot += new Vector3((_xTile * xSign), 0, (_zTile * zSign));
                _currVector3s.Add(rowIniDot);
            }
        }

        var t = this;
        //DebugHere();
    }

    private void DebugHere()
    {
        if (_eaten.Count == 4 && _currVector3s.Count == 12 && _debug.Count == 0)
        {
            ShowDebug(_currVector3s, Root.blueSphereHelp);
            ShowDebug(_eaten, Root.yellowCube);

            var cut = UList.ReturnTrunckedList(_grid, 500);
            ShowDebug(cut, Root.blueCube);
        }
    }

    private void DefineCurrentRow()
    {
        //_currRow.Add
        var xS = UList.FindXAxisCommonValues(_eaten, H.Descending);
        var zS = UList.FindZAxisCommonValues(_eaten, H.Descending);

        float find = 0;
        H axis = H.None;

        //norht find the top value on z
        if (_direction == 0)
        {
            find = UMath.ReturnMax(zS);
            axis = H.Z;
        }
        //south
        else if (_direction == 2)
        {
            find = UMath.ReturnMinimum(zS);
            axis = H.Z;
        }
        //east
        else if (_direction == 1)
        {
            find = UMath.ReturnMax(xS);
            axis = H.X;
        }
        //west
        else if (_direction == 3)
        {
            find = UMath.ReturnMinimum(xS);
            axis = H.X;
        }

        _currRow = UList.FindVectorsOnSameRange(_eaten, find, axis, 0.05f);
    }

    private void ChangeToNextDir()
    {
        _direction = UMath.GoAround(1, _direction, 0, 3);
    }

    //will feed
    private void Feed()
    {
        if (FeedIsOk())
        {
            Remove1stEatean();
            AddCurrentVector3sToEaten();
            RemoveCurrentVector3FromGrid();
            CleanCurrents();
            ChangeToNextDir();

            _stepFurther++;
        }
        else
        {
            StepDown();
            CleanCurrents();
        }
    }

    /// <summary>
    /// Will slow down on the steps
    /// </summary>
    private void StepDown()
    {
        if (_stepFurther > 1)
        {
            _stepFurther--;
        }
        //means cant move forward towards thi direction even 1 tile
        else
        {
            //exit point of while loop
            AddCurrentDirectionToConstraint();
            _stepFurther = STEP;
            ChangeToNextDir();
        }
    }

    private void AddCurrentDirectionToConstraint()
    {
        _constraint.Add(_direction);
    }

    /// <summary>
    /// Will rtell u if current Vectors 3 List can be feed. Need to know if the are a perfect Rectangle
    ///
    /// Will try to find all the Current Vector3 asked for on the first 1000 on the grid
    /// </summary>
    /// <returns></returns>
    private bool FeedIsOk()
    {
        int local = 0;
        var top = DefineTop();

        for (int i = 0; i < _currVector3s.Count; i++)
        {
            if (IsOnEaten(_currVector3s[i]))
            {
                local++;
                continue;
            }

            for (int j = 0; j < top; j++)
            {
                if (UMath.nearEqualByDistance(_currVector3s[i], _grid[j], 0.3f))
                {
                    //so it can be removed
                    _currVector3s[i] = _grid[j];

                    local++;
                    break;
                }
            }

            //means tht one went trheu and didnt find a mathc
            if (i > local)
            {
                return false;
            }
        }

        if (local == _currVector3s.Count)
        {
            return true;
        }
        return false;
    }

    private bool IsOnEaten(Vector3 a)
    {
        for (int i = 0; i < _eaten.Count; i++)
        {
            if (UMath.nearEqualByDistance(a, _eaten[i], 0.3f))
            {
                return true;
            }
        }
        return false;
    }

    private bool loop;

    public void Update()
    {
        if (loop)
        {
            loop = false;
            Grow();
        }
    }

    private int DefineTop()
    {
        int top = 100;

        if (top > _grid.Count)
        {
            top = _grid.Count;
        }
        return top;
    }

    /// <summary>
    /// Will remove the current Vector 3 list from the Grid
    /// </summary>
    private void RemoveCurrentVector3FromGrid()
    {
        for (int i = 0; i < _currVector3s.Count; i++)
        {
            _grid.Remove(_currVector3s[i]);
        }

        //Debug.Log("grid:"+_grid.Count);
    }

    /// <summary>
    /// bz the first is not removed anywere else and needs to be remvoed from the _grid
    ///
    /// this is only needed the 1st time
    /// </summary>
    private void Remove1stEatean()
    {
        if (_eaten.Count == 1)
        {
            for (int i = 0; i < _eaten.Count; i++)
            {
                _grid.Remove(_eaten[i]);
            }

            //Debug.Log("grid:" + _grid.Count);
        }
    }

    private void CleanCurrents()
    {
        _currRow.Clear();
        _currVector3s.Clear();

        //DeleteDebug();
    }

    /// <summary>
    /// Will add the current Vector 3 List to the existing Rect
    /// </summary>
    private void AddCurrentVector3sToEaten()
    {
        _eaten.AddRange(_currVector3s);
    }

    /// <summary>
    /// Return true if at least can keep growing in one direction
    /// </summary>
    /// <returns></returns>
    private bool StillOneDir()
    {
        return _constraint.Count < 4;
    }

    private void LastActions()
    {
        //jst in case is a Link tht only has one _eatean
        Remove1stEatean();

        ConformRect();
        CreateNewLinkRect();
    }

    private void ConformRect()
    {
        var poly = Registro.FromALotOfVertexToPolyMathCenterY(_eaten);

        //ShowDebug(poly, Root.blueCube);

        //expanding the rect
        //Expands this rect a bit to be linked to others thru with .Contain
        poly = UPoly.ScalePoly(poly, _rectForcedGrow);

        _rect = Registro.FromALotOfVertexToRect(poly);
        UVisHelp.CreateDebugLines(_rect, Color.yellow);

        FinalizeBaseProps();
    }

    /// <summary>
    /// Getting the crystal ready for the linking later
    /// </summary>
    private void FinalizeBaseProps()
    {
        Position = _rect.center;
    }

    /// <summary>
    /// the last step here
    /// </summary>
    private void CreateNewLinkRect()
    {
        m.MeshController.LandZoneManager1.AddNewLinkRectToCurrentLandZone();
    }

    private List<General> _debug = new List<General>();

    private void ShowDebug(List<Vector3> list, string root)
    {
        _debug.AddRange(UVisHelp.CreateHelpers(list, root));
    }

    private void DeleteDebug()
    {
        for (int i = 0; i < _debug.Count; i++)
        {
            _debug[i].Destroy();
        }
        _debug.Clear();
    }

    public void MakeDebugRed()
    {
        UVisHelp.CreateDebugLines(_rect, Color.red);
    }
}