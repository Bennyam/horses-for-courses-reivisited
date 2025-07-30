using HorsesForCourses.Domain;

namespace HorsesForCourses.Application.Courses;

public interface ICourseRepository
{
  void Add(Course course);
  Course? GetById(Guid id);
  IEnumerable<Course> GetAll();
  void Save();
  void ReplaceTimeslots(Guid courseId, List<Timeslot> newTimeslots);
}