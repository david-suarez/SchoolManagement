using System.IO;
using System.Text.RegularExpressions;
using SchoolManagement.Exceptions;
using SchoolManagement.Interfaces;

namespace SchoolManagement.Validators
{
    public class PathValidator : IValidator
    {
        public string PathToValidate { get; set; }

        public PathValidator() { }

        public PathValidator(string pathToValidate)
        {
            this.PathToValidate = pathToValidate;
        }
            
        public bool Validate()
        {
            var driveCheck = new Regex(@"^[a-zA-Z]:\\$");
            if (this.PathToValidate.Length < 3 || !driveCheck.IsMatch(this.PathToValidate.Substring(0, 3)))
            {
                throw new InvalidPathException("Invalid path");
            }
            var strTheseAreInvalidFileNameChars = new string(Path.GetInvalidPathChars());
            strTheseAreInvalidFileNameChars += @":/?*" + "\"";
            var containsABadCharacter = new Regex("[" + Regex.Escape(strTheseAreInvalidFileNameChars) + "]");
            if (containsABadCharacter.IsMatch(PathToValidate.Substring(3, PathToValidate.Length - 3)))
            {
                throw new InvalidPathException("Invalid path");
            }
           
            return true;
        }
    }
}
