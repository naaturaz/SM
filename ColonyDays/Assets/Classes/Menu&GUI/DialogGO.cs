using System;
using System.Net.Mail;
using System.Text.RegularExpressions;
//using Steamworks;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The dialog GameObject 
/// </summary>
class DialogGO : GUIElement
{

    private H _type;//type of Dialog 
    private Text _textHere;
    private string _str1;

    private GameObject _okBtnGO;
    private UnityEngine.UI.Button _ok;

    private InputField _inputText;

    private InputField _inputTextEmail1;
    private InputField _inputTextEmail2;

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

    public InputField InputText
    {
        get { return _inputText; }
        set { _inputText = value; }
    }

    static public DialogGO Create(string root, Transform container, Vector3 iniPos, H type, string str1 = "")
    {
        DialogGO obj = null;

        obj = (DialogGO)Resources.Load(root, typeof(DialogGO));
        obj = (DialogGO)Instantiate(obj, new Vector3(), Quaternion.identity);

        var localScale = obj.transform.localScale;

        obj.transform.position = iniPos;
        obj.transform.SetParent( container);

        obj.transform.localScale = localScale;
        obj.Type1 = type;
        obj.Str1 = str1;

        return obj;
    }

    void Start()
    {
        var t = GetChildCalled("TextHere");
        _textHere = t.GetComponentInChildren<Text>();

        _textHere.text = Languages.ReturnString(Type1+"") + Str1;

        _okBtnGO = GetChildCalled("Ok_Btn");
        _ok = _okBtnGO.GetComponent<UnityEngine.UI.Button>();


        var inText = GetChildCalled("Input_Field");
        if (inText != null)
        {
            InputText = inText.GetComponent<InputField>();
        }

        //for email invitation for private beta
        //Input_Field_Email_1
        var email1 = GetChildCalled("Input_Field_Email_1");
        if (email1 != null)
        {
            _inputTextEmail1 = email1.GetComponent<InputField>();
        }

        var email2 = GetChildCalled("Input_Field_Email_2");
        if (email2 != null)
        {
            _inputTextEmail2 = email2.GetComponent<InputField>();
        }


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



    internal void ValidateInvitation()
    {
        if (_inputTextEmail1.text == _inputTextEmail2.text && IsValidEmail(_inputTextEmail1.text)
            && !string.IsNullOrEmpty(_inputTextEmail1.text))
        {
            CreateInviteOnThisPC();
            Dialog.OKDialog(H.Info, "Invitation sent");
        }
        else
        {
            Dialog.OKDialog(H.Info, "Invitation was incorrect");
        }
    }

    bool IsValidEmail(string emailaddress)
    {
        Regex regPass = new Regex(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?");

        if (regPass.IsMatch(emailaddress))
        {
            return true;
        }
        return false;
    }

    private void CreateInviteOnThisPC()
    {        
        var text = "Email: " + _inputTextEmail1.text;
        //if (SteamFriends.GetPersonaName() != null)
        //{
        //    text = text + "\n\nReferred by: " + SteamFriends.GetPersonaName();
        //}

        try
        {
            Dialog.CreateFile(Type1+"", text);   
            print("mail Send");
        }
        catch (Exception ex)
        {
            Dialog.OKDialog(H.Info, "Invitation was incorrect");
            print(ex.ToString());
        }
    }





}
