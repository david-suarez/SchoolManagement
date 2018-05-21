using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity.Core;

using SchoolManagement.Interfaces;
using SchoolManagement.Models;

namespace SchoolManagement.Mapping
{
    public class MappingStudent : IMap<DataTable, IEnumerable<Student>>
    {
        public IEnumerable<Student> Map(DataTable data)
        {
            var propertiesNames = typeof(Student).GetProperties().Select(p => p.Name.ToUpper());
            var currentRows = data.Select(
                null, null, DataViewRowState.CurrentRows);
            var listResult = new List<Student>();
            if (!propertiesNames.Any(n => data.Columns.Contains(n)))
            {
                throw new MappingException("The data cannot be mapped to an student.");
            }

            foreach (var row in currentRows)
            {
                var student = new Student();
                foreach (DataColumn column in data.Columns)
                {
                    var columnName = column.ColumnName.Trim();
                    if (columnName == "NAME")
                    {
                        student.Name = (string)row[column];
                    }

                    if (columnName == "TYPE")
                    {
                        student.Type = (string)row[column];
                    }

                    if (columnName == "GENDER")
                    {
                        student.Gender = (string)row[column];
                    }

                    if (columnName == "LASTUPDATE" || columnName == "TIMESTAMP")
                    {
                        student.LastUpdate = DateTime.ParseExact(
                            ((string)row[column]).Trim(),
                            "yyyyMMddHHmmss",
                            System.Globalization.CultureInfo.InvariantCulture);
                    }
                }

                listResult.Add(student);
            } 

            return listResult;
        }
    }
}
