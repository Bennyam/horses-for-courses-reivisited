namespace HorsesForCourses.Application.Coaches.Dtos;

public class CoachDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public List<string> Skills { get; set; } = new();
    public List<CourseSummaryDto> Courses { get; set; } = new();
}

public class CourseSummaryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
}
