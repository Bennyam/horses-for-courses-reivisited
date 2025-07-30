using HorsesForCourses.Application.Courses;
using HorsesForCourses.Application.Courses.Dtos;
using HorsesForCourses.Application.Coaches;
using HorsesForCourses.Infrastructure.Courses;
using HorsesForCourses.Infrastructure.Coaches;
using HorsesForCourses.Domain;

namespace HorsesForCourses.Tests;

public class CourseServiceTests
{
    private readonly ICourseRepository _courseRepo = new InMemoryCourseRepository();
    private readonly ICoachRepository _coachRepo = new InMemoryCoachRepository();
    private readonly CourseService _service;

    public CourseServiceTests()
    {
        _service = new CourseService(_courseRepo, _coachRepo);
    }

    [Fact]
    public void Create_AddsCourseToRepository()
    {
        var dto = new CreateCourseDto
        {
            Name = "C# Basics",
            StartDate = new DateOnly(2025, 9, 1),
            EndDate = new DateOnly(2025, 9, 10)
        };

        var id = _service.Create(dto);
        var result = _service.GetById(id);

        Assert.NotNull(result);
        Assert.Equal("C# Basics", result!.Name);
    }

    [Fact]
    public void UpdateSkills_ReplacesAllSkills()
    {
        var id = _service.Create(new CreateCourseDto
        {
            Name = "Agile",
            StartDate = new DateOnly(2025, 10, 1),
            EndDate = new DateOnly(2025, 10, 10)
        });

        _service.UpdateSkills(id, new UpdateCourseSkillsDto
        {
            Skills = new List<string> { "Scrum", "Teamwork" }
        });

        var result = _service.GetById(id);
        Assert.Equal(new[] { "Scrum", "Teamwork" }, result!.Skills);
    }

    [Fact]
    public void UpdateTimeslots_ReplacesAllTimeslots()
    {
        var id = _service.Create(new CreateCourseDto
        {
            Name = "Planning",
            StartDate = new DateOnly(2025, 11, 1),
            EndDate = new DateOnly(2025, 11, 10)
        });

        _service.UpdateTimeslots(id, new UpdateCourseTimeslotsDto
        {
            Timeslots = new List<TimeslotDto>
            {
                new() { Day = "Monday", Start = 9, End = 11 },
                new() { Day = "Wednesday", Start = 10, End = 12 }
            }
        });

        var result = _service.GetById(id);
        Assert.Equal(2, result!.Timeslots.Count);
    }

    [Fact]
    public void Confirm_MarksCourseAsConfirmed()
    {
        var id = _service.Create(new CreateCourseDto
        {
            Name = "Security",
            StartDate = new DateOnly(2025, 12, 1),
            EndDate = new DateOnly(2025, 12, 10)
        });

        _service.UpdateSkills(id, new UpdateCourseSkillsDto
        {
            Skills = new List<string> { "Encryption" }
        });

        _service.UpdateTimeslots(id, new UpdateCourseTimeslotsDto
        {
            Timeslots = new List<TimeslotDto>
            {
                new() { Day = "Tuesday", Start = 9, End = 11 }
            }
        });

        _service.Confirm(id);

        var result = _service.GetById(id);
        Assert.True(result!.Coach == null); 
    }

    [Fact]
    public void AssignCoach_LinksCoachToCourse()
    {
        var coach = new Coach("Ben", "ben@coach.be");
        coach.AddSkill("Blazor");
        _coachRepo.Add(coach);

        var id = _service.Create(new CreateCourseDto
        {
            Name = "Blazor Advanced",
            StartDate = new DateOnly(2026, 1, 1),
            EndDate = new DateOnly(2026, 1, 15)
        });

        _service.UpdateSkills(id, new UpdateCourseSkillsDto
        {
            Skills = new List<string> { "Blazor" }
        });

        _service.UpdateTimeslots(id, new UpdateCourseTimeslotsDto
        {
            Timeslots = new List<TimeslotDto>
            {
                new() { Day = "Monday", Start = 9, End = 11 }
            }
        });

        _service.Confirm(id);
        _service.AssignCoach(id, new AssignCoachDto { CoachId = coach.Id });

        var course = _service.GetById(id);
        Assert.Equal(coach.Id, course!.Coach!.Id);
    }

    [Fact]
    public void GetAll_ReturnsAllCourses()
    {
        _service.Create(new CreateCourseDto
        {
            Name = "X",
            StartDate = new DateOnly(2025, 5, 1),
            EndDate = new DateOnly(2025, 5, 5)
        });

        _service.Create(new CreateCourseDto
        {
            Name = "Y",
            StartDate = new DateOnly(2025, 6, 1),
            EndDate = new DateOnly(2025, 6, 5)
        });

        var all = _service.GetAll().ToList();
        Assert.Equal(2, all.Count);
    }
}
