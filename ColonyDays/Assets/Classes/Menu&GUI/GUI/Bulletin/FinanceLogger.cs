using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class FinanceLogger
{
    List<Budget> _budgets = new List<Budget>();

    internal List<Budget> Budgets
    {
        get { return _budgets; }
        set { _budgets = value; }
    }

    public FinanceLogger() { }

    public FinanceLogger(bool first)
    {
        AddYearBudget();
    }

    public void AddYearBudget()
    {
        _budgets.Add(new Budget(Program.gameScene.GameTime1.Year));
    }

    public void AddToAcct(string acct, float bal)
    {
        var curr = _budgets[_budgets.Count - 1];
        curr.AddToAcct(acct, bal);
    }






    int showCurrent = -1;
    /// <summary>
    /// Will create the resume of the current budget 
    /// </summary>
    /// <returns></returns>
    internal List<DisplayAccount> ResumenCurrentBudget()
    {
        if (showCurrent == -1)
        {
            showCurrent = _budgets.Count - 1;
        }

        var curr = _budgets[showCurrent];
        return CreateDisplayAccts(curr);
    }


    public void SetResumenToPrevYear()
    {
        showCurrent -= 1;
    }
    public void SetResumenToNextYear()
    {
        showCurrent += 1;
    }


    private List<DisplayAccount> CreateDisplayAccts(Budget curr)
    {
        return curr.CreateDisplayAccts();
    }





    public bool ThereIsANextYear(int askingYear)
    {
        var index = _budgets.FindIndex(a => a.Year == askingYear + 1);
        return index != -1;
    }

    public bool ThereIsAPrevYear(int askingYear)
    {
        var index = _budgets.FindIndex(a => a.Year == askingYear - 1);
        return index != -1;
    }
}

class Budget
{
    int _year;

    public int Year
    {
        get { return _year; }
        set { _year = value; }
    }

    float _initBalAcct;

    public float InitBalAcct
    {
        get { return _initBalAcct; }
        set { _initBalAcct = value; }
    }

    //if change update negative or positve acct right below 
    List<string> _acctNames = new List<string>()
    {
        "Exports", "Overseas Trade", "Crown Trade", "Quests Completion",
        "Imports", "New bought lands", "Salary", "Construction"
    };


    List<string> _posAcctNames = new List<string>()
    {
         "Exports", "Overseas Trade", "Crown Trade", "Quests Completion",
    };
    List<string> _negativeAcctNames = new List<string>()
    {
        "Imports", "New bought lands", "Salary", "Construction"
    };

    List<Account> _accounts = new List<Account>();

    internal List<Account> Accounts
    {
        get { return _accounts; }
        set { _accounts = value; }
    }

    public Budget(){}

    public Budget(int year)
    {
        _year = year;
        _initBalAcct = Program.gameScene.GameController1.Dollars;

        for (int i = 0; i < _acctNames.Count; i++)
        {
            _accounts.Add(new Account(_acctNames[i], 0));
        }
    }

    internal void AddToAcct(string acct, float bal)
    {
        var index = _accounts.FindIndex(a => a.Name == acct);
        _accounts[index].Add(bal);
    }

    float ValOfAnAcct(string acct)
    {
        var index = _accounts.FindIndex(a => a.Name == acct);
        return _accounts[index].Balance;
    }

    float SubTotal(List<string> list)
    {
        float res = 0;
        for (int i = 0; i < list.Count; i++)
        {
            res += ValOfAnAcct(list[i]);
        }
        return res;
    }


    internal List<DisplayAccount> CreateDisplayAccts()
    {
        List<DisplayAccount> res = new List<DisplayAccount>();

        res.Add(new DisplayAccount(Color.blue, 0, "Budget Resumen", -1, true, 3));
        res.Add(new DisplayAccount(Color.black, 0, "Year", Year, false, 2));

        //initial
        //space
        //res.Add(new DisplayAccount(Color.white, 0, "", 0, false, 0));
        res.Add(new DisplayAccount(Color.black, 0, "Initial Balance", InitBalAcct, false, 1));


        //income
        //space
        //res.Add(new DisplayAccount(Color.white, 0, "", 0, false, 0));
        res.Add(new DisplayAccount(Color.black, 0, "Income", -1, true, 1));
        for (int i = 0; i < _posAcctNames.Count; i++)
        {
            var name =_posAcctNames[i];
            res.Add(new DisplayAccount(Color.green, 3, name, ValOfAnAcct(name), false, 0));
        }
        res.Add(new DisplayAccount(Color.yellow, 1, "Income SubTotal", SubTotal(_posAcctNames), false, 0));

        //expenses
        //space
        //res.Add(new DisplayAccount(Color.white, 0, "", 0, false, 0));
        res.Add(new DisplayAccount(Color.black, 0, "Expenses", -1, true, 1));
        for (int i = 0; i < _negativeAcctNames.Count; i++)
        {
            var name = _negativeAcctNames[i];
            res.Add(new DisplayAccount(Color.red, 3, name, ValOfAnAcct(name), false, 0));
        }
        res.Add(new DisplayAccount(Color.yellow, 1, "Expenses SubTotal", SubTotal(_negativeAcctNames), false, 0));


        //space
        res.Add(new DisplayAccount(Color.white, 0, "Balance", -1, false, 0));
        var yearBal =  SubTotal(_posAcctNames) - SubTotal(_negativeAcctNames);
        res.Add(new DisplayAccount(Color.yellow, 1, "Year balance"
            , yearBal, false, 0));
        res.Add(new DisplayAccount(Color.black, 0, "Current Balance", InitBalAcct + yearBal, true, 2));



        return res;
    }
}

class Account
{
    string _name;

    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }
    float _balance;

    public float Balance
    {
        get { return _balance; }
        set { _balance = value; }
    }

    public Account() { }

    public Account(string name, float bal)
    {
        _name = name;
        _balance = bal;
    }



    internal void Add(float bal)
    {
        _balance += bal;
    }
}


/// <summary>
/// For displaying on the bulletin
/// </summary>
public class DisplayAccount
{
    Color _color = Color.white;

    public Color Color
    {
        get { return _color; }
        set { _color = value; }
    }
    int _blankSpaces;

    public int BlankSpaces
    {
        get { return _blankSpaces; }
        set { _blankSpaces = value; }
    }
    string _name;

    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }
    float _balance;

    public float Balance
    {
        get { return _balance; }
        set { _balance = value; }
    }
    bool _boldFont;

    public bool BoldFont
    {
        get { return _boldFont; }
        set { _boldFont = value; }
    }
    float _addSizeFont;

    public float AddSizeFont
    {
        get { return _addSizeFont; }
        set { _addSizeFont = value; }
    }

    public DisplayAccount(Color color, int blankSpaces, string name, float balance, 
        bool boldFont, float addSizeFont)
    {
        _color = color;
        _blankSpaces = blankSpaces;
        _name = name;
        _balance = balance;
        _boldFont = boldFont;
        _addSizeFont = addSizeFont;
    }

}