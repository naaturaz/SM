using UnityEngine;
using UnityEngine.UI;

internal class FlashingGUI : MonoBehaviour
{
    public Color InitialColor;
    public Color FlashToColor;

    private Graphic _graphic;
    private float _interval = 2;
    private bool _goingUp = true;
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