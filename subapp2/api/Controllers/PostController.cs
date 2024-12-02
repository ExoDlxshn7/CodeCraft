using Microsoft.AspNetCore.Mvc;
using subapp2.DAL;
using subapp2.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace subapp2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        private readonly IWebHostEnvironment _env;

        public PostController(IPostRepository postRepository, IWebHostEnvironment env)
        {
            _postRepository = postRepository;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            var posts = await _postRepository.GetAllPostsAsync();
            return Ok(posts);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreatePost([FromForm] string PostContent, [FromForm] IFormFile PostImage)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            var post = new Post
            {
                Content = PostContent,
                CreatedAt = DateTime.Now,
                UserId = userId
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
            return CreatedAtAction(nameof(GetPostById), new { id = post.Id }, post);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostById(int id)
        {
            var post = await _postRepository.GetPostByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            return Ok(post);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditPost(int id, [FromForm] string PostContent, [FromForm] IFormFile PostImage)
        {
            var post = await _postRepository.GetPostByIdAsync(id);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (post == null || post.UserId != userId)
            {
                return Unauthorized();
            }

            post.Content = PostContent;

            if (PostImage != null)
            {
                var fileName = Path.Combine(_env.WebRootPath, "Images", Path.GetRandomFileName() + Path.GetExtension(PostImage.FileName));
                using (var fileStream = new FileStream(fileName, FileMode.Create))
                {
                    await PostImage.CopyToAsync(fileStream);
                }
                post.ImageUrl = "/Images/" + Path.GetFileName(fileName);
            }

            await _postRepository.UpdatePostAsync(post);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var post = await _postRepository.GetPostByIdAsync(id);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (post == null || post.UserId != userId)
            {
                return Unauthorized();
            }

            await _postRepository.DeletePostAsync(post);
            return NoContent();
        }
    }
}
