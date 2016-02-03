using System.Collections.Generic;

public class ShipRegistro  
{

    List<Ship> _registerdShips = new List<Ship>();

    /// <summary>
    /// Ships had touch our land  at least once 
    /// </summary>
    public List<Ship> RegisterdShips
    {
        get { return _registerdShips; }
        set { _registerdShips = value; }
    }

    public ShipRegistro() { }
}
