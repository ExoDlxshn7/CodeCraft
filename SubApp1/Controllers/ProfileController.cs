using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;
using System;
using SubApp1.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace SubApp1.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ILogger<ProfileController> _logger;
        private readonly UserDbContext _userDbcontext;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ProfileController(UserDbContext userDbContext, IWebHostEnvironment hostingEnvironment, ILogger<ProfileController> logger)
        {
            _userDbcontext = userDbContext;
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }

        // Display posts by the logged-in user
        [Authorize]
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var userPosts = _userDbcontext.Posts
                .Where(p => p.UserId == int.Parse(userId))
                .OrderByDescending(p => p.CreatedAt)
                .ToList();

            return View("Profile", userPosts);
        }

        // Show the list of friends
        public IActionResult Friends()
        {
            var friends = _userDbcontext.Friends.ToList();
            return View("~/Views/Home/Friends.cshtml", friends);
        }

        // GET: Show Remove Friend page
        [Authorize]
        [HttpGet]
        public IActionResult RemoveFriend(int id)
        {
            var friend = _userDbcontext.Friends.Find(id);
            if (friend == null)
            {
                return NotFound();
            }
            return View(friend);
        }

        // POST: Confirm removing a friend
        [Authorize]
        [HttpPost]
        public IActionResult RemoveFriendConfirmed(int id)
        {
            var friend = _userDbcontext.Friends.Find(id);
            if (friend == null)
            {
                return NotFound();
            }
            _userDbcontext.Friends.Remove(friend);
            _userDbcontext.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Show the Edit Profile page
        [Authorize]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var user = _userDbcontext.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Update profile information
        [Authorize]
        [HttpPost]
        public IActionResult EditProfile(int id, User user)
        {
            var userToUpdate = _userDbcontext.Users.Find(id);
            if (userToUpdate == null)
            {
                return NotFound();
            }

            userToUpdate.Username = user.Username;
            userToUpdate.Email = user.Email;
            // Add additional properties as needed

            _userDbcontext.Users.Update(userToUpdate);
            _userDbcontext.SaveChanges();

            return RedirectToAction("Index");
        }

        // POST: Create a new post
        [Authorize]
        [HttpPost]
        public IActionResult CreatePost(string postContent, IFormFile postImage)
        {
            if (string.IsNullOrEmpty(postContent) && postImage == null)
            {
                ModelState.AddModelError("", "Content or an image is required.");
                return RedirectToAction("Index");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            string? imageUrl = null;
            if (postImage != null && postImage.Length > 0)
            {
                var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                var filePath = Path.Combine(uploads, postImage.FileName);
                imageUrl = $"/uploads/{postImage.FileName}";

                if (!Directory.Exists(uploads))
                {
                    Directory.CreateDirectory(uploads);
                }

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    postImage.CopyTo(fileStream);
                }
            }

            var post = new Post
            {
                Content = postContent,
                CreatedAt = DateTime.Now,
                UserId = int.Parse(userId),
                ImageUrl = imageUrl
            };

            _userDbcontext.Posts.Add(post);
            _userDbcontext.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: Show the Edit Post page
        [Authorize]
        [HttpGet]
        public IActionResult EditPost(int id)
        {
            var post = _userDbcontext.Posts.Find(id);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (post == null || string.IsNullOrEmpty(userId) || post.UserId != int.Parse(userId))
            {
                return Unauthorized();
            }

            return View(post);
        }

        // POST: Confirm editing a post
        [Authorize]
        [HttpPost]
        public IActionResult EditPost(int id, string postContent, IFormFile postImage)
        {
            var post = _userDbcontext.Posts.Find(id);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (post == null || string.IsNullOrEmpty(userId) || post.UserId != int.Parse(userId))
            {
                return Unauthorized();
            }

            post.Content = postContent;

            if (postImage != null && postImage.Length > 0)
            {
                var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                var filePath = Path.Combine(uploads, postImage.FileName);
                post.ImageUrl = $"/uploads/{postImage.FileName}";

                if (!Directory.Exists(uploads))
                {
                    Directory.CreateDirectory(uploads);
                }

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    postImage.CopyTo(fileStream);
                }
            }

            _userDbcontext.Posts.Update(post);
            _userDbcontext.SaveChanges();

            return RedirectToAction("Index");
        }

        // POST: Delete a post
        [Authorize]
        [HttpPost]
        public IActionResult DeletePost(int id)
        {
            var post = _userDbcontext.Posts.Find(id);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (post == null || string.IsNullOrEmpty(userId) || post.UserId != int.Parse(userId))
            {
                return Unauthorized();
            }

            _userDbcontext.Posts.Remove(post);
            _userDbcontext.SaveChanges();

            return RedirectToAction("Index");
        }

        // POST: Logout
           [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return RedirectToAction("Index", "Login");
        }
    }
}
