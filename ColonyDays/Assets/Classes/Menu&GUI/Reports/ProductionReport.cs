using System.Collections.Generic;

public class ProductionReport
{
    private List<Inventory> _produceReport = new List<Inventory>();
    private List<Inventory> _consumeReport = new List<Inventory>();

    public List<Inventory> ProduceReport
    {
        get { return _produceReport; }
        set { _produceReport = value; }
    }

    public List<Inventory> ConsumeReport
    {
        get { return _consumeReport; }
        set { _consumeReport = value; }
    }

    public void AddProductionThisYear(P p, float amt)
    {
        var thisYear = Program.gameScene.GameTime1.Year + "";

        //find this year report
        var thisYearReport = _produceReport.Find(a => a.LocMyId == thisYear);

        //if none will create this year report right away
        if (thisYearReport == null)
        {
            thisYearReport = new Inventory(thisYear, H.YearReport);
            //so the newest in on top
            _produceReport.Insert(0, thisYearReport);
        }
        thisYearReport.Add(p, amt);
    }

    public void AddConsumeThisYear(P p, float amt)
    {
        var thisYear = Program.gameScene.GameTime1.Year + "";

        //find this year report
        var thisYearReport = _consumeReport.Find(a => a.LocMyId == thisYear);

        //if none will create this year report right away
        if (thisYearReport == null)
        {
            thisYearReport = new Inventory(thisYear, H.YearReport);
            //so the newest in on top
            _consumeReport.Insert(0, thisYearReport);
        }
        thisYearReport.Add(p, amt);
    }
}