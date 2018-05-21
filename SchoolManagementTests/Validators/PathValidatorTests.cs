using Microsoft.VisualStudio.TestTools.UnitTesting;

using SchoolManagement.Exceptions;
using SchoolManagement.Validators;

namespace SchoolManagementTests.Validators
{
    [TestClass]
    public class PathValidatorTests
    {
        [TestMethod]
        public void Validate_CorrectFilePath_ValidateCorrectly()
        {
            var filePath = "c:\\SomeDirectory\\SomeFile.txt";
            var pathValidator = new PathValidator();

            Assert.IsTrue(pathValidator.Validate(filePath));
        }

        [DataRow("cda:\\")]
        [DataRow("cda:\\SomeDirectory\\SomeFile.txt")]
        [DataTestMethod]
        public void Validate_IncorrectFilePath_ThrowException(string filePath)
        {
            
            var pathValidator = new PathValidator();

            Assert.ThrowsException<InvalidPathException>(() => pathValidator.Validate(filePath));
        }
    }
}
