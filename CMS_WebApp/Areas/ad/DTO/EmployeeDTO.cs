using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS_WebApp.Areas.ad.DTO
{
    public class EmployeeDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Position { get; set; }
        public string Degree { get; set; }
        public string DOB { get; set; }
        public string Email { get; set; }
        public string POB { get; set; }       
        public string Biography { get; set; }
        public string ImagePath { get; set; }
        public string MetaTitle { get; set; }
    }
}