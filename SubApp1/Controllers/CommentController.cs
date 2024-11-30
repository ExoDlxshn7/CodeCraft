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

        [HttpPost]
        public IActionResult AddCommentIndex(int postId, string comments)
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

        public IActionResult AddCommentProfile(int postId, string comments)
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
            return RedirectToAction("Profile", "Home");
        }

        // Edit a comment
        [HttpPost]
        public IActionResult EditCommentIndex(int commentId, string comments)
        {
            var comment = _context.Comments.Find(commentId);

            comment.Comments = comments; 

            _context.Comments.Update(comment);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult EditCommentProfile(int commentId, string comments)
        {
            var comment = _context.Comments.Find(commentId);

            comment.Comments = comments; 

            _context.Comments.Update(comment);
            _context.SaveChanges();

            return RedirectToAction("Profile", "Home");
        }
        
        // Delete a comment
        [HttpPost]
        public IActionResult DeleteComment(int commentId)
        {
            var comment = _context.Comments.Find(commentId);

            _context.Comments.Remove(comment);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
    }
}
