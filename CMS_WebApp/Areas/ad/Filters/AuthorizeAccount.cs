using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS_WebApp.Areas.ad.Filters
{
    public class AuthorizeAccount : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpSessionStateBase stateBase = filterContext.HttpContext.Session;
            if (((HttpContext.Current.Session["activeUser"] == null) && (!stateBase.IsNewSession)) || stateBase.IsNewSession)
            {
                var url = new UrlHelper(filterContext.RequestContext);
                var loginUrl = url.Content("/ad/log-out");
                stateBase.RemoveAll();
                stateBase.Clear();
                stateBase.Abandon();
                filterContext.HttpContext.Response.Redirect(loginUrl, true);
            }
            base.OnActionExecuting(filterContext);
        }
    }
}