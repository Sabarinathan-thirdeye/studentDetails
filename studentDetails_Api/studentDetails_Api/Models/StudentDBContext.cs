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

    public virtual DbSet<LoginRequest> LoginRequests { get; set; }

    public virtual DbSet<studentDetail> studentDetails { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=StudentCS");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LoginRequest>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("LoginRequest");

            entity.Property(e => e.studentPassword).HasMaxLength(100);
            entity.Property(e => e.userName).HasMaxLength(100);
        });

        modelBuilder.Entity<studentDetail>(entity =>
        {
            entity.HasKey(e => e.studentID).HasName("PK_StudentID_StudentDetails");

            entity.Property(e => e.createdOn).HasColumnType("datetime");
            entity.Property(e => e.email).HasMaxLength(100);
            entity.Property(e => e.gender).HasMaxLength(100);
            entity.Property(e => e.modifiedOn).HasColumnType("datetime");
            entity.Property(e => e.studentName).HasMaxLength(100);
            entity.Property(e => e.studentPassword).HasMaxLength(100);
            entity.Property(e => e.userName).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
