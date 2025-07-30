using HorsesForCourses.Application.Coaches.Dtos;

namespace HorsesForCourses.Application.Coaches;

public interface ICoachService
{
    Guid Create(CreateCoachDto dto);
    void UpdateSkills(Guid coachId, UpdateCoachSkillsDto dto);
    CoachDto? GetById(Guid id);
    IEnumerable<CoachListItemDto> GetAll();
}
