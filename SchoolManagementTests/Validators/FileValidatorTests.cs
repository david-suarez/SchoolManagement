using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SchoolManagement.Validators;

namespace SchoolManagementTests.Validators
{
    [TestClass]
    public class FileValidatorTests
    {
        [TestMethod]
        public void Validate_CorrectFilePath_ValidateCorrectly()
        {
            var filePath = "..\\..\\..\\Resources\\data.csv";
            var fileValidator = new FileValidator(filePath);

            Assert.IsTrue(fileValidator.Validate());
        }

        [DataRow("..\\..\\..\\Resources\\NoFile.csv")]
        [DataTestMethod]
        public void Validate_IncorrectFilePath_ThrowException(string filePath)
        {
            var fileValidator = new FileValidator(filePath);

            Assert.ThrowsException<FileNotFoundException>(() => fileValidator.Validate());
        }
    }
}
