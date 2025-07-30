using HorsesForCourses.Application.Coaches;
using HorsesForCourses.Application.Coaches.Dtos;
using HorsesForCourses.Infrastructure.Coaches;
using HorsesForCourses.Domain;

namespace HorsesForCourses.Tests;

public class CoachServiceTests
{
    private readonly ICoachRepository _repo = new InMemoryCoachRepository();
    private readonly CoachService _service;

    public CoachServiceTests()
    {
        _service = new CoachService(_repo);
    }

    [Fact]
    public void Create_AddsCoachToRepository()
    {
        var dto = new CreateCoachDto
        {
            Name = "Ben",
            Email = "ben@coach.be"
        };

        var id = _service.Create(dto);
        var result = _service.GetById(id);

        Assert.NotNull(result);
        Assert.Equal("Ben", result!.Name);
        Assert.Equal("ben@coach.be", result.Email);
    }

    [Fact]
    public void UpdateSkills_ReplacesAllSkills()
    {
        var id = _service.Create(new CreateCoachDto
        {
            Name = "Lynn",
            Email = "lynn@coach.be"
        });

        _service.UpdateSkills(id, new UpdateCoachSkillsDto
        {
            Skills = new List<string> { "C#", "Agile" }
        });

        var result = _service.GetById(id);
        Assert.Equal(new[] { "C#", "Agile" }, result!.Skills);
    }

    [Fact]
    public void GetById_ReturnsNull_WhenCoachNotFound()
    {
        var result = _service.GetById(Guid.NewGuid());
        Assert.Null(result);
    }

    [Fact]
    public void GetAll_ReturnsAllCoaches()
    {
        _service.Create(new CreateCoachDto { Name = "Sofie", Email = "sofie@coach.be" });
        _service.Create(new CreateCoachDto { Name = "Kris", Email = "kris@coach.be" });

        var all = _service.GetAll().ToList();
        Assert.Equal(2, all.Count);
    }
}
