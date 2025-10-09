using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MultiDatabase.Models.Postgres;

public partial class PostgresDbContext : DbContext
{
    public PostgresDbContext()
    {
    }

    public PostgresDbContext(DbContextOptions<PostgresDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblBlog> TblBlogs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=testdb;Username=postgres;Password=sasa@123");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblBlog>(entity =>
        {
            entity.HasKey(e => e.BlogId).HasName("tbl_blog_pkey");

            entity.ToTable("tbl_blog");

            entity.Property(e => e.BlogId).HasColumnName("blog_id");
            entity.Property(e => e.BlogAuthor)
                .HasMaxLength(100)
                .HasColumnName("blog_author");
            entity.Property(e => e.BlogContent).HasColumnName("blog_content");
            entity.Property(e => e.BlogTitle)
                .HasMaxLength(100)
                .HasColumnName("blog_title");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
