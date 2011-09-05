using System;
using System.Collections.Generic;
using System.Text;
using MindHarbor.GenControlLib;

namespace MindHarbor.GenControlLib
{
    public class PageMenuGroup
    {
        public event System.EventHandler PageChanged;
        private readonly PageMenu[] pms;

        public PageMenuGroup(params PageMenu[] pms)
        {
            this.pms = pms;
            foreach (PageMenu pm in pms)
            {
                pm.PageChanged += new System.EventHandler(pm_PageChanged);
            }
        }

        void pm_PageChanged(object sender, System.EventArgs e)
        {
            SyncPageMenuCurrentPage((PageMenu)sender);
            if (PageChanged != null)
                PageChanged(this, new EventArgs());
        }

        public bool Visible
        {
            get { return pms[0].Visible; }
            set
            {
                foreach (PageMenu pm in pms)
                    pm.Visible = value;
            }
        }

        public int CurrentPageNumber
        {
            get { return pms[0].CurrentPageNumber; }
            set
            {
                foreach (PageMenu pm in pms) pm.CurrentPageNumber = value;
            }
        }

        public void SetNumberOfPagesBy(int size, int count)
        {
            foreach (PageMenu pm in pms)
            {
                pm.SetNumberOfPagesBy(size, count);
            }
        }

        public void SyncPageMenuCurrentPage(PageMenu pm)
        {
            foreach (PageMenu menu in pms)
            {
                if (menu != pm)
                    menu.CurrentPageNumber = pm.CurrentPageNumber;
            }
        }
    }
}
