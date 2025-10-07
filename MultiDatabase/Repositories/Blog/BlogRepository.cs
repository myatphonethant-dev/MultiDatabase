using Microsoft.EntityFrameworkCore;

namespace MultiDatabase.Repositories.Blog;

public interface IBlogRepository<T> : IBaseRepository<T> where T : class
{
    Task<IEnumerable<T>> GetByAuthorAsync(string author);
    Task<IEnumerable<T>> SearchByTitleAsync(string searchTerm);
}

public class BlogRepository<T> : BaseRepository<T>, IBlogRepository<T> where T : class
{
    public BlogRepository(DbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<T>> GetByAuthorAsync(string author)
    {
        return await _dbSet
            .Where(b => EF.Property<string>(b, "BlogAuthor") != null &&
                       EF.Property<string>(b, "BlogAuthor").Contains(author))
            .ToListAsync();
    }

    public async Task<IEnumerable<T>> SearchByTitleAsync(string searchTerm)
    {
        return await _dbSet
            .Where(b => EF.Property<string>(b, "BlogTitle") != null &&
                       EF.Property<string>(b, "BlogTitle").Contains(searchTerm))
            .ToListAsync();
    }
}