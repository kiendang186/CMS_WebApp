using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS_WebApp.Areas.ad.DTO
{
    public class RoomDTO
    {
        public int Id { get; set; }       
        public string Name { get; set; }       
        public bool? Status { get; set; }       
        public string Description { get; set; }
        public string LocationName { get; set; }        
    }
}