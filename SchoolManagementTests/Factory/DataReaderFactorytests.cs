using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SchoolManagement.DataReader;
using SchoolManagement.Factory;
using SchoolManagement.Interfaces;
using SchoolManagement.Validators;

namespace SchoolManagementTests.Factory
{
    [TestClass]
    public class DataReaderFactoryTests
    {
        [TestMethod]
        public void Get_SendCsvFilePath_ReturnCsvDataReaderInstance()
        {
            var filePath = "..\\..\\..\\Resources\\data.csv";
            var factory = new DataReaderFactory(filePath, new FileValidator(filePath));
            var reader = factory.Get();
            Assert.AreEqual(typeof(CsvDataReader), reader.GetType());
        }

        [DataRow("..\\..\\..\\Resources\\data.xlsx")]
        [DataRow("..\\..\\..\\Resources\\data.xml")]
        [DataRow("..\\..\\..\\Resources\\data.txt")]
        [DataTestMethod]
        public void Get_SendNonCsvFilePath_ThrowException(string filePath)
        {
            var factory = new DataReaderFactory(filePath, new FileValidator(filePath));

            Assert.ThrowsException<NotSupportedException>(() => factory.Get());
        }
    }
}
