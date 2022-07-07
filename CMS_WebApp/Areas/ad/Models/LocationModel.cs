using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CMS_WebApp.Areas.ad.Models
{
    public class LocationModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên địa điểm")]
        [MaxLength(100, ErrorMessage = "Vui lòng nhập tên không quá 100 ký tự")]
        [Display(Name = "Địa điểm *")]
        public string Name { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { get; set; }
    }
}