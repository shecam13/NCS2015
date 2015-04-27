using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NetPressAssignment.ViewModels
{
    public class CategoryViewModel
    {


        [Display(Name = "Category")]
        [Required(ErrorMessage = "Category name required")]
        public string Name { get; set; }

         
    }
}