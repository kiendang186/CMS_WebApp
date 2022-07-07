//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CMS_WebApp.Areas.ad.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Nullable<System.DateTime> Start { get; set; }
        public Nullable<System.DateTime> End { get; set; }
        public string Description { get; set; }
        public int RoomId { get; set; }
        public string ThemeColor { get; set; }
        public Nullable<bool> IsFullDay { get; set; }
        public string Username { get; set; }
    
        public virtual Room Room { get; set; }
        public virtual User User { get; set; }
    }
}
