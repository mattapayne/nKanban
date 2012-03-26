﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
                var url = new UrlHelper(filterContext.RequestContext);
                filterContext.Result = new RedirectResult(url.Action(ActionToRedirectToIfAuthenticated, ControllerToRedirectToIfAuthenticated));
                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}