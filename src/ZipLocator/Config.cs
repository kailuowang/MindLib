using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;

namespace MindHarbor.ZipLocator
{
    public class Config
    {
        internal string ReadIncompleteSqlScriptWhenInitializeFromResourceFile()
        {
            foreach (string resource in this.GetType().Assembly.GetManifestResourceNames())
            {
                if (resource.EndsWith("GenerateWhereInitialize_Create.sql"))
                {
                    StreamReader scriptFileStreamReader =
                        new StreamReader(GetType().Assembly.GetManifestResourceStream(resource));

                    return scriptFileStreamReader.ReadToEnd();
                }
            }
            return string.Empty;
        }

        internal DataSet ReadZipCodesFromResourceFile()
        {
            foreach (string resource in this.GetType().Assembly.GetManifestResourceNames())
            {
                if (resource.EndsWith("ZipCode.csv"))
                {
                    StreamReader sr =
                        new StreamReader(GetType().Assembly.GetManifestResourceStream(resource));
                    string s = sr.ReadToEnd();

                    string fullCSVFileName = Path.Combine(System.Environment.CurrentDirectory, "ZipCode.csv");
                    using(StreamWriter sw=File.CreateText(fullCSVFileName))
                    {
                        sw.Write(s);
                    }

                    DataSet ds = CSVReaderUtil.ReadDSFromCSV(fullCSVFileName);
                    File.Delete(fullCSVFileName);
                    return ds;
                    
                }
            }
            return null;
        }
    }
}
