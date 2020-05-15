using UnityEngine;
using UnityEngine.EventSystems;

public class CUIDragWindow : MonoBehaviour, IDragHandler
{
    [SerializeField] private RectTransform dragRectTransform;
    [SerializeField] private Canvas canvas;
    private string _name;

    private void Awake()
    {
        _name = dragRectTransform.gameObject.name;

        //Load pos
        var x = PlayerPrefs.GetFloat(_name + ".x");
        var y = PlayerPrefs.GetFloat(_name + ".y");

        var savedPos = new Vector2(x, y);
        if (savedPos != new Vector2())
            dragRectTransform.anchoredPosition = savedPos;
    }

    public void OnDrag(PointerEventData eventData)
    {
        dragRectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    private void OnApplicationQuit()
    {
        //Save pos
        PlayerPrefs.SetFloat(_name + ".x", dragRectTransform.anchoredPosition.x);
        PlayerPrefs.SetFloat(_name + ".y", dragRectTransform.anchoredPosition.y);
    }

    public void ResetSavedPos()
    {
        PlayerPrefs.SetFloat(_name + ".x", 0);
        PlayerPrefs.SetFloat(_name + ".y", 0);
        Debug.Log("resetui:" + _name);
    }
}