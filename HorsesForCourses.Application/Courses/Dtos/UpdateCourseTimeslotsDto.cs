namespace HorsesForCourses.Application.Courses.Dtos;

public class UpdateCourseTimeslotsDto
{
    public List<TimeslotDto> Timeslots { get; set; } = new();
}

public class TimeslotDto
{
    public string Day { get; set; } = default!;
    public int Start { get; set; }
    public int End { get; set; }
}
