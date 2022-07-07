using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CMS_WebApp.Areas.ad.Models
{
    public class CategoryModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Vui lòng cung cấp tên thể loại")]
        [MaxLength(200, ErrorMessage = "Tên thể loại có tối đa 200 ký tự")]
        [Display(Name = "Tên thể loại *")]
        public string Name { get; set; }
        [Display(Name = "Mô tả")]
        public string Description { get; set; }
    }
}