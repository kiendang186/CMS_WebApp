using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS_WebApp.Areas.ad.DTO
{
    public class PostDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Excerpt { get; set; }
        public string Content { get; set; }
        public bool? Enable { get; set; }
        public int CategoryId { get; set; }
        public string Category { get; set; }
        public string AuthorId { get; set; }
        public string Author { get; set; }
        public string Date { get; set; }
        public string ImagePath { get; set; }
        public string AttachmentFile { get; set; }
        public string MetaTitle { get; set; }
    }
}