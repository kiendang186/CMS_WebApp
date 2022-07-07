using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS_WebApp.Areas.ad.Models
{
    public class EmployeeModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Position { get; set; }
        public string Degree { get; set; }
        public DateTime DOB { get; set; }
        public string Email { get; set; }
        public string POB { get; set; }
        [AllowHtml]
        public string Biography { get; set; } 
        public HttpPostedFileBase FileImage { get; set; }
    }
}