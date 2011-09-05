using System;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Text;
using LumenWorks.Framework.IO.Csv;

namespace MindHarbor.GenClassLib.MiscUtil{
    /// <summary>
    /// Summary description for CVSImporter.
    /// </summary>
    public class CSVReaderUtil{
        #region DelimitedMode enum

        public enum DelimitedMode{
            CSV,
            Tab,
            Auto
        }

        #endregion

        private readonly FileInfo fi;
        private string filterExpression;

        private int maximumNumOfRows;
        private DelimitedMode mode;

        public CSVReaderUtil(string fullFilePath, DelimitedMode m){
            fi = new FileInfo(fullFilePath);
            GetRealMode(m);
        }

        public int MaximumNumOfRows{
            get { return maximumNumOfRows; }
            set { maximumNumOfRows = value; }
        }

        public string FilterExpression{
            get { return filterExpression; }
            set { filterExpression = value; }
        }

        public DataSet Read(){
            return Read(true);
        }

        public DataSet Read(bool hasHeader, Encoding encoding){
            DataTable dt = new DataTable("Data");
            int count = 0;
            using (
                CsvReader csv =
                    new CsvReader(new StreamReader(fi.FullName, encoding), hasHeader,
                                  mode == DelimitedMode.Tab ? '\t' : ',')){
                int fieldCount = csv.FieldCount;
                if (hasHeader){
                    string[] headers = csv.GetFieldHeaders();
                    foreach (string s in headers)
                        dt.Columns.Add(s);
                }
                else{
                    for (int i = 0; i < fieldCount; i++){
                        dt.Columns.Add(i.ToString());
                    }
                }


                while (csv.ReadNextRecord()){
                    DataRow row = dt.NewRow();
                    for (int i = 0; i < fieldCount; i++)
                        row[i] = csv[i];
                    if (FilterRow(row))
                        dt.Rows.Add(row);

                    if (MaximumNumOfRows > 0 && count > MaximumNumOfRows)
                        break;
                    count++;
                }
            }
            DataSet ds = new DataSet("dataSet");
            ds.Tables.Add(dt);
            return ds;
        }

        public DataSet Read(bool hasHeader){
            return Read(hasHeader, Encoding.Default);
        }

        private bool FilterRow(DataRow row){
            if (string.IsNullOrEmpty(filterExpression))
                return true;
            else{
                foreach (
                    string conditionExpress in
                        filterExpression.Split(new string[] {" and ", " AND "}, StringSplitOptions.RemoveEmptyEntries))
                    if (!TestCondition(row, conditionExpress))
                        return false;
                return true;
            }
        }

        private bool TestCondition(DataRow row, string conditionExpression){
            string[] exps = conditionExpression.Split('=');
            string columnName = exps[0].Trim();
            string value = exps[1].Trim();
            return row[columnName].ToString().Trim() == value;
        }

        //private DataSet ReadUsingTextDriver(string fileTableName) {
        //    writeSchema(fileTableName);
        //    try {
        //        string conn_excel_str = "Driver={Microsoft Text Driver (*.txt; *.csv)};Dbq=" + _CSVFileLocation +
        //                                ";MaxScanRows=0;";
        //        //Server.MapPath(".//" + csv_store_read) //Path mapped to csv file temp stored on the server
        //        string sql_select;
        //        OdbcConnection obj_oledb_con;
        //        OdbcDataAdapter obj_oledb_da;
        //        //Create connection to CSV file
        //        obj_oledb_con = new OdbcConnection(conn_excel_str);
        //        //Open the connection 
        //        obj_oledb_con.Open();
        //        //Fetch records from CSV				
        //        sql_select = "select * from [" + fileTableName + "]";
        //        obj_oledb_da = new OdbcDataAdapter(sql_select, obj_oledb_con);
        //        //Fill dataset with the records from CSV file
        //        DataSet ds = new DataSet();
        //        obj_oledb_da.Fill(ds);
        //        //Close Connection to CSV file
        //        obj_oledb_con.Close();
        //        return ds;
        //    }
        //    finally {
        //        deleteSchema();
        //    }
        //}

        public static DataSet ReadDSFromCSV(string fullCSVFileName){
            return ReadDSFromCSV(fullCSVFileName,Encoding.Default);
        }
        public static DataSet ReadDSFromCSV(string fullCSVFileName,Encoding encoding)
        {
            return ReadDS(fullCSVFileName, DelimitedMode.CSV, encoding);
        }
        public static DataSet ReadDSFromTabTxt(string fullTxtFileName){
            return ReadDSFromTabTxt(fullTxtFileName,Encoding.Default);
        }

        public static DataSet ReadDSFromTabTxt(string fullTxtFileName,Encoding encoding)
        {
            return ReadDS(fullTxtFileName, DelimitedMode.Tab,encoding);
        }

        public static DataSet ReadDS(string fullTxtFileName)
        {
            return ReadDS(fullTxtFileName, DelimitedMode.Auto);
        }
        public static DataSet ReadDS(string fullTxtFileName,Encoding encoding){
            return ReadDS(fullTxtFileName, DelimitedMode.Auto, encoding);
        }

        private static DataSet ReadDS(string filePath, DelimitedMode mode){
            return ReadDS(filePath, mode, Encoding.Default);
        }

        private static DataSet ReadDS(string filePath, DelimitedMode mode,Encoding encoding)
        {
            CSVReaderUtil u = new CSVReaderUtil(filePath, mode);
            return u.Read(true,encoding);
        }

        //private void writeSchema(string fileTableName) {
        //    FileStream fsOutput = new FileStream(_CSVFileLocation + "\\schema.ini", FileMode.Create, FileAccess.Write);
        //    StreamWriter srOutput = new StreamWriter(fsOutput);
        //    string s1, s2, s3, s4, s5;
        //    s1 = "[" + fileTableName + "]";
        //    s2 = "ColNameHeader=true";
        //    s3 = string.Format("Format={0}Delimited", GetRealMode(fileTableName));
        //    s4 = "MaxScanRows=0";
        //    s5 = "CharacterSet=OEM";
        //    srOutput.WriteLine(s1.ToString() + '\n' + s2.ToString() + '\n' + s3.ToString() + '\n' + s4.ToString() + '\n' +
        //                       s5.ToString());
        //    srOutput.Close();
        //    fsOutput.Close();
        //}

        private void GetRealMode(DelimitedMode m){
            mode = m;
            if (m == DelimitedMode.Auto)
                if (fi.Extension.ToLower() == ".csv")
                    mode = DelimitedMode.CSV;
                else if (fi.Extension.ToLower() == ".txt")
                    mode = DelimitedMode.Tab;
                else
                    throw new NotSupportedException(fi.Extension + " is not a known format for auto delimated Mode");
        }

        /*private string GetFilePath(string fileName) {
        //    return _CSVFileLocation + "\\" + fileName;
        //}

        //private void deleteSchema() {
        //    FileInfo fi = new FileInfo(_CSVFileLocation + "\\schema.ini");
        //    fi.Delete();
     }  */

        /// <summary>
        /// Try to guess all the columns in a DataColumnCollection for a type's public properties
        /// </summary>
        /// <param name="columns"></param>
        /// <returns>return a NameValueCollection where name is the type's property name with a value of the guessed columnName</returns>
        public static NameValueCollection GuessMatchSettingsFromColumns(DataColumnCollection columns,
                                                                        NameValueCollection matchSetting){
            foreach (string propertyName in matchSetting.AllKeys){
                DataColumn guessedCol = GuessColumnByName(propertyName, columns);
                if (guessedCol != null)
                    matchSetting[propertyName] = GuessColumnByName(propertyName, columns).ColumnName;
                else
                    matchSetting[propertyName] = "";
            }
            return matchSetting;
        }

        public static DataColumn GuessColumnByName(string name, DataColumnCollection columns){
            foreach (DataColumn column in columns)
                if (name.Trim().ToLower().IndexOf(column.ColumnName.ToLower()) > -1
                    || column.ColumnName.ToLower().IndexOf(name.Trim().ToLower()) > -1)
                    return column;
            return null;
        }
    }
}