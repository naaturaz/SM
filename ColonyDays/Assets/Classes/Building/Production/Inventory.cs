using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
 * Each person and building will have one inventory
*/

public class Inventory
{
    //this is the items the inventory has inside
    private List<InvItem> _inventItems = new List<InvItem>();

    private string _info;
    private string _locMyId;
    private bool _isAStorage;
    private H _hType;

    private List<P> _foodItems = new List<P>();

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

    /// <summary>
    /// Needed to know bz if is one will report to gameInventory
    /// </summary>
    public bool IsAStorage
    {
        get { return _isAStorage; }
        set { _isAStorage = value; }
    }

    public H HType
    {
        get { return _hType; }
        set { _hType = value; }
    }

    public Inventory()
    {
    }

    public Inventory(string myId, H hTypeP)
    {
        _hType = hTypeP;
        _locMyId = myId;
        CapacityVol = Book.GiveMeStat(hTypeP).Capacity;

        if (_locMyId.Contains("Storage"))
        {
            IsAStorage = true;
        }

        if (hTypeP == H.YearReport)
        {//bz is called add will stop when the amt is zero //+1 is bz zero doesnt get added
            _inventItems.Insert(0, new InvItem(P.Year, float.Parse(LocMyId) + 1));
            //_inventItems.Add(new InvItem(P.Year, float.Parse(LocMyId) + 1));
        }
    }

    /// <summary>
    /// Will order items alphabetically
    /// </summary>
    internal void OrderItemsAlpha()
    {
        OrderItemsAlphaLang();
    }

    /// <summary>
    /// Will order items alphabetically
    /// </summary>
    private void OrderItemsAlphaLang()
    {
        InvItem year = null;
        for (int i = 0; i < _inventItems.Count; i++)
        {
            _inventItems[i].SetValueCurrentLang();
            if (_inventItems[i].Key == P.Year)
            {
                year = _inventItems[i];
                _inventItems.RemoveAt(i);
                i--;
            }
        }

        _inventItems = _inventItems.OrderBy(a => a.KeyLang.ToString()).ToList();

        if (year != null)
            _inventItems.Insert(0, year);
    }

    /// <summary>
    /// Will tell u wht amt of a specific type is on inventory
    /// </summary>
    /// <param name="Key"></param>
    /// <returns></returns>
    public float ReturnAmtOfItemOnInv(P Key)
    {
        if (Key == P.RandomMineOutput)
        {
            return ReturnAllAmountOnInv();
        }

        for (int i = 0; i < _inventItems.Count; i++)
        {
            if (Key == _inventItems[i].Key)
            {
                return _inventItems[i].Amount;
            }
        }
        return 0;
    }

    /// <summary>
    /// Will tell u wht amt of a specific key is
    /// </summary>
    /// <param name="Key"></param>
    /// <returns></returns>
    private InvItem SetAmtWithKey(P Key, float newVal)
    {
        for (int i = 0; i < _inventItems.Count; i++)
        {
            if (Key == _inventItems[i].Key)
            {
                _inventItems[i].Amount = newVal;
                return _inventItems[i];
            }
        }
        return null;
    }

    private void RemoveWithKey(P Key)
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

        if (intT <= 0)
        {
            return false;
        }
        return true;
    }

    private void UpdateInfo()
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
    public void Add(P key, float amt, MDate expiration = null)
    {
        if (key == P.None || key == P.Food || amt == 0)
        {
            //Debug.Log("ret Tried to add to inv:"+ key +" amt:"+ amt);
            return;
        }
        if (key == P.RandomMineOutput || key == P.RandomFoundryOutput)
        {
            //Debug.Log("trace random");
            DealWithRandomOutput(key, amt);
            return;
        }

        //in this way a new expiration date is going to be set as soon a prod is
        //added to a new inventory. is not pefect bz doesnt carry old expiration date
        //foward but at the same time the majority of the product expires as stays behind and
        //gets check in the inventory
        var expireCalc = ExpirationDate(key, expiration);
        InvItem curr = null;
        if (IsItemOnInv(key))
        {
            float intT = ReturnAmtOfItemOnInv(key) + amt;
            curr = SetAmtWithKey(key, intT);
        }
        else
        {
            AddToCategory(key);
            _inventItems.Add(new InvItem(key, amt));
            curr = SetAmtWithKey(key, amt);
        }
        SetAmtExpiration(curr, amt, expireCalc);
        AddressGameInventory(key, amt, true);
    }

    #region Expiration

    /// <summary>
    /// Will calcluate the expiration date of a product
    ///
    /// if already has one will return that one that has already
    ///
    /// if product doestn have expiration will return null
    /// </summary>
    /// <param name="prod"></param>
    /// <param name="current"></param>
    /// <returns></returns>
    public static MDate ExpirationDate(P prod, MDate current = null)
    {
        if (current != null)
        {
            return current;
        }

        var days = Program.gameScene.ExportImport1.ReturnExpireDays(prod);
        if (days == -1)
        {
            return null;
        }
        return Program.gameScene.GameTime1.ReturnCurrentDatePlsAdded(days);
    }

    private void SetAmtExpiration(InvItem item, float amt, MDate expiration)
    {
        if (expiration == null || item == null)
        {
            return;
        }

        item.AddExpirationDate(amt, expiration, this);
    }

    private void CheckIfAnAmtHasExpired(InvItem curr)
    {
        curr.CheckIfAnyHasExpired(this);
    }

    #endregion Expiration

    private void AddressGameInventory(P key, float amt, bool add)
    {
        if (!IsAStorage)
            return;

        if (key == P.Wood)
        {
            int aa = 1;
        }

        if (add)
            GameController.ResumenInventory1.GameInventory.Add(key, amt);
        else
            GameController.ResumenInventory1.GameInventory.RemoveByWeight(key, amt);
    }

    #region Main Inventory

    public void AddToSpecialInv(P key)
    {
        InventItems.Add(new InvItem(key, 0));
    }

    public void AddToSpecialInv(string key)
    {
        InventItems.Add(new InvItem(key));
    }

    internal void RemoveFromSpecialInv(string buildMyId)
    {
        for (int i = 0; i < InventItems.Count; i++)
        {
            if (InventItems[i].Info == buildMyId)
            {
                InventItems.RemoveAt(i);
                return;
            }
        }
    }

    public void SetToSpecialInv(P key, float amt)
    {
        SetAmtWithKey(key, amt);
    }

    #endregion Main Inventory

    private void AddToCategory(P key)
    {
        if (CategorizeProd(key) == PCat.Food)
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
        InvItem curr = null;

        if (IsItemOnInv(key))
        {
            //it means it can cover the amount asked to be removed
            if (ReturnAmtOfItemOnInv(key) - kg > 0)
            {
                float intT = ReturnAmtOfItemOnInv(key) - kg;
                curr = SetAmtWithKey(key, intT);
                CheckIfAnAmtHasExpired(curr);

                AddressGameInventory(key, kg, false);
                return kg;
            }
            //other wise will depleted
            float t = ReturnAmtOfItemOnInv(key);
            curr = SetAmtWithKey(key, 0);
            CheckIfAnAmtHasExpired(curr);

            RemoveWithKey(key);
            AddressGameInventory(key, kg, false);
            return t;
        }
        //storages has a well attached
        if (key == P.Water && LocMyId.Contains("Storage"))
        {
            return kg;
        }

        //docker having negative amount of item on inventory
        RemoveWithKey(key);
        return 0;
    }

    public void RemoveItems(List<InvItem> items)
    {
        for (int i = 0; i < items.Count; i++)
        {
            RemoveByWeight(items[i].Key, items[i].Amount);
        }
    }

    /// <summary>
    /// Will remove an item from the Inv. U should know tht item amt is zero to call this
    /// </summary>
    /// <param name="key"></param>
    internal void RemoveItem(P key)
    {
        RemoveWithKey(key);
    }

    public string Info()
    {
        return _info;
    }

    public bool IsEmpty()
    {
        if (_inventItems.Count == 0) { return true; }
        return false;
    }

    public bool IsFull()
    {
        var total = 0f;
        for (int i = 0; i < _inventItems.Count; i++)
        {
            total += _inventItems[i].Volume;
        }

        NotifyIfFilling(total);

        if (total > _capacityVol)
        {
            return true;
        }
        return false;
    }

    private float lastNoti;//so it notifies every 3min

    private void NotifyIfFilling(float currentOccupied)
    {
        var perc = currentOccupied / _capacityVol;
        if (perc > 0.74f && (lastNoti + NotificationsManager.NotiFrec < Time.time || lastNoti == 0) && IsAStorage)
        {
            lastNoti = Time.time;
            var realPerc = perc * 100;
            if (realPerc > 100)
            {
                //so it doesnt show Storage is at 115%
                realPerc = 100;
            }
            Program.gameScene.GameController1.NotificationsManager1.Notify("FullStore", realPerc.ToString("N0"));
        }
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

    private int ReturnPartThatBelongToThisProdInThisBuilding(P prod)
    {
        var build = Brain.GetBuildingFromKey(LocMyId);

        if (build == null)
        {
            //Debug.Log("building not found:"+LocMyId);
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

    public float CurrentKGsOnInv()
    {
        var total = 0f;
        for (int i = 0; i < _inventItems.Count; i++)
        {
            total += _inventItems[i].Amount;
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

    /// <summary>
    /// Deletes the whole invnetory
    /// </summary>
    internal void Delete()
    {
        _inventItems.Clear();
        _foodItems.Clear();
    }

    /// <summary>
    /// Random is better so people get ramd stuff from Storages
    /// </summary>
    /// <returns></returns>
    public P GiveRandomFood()
    {
        if (_foodItems.Count == 0)
        {
            return P.None;
        }
        return _foodItems[UnityEngine.Random.Range(0, _foodItems.Count)];
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

    public static PCat CategorizeProd(P prod)
    {
        if (prod == P.Water ||

            prod == P.Bean || prod == P.Potato || prod == P.SugarCane || prod == P.Corn
            || prod == P.Chicken || prod == P.Egg || prod == P.Pork || prod == P.Beef
            || prod == P.Fish //||prod == P.Sugar
            || prod == P.Coconut || prod == P.Banana

            //prod == P.CornFlower
            || prod == P.Bread || prod == P.Carrot || prod == P.Tomato
            || prod == P.Cucumber || prod == P.Cabbage || prod == P.Lettuce || prod == P.SweetPotato
            || prod == P.Cassava || prod == P.Pineapple
            //|| prod == P.Mango || prod == P.Avocado || prod == P.Guava || prod == P.Orange
            || prod == P.Papaya || prod == P.Chocolate //|| prod == P.Candy
            )
        {
            return PCat.Food;
        }
        return PCat.None;
    }

    /// <summary>
    /// Will return a list with all the products in the inventory of tht category.
    /// For ex if is food will put all the food items
    /// </summary>
    /// <param name="pCat"></param>
    /// <returns></returns>
    private List<P> ReturnListOfCatOfProd(PCat pCat)
    {
        List<P> res = new List<P>();

        if (pCat == PCat.Food)
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
    /// Use for see how much food is it in all Storages
    /// </summary>
    /// <param name="pCat"></param>
    /// <returns></returns>
    public float ReturnAllAmountOnInv()
    {
        float res = 0;
        for (int i = 0; i < _inventItems.Count; i++)
        {
            res += ReturnAmtOfItemOnInv(_inventItems[i].Key);
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
    /// Will tell u if the inventory has enought capacity to store this Load
    /// </summary>
    /// <param name="prod"></param>
    /// <param name="amt"></param>
    /// <returns></returns>
    internal bool HasEnoughtCapacityToStoreThis(P prod, float amt)
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
            order.ChangeAmountBy(-amtTaken);
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
        for (i = 0; i < iniVol; i = i + newI)
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
    private InvItem GiveMeRandomItem(float maxVol)
    {
        //random index of Item
        var ind = UnityEngine.Random.Range(0, InventItems.Count);
        //random amt
        var vol = UnityEngine.Random.Range(0, InventItems[ind].Volume);

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

    private bool IfSpecialItem(P prod)
    {
        if (prod == P.Tool || prod == P.Cloth || prod == P.Crockery || prod == P.Barrel || prod == P.Crate)
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

    /// <summary>
    ///
    /// </summary>
    /// <param name="pCat"></param>
    /// <param name="amtNeeded"></param>
    /// <returns>Returns wht is left from amt needed. if is 0 is was fully covered</returns>
    internal float RemoveByCategory(PCat pCat, float amtNeeded)
    {
        var items = ReturnAllItemsCat(pCat);

        for (int i = 0; i < items.Count; i++)
        {
            //if the items is not able to cover it all needed
            if (items[i].Amount < amtNeeded)
            {
                //amt Needed being covered
                amtNeeded -= items[i].Amount;
                //will delete tht item from inventory
                RemoveByWeight(items[i].Key, items[i].Amount);
            }
            //if can cover the rest needed
            else
            {
                RemoveByWeight(items[i].Key, amtNeeded);
                amtNeeded = 0;
            }
        }
        return amtNeeded;
    }

    public bool DoWeHaveOfThisCat(PCat cat)
    {
        return ReturnAmountOnCategory(cat) > 0;
    }

    internal bool IsCarryingLiquid()
    {
        if (_inventItems.Count == 0)
        {
            return false;
        }

        return IsLiquid(_inventItems[0].Key);
    }

    #region Containers

    private static bool IsLiquid(P prod)
    {
        return prod == P.Water || prod == P.Beer || prod == P.Rum || prod == P.Ink || prod == P.Clay;
    }

    public static bool ThereIsContainerForThis(P prod)
    {
        if (IsLiquid(prod))
        {
            return GameController.AreThereTonelsOnStorage;
        }
        else if (DoesNeedACrate(prod))
        {
            return GameController.AreThereCratesOnStorage;
        }
        return true;
    }

    public static void RemoveContainerUsed(P prod)
    {
        if (DoesNeedACrate(prod))
        {
            //each time a person uses a crrate
            //they get used and diminished
            GameController.ResumenInventory1.Remove(P.Crate, .01f);
        }
        else if (IsLiquid(prod))
        {
            GameController.ResumenInventory1.Remove(P.Barrel, .01f);
        }
    }

    private static bool DoesNeedACrate(P prod)
    {
        if (prod == P.Wood || prod == P.Stone || prod == P.Ore || IsLiquid(prod))
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// Created so 'Storages' appear to have water
    /// </summary>
    /// <returns></returns>
    internal bool DoWeHaveWater()
    {
        return LocMyId.Contains("Storage") || Contains(P.Water);
    }

    #endregion Containers

    /// <summary>
    /// Check if a this inventory has Stale product
    /// Stale is a product that is not in any order and is still in inventory
    /// </summary>
    /// <param name="dispatch1"></param>
    internal void CheckIfStaleInvetory(Dispatch dispatch1)
    {
        for (int i = 0; i < _inventItems.Count; i++)
        {
            if (!dispatch1.DoYouHaveAtLeastAnOrderWithMyProduct(_inventItems[i].Key))
            {
                RemoveItem(_inventItems[i].Key);
            }
        }
    }
}

public class InvItem
{
    public P Key;
    private float _amount;
    private float _volume;
    private string _info;//for year for reports
    private List<SubInvItem> _expiresAmts = new List<SubInvItem>();

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

    public string Info
    {
        get { return _info; }
        set { _info = value; }
    }

    public List<SubInvItem> ExpiresAmts
    {
        get { return _expiresAmts; }
        set { _expiresAmts = value; }
    }

    public string KeyLang { get; internal set; }

    public InvItem(P KeyP, float amtP)
    {
        Key = KeyP;
        Amount = amtP;
        Volume = Program.gameScene.ExportImport1.CalculateVolume(Key, amtP);
    }

    public InvItem(string info)
    {
        _info = info;
    }

    public InvItem()
    {
    }

    internal void AddExpirationDate(float amt, MDate expiration, Inventory inv)
    {
        //no expiration needed for this 2 below
        if (inv.HType == H.Person || inv.HType == H.None || inv.HType == H.YearReport || !Program.IsFood)
        {
            return;
        }

        //found is lumping expiration dates by month and year so if day is not the same
        //will be still lumped toghether
        var found = _expiresAmts.Find(a => a.Expires.Month1 == expiration.Month1 &&
            a.Expires.Year == expiration.Year);
        if (found != null)
        {
            found.Amt += amt;
        }
        else
        {
            _expiresAmts.Add(new SubInvItem(amt, expiration));
        }
    }

    internal void CheckIfAnyHasExpired(Inventory inv)
    {
        if (_expiresAmts.Count == 0 || !Program.IsFood)
        {
            return;
        }

        if (GameTime.IsPastOrNow(_expiresAmts[0].Expires))
        {
            BulletinWindow.AddProduction(Key, _expiresAmts[0].Amt, "Expire");
            if (inv.IsAStorage)
            {
                GameController.ResumenInventory1.GameInventory.RemoveByWeight(Key, _expiresAmts[0].Amt);
            }

            Amount -= _expiresAmts[0].Amt;
            if (Amount <= 0)
            {
                Amount = 0;
                inv.RemoveItem(Key);
            }

            _expiresAmts.RemoveAt(0);
            CheckIfAnyHasExpired(inv);
        }
    }

    internal void SetValueCurrentLang()
    {
        KeyLang = Languages.ReturnString(Key + "");
    }
}

/// <summary>
/// So the expirattion of a product can be done
/// </summary>
public class SubInvItem
{
    private float _amt;
    private MDate _expires;

    public SubInvItem()
    {
    }

    public SubInvItem(float amt, MDate expires)
    {
        _amt = amt;
        _expires = expires;
    }

    public float Amt
    {
        get { return _amt; }
        set { _amt = value; }
    }

    public MDate Expires
    {
        get { return _expires; }
        set { _expires = value; }
    }
}