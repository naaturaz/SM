using UnityEngine;
using UnityEngine.UI;

class OnScreenHelp : MonoBehaviour
{
    //Editor Inspector
    public string Key_Message;//write the key in the 
    public Transform KeepTransform;
    public Structure MyBuilding;



    private string _msg;
    private GameObject _geometry;
    private Text _text;
    private RectTransform _rectTransform;

    //the Key that is showing this. 
    //needed to pull the string from Languages.cs
    private string _key;

    // Use this for initialization
    void Start()
    {
        //for docks that are already in terrain
        if (Program.MouseListener == null || Program.MouseListener.Main == null)
            return;

        //Find canvas and make this child of it 
        GameObject mainGui = Program.MouseListener.Main.gameObject;
        var canvas = General.GetChildCalledOnThis("Canvas", mainGui);

        gameObject.transform.SetParent(canvas.transform);

        _key = Key_Message;
        _geometry = General.FindGameObjectInHierarchy("Geometry", gameObject);
        var textGO = General.FindGameObjectInHierarchy("Text", gameObject);

        if (textGO != null)
        {
            _text = textGO.GetComponent<Text>();
        }

        _rectTransform = transform.GetComponent<RectTransform>();
        _rectTransform.rotation = canvas.transform.rotation;

        _msg = Languages.ReturnString(_key);
        _text.text = _msg;
    }

    // Update is called once per frame
    void Update()
    {
        if (KeepTransform != null && _rectTransform != null)
        _rectTransform.position = CamControl.RTSCamera().WorldToScreenPoint(KeepTransform.position);

        if (MyBuilding == null || MyBuilding.PositionFixed)
            Destroy(gameObject);       
                
    }
}