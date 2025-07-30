namespace HorsesForCourses.Application.Coaches.Dtos;

public class CoachListItemDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public int NumberOfCoursesAssignedTo { get; set; }
}
