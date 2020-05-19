using System.Collections.Generic;
using UnityEngine;

public class ShipBB : CommandableBB
{
    private float _armor;
    private string _captain;
    private int _coins;
    private Sailor[] _crew;
    private CrownEnum _crown;
    private string _currentAni;
    private float _damage;
    private Immigrant[] _immigrant;
    private Inventory _inputToBeBuilt;
    private Inventory _inventory;
    private float _life;
    private Vector2 _mapCordinates;
    private string _name;
    private float _price;
    private TheRoute _route;
    private float _speed;
    private ShipStateEnum _state;
    private ShipEnum _type;

    public void Animate(string animation)
    {
        throw new System.NotImplementedException();
    }

    public void Combat()
    {
        throw new System.NotImplementedException();
    }

    public void CrewComsume(float food, float liquid)
    {
        throw new System.NotImplementedException();
    }

    public void Fire()
    {
        throw new System.NotImplementedException();
    }

    public void Load()
    {
        throw new System.NotImplementedException();
    }

    public void Resupply(List<Order> orders)
    {
        throw new System.NotImplementedException();
    }

    public void Save()
    {
        throw new System.NotImplementedException();
    }

    public void Storm()
    {
        throw new System.NotImplementedException();
    }

    public void Dock(Building dock)
    {
        throw new System.NotImplementedException();
    }

    public void LoadShip(Order order)
    {
        throw new System.NotImplementedException();
    }

    public void OffloadShip(Order order)
    {
        throw new System.NotImplementedException();
    }
}