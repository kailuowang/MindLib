using System.Data;
using System.Web.UI.WebControls;
using MindHarbor.GenClassLib.Data;

namespace MindHarbor.GenControlLib {
	/// <summary>
	/// Summary description for DLGeographicBase.
	/// </summary>
	public class DLGeographicBase : DropDownList {
		private bool ready = false;

		public DLGeographicBase() {
			//
			// TODO: Add constructor logic here
			//
			Setup();
		}

		public bool ShowPlzSelectItem {
			get {
				if (ViewState["ShowPlzSelectItem"] == null)
					ViewState["ShowPlzSelectItem"] = true;
				return (bool) ViewState["ShowPlzSelectItem"];
			}
			set {
				ViewState["ShowPlzSelectItem"] = value;
				Setup();
			}
		}

		protected override void CreateChildControls() {
			base.CreateChildControls();
			Setup();
		}

		private void Setup() {
			if (ready) return;
			ready = true;
			DataTable dt = ObtainDataSource();
			if (ShowPlzSelectItem)
				StatesCountriesDataHelper.AddSelectOneRow(dt);
			DataSource = dt;
			DataTextField = "fullName";
			DataValueField = "abbreviation";
			DataBind();
		}

		/// <summary>
		/// The safe way to set the selected region using the name, if there is no match the method do nothing
		/// </summary>
		/// <param name="regionName"></param>
		/// <remarks>
		/// the SelectedValue = "contry name" won't throw an exception immediatly, so trying to catch it will be no use. 
		/// </remarks>
		public void SetSelectedRegion(string regionName) {
			Setup();
			ListItem item = Items.FindByValue(regionName);
			if (Items.FindByValue(regionName) != null)
				SelectedValue = regionName;
		}

		protected virtual DataTable ObtainDataSource() {
			return StatesCountriesDataHelper.Countries;
		}
	}
}