using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GradeSystem.Models;

public partial class DBContext : DbContext
{
    public DBContext()
    {
    }

    public DBContext(DbContextOptions<DBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<CourseHasStudent> CourseHasStudents { get; set; }

    public virtual DbSet<Professor> Professors { get; set; }

    public virtual DbSet<Secretary> Secretaries { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LAPTOP-5JQTS1GA\\SQLEXPRESS;Database=GradeSystemDB;Trusted_Connection=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.IdCourse).HasName("course_pk");

            entity.Property(e => e.IdCourse).ValueGeneratedNever();

            entity.HasOne(d => d.Professor).WithMany(p => p.Courses).HasConstraintName("course_professors_AFM_fk");
        });

        modelBuilder.Entity<CourseHasStudent>(entity =>
        {
            entity.HasKey(e => new { e.IdCourse, e.RegistrationNumber }).HasName("PK__course_h__EF0D1337FF1B6994");

            entity.HasOne(d => d.Course).WithMany(p => p.CourseHasStudents)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("course_has_students_course_idCOURSE_fk");

            entity.HasOne(d => d.Student).WithMany(p => p.CourseHasStudents)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("course_has_students_students_RegistrationNumber_fk");
        });

        modelBuilder.Entity<Professor>(entity =>
        {
            entity.HasKey(e => e.Afm).HasName("professors_pk");

            entity.Property(e => e.Afm).ValueGeneratedNever();

            entity.HasOne(d => d.User).WithMany(p => p.Professors)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("professors_users_username_fk");
        });

        modelBuilder.Entity<Secretary>(entity =>
        {
            entity.HasKey(e => e.Phonenumber).HasName("secretaries_pk");

            entity.Property(e => e.Phonenumber).ValueGeneratedNever();

            entity.HasOne(d => d.User).WithMany(p => p.Secretaries)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("secretaries_users_username_fk");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.RegistrationNumber).HasName("PK__students__E886460334FF009F");

            entity.Property(e => e.RegistrationNumber).ValueGeneratedNever();

            entity.HasOne(d => d.User).WithMany(p => p.Students)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__students__userna__625A9A57");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Username).HasName("users_pk");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
