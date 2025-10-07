using Microsoft.EntityFrameworkCore;
using MultiDatabase.AppDbContext;
using MultiDatabase.Models.Common;

namespace MultiDatabase.Repositories.Blog.BlogRepository;

public class BlogRepository : IBlogRepository
{
    private readonly DbContext _context;
    private readonly DatabaseType _databaseType;

    public BlogRepository(DbContext context, DatabaseType databaseType)
    {
        _context = context;
        _databaseType = databaseType;
    }

    public async Task<BlogModel> GetByIdAsync(int id)
    {
        if (_databaseType == DatabaseType.SqlServer)
        {
            var sqlBlog = await _context.Set<SqlServer.TblBlog>().FindAsync(id);
            return sqlBlog == null ? null : MapToCommon(sqlBlog);
        }
        else
        {
            var postgresBlog = await _context.Set<Postgres.TblBlog>().FindAsync(id);
            return postgresBlog == null ? null : MapToCommon(postgresBlog);
        }
    }

    public async Task<IEnumerable<BlogModel>> GetAllAsync()
    {
        if (_databaseType == DatabaseType.SqlServer)
        {
            var sqlBlogs = await _context.Set<SqlServer.TblBlog>().ToListAsync();
            return sqlBlogs.Select(MapToCommon);
        }
        else
        {
            var postgresBlogs = await _context.Set<Postgres.TblBlog>().ToListAsync();
            return postgresBlogs.Select(MapToCommon);
        }
    }

    public async Task<IEnumerable<BlogModel>> GetByAuthorAsync(string author)
    {
        if (_databaseType == DatabaseType.SqlServer)
        {
            var sqlBlogs = await _context.Set<SqlServer.TblBlog>()
                .Where(b => b.BlogAuthor != null && b.BlogAuthor.Contains(author))
                .ToListAsync();
            return sqlBlogs.Select(MapToCommon);
        }
        else
        {
            var postgresBlogs = await _context.Set<Postgres.TblBlog>()
                .Where(b => b.BlogAuthor != null && b.BlogAuthor.Contains(author))
                .ToListAsync();
            return postgresBlogs.Select(MapToCommon);
        }
    }

    public async Task<IEnumerable<BlogModel>> SearchByTitleAsync(string searchTerm)
    {
        if (_databaseType == DatabaseType.SqlServer)
        {
            var sqlBlogs = await _context.Set<SqlServer.TblBlog>()
                .Where(b => b.BlogTitle != null && b.BlogTitle.Contains(searchTerm))
                .ToListAsync();
            return sqlBlogs.Select(MapToCommon);
        }
        else
        {
            var postgresBlogs = await _context.Set<Postgres.TblBlog>()
                .Where(b => b.BlogTitle != null && b.BlogTitle.Contains(searchTerm))
                .ToListAsync();
            return postgresBlogs.Select(MapToCommon);
        }
    }

    public async Task<BlogModel> AddAsync(BlogModel blog)
    {
        if (_databaseType == DatabaseType.SqlServer)
        {
            var sqlBlog = MapToSqlServer(blog);
            await _context.Set<SqlServer.TblBlog>().AddAsync(sqlBlog);
            await _context.SaveChangesAsync();
            blog.BlogId = sqlBlog.BlogId;
            return blog;
        }
        else
        {
            var postgresBlog = MapToPostgres(blog);
            await _context.Set<Postgres.TblBlog>().AddAsync(postgresBlog);
            await _context.SaveChangesAsync();
            blog.BlogId = postgresBlog.BlogId;
            return blog;
        }
    }

    public async Task UpdateAsync(BlogModel blog)
    {
        if (_databaseType == DatabaseType.SqlServer)
        {
            var sqlBlog = MapToSqlServer(blog);
            _context.Set<SqlServer.TblBlog>().Update(sqlBlog);
            await _context.SaveChangesAsync();
        }
        else
        {
            var postgresBlog = MapToPostgres(blog);
            _context.Set<Postgres.TblBlog>().Update(postgresBlog);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        if (_databaseType == DatabaseType.SqlServer)
        {
            var sqlBlog = await _context.Set<SqlServer.TblBlog>().FindAsync(id);
            if (sqlBlog != null)
            {
                _context.Set<SqlServer.TblBlog>().Remove(sqlBlog);
                await _context.SaveChangesAsync();
            }
        }
        else
        {
            var postgresBlog = await _context.Set<Postgres.TblBlog>().FindAsync(id);
            if (postgresBlog != null)
            {
                _context.Set<Postgres.TblBlog>().Remove(postgresBlog);
                await _context.SaveChangesAsync();
            }
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        if (_databaseType == DatabaseType.SqlServer)
        {
            return await _context.Set<SqlServer.TblBlog>().AnyAsync(b => b.BlogId == id);
        }
        else
        {
            return await _context.Set<Postgres.TblBlog>().AnyAsync(b => b.BlogId == id);
        }
    }

    public async Task<int> CountAsync()
    {
        if (_databaseType == DatabaseType.SqlServer)
        {
            return await _context.Set<SqlServer.TblBlog>().CountAsync();
        }
        else
        {
            return await _context.Set<Postgres.TblBlog>().CountAsync();
        }
    }

    // Mapping methods
    private BlogModel MapToCommon(SqlServer.TblBlog sqlBlog)
    {
        return new BlogModel
        {
            BlogId = sqlBlog.BlogId,
            BlogTitle = sqlBlog.BlogTitle,
            BlogAuthor = sqlBlog.BlogAuthor,
            BlogContent = sqlBlog.BlogContent
        };
    }

    private Blog MapToCommon(Postgres.TblBlog postgresBlog)
    {
        return new BlogModel
        {
            BlogId = postgresBlog.BlogId,
            BlogTitle = postgresBlog.BlogTitle,
            BlogAuthor = postgresBlog.BlogAuthor,
            BlogContent = postgresBlog.BlogContent
        };
    }

    private SqlServer.TblBlog MapToSqlServer(BlogModel blog)
    {
        return new SqlServer.TblBlog
        {
            BlogId = blog.BlogId,
            BlogTitle = blog.BlogTitle,
            BlogAuthor = blog.BlogAuthor,
            BlogContent = blog.BlogContent
        };
    }

    private Postgres.TblBlog MapToPostgres(BlogModel blog)
    {
        return new Postgres.TblBlog
        {
            BlogId = blog.BlogId,
            BlogTitle = blog.BlogTitle,
            BlogAuthor = blog.BlogAuthor,
            BlogContent = blog.BlogContent
        };
    }
}