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
    private int maxAmtOfExpImpOrders = 10;


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

    /// <summary>
    /// Exp and Imports orderss
    /// </summary>
    public List<Order> ExpImpOrders
    {
        get { return _expImpOrders; }
        set { _expImpOrders = value; }
    }

    /// <summary>
    /// Only exports needed so ships know what to get 
    /// </summary>
    public List<Order> ExportsOrders { get; private set; }

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
        _isUsingPrimary = DefineUsingPrimary(ReturnCurrentList());


        return ReturnCurrentList();

    }

    /// <summary>
    /// This is the public method that building problably a factory of some kind 
    /// will call when have zero of a ingredient in their inventory
    /// so an order will be on place, so WheelBarrowers can find where to pull the ingredient from if is 
    /// in  any Storage 
    /// </summary>
    /// <param name="prod">The product/ingredient  is needed</param>
    public void AddToOrdersToDock(Order prod)
    {
        if (!ListContainsCheckID(Orders, prod))
        {
           //Debug.Log("Order Added:" + prod.Product + ".placed by:" + prod.DestinyBuild);

            Orders.Add(prod);
            //OrderByPlacedTime(Orders);


            _recycledOrders.Remove(prod);
        }
    }

    /// <summary>
    /// Bz they dont need to check ID if building place a order of a material should wait until is completed 
    /// </summary>
    /// <param name="prod"></param>
    public void AddToOrdersToWheelBarrow(Order prod)
    {
        if (!ListContains(Orders, prod))
        {
            Debug.Log("Order Added:" + prod.Product + ".placed by:" + prod.DestinyBuild);
            Orders.Add(prod);
            _recycledOrders.Remove(prod);
            //OrderByPlacedTime(Orders);
        }
    }


#region Heavy Load
    /// <summary>
    /// Bz they dont need to check ID if building place a order of a material should wait until is completed 
    /// </summary>
    /// <param name="prod"></param>
    public void AddToOrdersToHeavyLoad(Order prod)
    {
        if (!ListContains(Orders, prod))
        {
            Debug.Log("Order Added on Heavy:" + prod.Product + ".placed by:" + prod.DestinyBuild);
            Orders.Add(prod);
            _recycledOrders.Remove(prod);
            //OrderByPlacedTime(Orders);
        }
    }


    /// <summary>
    /// Evac order in a wheelbacrrow will overwrite if they are from the same place and same prod 
    /// </summary>
    /// <param name="evacOrder"></param>
    public void AddEvacuationOrderToHeavyLoad(Order evacOrder)
    {
        if (!ListContains(Orders, evacOrder) && !ListContains(_dormantOrders, evacOrder))
        {
            Orders.Add(evacOrder);
            //OrderByPlacedTime(Orders);
            Debug.Log("evac order added:" + evacOrder.Product + " orig:" + evacOrder.SourceBuild);
        }
    }
#endregion




    //ordering them by time placed so first placed is given always first 
    void OrderByPlacedTime(List<Order> list)
    {
        list = list.OrderBy(a => a.PlacedTime).ToList();
    }

    /// <summary>
    /// This is the order added onces user is ordering to Destroy an non empty FoodSrc
    /// and is use too if a Factory has stopped producing bz workers have ther FoodSrc full and
    /// Factory inventor is full too
    /// 
    /// Evac orders in dock are all unique
    /// </summary>
    /// <param name="evacOrder"></param>
    public void AddEvacuationOrderInDock(Order evacOrder)
    {
        if (!ListContainsCheckID(Orders, evacOrder) && !ListContainsCheckID(_dormantOrders, evacOrder))
        {
            Orders.Add(evacOrder);


            //OrderByPlacedTime(Orders);

        }
    }
    /// <summary>
    /// Evac order in a wheelbacrrow will overwrite if they are from the same place and same prod 
    /// </summary>
    /// <param name="evacOrder"></param>
    public void AddEvacuationOrderToWheelBarrow(Order evacOrder)
    {
        if (!ListContainsCheckID(Orders, evacOrder) && !ListContainsCheckID(_dormantOrders, evacOrder))
        {
            Orders.Add(evacOrder);
            //OrderByPlacedTime(Orders);
            //Debug.Log("evac order added:"+evacOrder.Product+" orig:"+evacOrder.SourceBuild);
        }
    }

    /// <summary>
    /// Will return true if the 'prod' param was found in the 'list' as having the same 'Product and 
    /// (DestinyBuild or SourceBuild) and ID'
    /// </summary>
    bool ListContainsCheckID(List<Order> list, Order prod)
    {
        for (int i = 0; i < list.Count; i++)
        {
            //regular orders
            if (list[i].Product == prod.Product && list[i].DestinyBuild == prod.DestinyBuild 
                && list[i].TypeOrder == H.None && list[i].ID == prod.ID

                //evacutaion orders
                || (list[i].Product == prod.Product && list[i].SourceBuild == prod.SourceBuild 
                && list[i].TypeOrder == H.Evacuation && list[i].ID == prod.ID))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// does not check id.
    /// </summary>
    /// <param name="list"></param>
    /// <param name="prod"></param>
    /// <returns></returns>
    bool ListContains(List<Order> list, Order prod)
    {
        for (int i = 0; i < list.Count; i++)
        {
            //regular orders
            if (list[i].Product == prod.Product && list[i].DestinyBuild == prod.DestinyBuild

                //evacutaion orders
                || (list[i].Product == prod.Product && list[i].SourceBuild == prod.SourceBuild
                ))
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
            if (_recycledOrders.Count == 0)
            {
                _recycledOrders.Add(prod);
                return;
            }

            _recycledOrders.Insert(0,prod);    
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
        if (!ListContains(_recycledOrders, prod))
        {
            _recycledOrders.Add(prod);
        }
    }

    /// <summary>
    /// Main method where Docker and WheelBarrow as for an order
    /// </summary>
    /// <param name="person"></param>
    /// <returns></returns>
    public Order GiveMeOrder(Person person)
    {
        var currOrders = ReturnCurrentListAndDefinePrimary();

        for (int i = 0; i < currOrders.Count; i++)
        {
            //if the Inventory of destiny build is full will skip that order 
            if (IsDestinyBuildInvFullForThisProd(currOrders[i]) || IsDestinyWithOverSoManyKGOfThisProd(500, currOrders[i])
                || currOrders[i].IsCompleted)
            {
                //todo Notify
                Debug.Log("Inv full to DestBuild:"+currOrders[i].DestinyBuild+"|for prod:"+currOrders[i].Product+"" 
                    +"|order removed. Or  had >500KG on Destiny of the prod. or was completed");
                
                bool wasRemoved = RemoveOrderByIDExIm(currOrders[i].ID);

                //othwerwise is infinite loop
                if (wasRemoved)
                {
                    i--;
                }
                continue;
            }

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

    public Order GiveMeOrderDocker(Person person)
    {
        var currOrders = ReturnCurrentListAndDefinePrimary();

        for (int i = 0; i < currOrders.Count; i++)
        {
            //if the Inventory of destiny build is full will skip that order 
            if (IsDestinyBuildInvFullForThisProd(currOrders[i]) || currOrders[i].IsCompleted)
            {
                //todo Notify
                Debug.Log("Docker order removed:"+currOrders[i].DestinyBuild+"|for prod:"+currOrders[i].Product+"" +"");

                RemoveOrderByIDExIm(currOrders[i].ID);
                i--;
                continue;
            }

            if (currOrders[i].TypeOrder == H.None)
            {
                return RegularOrderDocker(person, currOrders[i]);
            }
            else if (currOrders[i].TypeOrder == H.Evacuation)
            {
                return EvacuationOrderDocker(person, currOrders[i]);
            }
        }
        return null;
    }

    /// <summary>
    /// Will tell u if on order destiny has more that 'amtMax' of the order prod.
    /// 
    /// In the context that we have 1000KG of iron on carpintery we dont need more 
    /// </summary>
    /// <param name="amtMax"></param>
    /// <param name="order"></param>
    /// <returns></returns>
    private bool IsDestinyWithOverSoManyKGOfThisProd(int amtMax, Order order)
    {
        var destBuild = Brain.GetBuildingFromKey(order.DestinyBuild);

        if (destBuild == null)
        {
            return false;
        }

        if (destBuild.Inventory.ReturnAmtOfItemOnInv(order.Product) > amtMax)
        {
            return true;
        }
        return false;
    }


    bool IsDestinyBuildInvFullForThisProd(Order order)
    {
        var destBuild = Brain.GetBuildingFromKey(order.DestinyBuild);

        if (destBuild == null)
        {
            return false;
        }

        if (destBuild.Inventory.IsFullForThisProd(order.Product))
        {
            return true;
        }
        return false;
    }  
    
    bool IsDestinyBuildInvFull(Order order)
    {
        var destBuild = Brain.GetBuildingFromKey(order.DestinyBuild);

        if (destBuild == null)
        {
            return false;
        }

        if (destBuild.Inventory.IsFull())
        {
            return true;
        }
        return false;
    }


    private Order EvacuationOrderDocker(Person person, Order order)
    {
        var destinFoodSrc = FindClosestFoodSrcNotFull(person, order.Product);

        if (destinFoodSrc != "")
        {
            Order temp = new Order();
            temp = Order.Copy(order);

            temp.Amount = order.ApproveThisAmt(person.HowMuchICanCarry());
            order.AddToFullFilled(temp.Amount);

            temp.DestinyBuild = destinFoodSrc;
            return temp;
        }
        else//could nt find any Food Source
        {
            if (!ListContainsCheckID(_dormantOrders, order))
            {
                _dormantOrders.Add(order);
            }
            RemoveOrderFromTheList(Orders, order);
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

            temp.Amount = person.HowMuchICanCarry();
            temp.DestinyBuild = destinFoodSrc;
            var sourceBuild = Brain.GetBuildingFromKey(temp.SourceBuild);

            CheckIfWasWatingToBeEvacuated(temp, sourceBuild);
            return temp;
        }
        else//could nt find any Food Source
        {
            if (!ListContainsCheckID(_dormantOrders, order))
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
            //OrderFound(order);

            temp.Amount = order.ApproveThisAmt(person.HowMuchICanCarry());
            order.AddToFullFilled(temp.Amount);

            temp.SourceBuild = foodSrc;
            return temp;
        }
        //if not found a FoodSrc with the Product need to seend it to the back of the queue
        //or send it to Recycled Orders
        OrderFound(order);
        
        return null;        
    }   
    
    public void AddToOrderAmtProcessed(Order ord, float amt)
    {
        var orderInd = _orders.FindIndex(a => a.ID == ord.ID);
        if (orderInd != -1)
        {
            _orders[orderInd].AddToFullFilled(amt);
        }
        else
        {
            var ind = _recycledOrders.FindIndex(a => a.ID == ord.ID);
            if (ind == -1)
            {
                Debug.Log("Docker order not found on report: " + ord.ID);
                return;
            }
            _recycledOrders[ind].AddToFullFilled(amt);
        }
    }

    Order RegularOrderDocker(Person person, Order order)
    {
        var foodSrc = FindFoodSrcWithProd(person, order.Product);

        if (foodSrc != "" //&& person.Work.Inventory.ReturnAmtOfItemOnInv(order.Product) < order.Amount
            )
        {
            Order temp = new Order();
            temp = Order.Copy( order);
            OrderFound(order);

            temp.Amount = order.ApproveThisAmt(person.HowMuchICanCarry());
            //order.AddToFullFilled(temp.Amount);
            temp.SourceBuild = foodSrc;
            return temp;
        }
        //bz dock inventory amount of this item must be less other wise wont be given
        //this is to avoid dockkers keep dumping an item into Dock Inventory until a ship arrives 
        else if (foodSrc != "" && person.Work.Inventory.ReturnAmtOfItemOnInv(order.Product) > order.Amount)
        {
            //this is once the invnetory in the port has more than the ammount. u need
            //to find a way to see how much is left of the order so that amt will become
            //the current order for this person... order object maybe needs a feel with completion amt, and completed bool

        }
        //if not found a FoodSrc with the Product need to seend it to the back of the queue
        //or send it to Recycled Orders
        OrderFound(order);
        
        return null;        
    }

    internal bool ThereIsWorkAtDock()
    {
        return _expImpOrders.Count > 0 || _orders.Count > 0;
    }

    ///// <summary>
    ///// Will return the Dispatch amount based on the type of person carrying 
    ///// 
    ///// Will carry so many KG of that prod bz the person can do it 
    ///// </summary>
    ///// <returns></returns>
    //public static float DispatchAmount(Person pers)
    //{
    //    var isWheel = pers.PrevJob == Job.WheelBarrow || pers.ProfessionProp.ProfDescription == Job.WheelBarrow;
    //    var isDocker = pers.PrevJob == Job.Docker || pers.ProfessionProp.ProfDescription == Job.Docker;

    //    var mul = 1;
    //    if (pers.ProfessionProp != null && (isWheel || isDocker))
    //    {
    //        mul = 3;
    //    }
    //    return pers.HowMuchICanCarry() * mul;
    //}

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
    public static string FindFoodSrcWithProd(Person person , P prod)
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

    static List<BuildRank> ScoreAllFoodSources(Person person)
    {
        List<BuildRank> rank = new List<BuildRank>();
        for (int i = 0; i < BuildingPot.Control.FoodSources.Count; i++)
        {
            var key = BuildingPot.Control.FoodSources[i];
            var foodSrc = Brain.GetStructureFromKey(key);

            var score = ScoreABuild(person, foodSrc);//tht in this case is just Distance
            if (score < Brain.Maxdistance && foodSrc.Instruction == H.None)
            {
                rank.Add(new BuildRank(key, score, score));     
            }
        }
        return rank.OrderBy(a => a.Score).ToList();
    }

    /// <summary>
    /// used to be from the closest from Home. Now is from Work
    /// 
    /// used by evacuation orders
    /// and regular orders. they need to find the closest FoodSrc to the Works of the Persons
    /// not their homes 
    /// </summary>
    /// <param name="person"></param>
    /// <param name="toScore"></param>
    /// <returns></returns>
    static float ScoreABuild(Person person, Building toScore)
    {
        if (person.Work == null)
        {
            return Vector3.Distance(person.transform.position, toScore.transform.position);    
        }
        return Vector3.Distance(person.Work.transform.position, toScore.transform.position);
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
    public void RemoveEvacuationOrder(string orderId)
    {
        var index = Orders.FindIndex(a => a.ID == orderId);

        if (index > -1)
        {
            Orders.RemoveAt(index);
        }

        index = DormantOrders.FindIndex(a => a.ID == orderId);

        if (index > -1)
        {
            DormantOrders.RemoveAt(index);
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

       //Debug.Log("Removed from list:"+order.Product+".type:"+order.TypeOrder);
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
            RemoveEvacuationOrder(order.ID);
           //Debug.Log("Removed evac order:" + order.Product+".date"+Program.gameScene.GameTime1.TodayYMD());

            //so people pass check in with Queues and this building finnaly gets removed 
            if (building.Inventory.IsEmpty())
            {
                PersonPot.Control.RestartController();
            }
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

    #region Dock Related

    internal bool HasImportOrders()
    {
        for (int i = 0; i < ExpImpOrders.Count; i++)
        {
            if (ExpImpOrders[i].SourceBuild == "Ship")
            {
                return true;
            }
        }

        for (int i = 0; i < Orders.Count; i++)
        {
            if (Orders[i].SourceBuild == "Ship")
            {
                return true;
            }
        }

        return false;
    }


    internal bool HasExportOrders()
    {
        return ExportsOrders != null && ExportsOrders.Count > 0;
    }

    /// <summary>
    /// The action of importing an order to Inventory. This is called by ship 
    /// </summary>
    /// <param name="dock"></param>
    internal bool Import(Building dock)
    {
        for (int i = 0; i < ExpImpOrders.Count; i++)
        {
            if (ExpImpOrders[i].SourceBuild == "Ship")
            {
                var ord = ExpImpOrders[i];

                //100 units is at least wht is needed go make an imnport
                if (dock.Inventory.HasEnoughtCapacityToStoreThis(ord.Product, ord.Amount))
                {
                    HandleThatImport(dock, ord);
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// The import handling. Modularity
    /// </summary>
    /// <param name="dock"></param>
    /// <param name="ord"></param>
    void HandleThatImport(Building dock, Order ord)
    {
        var maxAmtCanTake = dock.Inventory.MaxAmtOnKGCanTakeOfAProd(ord.Product);
        AddEvacuationOrderInDock(ord);

        //if can handle the import right away
        //will added to invent and will remove it from ExpImpOrders
        if (maxAmtCanTake > ord.Amount)
        {
           Debug.Log("Imported:" + ord.Product + " . " + ord.Amount+" Done" );
            Program.gameScene.ExportImport1.Buy(ord.Product, ord.Amount);

            dock.Inventory.Add(ord.Product, ord.Amount);
            ExpImpOrders.Remove(ord);
        }
        //if cant handle right away will import as much it can and will keep the order there
        //so its handled later
        else
        {
           Debug.Log("Imported:" + ord.Product + " . " + maxAmtCanTake);
            Program.gameScene.ExportImport1.Buy(ord.Product, maxAmtCanTake);

            dock.Inventory.Add(ord.Product, maxAmtCanTake);
            ord.ChangeAmountBy(- maxAmtCanTake);
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
            RemoveEvacuationOrder(ord.ID);
            
        }
    }

    /// <summary>
    /// The action of export from dock inventory to Ship. Ship object is the one calling this
    /// </summary>
    /// <param name="dock"></param>
    internal bool Export(Building dock)
    {
        for (int i = 0; i < ExportsOrders.Count; i++)
        {
            if (ExportsOrders[i].DestinyBuild == "Ship")
            {
                if (dock.Inventory.IsItemOnInv(ExportsOrders[i].Product))
                {
                    HandleThatExport(dock, ExportsOrders[i]);
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// The export. For modulatiry
    /// </summary>
    /// <param name="dock"></param>
    /// <param name="i"></param>
    void HandleThatExport(Building dock, Order ord)
    {
        float initialAmtNeed = ord.Amount;
        ord = dock.Inventory.ManageExportOrder(ord);
        float leftOnOrder = ord.Amount;
        float amtExpThisTime = (initialAmtNeed - leftOnOrder);

        Program.gameScene.ExportImport1.Sale(ord.Product, amtExpThisTime, dock.Name);
        if (ord.Amount == 0)
        {
            Debug.Log("Exported of:" + ord.Product + " done");
            //Removig from all. Could be in orders or in   RecycledOrders and always in   ExpImpOrders
            RemoveOrderFromAllListByID(ord.ID);
            return;
        }
        Debug.Log("Exported:" + ord.Product + " . " + amtExpThisTime + ".Still left:" + leftOnOrder);
    }

    public bool AddToExpImpOrders(Order order)
    {
        if (ExpImpOrders.Count < maxAmtOfExpImpOrders)
        {
            if (order.DestinyBuild == "Ship")
            {
                if (ExportsOrders == null)
                {
                    ExportsOrders = new List<Order>();
                }

                ExportsOrders.Add(order);
            }

            ExpImpOrders.Add(order);
            return true;
        }
        else
        {
            //todo notify
            Debug.Log("Will not handle over 10 Export Import orders at the same time . 10 is the max");
            Dialog.OKDialog(H.Info , "Ten orders is the limit on this building");
            return false;
        }
    }


    bool ThereAreEnoughShipsOnBay()
    {
        return true;
    }

#endregion

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
    /// 
    /// will return true if was removed 
    /// </summary>
    /// <param name="id"></param>
    internal bool RemoveOrderByIDExIm(string id)
    {
        var res = false;
        for (int i = 0; i < _orders.Count; i++)
        {
            if (_orders[i].ID == id)
            {
                _orders.RemoveAt(i);
                CheckIfExportAndStillOnDockStorage();
                res = true;
            }
        }
        for (int i = 0; i < _recycledOrders.Count; i++)
        {
            if (_recycledOrders[i].ID == id)
            {
                _recycledOrders.RemoveAt(i);
                CheckIfExportAndStillOnDockStorage();
                res = true;
            }
        }

        for (int i = 0; i < _expImpOrders.Count; i++)
        {
            if (_expImpOrders[i].ID == id)
            {
                _expImpOrders.RemoveAt(i);
                res = true;
            }
        }
        return res;
    }

    void RemoveOrderFromAllListByID(string id)
    {
        for (int i = 0; i < Orders.Count; i++)
        {
            if (Orders[i].ID == id)
            {
                Orders.RemoveAt(i);
                break;
            }
        }

        for (int i = 0; i < RecycledOrders.Count; i++)
        {
            if (RecycledOrders[i].ID == id)
            {
                RecycledOrders.RemoveAt(i);
                break;
            }
        }

        for (int i = 0; i < ExpImpOrders.Count; i++)
        {
            if (ExpImpOrders[i].ID == id)
            {
                ExpImpOrders.RemoveAt(i);
            }
        }

        for (int i = 0; i < ExportsOrders.Count; i++)
        {
            if (ExportsOrders[i].ID == id)
            {
                ExportsOrders.RemoveAt(i);
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
    public string SourceBuild;

    string _destinyBuild;

    //the amount dispatched in an order
    float _amount;

    float _fullFilled;
    bool _isCompleted;

    public float Amount
    {
        get { return _amount; }
        set { _amount = value; }
    }

    public float FullFilled
    {
        get
        {
            return _fullFilled;
        }

        set
        {
            _fullFilled = value;
        }
    }

    public bool IsCompleted
    {
        get
        {
            return _isCompleted;
        }

        set
        {
            _isCompleted = value;
        }
    }

    public string DestinyBuild
    {
        get
        {
            return _destinyBuild;
        }

        set
        {
            _destinyBuild = value;
        }
    }

    public DateTime PlacedTime;

    public string ID;//the id of an order. Used to find and removed
    private P p1;
    private int p2;

    public static Order Copy(Order aOrder)
    {
        Order res = new Order();

        H type = aOrder.TypeOrder;
        P prod = aOrder.Product;
        string dest = aOrder.DestinyBuild;
        string src = aOrder.SourceBuild;
        float amt = aOrder.Amount;
        string id = aOrder.ID;
        DateTime pTime = aOrder.PlacedTime;

        res.TypeOrder = type;
        res.Product = prod;
        res.DestinyBuild = dest;
        res.SourceBuild = src;
        res.Amount = amt;
        res.ID = id;
        res.PlacedTime = pTime;

        return res;
    }

    public Order() { }

    public Order(P prod, string destiny, int amt)
    {
        Product = prod;
        DestinyBuild = destiny;
        Amount = amt;

        ID = Product + ":" + amt + "|" + TypeOrder + "|" + DateTime.Now;
        PlacedTime = DateTime.Now;
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
        PlacedTime = DateTime.Now;
    }

    /// <summary>
    /// Used for Orders on Unlock Buildings. tht really they hold 2 values alone for Unlocking purposes
    /// </summary>
    /// <param name="prod"></param>
    /// <param name="amt"></param>
    public Order(P prod, int amt)
    {
        Product = prod;
        Amount = amt;
    }

    public void AddToFullFilled(float pls)
    {
        FullFilled += pls;

        if (FullFilled >= _amount)
        {
            IsCompleted=true;
        }
    }

    public float ApproveThisAmt(float pls)
    {
        if (IsCompleted)
        {
            return 0;
        }

        var left = _amount - FullFilled;

        if (left > pls)
        {
            return pls;
        }
        return left;
    }

    internal float Left()
    {
        return _amount - FullFilled;
    }

    public void ChangeAmountBy(float by)
    {
        _amount += by;
        if (_amount < 0)
        {
            _amount = 0;
        }
    }
}
