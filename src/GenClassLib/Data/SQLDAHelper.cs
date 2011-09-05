using System;
using System.Data;
using Microsoft.ApplicationBlocks.Data;

namespace MindHarbor.GenClassLib.Data {
	/// <summary>
	/// Set the connection String before using any of the method.
	/// this helper class is only useful one there is only one database
	/// </summary>
	public class SQLDAHelper {
		public SQLDAHelper(string connectionString) {
			_connectionString = connectionString;
		}

		#region Properteis

		private string _connectionString;

		public string ConnectionString {
			get { return _connectionString; }
		}

		#endregion

		#region DataAccess Helpers

		public DataSet ExecuteDataset(string command) {
			return SqlHelper.ExecuteDataset(ConnectionString,
			                                CommandType.Text, command);
		}

		public DataSet ExecuteDataset(string command, object[] parameters) {
			return SqlHelper.ExecuteDataset(ConnectionString,
			                                command, parameters);
		}

		public IDataReader ExecuteReader(string command) {
			return SqlHelper.ExecuteReader(ConnectionString,
			                               CommandType.Text, command);
		}

		public IDataReader ExecuteReader(string command, object[] parameters) {
			return SqlHelper.ExecuteReader(ConnectionString,
			                               command, parameters);
		}

		private object ExecuteScalar(string command) {
			return SqlHelper.ExecuteScalar(ConnectionString,
			                               CommandType.Text, command);
		}

		private object ExecuteScalar(string command, object[] parameters) {
			return SqlHelper.ExecuteScalar(ConnectionString,
			                               command, parameters);
		}

		public int ExecuteNonQuery(string command) {
			return SqlHelper.ExecuteNonQuery(ConnectionString,
			                                 CommandType.Text, command);
		}

		public int ExecuteNonQuery(string command, object[] parameters) {
			return SqlHelper.ExecuteNonQuery(ConnectionString,
			                                 command, parameters);
		}

		#endregion

		#region Query Helpers

		/// <summary>
		/// for generating a page query
		/// </summary>
		/// <param name="queryingFields">the fields' names </param>
		/// <param name="pageIndex">starts from 1</param>
		/// <param name="pageSize"></param>
		/// <param name="fromTables"> the tables names after "FROM" </param>
		/// <param name="orderByTable"></param>
		/// <param name="orderByColumn"></param>
		/// <param name="condition">the condition after WHERE</param>
		/// <param name="orderColumIsUnique">if the orderColumn is unique (different algorithm will be applied) </param>
		/// <param name="ASC">TRUE if order by ASC </param>
		/// <returns></returns>
		public static string BuildPageQuery(string queryingFields,
		                                    int pageIndex,
		                                    int pageSize,
		                                    string fromTables,
		                                    string orderByTable,
		                                    string orderByColumn,
		                                    string condition,
		                                    bool orderColumIsUnique,
		                                    bool ASC) {
			string orderWay = ASC ? " ASC" : " DESC";
			string reverseWay = !ASC ? " ASC" : " DESC";

			if (!orderColumIsUnique)
				return "SELECT  * FROM "
				       + "( SELECT TOP  " + pageSize.ToString() + " * FROM "
				       + "( SELECT TOP  " + (pageSize*pageIndex).ToString() + " " + queryingFields
				       + " FROM " + fromTables
				       + " " + (condition != "" ? " WHERE " + condition : "")
				       + " ORDER BY " + orderByTable + "." + orderByColumn + orderWay
				       + ") As Temp1 ORDER BY  Temp1." + orderByColumn + reverseWay
				       + ") As Temp2 ORDER BY  Temp2." + orderByColumn + orderWay;

			else {
				string NumBefore = Convert.ToString((pageIndex - 1)*pageSize);

				string query = "SELECT  TOP " + pageSize.ToString() + " " + queryingFields + " FROM " + fromTables;
				string conditionQuery = "";
				if (pageIndex > 1)
					conditionQuery = orderByTable + "." + orderByColumn +
					                 " > ( SELECT Max(" + orderByColumn + ") from ( SELECT TOP " + NumBefore + " " +
					                 orderByTable + "." + orderByColumn + "  FROM " + fromTables +
					                 (condition != "" ? " WHERE " + condition : "") +
					                 " ORDER BY " + orderByTable + "." + orderByColumn + orderWay + " ) As Temp1 ) "
					                 + (condition != "" ? " AND " : "");

				conditionQuery += condition;

				query += conditionQuery != "" ? " WHERE " + conditionQuery : "";

				query += " ORDER BY " + orderByTable + "." + orderByColumn + orderWay;

				return query;
			}
		}

		#endregion
	}
}