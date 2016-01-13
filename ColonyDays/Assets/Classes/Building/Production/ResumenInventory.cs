using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * This is the one that will Resume all Storage Inventories 
 * 
 */

public class ResumenInventory {

    /// <summary>
    /// Will retruen the amt of Category in all inventories.
    /// 
    /// Used for GUI
    /// </summary>
    /// <param name="pCat"></param>
    /// <returns></returns>
    public float ReturnAmountOnCategory(PCat pCat)
    {
        var storages = BuildingController.FindAllStructOfThisTypeContain(H.Storage);

        float res = 0;

        for (int i = 0; i < storages.Count; i++)
        {
            res += storages[i].Inventory.ReturnAmountOnCategory(pCat);
        }

        return res;
    }


    /// <summary>
    /// Will retruen the amt of item in all inventories.
    /// 
    /// Used for GUI
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public float ReturnAmtOfItemOnInv(P item)
    {
        var storages = BuildingController.FindAllStructOfThisTypeContain(H.Storage);

        float res = 0;

        for (int i = 0; i < storages.Count; i++)
        {
            res += storages[i].Inventory.ReturnAmtOfItemOnInv(item);
        }

        return res;
    }

    /// <summary>
    /// Will tell u if the item is on one of the inventorires 
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool IsItemOnInv(P item)
    {
        var storages = BuildingController.FindAllStructOfThisTypeContain(H.Storage);

        for (int i = 0; i < storages.Count; i++)
        {
            if (storages[i].Inventory.IsItemOnInv(item))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Will remove the item and the amount from the inventories.
    /// 
    /// This method is use for an building was built and 40 wood for ex needs to be removed 
    /// 
    /// Will loop thru the storages until remove the full amt
    /// </summary>
    /// <param name="item"></param>
    /// <param name="amt"></param>
    public void Remove(P item, int amt)
    {
        var storages = BuildingController.FindAllStructOfThisTypeContain(H.Storage);

        for (int i = 0; i < storages.Count; i++)
        {
            var left = LeftToRemove(item, amt, storages[i]);

            if (left == 0)
            {
                return;
            }
        }
    }


    /// <summary>
    /// Will remove from the 'building' and will tell u how much is left to be removed 
    /// </summary>
    float LeftToRemove(P item, int amt, Structure building)
    {
        if (building.Inventory.IsItemOnInv(item))
        {
            var removed = building.Inventory.RemoveByWeight(item, amt);
            var left = amt - removed;

            return left;
        }
        return amt;
    }

    /// <summary>
    /// Will tell u if all inventories are empty on the storages 
    /// </summary>
    /// <returns></returns>
    internal bool IsEmpty()
    {
        var storages = BuildingController.FindAllStructOfThisTypeContain(H.Storage);

        for (int i = 0; i < storages.Count; i++)
        {
            if (!storages[i].Inventory.IsEmpty())
            {
                return false;
            }
        }

        return true;
    }
}
