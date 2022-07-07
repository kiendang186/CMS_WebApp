using CMS_WebApp.Areas.ad.Models;
using System.Web;
using System.Web.Mvc;

namespace CMS_WebApp.Areas.ad.Common
{
    public class Auth
    {
        // Checking 'Admin' permission
        public static bool isAdminRole()
        {
            bool isAdmin = false;
            User activeUser = (User)HttpContext.Current.Session["activeUser"];
            if(activeUser != null)
            {
                if (activeUser.Role.Code.Equals(Constant.ADMIN))
                    isAdmin = true;
            }

            return isAdmin;
        }
    }
}