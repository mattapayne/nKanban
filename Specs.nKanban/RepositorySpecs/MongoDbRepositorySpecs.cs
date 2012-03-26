using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;
using nKanban.Services.Impl;
using FakeItEasy;
using nKanban.Persistence;
using nKanban.Domain;
using System.Linq.Expressions;
using nKanban.Services;
using nKanban.Persistence.MongoDb;
using MongoDB.Driver;

namespace Specs.nKanban.RepositorySpecs
{
    [Subject(typeof(MongoDbRepository))]
    public class constructed_without_required_dependencies
    {
        static Exception exception;

        Because of = () => { exception = Catch.Exception(() => { new MongoDbRepository(null); }); };

        It should_throw_an_exception = () => { exception.ShouldNotBeNull(); };
    }
}
