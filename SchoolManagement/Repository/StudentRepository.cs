using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

using SchoolManagement.Interfaces;
using SchoolManagement.Validators;

namespace SchoolManagement.Repository
{
    public class Repository<T> : IRepository<T> where T : class, IEntity, IEquatable<T>
    {

        private  IEnumerable<T> objects;

        public Repository(DataTable data, IMap<DataTable, IEnumerable<T>> mappingObject)
        {
            Guard.ArgumentIsNotNull(data, nameof(data));
            Guard.ArgumentIsNotNull(mappingObject, nameof(mappingObject));
            
            this.objects = mappingObject.Map(data);
        }

        public IEnumerable<T> GetAll()
        {
            return this.objects;
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> where)
        {
            return this.objects.ToList().Where(where.Compile());
        }

        public T Delete(T entity)
        {
            this.objects.ToList().RemoveAll(o => o.Equals(entity));
            return entity;
        }

        public void DeleteAll()
        {
            throw new NotImplementedException();
        }

        public T Add(T entity)
        {
            var newObjects = this.objects.ToList();
            newObjects.Add(entity);
            this.objects = newObjects;
            return entity;
        }

        public T Update(T data)
        {
            throw new NotImplementedException();
        }
    }
}
