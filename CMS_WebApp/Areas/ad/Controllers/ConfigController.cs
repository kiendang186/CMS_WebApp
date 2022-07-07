using CMS_WebApp.Areas.ad.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS_WebApp.Areas.ad.Controllers
{
    [Filters.AuthorizeAccount]
    public class ConfigController : Controller
    {
        // GET: ad/Config
        [HttpGet]
        public ActionResult Layout()
        {
            using(CMS_Entities _context = new CMS_Entities())
            {
                var homeViews = _context.HomeConfigs.ToList();
                foreach(HomeConfig hc in homeViews)
                {
                    hc.Category = hc.Category ?? 0;
                    hc.Items = hc.Items ?? 0;
                }
                ViewBag.Categories = CreateCategoriesList();
                return View(homeViews);
            }            
        }

        [HttpPost]
        public ActionResult Layout(List<HomeConfig> configs)
        {
            bool success = false;
            string message = "";
            using (CMS_Entities _context = new CMS_Entities())
            {
                try
                {   foreach(HomeConfig c in configs)
                    {
                        var config = _context.HomeConfigs.Where(i => i.Id == c.Id).FirstOrDefault();
                        if(config != null)
                        {
                            config.Category = c.Category;
                            config.Items = c.Items;
                            _context.Entry(config).State = System.Data.Entity.EntityState.Modified;
                            _context.SaveChanges();
                        }
                    }                
                    success = true;
                    message = "Cập nhật thành công";
                }
                catch
                {
                    success = false;
                    message = "Cập nhật không thành công";
                }
            }
            return Json(new { success, message }, JsonRequestBehavior.AllowGet);
        }

        private List<SelectListItem> CreateCategoriesList()
        {
            List<SelectListItem> categoryList = new List<SelectListItem>();
            using (CMS_Entities _context = new CMS_Entities())
            {
                var categories = _context.Categories.ToList();
                foreach(Category c in categories)
                {
                    categoryList.Add(new SelectListItem() { 
                        Value = c.Id.ToString(),
                        Text = c.Name
                    });
                }
            }

            return categoryList;
        }

        [HttpGet]
        public ActionResult Contact()
        {
            using (CMS_Entities _context = new CMS_Entities())
            {
                var contactInfo = _context.ContactInfoes.FirstOrDefault();
                ContactInfoModel model = new ContactInfoModel();
                if (contactInfo != null)
                {
                    model.Id = contactInfo.Id;
                    model.Address = contactInfo.Address;
                    model.Email = contactInfo.Email;
                    model.Phone = contactInfo.Phone;
                    model.MapEmbbed = contactInfo.MapEmbbed;
                }
                
                return View(model);
            }            
        }

        [HttpPost]
        public ActionResult Contact(ContactInfoModel model)
        {
            if (ModelState.IsValid)
            {
                using(CMS_Entities _context = new CMS_Entities())
                {
                    var contactInfo = _context.ContactInfoes.Where(c => c.Id == model.Id).FirstOrDefault();
                    if(contactInfo == null)
                    {
                        ContactInfo newContactInfo = new ContactInfo()
                        {
                            Address = model.Address,
                            Email = model.Email,
                            Phone = model.Phone,
                            MapEmbbed = model.MapEmbbed
                        };
                        _context.ContactInfoes.Add(newContactInfo);
                    }
                    else
                    {
                        contactInfo.Address = model.Address;
                        contactInfo.Email = model.Email;
                        contactInfo.Phone = model.Phone;
                        contactInfo.MapEmbbed = model.MapEmbbed;
                        _context.Entry(contactInfo).State = System.Data.Entity.EntityState.Modified;
                    }

                    _context.SaveChanges();
                    ViewBag.Success = true;

                    return View(model);
                }
            }

            return View(model);
        }

        public ContactInfo ContactInfo()
        {
            using (CMS_Entities _context = new CMS_Entities())
            {
                return _context.ContactInfoes.FirstOrDefault();
            }
        }
    }
}