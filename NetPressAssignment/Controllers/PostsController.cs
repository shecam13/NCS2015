using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NetPressAssignment.Models;
using System.Configuration;
using System.Data.SqlClient;

namespace NetPressAssignment.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        private NetPressAssignmentContext db = new NetPressAssignmentContext();

        //String cs = System.Configuration.ConfigurationManager.ConnectionStrings["NetPressEntities"].ConnectionString;

        // GET: Posts
        public ActionResult Index()
        {
            var posts = db.Posts.Include(p => p.Category).Include(p => p.User);
            return View(posts.ToList());
        }

        public ActionResult GetPosts(String state)
        {
            //state = "published";
            //get posts according to the status chosen
            //var posts = db.Posts.Where(x => x.State == state);      
            //return View(posts.ToList());

            //IList<Post> PostsList = new List<Post>();
            //var query = from posts in db.Posts
            //            join users in db.Users
            //            on posts.Username equals users.Username
            //            where posts.State == state
            //            select new Post
            //            {
            //                //PostID = posts.PostID,
            //                Title = posts.Title,
            //                CategoryID = posts.CategoryID,
            //                DateCreated = posts.DateCreated,
            //                LastModified = posts.LastModified,
            //                State = posts.State
            //            };

            var query = from p in db.Posts
                        join u in db.Users
                        on p.Username equals u.Username
                        join c in db.Categories
                        on p.CategoryID equals c.CategoryID
                        where p.State == state
                        select new 
                        {
                            PostID = p.PostID,
                            Title = p.Title,
                            //Name = c.Name,
                            DateCreated = p.DateCreated,
                            LastModified = p.LastModified,
                            State = p.State
                        };

            var PostsList = query.ToList().Select(x => new Post 
                            {
                                PostID = x.PostID,
                                Title = x.Title,
                                //Name = x.Name,
                                DateCreated = x.DateCreated,
                                LastModified = x.LastModified,
                                State = x.State                         
                            }).ToList();
            return View(PostsList);
        }

        // GET: Posts/Details/5
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

        // GET: Posts/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name");
            ViewBag.Username = new SelectList(db.Users, "Username", "Password");
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PostID,Title,Body,CategoryID,DateCreated,LastModified,Username,State")] Post post)
        {
      
            if (ModelState.IsValid)
            {
                db.Posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", post.CategoryID);
            ViewBag.Username = new SelectList(db.Users, "Username", "Password", post.Username);
            return View(post);
        }

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
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", post.CategoryID);
            ViewBag.Username = new SelectList(db.Users, "Username", "Password", post.Username);
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PostID,Title,Body,CategoryID,DateCreated,LastModified,Username,State")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", post.CategoryID);
            ViewBag.Username = new SelectList(db.Users, "Username", "Password", post.Username);
            return View(post);
        }

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
