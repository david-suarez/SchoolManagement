using System;
using System.Collections.Generic;
using System.IO;

using SchoolManagement.DataReader;
using SchoolManagement.Interfaces;
using SchoolManagement.Resourses.Enums;
using SchoolManagement.Validators;

namespace SchoolManagement.Factory
{
    public class DataReaderFactory : IFactory<IDataReader>
    {
        private readonly string filePath;

        private readonly IValidator compositeValidator;

        public DataReaderFactory(string filePath, IValidator compositeValidator)
        {
            Guard.ArgumentIsNotNull(compositeValidator, nameof(compositeValidator));

            this.filePath = filePath;
            this.compositeValidator = compositeValidator;
        }

        public IDataReader Get()
        {
            var fileExtension = this.GetFileExtension(filePath);
            switch(fileExtension)
            {
                case FileExtensions.CSV:
                    return new CsvDataReader(filePath);
                case FileExtensions.TXT:
                case FileExtensions.XML:
                case FileExtensions.XLSX:
                    throw new NotSupportedException();
            }
            throw new NotSupportedException();
        }

        private FileExtensions GetFileExtension(string filePath)
        {
            this.compositeValidator.Validate();
            var extension = Path.GetExtension(filePath);
            if(extension.Equals(".csv"))
            {
                return FileExtensions.CSV;
            }
            if (extension.Equals(".xml"))
            {
                return FileExtensions.XML;
            }
            if (extension.Equals(".txt"))
            {
                return FileExtensions.TXT;
            }
            if (extension.Equals(".xlsx"))
            {
                return FileExtensions.XLSX;
            }
            return FileExtensions.NonSuported;
        } 
    }
}