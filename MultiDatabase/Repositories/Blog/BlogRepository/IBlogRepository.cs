using MultiDatabase.Models.Common;

namespace MultiDatabase.Repositories.Blog.BlogRepository;

public interface IBlogRepository
{
    Task<BlogModel> GetByIdAsync(int id);
    Task<IEnumerable<BlogModel>> GetAllAsync();
    Task<IEnumerable<BlogModel>> GetByAuthorAsync(string author);
    Task<IEnumerable<BlogModel>> SearchByTitleAsync(string searchTerm);
    Task<BlogModel> AddAsync(BlogModel blog);
    Task UpdateAsync(BlogModel blog);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<int> CountAsync();
}