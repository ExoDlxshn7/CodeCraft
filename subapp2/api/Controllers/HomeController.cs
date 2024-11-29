using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SubApp2.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace SubApp2.Controllers;

[ApiController]
[Route("api/[controller]")]
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
        var posts = _context.Posts.Include(p => p.Users).OrderByDescending(p => p.CreatedAt).ToList();
        var comments = _context.Posts.Include(p => p.Comments).OrderByDescending(p => p.CreatedAt).ToList();
        return View(posts);
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