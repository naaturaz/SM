using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Classes.Menu_GUI.GUI.Bulletin
{
    class SubBulletinWorkers
    {
        private BulletinWindow _bulletinWindow;

        //how many people work in places
        Dictionary<string, int> _report = new Dictionary<string, int>(); 

        List<Item> _finReport = new List<Item>(); 

        public SubBulletinWorkers(BulletinWindow win)
        {
            _bulletinWindow = win;
        }

        public void Show()
        {
            _report.Clear();
            _finReport.Clear();

            _bulletinWindow.ShowInBody(Workers());
        }

        private string Workers()
        {
            var workers = PersonPot.Control.All.Where(a => a.Work != null).ToList();
            CreateDict(workers);

            string res = "Workers distribution: \n ";

            for (int i = 0; i < _finReport.Count; i++)
            {
                var pad = Pad(" ", (_finReport[i].Key + ": ").Length, 15, 0);
                res += _finReport[i].Key + ": " + pad + _finReport[i].Value + "\n ";
            }

            return res;
        }

        string Pad(string pad, int current, int max, int padCount)
        {
            string res = "";

            var needPads = (max - current) + padCount;

            for (int i = 0; i < needPads; i++)
            {
                res += pad;
            }
            return res;
        }

        void CreateDict(List<Person> workers)
        {
            for (int i = 0; i < workers.Count; i++)
            {
                AddKeyToReport(workers[i].Work.HType+"");
            }

            for (int i = 0; i < _report.Count; i++)
            {
                _finReport.Add(new Item(_report.ElementAt(i).Key, _report.ElementAt(i).Value));
            }
            _finReport = _finReport.OrderByDescending(a => a.Value).ToList();
        }

        void AddKeyToReport(string key)
        {
            if (key.Contains("AnimalFarm"))
            {
                key = "AnimalFarm";
            }
            else if (key.Contains("FieldFarm"))
            {
                key = "FieldFarm";
            }

            //addig
            if (_report.ContainsKey(key))
            {
                _report[key] += 1;
            }
            else
            {
                _report.Add(key, 1);
            }
        }
    }

    class Item
    {
        private string _key;
        private int _value;

        public Item(string key, int val)
        {
            _key = key;
            _value = val;
        }

        public int Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }
    }
}
