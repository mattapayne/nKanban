using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nKanban.Domain;
using System.Linq.Expressions;

namespace nKanban.Services
{
    public interface ISimpleService : IService
    {
        IEnumerable<T> GetAll<T>(params Expression<Func<T, bool>>[] filters) where T : AbstractDomainObject;
        T Get<T>(Guid id) where T : AbstractDomainObject;
        bool Insert<T>(T item) where T : AbstractDomainObject;
        bool BulkInsert<T>(params T[] items) where T : AbstractDomainObject;
    }
}
