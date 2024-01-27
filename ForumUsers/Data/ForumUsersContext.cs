using System;
using System.Collections.Generic;
using ForumUsers.Model;
using Microsoft.EntityFrameworkCore;

namespace ForumUsers.Data;

public partial class ForumUsersContext : DbContext
{
    public ForumUsersContext()
    {
    }

    public ForumUsersContext(DbContextOptions<ForumUsersContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-H2OJFG5\\SQLEXPRESS;Initial Catalog=ForumUsers;TrustServerCertificate=True;Encrypt=True;Trusted_Connection=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Banner).HasColumnType("image");
            entity.Property(e => e.ConfirmedEmailDate).HasColumnType("datetime");
            entity.Property(e => e.EmailAddress)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PasswordSalt)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.Photo).HasColumnType("image");
            entity.Property(e => e.RegistrationDate).HasColumnType("datetime").HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.Role)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("User");
            entity.Property(e => e.Username)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e._2fa).HasColumnName("2FA");
            entity.Property(e => e._2fadate)
                .HasColumnType("datetime")
                .HasColumnName("2FADate");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
