using UnityEngine;

public class Terreno : General
{
    NavigationArea _navArea;

    private string _root;
    private bool _default;//the one loaded at first time

    public string Root1
    {
        get { return _root; }
        set { _root = value; }
    }

    public bool Default
    {
        get { return _default; }
        set { _default = value; }
    }

    static public Terreno CreateTerrain(string root, bool defaultP = false)//, Transform spawn, string name = ""//)
    {
        Debug.Log("Creating terrain:"+root);
        Terreno obj = (Terreno)Resources.Load(root, typeof(Terreno));
        obj = (Terreno)Instantiate(obj, new Vector3(0, 0, 0), Quaternion.identity);
        obj.name = obj.name.Remove(obj.name.Length - 7);//removeing the '(clone)'
        obj.Root1 = root;
        obj.Default = defaultP;
        return obj;
    }

    private void Start()
    {
        base.Start();
        _navArea = new NavigationArea(gameObject);
    }

    void Update()
    {
        _navArea.AddNavArea();
    }
}