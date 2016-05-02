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
        newPos.y = -800f;

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

        var percentOcup = obj.Inventory.CurrentVolumeOcuppied()/obj.Inventory.CapacityVol;
        percentOcup = percentOcup*100;

        var volOccupied = Unit.VolConverted(obj.Inventory.CurrentVolumeOcuppied());
        var volCap = Unit.VolConverted(obj.Inventory.CapacityVol);

        var res = "Ocuppied:" + volOccupied.ToString("F1") +
            ". Inv Cap:" + volCap + " " + Unit.VolumeUnit() +" \n" +
            "Fill: "+percentOcup.ToString("F1") + "% \n";

        return res;
    }
}
