using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CMS_WebApp.Areas.ad.Models
{
    public class ResetPasswordModel
    {
        [Display(Name = "Mật khẩu mới")]
        [DataType(DataType.Password)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Mật khẩu không được rỗng")]
        [MinLength(6, ErrorMessage = "Mật khẩu có tối thiểu 6 ký tự")]
        public string Password { get; set; }
        [Display(Name = "Nhập lại mật khẩu")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Mật khẩu mới và nhập lại mật khẩu không giống")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string ResetCode { get; set; }
    }
}