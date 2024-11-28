using Microsoft.AspNetCore.Mvc;
using SubApp1.Models;
using System.Security.Claims; 

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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

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
                UserId = userId,
                Comments = comments,
                CreatedAt = DateTime.Now
            };

            _context.Comments.Add(comment);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
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
