using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;


/// <summary>
/// The dialog GameObject 
/// </summary>
class DialogGO : GUIElement
{

    private H _type;
    private Text _textHere;
    private string _str1;

    private GameObject _okBtnGO;
    private UnityEngine.UI.Button _ok;

    public H Type1
    {
        get { return _type; }
        set { _type = value; }
    }

    public string Str1
    {
        get { return _str1; }
        set { _str1 = value; }
    }

    static public DialogGO Create(string root, Transform container, Vector3 iniPos, H type, string str1 = "")
    {
        DialogGO obj = null;

        obj = (DialogGO)Resources.Load(root, typeof(DialogGO));
        obj = (DialogGO)Instantiate(obj, new Vector3(), Quaternion.identity);

        var localScale = obj.transform.localScale;

        obj.transform.position = iniPos;
        obj.transform.parent = container;

        obj.transform.localScale = localScale;
        obj.Type1 = type;
        obj.Str1 = str1;

        return obj;
    }

    void Start()
    {
        var t = GetChildCalled("TextHere");
        _textHere = t.GetComponentInChildren<Text>();

        _textHere.text = String.Format(Languages.ReturnString(Type1+""), Str1);


        _okBtnGO = GetChildCalled("Ok_Btn");
        _ok = _okBtnGO.GetComponent<UnityEngine.UI.Button>();


        AddressBuyRegionType();
    }

    private void AddressBuyRegionType()
    {
        if (_type != H.BuyRegion)
        {
            return;
        }


        //if has enough to buy
        var hasEnough = MeshController.BuyRegionManager1.HasEnoughResourcesToBuy();
        //if doesnt hide OkBtn
        if (!hasEnough)
        {
            //set text  WithOutMoney
            _textHere.text = String.Format(Languages.ReturnString(Type1 + ".WithOutMoney"), Str1);
            _okBtnGO.SetActive(false);
        }
        //set text WithMoney
        else
        {
            _textHere.text = String.Format(Languages.ReturnString(Type1 + ".WithMoney"), Str1);
        }
        _textHere.text += " Cost:" + MeshController.BuyRegionManager1.Cost();
    }
}
