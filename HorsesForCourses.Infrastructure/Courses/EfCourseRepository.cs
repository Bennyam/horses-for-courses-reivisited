using HorsesForCourses.Application.Courses;
using HorsesForCourses.Domain;
using HorsesForCourses.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HorsesForCourses.Infrastructure.Courses;

public class EfCourseRepository : ICourseRepository
{
    private readonly HorsesForCoursesDbContext _db;

    public EfCourseRepository(HorsesForCoursesDbContext db)
    {
        _db = db;
    }

    public void Add(Course course)
    {
        _db.Courses.Add(course);
    }

    public Course? GetById(Guid id)
    {
        return _db.Courses
            .Include(c => c.Coach)
            .Include(c => c.Timeslots)
            .FirstOrDefault(c => c.Id == id);
    }

    public IEnumerable<Course> GetAll()
    {
        return _db.Courses
            .Include(c => c.Coach)
            .Include(c => c.Timeslots)
            .ToList();
    }

    public void Save()
    {
        Console.WriteLine(_db.ChangeTracker.DebugView.ShortView);
        _db.SaveChanges();
    }

    public void ReplaceTimeslots(Guid courseId, List<Timeslot> newTimeslots)
    {
        var course = _db.Courses
            .Include(c => c.Timeslots)
            .FirstOrDefault(c => c.Id == courseId);

        if (course is null)
            throw new Exception("Course not found");

        // Verwijder alle oude timeslots uit de database
        var oldSlots = _db.Timeslots.Where(t => t.CourseId == course.Id).ToList();
        _db.Timeslots.RemoveRange(oldSlots);

        // Voeg nieuwe timeslots toe
        foreach (var slot in newTimeslots)
        {
            course.AddTimeslot(slot);
            _db.Entry(slot).State = EntityState.Added; // 💥 zeg EF dat dit echt nieuwe zijn
        }

        _db.SaveChanges();
    }
}
