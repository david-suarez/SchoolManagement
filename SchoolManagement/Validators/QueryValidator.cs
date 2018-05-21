using System.Text.RegularExpressions;

using SchoolManagement.Exceptions;
using SchoolManagement.Interfaces;

namespace SchoolManagement.Validators
{
    public class QueryValidator : IValidator<string>
    {
        public bool Validate(string query)
        {
            var patern = @"^[A-Za-z]*=[A-za-z]*((\s[A-Za-z]*=[A-Za-z]*)?)+(\s)?$";
            var queryMatch = Regex.IsMatch(query, patern);
            if (!queryMatch)
            {
                throw new QueryNotMatchException("The query does not have the expeted format.");
            }
            

            return true;
        }
    }
}
