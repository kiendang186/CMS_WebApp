using CMS_WebApp.Areas.ad.DTO;
using CMS_WebApp.Areas.ad.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS_WebApp.Areas.ad.Controllers
{
    [Filters.AuthorizeAccount]
    public class FeaturedController : Controller
    {
        // GET: ad/Featured
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetInfoList()
        {
            var featuredInfoList = new List<FeaturedInfoDTO>();

            using (CMS_Entities _context = new CMS_Entities())
            {
                IEnumerable<FeatureInfo> featureInfos = _context.FeatureInfoes.ToList();                
                foreach(FeatureInfo fi in featureInfos)
                {
                    string modifiedDate = "";
                    string desc;
                    if(fi.ModifiedDate != null)
                    {
                        modifiedDate = DateTime.Parse(fi.ModifiedDate.ToString()).ToString("dd/MM/yyyy HH:mm:ss");
                    } else if(fi.Date != null)
                    {
                        modifiedDate = DateTime.Parse(fi.Date.ToString()).ToString("dd/MM/yyyy HH:mm:ss");
                    }

                    desc = fi.Description;
                    if(fi.Description.Length > 150)
                    {
                        desc = fi.Description.Substring(0, 150) + " ...";
                    }

                    featuredInfoList.Add(new FeaturedInfoDTO() { 
                        Id = fi.Id,
                        Title = fi.Title,
                        Description = desc,
                        Enable = fi.Enable,
                        URL = fi.URL,
                        Date = modifiedDate,
                        Author = fi.User.Username
                    });
                }
            }
            return Json(new { data = featuredInfoList }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddFeatured(FeaturedModel featuredModel)
        {
            bool result = false;
            string msg = "";
            try
            {
                FeatureInfo fi = new FeatureInfo();
                fi.Title = featuredModel.Title;
                fi.Description = featuredModel.Description;
                fi.URL = featuredModel.URL;
                fi.Enable = featuredModel.Enable;

                // Author
                User activeUser = (User)Session["activeUser"];
                if (activeUser != null)
                    fi.AuthorId = activeUser.Username;
                else
                    fi.AuthorId = "";

                // Date
                fi.Date = DateTime.Now;
                fi.ModifiedDate = fi.Date;

                // Image Path
                HttpPostedFileBase file = featuredModel.UploadFile;
                if (file != null && file.ContentLength > 0)
                {
                    string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                    string fileExtension = Path.GetExtension(file.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + fileExtension;
                    fi.ImagePath = "/Areas/ad/Upload/featured-info/" + fileName;
                    file.SaveAs(Path.Combine(Server.MapPath("~/Areas/ad/Upload/featured-info/"), fileName));
                }

                using (CMS_Entities _context = new CMS_Entities())
                {
                    _context.FeatureInfoes.Add(fi);
                    _context.SaveChanges();
                }

                result = true;
                msg = "Thêm dữ liệu thành công";
            }
            catch
            {
                result = false;
                msg = "Có lỗi khi thực hiện thêm dữ liệu";
            }
            return Json(new { status  = result, message = msg}, JsonRequestBehavior.AllowGet);
        }

        // GET: ad/featured/edit/id
        public ActionResult Edit(int id)
        {
            ViewBag.Id = id;
            return View();
        }

        [HttpGet]
        public ActionResult SingleFeatured(int id)
        {
            bool result = false;
            FeaturedInfoDTO fiDTO = null;
            try
            {
                CMS_Entities _context = new CMS_Entities();               
                FeatureInfo fi = _context.FeatureInfoes.Where(f => f.Id == id).FirstOrDefault();
                fiDTO = new FeaturedInfoDTO()
                {
                    Id = fi.Id,
                    Title = fi.Title,
                    Description = fi.Description,
                    Enable = fi.Enable,
                    URL = fi.URL,
                    Date = "",
                    Author = ""
                };
                result = true;                
            }
            catch
            {
                result = false;               
            }
            return Json(new { status = result, featuredInfo = fiDTO }, JsonRequestBehavior.AllowGet);
        }

        // POST: ad/featured/edit/1
        [HttpPost]
        public ActionResult EditFeatured(FeaturedModel featuredModel)
        {
            bool result = false;
            string msg = "";
            try
            {
                using (CMS_Entities _context = new CMS_Entities())
                {
                    FeatureInfo fi = _context.FeatureInfoes.Where(f => f.Id == featuredModel.Id).FirstOrDefault();                    
                    fi.Title = featuredModel.Title;
                    fi.Description = featuredModel.Description;
                    fi.URL = featuredModel.URL;
                    fi.Enable = featuredModel.Enable;

                    // Author
                    User activeUser = (User)Session["activeUser"];
                    if (activeUser != null)
                        fi.AuthorId = activeUser.Username;
                    else
                        fi.AuthorId = "";

                    // Modified Date                    
                    fi.ModifiedDate = DateTime.Now;

                    // Image Path
                    HttpPostedFileBase file = featuredModel.UploadFile;
                    if (file != null && file.ContentLength > 0)
                    {
                        // delete old file
                        string path = Server.MapPath("~" + fi.ImagePath);
                        if(System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }

                        string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                        string fileExtension = Path.GetExtension(file.FileName);
                        fileName = fileName + DateTime.Now.ToString("yymmssfff") + fileExtension;
                        fi.ImagePath = "/Areas/ad/Upload/featured-info/" + fileName;
                        file.SaveAs(Path.Combine(Server.MapPath("~/Areas/ad/Upload/featured-info/"), fileName));
                    }
                    _context.Entry(fi).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();
                }

                result = true;
                msg = "Cập nhật dữ liệu thành công";
            }
            catch
            {
                result = false;
                msg = "Có lỗi khi thực hiện cập nhật dữ liệu";
            }
            return Json(new { status = result, message = msg }, JsonRequestBehavior.AllowGet);
        }

        // Delete
        [HttpPost]
        public ActionResult Delete(int id)
        {
            int ret = -1; // -1: failed; 0: not found; 1: success
            string message = "";
            try
            {
                using (CMS_Entities _context = new CMS_Entities())
                {
                    FeatureInfo featureInfo = _context.FeatureInfoes.Where(f => f.Id == id).FirstOrDefault();
                    if (featureInfo != null)
                    {
                        string path = Server.MapPath("~" + featureInfo.ImagePath);

                        _context.FeatureInfoes.Remove(featureInfo);
                        _context.SaveChanges();

                        // delete old file                   
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }

                        ret = 1;
                        message = "Thông tin đã được xóa";
                    }
                    else
                    {
                        ret = 0;
                        message = "Không tìm thấy thông tin bạn cần xóa";
                    }
                }
            }
            catch
            {
                ret = -1;
                message = "Gặp lỗi khi thực hiện xóa thông tin này";
            }
        
            return Json(new { result = ret, responseText = message }, JsonRequestBehavior.AllowGet);
        }
    }
}