using HorsesForCourses.Application.Coaches.Dtos;
using HorsesForCourses.Domain;

namespace HorsesForCourses.Application.Coaches;

public static class CoachMapper
{
    public static CoachDto ToDto(Coach coach)
    {
        return new CoachDto
        {
            Id = coach.Id,
            Name = coach.Name,
            Email = coach.Email,
            Skills = coach.Skills.ToList(),
            Courses = coach.AssignedCourses
                .Select(c => new CourseSummaryDto
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToList()
        };
    }

    public static CoachListItemDto ToListItemDto(Coach coach)
    {
        return new CoachListItemDto
        {
            Id = coach.Id,
            Name = coach.Name,
            Email = coach.Email,
            NumberOfCoursesAssignedTo = coach.NumberOfCoursesAssignedTo
        };
    }
}
