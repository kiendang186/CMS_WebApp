using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS_WebApp.Areas.ad.Models
{
    public class ContactInfoModel
    {
        public int Id { get; set; }

        [Display(Name = "Địa chỉ *")]
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
        [MaxLength(300, ErrorMessage = "Địa chỉ chấp nhận tối đa 300 ký tự")]
        public string Address { get; set; }

        [Display(Name = "Email *")]
        [Required(ErrorMessage = "Vui lòng nhập email")]        
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        [Display(Name = "Điện thoại *")]
        [Required(ErrorMessage = "Vui lòng nhập điện thoại")]              
        public string Phone { get; set; }

        [Display(Name = "Mã nhúng bản đồ")]
        [AllowHtml]
        public string MapEmbbed { get; set; }
    }
}