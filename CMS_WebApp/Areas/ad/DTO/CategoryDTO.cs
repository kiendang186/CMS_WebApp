using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS_WebApp.Areas.ad.DTO
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }        
        public string ExtCode { get; set; }
    }
}