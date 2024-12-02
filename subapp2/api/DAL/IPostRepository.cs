using subapp2.Models;

namespace subapp2.DAL
{   
    public interface IPostRepository
    {
        Task AddPostAsync(Post post);
        Task<Post> GetPostByIdAsync(int id);
        Task<IEnumerable<Post>> GetPostsByUserIdAsync(string userId);
        Task UpdatePostAsync(Post post);
        Task DeletePostAsync(Post post);
    }
}
