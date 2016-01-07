using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

/*
 * Script added to order to be shown in the Orders tab in the dock.
 * 
 * Each one will have one of this script added 
 */

public class OrderShow : GUIElement
{
    private P _prod;
    private float _amt;

    private Text _title;

    private UnityEngine.UI.Button _button;//

	// Use this for initialization
	void Start ()
    {
        _title = GetChildCalled(H.Title).GetComponent<Text>();

	    var rawBtn = GetChildCalled(H.Remove_Order_Btn);

        //check for the type or OrderShow that doesnt have the Close btn. 
	    if (rawBtn != null)
	    {
            _button = rawBtn.GetComponent<UnityEngine.UI.Button>();
	    }

        
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    static public OrderShow Create(string root,  Transform container)
    {
        OrderShow obj = null;
        obj = (OrderShow)Resources.Load(root, typeof(OrderShow));
        obj = (OrderShow)Instantiate(obj, new Vector3(), Quaternion.identity);
        

        obj.transform.parent = container; 

        return obj;
    }

    public void Show(Order order)
    {
        _prod = order.Product;
        _amt = order.Amount;

        Start();

        _title.text = _prod + " : " + _amt;
        transform.name = _title.text + " | " + Id;

        if (_button == null)
        {
            return;
        }

        _button.onClick.AddListener(() => Program.MouseClickListenerSt("AddOrder.Remove."+order.ID));
    }

    /// <summary>
    /// Resets the position of the element 
    /// </summary>
    /// <param name="i"></param>
    /// <param name="type"></param>
    internal void Reset(int i, H type, Vector3 orderPos, Vector3 onProcessOrderPos)
    {
        var rectT = GetComponent<RectTransform>();
        rectT.position = new Vector3();

        var x = 0;

        if (type == H.None)
        {
            x = ReturnPixelsToTheRight();
        }

        //bz if is on process goes down
        var yPls = AddYSpaceIfIsOnProcess(orderPos, onProcessOrderPos);

        rectT.localPosition = new Vector3(x, (-4 * i) - yPls, 0);

        transform.localScale = new Vector3(1,1,1);
    }


    float AddYSpaceIfIsOnProcess(Vector3 orderPos, Vector3 onProcessOrderPos)
    {
        //order that is not on process yet
        if (_button!=null)
        {
            return 0f;
        }

        //on Process order 
        //var yDiff = Mathf.Abs(orderPos.y - onProcessOrderPos.y);
        return 62f;
    }

    /// <summary>
    /// THis is to address when is export should be moved to the right some pixesl
    /// 
    /// Here have to address when screen is smaller. Or dif ratio
    /// </summary>
    /// <returns></returns>
    int ReturnPixelsToTheRight()
    {
        return 233;
    }
}
