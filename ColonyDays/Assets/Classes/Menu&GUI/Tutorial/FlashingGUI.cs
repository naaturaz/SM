using UnityEngine;
using UnityEngine.UI;

class FlashingGUI: MonoBehaviour
{
    public Color InitialColor;
    public Color FlashToColor;

    Graphic _graphic;
    float _interval = 2;
    bool _goingUp = true;
    private Color _lerpedColor;

    private void Start()
    {
        _graphic = GetComponent<Graphic>();
        _lerpedColor = InitialColor;
    }

    private void Update()
    {
        _lerpedColor = Color.Lerp(InitialColor, FlashToColor, Mathf.PingPong(Time.time, _interval));
        _graphic.color = _lerpedColor;
    }

}
