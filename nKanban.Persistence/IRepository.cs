using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using nKanban.Domain;

namespace nKanban.Persistence
{
    public interface IRepository
    {
        IEnumerable<T> Query<T>(params Expression<Func<T, bool>>[] query) where T : AbstractDomainObject;
        bool Insert<T>(T item) where T : AbstractDomainObject;
        void SetCollectionName(string collectionName);
    }
}
