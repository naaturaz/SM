using System.Collections.Generic;

public class EmigrateController
{
    private List<Emigrate> _emigrates = new List<Emigrate>();
    private Dictionary<string, Emigrate> _emigratesGC = new Dictionary<string, Emigrate>();

    private int _lastYearChecked;

    public List<Emigrate> Emigrates
    {
        get { return _emigrates; }
        set { _emigrates = value; }
    }

    public EmigrateController()
    {
    }

    // Update is called once per frame
    public void Update()
    {
        if (_lastYearChecked != Program.gameScene.GameTime1.Year)
        {
            CheckOnAllEmigrates();
        }
    }

    private int count;

    private void CheckOnAllEmigrates()
    {
        if (count < _emigrates.Count)
        {
            _emigrates[count].YearCheck();
            count++;
        }
        else
        {
            count = 0;
            _lastYearChecked = Program.gameScene.GameTime1.Year;
        }
    }

    internal void AddEmigrate(Person emigrate)
    {
        if (_emigratesGC.ContainsKey(emigrate.MyId))
        {
            return;
        }

        var newEmig = new Emigrate(emigrate);

        _emigratesGC.Add(emigrate.MyId, newEmig);
        _emigrates.Add(newEmig);
    }

    internal void RemoveEmigrate(Emigrate emigrate)
    {
        _emigratesGC.Remove(emigrate.MyId);
        _emigrates.Remove(emigrate);
    }

    /// <summary>
    /// Needs to be called when loading. So its recreates the Dictionary for GC purposes
    /// </summary>
    public void RecreateEmigratesGC()
    {
        for (int i = 0; i < _emigrates.Count; i++)
        {
            _emigratesGC.Add(Emigrates[i].MyId, Emigrates[i]);
        }
    }
}