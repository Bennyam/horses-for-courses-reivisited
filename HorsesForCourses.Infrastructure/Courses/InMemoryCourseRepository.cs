using HorsesForCourses.Application.Courses;
using HorsesForCourses.Domain;

namespace HorsesForCourses.Infrastructure.Courses;

public class InMemoryCourseRepository : ICourseRepository
{
    private readonly List<Course> _courses = new();

    public void Add(Course course)
    {
        _courses.Add(course);
    }

    public Course? GetById(Guid id)
    {
        return _courses.FirstOrDefault(c => c.Id == id);
    }

    public IEnumerable<Course> GetAll()
    {
        return _courses;
    }

    public void Save()
    {
    // In-memory repository does not require saving
    }
}
