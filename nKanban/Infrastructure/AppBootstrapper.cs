using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using nKanban.Models;
using nKanban.Domain;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using nKanban.Infrastructure.IoC;
using nKanban.Services;

namespace nKanban.Infrastructure
{
    public static class AppBootstrapper
    {
        public static void Bootstrap()
        {
            BootstrapAutoMapper();
            BookstrapMongoClasses();
            //Seed();
        }

        private static void Seed()
        {
            var service = IoCFactory.GetService<ISimpleService>();

            var canada = new Country() { Name = "Canada", DateCreated = DateTime.UtcNow };
            var usa = new Country() { Name = "USA", DateCreated = DateTime.UtcNow };

            service.BulkInsert<Country>(canada, usa);

            var bc = new Province() { CountryId = canada.Id.Value, Name = "British Columbia" };
            var ab = new Province() { CountryId = canada.Id.Value, Name = "Alberta" };
            var sk = new Province() { CountryId = canada.Id.Value, Name = "Saskatchewan" };
            var mb = new Province() { CountryId = canada.Id.Value, Name = "Manitoba" };
            var on = new Province() { CountryId = canada.Id.Value, Name = "Ontario" };
            var qc = new Province() { CountryId = canada.Id.Value, Name = "Quebec" };
            var nb = new Province() { CountryId = canada.Id.Value, Name = "New Brunswick" };
            var ns = new Province() { CountryId = canada.Id.Value, Name = "Nova Scotia" };
            var pei = new Province() { CountryId = canada.Id.Value, Name = "Prince Edward Island" };
            var nfld = new Province() { CountryId = canada.Id.Value, Name = "Newfoundland" };
            var wa = new Province() { CountryId = usa.Id.Value, Name = "Washington" };
            var or = new Province() { CountryId = usa.Id.Value, Name = "Oregon" };
            var ca = new Province() { CountryId = usa.Id.Value, Name = "California" };
            var id = new Province() { CountryId = usa.Id.Value, Name = "Idaho" };
            var mt = new Province() { CountryId = usa.Id.Value, Name = "Montana" };
            var ut = new Province() { CountryId = usa.Id.Value, Name = "Utah" };

            service.BulkInsert<Province>(bc, ab, sk, mb, on, qc, nb, ns, pei, nfld, wa, or, ca, id, mt, ut);
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

            if (!BsonClassMap.IsClassMapRegistered(typeof(Organization)))
            {
                BsonClassMap.RegisterClassMap<Organization>(cm => 
                {
                    cm.AutoMap();
                });
            }
        }

        private static void BootstrapAutoMapper()
        {
            //RegisterViewModel -> User
            Mapper.CreateMap<RegisterViewModel, User>();

            //RegisterViewModel -> Organization
            Mapper.CreateMap<RegisterViewModel, Organization>().ForMember(o => o.Name, opt => opt.MapFrom(v => v.OrganizationName));
        }
    }
}