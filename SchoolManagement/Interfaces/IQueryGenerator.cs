using System;
using System.Linq.Expressions;

namespace SchoolManagement.Interfaces
{
    public interface IQueryGenerator<T>
    {
        Expression<Func<T, bool>> Generate(string target);
    }
}
