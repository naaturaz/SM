using UnityEngine;
using UnityEngine.UI;

internal class MainNotificationGO : GUIElement
{
    private Text _text;
    private RectTransform _rectTransform;
    private Vector3 _iniPos;

    private Image _image;

    private void Start()
    {
        _iniPos = transform.position;

        var childText = GetChildCalled("Text");
        _text = childText.GetComponent<Text>();

        var image = GetChildCalled("Image");
        _image = image.GetComponent<Image>();

        _rectTransform = transform.GetComponent<RectTransform>();

        Hide();
    }

    public void Hide()
    {
        _rectTransform.position = new Vector3(2500, 2500);
        _text.text = "";
    }

    internal void Show(string which)
    {
        transform.position = _iniPos;
        _text.text = Languages.ReturnString(which);

        _image.sprite = LoadIcons(which);
    }

    private Sprite LoadIcons(string key)
    {
        var root = "Prefab/GUI/Notification_Icons/"
            + key;
        Sprite sp = Resources.Load<Sprite>(root);

        //the default is 'Attention'
        if (sp == null)
        {
            root = "Prefab/GUI/Notification_Icons/Attention";
            sp = Resources.Load<Sprite>(root);
        }

        return sp;
    }

    private void OnGUI()
    {
        if (Event.current.type == EventType.MouseUp)
        {
            Hide();
        }
    }
}