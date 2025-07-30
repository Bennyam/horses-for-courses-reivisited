using HorsesForCourses.Domain;
using HorsesForCourses.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace HorsesForCourses.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DebugController : ControllerBase
{
    private readonly HorsesForCoursesDbContext _db;

    public DebugController(HorsesForCoursesDbContext db)
    {
        _db = db;
    }

    [HttpPost("timeslot-test")]
    public IActionResult CreateCourseWithTimeslot()
    {
        var course = new Course("Test Course", new DateOnly(2025, 10, 1), new DateOnly(2025, 10, 10));
        course.AddRequiredSkill("C#");
        course.AddTimeslot(new Timeslot(DayOfWeek.Monday, 9, 11));
        course.Confirm();

        _db.Courses.Add(course);
        _db.SaveChanges(); // 🧨 Hier faalt EF als er écht iets mis is

        return Ok(course.Id);
    }
}
