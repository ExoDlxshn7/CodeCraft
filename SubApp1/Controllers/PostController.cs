using Microsoft.AspNetCore.Mvc;
using SubApp1.DAL;
using SubApp1.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace SubApp1.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly IWebHostEnvironment _env;

        public PostController(IPostRepository postRepository, IWebHostEnvironment env)
        {
            _postRepository = postRepository;
            _env = env;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePostIndex(string PostContent, IFormFile? PostImage)
        {
            if (string.IsNullOrEmpty(PostContent))
            {
                ModelState.AddModelError("PostContent", "Post content is required.");
                return View(new Post { Content = PostContent });
            }

            var post = new Post
            {
                Content = PostContent,
                CreatedAt = DateTime.Now,
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
            };

            if (PostImage != null)
            {
                var fileName = Path.Combine(_env.WebRootPath, "Images", Path.GetRandomFileName() + Path.GetExtension(PostImage.FileName));
                using (var fileStream = new FileStream(fileName, FileMode.Create))
                {
                    await PostImage.CopyToAsync(fileStream);
                }
                post.ImageUrl = "/Images/" + Path.GetFileName(fileName);
            }

            await _postRepository.AddPostAsync(post);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> CreatePostProfile(string PostContent, IFormFile PostImage)
        {
            var post = new Post
            {
                Content = PostContent,
                CreatedAt = DateTime.Now,
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
            };

            if (PostImage != null)
            {
                var fileName = Path.Combine(_env.WebRootPath, "Images", Path.GetRandomFileName() + Path.GetExtension(PostImage.FileName));
                using (var fileStream = new FileStream(fileName, FileMode.Create))
                {
                    await PostImage.CopyToAsync(fileStream);
                }
                post.ImageUrl = "/Images/" + Path.GetFileName(fileName);
            }

            await _postRepository.AddPostAsync(post);
            return RedirectToAction("Profile", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> GetPost(int id)
        {
            var post = await _postRepository.GetPostByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> EditPost(int id)
        {
            var post = await _postRepository.GetPostByIdAsync(id);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (post == null || post.UserId != userId)
            {
                return Unauthorized();
            }

            return View("~/Views/Home/EditPost.cshtml", post);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditPost(int id, string postContent, IFormFile? postImage)
        {
            var post = await _postRepository.GetPostByIdAsync(id);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (post == null || post.UserId != userId)
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
                    await postImage.CopyToAsync(fileStream);
                }
            }

            await _postRepository.UpdatePostAsync(post);
            return RedirectToAction("Profile", "Home");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeletePost(int id)
        {
            var post = await _postRepository.GetPostByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (post.UserId != userId)
            {
                return Unauthorized();
            }

            await _postRepository.DeletePostAsync(post);
            return RedirectToAction("Profile", "Home");
        }
    }
}