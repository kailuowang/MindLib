using System.Data;

namespace MindHarbor.GenClassLib.Data {
	/// <summary>
	/// Summary description for StatesCountriesDataHelper.
	/// </summary>
	public class StatesCountriesDataHelper {
		private const string SELECT_ONE_TEXT = "Please Select One";
		private const string SELECT_ONE_VALUE = "0";

		/// <summary>
		/// always return a new instance of the table
		/// </summary>
		public static DataTable Countries {
			get {
				return new CountriesDataTable();
				;
			}
		}

		/// <summary>
		/// always return a new instance of the table
		/// </summary>
		public static DataTable States {
			get { return new StatesDataTable(); }
		}

		public static void AddSelectOneRow(DataTable dt) {
			if (dt.Rows[0]["fullName"].ToString() == SELECT_ONE_TEXT)
				return;

			DataRow dr = dt.NewRow();
			dr[StatesDataTable.SDTColumns.fullName.ToString()] = SELECT_ONE_TEXT;
			dr[StatesDataTable.SDTColumns.abbreviation.ToString()] = SELECT_ONE_VALUE;
			dt.Rows.InsertAt(dr, 0);
			return;
		}
	}
}