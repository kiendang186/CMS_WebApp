using System.Web.Mvc;

namespace CMS_WebApp.Areas.ad
{
    public class adAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ad";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            // auth
            // login
            context.MapRoute(
                "Login Route",
                "ad/log-in",
                new { controller = "Auth", action = "Login", id = UrlParameter.Optional },
                new[] { "CMS_WebApp.Areas.ad.Controllers" }
            );

            // logout
            context.MapRoute(
                "Logout Route",
                "ad/log-out",
                new { controller = "Auth", action = "Logout", id = UrlParameter.Optional },
                new[] { "CMS_WebApp.Areas.ad.Controllers" }
            );


            // user profile
            context.MapRoute(
                "UserProfile Route",
                "ad/profile",
                new { controller = "Auth", action = "UserProfile", id = UrlParameter.Optional },
                new[] { "CMS_WebApp.Areas.ad.Controllers" }
            );

            // forgot password
            context.MapRoute(
                "ForgotPassword Route",
                "ad/forgot-password",
                new { controller = "Auth", action = "ForgotPassword", id = UrlParameter.Optional },
                new[] { "CMS_WebApp.Areas.ad.Controllers" }
            );

            // reset password
            context.MapRoute(
                "ResetPassword Route",
                "ad/reset-password/{id}",
                new { controller = "Auth", action = "ResetPassword", id = UrlParameter.Optional },
                new[] { "CMS_WebApp.Areas.ad.Controllers" }
            );            

            context.MapRoute(
                "ad_default",
                "ad/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { "CMS_WebApp.Areas.ad.Controllers" }
            );
        }
    }
}