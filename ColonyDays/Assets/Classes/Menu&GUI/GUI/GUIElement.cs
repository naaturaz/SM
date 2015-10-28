using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIElement : General {

    //inipos is used for Hide and show 
    protected Vector3 iniPos;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Show()
    {
        transform.position = iniPos;
    }


    protected void MakeAlphaColorZero(Text g)
    {
        Color bl = Color.white;
        bl.a = 0f;
        g.color = bl;
    }

    protected void MakeAlphaColorMax(Text g)
    {
        Color bl = Color.white;
        bl.a = 255f;
        g.color = bl;
    }

    protected Rect GetRectFromBoxCollider2D(Transform t)
    {
        var res = new Rect();
        var min = t.GetComponent<BoxCollider2D>().bounds.min;
        var max = t.GetComponent<BoxCollider2D>().bounds.max;

        res = new Rect();
        res.min = min;
        res.max = max;

        return res;
    }

    /// <summary>
    /// Hides the element 
    /// </summary>
    public virtual void Hide()
    {
        Vector3 newPos = transform.position;
        newPos.y = -400f;

        transform.position = newPos;
    }

    /// <summary>
    /// Will build the string to show cost 
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    protected string BuildStringInv(General obj)
    {
        if (obj.Inventory == null)
        {
            return "Just bugs";
        }

        var res = "Inventory Cap:" + obj.Inventory.Capacity+"\n";
        var invItems = obj.Inventory.InventItems;

        for (int i = 0; i < invItems.Count; i++)
        {
            if (invItems[i].Amount > 0)
            {
                res += invItems[i].Key + ":" + invItems[i].Amount + "\n";
            }
        }

        return res;
    }
}
