//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NetPressAssignment.Models
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    
    public partial class Post
    {
        public int PostID { get; set; }
        public string Title { get; set; }
        [AllowHtmlAttribute]
        public string Body { get; set; }
        public int CategoryID { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public Nullable<System.DateTime> LastModified { get; set; }
        public string UserID { get; set; }
        public Nullable<int> State { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual Category Category { get; set; }
    }
}
