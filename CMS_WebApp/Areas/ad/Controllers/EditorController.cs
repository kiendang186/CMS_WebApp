using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS_WebApp.Areas.ad.Controllers
{
    public class EditorController : Controller
    {		
		public ActionResult ImageMnt()
		{	
			string images = "";		
			try
			{				
				DirectoryInfo drf = new DirectoryInfo(Server.MapPath("~/Archived/"));
				foreach (FileInfo fi in drf.GetFiles())
				{
					images += fi.Name + ";";
				}				
			}
			catch
			{
				return Redirect("/error");
			}
			return Json(images, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public ActionResult UploadFile(HttpPostedFileBase file)
		{
			bool result = false;			

			if (file != null && file.ContentLength > 0)
			{
				var fileName = Path.GetFileName(file.FileName);
				var path = Path.Combine(Server.MapPath("~/Archived/"), fileName);
				file.SaveAs(path);
				result = true;
			}

			return Json(result);
		}
	}
}