using UnityEngine;

internal class CUIScrollRectGO : MonoBehaviour
{
    public void OnMouseEnter()
    {
        //lock the camera scroll
        Program.IsMouseOnScrollableContent = true;
    }

    public void OnMouseExit()
    {
        //unlock the camera scroll
        Program.IsMouseOnScrollableContent = false;
    }
}