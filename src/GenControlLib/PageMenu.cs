using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using MindHarbor.GenClassLib.MiscUtil;

namespace MindHarbor.GenControlLib {
    /// <summary>
    /// Summary description for PageMenu.
    /// </summary>
    public class PageMenu : WebControl {
        public const int DefaultNumberOfNumbersShown = 7;

        #region ViewState Properties
        public int NumberOfPages
        {
            get
            {
                if (ViewState["NumberOfPages"] == null)
                    ViewState["NumberOfPages"] = CurrentPageNumber;
                return (int)ViewState["NumberOfPages"];
            }
            set
            {
                bool changed = (ViewState["NumberOfPages"] == null || (int)ViewState["NumberOfPages"] != value);
                ViewState["NumberOfPages"] = value;

                if (changed)
                    CreateChildControls();
            }
        }


        public string PreviousButtonText
        {
            get
            {
                if (ViewState["PreviousButtonText"] == null)
                    ViewState["PreviousButtonText"] = "Previous";
                return (string)ViewState["PreviousButtonText"];
            }
            set
            {
                ViewState["PreviousButtonText"] = value;
            }
        }

        public string NextButtonText
        {
            get
            {
                if (ViewState["NextButtonText"] == null)
                    ViewState["NextButtonText"] = "Next";
                return (string)ViewState["NextButtonText"];
            }
            set
            {
                ViewState["NextButtonText"] = value;
            }
        }


        public int CurrentPageNumber
        {
            get
            {
                if (ViewState["CurrentPageNumber"] == null)
                    ViewState["CurrentPageNumber"] = 1;
                return (int)ViewState["CurrentPageNumber"];
            }
            set
            {
                bool changed = (ViewState["CurrentPageNumber"] == null || (int)ViewState["CurrentPageNumber"] != value);
                ViewState["CurrentPageNumber"] = value;
                if (changed)
                    CreateChildControls();
            }
        }

        public int NumberOfNumbersShown
        {
            get
            {
                if (ViewState["NumberOfNumbersShown"] == null)
                    ViewState["NumberOfNumbersShown"] = DefaultNumberOfNumbersShown;

                if (!MathUtil.IsOdd((int)ViewState["NumberOfNumbersShown"]))
                    ViewState["NumberOfNumbersShown"] = (int)ViewState["NumberOfNumbersShown"] + 1;

                int retVal = (int)ViewState["NumberOfNumbersShown"] < NumberOfPages
                                 ? (int)ViewState["NumberOfNumbersShown"]
                                 : NumberOfPages;

                return retVal;
            }
            set { ViewState["NumberOfNumbersShown"] = value; }
        } 
        #endregion

        private int startPage {
            get {
                if (NumberOfPages <= NumberOfNumbersShown)
                    return 1;
                int currentpg = CurrentPageNumber;
                int numOfNumshown = NumberOfNumbersShown;
                int onRight = (numOfNumshown - 1)/2 < (NumberOfPages - currentpg)
                                  ?
                                      (numOfNumshown - 1)/2
                                  : (NumberOfPages - currentpg);
                int start = currentpg - numOfNumshown + onRight + 1;
                if (start < 1)
                    start = 1;
                return start;
            }
        }

        private int endPage {
            get {
                if (NumberOfPages <= NumberOfNumbersShown)
                    return NumberOfPages;
                int end = startPage + NumberOfNumbersShown - 1;
                if (end > NumberOfPages)
                    end = NumberOfPages;
                return end;
            }
        }

        public event EventHandler PageChanged;

        protected override void OnInit(EventArgs e) {
            base.OnInit(e);
            Load += Page_Load;
        }

        private void AddEventHandlersToLinkButtons() {
            EnsureChildControls();
            foreach (HtmlTableCell cell in Controls[0].Controls[0].Controls)
                if (cell.Controls.Count > 0 && cell.Controls[0] is LinkButton)
                {
                    if (((LinkButton)cell.Controls[0]).CommandName == "Next")
                        ((LinkButton)cell.Controls[0]).Command += GoToNext_Command;
                    else if (((LinkButton)cell.Controls[0]).CommandName == "Previous")
                        ((LinkButton)cell.Controls[0]).Command += GoToPrevious_Command;
                    else 
                        ((LinkButton) cell.Controls[0]).Command += lnkBtn_Command;
                }

        }

        private void Page_Load(object sender, EventArgs e) {
            AddEventHandlersToLinkButtons();
        }

        public void SetNumberOfPagesBy(int pageSize, int numRecord) {
            NumberOfPages = Convert.ToInt32(Math.Ceiling(numRecord/(float) pageSize));
        }

        private void CheckCurrentPageNumber() {
            if (CurrentPageNumber > endPage)
                ViewState["CurrentPageNumber"] = 1;
        }

        protected override void CreateChildControls() {
            CheckCurrentPageNumber();
            int currentpg = CurrentPageNumber;
            base.CreateChildControls();
            Controls.Clear();
            HtmlTable table = new HtmlTable();

            table.Border = 0;
            table.CellPadding = 1;
            table.Style.Add("DISPLAY", "inline");
            table.Style.Add("VERTICAL-ALIGN", "middle");

            HtmlTableRow newRow = new HtmlTableRow();

            newRow.Cells.Add(CreatePreviousCell(PreviousButtonText));
            if (startPage > 1)
            {
                newRow.Cells.Add(CreatePageCell("1", 1, currentpg));
                HtmlTableCell pcell = new HtmlTableCell();
                pcell.InnerHtml = "...";
                newRow.Cells.Add(pcell);
            }

            for (int i = startPage; i <= endPage; i++)
                newRow.Cells.Add(CreatePageCell(i, currentpg));

            if (endPage < NumberOfPages)
            
            {
                HtmlTableCell pcell = new HtmlTableCell();
                pcell.InnerHtml = "...";
                newRow.Cells.Add(pcell);
                newRow.Cells.Add(CreatePageCell(NumberOfPages.ToString(), NumberOfPages, currentpg));

            }
            newRow.Cells.Add(CreateNextCell(NextButtonText));
            HtmlTableCell newCell = new HtmlTableCell();
            newCell.InnerText = "  Total: " + NumberOfPages + " Pages";
            newRow.Cells.Add(newCell);
            table.Rows.Add(newRow);
            Controls.Add(table);
        }


        private HtmlTableCell CreatePreviousCell(string text)
        {
            HtmlTableCell newCell = new HtmlTableCell();
            newCell.Align = "center";
            LinkButton lbPrevious = new LinkButton();
            lbPrevious.Text = text;

            lbPrevious.CommandName = "Previous";
            lbPrevious.ID = ID + "Previous";
            if (CurrentPageNumber == 1)
            {
                lbPrevious.Visible = false;
            }
                
            newCell.Controls.Add(lbPrevious);
            return newCell;
        }


        private HtmlTableCell CreateNextCell(string text)
        {
            HtmlTableCell newCell = new HtmlTableCell();
            newCell.Align = "center";
            LinkButton lbNext = new LinkButton();
            lbNext.Text = text;
            lbNext.CommandName = "Next";
            lbNext.ID = ID + "Next";
            if (CurrentPageNumber ==NumberOfPages)
            {
                lbNext.Visible = false;
            }

            newCell.Controls.Add(lbNext);
            return newCell;
        }

        private HtmlTableCell CreatePageCell(string text, int pageNumber, int currentPg) {
            HtmlTableCell newCell = new HtmlTableCell();
            newCell.Align = "center";

            if (pageNumber != currentPg) {
                LinkButton lnkBtn = new LinkButton();
                lnkBtn.Text = text;
                lnkBtn.CommandName = "Page";
                lnkBtn.CommandArgument = pageNumber.ToString();
                lnkBtn.Enabled = true;

                lnkBtn.ID = ID + pageNumber;

                newCell.Controls.Add(lnkBtn);
            }
            else
                newCell.InnerText = pageNumber.ToString();
            return newCell;
        }

        private HtmlTableCell CreatePageCell(int pageNumber, int currentpg) {
            return CreatePageCell(pageNumber.ToString(), pageNumber, currentpg);
        }


        private void GoToPrevious_Command(object sender, CommandEventArgs e)
        {
            CurrentPageNumber--;               
            if (PageChanged != null)
                PageChanged(this, e);
        }

        private void GoToNext_Command(object sender, CommandEventArgs e)
        {
           
            CurrentPageNumber++;
            if (PageChanged != null)
                PageChanged(this, e);
        }

        private void lnkBtn_Command(object sender, CommandEventArgs e) {
            if (CurrentPageNumber != Convert.ToInt32(e.CommandArgument)) {
                CurrentPageNumber = Convert.ToInt32(e.CommandArgument);

                if (PageChanged != null)
                    PageChanged(this, e);
            }
        }
    }
}