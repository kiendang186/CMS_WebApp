using CMS_WebApp.Areas.ad.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS_WebApp.Areas.ad.Controllers
{
    [Filters.AuthorizeAccount]
    public class MenuController : Controller
    {
        // GET: ad/Menu
        public ActionResult Index()
        {
            try
            {
                using(CMS_Entities _context = new CMS_Entities())
                {
                    return View(_context.Menus.ToList());
                }
            }
            catch
            {
                return Redirect("/error");
            }
        }

        [HttpPost]
        public ActionResult AddMenu(Menu menu)
        {
            try
            {
                using(CMS_Entities _context = new CMS_Entities())
                {
                    _context.Menus.Add(menu);
                    _context.SaveChanges();
                    return Json(new { result = true, message = "Thêm menu thành công" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Redirect("/error");
            }
        }

        [HttpPost]
        public ActionResult DeleteMenu(int id)
        {
            int ret = -1; //-1: error; 0: not found; 1: success
            string msg = "";
            try
            {
                using (CMS_Entities _context = new CMS_Entities())
                {
                    var menu = _context.Menus.Where(m => m.Id == id).FirstOrDefault();
                    if (menu != null)
                    {
                        _context.Menus.Remove(menu);
                        _context.SaveChanges();
                        ret = 1;
                        msg = "Menu đã được xóa";
                    }
                    else
                    {
                        ret = 0;
                        msg = "Không tìm thấy menu cần xóa";
                    }
                }
            }
            catch (Exception)
            {
                ret = -1;
                msg = "Có lỗi khi thực hiện xóa menu";
            }

            return Json(new { result = ret, responseText = msg }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditMenu(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");
            try
            {
                using (CMS_Entities _context = new CMS_Entities())
                {
                    Menu menu = _context.Menus.Where(m => m.Id == id).First();
                    if(menu != null)
                    {
                        MenuModel menuModel = new MenuModel()
                        {
                            Id = menu.Id,
                            Title = menu.Title,
                            URL = menu.URL,
                            IsShow = menu.IsShow == true
                        };
                        return View(menuModel);
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }                   
                };
            }
            catch (Exception)
            {
                return Redirect("/error");
            }            
        }

        [HttpPost]
        public ActionResult EditMenu(MenuModel menuModel)
        {    
            if (ModelState.IsValid)
            {
                using (CMS_Entities _context = new CMS_Entities())
                {
                    try
                    {
                        var menu = _context.Menus.Where(m => m.Id == menuModel.Id).FirstOrDefault();
                        menu.Title = menuModel.Title;
                        menu.URL = menuModel.URL;
                        menu.IsShow = menuModel.IsShow;
                        _context.Entry(menu).State = System.Data.Entity.EntityState.Modified;
                        _context.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    catch
                    {
                        return Redirect("/error");
                    }
                }
            }

            return View(menuModel);
        }
    }
}