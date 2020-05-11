/// <summary>
/// Since a building now needs an Inventory, WaterInventory
/// </summary>
public class InventoryManager
{
    private Inventory _inventory;
    private Inventory _waterInventory;
    private Inventory _liquidInventory;

    public Inventory Inventory1
    {
        get { return _inventory; }
        set { _inventory = value; }
    }

    public Inventory WaterInventory
    {
        get { return _waterInventory; }
        set { _waterInventory = value; }
    }

    public InventoryManager()
    {
    }

    public InventoryManager(General gen)
    {
        _inventory = new Inventory(gen.MyId, gen.HType);
        _waterInventory = new Inventory(gen.MyId, gen.HType);
        _liquidInventory = new Inventory(gen.MyId, gen.HType);
    }

    public void RemoveByWeight(P product, float amt)
    {
        //and not buckets : return
        if (IsLiquid(product))
        {
            //if not bukkect
            if (!GameController.AreThereTonelsOnStorage && IsLiquid(product))
            {
                return;
            }

            if (product == P.Water)
            {
                _waterInventory.RemoveByWeight(product, amt);
            }
            else
            {
                _liquidInventory.RemoveByWeight(product, amt);
            }
        }
        else
        {
            //if there are not crates and needs a crate then cant carry this item
            if (!GameController.AreThereCratesOnStorage && DoesNeedACrate(product))
            {
                return;
            }
            _inventory.RemoveByWeight(product, amt);
        }
    }

    public void Add(P product, float amt)
    {
        if (IsLiquid(product))
        {
            if (product == P.Water)
            {
                _waterInventory.Add(product, amt);
            }
            else
            {
                _liquidInventory.Add(product, amt);
            }
        }
        else
        {
            _inventory.Add(product, amt);
        }
        RemoveContainerUsed(product);
    }

    private bool IsLiquid(P prod)
    {
        return prod == P.Water || prod == P.Beer || prod == P.Rum || prod == P.Ink;
    }

    //Usage of Containers
    private void RemoveContainerUsed(P prod)
    {
        if (DoesNeedACrate(prod))
        {
            //each time a person uses a crrate
            //they get used and diminished
            GameController.ResumenInventory1.Remove(P.Crate, .01f);
        }
        else if (IsLiquid(prod))
        {
            GameController.ResumenInventory1.Remove(P.Bucket, .01f);
        }
    }

    private bool DoesNeedACrate(P prod)
    {
        if (prod == P.Wood || prod == P.Stone || prod == P.Ore)
        {
            return false;
        }
        return true;
    }

    public bool IsFullOfWater()
    {
        return _waterInventory.IsFull();
    }
}