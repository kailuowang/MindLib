using System.Collections.Specialized;
using System.Data;
using System.Data.Odbc;
using System.IO;

namespace MindHarbor.ZipLocator{
    /// <summary>
    /// Summary description for CVSImporter.
    /// </summary>
    public class CSVReaderUtil {
        public string CSVFileLocation {
            get { return _CSVFileLocation; }
        }

        private string _CSVFileLocation;

        public CSVReaderUtil(string CSVFileLocation) {
            _CSVFileLocation = CSVFileLocation;
        }

        private DataSet Read(string fileTableName) {
            writeSchema(fileTableName);

            try {
                string conn_excel_str = "Driver={Microsoft Text Driver (*.txt; *.csv)};Dbq=" + _CSVFileLocation +
                                        ";MaxScanRows=0;";
                //Server.MapPath(".//" + csv_store_read) //Path mapped to csv file temp stored on the server
                string sql_select;
                OdbcConnection obj_oledb_con;
                OdbcDataAdapter obj_oledb_da;
                //Create connection to CSV file
                obj_oledb_con = new OdbcConnection(conn_excel_str);
                //Open the connection 
                obj_oledb_con.Open();

                //Fetch records from CSV				
                sql_select = "select * from [" + fileTableName + "]";

                obj_oledb_da = new OdbcDataAdapter(sql_select, obj_oledb_con);

                //Fill dataset with the records from CSV file
                DataSet ds = new DataSet();
                obj_oledb_da.Fill(ds);
                //Close Connection to CSV file
                obj_oledb_con.Close();

                return ds;
            }
            finally {
                deleteSchema();
            }
        }

        public static DataSet ReadDSFromCSV(string fullCSVFileName) {
            FileInfo fi = new FileInfo(fullCSVFileName);
            CSVReaderUtil cu = new CSVReaderUtil(fi.DirectoryName);
            return cu.Read(fi.Name);
        }

        private void writeSchema(string fileTableName) {
            FileStream fsOutput = new FileStream(_CSVFileLocation + "\\schema.ini", FileMode.Create, FileAccess.Write);
            StreamWriter srOutput = new StreamWriter(fsOutput);
            string s1, s2, s3, s4, s5;
            s1 = "[" + fileTableName + "]";
            s2 = "ColNameHeader=true";
            s3 = "Format=CSVDelimited";
            s4 = "MaxScanRows=0";
            s5 = "CharacterSet=OEM";
            srOutput.WriteLine(s1.ToString() + '\n' + s2.ToString() + '\n' + s3.ToString() + '\n' + s4.ToString() + '\n' +
                               s5.ToString());
            srOutput.Close();
            fsOutput.Close();
        }

        private void deleteSchema() {
            FileInfo fi = new FileInfo(_CSVFileLocation + "\\schema.ini");
            fi.Delete();
        }
    }
}