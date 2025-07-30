using HorsesForCourses.Application.Coaches;
using HorsesForCourses.Application.Courses.Dtos;
using HorsesForCourses.Domain;

namespace HorsesForCourses.Application.Courses;

public class CourseService : ICourseService
{
    private readonly ICourseRepository _repo;
    private readonly ICoachRepository _coachRepo;

    public CourseService(ICourseRepository repo, ICoachRepository coachRepo)
    {
        _repo = repo;
        _coachRepo = coachRepo;
    }

    public Guid Create(CreateCourseDto dto)
    {
        var course = new Course(dto.Name, dto.StartDate, dto.EndDate);
        _repo.Add(course);
        _repo.Save();
        return course.Id;
    }

    public void UpdateSkills(Guid courseId, UpdateCourseSkillsDto dto)
    {
        var course = GetOrThrow(courseId);

        foreach (var skill in course.RequiredSkills.ToList())
            course.RemoveRequiredSkill(skill);

        foreach (var skill in dto.Skills)
            course.AddRequiredSkill(skill);

        _repo.Save();
    }

    public void UpdateTimeslots(Guid courseId, UpdateCourseTimeslotsDto dto)
    {
        var newSlots = dto.Timeslots.Select(dtoSlot =>
            new Timeslot(ParseDay(dtoSlot.Day), dtoSlot.Start, dtoSlot.End)
        ).ToList();

        _repo.ReplaceTimeslots(courseId, newSlots);
    }


    public void Confirm(Guid courseId)
    {
        var course = GetOrThrow(courseId);
        course.Confirm();
        _repo.Save();
    }

    public void AssignCoach(Guid courseId, AssignCoachDto dto)
    {
        var course = GetOrThrow(courseId);
        var coach = _coachRepo.GetById(dto.CoachId) ?? throw new Exception("Coach not found");

        course.AssignCoach(coach);
        _repo.Save();
    }

    public CourseDto? GetById(Guid id)
    {
        var course = _repo.GetById(id);
        return course is null ? null : CourseMapper.ToDto(course);
    }

    public IEnumerable<CourseListItemDto> GetAll()
    {
        return _repo.GetAll().Select(CourseMapper.ToListItemDto);
    }

    // Helpers
    private Course GetOrThrow(Guid id)
    {
        return _repo.GetById(id) ?? throw new Exception("Course not found");
    }

    private static DayOfWeek ParseDay(string day)
    {
        return Enum.TryParse<DayOfWeek>(day, true, out var parsed)
            ? parsed
            : throw new ArgumentException($"Invalid day: {day}");
    }
}
