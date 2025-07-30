using HorsesForCourses.Application.Courses.Dtos;

namespace HorsesForCourses.Application.Courses;

public interface ICourseService
{
  Guid Create(CreateCourseDto dto);
  void UpdateSkills(Guid courseId, UpdateCourseSkillsDto dto);
  void UpdateTimeslots(Guid courseId, UpdateCourseTimeslotsDto dto);
  void Confirm(Guid courseId);
  void AssignCoach(Guid courseId, AssignCoachDto dto);
  CourseDto? GetById(Guid id);
  IEnumerable<CourseListItemDto> GetAll();
}