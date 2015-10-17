using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CryRect
{
    private Rect _theRect;
    private Vector2 _a;//the start point of the route   
    private Vector2 _b;//the 2nd farther from final, srahes one axis with 'a'

    private Vector2 _c;//the corner closest to the Destiny
    private Vector2 _d;//the corner 2nd closest to the destiny, share one axis value with 'c'
    private bool _isSquare;//will say if the Rect was converted in a square 

    private H _growIn = H.None;//if a rect needs to grow will grow only in 1 axis 
    private float growFactor=1;

    private Crystal _aCrystal;
    private Crystal _bCrystal;
    private Crystal _cCrystal;
    private Crystal _dCrystal;

    public Rect TheRect
    {
        get { return _theRect; }
        set { _theRect = value; }
    }

    public Vector2 C
    {
        get { return _c; }
        set { _c = value; }
    }

    public Vector2 D
    {
        get { return _d; }
        set { _d = value; }
    }

    public bool IsSquare
    {
        get { return _isSquare; }
        set { _isSquare = value; }
    }

    public Vector2 B
    {
        get { return _b; }
        set { _b = value; }
    }

    public Vector2 A
    {
        get { return _a; }
        set { _a = value; }
    }

    public Crystal BCrystal
    {
        get { return _bCrystal; }
        set { _bCrystal = value; }
    }

    public Crystal CCrystal
    {
        get { return _cCrystal; }
        set { _cCrystal = value; }
    }

    public Crystal DCrystal
    {
        get { return _dCrystal; }
        set { _dCrystal = value; }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="ini"></param>
    /// <param name="end"></param>
    /// <param name="grow">The grow of the rect on scale to make sure contain first and last </param>
    /// <param name="lastPoint">The last point of the route is need to find 'D'</param>
    public CryRect(Vector3 ini, Vector3 end, Vector3 lastPoint, float grow)
    {
        _a = U2D.FromV3ToV2(ini);
        
        //eliminated bz uses blue RayCast 
        //var poly = Registro.FromALotOfVertexToPoly(new List<Vector3>() {ini, end});
        
        var poly = Registro.FromALotOfVertexToPolyMathCenterY(new List<Vector3>() {ini, end});
        poly = UPoly.ScalePoly(poly, grow);

        _theRect = Registro.FromALotOfVertexToRect(poly);
        _c = U2D.FromV3ToV2(end);

        _b = FindB();
        _d = FindD();

        RectifyCorners(poly);

        UVisHelp.CreateDebugLines(TheRect, Color.magenta, 5f);

        SetCrystals();
    }

    private void SetCrystals()
    {
        _cCrystal = new Crystal(U2D.FromV2ToV3(C), H.RectCorner, "", setIdAndName:false);
        _dCrystal = new Crystal(U2D.FromV2ToV3(D), H.RectCorner, "", setIdAndName: false);
        _bCrystal = new Crystal(U2D.FromV2ToV3(B), H.RectCorner, "", setIdAndName: false);

    }

    /// <summary>
    /// created so when the rect grows will assign the correct point for each dot(a-d)
    /// Can only grow as a Rectangle uniform scaling 
    /// </summary>
    /// <param name="poly"></param>
    private void RectifyCorners(List<Vector3> poly)
    {
        _a = FindClosestToMe(_a, poly);
        _b= FindClosestToMe(_b,poly);
        _c = FindClosestToMe(_c, poly);
        _d = FindClosestToMe(_d, poly);
    }

    Vector2 FindClosestToMe(Vector2 dot, List<Vector3> poly)
    {
        List<VectorM>lis=new List<VectorM>();

        for (int i = 0; i < poly.Count; i++)
        {
            lis.Add(new VectorM(poly[i], U2D.FromV2ToV3(dot)));
        }

        lis = lis.OrderBy(a => a.Distance).ToList();

        var closest = lis[0].Point;
        return U2D.FromV3ToV2(closest);
    }



    /// <summary>
    /// Will find wht is the other poiunt in the rect tht is closer to LastPoint
    /// </summary>
    /// <returns></returns>
    Vector2 FindD()
    {
        Vector2 x = new Vector2(_c.x, _a.y);
        Vector2 y = new Vector2( _a.x,_c.y);

        var distX = Vector2.Distance(_c, x);
        var distY = Vector2.Distance(_c, y);

        if (distX < distY)
        {
            return x;
        }
        return y;
    }

    /// <summary>
    /// Will find wht is the other poiunt in the rect tht is closer to LastPoint
    /// </summary>
    /// <returns></returns>
    Vector2 FindB()
    {
        Vector2 y = new Vector2(_c.x, _a.y);
        Vector2 x = new Vector2(_a.x, _c.y);

        var distX = Vector2.Distance(_a, x);
        var distY = Vector2.Distance(_a, y);

        if (distX < distY)
        {
            return x;
        }
        return y;
    }



    /// <summary>
    /// Will make the Rect grow in the side he is smaller 
    /// </summary>
    internal void Grow()
    {
        growFactor *= 2;
        DefineGrowAxis();

        if (_growIn == H.Width)
        {
            PushThemAway(H.X);
        }
        else { PushThemAway(H.Y); }

        UVisHelp.CreateDebugLines(TheRect, Color.magenta, 5f);
    }

    /// <summary>
    /// Will push the A,B,C,D aaway from center in the 'axis'
    /// 
    /// This is how grows, we need to specifically grow this vectors bz thouse are the ones used in the routing 
    /// </summary>
    /// <param name="h"></param>
    private void PushThemAway(H axis)
    {
        List<Vector2> lis = new List<Vector2>(){_a,_b,_c,_d};

        //for (int i = 0; i < lis.Count; i++)
        //{
        //    lis[i] = MoveAwayFromCenterInAxis(lis[i], axis);
        //}

        //var aa = _a;

        _a = MoveAwayFromCenterInAxis(_a);
        _b = MoveAwayFromCenterInAxis(_b);
        _c = MoveAwayFromCenterInAxis(_c);
        _d = MoveAwayFromCenterInAxis(_d);
    }

    /// <summary>
    /// Move each vector2 pass away from center only in the axis asked for 
    /// 
    /// If none is 'axis' then will be pushed in both
    /// </summary>
    /// <param name="vector2"></param>
    /// <param name="axis"></param>
    /// <returns></returns>
    private Vector2 MoveAwayFromCenterInAxis(Vector2 vector2, H axis = H.None)
    {
        var dist = Vector2.Distance(TheRect.center, vector2);//distnace to center of the rect 

        //so its moves away from center
        Vector2 newVal = Vector2.MoveTowards(vector2, TheRect.center, -(Mathf.Abs(dist))  * growFactor);


        if (axis == H.None)
        {
            return newVal;
        }

        if (axis == H.X)
        {
            return new Vector2(newVal.x, vector2.y);
        }
        return new Vector2(vector2.x, newVal.y);
    }



    private void DefineGrowAxis()
    {
        if (_growIn == H.None)
        {
            if (_theRect.width > _theRect.height)
            {
                _growIn = H.Height;
                return;
            }
            _growIn = H.Width;
        }
    }


}
