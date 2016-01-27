using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;

public class UVisHelp : MonoBehaviour {

    static SMe m = new SMe();

    public static List<General> CreateTextEnumarate(List<CheckPoint> list, string texto = "", int fontSize = 120)
    {
         List<General> res = new List<General>();
        for (int i = 0; i < list.Count; i++)
        {
            General g = General.Create(Root.texto3d, list[i].Point);
            TextMesh textObject = g.transform.GetComponent<TextMesh>();
            textObject.fontSize = fontSize;
            textObject.text = texto + "." + i;
            res.Add(g);
        }
        return res;
    }

    public static General CreateText(Vector3 iniPos, string texto, int fontSize = 120)
    {
        General g = General.Create(Root.texto3d, iniPos);
        TextMesh textObject = g.transform.GetComponent<TextMesh>();
        textObject.fontSize = fontSize;
        textObject.text = texto;
        return g;
    }

    public static List<General> CreateHelpers(List<Vector3> list, string root)
    {
        List<General> res = new List<General>();
        for (int i = 0; i < list.Count; i++)
        {
            res.Add(General.Create(root, list[i]));
        }
        return res;
    }


    public static General CreateHelpers(Vector3 pos, string root)
    {
        return General.Create(root, pos);
    }   
    
    public static General CreateHelpers(Vector2 pos, string root)
    {
        return General.Create(root, U2D.FromV2ToV3(pos));
    }

    public static List<General> CreateHelpers(List<Line> lines, string root)
    {
        List<General> res = new List<General>();

        res.Add(General.Create(root, new Vector3(lines[0].A1.x, m.IniTerr.MathCenter.y, lines[0].A1.y)));
        res.Add(General.Create(root, new Vector3(lines[0].B1.x, m.IniTerr.MathCenter.y, lines[0].B1.y)));

        res.Add(General.Create(root, new Vector3(lines[2].A1.x, m.IniTerr.MathCenter.y, lines[2].A1.y)));
        res.Add(General.Create(root, new Vector3(lines[2].B1.x, m.IniTerr.MathCenter.y, lines[2].B1.y)));  


        return res;
    }


    public static List<General> CreateHelpers(Rect rect, string root)
    {
        List<Vector3> list = new List<Vector3>();

        Vector2 NW = new Vector2(rect.xMin, rect.yMin);
        Vector2 NE = new Vector2(rect.xMax, rect.yMin);
        Vector2 SE = new Vector2(rect.xMax, rect.yMax);
        Vector2 SW = new Vector2(rect.xMin, rect.yMax);

        list.Add(new Vector3(NW.x, m.IniTerr.MathCenter.y, NW.y));
        list.Add(new Vector3(NE.x, m.IniTerr.MathCenter.y, NE.y));
        list.Add(new Vector3(SE.x, m.IniTerr.MathCenter.y, SE.y));
        list.Add(new Vector3(SW.x, m.IniTerr.MathCenter.y,  SW.y));

        return CreateHelpers(list, root);
    }

    public static void CreateDebugLines(Rect rect, Color color, float duration = 6000f)
    {
        Vector3 NW = U2D.FromV2ToV3(new Vector2(rect.xMin, rect.yMin));
        Vector3 NE = U2D.FromV2ToV3(new Vector2(rect.xMax, rect.yMin));
        Vector3 SE = U2D.FromV2ToV3(new Vector2(rect.xMax, rect.yMax));
        Vector3 SW = U2D.FromV2ToV3(new Vector2(rect.xMin, rect.yMax));

        Debug.DrawLine(NW, NE, color, duration);
        Debug.DrawLine(NE, SE, color, duration);
        Debug.DrawLine(SE, SW, color, duration);
        Debug.DrawLine(SW, NW, color, duration);
    }




    public static List<PreviewWay> CreatePreviewWay(List<Vector3> list, string root, float radius)
    {
        List<PreviewWay> res = new List<PreviewWay>();
        for (int i = 0; i < list.Count; i++)
        {
            //if (list[i] != new Vector3() || list[i] != null)
            //{
                PreviewWay w = (PreviewWay) General.Create(root, list[i]);
                w.Radius = radius;
                res.Add(w);
            //}
        }
        return res;
    }

    static public Object CreateForm(string root, Vector3 origin)
    {
        Object obj = null;
        obj = Resources.Load(root, typeof(Object));
        obj = Instantiate(obj, origin, Quaternion.identity);

        return obj;
    }


    static public Canvas CreateFormCanvas(string root, Vector3 origin)
    {
        Canvas obj = null;
        obj = (Canvas)Resources.Load(root, typeof(Canvas));
        obj = (Canvas)Instantiate(obj, origin, Quaternion.identity);

        return obj;
    }



    static public Material CreateMaterial(string root)
    {
        Material obj = null;
        obj = (Material)Resources.Load(root, typeof(Material));
        return obj;
    }

    public static GameObject RetText(Vector3 iniPos, string textMsg)
    {
        GameObject texto = (GameObject)Resources.Load("Prefab/Misc/3dText", typeof(GameObject));

            GameObject cloneTexto = Instantiate(texto, iniPos, Quaternion.identity) as GameObject;

            cloneTexto.name = textMsg;
        return cloneTexto;
    }



    internal static List<General> CreateHelpers(List<Vector2> _seaSoul, string root)
    {
        List<General> res = new List<General>();
        var list = U2D.FromListV2ToV3(_seaSoul);


        for (int i = 0; i < list.Count; i++)
        {
            res.Add(General.Create(root, list[i]));
        }
        return res;
    }



    internal static List<General> CreateHelpers(List<Crystal> _eval, string root)
    {
        List<General> res = new List<General>();

        for (int i = 0; i < _eval.Count; i++)
        {
            res.Add(General.Create(root, U2D.FromV2ToV3(_eval[i].Position)));
        }
        return res;
    }
}
