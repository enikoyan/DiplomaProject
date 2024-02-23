using System;
using System.Collections.Generic;
using EdManagementSystem.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace EdManagementSystem.DataAccess.Data;

public partial class User004Context : DbContext
{
    public User004Context()
    {
    }

    public User004Context(DbContextOptions<User004Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Squad> Squads { get; set; }

    public virtual DbSet<SquadStudent> SquadStudents { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PRIMARY");
        });

        modelBuilder.Entity<Squad>(entity =>
        {
            entity.HasKey(e => e.SquadId).HasName("PRIMARY");

            entity.HasOne(d => d.IdCourseNavigation).WithMany(p => p.Squads).HasConstraintName("Squad_ibfk_1");
        });

        modelBuilder.Entity<SquadStudent>(entity =>
        {
            entity.HasOne(d => d.IdSquadNavigation).WithMany().HasConstraintName("SquadStudent_ibfk_1");

            entity.HasOne(d => d.IdStudentNavigation).WithMany().HasConstraintName("SquadStudent_ibfk_2");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PRIMARY");

            entity.Property(e => e.Rate).HasDefaultValueSql("'4'");
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.Property(e => e.Rate).HasDefaultValueSql("'4'");

            entity.HasOne(d => d.TeacherNavigation).WithMany().HasConstraintName("Teacher_ibfk_1");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.Property(e => e.UserPassword).IsFixedLength();
            entity.Property(e => e.UserRole).HasDefaultValueSql("'teacher'");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
