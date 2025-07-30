namespace HorsesForCourses.Application.Courses.Dtos;

public class CourseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public List<string> Skills { get; set; } = new();
    public List<TimeslotDto> Timeslots { get; set; } = new();
    public CoachSummaryDto? Coach { get; set; }
}

public class CoachSummaryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
}
