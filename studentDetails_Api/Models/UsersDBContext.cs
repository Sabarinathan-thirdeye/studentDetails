using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace UsersDetailsApi.Models;

public partial class UsersDBContext : DbContext
{
    public UsersDBContext()
    {
    }

    public UsersDBContext(DbContextOptions<UsersDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserTypeMaster> UserTypeMasters { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=UsersCS");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserID).HasName("PK_Users_UserID");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.FirstName).HasMaxLength(250);
            entity.Property(e => e.LastName).HasMaxLength(250);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.Password).HasMaxLength(250);
            entity.Property(e => e.UserEmail).HasMaxLength(250);
            entity.Property(e => e.UserName).HasMaxLength(250);
            entity.Property(e => e.UserTypeStatus).HasDefaultValueSql("('0')");
        });

        modelBuilder.Entity<UserTypeMaster>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("UserTypeMaster");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.UserTypeName).HasMaxLength(250);
            entity.Property(e => e.UserTypeStatus).HasDefaultValueSql("('0')");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
