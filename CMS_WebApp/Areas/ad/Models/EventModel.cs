using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS_WebApp.Areas.ad.Models
{
    public class EventModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Nullable<System.DateTime> Start { get; set; }
        public Nullable<System.DateTime> End { get; set; }
        public string Description { get; set; }
        public int RoomId { get; set; }
        public string ThemeColor { get; set; }
        public Nullable<bool> IsFullDay { get; set; }        
    }
}