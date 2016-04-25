using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// The dialog GameObject 
/// </summary>
class DialogGO : GUIElement
{

    private H _type;
    private Text _textHere;

    public H Type1
    {
        get { return _type; }
        set { _type = value; }
    }

    static public DialogGO Create(string root, Transform container, Vector3 iniPos, H type)
    {
        DialogGO obj = null;

        obj = (DialogGO)Resources.Load(root, typeof(DialogGO));
        obj = (DialogGO)Instantiate(obj, new Vector3(), Quaternion.identity);

        var localScale = obj.transform.localScale;

        obj.transform.position = iniPos;
        obj.transform.parent = container;

        obj.transform.localScale = localScale;
        obj.Type1 = type;

        return obj;
    }

    void Start()
    {
        var t = GetChildCalled("TextHere");
        _textHere = t.GetComponentInChildren<Text>();

        _textHere.text = Languages.ReturnString(Type1+"");
    }

    private void Set()
    {
        if (Type1 == H.OverWrite)
        {
            
        }
    }
}
