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
            var csvData = new DataTable();
            var fulltext = string.Empty;
            try
            {
                using (var sr = new StreamReader(this.csvFilePath))
                {
                    while (!sr.EndOfStream)
                    {
                        fulltext = sr.ReadToEnd(); 
                        var rows = fulltext.Split('\n');  
                        for (var i = 0; i < rows.Count(); i++)
                        {
                            var rowValues = rows[i].Split(','); 
                            {
                                if (i == 0)
                                {
                                    for (var j = 0; j < rowValues.Count(); j++)
                                    {
                                        var rowValue = Regex.Replace(rowValues[j], @"[\r\n\t ]+", " ");
                                        csvData.Columns.Add(rowValue);
                                    }
                                }
                                else
                                {
                                    var dataRow = csvData.NewRow();
                                    for (var k = 0; k < rowValues.Count(); k++)
                                    {
                                        var rowValue = Regex.Replace(rowValues[k], @"[\r\n\t ]+", " ");
                                        dataRow[k] = rowValue;
                                    }
                                    csvData.Rows.Add(dataRow); 
                                }
                            }
                        }
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
                this.Log.Error(ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                this.Log.Error(ex.Message);
                throw;
            }

            return csvData;
        }
    }
        
}
