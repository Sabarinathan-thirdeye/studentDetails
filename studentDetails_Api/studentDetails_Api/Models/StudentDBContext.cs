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

    public virtual DbSet<LogInRequest> LogInRequests { get; set; }

    public virtual DbSet<UsersDetail> UsersDetails { get; set; }

    public virtual DbSet<studentDetail> studentDetails { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=StudentCS");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LogInRequest>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("LogInRequest");

            entity.HasIndex(e => e.Email, "UQ__LogInReq__A9D1053463D73AD0").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(250);
            entity.Property(e => e.Password).HasMaxLength(256);

            entity.HasOne(d => d.student).WithMany()
                .HasForeignKey(d => d.studentID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LogInRequest_StudentDetails");
        });

        modelBuilder.Entity<UsersDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UsersDet__3214EC073765630C");

            entity.HasIndex(e => e.Email, "UQ__UsersDet__A9D10534606A9F34").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(255);
        });

        modelBuilder.Entity<studentDetail>(entity =>
        {
            entity.HasKey(e => e.studentID).HasName("PK_StudentID_StudentDetails");

            entity.HasIndex(e => e.email, "UQ__studentD__AB6E61644AA15293").IsUnique();

            entity.Property(e => e.createdOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.email).HasMaxLength(250);
            entity.Property(e => e.firstName).HasMaxLength(100);
            entity.Property(e => e.lastName).HasMaxLength(100);
            entity.Property(e => e.mobileNumber).HasMaxLength(15);
            entity.Property(e => e.modifiedOn).HasColumnType("datetime");
            entity.Property(e => e.studentPassword).HasMaxLength(250);
            entity.Property(e => e.studentstatus).HasDefaultValueSql("('0')");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
