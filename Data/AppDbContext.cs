using Microsoft.EntityFrameworkCore;
using SchoolApi.Models;

namespace SchoolApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Student> Students => Set<Student>();
    public DbSet<Teacher> Teachers => Set<Teacher>();
    public DbSet<Grade> Grades => Set<Grade>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Student>(entity =>
        {
            entity.ToTable("Student");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(150);
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.ToTable("Teacher");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(150);
        });

        modelBuilder.Entity<Grade>(entity =>
        {
            entity.ToTable("Grade");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(150);
            entity.Property(e => e.Value).HasColumnType("decimal(5,2)");

            // Foreign key constraint: TeacherId -> Teacher.Id
            entity.HasOne(g => g.Teacher)
                  .WithMany(t => t.Grades)
                  .HasForeignKey(g => g.TeacherId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Foreign key constraint: StudentId -> Student.Id
            entity.HasOne(g => g.Student)
                  .WithMany(s => s.Grades)
                  .HasForeignKey(g => g.StudentId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Seed data so the API can be tested right away
        modelBuilder.Entity<Student>().HasData(
            new Student { Id = 1, Name = "John Smith" },
            new Student { Id = 2, Name = "Emily Johnson" }
        );

        modelBuilder.Entity<Teacher>().HasData(
            new Teacher { Id = 1, Name = "Michael Brown" },
            new Teacher { Id = 2, Name = "Sarah Davis" }
        );

        modelBuilder.Entity<Grade>().HasData(
            new Grade { Id = 1, Name = "Mathematics - Term 1", Value = 4.5m, TeacherId = 1, StudentId = 1 },
            new Grade { Id = 2, Name = "English - Term 1", Value = 3.8m, TeacherId = 2, StudentId = 2 }
        );
    }
}
