using System.Collections.Generic;
using UnityEngine;

public class Line
{
    private string _Id;
    private Vector2 _a;
    private Vector2 _b;

    public Vector2 A1
    {
        get { return _a; }
        set { _a = value; }
    }

    public Vector2 B1
    {
        get { return _b; }
        set { _b = value; }
    }

    public string Id
    {
        get { return _Id; }
        set { _Id = value; }
    }

    public Line() { }

    public Line(Vector2 a, Vector2 b, bool debugRender = true)
    {
        _a = a;
        _b = b;

        if (debugRender)
        {
            DebugRender();
        }
    }

    public Line(Vector3 a, Vector3 b, float duration, bool debugRender=true)
    {
        _a = U2D.FromV3ToV2( a);
        _b = U2D.FromV3ToV2( b);

        if (debugRender)
        {
            DebugRender(duration);
        }
    }

    public Line(Crystal a, Crystal b, bool debugRender = true)
    {
        _a = a.Position;
        _b = b.Position;

        Id = a.Id + " | " + b.Id;

        if (debugRender)
        {
            DebugRender();
        }
    }

    /// <summary>
    /// Will tell u if 'line' intersects this line
    /// 
    /// http://gamedev.stackexchange.com/questions/26004/how-to-detect-2d-line-on-line-collision
    /// </summary>
    /// <param name="line"></param>
    /// <returns></returns>
    public bool IsIntersecting(Line line)
    {
        var a = _a;
        var b = _b;

        var c = line.A1;
        var d = line.B1;

        float denominator = ((b.x - a.x) * (d.y - c.y)) - ((b.y - a.y) * (d.x - c.x));
        float numerator1 = ((a.y - c.y) * (d.x - c.x)) - ((a.x - c.x) * (d.y - c.y));
        float numerator2 = ((a.y - c.y) * (b.x - a.x)) - ((a.x - c.x) * (b.y - a.y));

        // Detect coincident lines (has a problem, read below)
        if (denominator == 0) return numerator1 == 0 && numerator2 == 0;

        float r = numerator1 / denominator;
        float s = numerator2 / denominator;

        return (r >= 0 && r <= 1) && (s >= 0 && s <= 1);
    }

    //http://csharphelper.com/blog/2014/08/determine-where-two-lines-intersect-in-c/
    /// <summary>
    /// This line and the line two 
    /// 
    /// Find the point of intersection between
    /// the lines p1 --> p2 and p3 --> p4.
    /// </summary>
    /// <param name="two">the other line </param>
    /// <param name="lines_intersect"></param>
    /// <param name="intersection"></param>
    /// <returns></returns>
    public Vector2 FindIntersection(Line two)
    {
        bool lines_intersect;
        Vector2 intersection;

        Vector2 p1 = A1;
        Vector2 p2 = B1;

        Vector2 p3 = two.A1;
        Vector2 p4 = two.B1;

        // Get the segments' parameters.
        float dx12 = p2.x - p1.x;
        float dy12 = p2.y - p1.y;
        float dx34 = p4.x - p3.x;
        float dy34 = p4.y - p3.y;

        // Solve for t1 and t2
        float denominator = (dy12*dx34 - dx12*dy34);

        float t1 =
            ((p1.x - p3.x)*dy34 + (p3.y - p1.y)*dx34)
            /denominator;
        if (float.IsInfinity(t1))
        {
            // The lines are parallel (or close enough to it).
            lines_intersect = false;
            intersection = new Vector2(float.NaN, float.NaN);
            return new Vector2();
        }
        lines_intersect = true;

        float t2 =
            ((p3.x - p1.x)*dy12 + (p1.y - p3.y)*dx12)
            /-denominator;

        // Find the point of intersection.
        intersection = new Vector2(p1.x + dx12*t1, p1.y + dy12*t1);
        return intersection;
    }

    SMe m = new SMe();
    public void DebugRender(float duration = 500f)
    {
        var a = new Vector3(A1.x, m.IniTerr.MathCenter.y, A1.y);
        var b =new Vector3(B1.x, m.IniTerr.MathCenter.y, B1.y);



        Debug.DrawLine(a, b, Color.white, duration);
    }

    public void DebugRender(Color colorH, float duration = 500f)
    {
        var a = new Vector3(A1.x, m.IniTerr.MathCenter.y, A1.y);
        var b = new Vector3(B1.x, m.IniTerr.MathCenter.y, B1.y);

        Debug.DrawLine(a, b, colorH, duration);
    }


    public void DeleteRender()
    {
        
    }

    #region Return Random Points In Line
    private float howFarPoints = .95f;//.8 .55
    private float distanceLeft;
    /// <summary>
    /// Will return random point in line 
    /// </summary>
    /// <returns></returns>
    internal List<Vector3> ReturnRandomPointsInLine()
    {
        var a1Loc = U2D.FromV2ToV3(A1);
        var b1Loc = U2D.FromV2ToV3(B1);

        List<Vector3> res = new List<Vector3>();
        distanceLeft = Vector3.Distance(a1Loc, b1Loc);
        var add = AddToLoop();


        for (float i = 0; i < distanceLeft; i+= add)
        {
            var point = Vector3.MoveTowards(a1Loc, b1Loc, add);
            res.Add(point);
            a1Loc = point;
            add = AddToLoop();
        }

        return res;
    }

    float AddToLoop()
    {
        return UMath.GiveRandom(howFarPoints - .4f, howFarPoints + .4f);
    }


    internal List<Vector3> ReturnPointsInLineEvery(float step)
    {
        var a1Loc = U2D.FromV2ToV3(A1);
        var b1Loc = U2D.FromV2ToV3(B1);

        List<Vector3> res = new List<Vector3>();
        distanceLeft = Vector3.Distance(a1Loc, b1Loc);
        var add = step;

        for (float i = 0; i < distanceLeft; i += add)
        {
            var point = Vector3.MoveTowards(a1Loc, b1Loc, add);
            res.Add(point);
            a1Loc = point;
        }
        return res;
    }

    #endregion
}
