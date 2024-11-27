using Microsoft.AspNetCore.Mvc;
using SubApp1.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;

namespace SubApp1.Controllers
{
    public class PostController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly UserDbContext _userDbcontext;

        public PostController(UserDbContext userDbcontext, IWebHostEnvironment env, ILogger<PostController> logger)
        {
            _userDbcontext = userDbcontext;
            _env = env;
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

            _userDbcontext.Posts.Add(post);
            _userDbcontext.SaveChanges();

            return RedirectToAction("Profile", "Home");
        }

        // GET: Show the Edit Post page
            [Authorize]
            [HttpGet]
            public IActionResult EditPost(int id)
            {
                var post = _userDbcontext.Posts.Find(id);
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (post == null || string.IsNullOrEmpty(userId) || post.UserId != userId)
                {
                    return Unauthorized();
                }

                return View("~/Views/Home/EditPost.cshtml", post);
            }

            // POST: Confirm editing a post
            [Authorize]
            [HttpPost]
            public IActionResult EditPost(int id, string postContent, IFormFile postImage)
            {
                var post = _userDbcontext.Posts.Find(id);
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (post == null || string.IsNullOrEmpty(userId) || post.UserId != userId)
                {
                    return Unauthorized();
                }

                post.Content = postContent;

                if (postImage != null && postImage.Length > 0)
                {
                    var uploads = Path.Combine(_env.WebRootPath, "uploads");
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

                return RedirectToAction("Profile", "Home");
            }

            // POST: Delete a post
            [Authorize]
            [HttpPost]
            public IActionResult DeletePost(int id)
            {
                var post = _userDbcontext.Posts.Find(id);
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (post == null || string.IsNullOrEmpty(userId) || post.UserId != userId)
                {
                    return Unauthorized();
                }

                _userDbcontext.Posts.Remove(post);
                _userDbcontext.SaveChanges();

                return RedirectToAction("Profile", "Home");
            }
     }
}