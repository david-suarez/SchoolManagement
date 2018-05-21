using System;
using System.IO;

using SchoolManagement.DataReader;
using SchoolManagement.Interfaces;
using SchoolManagement.Resourses.Enums;
using SchoolManagement.Validators;

namespace SchoolManagement.Factory
{
    public class DataReaderFactory : IFactory<IDataReader>
    {
        private readonly string FilePath;

        private readonly IValidator<string> compositeValidator;

        public DataReaderFactory(string filePath, IValidator<string> compositeValidator)
        {
            Guard.ArgumentIsNotNull(compositeValidator, nameof(compositeValidator));

            this.FilePath = filePath;
            this.compositeValidator = compositeValidator;
        }

        public IDataReader Get()
        {
            var fileExtension = this.GetFileExtension();
            switch(fileExtension)
            {
                case FileExtensions.CSV:
                    return new CsvDataReader(this.FilePath);
                case FileExtensions.TXT:
                case FileExtensions.XML:
                case FileExtensions.XLSX:
                    throw new NotSupportedException();
            }
            throw new NotSupportedException();
        }

        private FileExtensions GetFileExtension()
        {
            this.compositeValidator.Validate(this.FilePath);
            var extension = Path.GetExtension(this.FilePath);
            if (extension.Equals(".csv"))
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

            return extension.Equals(".xlsx") ? FileExtensions.XLSX : FileExtensions.NonSuported;
        } 
    }
}