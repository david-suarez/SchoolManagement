using System;
using System.Linq.Expressions;

using SchoolManagement.Interfaces;
using SchoolManagement.Models;

using Dynamic = System.Linq.Dynamic;

namespace SchoolManagement.Mapping
{
    public class StudentQueryGenerator : IQueryGenerator<Student>
    {
        public Expression<Func<Student, bool>> Generate(string target)
        {
            var conditions = target.Split(' ');
            var expressionString = string.Empty;
            for (var index = 0; index < conditions.Length; index++)
            {
                var condition = conditions[index];
                var operators = condition.Split('=');
                if (operators.Length > 1)
                {
                    expressionString += this.GenerateConditionString(operators[0], operators[1]);
                    if (index + 1 < conditions.Length)
                    {
                        expressionString += " && ";
                    }
                }
            }
            //return s => s.Name.Equals("Gianine", StringComparison.InvariantCultureIgnoreCase) && s.Type.Equals("Kinder", StringComparison.InvariantCultureIgnoreCase);
            return Dynamic.DynamicExpression.ParseLambda<Student, bool>(expressionString);
        }

        private string GenerateConditionString(string property, string value)
        {
            if (property.Equals("LastUpdate", StringComparison.InvariantCultureIgnoreCase))
            {
                return $"LastUpdate.ToString(\"yyyyMMddHHmmss\")" +
                       $".Equals(\"{value}\")";
            }
            else
            {
                return $"{property}.Equals(\"{value}\")";
            }
        }
    }
}
