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
    
    public partial class ExLink
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string URL { get; set; }
        public string Group { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string ImagePath { get; set; }
    }
}