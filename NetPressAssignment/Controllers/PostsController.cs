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
            //get posts according to the status chosen
            var posts = db.Posts.Where(s => s.State == state);
            return View(posts.ToList());
        }

        //int[] GetPosts()
        //{
        //    DataTable data = GetDataFromQuery("SELECT PostID FROM Posts WHERE State = 'Published'");
            
        //    if (data != null)
        //    {
        //        foreach(var row in data)
        //        {

        //        }
        //    }
        // }

        //public List<int> ConvertTo<int>(DataTable data)
        //{
        //    List<int> Temp = new List<int>();
        //    try
        //    {
        //        List<string> columnsNames = new List<string>();
        //        foreach (DataColumn DataColumn in data.Columns)
        //            columnsNames.Add(DataColumn.ColumnName);
        //        Temp = data.AsEnumerable().ToList().ConvertAll<int>(row => getObject<int>(row, columnsNames));
        //        return Temp;
        //    }
        //    catch
        //    {
        //        return Temp;
        //    }

        //}

        //DataTable GetDataFromQuery(string query)
        //{
        //    SqlDataAdapter adap = new SqlDataAdapter(query, "cs");
        //    DataTable data = new DataTable();
        //    adap.Fill(data);
        //    return data;
        //}  

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
