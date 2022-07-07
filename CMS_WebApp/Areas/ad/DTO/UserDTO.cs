using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS_WebApp.Areas.ad.DTO
{
    public class UserDTO
    {
        public string Username { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool? IsActived { get; set; }
        public string LastLogin { get; set; }
    }
}