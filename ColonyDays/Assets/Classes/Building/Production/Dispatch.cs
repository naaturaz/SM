using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using UnityEngine;

/*
 * Used for keep track and organization of the orders for WheelBarrowers
 * 
 * Regular orders are the one place for example for the SmallPrint when they need paper. So
 * U know the DestinyBuild
 * 
 * Evacuation orders are place when deleting a FoodSrc or a Factory storage is full. So the SourceBuilding
 * is known wht is needed is a destiniyBuild to get rid of the products
 * 
 * 
 * In the Dock case tht has a Dispatch too 
 * the Import are evac orders
 * and Export are regular orders
 * 
 */

public class Dispatch
{
    private bool _isUsingPrimary;// IF true we are using the Primary _orders list
    List<Order> _orders = new List<Order>();//the current orders
    //the old orders kept here in the order they were processed so once we run out of order will
    //start from the beginning of recicled and will put it at the end , so in that way WheelBarrowers
    //will keep working , even if we have no orders 
    List<Order> _recycledOrders = new List<Order>();
    //here will be added Evacuated orders than cant find a spot into any Food Src
    List<Order> _dormantOrders = new List<Order>();
    private H _type = H.None;//type of Dispatch. So for there is the Regular one and Dock

    List<Order> _expImpOrders = new List<Order>(); 

    public bool IsUsingPrimary
    {
        get { return _isUsingPrimary; }
        set { _isUsingPrimary = value; }
    }

    public List<Order> Orders
    {
        get { return _orders; }
        set { _orders = value; }
    }

    public List<Order> RecycledOrders
    {
        get { return _recycledOrders; }
        set { _recycledOrders = value; }
    }

    public List<Order> DormantOrders
    {
        get { return _dormantOrders; }
        set { _dormantOrders = value; }
    }

    public H Type
    {
        get { return _type; }
        set { _type = value; }
    }

    public List<Order> ExpImpOrders
    {
        get { return _expImpOrders; }
        set { _expImpOrders = value; }
    }

    public Dispatch() { }

    public Dispatch(H type)
    {
        this._type = type;
    }

    /// <summary>
    /// Will return the Orders if at least is one other wise the RecycledOrders
    /// </summary>
    /// <returns></returns>
    List<Order> ReturnCurrentList()
    {
        if (Orders.Count > 0)
        {
            return Orders;
        }
        return _recycledOrders;
    }

    /// <summary>
    /// Will define _isUsingPrimary
    /// </summary>
    bool DefineUsingPrimary(List<Order> list )
    {
        if (list == Orders)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Will return the cureent list of orders and will define _isUsingPrimary
    /// </summary>
    /// <returns></returns>
    List<Order> ReturnCurrentListAndDefinePrimary()
    {
        var list = ReturnCurrentList();
        _isUsingPrimary = DefineUsingPrimary(list);
        return list;
    }

    /// <summary>
    /// This is the public method that building problably a factory of some kind 
    /// will call when have zero of a ingredient in their inventory
    /// so an order will be on place, so WheelBarrowers can find where to pull the ingredient from if is 
    /// in  any Storage 
    /// </summary>
    /// <param name="prod">The product/ingredient  is needed</param>
    public void AddToOrders(Order prod)
    {
        if (!ListContains(Orders, prod))
        {
            Debug.Log("Order Added:" + prod.Product + ".placed by:" + prod.DestinyBuild);

            Orders.Add(prod);
            _recycledOrders.Remove(prod);
        }
    }

    /// <summary>
    /// This is the order added onces user is ordering to Destroy an non empty FoodSrc
    /// and is use too if a Factory has stopped producing bz workers have ther FoodSrc full and
    /// Factory inventor is full too
    /// </summary>
    /// <param name="evacOrder"></param>
    public void AddEvacuationOrder(Order evacOrder)
    {
        if (!ListContains(Orders, evacOrder) && !ListContains(_dormantOrders, evacOrder))
        {
            Orders.Insert(0, evacOrder);          
        }
    }

    /// <summary>
    /// Will return true if the 'prod' param was found in the 'list' as having the same 'Product and 
    /// (DestinyBuild or SourceBuild)'
    /// </summary>
    bool ListContains(List<Order> list, Order prod)
    {
        for (int i = 0; i < list.Count; i++)
        {
            //regular orders
            if (list[i].Product == prod.Product && list[i].DestinyBuild == prod.DestinyBuild 
                && list[i].TypeOrder == H.None

                //evacutaion orders
                || (list[i].Product == prod.Product && list[i].SourceBuild == prod.SourceBuild 
                && list[i].TypeOrder == H.Evacuation))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Will return the index of the 'prod' on the 'list' if found. Other wise -1
    /// </summary>
    /// <param name="list"></param>
    /// <param name="prod"></param>
    /// <returns></returns>
    int ReturnItemIndex(List<Order> list, Order prod)
    {
        for (int i = 0; i < list.Count; i++)
        {
            //regular orders
            if (list[i].Product == prod.Product && list[i].DestinyBuild == prod.DestinyBuild
                && list[i].TypeOrder == H.None

                //evacutaion orders
                || (list[i].Product == prod.Product && list[i].SourceBuild == prod.SourceBuild
                && list[i].TypeOrder == H.Evacuation))
            {
                return i;
            }
        }
        return -1;
    }

    /// <summary>
    /// Call when a Order is being sent to WheelBarrower
    /// </summary>
    /// <param name="prod"></param>
    void RemoveFromOrdersAddToRecycled(Order prod)
    {
        RemoveOrderFromTheList(Orders, prod);

        if (!ListContains(_recycledOrders, prod))
        {
            _recycledOrders.Add(prod);    
        }
    }

    /// <summary>
    /// This is for when using the Recycled orders, once an order is given 
    /// needs to be sent to the last place on the list 
    /// </summary>
    void RemoveFromPosToLast(Order prod)
    {
        var indx = _recycledOrders.IndexOf(prod);
        Order temp = _recycledOrders[indx];
        _recycledOrders.RemoveAt(indx);
        _recycledOrders.Add(temp);
    }

    public Order GiveMeOrder(Person person)
    {
        var currOrders = ReturnCurrentListAndDefinePrimary();

        for (int i = 0; i < currOrders.Count; i++)
        {
            if (currOrders[i].TypeOrder == H.None)
            {
                return RegularOrder(person, currOrders[i]);
            }
            else if (currOrders[i].TypeOrder == H.Evacuation)
            {
                return EvacuationOrder(person, currOrders[i]);
            }
        }
        return null;      
    }

    private Order EvacuationOrder(Person person, Order order)
    {
        var destinFoodSrc = FindClosestFoodSrcNotFull(person, order.Product);

        if (destinFoodSrc != "")
        {
            Order temp = new Order();
            temp = Order.Copy(order);

            temp.Amount = DispatchAmount();
            temp.DestinyBuild = destinFoodSrc;
            var sourceBuild = Brain.GetBuildingFromKey(temp.SourceBuild);

            CheckIfWasWatingToBeEvacuated(temp, sourceBuild);
            return temp;
        }
        else//could nt find any Food Source
        {
            if (!ListContains(_dormantOrders, order))
            {
                _dormantOrders.Add(order);
            }
            RemoveOrderFromTheList(Orders, order);
        }
        return null;
    }

    /// <summary>
    /// Returns a regular order 
    /// </summary>
    /// <param name="person"></param>
    /// <param name="order"></param>
    /// <returns></returns>
    Order RegularOrder(Person person, Order order)
    {
        var foodSrc = FindFoodSrcWithProd(person, order.Product);

        if (foodSrc != "")
        {
            Order temp = new Order();
            temp = Order.Copy( order);
            OrderFound(order);

            temp.Amount = DispatchAmount();
            temp.SourceBuild = foodSrc;
            return temp;
        }
        //if not found a FoodSrc with the Product need to seend it to the back of the queue
        //or send it to Recycled Orders
        OrderFound(order);
        
        return null;        
    }




    int DispatchAmount()
    {
        return 5000;
    }

    /// <summary>
    /// Wil do the actions needed once an order is found 
    /// </summary>
    void OrderFound(Order prod)
    {
        if (_isUsingPrimary)
        {
            RemoveFromOrdersAddToRecycled(prod);
        }
        else
        {
            RemoveFromPosToLast(prod);
        }
    }

    /// <summary>
    /// Here will loop thru all Food Srcs looking for the 'prod'
    /// 
    /// Will return a string value if one found with the prod other wise returs ""
    /// </summary>
    string FindFoodSrcWithProd(Person person , P prod)
    {
        var foodSrcs = ScoreAllFoodSources(person);

        for (int j = 0; j < foodSrcs.Count; j++)
        {
            var build = Brain.GetStructureFromKey(foodSrcs[j].Key);

            if (build.HaveThisProdOnInv(prod))
            {
                return build.MyId;
            }
        }
        return "";
    }

    string FindClosestFoodSrcNotFull(Person person, P prod)
    {
        var foodSrcs = ScoreAllFoodSources(person);

        for (int j = 0; j < foodSrcs.Count; j++)
        {
            var build = Brain.GetStructureFromKey(foodSrcs[j].Key);

            if (!build.Inventory.IsFull())
            {
                return build.MyId;
            }
        }
        return "";
    }

    List<BuildRank> ScoreAllFoodSources(Person person)
    {
        List<BuildRank> rank = new List<BuildRank>();
        for (int i = 0; i < BuildingPot.Control.FoodSources.Count; i++)
        {
            var key = BuildingPot.Control.FoodSources[i];
            var foodSrc = Brain.GetStructureFromKey(key);

            var score = ScoreABuild(person, foodSrc);//tht in this case is just Distance
            if (score < Profession.Radius && foodSrc.Instruction == H.None)
            {
                rank.Add(new BuildRank(key, score));     
            }
        }
        return rank.OrderByDescending(a => a.Score).ToList();
    }

    float ScoreABuild(Person person, Building toScore)
    {
        if (person.Home == null)
        {
            return Vector3.Distance(person.transform.position, toScore.transform.position);    
        }

        return Vector3.Distance(person.Home.transform.position, toScore.transform.position);
    }

#region Evacuation Order

    /// <summary>
    /// Will pass all orders in dormant to Orders
    /// This is call everytime a new FoodSrc is fully built
    /// </summary>
    public void ActiveDormantList()
    {
        for (int i = 0; i < _dormantOrders.Count; i++)
        {
            Orders.Insert(0, _dormantOrders[i]);
        }

        _dormantOrders.Clear();
    }

    /// <summary>
    /// Will remove the Evacuation order from Orders or _dormantOrders
    /// </summary>
    /// <param name="order"></param>
    public void RemoveEvacuationOrder(Order order)
    {
        if (ListContains(Orders, order))
        {
            RemoveOrderFromTheList(Orders, order);
        }
        else if (ListContains(_dormantOrders, order))
        {
            RemoveOrderFromTheList(_dormantOrders, order);
        }
    }

    /// <summary>
    /// Remove the 'order' from the 'list'
    /// </summary>
    /// <param name="list"></param>
    /// <param name="order"></param>
    void RemoveOrderFromTheList(List<Order> list, Order order)
    {
        var index = ReturnItemIndex(list, order);

        if (index == -1)
        {
            return;
        }
        
        list.RemoveAt(index);

        Debug.Log("Removed from list:"+order.Product+".type:"+order.TypeOrder);
    }

    /// <summary>
    /// Will take care of destroying the building if was waiting for the inventory
    /// for be evacuated 
    /// </summary>
    internal void CheckIfWasWatingToBeEvacuated(Order order, Building building)
    {
        //to see if can cover thjis ingredient
        InputElement ing = new InputElement(order.Product, order.Amount);

        //if doesnt have enought means we will deplete this Item in the inventory with this .
        //So I can go ahead and remove tht order from evacuation order so its not used anymore 
        if (building != null && building.Inventory != null &&
            !building.Inventory.IsHasEnoughToCoverThisIngredient(ing))
        {
            RemoveEvacuationOrder(new Order(order.Product, "", building.MyId));

            Debug.Log("Removed evac order:" + order.Product);
        }
    }

#endregion

    public bool DoIHaveAnyOrderOnDispatch(Building build)
    {
        var allList = Orders;
        allList.AddRange(_recycledOrders);
        allList.AddRange(_dormantOrders);

        for (int i = 0; i < allList.Count; i++)
        {
            if (allList[i].SourceBuild == build.MyId || allList[i].DestinyBuild == build.MyId)
            {
                return true;
            }
        }
        return false;
    }


    /// <summary>
    /// Will remove all the regular orders that contanint param 'myId' as the DestinBuild
    /// </summary>
    /// <param name="myId"></param>
    internal void RemoveRegularOrders(string myId)
    {
        RemoveRegularOrdersOnList(Orders, myId);
        RemoveRegularOrdersOnList(_recycledOrders, myId);
    }

    void RemoveRegularOrdersOnList(List<Order> list, string destinBuild)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].DestinyBuild == destinBuild)
            {
                list.RemoveAt(i);
                i--;
            }
        }
    }

    internal bool HasImportOrders()
    {
        for (int i = 0; i < ExpImpOrders.Count; i++)
        {
            if (ExpImpOrders[i].SourceBuild == "Ship")
            {
                return true;
            }
        }

        return false;
    }


    internal bool HasExportOrders()
    {
        var list = ReturnCurrentList();

        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].DestinyBuild == "Ship")
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// The action of importing an order to Inventory. This is called by ship 
    /// </summary>
    /// <param name="dock"></param>
    internal void Import(Building dock)
    {
        for (int i = 0; i < ExpImpOrders.Count; i++)
        {
            if (ExpImpOrders[i].SourceBuild == "Ship")
            {
                var ord = ExpImpOrders[i];

                //100 units is at least wht is needed go make an imnport
                if (dock.Inventory.HasEnoughtCapacityToStoreThis(100))
                {
                    HandleThatImport(dock, ord);
                }
            }
        }
    }

    /// <summary>
    /// The import handling. Modularity
    /// </summary>
    /// <param name="dock"></param>
    /// <param name="ord"></param>
    void HandleThatImport(Building dock, Order ord)
    {
        var maxAmtCanTake = dock.Inventory.MaxAmtCanTakeOfAProd(ord.Product);
        AddEvacuationOrder(ord);

        if (maxAmtCanTake > ord.Amount)
        {
            Debug.Log("Imported:" + ord.Product + " . " + ord.Amount+" Done" );
            Program.gameScene.ExportImport1.Buy(ord.Product, ord.Amount);

            dock.Inventory.Add(ord.Product, ord.Amount);
            ExpImpOrders.Remove(ord);
        }
        else
        {
            Debug.Log("Imported:" + ord.Product + " . " + maxAmtCanTake);
            Program.gameScene.ExportImport1.Buy(ord.Product, maxAmtCanTake);

            dock.Inventory.Add(ord.Product, maxAmtCanTake);
            ord.Amount -= maxAmtCanTake;
        }
    }

    /// <summary>
    /// Will remove the Import order from Dispatch. Import orders are evacuation orders
    /// 
    /// Export orders are removed at HandleThatExport()
    /// </summary>
    public void RemoveImportOrder(Building dock, Order ord)
    {
        if (ord.SourceBuild != "Ship")
        {
            return;
        }

        if(!dock.Inventory.IsItemOnInv(ord.Product) && !ExpImpOrders.Contains(ord))
        {
            Debug.Log("Docker removed the evac order:"+ord.Product);
            RemoveEvacuationOrder(ord);
            
        }
    }

    /// <summary>
    /// The action of export from dock inventory to Ship. Ship object is the one calling this
    /// </summary>
    /// <param name="dock"></param>
    internal void Export(Building dock)
    {
        for (int i = 0; i < ExpImpOrders.Count; i++)
        {
            if (ExpImpOrders[i].DestinyBuild == "Ship")
            {
                if (dock.Inventory.IsItemOnInv(ExpImpOrders[i].Product))
                {
                    HandleThatExport(dock, i);
                }
            }
        }
    }

    /// <summary>
    /// The export. For modulatiry
    /// </summary>
    /// <param name="dock"></param>
    /// <param name="i"></param>
    void HandleThatExport(Building dock, int i)
    {
        //var list = ReturnCurrentList();

        //if (i >= list.Count)
        //{
        //    throw new Exception("If happen after dock is delete then u need to delete the dispatch related to the dock");
        //    return;
        //}

        var ord = ExpImpOrders[i];

        int initialAmtNeed = ord.Amount;
        ord = dock.Inventory.ManageExportOrder(ord);
        int leftOnOrder = ord.Amount;
        int amtExpThisTime = (initialAmtNeed - leftOnOrder);

        Program.gameScene.ExportImport1.Sale(ord.Product, amtExpThisTime);

        if (ord.Amount == 0)
        {
            Debug.Log("Exported of:" + ord.Product + " done");

            //Removig from all. Could be in orders or in   RecycledOrders and always in   ExpImpOrders
            Orders.Remove(ord);
            RecycledOrders.Remove(ord);
            ExpImpOrders.Remove(ord);
 
            return;
        }

        Debug.Log("Exported:" + ord.Product + " . " + amtExpThisTime + ".Still left:" + leftOnOrder);
    }

    internal void AddToExpImpOrders(Order order)
    {
        ExpImpOrders.Add(order);
    }


    bool ThereAreEnoughShipsOnBay()
    {
        return true;
    }

    /// <summary>
    /// Only on _orders list 
    ///      Created to pull the import orders on Building Window
    /// </summary>
    /// <returns></returns>
    public List<Order> ReturnRegularOrders()
    {
        List<Order> re = new List<Order>();
        for (int i = 0; i < _orders.Count; i++)
        {
            if (_orders[i].TypeOrder == H.None)
            {
                re.Add(_orders[i]);
            }
        }
        return re;
    }

    /// <summary>
    /// Only on _orders list 
    /// 
    /// Created to pull the import orders on Building Window
    /// </summary>
    /// <returns></returns>
    public List<Order> ReturnEvacuaOrders()
    {
        List<Order> re = new List<Order>();
        for (int i = 0; i < _orders.Count; i++)
        {
            if (_orders[i].TypeOrder == H.Evacuation)
            {
                re.Add(_orders[i]);
            }
        }
        return re;
    }

    /// <summary>
    /// Bz as amount change in the order and .
    /// 
    /// use to remove the export and import orders
    /// </summary>
    /// <param name="id"></param>
    internal void RemoveOrderByIDExIm(string id)
    {
        for (int i = 0; i < _orders.Count; i++)
        {
            if (_orders[i].ID == id)
            {
                _orders.RemoveAt(i);
                CheckIfExportAndStillOnDockStorage();

                break;
            }
        }

        for (int i = 0; i < _expImpOrders.Count; i++)
        {
            if (_expImpOrders[i].ID == id)
            {
                _expImpOrders.RemoveAt(i);
                return;
            }
        }
    }

    /// <summary>
    /// in this case will be added as a evacuation order on the Dock with Price zero
    /// bz then will appear as an Import 
    /// 
    /// this is when an Export Order was candelled by user and still some goods on Dock
    /// </summary>
    void CheckIfExportAndStillOnDockStorage()
    {
        
    }

    /// <summary>
    /// Regular is Export
    /// </summary>
    /// <returns></returns>
    public List<Order> ReturnRegularOrdersOnProcess()
    {
        return _expImpOrders.Where(a=>a.TypeOrder==H.None).ToList();
    }

    /// <summary>
    /// Evacuation is Import
    /// </summary>
    /// <returns></returns>
    public List<Order> ReturnEvacOrdersOnProcess()
    {
        return _expImpOrders.Where(a => a.TypeOrder == H.Evacuation).ToList();
    }
}

public class Order
{
    public H TypeOrder = H.None;//the type of order is 
    public P Product;
    public string DestinyBuild;
    public string SourceBuild;
    public int Amount;//the amount dispatched in an order

    public string ID;//the id of an order. Used to find and removed

    public static Order Copy(Order aOrder)
    {
        Order res = new Order();

        H type = aOrder.TypeOrder;
        P prod = aOrder.Product;
        string dest = aOrder.DestinyBuild;
        string src = aOrder.SourceBuild;
        int amt = aOrder.Amount;

        res.TypeOrder = type;
        res.Product = prod;
        res.DestinyBuild = dest;
        res.SourceBuild = src;
        res.Amount = amt;

        return res;
    }

    public Order() { }

    public Order(P prod, string destiny, int amt )
    {
        Product = prod;
        DestinyBuild = destiny;
        Amount = amt;

        ID = Product + ":" + amt + "|" + TypeOrder + "|" + DateTime.Now;
    }

    /// <summary>
    /// Use for evacuation Orders
    /// </summary>
    /// <param name="prod"></param>
    /// <param name="destiny"></param>
    /// <param name="source">is the source, but could be 'Ship' which is an import</param>
    public Order(P prod, string destiny, string source)
    {
        Product = prod;
        SourceBuild = source;
        TypeOrder = H.Evacuation;

        ID = Product + ":" + Amount + "|" + TypeOrder + "|" + DateTime.Now;
    }

 
}
