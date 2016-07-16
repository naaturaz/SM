using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class U2D : MonoBehaviour {

    static SMe m = new SMe();

    /// <summary>
    /// Due to GUI.DrawTexture start with 0,0 in the top left corner, 
    /// and Rect 0,0 is the botton left corner we have to invert the values of the 
    /// start Y. Only usefull for Screen GUI functions
    /// </summary>
    /// <param name="toBeInversed"></param>
    public static Rect ReturnDrawRectYInverted(Rect toBeInversed)
    {
        toBeInversed.y = Screen.height - toBeInversed.y - toBeInversed.height;
        return toBeInversed;
    }

    /// <summary>
    /// Will inverse Rectangle on Y for a cordinate system where in Y 100 is on top and 0 in bottom.
    /// Which is the way I have setup the current Cardinal System NW,NE, SE, SW; 
    /// This is with the concept of substituying giving Y here the Z value in World 
    /// </summary>
    /// <param name="toBeInversed"></param>
    /// <returns></returns>
    public static Rect ReturnRectYInverted(Rect toBeInversed)
    {
        toBeInversed.y = toBeInversed.y - toBeInversed.height;
        return toBeInversed;
    }

    /// <summary>
    /// Taken a list of vectors 3 being a Poly with seq:  NW, NE, SE, SW . will create a new rectangle
    /// Here is really onlye needed the NW, NE, and SW to find the new Rect
    /// </summary>
    public static Rect FromPolyToRect(List<Vector3> poly)
    {
        Vector3 NW = poly[0];
        Vector3 NE = poly[1];
        Vector3 SW = poly[3];

        float width = NW.x - NE.x;
        float height = NW.z - SW.z;

        Rect res = new Rect();
        res.x = NW.x;
        res.y = NW.z;
        res.width = Mathf.Abs(width);
        res.height = Mathf.Abs(height);

        return res;
    }

    /// <summary>
    /// Taken a list of vectors 3 being a Poly with seq:  NW, NE, SE, SW . will create a new rectangle
    /// Here is really onlye needed the NW, NE, and SW to find the new Rect
    /// </summary>
    public static List<Line> FromPolyToLines(List<Vector3> poly)
    {
        var rect = FromPolyToRect(poly);
        rect = ReturnRectYInverted(rect);

        List<Line> res = new List<Line>();

        Vector2 NW = new Vector2(rect.xMin, rect.yMin);
        Vector2 NE = new Vector2(rect.xMax, rect.yMin);
        Vector2 SE = new Vector2(rect.xMax, rect.yMax);
        Vector2 SW = new Vector2(rect.xMin, rect.yMax);

        res.Add(new Line(NW, NE));
        res.Add(new Line(NE, SE));
        res.Add(new Line(SE, SW));
        res.Add(new Line(SW, NW));

        return res;
    }

    /// <summary>
    /// Taken a list of vectors 3 being a Poly with seq:  NW, NE, SE, SW . will create a new rectangle
    /// Here is really onlye needed the NW, NE, and SW to find the new Rect
    /// </summary>
    public static List<Line> FromRectToLines(Rect rect)
    {
        List<Line> res = new List<Line>();

        Vector2 NW = new Vector2(rect.xMin, rect.yMin);
        Vector2 NE = new Vector2(rect.xMax, rect.yMin);
        Vector2 SE = new Vector2(rect.xMax, rect.yMax);
        Vector2 SW = new Vector2(rect.xMin, rect.yMax);

        res.Add(new Line(NW, NE, false));
        res.Add(new Line(NE, SE, false));
        res.Add(new Line(SE, SW, false));
        res.Add(new Line(SW, NW, false));

        return res;
    }   
    

    public static List<Vector3> FromRectToPoly(Rect rect)
    {

        Vector2 NW = new Vector2(rect.xMin, rect.yMin);
        Vector2 NE = new Vector2(rect.xMax, rect.yMin);
        Vector2 SE = new Vector2(rect.xMax, rect.yMax);
        Vector2 SW = new Vector2(rect.xMin, rect.yMax);



        return new List<Vector3>()
        {
            FromV2ToV3( NW), 
            FromV2ToV3(NE), 
            FromV2ToV3(SE), 
            FromV2ToV3(SW)
        };
    }


    /// <summary>
    /// Will make Y:  m.IniTerr.MathCenter.y. and z: will be vector2.y
    /// </summary> 
    /// <param name="a"></param>
    /// <returns></returns>
    public static Vector3 FromV2ToV3(Vector2 a)
    {
        return new Vector3(a.x, m.IniTerr.MathCenter.y, a.y);
    }

    public static List<Vector3> FromListV2ToV3(List<Vector2> list)
    {
        List<Vector3> res = new List<Vector3>();

        for (int i = 0; i < list.Count; i++)
        {
            res.Add(FromV2ToV3(list[i]));
        }

        return res;
    }

    /// <summary>
    /// return new Vector2(a.x,  a.z) 
    /// </summary>
    /// <param name="a"></param>
    /// <returns></returns>
    public static Vector2 FromV3ToV2(Vector3 a)
    {
        return new Vector2(a.x,  a.z);
    }


    static Rect screenRect = new Rect(0, 0, Screen.width, Screen.height);
    public static bool IsMouseOnScreen()
    {
        if (!screenRect.Contains(Input.mousePosition))
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// called when resolution changed
    /// </summary>
    public static void RedoScreenRect()
    {
        screenRect = new Rect(0, 0, Screen.width, Screen.height);
    }

}
