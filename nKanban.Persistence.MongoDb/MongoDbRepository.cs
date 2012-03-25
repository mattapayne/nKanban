using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nKanban.Persistence;
using System.Linq.Expressions;
using nKanban.Domain;
using MongoDB.Driver;
using FluentMongo.Linq;

namespace nKanban.Persistence.MongoDb
{
    public class MongoDbRepository : IRepository
    {
        private readonly MongoDatabase _db;
        private string _collectionName;

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
            var q = _db.GetCollection<T>(_collectionName, SafeMode.True).AsQueryable();

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
            var result = _db.GetCollection<T>(_collectionName).Insert(item, SafeMode.True);
            return result.Ok;
        }

        public void SetCollectionName(string collectionName)
        {
            _collectionName = collectionName;
        }
    }
}
