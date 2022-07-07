using CMS_WebApp.Areas.ad.Common;
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
    public class PostController : Controller
    {
        // GET: ad/Post
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddPost()
        {
            List<SelectListItem> categories = new List<SelectListItem>();
            try
            {
                using (CMS_Entities _context = new CMS_Entities())
                {
                    foreach (Category c in _context.Categories)
                    {
                        categories.Add(new SelectListItem()
                        {
                            Value = c.Id.ToString(),
                            Text = c.Name,
                            Selected = (c.Persistent == true)
                        });
                    }

                    ViewData["Categories"] = categories;

                    return View();
                }
            }
            catch
            {
                return Redirect("/error");
            }
        }

        [HttpPost]
        public ActionResult AddPost(PostModel postModel)
        {
            bool result = false;
            string msg = "";
            try
            {
                Post post = new Post();
                post.Title = postModel.Title;
                post.Excerpt = postModel.Excerpt;
                post.Content = postModel.Content;
                post.Enable = postModel.Enable;
                post.CategoryId = postModel.CategoryId;
                post.MetaTitle = Utils.ConvertToUnSign(postModel.Title);

                // Author
                User activeUser = (User)Session["activeUser"];
                if (activeUser != null)
                    post.AuthorId = activeUser.Username;
                else
                    post.AuthorId = "";

                // Date
                post.Date = DateTime.Now;
                post.ModifiedDate = post.Date;

                // Image Path
                HttpPostedFileBase file = postModel.ImageFile;
                if (file != null && file.ContentLength > 0)
                {
                    string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                    string fileExtension = Path.GetExtension(file.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + fileExtension;
                    post.ThumnailImagePath = "/Areas/ad/Upload/img-post/" + fileName;
                    file.SaveAs(Path.Combine(Server.MapPath("~/Areas/ad/Upload/img-post/"), fileName));
                }
                else
                {
                    post.ThumnailImagePath = "";
                }

                // Attachment File Path
                HttpPostedFileBase afile = postModel.AttachmentFile;
                if (afile != null && afile.ContentLength > 0)
                {
                    string fileName = Path.GetFileNameWithoutExtension(afile.FileName);
                    string fileExtension = Path.GetExtension(afile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + fileExtension;
                    post.Attachment = "/Areas/ad/Upload/file-post/" + fileName;
                    afile.SaveAs(Path.Combine(Server.MapPath("~/Areas/ad/Upload/file-post/"), fileName));
                }
                else
                {
                    post.Attachment = "";
                }

                using (CMS_Entities _context = new CMS_Entities())
                {
                    _context.Posts.Add(post);
                    _context.SaveChanges();
                }

                result = true;
                msg = "Bài viết đã được thêm thành công";
            }
            catch
            {
                result = false;
                msg = "Có lỗi khi thực hiện thêm bài viết";
            }
            return Json(new { status = result, message = msg }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetPostList()
        {
            var postList = new List<PostDTO>();
            using (CMS_Entities _context = new CMS_Entities())
            {
                IEnumerable<Post> posts = _context.Posts.ToList();
                foreach (Post p in posts)
                {
                    string modifiedDate = "";
                    string excerpt;
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
                    {
                        excerpt = p.Excerpt.Substring(0, 150) + " ...";
                    }

                    postList.Add(new PostDTO()
                    {
                        Id = p.Id,
                        Title = p.Title,
                        Excerpt = excerpt,
                        Enable = p.Enable,                        
                        Date = modifiedDate,
                        Author = p.User.Username,
                        Category = p.Category.Name,
                        MetaTitle = p.MetaTitle
                    });
                }
            }
            return Json(new { data = postList }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditPost(int id)
        {            
            try
            {
                using(CMS_Entities _context = new CMS_Entities())
                {
                    Post post = _context.Posts.Where(p => p.Id == id).FirstOrDefault();                    
                    if (post != null)
                    {
                        string attachFile = "";
                        if(post.Attachment.Length > 0)
                        {
                            int idx = post.Attachment.LastIndexOf('/');
                            if(idx >= 0)
                            {
                                attachFile = post.Attachment.Substring(idx + 1);
                            }                            
                        }
                        PostDTO postDTO = new PostDTO()
                        {
                            Id = post.Id,
                            Title = post.Title,
                            Excerpt = post.Excerpt,
                            Content = post.Content,
                            Enable = post.Enable,
                            CategoryId = post.CategoryId,
                            AuthorId = post.AuthorId,
                            ImagePath = post.ThumnailImagePath,
                            AttachmentFile = attachFile
                        };

                        ViewData["Post"] = postDTO;

                        List<SelectListItem> categories = new List<SelectListItem>();
                        foreach(Category c in _context.Categories)
                        {
                            categories.Add(new SelectListItem()
                            {
                                Value = c.Id.ToString(),
                                Text = c.Name,
                                Selected = (c.Id == post.CategoryId)
                            });
                        }

                        ViewData["Categories"] = categories;
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            catch
            {
                return Redirect("/error");
            }
            return View();
        }    
        
        [HttpPost]
        public ActionResult EditPost(PostModel postModel)
        {
            bool result = false;
            string msg = "";
            try
            {
                using(CMS_Entities _context = new CMS_Entities())
                {
                    Post post = _context.Posts.Where(p => p.Id == postModel.Id).FirstOrDefault();
                    post.Title = postModel.Title;
                    post.Excerpt = postModel.Excerpt;
                    post.Content = postModel.Content;
                    post.Enable = postModel.Enable;
                    post.CategoryId = postModel.CategoryId;
                    post.MetaTitle = Utils.ConvertToUnSign(postModel.Title);

                    // Author
                    User activeUser = (User)Session["activeUser"];
                    if (activeUser != null)
                        post.AuthorId = activeUser.Username;
                    else
                        post.AuthorId = "";

                    // Date                    
                    post.ModifiedDate = DateTime.Now;

                    // Image Path
                    HttpPostedFileBase file = postModel.ImageFile;
                    if (file != null && file.ContentLength > 0)
                    {
                        // delete old file
                        string path = Server.MapPath("~" + post.ThumnailImagePath);
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }

                        string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                        string fileExtension = Path.GetExtension(file.FileName);
                        fileName = fileName + DateTime.Now.ToString("yymmssfff") + fileExtension;
                        post.ThumnailImagePath = "/Areas/ad/Upload/img-post/" + fileName;
                        file.SaveAs(Path.Combine(Server.MapPath("~/Areas/ad/Upload/img-post/"), fileName));
                    }                   

                    // Attachment File Path
                    HttpPostedFileBase afile = postModel.AttachmentFile;
                    if (afile != null && afile.ContentLength > 0)
                    {
                        // delete old file
                        string path = Server.MapPath("~" + post.Attachment);
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }

                        string fileName = Path.GetFileNameWithoutExtension(afile.FileName);
                        string fileExtension = Path.GetExtension(afile.FileName);
                        fileName = fileName + DateTime.Now.ToString("yymmssfff") + fileExtension;
                        post.Attachment = "/Areas/ad/Upload/file-post/" + fileName;
                        afile.SaveAs(Path.Combine(Server.MapPath("~/Areas/ad/Upload/file-post/"), fileName));
                    }

                    _context.Entry(post).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();

                    result = true;
                    msg = "Bài viết đã được cập nhật thành công";
                }
            }
            catch
            {
                result = false;
                msg = "Có lỗi khi thực hiện cập nhật bài viết";
            }
            return Json(new { status = result, message = msg }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeletePost(int id)
        {
            int ret = -1; // -1: failed; 0: not found; 1: success
            string message = "";            
            try
            {
                using (CMS_Entities _context = new CMS_Entities())
                {
                    Post post = _context.Posts.Where(p => p.Id == id).FirstOrDefault();
                    if (post != null)
                    {
                        string imagePath = Server.MapPath("~" + post.ThumnailImagePath);
                        string filePath = Server.MapPath("~" + post.Attachment);

                        _context.Posts.Remove(post);
                        _context.SaveChanges();

                        // delete old images and files                   
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }

                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }

                        ret = 1;
                        message = "Bài viết đã được xóa";
                    }
                    else
                    {
                        ret = 0;
                        message = "Không tìm thấy bài viết cần xóa";
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

        [HttpPost]
        public ActionResult DuplicatePost(int id)
        {
            int ret = -1; // -1: failed; 0: not found; 1: success
            string message = "";
            try
            {
                using (CMS_Entities _context = new CMS_Entities())
                {
                    Post post = _context.Posts.Where(p => p.Id == id).FirstOrDefault();
                    if (post != null)
                    {
                        Post p = new Post()
                        {
                            Id = 0,
                            Title = "[Sao chép] " + post.Title,
                            Excerpt = post.Excerpt,
                            Content = post.Content,
                            Enable = post.Enable,
                            ThumnailImagePath = post.ThumnailImagePath,
                            CategoryId = post.CategoryId,
                            AuthorId = post.AuthorId,
                            Date = post.Date,
                            ModifiedDate = post.ModifiedDate,
                            Attachment = post.Attachment,
                            MetaTitle = Utils.ConvertToUnSign(post.Title)
                        };
                        
                        _context.Posts.Add(p);
                        _context.SaveChanges();

                        ret = 1;
                        message = "Sao chép bài viết thành công";
                    }
                    else
                    {
                        ret = 0;
                        message = "Không tìm thấy bài viết cần sao chép";
                    }
                }
            }
            catch(Exception e)
            {
                ret = -1;
                message = "Gặp lỗi khi thực hiện sao chép bài viết";
            }
            return Json(new { result = ret, responseText = message }, JsonRequestBehavior.AllowGet);
        }
    }
}