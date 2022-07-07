using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS_WebApp.Areas.ad.Models
{
    public class PostModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Excerpt { get; set; }

        [AllowHtml]
        public string Content { get; set; }
        public bool Enable { get; set; }        
        public HttpPostedFileBase ImageFile { get; set; }
        public int CategoryId { get; set; }
        public HttpPostedFileBase AttachmentFile { get; set; }
    }
}