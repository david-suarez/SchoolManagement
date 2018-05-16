using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using IDataReader = SchoolManagement.Interfaces.IDataReader;

namespace SchoolManagement.DataReader
{
    public class CsvDataReader : IDataReader
    {
        private readonly string csvFilePath;

        private readonly log4net.ILog Log =
           log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public CsvDataReader() { }

        public CsvDataReader(string textFilePath)
        {
            this.csvFilePath = textFilePath;

        }

        public DataTable ReadData()
        {
            DataTable csvData = new DataTable();
            string Fulltext;
            try
            {
                using (StreamReader sr = new StreamReader(this.csvFilePath))
                {
                    while (!sr.EndOfStream)
                    {
                        Fulltext = sr.ReadToEnd().ToString(); //read full file text
                        var rows = Fulltext.Split('\n'); //split full file text into rows  
                        for (int i = 0; i < rows.Count(); i++)
                        {
                            var rowValues = rows[i].Split(','); //split each row with comma to get individual values  
                            {
                                if (i == 0)
                                {
                                    for (int j = 0; j < rowValues.Count(); j++)
                                    {
                                        var rowValue = Regex.Replace(rowValues[j].ToString(), @"[\r\n\t ]+", " ");
                                        csvData.Columns.Add(rowValue); //add headers  
                                    }
                                }
                                else
                                {
                                    var dataRow = csvData.NewRow();
                                    for (int k = 0; k < rowValues.Count(); k++)
                                    {
                                        var rowValue = Regex.Replace(rowValues[k].ToString(), @"[\r\n\t ]+", " ");
                                        dataRow[k] =  rowValue;
                                    }
                                    csvData.Rows.Add(dataRow); //add other rows  
                                }
                            }
                        }
                    }
                }
            }
            catch(FileNotFoundException ex)
            {
                Log.Error(ex.Message);
                throw;
            }
            catch(Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
            return csvData;
        }
    }
        
}
