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

namespace NetPressAssignment.Controllers
{
    public class HomeController : Controller
    {

        private NetPressAssignmentContext db = new NetPressAssignmentContext();

        Models.Post p = new Models.Post();
        public ActionResult Index()
        {
            //get the logged in userId by using Identity
            var userId = User.Identity.GetUserId();
            
            //create a list of posts that will be returned to the view
            List<Post> posts = null;
            //check if user logged in is admin then show all posts
            if(User.IsInRole("admin"))
            {
                //use include to get all post related information including the categories that will be stored in memmory
                posts = db.Posts.Include(p => p.Category).ToList();
            }
            //else show only the author's posts
            else
            {
                //get only the posts that match the user id
                posts = db.Posts.Include(p => p.Category).Where(p => p.UserID == userId).ToList();
            }

            return View(posts);
            
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