//using NetPressAssignment.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NetPressAssignment.Models;
using Microsoft.AspNet.Identity;
using System.Configuration;
using System.Data.SqlClient;

namespace NetPressAssignment.Controllers
{
    public class HomeController : Controller
    {

        private NetPressAssignmentContext db = new NetPressAssignmentContext();

        Models.Post p = new Models.Post();
        public ActionResult Index(string postCategory, string searchString, string authorSearch)
        {
            var posts = db.Posts.Where(x => x.State == 2).ToList() ;
            var posts2 = posts.OrderByDescending(x => x.DateCreated);  //order by descending according to date created. 

            var CList = new List<string>();

            var CQuery = from c in db.Categories
                           orderby c.Name
                           select c.Name;

            CList.AddRange(CQuery.Distinct());
            ViewBag.postCategory = new SelectList(CList);


            var posts3 = from p in db.Posts
                         select p;
         
            if (!string.IsNullOrEmpty(searchString))  //searching by Title
            {
                posts3 = posts3.Where(s => s.Title.Contains(searchString));
                return View(posts3);
            }

            if (!string.IsNullOrEmpty(postCategory))  //searching by Category (already in the database)
            {
                posts3 = posts3.Where(c => c.Category.Name == postCategory);
                return View(posts3);
            }

            if (!string.IsNullOrEmpty(authorSearch))  //searching by author
            {
                posts3 = posts3.Where(g => g.AspNetUser.UserName == authorSearch);
                return View(posts3);
            }

            return View(posts2);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}