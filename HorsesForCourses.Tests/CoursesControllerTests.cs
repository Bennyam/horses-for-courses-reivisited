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
    
  [Fact]
  public void CreateCourse_ReturnsProblem_WhenDtoIsNull()
  {
      var result = _controller.CreateCourse(null!) as ObjectResult;
      Assert.NotNull(result);
      Assert.Equal(500, result!.StatusCode);
  }

  [Fact]
  public void Confirm_ReturnsProblem_WhenCourseNotFound()
  {
      var result = _controller.Confirm(Guid.NewGuid()) as ObjectResult;
      Assert.NotNull(result);
      Assert.Equal(500, result!.StatusCode);
  }

  [Fact]
  public void UpdateSkills_ReturnsProblem_WhenCourseNotFound()
  {
      var dto = new UpdateCourseSkillsDto { Skills = new List<string> { "X" } };
      var result = _controller.UpdateSkills(Guid.NewGuid(), dto) as ObjectResult;

      Assert.NotNull(result);
      Assert.Equal(500, result!.StatusCode);
  }

  [Fact]
  public void UpdateSkills_ReturnsProblem_WhenDtoIsNull()
  {
      var result = _controller.UpdateSkills(Guid.NewGuid(), null!) as ObjectResult;
      Assert.NotNull(result);
      Assert.Equal(500, result!.StatusCode);
  }

  [Fact]
  public void UpdateTimeslots_ReturnsProblem_WhenDtoIsNull()
  {
      var result = _controller.UpdateTimeslots(Guid.NewGuid(), null!) as ObjectResult;
      Assert.NotNull(result);
      Assert.Equal(500, result!.StatusCode);
  }

  [Fact]
  public void UpdateTimeslots_ReturnsProblem_WhenInvalidId()
  {
      var dto = new UpdateCourseTimeslotsDto
      {
          Timeslots = new List<TimeslotDto> {
              new() { Day = "Monday", Start = 9, End = 11 }
          }
      };

      var result = _controller.UpdateTimeslots(Guid.NewGuid(), dto) as ObjectResult;
      Assert.NotNull(result);
      Assert.Equal(500, result!.StatusCode);
  }

  [Fact]
  public void AssignCoach_ReturnsProblem_WhenDtoIsNull()
  {
      var result = _controller.AssignCoach(Guid.NewGuid(), null!) as ObjectResult;
      Assert.NotNull(result);
      Assert.Equal(500, result!.StatusCode);
  }

  [Fact]
  public void AssignCoach_ReturnsProblem_WhenCoachNotFound()
  {
      var courseId = _controller.CreateCourse(new CreateCourseDto
      {
          Name = "Test",
          StartDate = new DateOnly(2025, 10, 1),
          EndDate = new DateOnly(2025, 10, 10)
      }) as OkObjectResult;

      var dto = new AssignCoachDto { CoachId = Guid.NewGuid() };

      var result = _controller.AssignCoach((Guid)courseId!.Value!, dto) as ObjectResult;
      Assert.NotNull(result);
      Assert.Equal(500, result!.StatusCode);
  }

  [Fact]
  public void GetById_ReturnsProblem_WhenCourseNotFound()
  {
      var result = _controller.GetById(Guid.NewGuid());
      Assert.IsType<ObjectResult>(result);
  }
}
