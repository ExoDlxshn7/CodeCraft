using System.Security.Claims;
using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Hosting;
using SubApp1.Models;

namespace SubApp1.Controllers
{
    public class ProfileController : Controller
    {
       private readonly ILogger<ProfileController> _logger; 
       private readonly UserDbContext _userDbcontext;
       private readonly IWebHostEnvironment _hostingEnvironment;
        public ProfileController(UserDbContext userDbContext, IWebHostEnvironment hostingEnvironment, ILogger<ProfileController> logger){
              _userDbcontext = userDbContext;
              _hostingEnvironment = hostingEnvironment;
              _logger = logger;

        }
    
        public IActionResult Index(){
            var friends = _userDbcontext.Friends.ToList();
            return View(friends);
        }

        public IActionResult Friends(){
            var friends = _userDbcontext.Friends.ToList();
            return View("~/Views/Home/Friends.cshtml",friends);
        }

        [HttpGet]
        public IActionResult RemoveFriend(int id){
            var friend = _userDbcontext.Friends.Find(id);
             if(friend == null){
                return NotFound();
            }
            return View(friend);
        }

        [HttpPost]

        public IActionResult RemoveFriendConfirmed(int id){
            var friend = _userDbcontext.Friends.Find(id);
            if(friend == null){
                return NotFound();
            }
            _userDbcontext.Friends.Remove(friend);
            _userDbcontext.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Edit profile
        [HttpGet]
           public IActionResult Edit(int id){
        var user = _userDbcontext.Users.Find(id);
        if (user == null)
        {
            return NotFound();
        }
        return View(user);
        }
   

        // POST: Edit profile
        [HttpPost]
        public IActionResult EditProfile(int id, User user)
{
    var userToUpdate = _userDbcontext.Users.Find(id);
    if (userToUpdate == null)
    {
        return NotFound();
    }

    // Update the user properties
    userToUpdate.Username = user.Username;
    // Continue for all properties

    _userDbcontext.Users.Update(userToUpdate);
    _userDbcontext.SaveChanges();

    return RedirectToAction("Index");
}


[HttpPost]
public IActionResult CreatePost(string PostContent, IFormFile PostImage)
{
    _logger.LogInformation("[ItemController] Item list not found while executing _itemRepository.GetAll()");
    _logger.LogWarning("[ItemController] Item list not found while executing _itemRepository.GetAll()");
    _logger.LogError("[ItemController] Item list not found while executing _itemRepository.GetAll()");

    if (string.IsNullOrEmpty(PostContent) && PostImage == null)
    {
        // Handle error or return the form with validation message
        return RedirectToAction("ProfilePage"); // Redirect or reload the page
    }

    // Get the current logged-in user ID
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

     // Process the image upload
    string imageUrl = null;
    if (PostImage != null && PostImage.Length > 0)
    {
        var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
        var filePath = Path.Combine(uploads, PostImage.FileName);
        
        if (!Directory.Exists(uploads))
        {
            Directory.CreateDirectory(uploads);
        }

    }

     // Create the new post
    var post = new Post
    {
        Content = PostContent,
        CreatedAt = DateTime.Now,
        UserId = int.Parse(userId),
        ImageUrl = imageUrl  // Store image URL in the post
    };

        _userDbcontext.Posts.Add(post);  // Assuming _userDbcontext has a Posts DbSet
        _userDbcontext.SaveChanges();

        return RedirectToAction("ProfilePage");
}

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return RedirectToAction("Index", "Login");
            }
    }
}