using NetPressAssignment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NetPressAssignment.Controllers
{
    public class PostSecurity
    {
        private NetPressAssignmentContext db = new NetPressAssignmentContext();

        public Boolean hasAccessTo (int? postId, String userId)
        {
            // if post id is null return false immediately
            if (postId == null)
            {
                return false;
            }

            // check that the post belongs to the user
            Post post = db.Posts.Find(postId);
            var userID = post.UserID;

            if (userID == userId)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }
    }

}