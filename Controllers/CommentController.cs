using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Moboguide.Models;

namespace Moboguide.Controllers
{
    public class CommentController : Controller
    {
        Pcontext context = new Pcontext();
        //AddComment
        public IActionResult Add(IFormCollection vals)
        {
            Comment comment = new Comment();
            if (string.IsNullOrEmpty(vals["comment"]))
            {
                TempData["commentNote"] = "Please enter your comment first.";
                string url = Url.Action("details", "Search", new { name = vals["mobileName"] }) + "#comments";
                return Redirect(url);
            }
            else if (vals.ContainsKey("id"))
            {
                comment.UserId = int.Parse(vals["id"]);
                comment.MobileName = (string)vals["mobileName"];
                comment.Content = (string)vals["comment"];
                comment.DateTime = DateTime.Now;
                context.comments.Add(comment);
                context.SaveChanges();
                string url = Url.Action("details", "Search", new { name = comment.MobileName }) + "#comments";
                return Redirect(url);
            }
            else
            {
                TempData["commentNote"] = "Please sign in first.";
                string url = Url.Action("details", "Search", new { name = vals["mobileName"] }) + "#comments";
                return Redirect(url);
            }

        }
        //DeleteComment
        [Authorize(Policy = "AuthenticatedOnly")]
        public IActionResult Delete(int id, string mobileName)
        {
            var comment = context.comments.Find(id);
            context.comments.Remove(comment);
            context.SaveChanges();
            string url = Url.Action("details", "Search", new { name = mobileName }) + "#comments";
            return Redirect(url);
        }
        //UpdateComment
        [Authorize(Policy = "AuthenticatedOnly")]
        public IActionResult Update(int id, string mobileName)
        {
            TempData["editCheck"] = id.ToString();
            string url = Url.Action("details", "Search", new { name = mobileName }) + "#comments";
            return Redirect(url);
        }
        [HttpPost, ActionName("Update")]
        [Authorize(Policy = "AuthenticatedOnly")]
        public IActionResult UpdatePost(int id, string content)
        {
            var comment = context.comments.Find(id);
            comment.Content = content;
            context.Update(comment);
            context.SaveChanges();
            string url = Url.Action("details", "Search", new { name = comment.MobileName }) + "#comments";
            return Redirect(url);
        }
    }
}
