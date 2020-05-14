using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class CUIMainInventory : MonoBehaviour
{
    public bool AddDebugProd;
    public CUICellInventory Cell;
    public float DebugAmount;
    public P DebugProd;
    public bool IsDebug;
    public bool RemoveDebugProd;

    private List<CUICellInventory> _cells = new List<CUICellInventory>();
    private Inventory _inv;

    private void CheckOnItems()
    {
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

        for (int i = 0; i < _inv.InventItems.Count; i++)
        {
            _inv.OrderItemsAlpha();
            var item = _inv.InventItems[i];
            var cell = _cells[i];
            cell.SetInvItem(item);
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

    private IEnumerator OneSecUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(1); // wait
            CheckOnItems();
        }
    }

    private void Start()
    {
        LoadDebug();
        StartCoroutine("OneSecUpdate");

        _inv = GameController.ResumenInventory1.GameInventory;

        foreach (var item in _inv.InventItems)
        {
            var cell = Instantiate(Cell, transform);
            cell.SetInvItem(item);
            _cells.Add(cell);
        }
    }

    private void Update()
    {
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
}