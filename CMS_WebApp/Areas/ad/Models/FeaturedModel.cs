using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS_WebApp.Areas.ad.Models
{
    public class FeaturedModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string URL { get; set; }
        public bool? Enable { get; set; }
        public HttpPostedFileBase UploadFile { get; set; }
    }
}