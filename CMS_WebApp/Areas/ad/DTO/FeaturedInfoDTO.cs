using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS_WebApp.Areas.ad.DTO
{
    public class FeaturedInfoDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool? Enable { get; set; }
        public string URL { get; set; }
        public string Date { get; set; }
        public string Author { get; set; }
    }
}