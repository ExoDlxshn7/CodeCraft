using Microsoft.AspNetCore.Mvc;
using SubApp1.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Security.Claims;

public class PostController : Controller
{
    private readonly UserDbContext _context;
    private readonly IWebHostEnvironment _env;

    public PostController(UserDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    public IActionResult Index()
    {
        var posts = _context.Posts.Include(p => p.User).ToList();
        return View(posts);
    }

    [HttpPost]
    public IActionResult CreatePost(string PostContent, IFormFile PostImage)
    {
        var post = new Post
        {
            Content = PostContent,
            CreatedAt = DateTime.Now,
            UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
        };

        if (PostImage != null)
        {
            var fileName = "";
            do
            {
                fileName = Path.Combine(_env.WebRootPath, "Images", Path.GetRandomFileName() + DateTime.Now.Ticks + Path.GetExtension(PostImage.FileName));
            } while (System.IO.File.Exists(fileName));

            using (var fileStream = new FileStream(fileName, FileMode.Create))
            {
                PostImage.CopyTo(fileStream);
            }
            post.ImageUrl = "/Images/" + Path.GetFileName(fileName);
        }

        _context.Posts.Add(post);
        _context.SaveChanges();

        return RedirectToAction("Profile", "Home");
    }
}