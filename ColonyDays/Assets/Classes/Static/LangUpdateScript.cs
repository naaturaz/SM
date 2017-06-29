using UnityEngine;
using UnityEngine.UI;

public class LangUpdateScript : MonoBehaviour
{
    //Must be added in a Obt that has a Text elemente attached to it 

    //the initial value of this that always will be the key
    //to pull info from langagues.cs
    private string _key;

    private Text _text;

	// Use this for initialization
	void Start ()
	{
	    _text = GetComponent<Text>();
	    _key = _text.text;

	    _text.text = Languages.ReturnString(_key);
	}

    private void Update()
    {
        if (_key == _text.text && _text.text != Languages.ReturnString(_key))
        {
            _text.text = Languages.ReturnString(_key);
        }
    }
}
