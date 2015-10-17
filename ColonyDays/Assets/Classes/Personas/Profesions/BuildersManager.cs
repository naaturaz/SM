using UnityEngine;
using System.Collections.Generic;

/*This class manage wich buildings need to be built next and which has already
 * the resources assigned and ready to built
 * 
 */

public class BuildersManager
{
    List<Construction> _constructions = new List<Construction>();//constructions on list waiting to be greenlit
    List<Construction> _greenLight = new List<Construction>();//constrtucyions that have receive the resources already 

    public List<Construction> Constructions
    {
        get { return _constructions; }
        set { _constructions = value; }
    }

    public List<Construction> GreenLight
    {
        get { return _greenLight; }
        set { _greenLight = value; }
    }

    public BuildersManager() { }

    public string GiveMeBestConstruction()
    {
        if (_greenLight.Count == 0)
        {
            return "None";
        }

        return _greenLight[0].Key;
    }

    public void AddNewConstruction(string key, H hTypeP, int priority, Vector3 pos)
    {
        if (hTypeP == H.Shack)
        {return;}

        //Brain.GetStructureFromKey(key) == null is a way
        if (Brain.GetStructureFromKey(key) == null ||   Brain.GetStructureFromKey(key).StartingStage == H.Done)
        {
            return;
        }

        Construction t = new Construction();
        t.Key = key;
        t.HType = hTypeP;
        t.Priority = priority;
        t.Position = pos;
        _constructions.Add(t);
        ReorderItemOnList(_constructions.Count - 1, _constructions);
    }

    public void RemoveConstruction(string key)
    {
        for (int i = 0; i < _greenLight.Count; i++)
        {
            if (_greenLight[i].Key == key)
            {
                _greenLight.RemoveAt(i);
                return;
            }

        }

        for (int i = 0; i < _constructions.Count; i++)
        {
            if (_constructions[i].Key == key)
            {
                _constructions.RemoveAt(i); //in case was built by user or game before was passed to greenLight
                return;
            }
        }
    }

    /// <summary>
    /// Will change the priority on the list that contains the key
    /// </summary>
    /// <param name="key">The building 'MyId' Prop</param>
    /// <param name="priority"></param>
    public void ChangePriority(string key, int priority)
    {
        if (ContainKey(_constructions, key))
        {
            ChangePriorityList(key, priority, _constructions);
        }
        else if (ContainKey(_greenLight, key))
        {
            ChangePriorityList(key, priority, _greenLight);
        }
    }

    /// <summary>
    /// Trrue if'Key' is contained on the list 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="priority"></param>
    /// <param name="list"></param>
    /// <returns></returns>
    List<Construction> ChangePriorityList(string key, int priority, List<Construction> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].Key == key)
            {
                list[i].Priority = priority;
                ReorderItemOnList(i, list);
                break;
            }
        }
        return list;
    }

    bool ContainKey(List<Construction> list, string key)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].Key == key)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Will put the item passed as index in the right position 
    /// </summary>
    /// <param name="index"></param>
    List<Construction> ReorderItemOnList(int index, List<Construction> list)
    {
        while (index > 0)
        {
            //if the item in the list below me the prioritu is smaller than mine then I can
            //ocupy its spot , so we swap positions on the list 
            if (list[index - 1].Priority < list[index].Priority)
            {
                SwapItems(index - 1, index, list);
            }
            else
            {
                break;
            }
            //make index one smaller so in next iteration im still talking to the same item
            index--;
        }
        return list;
    }

    /// <summary>
    /// Swaps items in the list 
    /// </summary>
    /// <param name="indexA"></param>
    /// <param name="indexB"></param>
    List<Construction> SwapItems(int indexA, int indexB, List<Construction>list)
    {
        var t = list[indexA];
        list[indexA] = list[indexB];
        list[indexB] = t;

        return list;
    }

    /// <summary>
    /// Will commu8nicate with GameCOntrooler to see if have enought material to authorizr a building contruction
    /// 
    /// 
    /// </summary>
    /// <returns></returns>
    bool CanGreenLight(H hTypeP)
    {
        var stat = Book.GiveMeStat(hTypeP);

        bool wood = GameController.Inventory1.ReturnAmtOfItemOnInv(P.Wood) >= stat.Wood;
        bool stone = GameController.Inventory1.ReturnAmtOfItemOnInv(P.Stone) >= stat.Stone;
        bool brick = GameController.Inventory1.ReturnAmtOfItemOnInv(P.Brick) >= stat.Brick;
        bool iron = GameController.Inventory1.ReturnAmtOfItemOnInv(P.Iron) >= stat.Iron;
        bool gold = GameController.Inventory1.ReturnAmtOfItemOnInv(P.Gold) >= stat.Gold;
        bool dollar = GameController.Dollars >= stat.Dollar;

        return wood && stone && brick && iron && gold && dollar;
    }

    public void Update()
    {
        CheckIfAnyToGreenLight();
    }

    /// <summary>
    /// Will check if at least there is one that need greenlight 
    /// </summary>
    void CheckIfAnyToGreenLight()
    {
        if (_constructions.Count == 0)
        { return; }

        var isGreen = CanGreenLight(_constructions[0].HType);

        if (isGreen)
        {
            RemoveFromGameController(_constructions[0].HType);
            HandleList(_constructions[0]);
        }
    }

    /// <summary>
    /// Will remove 
    /// </summary>
    /// <param name="construction"></param>
    void HandleList(Construction construction)
    {
        _constructions.Remove(construction);
       
        Structure st = Brain.GetStructureFromKey(construction.Key);
        if (st.StartingStage != H.Done)
        {

            _greenLight.Add(construction);
        }
        
       
    }

    void RemoveFromGameController(H hTypeP)
    {
        var stat = Book.GiveMeStat(hTypeP);

        GameController.Inventory1.Remove(P.Wood, stat.Wood);
        GameController.Inventory1.Remove(P.Stone, stat.Stone);
        GameController.Inventory1.Remove(P.Brick, stat.Brick);
        GameController.Inventory1.Remove(P.Iron, stat.Iron);
        GameController.Inventory1.Remove(P.Gold, stat.Gold);
        GameController.Inventory1.Remove(P.Dollar, stat.Dollar);
    }

    public bool IsAtLeastOneBuildUp()
    {
        return _greenLight.Count > 0;
    }
}

public class Construction
{
    public string Key;
    public H HType;
    public int Priority;
    public Vector3 Position;
}