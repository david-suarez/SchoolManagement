using System.IO;

using SchoolManagement.Interfaces;

namespace SchoolManagement.Validators
{
    public class FileValidator : IValidator<string>
    {
        public bool Validate(string filePath)
        {
            if (File.Exists(filePath))
            {
                return true;
            }

            var fileName = Path.GetFileName(filePath);
            var path = Path.GetDirectoryName(filePath);
            throw new FileNotFoundException($"The {fileName} does not exist in the path {path}", fileName);
        }
    }
}
