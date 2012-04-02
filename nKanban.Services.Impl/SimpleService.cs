using System;
using System.Collections.Generic;
using System.Linq;
using nKanban.Domain;
using nKanban.Persistence;
using System.Linq.Expressions;

namespace nKanban.Services.Impl
{
    public class SimpleService : AbstractBaseService, ISimpleService
    {
        public SimpleService(IRepository repository)
            : base(repository)
        {

        }

        public bool Insert<T>(T item) where T : AbstractDomainObject
        {
            return Repository.Insert<T>(item);
        }

        public bool BulkInsert<T>(params T[] items) where T : AbstractDomainObject
        {
            if (items == null || !items.Any())
            {
                return false;
            }

            return Repository.BulkInsert<T>(items);
        }

        public IEnumerable<T> GetAll<T>(params Expression<Func<T, bool>>[] filters) where T : AbstractDomainObject
        {
            return Repository.Query<T>(filters).ToList();
        }

        public T Get<T>(Guid id) where T : AbstractDomainObject
        {
            return Repository.Query<T>(item => item.Id == id).FirstOrDefault();
        }
    }
}
