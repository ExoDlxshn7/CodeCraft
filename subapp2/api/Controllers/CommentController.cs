using Microsoft.AspNetCore.Mvc;
using subapp2.DAL;
using subapp2.Models;
using System.Security.Claims;

namespace subapp2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;

        public CommentController(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddComment(int postId, string comments)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var comment = new Comment
            {
                PostId = postId,
                UserId = userId,
                Comments = comments,
                CreatedAt = DateTime.Now
            };

            await _commentRepository.AddCommentAsync(comment);
            return CreatedAtAction(nameof(GetCommentById), new { id = comment.Id }, comment);
        }

        [HttpPut("Edit/{commentId}")]
        public async Task<IActionResult> EditComment(int commentId, string comments)
        {
            var comment = await _commentRepository.GetCommentByIdAsync(commentId);
            if (comment == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (comment.UserId != userId)
            {
                return Unauthorized();
            }

            comment.Comments = comments;
            await _commentRepository.UpdateCommentAsync(comment);
            return NoContent();
        }

        [HttpDelete("Delete/{commentId}")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            var comment = await _commentRepository.GetCommentByIdAsync(commentId);
            if (comment == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (comment.UserId != userId)
            {
                return Unauthorized();
            }

            await _commentRepository.DeleteCommentAsync(commentId);
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCommentById(int id)
        {
            var comment = await _commentRepository.GetCommentByIdAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            return Ok(new
            {
                comment.Id,
                comment.PostId,
                comment.Comments,
                comment.CreatedAt,
                User = new
                {
                    comment.Users.Id,
                    comment.Users.UserName,
                    comment.Users.ProfilePic
                }
            });
        }
    }
}
