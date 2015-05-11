using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NetPressAssignment.ViewModels
{

    public class PostViewModel
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int PostID { get; set; }
        
        [Display(Name="Category")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        [AllowHtmlAttribute]
        public string Body { get; set; }

        [Display(Name = "Date Created")]
        public Nullable<System.DateTime> DateCreated { get; set; }

        [Display(Name = "Last Modified")]
        public Nullable<System.DateTime> LastModified { get; set; }
        
        public Nullable<int> State { get; set; }

        public string getState()
        {
            string str = "";

            if (State == 1)
            {
                str = "Draft";
            }
            else if (State == 2)
            {
                str = "Published";
            }
            else if (State == 3)
            {
                str = "Archived";
            }
            
            return str;
        }
    }

    public class ModifyPostViewModel
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int PostID { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Body is required.")]
        [AllowHtmlAttribute]
        public string Body { get; set; }

        public int CategoryID { get; set; }

        public Nullable<int> State { get; set; }
    }
}