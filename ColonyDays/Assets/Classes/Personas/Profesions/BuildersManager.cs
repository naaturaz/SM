using UnityEngine;
using System.Collections.Generic;
using System.Net;

/*This class manage wich buildings need to be built next and which has already
 * the resources assigned and ready to built
 * 
 */

public class BuildersManager
{
    List<Construction> _constructions = new List<Construction>();//constructions on list waiting to be greenlit
    List<Construction> _greenLight = new List<Construction>();//constrtucyions that have receive the resources already 

    //the buildings that were on Queue and are not anymore so all person checked on them 
    List<string> _passedQueue = new List<string>();

    private Building _building;

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

    public List<string> PassedQueue
    {
        get { return _passedQueue; }
        set { _passedQueue = value; }
    }

    public BuildersManager() { }

    public BuildersManager(Building building)
    {
        _building = building;
    }

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
        //if (hTypeP == H.Shack)
        //{return;}

        //Brain.GetStructureFromKey(key) == null is a way and is not a brdige 
        if (Brain.GetStructureFromKey(key) == null && !key.Contains("Bridge"))
        {
            return;
        }
        if (Brain.GetStructureFromKey(key)!=null && Brain.GetStructureFromKey(key).StartingStage == H.Done)
        {
            return;
        }

//        Debug.Log("construction aded :"+key+"."+Program.gameScene.GameTime1.TodayYMD());
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
    bool CanGreenLight(Construction cons)
    {
        var stat = Book.GiveMeStat(cons.HType);
        
        bool wood = GameController.Inventory1.ReturnAmtOfItemOnInv(P.Wood) >= stat.Wood || stat.Wood == 0;//if is zero
        //the needed amt will pass reagardless the amt we have on storage
        bool stone = GameController.Inventory1.ReturnAmtOfItemOnInv(P.Stone) >= stat.Stone || stat.Stone == 0;
        bool brick = GameController.Inventory1.ReturnAmtOfItemOnInv(P.Brick) >= stat.Brick || stat.Brick == 0;
        bool iron = GameController.Inventory1.ReturnAmtOfItemOnInv(P.Iron) >= stat.Iron || stat.Iron == 0;
        bool gold = GameController.Inventory1.ReturnAmtOfItemOnInv(P.Gold) >= stat.Gold || stat.Gold == 0;
        bool dollar = Program.gameScene.GameController1.Dollars >= stat.Dollar || stat.Dollar == 0;
        bool passedQue = _passedQueue.Contains(cons.Key);

        //other wise would remove it from _passedQueue if was mising Brick for example and wont be 
        //build it ever again
        if (wood && stone && brick && iron && gold && dollar && passedQue)
        {
            _passedQueue.Remove(cons.Key);
            return true;
        }
        return false;
    }

    public void Update()
    {
        CheckIfAnyToGreenLight();
        RemoveFullyBuiltOrRemoved();
    }

    /// <summary>
    /// Bz if they are fuly built sometimes they stay in the _constructions
    /// 
    /// So if is fully built or being removed can be removed from _constructions 
    /// </summary>
    private void RemoveFullyBuiltOrRemoved()
    {
        if (_constructions.Count == 0 || _constructions[0].Key.Contains("Bridge"))//bz brdige will be null on below if 
        { return; }

        var st = Brain.GetStructureFromKey(Constructions[0].Key);

        if (st==null || st.StartingStage==H.Done)
        {
            PassedQueue.Remove(_constructions[0].Key);
            Constructions.RemoveAt(0);
        }
    }

    /// <summary>
    /// Will check if at least there is one that need greenlight 
    /// </summary>
    void CheckIfAnyToGreenLight()
    {
        if (_constructions.Count == 0)
        { return; }

        var isGreen = CanGreenLight(_constructions[0]);

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

        //is the main BuildersManager
        if (_building==null)
        {
            AddBuildingToClosestBuildingOffice(st, construction);
        }
    }

    /// <summary>
    /// The main BuildersManager contain in person controller will send this greenlight 
    /// building to the closest BuilderManager to the building 
    /// </summary>
    void AddBuildingToClosestBuildingOffice(Structure st, Construction construction)
    {
        //if is null is a brdige 
        if (st == null || st.StartingStage != H.Done)
        {
            //_greenLight.Add(construction);
            var closest = FindClosestWheelBarrowerOfficeFullyBuilt(construction.Position);
            closest.BuildersManager1.GreenLight.Add(construction);
        }
    }

    Structure FindClosestWheelBarrowerOfficeFullyBuilt(Vector3 construcPos)
    {
        return BuildingController.FindTheClosestOfThisTypeFullyBuilt(H.Masonry, construcPos);
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

    /// <summary>
    /// Called from QueuesContainer when all clearing a list of new builds
    /// 
    /// Its adding buildings that are being checked to _passedQueue list
    /// </summary>
    /// <param name="_newBuildsQueue"></param>
    internal void AddGreenLightKeys(QueueTask newBuildsQueue)
    {
        for (int i = 0; i < newBuildsQueue.Elements.Count; i++)
        {
            var key = newBuildsQueue.Elements[i].Key;
            if (!string.IsNullOrEmpty(key) && !newBuildsQueue.Elements[i].WasUsedToGreenLightOrDestroy
                && newBuildsQueue.Elements[i].IsCheckedByAll())
            {
                _passedQueue.Add(key);
                //so its not readded here again later 
                newBuildsQueue.Elements[i].WasUsedToGreenLightOrDestroy = true;
            }
        }
    }

    internal void AddGreenLightKeys(QueueElement qEle)
    {
        if (qEle.WasUsedToGreenLightOrDestroy)
        {
            return;
        }

        _passedQueue.Add(qEle.Key);
        qEle.WasUsedToGreenLightOrDestroy = true;
    }


#region User Changing Order of Buildings to be Greenlight and Greenlit









#endregion 
}

public class Construction
{
    public string Key;
    public H HType;
    public int Priority;
    public Vector3 Position;

    public Construction() { }
}