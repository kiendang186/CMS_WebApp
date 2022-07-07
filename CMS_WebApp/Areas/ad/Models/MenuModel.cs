using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CMS_WebApp.Areas.ad.Models
{
    public class MenuModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên menu")]
        [MaxLength(200, ErrorMessage = "Tên menu chấp nhận tối đa 200 ký tự")]
        [Display(Name = "Tên menu *")]
        public string Title { get; set; }
        [Display(Name = "Liên kết")]
        public string URL { get; set; }
        [Display(Name = "Hiển thị")]
        public bool IsShow { get; set; }
    }
}