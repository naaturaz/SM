using System.Collections.Generic;
/*
 * Each person and building will have one inventory
*/
public class Inventory  {

    //this is the items the inventory has inside
    private List<InvItem> _inventItems =  new List<InvItem>();
    private string _info;
    private string _locMyId;

    //Cubic meters of a Inventory
    private float _capacityVol;

    public List<InvItem> InventItems
    {
        get { return _inventItems; }
        set { _inventItems = value; }
    }

    public string LocMyId
    {
        get { return _locMyId; }
        set { _locMyId = value; }
    }

    public float CapacityVol
    {
        get { return _capacityVol; }
        set { _capacityVol = value; }
    }

    public Inventory(){}

    public Inventory(string myId, H hTypeP)
    {
        //LoadFromFile();
        _locMyId = myId;
        CapacityVol = Book.GiveMeStat(hTypeP).Capacity;
    }

    /// <summary>
    /// Will tell u wht amt of a specific type is on inventory 
    /// </summary>
    /// <param name="Key"></param>
    /// <returns></returns>
    public float ReturnAmtOfItemOnInv(P Key)
    {
        for (int i = 0; i < _inventItems.Count; i++)
        {
            if (Key == _inventItems[i].Key)
            {
                return _inventItems[i].Amount;
            }
        }
        return -1;
    }

    /// <summary>
    /// Will tell u wht amt of a specific key is 
    /// </summary>
    /// <param name="Key"></param>
    /// <returns></returns>
    public void SetAmtWithKey(P Key, float newVal)
    {
        for (int i = 0; i < _inventItems.Count; i++)
        {
            if (Key == _inventItems[i].Key)
            {
                _inventItems[i].Amount = newVal;
            }
        }
    }

    public void RemoveWithKey(P Key)
    {
        for (int i = 0; i < _inventItems.Count; i++)
        {
            if (Key == _inventItems[i].Key)
            {
                _inventItems.RemoveAt(i);
                return;
            }
        }
    }

    public bool IsItemOnInv(P Key)
    {
        var intT = ReturnAmtOfItemOnInv(Key);

        if (intT == -1)
        {
            return false;
        }
        return true;
    }

    void UpdateInfo()
    {
        if (IsItemOnInv(P.Corn))
        {
            _info = //"Potato:" + _inventItems[Inv.Potato] + "\n" +
             "Rice:" + ReturnAmtOfItemOnInv(P.Corn) + "\n" +
             ":" + "" + "\n";
        }
        else
        {
            _info = "Emptied";
        }
        //if is a building 
        if (BuildingPot.Control.Registro.AllBuilding.ContainsKey(_locMyId))
        {
            BuildingPot.Control.Registro.AllBuilding[_locMyId].UpdateInfo();
        }
    }

    /// <summary>
    /// The add of an item to the Inventory
    /// </summary>
    /// <param name="key"></param>
    /// <param name="amt"></param>
    public void Add(P key, float amt)
    {
        if (IsItemOnInv(key))
        {
            float intT = ReturnAmtOfItemOnInv(key) + amt;
            SetAmtWithKey(key, intT);
        }
        else
        {
            _inventItems.Add(new InvItem( key , amt));
            SetAmtWithKey(key,amt);
        }
        //UpdateInfo();
        ResaveOnRegistro();
        UpdateOnGameController(H.Add, key, amt);
    }
    
    /// <summary>
    /// The add of more that one item to the inventory
    /// </summary>
    /// <param name="items"></param>
    public void AddItems(List<InvItem> items)
    {
        for (int i = 0; i < items.Count; i++)
        {
            Add(items[i].Key, items[i].Amount);
        }
    }

    /// <summary>
    /// The int returned is the amount was removed from the inventory.
    /// If wht was asked in 'amt' was not fully returned means the inventory hadnt enough to coverred it
    /// 
    /// Then item will be removed from inventory
    /// </summary>
    /// <param name="key"></param>
    /// <param name="amt"></param>
    /// <returns></returns>
    public float Remove(P key, float amt)
    {
        if (IsItemOnInv(key))
        {
            //it means it can cover the amount asked to be removed
            if (ReturnAmtOfItemOnInv(key) - amt > 0)
            {
                float intT = ReturnAmtOfItemOnInv(key) - amt;
                SetAmtWithKey(key, intT);

                //UpdateInfo();
                return amt;
            }
            //other wise will depleted
            float t = ReturnAmtOfItemOnInv(key);

            SetAmtWithKey(key, 0);

            RemoveWithKey(key);

            //UpdateInfo();
            ResaveOnRegistro();
            UpdateOnGameController(H.Remove, key, amt);
            return t;
        }
        return 0;
    }

    public void RemoveItems(List<InvItem> items)
    {
        for (int i = 0; i < items.Count; i++)
        {
            Remove(items[i].Key, items[i].Amount);
        }
    }

    public string Info(){return _info;}

    public bool IsEmpty()
    {
        if (_inventItems.Count == 0) { return true;}
        return false;
    }

    public bool IsFull()
    {
        var total = 0f;
        for (int i = 0; i < _inventItems.Count; i++)
        {
            total += _inventItems[i].Volume;
        }

        if (total > _capacityVol)
        {
            return true;
        }
        return false;
    }

    public bool IsHasEnoughToCoverThisIngredient(InputElement ingredient)
    {
        var onInventory = IsItemOnInv(ingredient.Element);

        if (onInventory)
        {
            var amtOnInv = ReturnAmtOfItemOnInv(ingredient.Element);
            return amtOnInv > ingredient.Units;
        }

        return false;
    }

    internal void Delete()
    {
        _inventItems.Clear();
    }

    /// <summary>
    /// People will ask at their FoodSrc for this and once their are home too
    /// </summary>
    /// <returns></returns>
    public P GiveBestFood()
    {
        var listOfFoodPrd = ReturnListOfCatOfProd(PCat.Food);

        if (listOfFoodPrd.Count == 0)
        {
            return P.None;
        }

        var order = BuildingPot.Control.ProductionProp.Food1.OrderListByNutriVal(listOfFoodPrd);

        return order[0];
    }

    PCat CategorizeProd(P prod)
    {
        if (prod == P.Bean || prod == P.Potato || prod == P.SugarCane || prod == P.Corn
            || prod == P.Chicken || prod == P.Egg || prod == P.Pork || prod == P.Beef
            || prod == P.Fish  || prod == P.Sugar)
        {
            return PCat.Food;
        }
        else
        {
            return PCat.None;
        }
    }

    /// <summary>
    /// Will return a list with all the products in the inventory of tht category.
    /// For ex if is food will put all the food items 
    /// </summary>
    /// <param name="pCat"></param>
    /// <returns></returns>
    List<P> ReturnListOfCatOfProd(PCat pCat)
    {
        List<P> res = new List<P>();

        for (int i = 0; i < _inventItems.Count; i++)
        {
            var cateOfCurr = CategorizeProd(_inventItems[i].Key);
            if (cateOfCurr == pCat)
            {
                res.Add(_inventItems[i].Key);
            }
        }
        return res;
    }


    /// <summary>
    /// Use for see how much food is it in all Storages
    /// </summary>
    /// <param name="pCat"></param>
    /// <returns></returns>
    public float ReturnAmountOnCategory(PCat pCat)
    {
        List<P> foodItems = ReturnListOfCatOfProd(pCat);
        float res = 0;

        for (int i = 0; i < foodItems.Count; i++)
        {
            res += ReturnAmtOfItemOnInv(foodItems[i]);
        }

        return res;
    }

    /// <summary>
    /// Will return items of type  
    /// 
    /// Used to get all the food out of a person
    /// </summary>
    /// <returns></returns>
    public List<InvItem> ReturnAllItemsCat(PCat pCat)
    {
        List<InvItem> res = new List<InvItem>();

        for (int i = 0; i < _inventItems.Count; i++)
        {
            var cateOfCurr = CategorizeProd(_inventItems[i].Key);
            if (cateOfCurr == pCat)
            {
                res.Add(_inventItems[i]);
            }
        }
        return res;
    }

    /// <summary>
    /// Every time a inventory is changed need to be updated on Registro
    /// </summary>
    /// <param name="buildingId"></param>
    void ResaveOnRegistro()
    {
        int index = BuildingPot.Control.Registro.AllRegFile.FindIndex(a => a.MyId == LocMyId);

        //so it wont try it whhile loaiding 
        if (BuildingPot.Control.Registro.AllRegFile.Count == 0 || index == -1)
        {
            return;
        }

        BuildingPot.Control.Registro.AllRegFile[index].Inventory = this;
    }

    /// <summary>
    /// The action of updating the Fake Inventory on GameController. tht is te one shown
    /// on GUI
    /// </summary>
    /// <param name="action"></param>
    void UpdateOnGameController(H action, P item, float amt)
    {
        //building containing thsi inventory must be a Food Scr
        if (!BuildingPot.Control.FoodSources.Contains(LocMyId))
        {
            return;
        }

        MyText.ManualUpdate();
    }

    internal bool HasEnoughtCapacityToStoreThis(int amt)
    {
        if (CurrentStoreUsage() + amt > CapacityVol)
        {
            return false;
        }
        return true;
    }

    float CurrentStoreUsage()
    {
        float curr = 0;

        for (int i = 0; i < _inventItems.Count; i++)
        {
            curr += _inventItems[i].Amount;
        }

        return curr;
    }

    /// <summary>
    /// Will manage the Export Order. Will take from Inventory wht is needed and if order
    /// is fully covered will make it zero, other wise will remove wht was on inventory then 
    /// </summary>
    /// <param name="order"></param>
    internal Order ManageExportOrder(Order order)
    {
        float amtTaken = Remove(order.Product, order.Amount);

        //not all of the order was taken
        if (amtTaken < order.Amount)
        {
            order.Amount -= amtTaken;
        }
        //the whole order was taken 
        else if (amtTaken == order.Amount)
        {
            order.Amount = 0;
        }

        return order;
    }

    /// <summary>
    /// Will tell u the max amnt of a prod can take now the nventory
    /// </summary>
    /// <param name="p"></param>
    /// <returns></returns>
    internal float MaxAmtCanTakeOfAProd(P p)
    {
        var vol = ReturnProdVolume(p);
        var spaceNow = CapacityVol - CurrentStoreUsage();

        return spaceNow * vol;
    }

    /// <summary>
    /// Return how many units of one product can be stored on one unit of Volume wich could be metric cube
    /// </summary>
    /// <param name="p"></param>
    /// <returns></returns>
    int ReturnProdVolume(P p)
    {
        return 1;
    }
}

public class InvItem
{
    public P Key;
    private float _amount;
    private float _volume;

    /// <summary>
    /// How many KG of this Item 
    /// 
    /// Everytime is set.
    /// Will recalculte the Volume
    /// </summary>
    public float Amount
    {
        get { return _amount; }
        set
        {
            _amount = value;
            Volume = Program.gameScene.ExportImport1.CalculateVolume(Key, Amount);
        }
    }

    /// <summary>
    /// The Volume this item ocuppies
    /// </summary>
    public float Volume
    {
        get { return _volume; }
        set { _volume = value; }
    }

    public InvItem(P KeyP, float amtP)
    {
        Key = KeyP;
        Amount = amtP;
        Volume = Program.gameScene.ExportImport1.CalculateVolume(Key, amtP);
    }

    public InvItem() { }

}