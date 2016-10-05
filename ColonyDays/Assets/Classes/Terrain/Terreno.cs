using UnityEngine;

public class Terreno : General
{
    private string _root;

    public string Root1
    {
        get { return _root; }
        set { _root = value; }
    }

    static public Terreno CreateTerrain(string root)//, Transform spawn, string name = ""//)
    {
        Debug.Log("Creating terrain:"+root);
        Terreno obj = (Terreno)Resources.Load(root, typeof(Terreno));
        obj = (Terreno)Instantiate(obj, new Vector3(0, 0, 0), Quaternion.identity);
        obj.name = obj.name.Remove(obj.name.Length - 7);//removeing the '(clone)'
        obj.Root1 = root;
        return obj;
    }
}