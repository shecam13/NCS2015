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
        public ActionResult Index()
        {
            var posts = db.Posts.Where(x => x.State == 2).ToList() ;
            var posts2 = posts.OrderByDescending(x => x.DateCreated);  //order by descending according to date created. 
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