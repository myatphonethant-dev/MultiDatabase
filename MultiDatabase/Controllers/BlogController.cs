using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiDatabase.AppDbContext;
using MultiDatabase.Models.Common;
using MultiDatabase.Services;
using System.Reflection.Metadata;

namespace MultiDatabase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IMultiDatabaseServiceFactory _serviceFactory;
        private readonly ILogger<BlogController> _logger;

        public BlogController(IMultiDatabaseServiceFactory serviceFactory, ILogger<BlogController> logger)
        {
            _serviceFactory = serviceFactory;
            _logger = logger;
        }

        [HttpGet("{databaseType}")]
        public async Task<ActionResult<IEnumerable<BlogModel>>> GetBlogs(DatabaseType databaseType)
        {
            try
            {
                var service = _serviceFactory.CreateBlogService(databaseType);
                var blogs = await service.GetAllBlogsAsync();
                return Ok(blogs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting blogs from {DatabaseType}", databaseType);
                return StatusCode(500, $"Error retrieving blogs: {ex.Message}");
            }
        }

        [HttpGet("{databaseType}/{id}")]
        public async Task<ActionResult<BlogModel>> GetBlog(DatabaseType databaseType, int id)
        {
            try
            {
                var service = _serviceFactory.CreateBlogService(databaseType);
                var blog = await service.GetBlogByIdAsync(id);

                if (blog == null)
                    return NotFound($"Blog with ID {id} not found");

                return Ok(blog);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting blog {BlogId} from {DatabaseType}", id, databaseType);
                return StatusCode(500, $"Error retrieving blog: {ex.Message}");
            }
        }

        [HttpGet("{databaseType}/author/{author}")]
        public async Task<ActionResult<IEnumerable<BlogModel>>> GetBlogsByAuthor(DatabaseType databaseType, string author)
        {
            try
            {
                var service = _serviceFactory.CreateBlogService(databaseType);
                var blogs = await service.GetBlogsByAuthorAsync(author);
                return Ok(blogs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting blogs by author {Author} from {DatabaseType}", author, databaseType);
                return StatusCode(500, $"Error retrieving blogs by author: {ex.Message}");
            }
        }

        [HttpGet("{databaseType}/search/{searchTerm}")]
        public async Task<ActionResult<IEnumerable<BlogModel>>> SearchBlogs(DatabaseType databaseType, string searchTerm)
        {
            try
            {
                var service = _serviceFactory.CreateBlogService(databaseType);
                var blogs = await service.SearchBlogsAsync(searchTerm);
                return Ok(blogs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching blogs with term {SearchTerm} from {DatabaseType}", searchTerm, databaseType);
                return StatusCode(500, $"Error searching blogs: {ex.Message}");
            }
        }

        [HttpPost("{databaseType}")]
        public async Task<ActionResult<BlogModel>> CreateBlog(DatabaseType databaseType, BlogModel blog)
        {
            try
            {
                var service = _serviceFactory.CreateBlogService(databaseType);
                var createdBlog = await service.CreateBlogAsync(blog);
                return CreatedAtAction(nameof(GetBlog), new { databaseType, id = createdBlog.BlogId }, createdBlog);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating blog in {DatabaseType}", databaseType);
                return StatusCode(500, $"Error creating blog: {ex.Message}");
            }
        }

        [HttpPut("{databaseType}/{id}")]
        public async Task<IActionResult> UpdateBlog(DatabaseType databaseType, int id, BlogModel blog)
        {
            try
            {
                if (id != blog.BlogId)
                    return BadRequest("Blog ID mismatch");

                var service = _serviceFactory.CreateBlogService(databaseType);
                await service.UpdateBlogAsync(blog);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating blog {BlogId} in {DatabaseType}", id, databaseType);
                return StatusCode(500, $"Error updating blog: {ex.Message}");
            }
        }

        [HttpDelete("{databaseType}/{id}")]
        public async Task<IActionResult> DeleteBlog(DatabaseType databaseType, int id)
        {
            try
            {
                var service = _serviceFactory.CreateBlogService(databaseType);
                await service.DeleteBlogAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting blog {BlogId} from {DatabaseType}", id, databaseType);
                return StatusCode(500, $"Error deleting blog: {ex.Message}");
            }
        }

        [HttpGet("{databaseType}/count")]
        public async Task<ActionResult<int>> GetBlogCount(DatabaseType databaseType)
        {
            try
            {
                var service = _serviceFactory.CreateBlogService(databaseType);
                var count = await service.GetBlogCountAsync();
                return Ok(count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting blog count from {DatabaseType}", databaseType);
                return StatusCode(500, $"Error getting blog count: {ex.Message}");
            }
        }

        [HttpGet("{databaseType}/exists/{id}")]
        public async Task<ActionResult<bool>> BlogExists(DatabaseType databaseType, int id)
        {
            try
            {
                var service = _serviceFactory.CreateBlogService(databaseType);
                var exists = await service.BlogExistsAsync(id);
                return Ok(exists);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if blog {BlogId} exists in {DatabaseType}", id, databaseType);
                return StatusCode(500, $"Error checking blog existence: {ex.Message}");
            }
        }
    }
}
