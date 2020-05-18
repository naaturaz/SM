using UnityEngine;
using UnityEngine.EventSystems;

public class CUIDragWindow : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    //must be the object that has possibly a GuiElement
    [SerializeField] private RectTransform dragRectTransform;

    [SerializeField] private Canvas canvas;
    private string _name;
    private int _count;
    private GUIElement _guiEle;

    private void Start()
    {
        _name = dragRectTransform.gameObject.name;

        //Initial Pos

        var inix = PlayerPrefs.GetFloat(_name + ".ini.x");
        var iniy = PlayerPrefs.GetFloat(_name + ".ini.y");
        var ini = new Vector2(inix, iniy);

        if (ini == new Vector2())
        {
            //Save ini pos
            PlayerPrefs.SetFloat(_name + ".ini.x", dragRectTransform.anchoredPosition.x);
            PlayerPrefs.SetFloat(_name + ".ini.y", dragRectTransform.anchoredPosition.y);
        }

        //Load pos
        var x = PlayerPrefs.GetFloat(_name + ".x");
        var y = PlayerPrefs.GetFloat(_name + ".y");

        _guiEle = dragRectTransform.gameObject.GetComponent<GUIElement>();

        var savedPos = new Vector2(x, y);
        //will get loaded right away in one that doesnt have guiElement
        if (savedPos != new Vector2() && _guiEle == null)
            dragRectTransform.anchoredPosition = savedPos;

        if (_guiEle != null && savedPos != new Vector2() && savedPos.y != -800f)
            _guiEle.IniPos = savedPos;
    }

    private void Update()
    {
        if (PlayerPrefs.GetInt("ResetUI") > 0)
        {
            _count++;

            if (PlayerPrefs.GetFloat(_name + ".x") != PlayerPrefs.GetFloat(_name + ".ini.x"))
            {
                ResetSavedPos();

                var x = PlayerPrefs.GetFloat(_name + ".ini.x");
                var y = PlayerPrefs.GetFloat(_name + ".ini.y");
                var ini = new Vector2(x, y);

                dragRectTransform.anchoredPosition = ini;
            }

            if (_count > 30)
            {
                _count = 0;
                PlayerPrefs.SetInt("ResetUI", 0);
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        dragRectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

        PlayerPrefs.SetFloat(_name + ".x", dragRectTransform.anchoredPosition.x);
        PlayerPrefs.SetFloat(_name + ".y", dragRectTransform.anchoredPosition.y);

        if (_guiEle != null)
            _guiEle.IniPos = dragRectTransform.anchoredPosition;
    }

    private void ResetSavedPos()
    {
        PlayerPrefs.SetFloat(_name + ".x", 0);
        PlayerPrefs.SetFloat(_name + ".y", 0);
        Debug.Log("resetui:" + _name);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //dragRectTransform.SetAsLastSibling();
    }
}