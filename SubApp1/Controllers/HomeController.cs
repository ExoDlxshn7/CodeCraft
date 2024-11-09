using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SubApp1.Models;
using Microsoft.EntityFrameworkCore;

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
        return View();
    }

    public IActionResult Profile()
    {
        var posts = _context.Posts.Include(p => p.User).ToList();
        return View(posts);
    }


    public IActionResult signup()
    {
        return View();
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}