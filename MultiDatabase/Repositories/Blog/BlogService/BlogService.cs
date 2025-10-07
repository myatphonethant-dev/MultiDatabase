namespace MultiDatabase.Repositories.Blog.BlogService;

public class BlogService : IBlogService
{
    private readonly IBlogRepository _blogRepository;

    public BlogService(IBlogRepository blogRepository)
    {
        _blogRepository = blogRepository;
    }

    public async Task<BlogModel> GetBlogByIdAsync(int id)
    {
        return await _blogRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<BlogModel>> GetAllBlogsAsync()
    {
        return await _blogRepository.GetAllAsync();
    }

    public async Task<IEnumerable<BlogModel>> GetBlogsByAuthorAsync(string author)
    {
        if (string.IsNullOrWhiteSpace(author))
            return await GetAllBlogsAsync();

        return await _blogRepository.GetByAuthorAsync(author);
    }

    public async Task<IEnumerable<BlogModel>> SearchBlogsAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return await GetAllBlogsAsync();

        return await _blogRepository.SearchByTitleAsync(searchTerm);
    }

    public async Task<BlogModel> CreateBlogAsync(BlogModel blog)
    {
        if (blog == null)
            throw new ArgumentNullException(nameof(blog));

        if (string.IsNullOrWhiteSpace(blog.BlogTitle))
            throw new ArgumentException("Blog title is required");

        blog.BlogId = 0;

        return await _blogRepository.AddAsync(blog);
    }

    public async Task<BlogModel> UpdateBlogAsync(BlogModel blog)
    {
        if (blog == null)
            throw new ArgumentNullException(nameof(blog));

        var existingBlog = await _blogRepository.GetByIdAsync(blog.BlogId);
        if (existingBlog == null)
            throw new KeyNotFoundException($"Blog with ID {blog.BlogId} not found");

        await _blogRepository.UpdateAsync(blog);
        return blog;
    }

    public async Task DeleteBlogAsync(int id)
    {
        await _blogRepository.DeleteAsync(id);
    }

    public async Task<bool> BlogExistsAsync(int id)
    {
        return await _blogRepository.ExistsAsync(id);
    }

    public async Task<int> GetBlogCountAsync()
    {
        return await _blogRepository.CountAsync();
    }
}