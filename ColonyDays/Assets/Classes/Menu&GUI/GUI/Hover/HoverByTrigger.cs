using UnityEngine;
using UnityEngine.EventSystems;

internal class HoverByTrigger : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    public bool IsAProductHover;

    private HoverWindow hoverWindow;//the window tht will pop up msg

    private void Start()
    {
        if (hoverWindow == null)
            hoverWindow = FindObjectOfType<HoverWindow>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PublicSpawnHelp();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PublicDestroyHelp();
    }

    private void OnGUI()
    {
        if (Event.current.type == EventType.KeyUp || Event.current.type == EventType.MouseUp)
        {
            PublicDestroyHelp();
        }
    }

    /// <
    /// summary>
    /// For unity eventTrigger
    ///
    /// </summary>
    private void PublicSpawnHelp()
    {
        var pos = Hoverable.MousePositionTowardsScreenCenter();

        if (IsAProductHover)
            hoverWindow.ShowMsg(MyMsg());
        else
            hoverWindow.Show(MyMsg());
    }

    /// <summary>
    /// For unity event
    /// </summary>
    private void PublicDestroyHelp()
    {
        hoverWindow.Hide();
    }

    private string MyMsg()
    {
        return transform.name;
    }
}