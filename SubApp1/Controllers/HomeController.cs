using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SubApp1.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace SubApp1.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserDbContext _context;

    public HomeController(ILogger<HomeController> logger, UserDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        var posts = _context.Posts
            .Include(p => p.Users) // Include the related Users (for the post author)
            .Include(p => p.Comments) // Include the related Comments
            .ThenInclude(c => c.Users) // Optionally, include the user who made the comment
            .OrderByDescending(p => p.CreatedAt) // Order posts by creation date
            .ToList();

        return View(posts); // Pass the posts (with users and comments) to the view
    }

    public IActionResult Profile()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var userPosts = _context.Posts
            .Where(p => p.UserId == userId)
            .Include(p => p.Users)
            .OrderByDescending(p => p.CreatedAt)
            .ToList();

        return View(userPosts);
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}