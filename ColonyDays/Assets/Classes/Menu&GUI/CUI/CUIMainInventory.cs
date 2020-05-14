using System.Collections.Generic;
using UnityEngine;

internal class CUIMainInventory : MonoBehaviour
{
    public CUICellInventory Cell;
    public P DebugProd;
    public float DebugAmount;
    public bool AddDebugProd;
    public bool RemoveDebugProd;

    private Inventory _inv;
    private List<CUICellInventory> _cells = new List<CUICellInventory>();

    private void Start()
    {
        //_inv = GameController.ResumenInventory1.GameInventory;

        _inv = new Inventory();
        _inv.Add(P.Bean, 100);
        _inv.Add(P.Sugar, 55555);

        foreach (var item in _inv.InventItems)
        {
            var cell = Instantiate(Cell, transform);
            cell.SetInvItem(item);
            _cells.Add(cell);
        }
    }

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
                Destroy(_cells[i]);
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

    private void Update()
    {
        if (AddDebugProd)
        {
            AddDebugProd = false;
            _inv.Add(DebugProd, DebugAmount);
            CheckOnItems();
        }
    }
}