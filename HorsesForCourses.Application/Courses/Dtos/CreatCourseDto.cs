namespace HorsesForCourses.Application.Courses.Dtos;

public class CreateCourseDto
{
  public string Name { get; set; } = default!;
  public DateOnly StartDate { get; set; }
  public DateOnly EndDate { get; set; }
}