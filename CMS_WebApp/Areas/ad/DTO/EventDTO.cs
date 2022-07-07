using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS_WebApp.Areas.ad.DTO
{
    public class EventDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public string Description { get; set; }
        public int RoomId { get; set; }
        public string Room { get; set; }
        public int LocationId { get; set; }
        public string Location { get; set; }
        public string ThemeColor { get; set; }
        public bool IsFullDay { get; set; }
    }
}