using UnityEngine;

public class Terreno : General
{
    static public Terreno CreateTerrain(string root)//, Transform spawn, string name = ""//)
    {
        Terreno obj = (Terreno)Resources.Load(root, typeof(Terreno));
        obj = (Terreno)Instantiate(obj, new Vector3(0, 0, 0), Quaternion.identity);
        obj.name = obj.name.Remove(obj.name.Length - 7);//removeing the '(clone)'
        return obj;
    }
}