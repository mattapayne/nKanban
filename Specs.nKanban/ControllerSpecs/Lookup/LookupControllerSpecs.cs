using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;
using nKanban.Controllers;
using nKanban.Services;
using FakeItEasy;
using System.Web.Mvc;
using nKanban.Domain;
using System.Linq.Expressions;

namespace Specs.nKanban.ControllerSpecs.Lookup
{
    [Subject(typeof(LookupController))]
    public class context_for_controller
    {
        protected static LookupController controller;
        protected static ISimpleService simpleService;

        Establish context = () => 
        {
            simpleService = A.Fake<ISimpleService>();
            controller = new LookupController(simpleService);
        };
    }

    [Subject(typeof(LookupController))]
    public class when_contructed_without_simple_service
    {
        static Exception exception;

        Because of = () => { exception = Catch.Exception(() => { new LookupController(null); }); };

        It should_throw_an_exception = () => { exception.ShouldNotBeNull(); };
    }

    [Subject(typeof(LookupController))]
    public class when_getting_provinces : context_for_controller
    {
        static ActionResult result;
        static Guid countryId = new Guid("bcea0d14-8d95-4ea0-8ef1-8d1c22e09445");
        static IEnumerable<Province> provinces;

        Establish ctx = () => {
            provinces = TestUtilities.GetProvinces();
            A.CallTo(() => simpleService.GetAll<Province>(A<Expression<Func<Province, bool>>[]>.Ignored)).Returns(provinces);
        };

        Because of = () => { result = controller.Provinces(countryId); };

        It should_ask_the_simple_service_for_provinces = () => {
            A.CallTo(() => simpleService.GetAll<Province>(A<Expression<Func<Province, bool>>[]>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);
        };

        It should_be_a_json_result = () => { result.ShouldBeOfType<JsonResult>(); };

        It should_allow_get = () => {
            var jsonResult = result as JsonResult;
            jsonResult.JsonRequestBehavior.ShouldEqual(JsonRequestBehavior.AllowGet);
        };

        It should_contain_the_provinces = () => {
            var jsonResult = result as JsonResult;
            jsonResult.Data.ShouldEqual(provinces);
        };
    }
}
