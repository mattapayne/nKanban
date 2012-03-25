using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using nKanban.Models;
using nKanban.Domain;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;

namespace nKanban.Infrastructure
{
    public static class AppBootstrapper
    {
        public static void Bootstrap()
        {
            BootstrapAutoMapper();
            BookstrapMongoClasses();
        }

        private static void BookstrapMongoClasses()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(AbstractDomainObject)))
            {
                BsonClassMap.RegisterClassMap<AbstractDomainObject>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIdMember(cm.GetMemberMap(c => c.Id));
                    cm.IdMemberMap.SetIdGenerator(GuidGenerator.Instance);
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(User)))
            {
                BsonClassMap.RegisterClassMap<User>(cm =>
                {
                    cm.AutoMap();
                });
            }
        }

        private static void BootstrapAutoMapper()
        {
            //RegisterViewModel -> User
            Mapper.CreateMap<RegisterViewModel, User>();
        }
    }
}