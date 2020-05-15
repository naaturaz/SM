using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Classes.Menu_GUI.GUI.Bulletin
{
    internal class SubBulletinGeneral
    {
        private BulletinWindow _bulletinWindow;

        //how many people work in places
        private Dictionary<string, int> _report = new Dictionary<string, int>();

        private List<Item> _finReport = new List<Item>();

        public SubBulletinGeneral(BulletinWindow win)
        {
            _bulletinWindow = win;
        }

        #region Control Workers

        private bool reDoneSalaries;//for users  that changed salaries already

        public void ShowWorkers()
        {
            if (!reDoneSalaries)
            {
                reDoneSalaries = true;
                BuildingPot.Control.Registro.DoEquealPaymentForAllWorks();
            }

            _report.Clear();

            var list = BuildingPot.Control.Registro.StringOfAllBuildingsThatAreAWork().Distinct().ToList();

            //list.Insert(0, "Unemployed");
            ShowWorkers(list);

            _bulletinWindow.AdjustContentHeight(list.Count * 5.2f);
        }

        private List<WorkerTile> _reports = new List<WorkerTile>();

        private void ShowWorkers(List<string> list)
        {
            var tileHeight = -4.35f;
            var height = list.Count * tileHeight;

            Hide();
            _bulletinWindow.ShowScrool();
            _bulletinWindow.AdjustContentHeight(height);

            for (int i = 0; i < list.Count; i++)
            {

                var a = WorkerTile.CreateTile(_bulletinWindow.Content.gameObject.transform, list[i],
                    new Vector3());

                _reports.Add(a);
            }
        }

        private float ReturnTileYScale()
        {
            if (_reports.Count > 0)
            {
                return _reports[0].transform.localScale.y;
            }
            return 0;
        }

        public static float ReturnRelativeYSpace(float relative, float ySpaceOfTile)
        {
            return relative * ySpaceOfTile;
        }

        internal void Hide()
        {
            for (int i = 0; i < _reports.Count; i++)
            {
                _reports[i].Destroy();
            }
            _reports.Clear();
        }

        #endregion Control Workers

        private string Pad(string pad, int current, int max, int padCount)
        {
            string res = "";

            var needPads = (max - current) + padCount;

            for (int i = 0; i < needPads; i++)
            {
                res += pad;
            }
            return res;
        }

        private void CreateDict(List<Person> workers)
        {
            for (int i = 0; i < workers.Count; i++)
            {
                AddKeyToReport(workers[i].Work.HType + "");
            }

            for (int i = 0; i < _report.Count; i++)
            {
                _finReport.Add(new Item(_report.ElementAt(i).Key, _report.ElementAt(i).Value));
            }
            _finReport = _finReport.OrderByDescending(a => a.Value).ToList();
        }

        private void CreateDict(List<string> builds)
        {
            for (int i = 0; i < builds.Count; i++)
            {
                AddKeyToReport(builds[i]);
            }

            for (int i = 0; i < _report.Count; i++)
            {
                _finReport.Add(new Item(_report.ElementAt(i).Key, _report.ElementAt(i).Value));
            }
            _finReport = _finReport.OrderByDescending(a => a.Value).ToList();
        }

        private void AddKeyToReport(string key)
        {
            //if (key.Contains("AnimalFarm"))
            //{
            //    key = "AnimalFarm";
            //}
            //else if (key.Contains("FieldFarm"))
            //{
            //    key = "FieldFarm";
            //}

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

        #region Buildings

        public void ShowBuildings()
        {
            _report.Clear();
            _finReport.Clear();

            _bulletinWindow.ShowInBody(Buildings());
        }

        private string Buildings()
        {
            var hTypes = BuildingPot.Control.Registro.StringOfAllBuildingsHType();
            CreateDict(hTypes);

            string res = Languages.ReturnString("Buildings") + ":\n ";

            var count = _finReport.Count > 15 ? 15 : _finReport.Count;

            for (int i = 0; i < count; i++)
            {
                //var pad = Pad(" ", (_finReport[i].Key + ": ").Length, 15, 0);
                res += Languages.ReturnString(_finReport[i].Key) + ": " + _finReport[i].Value + "\n ";
            }

            return res;
        }

        #endregion Buildings
    }

    internal class Item
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