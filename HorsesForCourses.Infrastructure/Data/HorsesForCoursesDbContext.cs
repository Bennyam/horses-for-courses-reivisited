using HorsesForCourses.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace HorsesForCourses.Infrastructure.Data;

public class HorsesForCoursesDbContext : DbContext
{
    public HorsesForCoursesDbContext(DbContextOptions<HorsesForCoursesDbContext> options)
        : base(options)
    {
    }

    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Coach> Coaches => Set<Coach>();
    public DbSet<Timeslot> Timeslots => Set<Timeslot>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Timeslot>()
            .HasKey(t => t.Id);

        modelBuilder.Entity<Timeslot>()
            .HasOne<Course>()
            .WithMany(c => c.Timeslots)
            .HasForeignKey("CourseId")
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Course>()
            .Ignore(c => c.RequiredSkills);

        modelBuilder.Entity<Course>()
            .Property<List<string>>("_requiredSkills")
            .HasColumnName("RequiredSkills")
            .HasConversion(
                v => string.Join(';', v),
                v => v.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList()
            )
            .Metadata.SetValueComparer(new ValueComparer<List<string>>(
                (c1, c2) => (c1 ?? new()).SequenceEqual(c2 ?? new()),
                c => c == null ? 0 : c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c == null ? new List<string>() : c.ToList()
            ));

        modelBuilder.Entity<Coach>()
            .Ignore(c => c.Skills);

        modelBuilder.Entity<Coach>()
            .Property<List<string>>("_skills")
            .HasColumnName("Skills")
            .HasConversion(
                v => string.Join(';', v),
                v => v.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList()
            )
            .Metadata.SetValueComparer(new ValueComparer<List<string>>(
                (c1, c2) => (c1 ?? new()).SequenceEqual(c2 ?? new()),
                c => c == null ? 0 : c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c == null ? new List<string>() : c.ToList()
            ));

        modelBuilder.Entity<Coach>()
            .HasMany(typeof(Course), "_assignedCourses")
            .WithOne("Coach")
            .HasForeignKey("CoachId")
            .OnDelete(DeleteBehavior.SetNull);
    }
}
