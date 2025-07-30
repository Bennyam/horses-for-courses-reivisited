using HorsesForCourses.Application.Coaches;
using HorsesForCourses.Application.Coaches.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace HorsesForCourses.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoachesController : ControllerBase
{
    private readonly ICoachService _service;

    public CoachesController(ICoachService service)
    {
        _service = service;
    }

    [HttpPost]
    public IActionResult CreateCoach([FromBody] CreateCoachDto dto)
    {
        if (dto is null) return Problem("Ongeldige invoer.");

        var id = _service.Create(dto);
        return Ok(id);
    }

    [HttpPost("{id}/skills")]
    public IActionResult UpdateSkills(Guid id, [FromBody] UpdateCoachSkillsDto dto)
    {
        if (dto is null) return Problem("Ongeldige invoer.");

        try
        {
            _service.UpdateSkills(id, dto);
            return NoContent();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet]
    public IActionResult GetAllCoaches()
    {
        var coaches = _service.GetAll();
        return Ok(coaches);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        var coach = _service.GetById(id);
        return coach is null ? Problem("Coach not found") : Ok(coach);
    }
}
