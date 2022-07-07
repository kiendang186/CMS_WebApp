using CMS_WebApp.Areas.ad.Common;
using CMS_WebApp.Areas.ad.Controllers;
using CMS_WebApp.Areas.ad.DTO;
using CMS_WebApp.Areas.ad.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS_WebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ConfigController configController = new ConfigController();           
            return View(configController.ContactInfo());
        }

        [ChildActionOnly]
        public ActionResult MainMenu()
        {
            return PartialView(Menus());
        }

        [ChildActionOnly]
        public ActionResult FooterMenu()
        {
            return PartialView(Menus());
        }

        [ChildActionOnly]
        public ActionResult Slider()
        {
            List<FeatureInfo> fiList = new List<FeatureInfo>();
            using (CMS_Entities _context = new CMS_Entities())
            {
                fiList = _context.FeatureInfoes.Where(f => f.Enable == true).ToList();                
            }
            return PartialView(fiList);
        }

        [ChildActionOnly]
        public ActionResult Announcement()
        {
            return PartialView(GetPosts(Constant.NOTICE));
        }

        [ChildActionOnly]
        public ActionResult RecentPosts()
        {           
            return PartialView(GetPosts(Constant.NEWS));
        }

        [ChildActionOnly]
        public ActionResult Documents()
        {
            return PartialView(GetPosts(Constant.DOCUMENT));
        }
        public List<PostDTO> GetPosts(string categoryCode)
        {
            List<PostDTO> postDTOs = new List<PostDTO>();
            using (CMS_Entities _context = new CMS_Entities())
            {
                var config = _context.HomeConfigs.Where(c => c.Alias.Equals(categoryCode)).FirstOrDefault();
                int top = config.Items ?? 0;
                var posts = _context.Posts.Where(p => p.Category.Id == config.Category && p.Enable == true).Take(top).ToList();
                if(posts != null)
                {
                    foreach (Post p in posts)
                    {
                        string modifiedDate = "";
                        string excerpt = "";
                        if (p.ModifiedDate != null)
                        {
                            modifiedDate = DateTime.Parse(p.ModifiedDate.ToString()).ToString("dd/MM/yyyy HH:mm:ss");
                        }
                        else if (p.Date != null)
                        {
                            modifiedDate = DateTime.Parse(p.Date.ToString()).ToString("dd/MM/yyyy HH:mm:ss");
                        }

                        excerpt = p.Excerpt;
                        if (p.Excerpt.Length > 150)
                            excerpt = p.Excerpt.Substring(0, 150);

                        postDTOs.Add(new PostDTO() { 
                            Id = p.Id,
                            Title = p.Title,
                            Excerpt = excerpt,
                            ImagePath = p.ThumnailImagePath,
                            Author = p.User.Fullname,
                            Date = modifiedDate,
                            MetaTitle = p.MetaTitle
                        });
                    }
                }
            }
            return postDTOs;
        }

        [ChildActionOnly]
        public ActionResult TopContactInfo()
        {
            ConfigController configController = new ConfigController();           
            return PartialView(configController.ContactInfo());
        }

        [ChildActionOnly]
        public ActionResult FooterContactInfo()
        {
            ConfigController configController = new ConfigController();           
            return PartialView(configController.ContactInfo());
        }
        private List<MenuModel> Menus()
        {
            List<MenuModel> menuModels = new List<MenuModel>();
            using (CMS_Entities _context = new CMS_Entities())
            {                
                var menus = _context.Menus.Where(m => m.IsShow == true).ToList();
                if (menus != null)
                {
                    foreach (Menu m in menus)
                    {
                        menuModels.Add(new MenuModel()
                        {
                            Id = m.Id,
                            Title = m.Title,
                            URL = m.URL
                        });
                    }
                }                
            }
            return menuModels;
        }
    }
        
}