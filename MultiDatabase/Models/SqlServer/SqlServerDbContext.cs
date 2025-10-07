namespace MultiDatabase.Models.SqlServer;

public partial class SqlServerDbContext : DbContext
{
    public SqlServerDbContext()
    {
    }

    public SqlServerDbContext(DbContextOptions<SqlServerDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblBlog> TblBlogs { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=TestDb;User Id=sa;Password=sasa@123;TrustServerCertificate=true;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblBlog>(entity =>
        {
            entity.HasKey(e => e.BlogId)
                .HasName("Tbl_Blog_pk");

            entity.ToTable("Tbl_Blog");

            entity.Property(e => e.BlogId).ValueGeneratedNever();

            entity.Property(e => e.BlogAuthor)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.BlogContent)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.BlogTitle)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}