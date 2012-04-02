using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using nKanban.Infrastructure.IoC;
using nKanban.Infrastructure;
using System.Threading;
using nKanban.Shared;

namespace nKanban
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Login", "Login", new { controller = "Session", action = "New" });
            routes.MapRoute("Logout", "Logout", new { controller = "Session", action = "Delete" });
            routes.MapRoute("ForgotPassword", "ForgotPassword", new { controller = "ForgotPassword", action = "New" });
            routes.MapRoute("ResetPassword", "ResetPassword", new { controller = "ResetPassword", action = "New" });
            routes.MapRoute("Register", "Register", new { controller = "Register", action = "New" });
            routes.MapRoute("Dashboard", "Dashboard", new { controller = "Dashboard", action = "Show" });

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterRoutes(RouteTable.Routes);

            DependencyResolver.SetResolver(IoCFactory.CreateDependencyResolver());

            AppBootstrapper.Bootstrap();
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            //TODO - Add code to ignore requests for assets like images, js, css, etc
            if (HttpContext.Current == null || HttpContext.Current.User == null)
            {
                return;
            }

            var customPrincipal = new NKanbanPrincipal(new NKanbanIdentity(new FormsIdentityWrapper(HttpContext.Current.User.Identity)));
            HttpContext.Current.User = customPrincipal;
            Thread.CurrentPrincipal = customPrincipal;
        }
    }
}