﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using nKanban.Domain;

namespace nKanban.Persistence
{
    public interface IRepository
    {
        IEnumerable<T> Query<T>(params Expression<Func<T, bool>>[] query) where T : AbstractDomainObject;
        bool Insert<T>(T item) where T : AbstractDomainObject;
        bool BulkInsert<T>(params T[] items) where T : AbstractDomainObject;
    }
}
