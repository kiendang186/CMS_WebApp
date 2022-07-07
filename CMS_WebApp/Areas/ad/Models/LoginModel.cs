using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CMS_WebApp.Areas.ad.Models
{
    public class LoginModel
    {
        [Display(Name = "Tên tài khoản")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Vui lòng nhập tên đăng nhập")]
        public string Username { get; set; }

        [Display(Name = "Mật khẩu")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Vui lòng nhập mật khẩu")]
        public string Password { get; set; }
    }
}