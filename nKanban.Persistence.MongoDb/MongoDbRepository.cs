using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using nKanban.Domain;
using MongoDB.Driver;
using FluentMongo.Linq;

namespace nKanban.Persistence.MongoDb
{
    public class MongoDbRepository : IRepository
    {
        private readonly MongoDatabase _db;

        public MongoDbRepository(MongoDatabase db)
        {
            if (db == null)
            {
                throw new ArgumentNullException("db");
            }

            _db = db;
        }

        public IEnumerable<T> Query<T>(params Expression<Func<T, bool>>[] filters) where T : AbstractDomainObject
        {
            var q = _db.GetCollection<T>(GetCollectionName<T>(), SafeMode.True).AsQueryable();

            if (filters != null)
            {
                foreach (var filter in filters)
                {
                    q = q.Where(filter);
                }
            }

            return q.ToList();
        }

        public bool Insert<T>(T item) where T : AbstractDomainObject
        {
            var result = _db.GetCollection<T>(GetCollectionName<T>()).Insert(item, SafeMode.True);
            return result.Ok;
        }

        public bool BulkInsert<T>(params T[] items) where T : AbstractDomainObject
        {
            if (items == null || !items.Any())
            {
                return false;
            }

            var results = _db.GetCollection<T>(GetCollectionName<T>()).InsertBatch(items, SafeMode.True);
            return results.All(r => r.Ok);
        }

        private string GetCollectionName<T>()
        {
            return typeof(T).Name;
        }
    }
}
