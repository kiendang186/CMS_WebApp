using CMS_WebApp.Areas.ad.Common;
using CMS_WebApp.Areas.ad.DTO;
using CMS_WebApp.Areas.ad.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS_WebApp.Areas.ad.Controllers
{
    [Filters.AuthorizeAccount]
    public class CategoryController : Controller
    {           
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetCategoryList()
        {
            CMS_Entities _context = new CMS_Entities();
            var categories = _context.Categories.OrderByDescending(c => c.Id).ToList();
            List<CategoryDTO> retCategories = new List<CategoryDTO>();
            foreach(Category c in categories)
            {
                string peristent = "ACTION";
                if(c.Persistent == true)
                {
                    peristent = "NO_ACTION";
                }
                retCategories.Add(new CategoryDTO() { 
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,                    
                    ExtCode = c.Id + "-" + peristent
                });
            }
            return Json(new { data = retCategories }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]        
        public ActionResult AddCategory(Category category)
        {
            bool success = false;
            string message = "";
            using (CMS_Entities _context = new CMS_Entities())
            {
                try
                {
                    category.Persistent = false;
                    category.Code = "";
                    category.MetaTitle = Utils.ConvertToUnSign(category.Name);
                    _context.Categories.Add(category);
                    _context.SaveChanges();
                    success = true;
                    message = "Thêm thành công";
                }
                catch
                {
                    success = false;
                    message = "Thêm không thành công";
                }
            }
            return Json(new { success, message}, JsonRequestBehavior.AllowGet);
        } 

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");

            try
            {
                using (CMS_Entities _context = new CMS_Entities())
                {
                    Category category = _context.Categories.Where(c => c.Id == id).FirstOrDefault();
                    if (category != null)
                    {
                        CategoryModel categoryModel = new CategoryModel()
                        {
                            Id = category.Id,
                            Name = category.Name,
                            Description = category.Description
                        };

                        return View(categoryModel);
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            catch(Exception)
            {
                return Redirect("/error");
            }
        }


        [HttpPost]
        public ActionResult Edit(CategoryModel category)
        {           
            if (ModelState.IsValid)
            {
                using (CMS_Entities _context = new CMS_Entities())
                {
                    try
                    {
                        var foundCategory = _context.Categories.Where(c => c.Id == category.Id).FirstOrDefault();   
                        foundCategory.Name = category.Name;
                        foundCategory.Description = category.Description;
                        foundCategory.MetaTitle = Utils.ConvertToUnSign(category.Name);
                        _context.Entry(foundCategory).State = System.Data.Entity.EntityState.Modified;
                        _context.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    catch
                    {
                        return Redirect("/error");
                    }
                }
            }

            return View(category);
        }
        
        [HttpPost]
        public ActionResult Delete(int id)
        {
            int ret = -1; // -1: failed; 0: not found; 1: success
            string message = "";
            CMS_Entities _context = new CMS_Entities();
            try
            {
                Category category = _context.Categories.Where(c => c.Id == id && c.Persistent == false).FirstOrDefault();
                if (category != null)
                {
                    if (category.Posts.Count() > 0)
                        _context.Posts.RemoveRange(category.Posts);
                    _context.Categories.Remove(category);                    
                    _context.SaveChanges();

                    ret = 1;
                    message = "Thể loại \'" + category.Name + "\' đã được xóa";
                }
                else
                {
                    ret = 0;
                    message = "Không tìm thấy thể loại bạn cần xóa";
                }
            }
            catch
            {
                ret = -1;
                message = "Gặp lỗi khi thực hiện xóa thể loại này";
            }
            return Json(new { result = ret, responseText = message }, JsonRequestBehavior.AllowGet);
        }
    }
}