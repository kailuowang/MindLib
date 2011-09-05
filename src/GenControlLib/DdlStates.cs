using System.Web.UI.WebControls;
using MindHarbor.GenClassLib.Data;
using MindHarbor.GenControlLib.Bases;

namespace MindHarbor.GenControlLib {
	/// <summary>
	/// Summary description for DdlCountries.
	/// </summary>
	public class DdlStates : SpecializedDDLBase {
		public DdlStates() {}

		public override string DataTextField {
			get { return "Name"; }
		}

		public override string DataValueField {
			get { return "Abbr"; }
		}

		public override object DataSource {
			get { return StatesEnum.GetAllStates(); }
		}

		public StatesEnum SelectedState {
			get {
				if (SelectedValue == string.Empty)
					return null;
				else
					return StatesEnum.Parse(SelectedValue);
			}
			set {
				EnsureDataBound();
				SelectedValue = value.Abbr;
			}
		}

		public void SetSelectedRegion(string regionAbbr) {
			EnsureDataBound();
			ListItem item = Items.FindByValue(regionAbbr);
			if (item != null)
				SelectedValue = regionAbbr;
		}
	}
}