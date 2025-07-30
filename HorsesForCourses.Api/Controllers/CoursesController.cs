using HorsesForCourses.Application.Courses;
using HorsesForCourses.Application.Courses.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace HorsesForCourses.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly ICourseService _service;

    public CoursesController(ICourseService service)
    {
        _service = service;
    }

    [HttpPost]
    public IActionResult CreateCourse([FromBody] CreateCourseDto dto)
    {
        if (dto is null) return Problem("Ongeldige invoer.");

        var id = _service.Create(dto);
        return Ok(id);
    }

    [HttpGet]
    public IActionResult GetAllCourses()
    {
        var courses = _service.GetAll();
        return Ok(courses);
    }

    [HttpPost("{id}/confirm")]
    public IActionResult Confirm(Guid id)
    {
        try
        {
            _service.Confirm(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpPost("{id}/skills")]
    public IActionResult UpdateSkills(Guid id, [FromBody] UpdateCourseSkillsDto dto)
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

    [HttpPost("{id}/timeslots")]
    public IActionResult UpdateTimeslots(Guid id, [FromBody] UpdateCourseTimeslotsDto dto)
    {
        if (dto is null) return Problem("Ongeldige invoer.");

        try
        {
            _service.UpdateTimeslots(id, dto);
            return NoContent();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpPost("{id}/assign-coach")]
    public IActionResult AssignCoach(Guid id, [FromBody] AssignCoachDto dto)
    {
        if (dto is null) return Problem("Ongeldige invoer.");

        try
        {
            _service.AssignCoach(id, dto);
            return NoContent();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        var course = _service.GetById(id);
        return course is null ? Problem("Course not found") : Ok(course);
    }

}
