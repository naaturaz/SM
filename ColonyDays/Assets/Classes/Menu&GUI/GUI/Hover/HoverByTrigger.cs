using UnityEngine;
using UnityEngine.EventSystems;

class HoverByTrigger : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    public bool IsAProductHover;

    private HoverWindow hoverWindow;//the window tht will pop up msg

    void Start()
    {
        if (hoverWindow == null)
        {
            hoverWindow = FindObjectOfType<HoverWindow>();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PublicSpawnHelp();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PublicDestroyHelp();
    }

    void OnGUI()
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
    void PublicSpawnHelp()
    {
        var pos = Hoverable.MousePositionTowardsScreenCenter();

        if(IsAProductHover)
            hoverWindow.ShowMsg(pos, MyMsg());
        else
            hoverWindow.Show(pos, MyMsg());
    }
    
    /// <summary>
    /// For unity event
    /// </summary>
    void PublicDestroyHelp()
    {
        hoverWindow.Hide();
    }

    string MyMsg()
    {
        return transform.name;
    }
}

