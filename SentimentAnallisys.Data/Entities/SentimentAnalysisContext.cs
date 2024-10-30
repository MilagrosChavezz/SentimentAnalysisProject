using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SentimentAnalysis.Data.Entities;

public partial class SentimentAnalysisContext : DbContext
{
    public SentimentAnalysisContext()
    {
    }

    public SentimentAnalysisContext(DbContextOptions<SentimentAnalysisContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-J0GTGC0;Database=SentimentAnalysis;User Id=usuario;Password=password;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Post__3213E83F29927342");

            entity.ToTable("Post");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IsDepression).HasColumnName("is_depression");
            entity.Property(e => e.PostDate).HasColumnName("post_date");
            entity.Property(e => e.Text).HasColumnName("text");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.User).WithMany(p => p.Posts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Post");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3213E83FFCC8376F");

            entity.ToTable("User");

            entity.HasIndex(e => e.Email, "UQ__User__AB6E6164B053E980").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Birthday).HasColumnName("birthday");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Genre)
                .HasMaxLength(50)
                .HasColumnName("genre");
            entity.Property(e => e.UserName)
                .HasMaxLength(255)
                .HasColumnName("userName");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
