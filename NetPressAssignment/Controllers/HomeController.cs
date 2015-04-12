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

namespace NetPressAssignment.Controllers
{
    public class HomeController : Controller
    {

        private NetPressAssignmentContext db = new NetPressAssignmentContext();

        Models.Post p = new Models.Post();
        public ActionResult Index()
        {
            //return View();
            var posts = db.Posts.Include(p => p.Category).Include(p => p.User); //now
            return View(posts.ToList());  //now
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