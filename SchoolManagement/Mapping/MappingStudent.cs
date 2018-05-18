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
                foreach (DataColumn column in data.Columns)
                {
                    var student = new Student();
                    if (column.ColumnName == "NAME")
                    {
                        student.Name = (string)row[column];
                    }

                    if (column.ColumnName == "TYPE")
                    {
                        student.Type = (string)row[column];
                    }

                    if (column.ColumnName == "GENDER")
                    {
                        student.Gender = (string)row[column];
                    }

                    if (column.ColumnName == "LASTUPDATE")
                    {
                        student.LastUpdate = DateTime.ParseExact(
                            (string)row[column],
                            "yyyyMMddHHmmss",
                            System.Globalization.CultureInfo.InvariantCulture);
                    }

                    listResult.Add(student);
                }
            } 

            return listResult;
        }
    }
}
