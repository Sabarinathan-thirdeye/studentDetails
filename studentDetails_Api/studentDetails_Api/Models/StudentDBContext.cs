using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace studentDetails_Api.Models;

public partial class StudentDBContext : DbContext
{
    public StudentDBContext()
    {
    }

    public StudentDBContext(DbContextOptions<StudentDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<studentDetail> studentDetails { get; set; }

    public virtual DbSet<userMaster> userMasters { get; set; }

    public virtual DbSet<userType> userTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=StudentCS");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<studentDetail>(entity =>
        {
            entity.HasKey(e => e.studentID).HasName("PK_StudentID_StudentDetails");

            entity.Property(e => e.createdOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.email).HasMaxLength(100);
            entity.Property(e => e.firstName).HasMaxLength(100);
            entity.Property(e => e.gender).HasMaxLength(100);
            entity.Property(e => e.lastName).HasMaxLength(100);
            entity.Property(e => e.modifiedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<userMaster>(entity =>
        {
            entity.HasKey(e => e.userID).HasName("PK_userID_userMaster");

            entity.ToTable("userMaster");

            entity.Property(e => e.createdOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.email).HasMaxLength(100);
            entity.Property(e => e.firstName).HasMaxLength(100);
            entity.Property(e => e.gender).HasMaxLength(100);
            entity.Property(e => e.lastName).HasMaxLength(100);
            entity.Property(e => e.modifiedOn).HasColumnType("datetime");
            entity.Property(e => e.userName).HasMaxLength(100);
            entity.Property(e => e.userPassword).HasMaxLength(100);

            entity.HasOne(d => d.userType).WithMany(p => p.userMasters)
                .HasForeignKey(d => d.userTypeID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_userTypeID_userType");
        });

        modelBuilder.Entity<userType>(entity =>
        {
            entity.HasKey(e => e.userTypeID).HasName("PK_userTypeID_userTypes");

            entity.ToTable("userType");

            entity.Property(e => e.createdOn).HasColumnType("datetime");
            entity.Property(e => e.modifiedOn).HasColumnType("datetime");
            entity.Property(e => e.userTypeName).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
