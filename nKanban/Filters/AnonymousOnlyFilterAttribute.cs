using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace nKanban.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AnonymousOnlyFilterAttribute : ActionFilterAttribute
    {
        public string ActionToRedirectToIfAuthenticated { get; set; }
        public string ControllerToRedirectToIfAuthenticated { get; set; }

        public AnonymousOnlyFilterAttribute()
        {
            ActionToRedirectToIfAuthenticated = "Show";
            ControllerToRedirectToIfAuthenticated = "Dashboard";
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { 
                    { "Controller", ControllerToRedirectToIfAuthenticated}, { "Action", ActionToRedirectToIfAuthenticated } 
                });

                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}