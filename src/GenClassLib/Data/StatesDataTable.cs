using System;
using System.Data;

namespace MindHarbor.GenClassLib.Data {
	public class StatesDataTable : DataTable {
		#region SDTColumns enum

		public enum SDTColumns {
			abbreviation,
			fullName
		}

		#endregion

		public StatesDataTable() {
			Columns.Add(new DataColumn("abbreviation", typeof (String)));

			// The second column of the DataSource contain the Value
			Columns.Add(new DataColumn("fullName", typeof (String)));

			// Populate the table with rows.

			Rows.Add(CreateRow("AL", "Alabama", this));
			Rows.Add(CreateRow("AK", "Alaska", this));
			Rows.Add(CreateRow("AZ", "Arizona", this));
			Rows.Add(CreateRow("AR", "Arkansas", this));
			Rows.Add(CreateRow("CA", "California", this));
			Rows.Add(CreateRow("CO", "Colorado", this));
			Rows.Add(CreateRow("CT", "Connecticut", this));
			Rows.Add(CreateRow("DC", "D.C.", this));
			Rows.Add(CreateRow("DE", "Delaware", this));
			Rows.Add(CreateRow("FL", "Florida", this));
			Rows.Add(CreateRow("GA", "Georgia", this));
			Rows.Add(CreateRow("HI", "Hawaii", this));
			Rows.Add(CreateRow("ID", "Idaho", this));
			Rows.Add(CreateRow("IL", "Illinois", this));
			Rows.Add(CreateRow("IN", "Indiana", this));
			Rows.Add(CreateRow("IA", "Iowa", this));
			Rows.Add(CreateRow("KS", "Kansas", this));
			Rows.Add(CreateRow("KY", "Kentucky", this));
			Rows.Add(CreateRow("LA", "Louisiana", this));
			Rows.Add(CreateRow("ME", "Maine", this));
			Rows.Add(CreateRow("MD", "Maryland", this));
			Rows.Add(CreateRow("MA", "Massachusetts", this));
			Rows.Add(CreateRow("MI", "Michigan", this));
			Rows.Add(CreateRow("MN", "Minnesota", this));
			Rows.Add(CreateRow("MS", "Mississippi", this));
			Rows.Add(CreateRow("MO", "Missouri", this));
			Rows.Add(CreateRow("MT", "Montana", this));
			Rows.Add(CreateRow("NE", "Nebraska", this));
			Rows.Add(CreateRow("NV", "Nevada", this));
			Rows.Add(CreateRow("NH", "New Hampshire", this));
			Rows.Add(CreateRow("NJ", "New Jersey", this));
			Rows.Add(CreateRow("NM", "New Mexico", this));
			Rows.Add(CreateRow("NY", "New York", this));
			Rows.Add(CreateRow("NC", "North Carolina", this));
			Rows.Add(CreateRow("ND", "North Dakota", this));
			Rows.Add(CreateRow("OH", "Ohio", this));
			Rows.Add(CreateRow("OK", "Oklahoma", this));
			Rows.Add(CreateRow("OR", "Oregon", this));
			Rows.Add(CreateRow("PA", "Pennsylvania", this));
			Rows.Add(CreateRow("RI", "Rhode Island", this));
			Rows.Add(CreateRow("SC", "South Carolina", this));
			Rows.Add(CreateRow("SD", "South Dakota", this));
			Rows.Add(CreateRow("TN", "Tennessee", this));
			Rows.Add(CreateRow("TX", "Texas", this));
			Rows.Add(CreateRow("UT", "Utah", this));
			Rows.Add(CreateRow("VT", "Vermont", this));
			Rows.Add(CreateRow("VA", "Virginia", this));
			Rows.Add(CreateRow("WA", "Washington", this));
			Rows.Add(CreateRow("WV", "West Virginia", this));
			Rows.Add(CreateRow("WI", "Wisconsin", this));
			Rows.Add(CreateRow("WY", "Wyoming", this));
			Rows.Add(CreateRow("99", "International", this));
		}

		protected DataRow CreateRow(String abbreviation, String fullName, DataTable dt) {
			DataRow dr = dt.NewRow();
			dr["abbreviation"] = abbreviation;
			dr["fullName"] = fullName;
			return dr;
		}
	}
}