using UnityEngine;
using UnityEngine.UI;

public class Window : GUIElement
{
    protected Color _initialTabColor;
    Color _activeTabColor = Color.green;

    protected void ColorTabActive(GameObject go)
    {
        var img = go.GetComponent<Image>();
        img.color = _activeTabColor;
    }

    protected void ColorTabInactive(GameObject go)
    {
        if (_initialTabColor == null) return;

        var img = go.GetComponent<Image>();
        img.color = _initialTabColor;
    }
}
