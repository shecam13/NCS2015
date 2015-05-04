using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NetPressAssignment.ViewModels
{
    public class CategoryViewModel
    {


        [Display(Name = "Category")]
        [Required(ErrorMessage = "Category name required")]
        public string Name { get; set; }

        [Key]
        [HiddenInput(DisplayValue = false)]
        public int CategoryID { get; set; }
         
    }
}