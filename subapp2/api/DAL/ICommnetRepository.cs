using subapp2.Models;

namespace subapp2.DAL
{
      public interface ICommentRepository
    {
        Task AddCommentAsync(Comment comment);
        Task<Comment> GetCommentByIdAsync(int commentId);
        Task<IEnumerable<Comment>> GetCommentsByPostIdAsync(int postId);
        Task UpdateCommentAsync(Comment comment);
        Task DeleteCommentAsync(int commentId);
    }
}
