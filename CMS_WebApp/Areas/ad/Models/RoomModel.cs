using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS_WebApp.Areas.ad.Models
{
    public class RoomModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên phòng")]
        [MaxLength(300, ErrorMessage = "Vui lòng nhập không quá 300 ký tự")]
        [Display(Name = "Tên phòng *")]
        public string Name { get; set; }
        [Display(Name = "Có thể sử dụng")]
        public bool Status { get; set; }
        [Display(Name = "Ghi chú")]
        public string Description { get; set; }
        [Display(Name = "Địa điểm *")]
        public int LocationId { get; set; } 
        public List<SelectListItem> LocationList { get; set; }
    }
}