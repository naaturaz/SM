using UnityEngine;
using UnityEngine.UI;

public class LangUpdateScript : MonoBehaviour
{
    private Text _text;

    public string Key { get; set; }

    // Use this for initialization
    private void Start()
    {
        _text = GetComponent<Text>();

        if (Key == null)
            Key = _text.text;

        _text.text = Languages.ReturnString(Key);
    }

    private void Update()
    {
        if (Key == _text.text && _text.text != Languages.ReturnString(Key))
        {
            _text.text = Languages.ReturnString(Key);
        }
    }

    internal void SetKey(string newKey)
    {
        Key = newKey;
        if (_text == null) return;
        _text.text = Languages.ReturnString(Key);
    }
}