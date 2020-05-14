using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

internal class CUICellInventory : MonoBehaviour
{
    public TextMeshProUGUI Text;
    public Image Image;
    public InvItem _invItem;

    private void Start()
    {
        StartCoroutine("OneSecUpdate");
    }

    private void Update()
    {
    }

    public void SetInvItem(InvItem invItem)
    {
        _invItem = invItem;
        SetTextAndIcon(_invItem.Amount + "", _invItem.Key);
        gameObject.name = _invItem.Key + "";
    }

    private void SetTextAndIcon(string text, P prod)
    {
        Text.text = text;
        LoadIcon(prod);
    }

    protected void LoadIcon(P key = P.None)
    {
        //var root = Program.gameScene.ExportImport1.ReturnIconRoot(key);
        var root = "Prefab/GUI/Inventory_Icons/" + key;
        Sprite sp = Resources.Load<Sprite>(root);

        //debug only bz all should have a root
        if (sp == null)//new Sprite()
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
                SetTextAndIcon(_invItem.Amount + "", _invItem.Key);
            }
        }
    }
}