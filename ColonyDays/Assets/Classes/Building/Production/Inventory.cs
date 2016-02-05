using System.Collections.Generic;
using UnityEngine;

/*
 * Each person and building will have one inventory
*/
public class Inventory  {

    //this is the items the inventory has inside
    private List<InvItem> _inventItems =  new List<InvItem>();
    private string _info;
    private string _locMyId;


    List<P> _foodItems=new List<P>(); 

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

    public List<P> FoodCatItems
    {
        get { return _foodItems; }
        set { _foodItems = value; }
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
                RemoveFromCategory(Key);
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
        if (key == P.None || amt == 0)
        {
            Debug.Log("ret Tried to add to inv:"+ key +" amt:"+ amt);
            return;
        }

        if (key.ToString().Contains("Random"))
        {
            Debug.Log("trace random");
            DealWithRandomOutput(key, amt);
            return;
        }

        if (IsItemOnInv(key))
        {
            float intT = ReturnAmtOfItemOnInv(key) + amt;
            SetAmtWithKey(key, intT);
        }
        else
        {
            AddToCategory(key);
            _inventItems.Add(new InvItem( key , amt));
            SetAmtWithKey(key,amt);
        }
        //UpdateInfo();
        ResaveOnRegistro();
    }

    private void AddToCategory(P key)
    {
        if (CategorizeProd(key)==PCat.Food)
        {
            _foodItems.Add(key);
        }
    }

    private void RemoveFromCategory(P key)
    {
        if (CategorizeProd(key) == PCat.Food)
        {
            _foodItems.Remove(key);
        }
    }

    /// <summary>
    /// Bz if is random has to be decompose 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="amt"></param>
    private void DealWithRandomOutput(P key, float amt)
    {
        var prdInfo = BuildingPot.Control.ProductionProp.ReturnProdInfoWithOutput(key);
        var listItems = prdInfo.DecomposeRandomLoad(amt);

        AddItems(listItems);
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
    /// <param name="kg"></param>
    /// <returns></returns>
    public float RemoveByWeight(P key, float kg)
    {
        if (IsItemOnInv(key))
        {
            //it means it can cover the amount asked to be removed
            if (ReturnAmtOfItemOnInv(key) - kg > 0)
            {
                float intT = ReturnAmtOfItemOnInv(key) - kg;
                SetAmtWithKey(key, intT);

                //UpdateInfo();
                return kg;
            }
            //other wise will depleted
            float t = ReturnAmtOfItemOnInv(key);

            SetAmtWithKey(key, 0);

            RemoveWithKey(key);

            //UpdateInfo();
            ResaveOnRegistro();
            return t;
        }
        return 0;
    }

    public void RemoveItems(List<InvItem> items)
    {
        for (int i = 0; i < items.Count; i++)
        {
            RemoveByWeight(items[i].Key, items[i].Amount);
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

    /// <summary>
    /// Will tell u if inventory is full for this prod
    /// </summary>
    /// <param name="prodP"></param>
    /// <returns></returns>
    public bool IsFullForThisProd(P prodP)
    {
        var invItem = _inventItems.Find(a => a.Key == prodP);

        if (invItem == null)
        {
            return false;
        }

        var total = invItem.Volume;
        var parts = ReturnPartThatBelongToThisProdInThisBuilding(prodP);

        if (total > _capacityVol / parts)
        {
            return true;
        }
        return false;
    }

    int ReturnPartThatBelongToThisProdInThisBuilding(P prod)
    {
        var build = Brain.GetBuildingFromKey(LocMyId);

        if (build == null)
        {
            Debug.Log("building not found:"+LocMyId);
            return 1;
        }

        var parts = BuildingPot.Control.ProductionProp.ReturnPartOfStorageThatBelongsToThisProd(build.HType, prod);
        return parts;
    }

    public float CurrentVolumeOcuppied()
    {
        var total = 0f;
        for (int i = 0; i < _inventItems.Count; i++)
        {
            total += _inventItems[i].Volume;
        }

        return total;
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
    /// Random is better so people get ramd stuff from Storages
    /// </summary>
    /// <returns></returns>
    public P GiveRandomFood()
    {
        var listOfFoodPrd = ReturnListOfCatOfProd(PCat.Food);

        if (listOfFoodPrd.Count == 0)
        {
            return P.None;
        }

        return listOfFoodPrd[Random.Range(0, listOfFoodPrd.Count)];
    }



    

    /// <summary>
    /// Gives the best food
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

        if (pCat==PCat.Food)
        {
            return FoodCatItems;
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
    /// Will tell u if the inventory has enought capacity to store this Load
    /// </summary>
    /// <param name="prod"></param>
    /// <param name="amt"></param>
    /// <returns></returns>
    internal bool HasEnoughtCapacityToStoreThis( P prod, float amt)
    {
        var volOfLoad = Program.gameScene.ExportImport1.CalculateVolume(prod, amt);

        if (CurrentVolumeOcuppied() + volOfLoad > CapacityVol)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// Will manage the Export Order. Will take from Inventory wht is needed and if order
    /// is fully covered will make it zero, other wise will remove wht was on inventory then 
    /// </summary>
    /// <param name="order"></param>
    internal Order ManageExportOrder(Order order)
    {
        float amtTaken = RemoveByWeight(order.Product, order.Amount);

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
    /// Will tell u the max amnt of a prod can take now the nventory on KG
    /// </summary>
    /// <param name="p"></param>
    /// <returns></returns>
    internal float MaxAmtOnKGCanTakeOfAProd(P p)
    {
        var spaceAvailNow = CapacityVol - CurrentVolumeOcuppied();

        if (spaceAvailNow < 0)
        {
            return 0;
        }

        return Program.gameScene.ExportImport1.CalculateMass(p, spaceAvailNow);
    }

    /// <summary>
    /// Will return the inv items needed to repair a ship of that Volume
    /// This volume is not the ship invetory volume is certainly bigger than that
    /// is the Whole Ship volume
    /// 
    /// This should refer to the inventory of a DryDock 
    /// 
    /// Will remove the items from the invenroty 
    /// </summary>
    /// <param name="vol"></param>
    /// <returns></returns>
    public List<InvItem> ReturnInvItemsForSize(float vol)
    {
        var iniVol = vol;
        List<InvItem> res = new List<InvItem>();
        float i = 0;
        float newI = 0;

        //doing a Pseud While loop
        for (i = 0; i < iniVol; i=i+newI)
        {
            //gets new item
            var newItem = GiveMeRandomItem(vol);
            //sets newI so its added to the loop
            newI = newItem.Volume;
            //removes vol value so is smaller so next iteration is not the whole thing anymore
            vol -= newItem.Volume;
            res.Add(newItem);
        }
        return res;
    }

    /// <summary>
    /// Will give u a random prod of max amt 'param'
    /// 
    /// Will remove the item from the invetory too 
    /// </summary>
    /// <param name="maxVol"></param>
    /// <returns></returns>
    InvItem GiveMeRandomItem(float maxVol)
    {
        //random index of Item
        var ind = Random.Range(0, InventItems.Count);
        //random amt
        var vol = Random.Range(0, InventItems[ind].Volume);

        //capping amt
        if (vol > maxVol)
        {
            vol = maxVol;
        }
        //will remove amt of item from Inventory
        RemoveByVolume(InventItems[ind].Key, vol);
        return new InvItem(InventItems[ind].Key, vol);
    }

    /// <summary>
    /// will remove by volume
    /// finds the weight and then removes by weight 
    /// </summary>
    /// <param name="p"></param>
    /// <param name="vol"></param>
    private void RemoveByVolume(P p, float vol)
    {
        var amt = Program.gameScene.ExportImport1.CalculateMass(p, vol);
        RemoveByWeight(p, amt);
    }

    /// <summary>
    /// Will say if this inventory contains 'prod'
    /// </summary>
    /// <param name="prod"></param>
    /// <returns></returns>
    internal bool Contains(P prod)
    {
        var prodHere = InventItems.Find(a => a.Key == prod);

        if (prodHere == null)
        {
            return false;
        }
        return true;
    }

    bool IfSpecialItem(P prod)
    {
        if (prod == P.Tool || prod == P.Cloth || prod == P.Ceramic || prod == P.Tonel || prod == P.Crate)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Will create a list of Evacation Orders . 
    /// So this Building invetory is all evacuated 
    /// </summary>
    /// <returns></returns>
    public List<Order> CreateOrderToEvacWholeInv()
    {
        List<Order> res = new List<Order>();

        for (int i = 0; i < InventItems.Count; i++)
        {
            Order order = new Order(InventItems[i].Key, "", LocMyId);
            res.Add(order);            
        }
        return res;
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