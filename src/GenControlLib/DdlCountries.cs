using System.Data;
using MindHarbor.GenClassLib.Data;

namespace MindHarbor.GenControlLib {
	/// <summary>
	/// Summary description for DdlCountries.
	/// </summary>
	public class DdlCountries : DLGeographicBase {
		public DdlCountries() {
			//
			// TODO: Add constructor logic here
			//
		}

		protected override DataTable ObtainDataSource() {
			return StatesCountriesDataHelper.Countries;
		}
	}
}