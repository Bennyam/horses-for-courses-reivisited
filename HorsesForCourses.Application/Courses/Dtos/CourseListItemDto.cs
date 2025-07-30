namespace HorsesForCourses.Application.Courses.Dtos;

public class CourseListItemDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public bool HasSchedule { get; set; }
    public bool HasCoach { get; set; }
}
