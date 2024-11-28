using Microsoft.AspNetCore.Mvc;
using System.Linq;
using SubApp1.Models; // Assuming the models are in this namespace

namespace SubApp1.Controllers
{
    public class CommentController : Controller
    {
        private readonly UserDbContext _context;

        public CommentController(UserDbContext context)
        {
            _context = context;
        }

        // Add a comment
        [HttpPost]
        public IActionResult AddComment(int postId, string comments)
        {
            if (string.IsNullOrWhiteSpace(comments))
                {
                    return BadRequest("Comment content cannot be empty.");
                }
            
            var post = _context.Posts.FirstOrDefault(p => p.Id == postId);
            if (post == null)
            {
                return NotFound("Post not found.");
            }
            var comment = new Comment
            {
                PostId = postId,
                Comments = comments,
                CreatedAt = DateTime.Now,
                UserId = User.Identity.Name // Assuming User.Identity.Name holds the current user's ID
            };
            _context.Comments.Add(comment);
            _context.SaveChanges();
            return RedirectToAction("Index", "Post");
        }

        // Edit a comment
        [HttpPost]
        public IActionResult EditComment(int commentId, string comments)
        {
            var comment = _context.Comments.FirstOrDefault(c => c.Id == commentId && c.UserId == User.Identity.Name);
            if (comment == null)
            {
                return Forbid();
            }

            comment.Comments = comments;
            _context.SaveChanges();
            return RedirectToAction("Index", "Post");
        }

        // Delete a comment
        [HttpPost]
        public IActionResult DeleteComment(int commentId)
        {
            var comment = _context.Comments.FirstOrDefault(c => c.Id == commentId);
            if (comment == null)
            {
                return NotFound();
            }

            var post = _context.Posts.FirstOrDefault(p => p.Id == comment.PostId);
            if (comment.UserId != User.Identity.Name && post.UserId != User.Identity.Name)
            {
                return Forbid();
            }

            _context.Comments.Remove(comment);
            _context.SaveChanges();
            return RedirectToAction("Index", "Post");
        }
    }
}
