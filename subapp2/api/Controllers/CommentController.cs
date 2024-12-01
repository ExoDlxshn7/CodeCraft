using Microsoft.AspNetCore.Mvc;
using SubApp1.DAL;
using SubApp1.Models;
using System.Security.Claims;

namespace SubApp1.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentRepository _commentRepository;

        public CommentController(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        [HttpPost]
        public async Task<IActionResult> AddCommentIndex(int postId, string comments)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var comment = new Comment
            {
                PostId = postId,
                UserId = userId,
                Comments = comments,
                CreatedAt = DateTime.Now
            };

            await _commentRepository.AddCommentAsync(comment);
            return RedirectToAction("Index", "Home");
        }
          public async Task<IActionResult> AddCommentProfile(int postId, string comments)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var comment = new Comment
            {
                PostId = postId,
                UserId = userId,
                Comments = comments,
                CreatedAt = DateTime.Now
            };

            await _commentRepository.AddCommentAsync(comment);
            return RedirectToAction("Profile", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> EditCommentIndex(int commentId, string comments)
        {
            var comment = await _commentRepository.GetCommentByIdAsync(commentId);
            if (comment != null)
            {
                comment.Comments = comments;
                await _commentRepository.UpdateCommentAsync(comment);
            }
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> EditCommentProfile(int commentId, string comments)
        {
            var comment = await _commentRepository.GetCommentByIdAsync(commentId);
            if (comment != null)
            {
                comment.Comments = comments;
                await _commentRepository.UpdateCommentAsync(comment);
            }
            return RedirectToAction("Profile", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCommentIndex(int commentId)
        {
            await _commentRepository.DeleteCommentAsync(commentId);
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> DeleteCommentProfile(int commentId)
        {
            await _commentRepository.DeleteCommentAsync(commentId);
            return RedirectToAction("Profile", "Home");
        }
    }
}

       

   