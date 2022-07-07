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
    public class HomeController : Controller
    {
        // GET: ad/Home
        public ActionResult Index()
        {
            try
            {
                ReadGeneralInfo();
                return View();
            }
            catch
            {
                return Redirect("/error");
            }            
        }

        private void ReadGeneralInfo()
        {
            using(CMS_Entities _context = new CMS_Entities())
            {
                // category
                var categories = _context.Categories.ToList();
                // post
                var posts = _context.Posts.ToList();
                // featured info
                var featureds = _context.FeatureInfoes.ToList();
                // room
                var rooms = _context.Rooms.Where(r => r.IsDeleted == false).ToList();

                // recent posts
                var recentPosts = _context.Posts.OrderByDescending(p => p.ModifiedDate).Take(6).ToList();
                List<PostDTO> postDTOs = new List<PostDTO>();
                if(recentPosts != null)
                {
                    foreach(Post p in recentPosts)
                    {
                        string modifiedDate = "";                       
                        if (p.ModifiedDate != null)
                        {
                            modifiedDate = DateTime.Parse(p.ModifiedDate.ToString()).ToString("dd/MM/yyyy HH:mm:ss");
                        }
                        else if (p.Date != null)
                        {
                            modifiedDate = DateTime.Parse(p.Date.ToString()).ToString("dd/MM/yyyy HH:mm:ss");
                        }

                        postDTOs.Add(new PostDTO() { 
                            Id = p.Id,
                            Title = p.Title,
                            Date = modifiedDate,
                            Author = p.User.Username,
                            ImagePath = p.ThumnailImagePath
                        });
                    }
                }

                ViewBag.NumOfCategories = categories == null ? 0 : categories.Count;
                ViewBag.NumOfPosts = posts == null ? 0 : posts.Count;
                ViewBag.NumOfFeatureds = featureds == null ? 0 : featureds.Count;
                ViewBag.NumOfRooms = rooms == null ? 0 : rooms.Count;
                ViewBag.RecentPosts = postDTOs;
            }
        }
    }
}