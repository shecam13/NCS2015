using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NetPressAssignment.Models;
using NetPressAssignment.ViewModels;
using System.Configuration;
using System.Data.SqlClient;
using Microsoft.AspNet.Identity;

namespace NetPressAssignment.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        private NetPressAssignmentContext db = new NetPressAssignmentContext();

        //String cs = System.Configuration.ConfigurationManager.ConnectionStrings["NetPressEntities"].ConnectionString;

        // GET: Posts
        public ActionResult Index() //int page = 1, int pageSize = 10
        {
            //this code is repeated in Home Controller index() method --> Decide where it is needed? 

            //get the logged in userId by using Identity
            var userId = User.Identity.GetUserId();
                        

            //create a list of posts that will be returned to the view
            List<Post> posts = null;
            
            //check if user logged in is admin then show all posts
            if (User.IsInRole("admin"))
            {
                //use include to get all post related information including the categories that will be stored in memmory
                posts = db.Posts.Include(p => p.Category).ToList();

                var viewmodel = (from p in db.Posts
                                 join c in db.Categories on p.CategoryID equals c.CategoryID
                                 //where userId == p.UserID
                                 //where userID equals p.UserID
                                 select new PostViewModel()
                                 {
                                     PostID = p.PostID,
                                     Name = c.Name,
                                     Title = p.Title,
                                     DateCreated = p.DateCreated,
                                     LastModified = p.LastModified,
                                     State = p.State,
                                 });
                return View(viewmodel.ToList());   
            }
            //else show only the author's posts
            else
            {             
                posts = db.Posts.Include(p => p.Category).Where(p => p.UserID == userId).ToList();

                //var userID = User.Identity.GetUserId();

                var viewmodel = (from p in db.Posts
                                 join c in db.Categories on p.CategoryID equals c.CategoryID
                                 where userId == p.UserID
                                 //where userID equals p.UserID
                                 select new PostViewModel()
                                 {
                                     PostID = p.PostID,
                                     Name = c.Name,
                                     Title = p.Title,
                                     DateCreated = p.DateCreated,
                                     LastModified = p.LastModified,
                                     State = p.State,
                                 });

                return View(viewmodel.ToList());          
                
            }
            
            return View(posts);
            
        }


        public ActionResult GetPosts()
        {
            var userId = User.Identity.GetUserId();
            //var posts = db.Posts.Where(x => x.State == state).Where(x => x.UserID == userId); 
            var posts = db.Posts.Where(x => x.UserID == userId); 
            return View(posts.ToList());      
        } 

        // GET: Posts/Details/5
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        public ActionResult Create()
        {
            //lists are loaded in the view 
            ViewBag.CategoryList = new SelectList(db.Categories, "CategoryID", "Name");
            //ViewBag.StateList = new SelectList(db.Posts, "State");

            return View(new ModifyPostViewModel());
        }

        private void UpdatePost(Post post, ModifyPostViewModel mpvm)
        {
            post.PostID = mpvm.PostID;
            post.Title = mpvm.Title;
            post.Body = mpvm.Body;
            post.CategoryID = mpvm.CategoryID;
            post.State = mpvm.State;

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //it is not important to bind everything, just bind items that are requested in the view
        public ActionResult Create(ModifyPostViewModel mpvm)
        {
            if (ModelState.IsValid)
            {
                var post = new Post();

                UpdatePost(post, mpvm);

                post.UserID = User.Identity.GetUserId();
                post.DateCreated = DateTime.Now;
                post.LastModified = DateTime.Now;

                db.Posts.Add(post);
                db.SaveChanges();

                //get the user id of the logged in user and save it to the post userid column
                post.UserID = User.Identity.GetUserId();
                //pass the date created and modified 
               
                db.Posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryList = new SelectList(db.Categories, "CategoryID", "Name", mpvm.CategoryID);
            //  ViewBag.StateList = new SelectList(db.Posts, "State");

            return View(mpvm);
        }

        // GET: Posts/Create
        //public ActionResult Create()
        //{
        //    //lists are loaded in the view 
        //    ViewBag.CategoryList = new SelectList(db.Categories, "CategoryID", "Name");
        //    //ViewBag.StateList = new SelectList(db.Posts, "State");

        //    return View();
        //}

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        ////it is not important to bind everything, just bind items that are requested in the view
        //public ActionResult Create([Bind(Include = "Title,Body,CategoryID,State")] Post post)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        //get the user id of the logged in user and save it to the post userid column
        //        post.UserID = User.Identity.GetUserId();
        //        //pass the date created and modified 
        //        post.DateCreated = DateTime.Now;
        //        post.LastModified = DateTime.Now;
        //        db.Posts.Add(post);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.CategoryList = new SelectList(db.Categories, "CategoryID", "Name", post.CategoryID);
        //    //  ViewBag.StateList = new SelectList(db.Posts, "State");

        //    return View(post);
        //}

        // GET: Posts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }

            var mpvm = new ModifyPostViewModel
            {
                PostID = post.PostID,
                Title = post.Title,
                Body = post.Body,
                CategoryID = post.CategoryID,
                State = post.State,
            };

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", post.CategoryID);


            return View(mpvm);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ModifyPostViewModel mpvm)
        {
            if (ModelState.IsValid)
            {
                var existingPost = db.Posts.Find(mpvm.PostID);
                //get the user id of the logged in user and save it to the post userid column

                UpdatePost(existingPost, mpvm);
                existingPost.UserID = User.Identity.GetUserId();
                //keep the same value for the date created 

                //pass the date modified 
                existingPost.LastModified = DateTime.Now;
                //db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", mpvm.CategoryID);

            return View(mpvm);
        }

        //// GET: Posts/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Post post = db.Posts.Find(id);
        //    if (post == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", post.CategoryID);
            
        //    return View(post);
        //}

        //// POST: Posts/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "PostID,Title,Body,CategoryID,State")] Post post)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        //get the user id of the logged in user and save it to the post userid column
        //        post.UserID = User.Identity.GetUserId();
        //        //keep the same value for the date created 
               
        //        //pass the date modified 
        //        post.LastModified = DateTime.Now;
        //        db.Entry(post).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", post.CategoryID);
            
        //    return View(post);
        //}

        // GET: Posts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
