using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SchoolManagement.DataReader;

namespace SchoolManagementTests
{
    [TestClass]
    public class CsvDataReaderTests
    {
        [TestMethod]
        public void ReadData_SendCorrectFilePath_ShouldLoadData()
        {
            var filePath = "..\\..\\..\\Resources\\data.csv";
            var expectedColumns = 4;
            var expectedRows = 10;
            var dataReader = new CsvDataReader(filePath);
            var loadedData = dataReader.ReadData();

            Assert.AreEqual(expectedColumns, loadedData.Columns.Count);
            Assert.AreEqual(expectedRows, loadedData.Rows.Count);
        }

        [TestMethod]
        public void ReadData_SendIncorrectFilePath_ThrowExcepion()
        {
            var filePath = "..\\..\\..\\Resources\\NonExistFile.csv";
            var dataReader = new CsvDataReader(filePath);

            Assert.ThrowsException<FileNotFoundException>(() => dataReader.ReadData());
        }
    }
}
