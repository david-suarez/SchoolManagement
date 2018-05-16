using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var pathValidator = new PathValidator(filePath);

            Assert.IsTrue(pathValidator.Validate());
        }

        [DataRow("cda:\\")]
        [DataRow("cda:\\SomeDirectory\\SomeFile.txt")]
        [DataTestMethod]
        public void Validate_IncorrectFilePath_ThrowException(string filePath)
        {
            
            var pathValidator = new PathValidator(filePath);

            Assert.ThrowsException<InvalidPathException>(() => pathValidator.Validate());
        }
    }
}
