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

    GameObject _rateBtnGO;

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
        obj.transform.SetParent(container);

        obj.transform.localScale = localScale;
        obj.Type1 = type;
        obj.Str1 = str1;

        return obj;
    }

    void Start()
    {
        var t = GetChildCalled("TextHere");
        _textHere = t.GetComponentInChildren<Text>();

        _textHere.text = Languages.ReturnString(Type1 + "") + Str1;

        //doesnt have any of below
        if (Type1 == H.MandatoryFeedback)
        {
            return;
        }

        _okBtnGO = GetChildCalled("Ok_Btn");
        _ok = _okBtnGO.GetComponent<UnityEngine.UI.Button>();

        _rateBtnGO = GetChildCalled("Rate_Btn");
        if (_rateBtnGO != null)
        {
            _rateBtnGO.SetActive(false);

        }

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
        AddressInfoKeyedDialog();
        AddressCompleteQuest();
    }

    private void AddressInfoKeyedDialog()
    {
        if (_type != H.InfoKey)
        {
            return;
        }
        _textHere.text = Languages.ReturnString(Str1);
    }

    private void AddressCompleteQuest()
    {
        if (_type != H.CompleteQuest)
        {
            return;
        }
        _textHere.text = String.Format(Languages.ReturnString(Type1 + ""), Str1);
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
        _textHere.text += " Cost: " + MeshController.BuyRegionManager1.Cost();
    }



    internal void ValidateInvitation()
    {
        if (IsValidEmail(_inputTextEmail1.text)
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
            Dialog.CreateFile(Type1 + "", text);
            print("mail Send");
        }
        catch (Exception ex)
        {
            Dialog.OKDialog(H.Info, "Invitation was incorrect");
            print(ex.ToString());
        }
    }



    /// <summary>
    /// Called from GUI
    /// </summary>
    /// <param name="add"></param>
    public void AddToInput(string add)
    {
        var isGood = add == "Thumbs up" || add == "Love it" || add == "Best ever"
            || add == "Can't stop playing" || add == "I will recommend it";

        _inputText.text += add + ". \n";

        if (isGood)
        {
            _rateBtnGO.SetActive(true);
            _inputText.text += "Thanks for the support.Dev Team\n";
            PlayerPrefs.SetInt("Rate", 1);
        }
        else
        {
            PlayerPrefs.SetInt("Rate", 0);
        }

        if (add == "Game art is not good")
        {
            _inputText.text += "What is exactly what bothers you the most? Thanks\n";
        }
        if (add == "Game is confusing")
        {
            _inputText.text += "What is exactly what confuses you the most? Thanks\n";
        }
        if (add == "Like it a bit")
        {
            _inputText.text += "What you like the least and most? Thanks\n";
        }
        if (add == "Thumbs down")
        {
            _inputText.text += "How could we change that to 'Thumbs up'? Thanks\n";
        }
        if (add == "Is bad")
        {
            _inputText.text += "How could we change that to 'Is good!'? Thanks\n";
        }
    }

    public void Rate()
    {
        Application.OpenURL("http://store.steampowered.com/recommended/recommendgame/538990");
    }


    /// <summary>
    /// Called from GUI
    /// </summary>
    /// <param name="add"></param>
    public void ThumbsCall(string add)
    {
        Dialog.CreateFile("MandatoryFeedBack", add);
        Program.MouseClickListenerSt("Dialog.OKBtn");
    }



}
