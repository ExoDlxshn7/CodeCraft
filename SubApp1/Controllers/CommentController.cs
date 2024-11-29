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
            
            var post = _context.Posts.FirstOrDefault(p => p.Id == postId);
            
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
            var comment = _context.Comments.Find(commentId);

            comment.Comments = comments; 

            _context.Comments.Update(comment);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
        
        // Delete a comment
        [HttpPost]
        public IActionResult DeleteComment(int commentId)
        {
            var comment = _context.Comments.FirstOrDefault(c => c.Id == commentId);

            _context.Comments.Remove(comment);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
    }
}
