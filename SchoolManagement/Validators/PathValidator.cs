using System.IO;
using System.Text.RegularExpressions;
using SchoolManagement.Exceptions;
using SchoolManagement.Interfaces;

namespace SchoolManagement.Validators
{
    public class PathValidator : IValidator<string>
    {
            
        public bool Validate(string pathToValidate)
        {
            var driveCheck = new Regex(@"^[a-zA-Z]:\\$");
            if (pathToValidate.Length < 3 || !driveCheck.IsMatch(pathToValidate.Substring(0, 3)))
            {
                throw new InvalidPathException("Invalid path");
            }
            var strTheseAreInvalidFileNameChars = new string(Path.GetInvalidPathChars());
            strTheseAreInvalidFileNameChars += @":/?*" + "\"";
            var containsABadCharacter = new Regex("[" + Regex.Escape(strTheseAreInvalidFileNameChars) + "]");
            if (containsABadCharacter.IsMatch(pathToValidate.Substring(3, pathToValidate.Length - 3)))
            {
                throw new InvalidPathException("Invalid path");
            }
           
            return true;
        }
    }
}
