using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Classes.Menu_GUI.GUI.Bulletin
{
    class SubBulletinFinance
    {
        private BulletinWindow _bulletinWindow;

        public SubBulletinFinance(BulletinWindow bulletinWindow)
        {
            _bulletinWindow = bulletinWindow;
        }

        internal void ShowResume()
        {
            _bulletinWindow.ShowInBody("Finance Resume");
        }

        internal void ShowPrices()
        {
            ShowPrices(GetAllInInventories());
        }

        List<ProdSpec> GetAllInInventories()
        {
            List<ProdSpec> res = new List<ProdSpec>();
            return Program.gameScene.ExportImport1.TownProdSpecs;
        }


        List<PriceTile> _reports = new List<PriceTile>();
        private void ShowPrices(List<ProdSpec> list)
        {
            Hide();
            _bulletinWindow.ShowScrool();

            for (int i = 0; i < list.Count; i++)
            {
                var iniPosHere = _bulletinWindow.ScrollIniPosGo.transform.localPosition +
                                 new Vector3(0, -3.5f*i, 0);

                var a = PriceTile.CreateTile(_bulletinWindow.Content.gameObject.transform, list[i],
                    iniPosHere);

                _reports.Add(a);
            }
        }

        internal void Hide()
        {
            for (int i = 0; i < _reports.Count; i++)
            {
                _reports[i].Destroy();
            }
            _reports.Clear();
        }
    }
}
