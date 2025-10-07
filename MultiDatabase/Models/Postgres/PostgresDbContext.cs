using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MultiDatabase.Models.Postgres
{
    public partial class PostgresDbContext : DbContext
    {
        public PostgresDbContext()
        {
        }

        public PostgresDbContext(DbContextOptions<PostgresDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblBlog> TblBlogs { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host=localhost;Database=testdb;Username=postgres;Password=your_password");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TblBlog>(entity =>
            {
                entity.HasKey(e => e.BlogId)
                    .HasName("tbl_blog_pkey");

                entity.ToTable("tbl_blog");

                entity.Property(e => e.BlogId)
                    .ValueGeneratedNever()
                    .HasColumnName("blog_id");

                entity.Property(e => e.BlogAuthor)
                    .HasMaxLength(50)
                    .HasColumnName("blog_author");

                entity.Property(e => e.BlogContent)
                    .HasMaxLength(50)
                    .HasColumnName("blog_content");

                entity.Property(e => e.BlogTitle)
                    .HasMaxLength(50)
                    .HasColumnName("blog_title");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}