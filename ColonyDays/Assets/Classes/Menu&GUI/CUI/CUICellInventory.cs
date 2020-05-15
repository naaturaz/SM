using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

internal class CUICellInventory : MonoBehaviour
{
    public TextMeshProUGUI Text;
    public TextMeshProUGUI DescText;
    public Image Image;
    public InvItem _invItem;
    private ScrollItemsWindow _which;

    private void Start()
    {
        StartCoroutine("OneSecUpdate");
    }

    public void SetInvItem(InvItem invItem, ScrollItemsWindow which)
    {
        _invItem = invItem;
        SetTextAndIcon(_invItem.Amount, _invItem.Key);
        gameObject.name = _invItem.Key + "";
        _which = which;
        HandleWhich();
    }

    private void HandleWhich()
    {
        if (_which == ScrollItemsWindow.Building)
        {
            DescText.text = Languages.ReturnString(_invItem.Key + "");
            Text.text = String.Format("{0:n1}", Unit.WeightConverted(_invItem.Amount));
        }
    }

    private void SetTextAndIcon(float text, P prod)
    {
        Text.text = ShortFormat(text);
        LoadIcon(prod);
        HandleWhich();
    }

    //String.Format("{0:n}", 1234);  // Output: 1,234.00
    //String.Format("{0:n0}", 9876); // No digits after the decimal point. Output: 9,876
    private string ShortFormat(float amt)
    {
        amt = Unit.WeightConverted(amt);

        if (amt < 10000)
            return String.Format("{0:n1}", amt);
        if (amt >= 1000000)
            return (int)(amt / 1000000) + "M";
        if (amt >= 100000)
            return (int)(amt / 1000) + "K";

        return String.Format("{0:n0}", amt);
    }

    protected void LoadIcon(P key = P.None)
    {
        var root = "Prefab/GUI/Inventory_Icons/" + key;
        Sprite sp = Resources.Load<Sprite>(root);

        if (sp == null)
        {
            root = "Prefab/GUI/Inventory_Icons/EmptyPNG";
            sp = Resources.Load<Sprite>(root);
        }

        Image.sprite = sp;
    }

    private IEnumerator OneSecUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(1); // wait
            if (_invItem != null)
            {
                SetTextAndIcon(_invItem.Amount, _invItem.Key);
            }
        }
    }

    /// <summary>
    /// Called from GUI
    /// </summary>
    public void ClickOnIt()
    {
        Program.MouseListener.ClickOnAnInvItem(_invItem);
    }
}