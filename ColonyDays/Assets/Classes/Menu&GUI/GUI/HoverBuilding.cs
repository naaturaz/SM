using UnityEngine;

/// <summary>
/// This script will be attaced to all Slots so they can call the Windows Description
/// </summary>
public class HoverBuilding : MonoBehaviour
{
    private Rect myRect;//the rect area of my element. Must have attached a BoxCollider2D
    private BuildingDescriptionMenu _descriptionWindow;//the window tht will pop up msg

    // Use this for initialization
    private void Start()
    {
        //for this to work only one gameObj can have the HoverWindow attached
        if (_descriptionWindow == null)
        {
            _descriptionWindow = FindObjectOfType<BuildingDescriptionMenu>();
        }
    }

    public void InitObjects()
    {
        myRect = GetRectFromBoxCollider2D();
    }

    private Rect GetRectFromBoxCollider2D()
    {
        var res = new Rect();
        var min = transform.GetComponent<BoxCollider2D>().bounds.min;
        var max = transform.GetComponent<BoxCollider2D>().bounds.max;

        res = new Rect();
        res.min = min;
        res.max = max;

        return res;
    }

    // Update is called once per frame
    private void Update()
    {
        //if got in my area
        if (myRect.Contains(Input.mousePosition))
        {
            SpawnHelp();
        }
        //ig got out
        else if (!myRect.Contains(Input.mousePosition) && ReturnMyType() == _descriptionWindow.CurrentType()
            && ReturnMyType() != H.None)//to avoid None killing tht window
        {
            var therType = _descriptionWindow.CurrentType();
            var myT = ReturnMyType();

            DestroyHelp();
        }
    }

    private void SpawnHelp()
    {
        var myType = ReturnMyType();

        if (myType != H.None)
        {
            _descriptionWindow.Show(myType);
        }
    }

    private H ReturnMyType()
    {
        return Program.MouseListener.ReturnThisSlotVal(name);
    }

    private void DestroyHelp()
    {
        _descriptionWindow.Hide();
    }
}