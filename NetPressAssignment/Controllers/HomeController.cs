﻿//using NetPressAssignment.Models;
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
using PagedList.Mvc;
using PagedList;


namespace NetPressAssignment.Controllers
{
    public class HomeController : Controller
    {

        private NetPressAssignmentContext db = new NetPressAssignmentContext();

        Models.Post p = new Models.Post();
        public ActionResult Index(string authorSearch, string postCategory, string searchString, int? page)
        {

            var posts = db.Posts.Where(x => x.State == 2).ToList();
            var posts2 = posts.OrderByDescending(x => x.DateCreated);  //order by descending according to date created.
             
            var CList = new List<string>();

            var CQuery = from c in db.Categories  //query to get the category name ordered 
                         orderby c.Name
                         select c.Name;

            CList.AddRange(CQuery.Distinct());    //Adds the elements of the specified collection to the end of the list. It is only adding the distinct values of CQuery results. 
            ViewBag.postCategory = new SelectList(CList);

            var posts3 = from p in db.Posts    //getting the posts for the database
                         select p;

            if (!string.IsNullOrEmpty(authorSearch) || !string.IsNullOrEmpty(postCategory) || !string.IsNullOrEmpty(searchString))
            {
                if (!string.IsNullOrEmpty(authorSearch))  //searching by author
                {
                    posts3 = posts3.Where(g => g.AspNetUser.UserName == authorSearch);
                }
                if (!string.IsNullOrEmpty(postCategory))  //searching by Category (already in the database)
                {
                    posts3 = posts3.Where(c => c.Category.Name == postCategory);

                }
                if (!string.IsNullOrEmpty(searchString))  //searching by Title
                {
                    posts3 = posts3.Where(s => s.Title.Contains(searchString));
                }

                return View(posts3.OrderByDescending(x => x.DateCreated).ToPagedList(page ?? 1, 3));
            }
            else
            {
                return View(posts2.ToPagedList(page ?? 1, 3));
            }   
        }

        public ActionResult About()
        {
            ViewBag.Message = "CIS 2054 Assignment";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Designed and Developed by Clara Galea, Sheryl Camilleri & Natasha Nikolic";

            return View();
        }
    }
}