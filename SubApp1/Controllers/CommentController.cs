using Microsoft.AspNetCore.Mvc;
using SubApp1.DAL;
using SubApp1.Models;
using System.Security.Claims;

namespace SubApp1.Controllers
{
    public class CommentController : Controller
    {
        // Dependency injection for the comment repository
        private readonly ICommentRepository _commentRepository;

        // Constructor to initialize the comment repository
        public CommentController(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        // Action method to add a comment and redirect to the Index page
        [HttpPost]
        public async Task<IActionResult> AddCommentIndex(int postId, string comments)
        {
            // Get the user ID from the claims
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // Create a new comment object
            var comment = new Comment
            {
                PostId = postId,
                UserId = userId,
                Comments = comments,
                CreatedAt = DateTime.Now
            };

            // Add the comment to the repository
            await _commentRepository.AddCommentAsync(comment);
            // Redirect to the Index page
            return RedirectToAction("Index", "Home");
        }

        // Action method to add a comment and redirect to the Profile page
        public async Task<IActionResult> AddCommentProfile(int postId, string comments)
        {
            // Get the user ID from the claims
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // Create a new comment object
            var comment = new Comment
            {
                PostId = postId,
                UserId = userId,
                Comments = comments,
                CreatedAt = DateTime.Now
            };

            // Add the comment to the repository
            await _commentRepository.AddCommentAsync(comment);
            // Redirect to the Profile page
            return RedirectToAction("Profile", "Home");
        }

        // Action method to edit a comment and redirect to the Index page
        [HttpPost]
        public async Task<IActionResult> EditCommentIndex(int commentId, string comments)
        {
            // Get the comment by ID from the repository
            var comment = await _commentRepository.GetCommentByIdAsync(commentId);
            if (comment != null)
            {
                // Update the comment text
                comment.Comments = comments;
                // Save the updated comment to the repository
                await _commentRepository.UpdateCommentAsync(comment);
            }
            // Redirect to the Index page
            return RedirectToAction("Index", "Home");
        }

        // Action method to edit a comment and redirect to the Profile page
        public async Task<IActionResult> EditCommentProfile(int commentId, string comments)
        {
            // Get the comment by ID from the repository
            var comment = await _commentRepository.GetCommentByIdAsync(commentId);
            if (comment != null)
            {
                // Update the comment text
                comment.Comments = comments;
                // Save the updated comment to the repository
                await _commentRepository.UpdateCommentAsync(comment);
            }
            // Redirect to the Profile page
            return RedirectToAction("Profile", "Home");
        }

        // Action method to delete a comment and redirect to the Index page
        [HttpPost]
        public async Task<IActionResult> DeleteCommentIndex(int commentId)
        {
            // Delete the comment from the repository
            await _commentRepository.DeleteCommentAsync(commentId);
            // Redirect to the Index page
            return RedirectToAction("Index", "Home");
        }

        // Action method to delete a comment and redirect to the Profile page
        public async Task<IActionResult> DeleteCommentProfile(int commentId)
        {
            // Delete the comment from the repository
            await _commentRepository.DeleteCommentAsync(commentId);
            // Redirect to the Profile page
            return RedirectToAction("Profile", "Home");
        }
    }
}