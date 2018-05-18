using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SchoolManagement.Interfaces
{
    public interface IRepository<T> where T : class, IEntity
    {
        IEnumerable<T> GetAll();

        IEnumerable<T> Find(Func<T, bool> where);

        T Delete(T entity);

        void DeleteAll();

        T Add(T entity);
        
        T Update(T data);
    }
}
