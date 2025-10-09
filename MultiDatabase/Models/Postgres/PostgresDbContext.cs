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

    public virtual DbSet<TblBranch> TblBranches { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblBlog>(entity =>
        {
            entity.HasKey(e => e.BlogId).HasName("tbl_blog_pkey");

            entity.ToTable("tbl_blog");

            entity.Property(e => e.BlogId).HasColumnName("blog_id");
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

        modelBuilder.Entity<TblBranch>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tbl_branch_pkey");

            entity.ToTable("tbl_branch");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.Createddatetime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddatetime");
            entity.Property(e => e.Createduser)
                .HasMaxLength(100)
                .HasColumnName("createduser");
            entity.Property(e => e.Delflag)
                .HasDefaultValue(false)
                .HasColumnName("delflag");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Latitude)
                .HasPrecision(15, 8)
                .HasColumnName("latitude");
            entity.Property(e => e.Longitude)
                .HasPrecision(15, 8)
                .HasColumnName("longitude");
            entity.Property(e => e.Map).HasColumnName("map");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Phone)
                .HasMaxLength(255)
                .HasColumnName("phone");
            entity.Property(e => e.Updateddatetime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updateddatetime");
            entity.Property(e => e.Updateduser)
                .HasMaxLength(50)
                .HasColumnName("updateduser");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
