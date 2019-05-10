using UnityEngine;
using System.Collections.Generic;
using System.Net;
using System;
using System.Linq;

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

    public string GiveMeBestConstruction(Person person)
    {
        if (_greenLight.Count == 0)
        {
            return "None";
        }

        for (int i = 0; i < _greenLight.Count; i++)
        {
            if (!person.Brain.BlackList.Contains(_greenLight[i].Key))
            {
                var build = Brain.GetBuildingFromKey(_greenLight[i].Key);

                if (build != null)
                {
                    return _greenLight[i].Key;
                }
                else//means the build was deleted 
                {
                    _greenLight.RemoveAt(i);
                    i--;
                }
            }
        }
        return "None";
    }

    public void AddNewConstruction(string key, H hTypeP, int priority, Vector3 pos)
    {
        if (hTypeP == H.Road)
        { return; }

        //Brain.GetStructureFromKey(key) == null is a way and is not a brdige 
        if (Brain.GetStructureFromKey(key) == null && !key.Contains("Bridge"))
        {
            return;
        }
        if (Brain.GetStructureFromKey(key) != null && Brain.GetStructureFromKey(key).StartingStage == H.Done)
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
            _constructions = ChangePriorityList(key, priority, _constructions);
        }
        else if (ContainKey(_greenLight, key))
        {
            _greenLight = ChangePriorityList(key, priority, _greenLight);
        }
    }

    /// <summary>
    /// Reordered list 
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
                //send one step close ahead in the queue
                if(priority == 1)
                {
                    if(i > 0)
                    {
                        var current = list[i];
                        var prev = list[i - 1];

                        list[i - 1] = current;
                        list[i] = prev;

                        ReturnMaterials(prev);
                        UpdateClosestMasonry(prev);
                    }
                }
                //send one step behind in the queue
                else
                {
                    if(i + 1 < list.Count)
                    {
                        var current = list[i];
                        var next = list[i + 1];

                        list[i + 1] = current;
                        list[i] = next;

                        ReturnMaterials(current);
                        UpdateClosestMasonry(current);
                    }
                }
                break;
            }
        }
        return list;
    }

    private void UpdateClosestMasonry(Construction construction)
    {
        var closest = BuildingController.FindTheClosestOfThisType(H.Masonry, construction.Position,
            Brain.Maxdistance, true);

        if (closest == null)
        {
            Debug.Log("Not Masonry close enought to " + construction.Key + " found");
            return;
        }

        if (construction.WasGreenlit)
        {
            var build = Brain.GetBuildingFromKey(construction.Key);
            construction.WasGreenlit = false;

            if (_passedQueue != null)
                _passedQueue.Add(construction.Key);
        }

        closest.BuildersManager1.GreenLight.Remove(construction);
    }

    private void ReturnMaterials(Construction current)
    {
        if (!current.WasGreenlit) return;

        var st = Brain.GetStructureFromKey(current.Key);
        st.ReturnMaterialsCostNoQuestion();
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
    List<Construction> SwapItems(int indexA, int indexB, List<Construction> list)
    {
        var t = list[indexA];
        list[indexA] = list[indexB];
        list[indexB] = t;

        return list;
    }

    public static bool CanGreenLight(H hType)
    {
        var c = new Construction();
        c.HType = hType;
        return CanGreenLight(c, null);
    }

    /// <summary>
    /// Will commu8nicate with GameCOntrooler to see if have enought material to authorizr a building contruction
    /// 
    /// 
    /// </summary>
    /// <returns></returns>
    static bool CanGreenLight(Construction cons, List<string> passedQueue)
    {
        var stat = Book.GiveMeStat(cons.HType);

        bool wood = GameController.ResumenInventory1.ReturnAmtOfItemOnInv(P.Wood) >= stat.Wood || stat.Wood == 0;//if is zero
        //the needed amt will pass reagardless the amt we have on storage
        bool stone = GameController.ResumenInventory1.ReturnAmtOfItemOnInv(P.Stone) >= stat.Stone || stat.Stone == 0;
        bool brick = GameController.ResumenInventory1.ReturnAmtOfItemOnInv(P.Brick) >= stat.Brick || stat.Brick == 0;
        bool iron = GameController.ResumenInventory1.ReturnAmtOfItemOnInv(P.Iron) >= stat.Iron || stat.Iron == 0;
        bool gold = GameController.ResumenInventory1.ReturnAmtOfItemOnInv(P.Gold) >= stat.Gold || stat.Gold == 0;
        bool dollar = Program.gameScene.GameController1.Dollars >= stat.Dollar || stat.Dollar == 0;

        bool nail = GameController.ResumenInventory1.ReturnAmtOfItemOnInv(P.Nail) >= stat.Nail || stat.Nail == 0;
        bool furniture = GameController.ResumenInventory1.ReturnAmtOfItemOnInv(P.Furniture) >= stat.Furniture
            || stat.Furniture == 0;
        bool mortar = GameController.ResumenInventory1.ReturnAmtOfItemOnInv(P.Mortar) >= stat.Mortar || stat.Mortar == 0;
        bool floor = GameController.ResumenInventory1.ReturnAmtOfItemOnInv(P.FloorTile) >= stat.FloorTile
            || stat.FloorTile == 0;
        bool roof = GameController.ResumenInventory1.ReturnAmtOfItemOnInv(P.RoofTile) >= stat.RoofTile
          || stat.RoofTile == 0;
        bool machine = GameController.ResumenInventory1.ReturnAmtOfItemOnInv(P.Machinery) >= stat.Machinery
          || stat.Machinery == 0;

        //other wise would remove it from _passedQueue if was mising Brick for example and wont be 
        //build it ever again
        if (wood && stone && brick && iron && gold && dollar //&& passedQue
            && nail && furniture && mortar && floor && roof && machine)
        {
            //if is null is just a question for a building if can be greenlit
            //if not null means is this class trying to pass a Construction 
            if (passedQueue != null)
            {
                var build = Brain.GetBuildingFromKey(cons.Key);
                passedQueue.Remove(cons.Key);
            }
            return true;
        }
        return false;
    }

    public void Update()
    {
        CheckIfAnyToGreenLight();
        RemoveFullyBuiltOrRemoved();

        FreeUp();
    }


    private float _lastFreeUp;
    /// <summary>
    /// So people check their surroundings if constructions are up for over a minute 
    /// </summary>
    void FreeUp()
    {
        if (Time.time > _lastFreeUp + 60 && _constructions.Count > 0)
        {
            _lastFreeUp = Time.time;
            PersonPot.Control.RestartController();
        }
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

        if (st == null || st.StartingStage == H.Done)
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
        if (_constructions.Count == 0) return;
        if (_constructions[0].WasGreenlit) return;

        var isGreen = CanGreenLight(_constructions[0], _passedQueue);

        var st = Brain.GetStructureFromKey(_constructions[0].Key);
        var standingStructure = st != null && st.Instruction != H.WillBeDestroy;

        if (isGreen && standingStructure)
        {
            Debug.Log("Will Remove:" + _constructions[0].Key);
            RemoveFromGameControllerInventory(_constructions[0].HType);
            HandleList(_constructions[0]);
        }
    }

    /// <summary>
    /// Will remove 
    /// </summary>
    /// <param name="construction"></param>
    void HandleList(Construction construction)
    {
        construction.WasGreenlit = true;
        Structure st = Brain.GetStructureFromKey(construction.Key);

        //is the main BuildersManager
        if (_building == null)
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
            var closest = BuildingController.FindTheClosestOfThisType(H.Masonry, construction.Position,
                Brain.Maxdistance, true);

            if (closest == null)
            {
                //todo Notify not wheelBarrow close enought to me 3d icon
                Debug.Log("Not Masonry close enought to " + construction.Key + " found");
                return;
            }
            closest.BuildersManager1.GreenLight.Add(construction);
        }
    }

    void RemoveFromGameControllerInventory(H hTypeP)
    {
        var stat = Book.GiveMeStat(hTypeP);

        GameController.ResumenInventory1.Remove(P.Wood, stat.Wood);
        GameController.ResumenInventory1.Remove(P.Stone, stat.Stone);
        GameController.ResumenInventory1.Remove(P.Brick, stat.Brick);
        GameController.ResumenInventory1.Remove(P.Iron, stat.Iron);
        GameController.ResumenInventory1.Remove(P.Gold, stat.Gold);
        GameController.ResumenInventory1.Remove(P.Dollar, stat.Dollar);

        GameController.ResumenInventory1.Remove(P.Nail, stat.Nail);
        GameController.ResumenInventory1.Remove(P.Furniture, stat.Furniture);
        GameController.ResumenInventory1.Remove(P.Mortar, stat.Mortar);
        GameController.ResumenInventory1.Remove(P.FloorTile, stat.FloorTile);
        GameController.ResumenInventory1.Remove(P.RoofTile, stat.RoofTile);
        GameController.ResumenInventory1.Remove(P.Machinery, stat.Machinery);
    }

    internal bool WasIGreenLight(string myId)
    {
        for (int i = 0; i < _constructions.Count; i++)
        {
            if (_constructions[i].Key == myId)
            {
                return _constructions[i].WasGreenlit;
            }
        }
        //if is not found means was greenlit a while ago 
        return false;
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




    static List<string> _resources = new List<string>()
    {
        "wood", "stone", "brick" , "iron" ,"gold" , "money",
            "nail", "furniture", "mortar" , "floor" , "roof"  ,"machinery"
    };
    internal static string MissingResources(H hType)
    {
        var stat = Book.GiveMeStat(hType);

        bool wood = GameController.ResumenInventory1.ReturnAmtOfItemOnInv(P.Wood) >= stat.Wood || stat.Wood == 0;//if is zero
        //the needed amt will pass reagardless the amt we have on storage
        bool stone = GameController.ResumenInventory1.ReturnAmtOfItemOnInv(P.Stone) >= stat.Stone || stat.Stone == 0;
        bool brick = GameController.ResumenInventory1.ReturnAmtOfItemOnInv(P.Brick) >= stat.Brick || stat.Brick == 0;
        bool iron = GameController.ResumenInventory1.ReturnAmtOfItemOnInv(P.Iron) >= stat.Iron || stat.Iron == 0;
        bool gold = GameController.ResumenInventory1.ReturnAmtOfItemOnInv(P.Gold) >= stat.Gold || stat.Gold == 0;
        bool dollar = Program.gameScene.GameController1.Dollars >= stat.Dollar || stat.Dollar == 0;

        bool nail = GameController.ResumenInventory1.ReturnAmtOfItemOnInv(P.Nail) >= stat.Nail || stat.Nail == 0;
        bool furniture = GameController.ResumenInventory1.ReturnAmtOfItemOnInv(P.Furniture) >= stat.Furniture
            || stat.Furniture == 0;
        bool mortar = GameController.ResumenInventory1.ReturnAmtOfItemOnInv(P.Mortar) >= stat.Mortar || stat.Mortar == 0;
        bool floor = GameController.ResumenInventory1.ReturnAmtOfItemOnInv(P.FloorTile) >= stat.FloorTile
            || stat.FloorTile == 0;
        bool roof = GameController.ResumenInventory1.ReturnAmtOfItemOnInv(P.RoofTile) >= stat.RoofTile
          || stat.RoofTile == 0;
        bool machine = GameController.ResumenInventory1.ReturnAmtOfItemOnInv(P.Machinery) >= stat.Machinery
          || stat.Machinery == 0;

        List<bool> passed = new List<bool>()
        {
            wood, stone, brick , iron ,gold , dollar,
                nail, furniture, mortar , floor , roof, machine
        };

        string res = "";
        for (int i = 0; i < passed.Count; i++)
        {
            if (!passed[i])
            {
                res += Naming.CaseItRight(_resources[i]) + ", ";
            }
        }

        res = res.Substring(0, res.Length - 2);
        return res;
    }

    internal string PriorityInfo(String myId)
    {
        for (int i = 0; i < _constructions.Count; i++)
        {
            if (_constructions[i].Key == myId)
                return String.Format("\n\nConstruction Queue Rank: {0}, the shorter the number the better", (i + 1));
        }
        return "";
    }
    
    internal string CurrentPriorityRank(string myId)
    {
        for (int i = 0; i < _constructions.Count; i++)
        {
            if (_constructions[i].Key == myId)
                return "" + (i + 1);
        }
        return "";
    }

}

public class Construction
{
    public string Key;
    public H HType;
    public int Priority;
    public Vector3 Position;

    public Construction() { }

    public bool WasGreenlit { get; internal set; }
}