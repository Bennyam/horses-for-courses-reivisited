using HorsesForCourses.Application.Courses;
using HorsesForCourses.Application.Courses.Dtos;
using HorsesForCourses.Infrastructure.Courses;
using HorsesForCourses.Infrastructure.Coaches;
using HorsesForCourses.Api.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace HorsesForCourses.Tests;

public class CoursesControllerTests
{
    private readonly CoursesController _controller;

    public CoursesControllerTests()
    {
        var courseRepo = new InMemoryCourseRepository();
        var coachRepo = new InMemoryCoachRepository();

        var service = new CourseService(courseRepo, coachRepo);
        _controller = new CoursesController(service);
    }

    [Fact]
    public void CreateCourse_ReturnsOk_WithValidId()
    {
        var dto = new CreateCourseDto
        {
            Name = "Test Course",
            StartDate = new DateOnly(2025, 10, 1),
            EndDate = new DateOnly(2025, 10, 10)
        };

        var result = _controller.CreateCourse(dto) as OkObjectResult;

        Assert.NotNull(result);
        Assert.IsType<Guid>(result!.Value);
    }

    [Fact]
    public void Confirm_ReturnsNoContent_WhenCourseExists()
    {
        var dto = new CreateCourseDto
        {
            Name = "Course to Confirm",
            StartDate = new DateOnly(2025, 10, 1),
            EndDate = new DateOnly(2025, 10, 10)
        };

        var createResult = _controller.CreateCourse(dto) as OkObjectResult;
        var courseId = (Guid)createResult!.Value!;

        _controller.UpdateSkills(courseId, new UpdateCourseSkillsDto
        {
            Skills = new List<string> { "C#" }
        });

        _controller.UpdateTimeslots(courseId, new UpdateCourseTimeslotsDto
        {
            Timeslots = new List<TimeslotDto>
            {
                new() { Day = "Monday", Start = 9, End = 11 }
            }
        });

        var result = _controller.Confirm(courseId);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public void GetAllCourses_ReturnsOk_WithList()
    {
        var result = _controller.GetAllCourses() as OkObjectResult;

        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<CourseListItemDto>>(result!.Value);
    }
}
