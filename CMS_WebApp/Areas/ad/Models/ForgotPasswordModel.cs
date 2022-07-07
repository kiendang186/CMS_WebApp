using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CMS_WebApp.Areas.ad.Models
{
    public class ForgotPasswordModel
    {
        [Display(Name = "Tên đăng nhập")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Tên đăng nhập không được rỗng")]
        public String Username { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email không được rỗng")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public String Email { get; set; }
        public String ResetCode { get; set; }
    }
}