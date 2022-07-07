using CMS_WebApp.Areas.ad.Models;
using CMS_WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using CMS_WebApp.Areas.ad.DTO;
using CMS_WebApp.Areas.ad.Common;

namespace CMS_WebApp.Controllers
{
    public class BlogController : Controller
    {
        private const int pageSize = 12;

        // GET: Category       
        public ActionResult Index(int? id, int? page)
        {
            if(id == null)
            {
                return RedirectToAction("Index", "Home");
            }

            try
            {                
                List<BlogItem> blogItems = GetBlogsByCategory((int)id);
                int pageNumber = page ?? 1;
                return View(blogItems.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception)
            {
                return Redirect("/error");
            }            
        }

        private List<BlogItem> GetBlogsByCategory(int id)
        {
            CMS_Entities _context = new CMS_Entities();

            // Category
            var categories = _context.Categories.ToList();
            Category category = new Category();
            foreach(Category c in categories)
            {
                if(c.Persistent == true)
                {
                    if (c.Code.Equals(id.ToString()))
                    {
                        category = c;
                        break;
                    }                        
                }
                else
                {
                    if(c.Id == id)
                    {
                        category = c;
                        break;
                    }
                }
            }

            ViewBag.BlogName = category.Name;
            ViewBag.BlogId = id;

            // Post
            var posts = _context.Posts.Where(p => p.CategoryId == category.Id && p.Enable == true).ToList();
            List<BlogItem> blogItems = new List<BlogItem>();
            if(posts != null)
            {
                foreach(Post p in posts)
                {
                    blogItems.Add(new BlogItem()
                    {
                        Id = p.Id,
                        Title = p.Title,
                        Date = ConvertPostDate(p),
                        ImagePath = p.ThumnailImagePath,
                        Author = p.User.Fullname,
                        MetaTitle = p.MetaTitle
                    });
                }
            }
            return blogItems;
        }

        private string ConvertPostDate(Post p)
        {
            string date = "";
            string tmpDtc = "";
            if (p.ModifiedDate != null)
            {
                tmpDtc = p.ModifiedDate.ToString();
                date = DateTime.Parse(tmpDtc).ToString("dd/MM/yyyy");
            }
            else if (p.Date != null)
            {
                tmpDtc = p.ModifiedDate.ToString();
                date = DateTime.Parse(tmpDtc).ToString("dd/MM/yyyy");
            }

            return date;
        }
        
        public ActionResult Detail(int? id)
        {
            if (id == null)
                return RedirectToAction("Index", "Home");
            CMS_Entities _context = new CMS_Entities();
            var post = _context.Posts.Where(p => p.Id == id && p.Enable == true).FirstOrDefault();
            if(post != null)
            {       
                // category
                if(post.Category.Persistent == true)
                {
                    ViewBag.CategoryId = post.Category.Code;
                }
                else
                {
                    ViewBag.CategoryId = post.Category.Id;
                }

                ViewBag.CategoryName = post.Category.Name;
                ViewBag.MetaTitle = post.Category.MetaTitle;

                // Post
                PostDTO postDTO = new PostDTO() { 
                    Id = post.Id,
                    Title = post.Title,
                    Excerpt = post.Excerpt,
                    Content = post.Content,
                    Date = ConvertPostDate(post),
                    Author = post.User.Fullname,
                    AttachmentFile = post.Attachment
                };

                string filename = "";
                string attachment = postDTO.AttachmentFile;
                if(attachment.Length > 0)
                {
                    int idx = attachment.LastIndexOf("/");
                    if (idx > -1)
                    {
                        filename = attachment.Substring(idx + 1);
                    }
                }

                ViewBag.AttachmentFileName = filename;                

                return View(postDTO);
            }
            else
            {
                return RedirectToAction("Index", "Error");
            }
        }
        
        [ChildActionOnly]
        public ActionResult RecentAnouncements()
        {
            HomeController homeController = new HomeController();
            List<PostDTO> postDTOs = homeController.GetPosts(Constant.NOTICE);
            return PartialView(postDTOs);
        }

        [ChildActionOnly]
        public ActionResult RecentPosts()
        {
            HomeController homeController = new HomeController();
            List<PostDTO> postDTOs = homeController.GetPosts(Constant.NEWS);
            return PartialView(postDTOs);
        }
    }
}