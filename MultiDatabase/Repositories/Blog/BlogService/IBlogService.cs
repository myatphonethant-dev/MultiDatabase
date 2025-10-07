namespace MultiDatabase.Repositories.Blog.BlogService;

public interface IBlogService
{
    Task<BlogModel> GetBlogByIdAsync(int id);

    Task<IEnumerable<BlogModel>> GetAllBlogsAsync();

    Task<IEnumerable<BlogModel>> GetBlogsByAuthorAsync(string author);

    Task<IEnumerable<BlogModel>> SearchBlogsAsync(string searchTerm);

    Task<BlogModel> CreateBlogAsync(BlogModel blog);

    Task<BlogModel> UpdateBlogAsync(BlogModel blog);

    Task DeleteBlogAsync(int id);

    Task<bool> BlogExistsAsync(int id);

    Task<int> GetBlogCountAsync();
}