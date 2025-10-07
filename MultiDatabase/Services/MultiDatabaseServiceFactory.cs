using MultiDatabase.AppDbContext;
using MultiDatabase.Repositories.Blog.BlogRepository;
using MultiDatabase.Repositories.Blog.BlogService;

namespace MultiDatabase.Services;

public interface IMultiDatabaseServiceFactory
{
    IBlogService CreateBlogService(DatabaseType databaseType);
}

public class MultiDatabaseServiceFactory : IMultiDatabaseServiceFactory
{
    private readonly IDatabaseContextFactory _contextFactory;

    public MultiDatabaseServiceFactory(IDatabaseContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public IBlogService CreateBlogService(DatabaseType databaseType)
    {
        var context = _contextFactory.CreateDbContext("TestDb", databaseType);
        var blogRepository = new BlogRepository(context, databaseType);
        return new BlogService(blogRepository);
    }
}