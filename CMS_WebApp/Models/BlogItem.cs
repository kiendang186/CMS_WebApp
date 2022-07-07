using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS_WebApp.Models
{
    public class BlogItem
    {
        public int Id { get; set; }
        public string Title { get; set; }       
        public string Author { get; set; }
        public string Date { get; set; }
        public string ImagePath { get; set; }
        public string MetaTitle { get; set; }
    }
}