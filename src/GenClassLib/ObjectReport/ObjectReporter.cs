using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace MindHarbor.GenClassLib.ObjectReport {
	///<summary>
	/// A helper class that generate report <see cref="DataTable"/> on from objects
	///</summary>
	public class ObjectReporter {
		private const string EMPTY_COLUMN_NAME = "EMPTY_COLUMN_";
		private IList<string> deepCopyPath = new List<string>();
		private string defaultFormatString;
		private int depthCopingAt = 0;
		private int maximumCopyDepth = 4;
		private int numOfValidMappings = 0;
		private Dictionary<string, ColumnSetting> propertyColumnMappings = new Dictionary<string, ColumnSetting>();

		private Dictionary<string, ColumnSetting> PropertyColumnMappings {
			get { return propertyColumnMappings; }
		}

		#region public members

		public int MaximumCopyDepth {
			get { return maximumCopyDepth; }
			set { maximumCopyDepth = value; }
		}

		public string DefaultFormatString {
			get { return defaultFormatString; }
			set { defaultFormatString = value; }
		}

		/// <summary>
		/// Add a property-column mapping
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="columnName"></param>
		/// <remarks>
		/// The sequence of adding will be the sequence of the column. 
		/// If the columnName is null or string.Empty, the property won't be included in the report unless you set it as a deep copy
		/// </remarks>
		/// <param name="formatString"></param>
		/// <param name="deepCopy"></param>
		private void AddMapping(string propertyName, string columnName, string formatString, bool deepCopy) {
			if (PropertyColumnMappings.ContainsKey(propertyName)) {
				PropertyColumnMappings[propertyName].ColumnName = columnName;
				if (string.IsNullOrEmpty(columnName))
					throw new Exception("the property " + propertyName +
					                    " is already adedd, you can't remove it afterwards");
				return;
			}
			int index = -1;
			if (!string.IsNullOrEmpty(columnName)) {
				index = numOfValidMappings;
				numOfValidMappings ++;
			}
			PropertyColumnMappings[propertyName] = new ColumnSetting(columnName, index, formatString, deepCopy);
		}

		public void AddMapping(string propertyName, string columnName) {
			AddMapping(propertyName, columnName, null);
		}

		/// <summary>
		/// Set ObjectReporter to deepCopy the property with <paramref name="propertyName"/>
		/// </summary>
		/// <param name="propertyName"></param>
		public void AddDeepCopyMapping(string propertyName) {
			AddMapping(propertyName, string.Empty, string.Empty, true);
		}

		public void AddIgnoringMapping(string propertyName) {
			AddMapping(propertyName, string.Empty);
		}

		public void AddMapping(string propertyName, string columnName, string formatString) {
			AddMapping(propertyName, columnName, formatString, false);
		}

		public DataTable RotateTable(DataTable dt) {
			if (dt == null) return null;
			DataTable retVal = new DataTable(dt.TableName);
			retVal.Columns.Add("  ");
			for (int i = 0; i < dt.Rows.Count; i++) {
				string columnName = dt.Rows[i][0].ToString();
                if (retVal.Columns.Contains(columnName))
                {
                    columnName += "_DIFF";
                }

                //if (retVal.Columns.Contains(columnName))
                //    throw new Exception(
                //        "The first row of the table to rotate will be used as column and thus cannot have duplicate record value ");
				retVal.Columns.Add(columnName);
			}
			for (int i = 1; i < dt.Columns.Count; i++) {
				DataColumn column = dt.Columns[i];
				DataRow newRow = retVal.NewRow();
				newRow[0] = column.ColumnName;
				for (int j = 0; j < dt.Rows.Count; j++)
					newRow[j + 1] = dt.Rows[j][column.ColumnName];
				retVal.Rows.Add(newRow);
			}
			return retVal;
		}

		public DataTable ObjectsToTable(IEnumerable objects) {
			DataTable retVal = new DataTable();

			foreach (object item in objects) {
				ResetDeepCopyInfo();
				DataRow row = retVal.NewRow();
				ObjectItemToRow(item, retVal, row);
				retVal.Rows.Add(row);
			}
			if (retVal.Rows.Count > 0)
				CleanUpEmptyColumns(retVal);
			return retVal;
		}

		private void ResetDeepCopyInfo() {
			depthCopingAt = 0;
			deepCopyPath.Clear();
		}

		#endregion

		#region private methods

		private DataTable CleanUpEmptyColumns(DataTable val) {
			foreach (DataColumn column in new ArrayList(val.Columns))
				if (column.ColumnName.Contains(EMPTY_COLUMN_NAME))
					val.Columns.Remove(column);
			return val;
		}

		private void ObjectItemToRow(object item, DataTable retVal, DataRow row) {
			Type itemType = item.GetType();
			PropertyInfo[] itemProperties = itemType.GetProperties();
			foreach (PropertyInfo pi in itemProperties)
				AttributeStringToRow(pi.GetValue(item, null), pi.Name, retVal, row);
		}

		private void AttributeStringToRow(object attributeValue, string attributeName, DataTable dataTable, DataRow row) {
			ColumnSetting cs = MapTableColumn(attributeName, dataTable);
			if (cs.IsShowing)
				row[cs.ColumnName] = NullSafeToString(attributeValue, cs.FormatString);
			else if (cs.DeepCopy && depthCopingAt < MaximumCopyDepth && !deepCopyPath.Contains(attributeName)) {
				deepCopyPath.Add(attributeName);
				ObjectItemToRow(attributeValue, dataTable, row);
				depthCopingAt++;
			}
		}

		private object NullSafeToString(object o, string formatString) {
			if (o == null) return string.Empty;
			if (string.IsNullOrEmpty(formatString))
				return o.ToString();

			MethodInfo mi = o.GetType().GetMethod("ToString", new Type[] {typeof (string)});
			if (mi != null)
				try {
					object retVal = mi.Invoke(o, new object[] {formatString});
					return retVal;
				}
				catch (TargetInvocationException e) {
					if (e.InnerException is FormatException)
						Console.WriteLine("MindHarbor.GenClassLib.ObjectReport: Warnning! Format String \""
						                  + formatString + "\" is not supported by type " + o.GetType().Name +
						                  "! Plain ToString() is used instead.");
					else
						throw e.InnerException;
				}
			return o.ToString();
		}

		/// <summary>
		/// Get the column Name by the property Name, and ensure that the column exists.
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="dt"></param>
		/// <returns>the mapped column name if the property is not ignored according to the mapping, otherwise string.empty</returns>
		private ColumnSetting MapTableColumn(string propertyName, DataTable dt) {
			ColumnSetting retVal = new ColumnSetting(propertyName,
			                                         Math.Max(dt.Columns.Count, numOfValidMappings),
			                                         DefaultFormatString,
			                                         false);
			if (PropertyColumnMappings.ContainsKey(propertyName))
				retVal = PropertyColumnMappings[propertyName];

			if (retVal.IsShowing && dt.Columns.IndexOf(retVal.ColumnName) < 0) {
				for (int i = dt.Columns.Count; i <= retVal.ColumnIndex; i++)
					dt.Columns.Add(EMPTY_COLUMN_NAME + i);
				dt.Columns[retVal.ColumnIndex].ColumnName = retVal.ColumnName;
			}

			return retVal;
		}

		#endregion

		#region Nested type: ColumnSetting

		private class ColumnSetting {
			private int columnIndex;
			private string columnName;
			private bool deepCopy = false;
			private string formatString;

			internal ColumnSetting(string columnName, int columnIndex, string formatString, bool deepCopy) {
				this.columnName = columnName;
				this.columnIndex = columnIndex;
				this.formatString = formatString;
				this.deepCopy = deepCopy;
			}

			/// <summary>
			/// whether to deep copy the property entity - instead of just using ToString(), deep copy copy all the properties in the entity
			/// </summary>
			public bool DeepCopy {
				get { return deepCopy; }
				set { deepCopy = value; }
			}

			public string FormatString {
				get { return formatString; }
				private set { formatString = value; }
			}

			public int ColumnIndex {
				get { return columnIndex; }
				private set { columnIndex = value; }
			}

			public string ColumnName {
				get { return columnName; }
				set { columnName = value; }
			}

			public bool IsShowing {
				get { return ColumnIndex >= 0; }
			}
		}

		#endregion
	}
}