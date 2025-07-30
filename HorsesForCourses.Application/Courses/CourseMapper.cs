using HorsesForCourses.Application.Courses.Dtos;
using HorsesForCourses.Domain;

namespace HorsesForCourses.Application.Courses;

public static class CourseMapper
{
    public static CourseDto ToDto(Course course)
    {
        return new CourseDto
        {
            Id = course.Id,
            Name = course.Name,
            StartDate = course.StartDate,
            EndDate = course.EndDate,
            Skills = course.RequiredSkills.ToList(),
            Timeslots = course.Timeslots.Select(ts => new TimeslotDto
            {
                Day = ts.Day.ToString(),
                Start = ts.StartHour,
                End = ts.EndHour
            }).ToList(),
            Coach = course.Coach is null ? null : new CoachSummaryDto
            {
                Id = course.Coach.Id,
                Name = course.Coach.Name
            }
        };
    }

    public static CourseListItemDto ToListItemDto(Course course)
    {
        return new CourseListItemDto
        {
            Id = course.Id,
            Name = course.Name,
            StartDate = course.StartDate,
            EndDate = course.EndDate,
            HasSchedule = course.Timeslots.Any(),
            HasCoach = course.Coach is not null
        };
    }
}
