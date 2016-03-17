using System.Collections.Generic;

/// <summary>
/// Will address stuff and question thru all Dispatches that are in the game 
/// 
/// WheelBarrow Offices and Dock are the only tht have dispatch so far 
/// </summary>

public class DispatchManager {

    /// <summary>
    /// Will make all dormant Orders active in all Disptaches 
    /// </summary>
    internal void ActiveDormantList()
    {
        var all = FindAllWheelBarrAndDockBuilds();

        for (int i = 0; i < all.Count; i++)
        {
            all[i].Dispatch1.ActiveDormantList();
        }

    }

    /// <summary>
    /// Will find out if has an order in any dispatch 
    /// </summary>
    /// <param name="building"></param>
    /// <returns></returns>
    internal bool DoIHaveAnyOrderOnAnyDispatch(Building building)
    {
        var all = FindAllWheelBarrAndDockBuilds();

        for (int i = 0; i < all.Count; i++)
        {
            if (all[i].Dispatch1.DoIHaveAnyOrderOnDispatch(building))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Will remove all regular orders find in every single dispatch with this 'myId'
    /// </summary>
    /// <param name="myId"></param>
    internal void RemoveRegularOrders(string myId)
    {
        var all = FindAllWheelBarrAndDockBuilds();

        for (int i = 0; i < all.Count; i++)
        {
            all[i].Dispatch1.RemoveRegularOrders(myId);
        }
    }

    /// <summary>
    /// Will remove all evac orders find in every single dispatch with this 'myId'
    /// </summary>
    /// <param name="myId"></param>
    internal void RemoveEvacOrders(string myId)
    {
        var all = FindAllWheelBarrAndDockBuilds();

        for (int i = 0; i < all.Count; i++)
        {
            all[i].Dispatch1.RemoveEvacuationOrder(myId);
        }
    }

    List<Structure> FindAllWheelBarrAndDockBuilds()
    {
        List<Structure> all = BuildingController.FindAllStructOfThisType(H.Masonry);
        all.AddRange(BuildingController.FindAllStructOfThisType(H.Dock));
        return all;
    }

  
}
