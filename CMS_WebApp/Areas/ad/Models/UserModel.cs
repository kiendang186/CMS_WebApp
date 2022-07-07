using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS_WebApp.Areas.ad.Models
{
    public class UserModel
    {
        [Display(Name = "Tên đăng nhập *")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Tên đăng nhập không được rỗng")]
        [MinLength(6, ErrorMessage = "Tên đăng nhập có tối thiểu 6 ký tự")]
        [RegularExpression(@"^[a-zA-Z0-9'_\s]+$", ErrorMessage = "Tên đăng nhập chỉ chứa các ký tự 0-9, a-z, A-Z hoặc dấu _")]
        public string UserId { get; set; }

        [Display(Name = "Mật khẩu *")]
        [DataType(DataType.Password)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Mật khẩu không được rỗng")]
        [MinLength(6, ErrorMessage = "Mật khẩu có tối thiểu 6 ký tự")]
        public string Password { get; set; }

        [Display(Name = "Nhập lại mật khẩu *")]
        [DataType(DataType.Password)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Nhập lại mật khẩu không được rỗng")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Nhập lại mật khẩu không giống mật khẩu")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Họ tên *")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Họ tên không được rỗng")]
        public string FullName { get; set; }

        [Display(Name = "Email *")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email không được rỗng")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        [Display(Name = "Điện thoại")]
        [RegularExpression(@"^(\d)+$", ErrorMessage = "Số điện thoại không hợp lệ")]
        [MinLength(10, ErrorMessage = "Số điện thoại phải đủ 10 số")]
        [MaxLength(10, ErrorMessage = "Số điện thoại phải đủ 10 số")]
        public string Phone { get; set; }

        public byte IsDeleted { get; set; }

        [Display(Name = "Kích hoạt tài khoản")]
        public bool IsActived { get; set; }

        public Nullable<System.DateTime> LastLogin { get; set; }
        public Nullable<System.DateTime> LastLogout { get; set; }

        [Display(Name = "Ảnh đại diện")]
        public string ProfileImage { get; set; }

        public HttpPostedFileBase UploadFile { get; set; }

        [Display(Name = "Vai trò")]
        [Required(ErrorMessage = "Chọn vai trò cho người dùng")]
        public int RoleId { get; set; }
        public List<SelectListItem> RoleList { get; set; }

        public UserModel()
        {
            this.ProfileImage = "/Areas/ad/Profiles/default.png";
        }
    }
}