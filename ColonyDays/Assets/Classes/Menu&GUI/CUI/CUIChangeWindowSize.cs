using UnityEngine;

public class CUIChangeWindowSize : MonoBehaviour
{
    [SerializeField] private RectTransform dragRectTransform;
    [SerializeField] private Canvas canvas;
    private string _name;

    private void Start()
    {
        _name = dragRectTransform.gameObject.name;

        //load
        //var ls = PlayerPrefs.GetFloat(_name + ".localScale");
        //if (ls != 0)
        //    dragRectTransform.localScale = new Vector3(ls, ls, ls);
    }

    public void ClickOnIncrease()
    {
        dragRectTransform.localScale *= 1.1f;
    }

    public void ClickOnDecrease()
    {
        dragRectTransform.localScale *= 0.9f;
    }

    private void OnApplicationQuit()
    {
        var x = dragRectTransform.localScale.x;
        var y = dragRectTransform.localScale.y;
        var z = dragRectTransform.localScale.z;

        //All values must be the same to be saved
        if (UMath.nearlyEqual(x, y, 0.01f) && UMath.nearlyEqual(z, y, 0.01f))
        {
            //Save
            PlayerPrefs.SetFloat(_name + ".localScale", dragRectTransform.localScale.x);
        }
        else
            Debug.Log("cant save localScale:" + _name);
    }
}