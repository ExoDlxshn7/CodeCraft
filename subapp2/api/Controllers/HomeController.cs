using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using subapp2.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace subapp2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserDbContext _context;

        public HomeController(ILogger<HomeController> logger, UserDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("Index")]
        public IActionResult GetPosts()
        {
            var posts = _context.Posts
                .Include(p => p.Users) // Include the related Users (for the post author)
                .Include(p => p.Comments) // Include the related Comments
                .ThenInclude(c => c.Users) // Include the user who made the comment
                .OrderByDescending(p => p.CreatedAt) // Order posts by creation date
                .Select(p => new
                {
                    p.Id,
                    p.Content,
                    p.CreatedAt,
                    p.ImageUrl,
                    User = new
                    {
                        p.Users.Id,
                        p.Users.UserName,
                        p.Users.ProfilePic
                    },
                    Comments = p.Comments.Select(c => new
                    {
                        c.Id,
                        c.Comments,
                        c.CreatedAt,
                        User = new
                        {
                            c.Users.Id,
                            c.Users.UserName,
                            c.Users.ProfilePic
                        }
                    })
                })
                .ToList();

            return Ok(posts);
        }

        [HttpGet("Profile")]
        public IActionResult GetUserProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var userPosts = _context.Posts
                .Where(p => p.UserId == userId)
                .Include(p => p.Users)
                .Include(p => p.Comments)
                .ThenInclude(c => c.Users)
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new
                {
                    p.Id,
                    p.Content,
                    p.CreatedAt,
                    p.ImageUrl,
                    User = new
                    {
                        p.Users.Id,
                        p.Users.UserName,
                        p.Users.ProfilePic
                    },
                    Comments = p.Comments.Select(c => new
                    {
                        c.Id,
                        c.Comments,
                        c.CreatedAt,
                        User = new
                        {
                            c.Users.Id,
                            c.Users.UserName,
                            c.Users.ProfilePic
                        }
                    })
                })
                .ToList();

            var user = _context.Users.Find(userId);
            if (user == null) return NotFound();

            return Ok(new
            {
                ProfilePicUrl = user.ProfilePic,
                Posts = userPosts
            });
        }

        [HttpGet("Error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult GetError()
        {
            return Problem("An error occurred.", null, 500);
        }
    }
}
