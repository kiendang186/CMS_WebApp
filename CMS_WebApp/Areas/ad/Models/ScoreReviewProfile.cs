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
    
    public partial class ScoreReviewProfile
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ScoreReviewProfile()
        {
            this.ScoreReviews = new HashSet<ScoreReview>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<byte> Semester { get; set; }
        public Nullable<int> SchoolYear { get; set; }
    
        public virtual SchoolYear SchoolYear1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ScoreReview> ScoreReviews { get; set; }
    }
}
