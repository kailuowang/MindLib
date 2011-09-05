using System;
using System.Web.UI.WebControls;

namespace MindHarbor.GenControlLib.Bases {
	public abstract class SpecializedDDLBase : DropDownList {
		protected const string PLZ_SELECT_ITEM_VALUE = "0";

		public bool ShowPlzSelectItem {
			get {
				if (ViewState["ShowPlzSelectItem"] == null)
					ViewState["ShowPlzSelectItem"] = true;
				return (bool) ViewState["ShowPlzSelectItem"];
			}
			set {
				bool changed = ShowPlzSelectItem != value;
				ViewState["ShowPlzSelectItem"] = value;
				if (changed)
					DataBind();
			}
		}

		public string PlzSelectText {
			get {
				if (ViewState["PlzSelectText"] == null)
					ViewState["PlzSelectText"] = "Please Select";
				return (string) ViewState["PlzSelectText"];
			}
			set {
				bool changed = PlzSelectText != value;
				ViewState["PlzSelectText"] = value;
				if (changed && ShowPlzSelectItem && Items.Count > 0)
					Items[0].Text = value;
			}
		}

		public override sealed int SelectedIndex {
			set {
				EnsureDataBound();
				base.SelectedIndex = value;
			}
		}

		public override sealed string SelectedValue {
			set {
				EnsureDataBound();
				base.SelectedValue = value;
			}
		}

		public abstract override string DataValueField { get; }
		public abstract override string DataTextField { get; }
		public abstract override object DataSource { get; }

		protected override sealed void EnsureDataBound() {
			base.EnsureDataBound();
			if (Items.Count <= 0)
				DataBind();
		}

		protected override sealed void OnPreRender(EventArgs e) {
			EnsureDataBound();
		}

		/// <summary>
		/// will Add the "Please Select Item"
		/// </summary>
		public override sealed void DataBind() {
			if (DataSource == null) return;
			base.DataBind();
			if (ShowPlzSelectItem)
				Items.Insert(0, new ListItem(PlzSelectText, PLZ_SELECT_ITEM_VALUE));
		}
	}
}