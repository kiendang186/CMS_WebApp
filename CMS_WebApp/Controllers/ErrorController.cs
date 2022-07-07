using CMS_WebApp.Areas.ad.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS_WebApp.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            User user = (User)Session["activeUser"];
            string message = "";
            string title = "";
            if(user != null)
            {
                message = "Bạn có thể <a href='/ad'>Trở lại Bảng điều khiển</a> hoặc quay trở lại sau.";
                title = "Hệ thống <b>CMS</b>";
            }
            else
            {
                message = "Bạn có thể <a href='/'>Trở lại Trang chủ</a> hoặc quay trở lại sau.";
                title = "";
            }
            ViewBag.Message = message;
            ViewBag.Title = title;
            return View();
        }
    }
}