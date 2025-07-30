using HorsesForCourses.Api.Controllers;
using HorsesForCourses.Application.Coaches;
using HorsesForCourses.Application.Coaches.Dtos;
using HorsesForCourses.Infrastructure.Coaches;
using Microsoft.AspNetCore.Mvc;

namespace HorsesForCourses.Tests;

public class CoachesControllerTests
{
    private readonly CoachesController _controller;

    public CoachesControllerTests()
    {
        var repo = new InMemoryCoachRepository();
        var service = new CoachService(repo);
        _controller = new CoachesController(service);
    }

    [Fact]
    public void CreateCoach_ReturnsOk_WithValidId()
    {
        var dto = new CreateCoachDto
        {
            Name = "Ben",
            Email = "ben@coach.be"
        };

        var result = _controller.CreateCoach(dto) as OkObjectResult;

        Assert.NotNull(result);
        Assert.IsType<Guid>(result!.Value);
    }

    [Fact]
    public void UpdateSkills_ReturnsNoContent_WhenValid()
    {
        var createDto = new CreateCoachDto { Name = "Anna", Email = "anna@coach.be" };
        var createResult = _controller.CreateCoach(createDto) as OkObjectResult;
        var id = (Guid)createResult!.Value!;

        var skillsDto = new UpdateCoachSkillsDto { Skills = new List<string> { "C#", "Agile" } };

        var result = _controller.UpdateSkills(id, skillsDto);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public void GetById_ReturnsCoachDto_WhenFound()
    {
        var createDto = new CreateCoachDto { Name = "Tom", Email = "tom@coach.be" };
        var createResult = _controller.CreateCoach(createDto) as OkObjectResult;
        var id = (Guid)createResult!.Value!;

        var result = _controller.GetById(id) as OkObjectResult;

        Assert.NotNull(result);
        Assert.Equal(id, ((CoachDto)result!.Value!).Id);
    }

    [Fact]
    public void GetAllCoaches_ReturnsListOfCoaches()
    {
        _controller.CreateCoach(new CreateCoachDto { Name = "A", Email = "a@x.be" });
        _controller.CreateCoach(new CreateCoachDto { Name = "B", Email = "b@x.be" });

        var result = _controller.GetAllCoaches() as OkObjectResult;

        Assert.NotNull(result);
        var list = result!.Value as IEnumerable<CoachListItemDto>;
        Assert.NotNull(list);
        Assert.True(list!.Count() >= 2);
    }
}
