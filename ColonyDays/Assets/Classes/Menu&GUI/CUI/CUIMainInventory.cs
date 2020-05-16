using System.Collections.Generic;
using UnityEngine;

internal class CUIMainInventory : MonoBehaviour
{
    public ScrollItemsWindow Which;

    public bool AddDebugProd;
    public CUICellInventory Cell;
    public float DebugAmount;
    public P DebugProd;
    public bool IsDebug;
    public bool RemoveDebugProd;

    private List<CUICellInventory> _cells = new List<CUICellInventory>();
    private Inventory _inv;
    private int _count;

    private void CheckOnItems()
    {
        if (_inv == null)
        {
            Start();
            return;
        }

        for (int i = 0; i < _inv.InventItems.Count; i++)
        {
            if (i > _cells.Count - 1)
            {
                var cell = Instantiate(Cell, transform);
                _cells.Add(cell);
            }
        }

        for (int i = 0; i < _cells.Count; i++)
        {
            if (i > _inv.InventItems.Count - 1)
            {
                Destroy(_cells[i].gameObject);
                _cells.RemoveAt(i);
                i--;
            }
        }

        if (_inv.InventItems.Count > 0 && _inv.InventItems[0].Key != P.Year)
            _inv.OrderItemsAlpha();

        for (int i = 0; i < _inv.InventItems.Count; i++)
        {
            var item = _inv.InventItems[i];
            var cell = _cells[i];
            cell.SetInvItem(item, Which);
            cell.transform.SetSiblingIndex(i);
        }
    }

    private void LoadDebug()
    {
        if (!IsDebug) return;
        _inv = new Inventory();
        _inv.Add(P.Bean, 100);
        _inv.Add(P.Sugar, 55555);
        _inv.Add(P.Sail, 55555);
        _inv.Add(P.Gold, 55555);
        _inv.Add(P.Banana, 55555);
        _inv.Add(P.Diamond, 55555);
        _inv.Add(P.Beef, 55555);
        _inv.Add(P.PalmLeaf, 55555);
        _inv.Add(P.Fabric, 55555);
        _inv.Add(P.QuickLime, 55555);
        _inv.Add(P.Leather, 55555);
        _inv.Add(P.Leather, 55555);
        _inv.Add(P.Papaya, 55555);
        _inv.Add(P.Paper, 55555);
    }

    private void Start()
    {
        LoadDebug();

        if (Which == ScrollItemsWindow.Main || Which == ScrollItemsWindow.OurInventories)
            _inv = GameController.ResumenInventory1.GameInventory;
        else if (Which == ScrollItemsWindow.ProduceReport)
            _inv = CreateSimpleInv(BulletinWindow.SubBulletinProduction1.ProductionReport1.ProduceReport);
        else if (Which == ScrollItemsWindow.ConsumeReport)
            _inv = CreateSimpleInv(BulletinWindow.SubBulletinProduction1.ProductionReport1.ConsumeReport);
        else if (Which == ScrollItemsWindow.ExpireReport)
            _inv = CreateSimpleInv(BulletinWindow.SubBulletinProduction1.ExpirationReport.ProduceReport);
    }

    private Inventory CreateSimpleInv(List<Inventory> list)
    {
        Inventory inv = new Inventory();
        for (int i = 0; i < list.Count; i++)
        {
            list[i].OrderItemsAlpha();
            inv.Add(list[i]);
        }
        return inv;
    }

    private void Update()
    {
        CheckOnItems();
        ShowBuildingOrPerson();

        if (AddDebugProd)
        {
            AddDebugProd = false;
            _inv.Add(DebugProd, DebugAmount);
            CheckOnItems();
        }
        if (RemoveDebugProd)
        {
            RemoveDebugProd = false;
            _inv.RemoveByWeight(DebugProd, DebugAmount);
            CheckOnItems();
        }
    }

    private void ShowBuildingOrPerson()
    {
        if (Which == ScrollItemsWindow.Building)
            if (Program.MouseListener.BuildingWindow1 != null && Program.MouseListener.BuildingWindow1.Building != null)
                _inv = Program.MouseListener.BuildingWindow1.Building.Inventory;

        if (Which == ScrollItemsWindow.BuildingStats)
            if (Program.MouseListener.BuildingWindow1 != null && Program.MouseListener.BuildingWindow1.Building != null &&
                Program.MouseListener.BuildingWindow1.Building.ProductionReport != null)
                _inv = CreateSimpleInv(Program.MouseListener.BuildingWindow1.Building.ProductionReport.ProduceReport);

        if (Which == ScrollItemsWindow.Person)
            if (Program.MouseListener.PersonWindow1 != null && Program.MouseListener.PersonWindow1.Person1 != null)
                _inv = Program.MouseListener.PersonWindow1.Person1.Inventory;
    }
}

public enum ScrollItemsWindow //to be use for the person class
{ Main, OurInventories, Building, BuildingStats, Person, ProduceReport, ConsumeReport, ExpireReport }