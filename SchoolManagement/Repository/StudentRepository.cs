using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using SchoolManagement.Interfaces;
using SchoolManagement.Validators;

namespace SchoolManagement.Repository
{
    public class Repository<T> : IRepository<T> where T : class, IEntity, IComparable
    {
        private readonly DataTable data;

        private readonly IEnumerable<T> objects;

        public Repository(DataTable data, IMap<DataTable, IEnumerable<T>> mappingObject)
        {
            Guard.ArgumentIsNotNull(data, nameof(data));
            Guard.ArgumentIsNotNull(mappingObject, nameof(mappingObject));

            this.data = data;
            this.objects = mappingObject.Map(data);
        }

        public IEnumerable<T> GetAll()
        {
            return this.objects;
        }

        public IEnumerable<T> Find(Func<T, bool> where)
        {
            return this.objects.ToList().Where(where);
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
            this.objects.ToList().Add(entity);
            return entity;
        }

        public T Update(T data)
        {
            throw new NotImplementedException();
        }
    }
}
