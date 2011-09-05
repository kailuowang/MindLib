using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace MindHarbor.ZipLocator
{
    public class ZipSearch
    {
        private readonly string sp_ZipLocator = "sp_Mindharbor_ZipLocator";
        private readonly string tblZipCode = "Mindharbor_ZIPCodes";
        private readonly string tableName;
        private readonly string zipCodeColumnName;
        private readonly string connectionString;
        private bool initialized;
        private static readonly object lockObj = new object();
        private  Thread t;

        public bool Initialized {
            get { return initialized; }
        }

        public string MilesColumnName {
            get { return "Distance"; }
        }

        public void AsynchronizedInitialization() {
            t = new Thread(EnsureDBElements);
            t.Start();
        }

        public ZipSearch(string spSearchName, string connectionString) : this(null, null, connectionString) {
            sp_ZipLocator = spSearchName;
        }

        /// <summary>
        ///  
        /// </summary>
        ///<param name="tableName">Name of the Target Table</param>
        /// <param name="zipCodeColumnName">Zipcode column name of the target table</param>
        /// <param name="ZipCoordinateTableName"></param>
        /// <param name="connectionString">Sql Connection</param>
        public ZipSearch(string ZipCoordinateTableName, string tableName,
                         string zipCodeColumnName, string connectionString)
            : this(tableName, zipCodeColumnName, connectionString) {
            this.tblZipCode = ZipCoordinateTableName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName">Name of the Target Table</param>
        /// <param name="zipCodeColumnName">Zipcode column name of the target table</param>
        /// <param name="connectionString">Sql Connection</param>
        public ZipSearch(string tableName,
                         string zipCodeColumnName, string connectionString) {
            this.tableName = tableName;
            this.zipCodeColumnName = zipCodeColumnName;
            this.connectionString = connectionString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="zipCode"></param>
        /// <param name="miles"></param>
        /// <param name="whereClause"></param>
        /// <returns></returns>
        public DataTable Search(string zipCode, decimal miles, string whereClause) {
            SqlParameter paramZipCode = new SqlParameter("@ZIPCode", SqlDbType.Char, 5);
            paramZipCode.Value = zipCode;
            SqlParameter paramMiles = new SqlParameter("@Miles", SqlDbType.Float);
            paramMiles.Value = miles;

            DataTable dt;
            if (string.IsNullOrEmpty(zipCodeColumnName) && string.IsNullOrEmpty(tableName))
                dt =
                    SqlHelper.ExcuteDataTable(connectionString, CommandType.StoredProcedure, sp_ZipLocator, paramZipCode,
                                              paramMiles);
            else {
                EnsureDBElements();
                string query = GetSqlScriptWhenSearch(whereClause, zipCode, miles);

                dt =
                    SqlHelper.ExcuteDataTable(connectionString, CommandType.Text, query);
            }

            return dt;
        }

        public DataTable Search(string zipCode, decimal miles) {
            return Search(zipCode, miles, null);
        }

        internal void EnsureDBElements() {
            if (initialized)
                return;
            lock (lockObj) {
                ExcuteSqlScript();
                InsertDataToZipCodeTable();
                initialized = true;
            }
        }

        private void ExcuteSqlScript() {
            string sqlScript = GetSqlScriptWhenInitialize();
            string delimiter = "^GO";
            string[] sqlQueries =
                Regex.Split(sqlScript, delimiter + @"\s*\n", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            sqlQueries[sqlQueries.Length - 1] =
                Regex.Replace(sqlQueries[sqlQueries.Length - 1], delimiter, String.Empty,
                              RegexOptions.IgnoreCase | RegexOptions.Multiline);

            foreach (string sqlQuery in sqlQueries)
                SqlHelper.ExecuteNonQuery(connectionString, CommandType.Text, sqlQuery);
        }

        private void InsertDataToZipCodeTable() {
            string query = string.Format("select  count(*) from {0}", tblZipCode);
            int count = (int) SqlHelper.ExecuteScalar(connectionString, CommandType.Text, query);
            if (count == 0) {
                DataSet ds = new Config().ReadZipCodesFromResourceFile();
                SqlHelper.BatchExecuteNonQuery(connectionString, CommandType.StoredProcedure, tblZipCode, ds, null);
            }
        }

        private string GetSqlScriptWhenInitialize() {
            string CompleteScript = new Config().ReadIncompleteSqlScriptWhenInitializeFromResourceFile();
            CompleteScript = CompleteScript.Replace("**tblZipCode**", tblZipCode);
            CompleteScript =
                CompleteScript.Replace("**sp_InsertZipCodeDataToTable**",
                                       SqlHelper.sp_InsertZipCodeDataToTable);

            return CompleteScript;
        }

        private  string GetSqlScriptWhenSearch(string whereClause, string zipCode, decimal miles) {
            whereClause = string.IsNullOrEmpty(whereClause) ? string.Empty : string.Format(" AND {0}", whereClause);
            string sql =
                "SELECT	t.* ,dbo.Mindharbor_DistanceAssistant(z.Latitude,z.Longitude,r.Latitude,r.Longitude) As Distance";
            sql +=
                string.Format(" FROM	{0} z,	dbo.Mindharbor_RadiusAssistant('{1}', {2}) r, {3} t ", tblZipCode, zipCode,
                              miles, tableName);
            sql += string.Format(" WHERE	1=1 {0} AND z.Latitude BETWEEN r.MinLat AND r.MaxLat", whereClause);
            sql += " AND z.Longitude BETWEEN r.MinLong AND r.MaxLong	AND ";
            sql +=
                string.Format(
                    " dbo.Mindharbor_DistanceAssistant(z.Latitude,z.Longitude,r.Latitude,r.Longitude) <= {0}", miles);
            sql += string.Format(" AND t.{0} = z.ZIPCode", zipCodeColumnName);

            sql += " ORDER BY Distance, ZIPCode";

            return sql;
        }


    }
}
