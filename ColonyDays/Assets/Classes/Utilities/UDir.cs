using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UDir  {

    /// <summary>
    /// Returns rectangle of dragged mouse action based on first and second Vector2
    /// </summary>
    /// <param name="first">first position mouse was clicked</param>
    /// <param name="second">seconde position where mouse is</param>
    /// <returns>rectangle of dragged mouse action</returns>
    public static Rect ReturnDragRect(Vector2 first, Vector2 second)
    {
        Rect res = new Rect();
        //SW to NE case
        if (first.x < second.x && first.y < second.y)
        {
            res = new Rect(first.x, first.y, Mathf.Abs(first.x - second.x),
                Mathf.Abs(first.y - second.y));
        }
        //SE to NW case
        else if (first.x > second.x && first.y < second.y)
        {
            res = new Rect(second.x, first.y, Mathf.Abs(first.x - second.x),
                Mathf.Abs(first.y - second.y));
        }
        //NW to SE case
        else if (first.x < second.x && first.y > second.y)
        {
            res = new Rect(first.x, second.y, Mathf.Abs(first.x - second.x),
                Mathf.Abs(first.y - second.y));
        }
        //NE to SW case
        else if (first.x > second.x && first.y > second.y)
        {
            res = new Rect(second.x, second.y, Mathf.Abs(first.x - second.x),
               Mathf.Abs(first.y - second.y));
        }
        //print("mapDimRect instante obj");
        return res;
    }

    /// <summary>
    /// Return the direction from first to second. 
    /// This is suitable for onscreen points 
    /// First dragged point and second point,.
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static Dir ReturnDragDir(Vector2 first, Vector2 second)
    {
        Dir res = Dir.None;
        //SW to NE case
        if (first.x < second.x && first.y < second.y)
        {
            res = Dir.SWtoNE;
        }
        //SE to NW case
        else if (first.x > second.x && first.y < second.y)
        {
            res = Dir.SEtoNW;
        }
        //NW to SE case
        else if (first.x < second.x && first.y > second.y)
        {
            res = Dir.NWtoSE;
        }
        //NE to SW case
        else if (first.x > second.x && first.y > second.y)
        {
            res = Dir.NEtoSW;
        }
        //print("mapDimRect instante obj");
        return res;
    }

    /// <summary>
    /// Return the direction from first to second. 
    /// This is suitable for Vector3 3 positions in world. With out cordinate system
    /// </summary>
    public static Dir ReturnDragDir(Vector3 first, Vector3 second)
    {
        Dir res = Dir.None;
        //SW to NE case
        if (first.x <= second.x && first.z <= second.z)
        {
            res = Dir.SWtoNE;
        }
        //SE to NW case
        else if (first.x > second.x && first.z <= second.z)
        {
            res = Dir.SEtoNW;
        }
        //NW to SE case
        else if (first.x <= second.x && first.z > second.z)
        {
            res = Dir.NWtoSE;
        }
        //NE to SW case
        else if (first.x > second.x && first.z > second.z)
        {
            res = Dir.NEtoSW;
        }
        //print("mapDimRect instante obj");
        return res;
    }

    public static Dir ItMovedTowards(Vector3 oldPos, Vector3 newPos)
    {
        Dir res = Dir.None;
        res = MovedToCompoundDir(oldPos, newPos);

        if (res == Dir.None)
        {
            res = MovedToSimpleDir(oldPos, newPos);
        }

        return res;
    }

    public static Dir MovedToSimpleDir(Vector3 oldPos, Vector3 newPos)
    {
        if (oldPos.z > newPos.z)
        {
            return Dir.S;
        }
        if (oldPos.x < newPos.x)
        {
            return Dir.E;
        }
        if (oldPos.x > newPos.x)
        {
            return Dir.W;
        }

        //this notrth and south is oblisuly respecting the world cordinates
        if (oldPos.z < newPos.z)
        {
            return Dir.N;
        }
    
        return Dir.None;
    }

    public static Dir MovedToCompoundDir(Vector3 oldPos, Vector3 newPos)
    {
        if (oldPos.x < newPos.x && oldPos.z > newPos.z)
        {
            return Dir.SE;
        }
        // SW case
        if (oldPos.x > newPos.x && oldPos.z > newPos.z)
        {
            return Dir.SW;
        }
        //NE case
        if (oldPos.x < newPos.x && oldPos.z < newPos.z)
        {
            return Dir.NE;
        }
        // NW case
        if (oldPos.x > newPos.x && oldPos.z < newPos.z)
        {
            return Dir.NW;
        }
        // SE case
    
        //print("mapDimRect instante obj");
        return Dir.None;
    }

    /// <summary>
    /// Tells u in which side a hit point landed on ppoly 
    /// </summary>
    public static Dir TellMeWhenHitLanded(List<Vector3> poly, Vector3 hitVector3)
    {
        float ep = 0.01f;
        if(UMath.nearlyEqual(hitVector3.x,poly[0].x, ep ) && UMath.nearlyEqual(hitVector3.x,poly[3].x, ep ))
        {
            return Dir.W;
        }
        if (UMath.nearlyEqual(hitVector3.x, poly[1].x, ep) && UMath.nearlyEqual(hitVector3.x, poly[2].x, ep))
        {
            return Dir.E;
        }
        if (UMath.nearlyEqual(hitVector3.z, poly[0].z, ep) && UMath.nearlyEqual(hitVector3.z, poly[1].z, ep))
        {
            return Dir.N;
        }
        if (UMath.nearlyEqual(hitVector3.z, poly[3].z, ep) && UMath.nearlyEqual(hitVector3.z, poly[2].z, ep))
        {
            return Dir.S;
        }
        return Dir.None;
    }


    static public Dir FromIntToDir(int intD)
    {
        if (intD == 0)
        {
            return Dir.N;
        }
        else if (intD == 1)
        {
            return Dir.E;
        }
        else if (intD == 2)
        {
            return Dir.S;
        }
        else if (intD == 3)
        {
            return Dir.W;
        }
        return Dir.None;
    }

    /// <summary>
    /// 
    ///N, E, S, W..... : 0,1,2,3
    /// 
    /// will return the sign of a multiplication towards that direction 
    /// for ex: North will return: new Vector3(0,0,1)
    ///
    /// </summary>
    /// <param name="intD"></param>
    /// <returns></returns>
    static public Vector3 FromDirToVector3(int intD)
    {
        if (intD == 0)
        {
            return new Vector3(0,0,1);
        }
        else if (intD == 1)
        {
            return new Vector3(1, 0, 0);
        }
        else if (intD == 2)
        {
            return new Vector3(0, 0, -1);
        }
        else if (intD == 3)
        {
            return new Vector3(-1, 0, 0);
        }
        return new Vector3();
    }

    /// <summary>
    /// Created to convet int to cardinal. Only useful to int the are organize correctly from 0-3, like a 
    /// building anchors
    /// 
    /// 0 = NW, 1 = NE, 2 = SE, 3 = SW
    /// </summary>
    /// <param name="i"></param>
    static public Dir BuildingFromIntToCardinal(int i)
    {
        if (i==0)
        {
            return Dir.NW;
        }     
        if (i==1)
        {
            return Dir.NE;
        }    
        if (i==2)
        {
            return Dir.SE;
        }  
        if (i==3)
        {
            return Dir.SW;
        }
        return Dir.None;
    }


}
