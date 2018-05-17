using System.IO;

using SchoolManagement.Interfaces;

namespace SchoolManagement.Validators
{
    public class FileValidator : IValidator
    {
        private readonly string filePath;

        public FileValidator(string filePath)
        {
            this.filePath = filePath;
        }

        public bool Validate()
        {
            if (File.Exists(this.filePath))
            {
                return true;
            }

            var fileName = Path.GetFileName(this.filePath);
            var path = Path.GetDirectoryName(this.filePath);
            throw new FileNotFoundException($"The {fileName} does not exist in the path {path}", fileName);
        }
    }
}
