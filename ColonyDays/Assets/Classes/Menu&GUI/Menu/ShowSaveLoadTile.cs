using UnityEngine;
using UnityEngine.UI;

public class ShowSaveLoadTile : GUIElement
{
    private UnityEngine.UI.Button _btn;
    private string _saveName;

    public string SaveName
    {
        get { return _saveName; }
        set { _saveName = value; }
    }

    /// <summary>
    /// For show Save Load Tiles
    /// </summary>
    /// <param name="container"></param>
    /// <param name="invItem"></param>
    /// <param name="iniPos"></param>
    /// <param name="parent"></param>
    /// <param name="invType"></param>
    /// <returns></returns>
    static public ShowSaveLoadTile Create(string root, Transform container, Vector3 iniPos, string saveName)
    {
        ShowSaveLoadTile obj = null;

        obj = (ShowSaveLoadTile)Resources.Load(root, typeof(ShowSaveLoadTile));
        obj = (ShowSaveLoadTile)Instantiate(obj, new Vector3(), Quaternion.identity);

        var localScale = obj.transform.localScale;

        obj.transform.position = iniPos;
        obj.transform.SetParent(container);

        obj.transform.localScale = localScale;
        obj.SaveName = saveName;

        return obj;
    }

    private void Start()
    {
        Set(SaveName);
    }

    public void Set(string saveName)
    {
        //making the string Readable
        var len = DataController.SugarMillPath().Length;
        saveName = saveName.Substring(len + 1);

        var text = GetChildCalledOnThis("Title", gameObject).GetComponent<Text>();
        text.text = saveName;

        // _btn = GetChildCalledOnThis("Btn", gameObject).GetComponent<UnityEngine.UI.Button>();
        _btn = gameObject.GetComponent<UnityEngine.UI.Button>();
        _btn.onClick.AddListener(() => Program.MouseClickListenerSt("MainMenu.Save." + saveName));
    }

    private void Update()
    {
    }
}