using CMS_WebApp.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CMS_WebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            //ModelBinders.Binders.Add(typeof(DateTime), new DateTimeModelBinder());
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_BeginRequest()
        {
            Response.Cache.SetCacheability(HttpCacheability.ServerAndNoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            //Exception exception = Server.GetLastError();
            //Server.ClearError();

            //RouteData routeData = new RouteData();
            //routeData.Values.Add("controller", "Error");
            //routeData.Values.Add("action", "Index");
            //routeData.Values.Add("exception", exception);

            //if (exception.GetType() == typeof(HttpException))
            //{
            //    routeData.Values.Add("statusCode", ((HttpException)exception).GetHttpCode());
            //}
            //else
            //{
            //    routeData.Values.Add("statusCode", 500);
            //}

            //IController controller = new ErrorController();
            //controller.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
            //Response.End();
        }
    }
}
