using HorsesForCourses.Application.Coaches.Dtos;
using HorsesForCourses.Domain;

namespace HorsesForCourses.Application.Coaches;

public class CoachService : ICoachService
{
    private readonly ICoachRepository _repo;

    public CoachService(ICoachRepository repo)
    {
        _repo = repo;
    }

    public Guid Create(CreateCoachDto dto)
    {
        var coach = new Coach(dto.Name, dto.Email);
        _repo.Add(coach);
        _repo.Save();
        return coach.Id;
    }

    public void UpdateSkills(Guid coachId, UpdateCoachSkillsDto dto)
    {
        var coach = GetOrThrow(coachId);

        // Eerst alles verwijderen
        foreach (var skill in coach.Skills.ToList())
        {
            coach.RemoveSkill(skill);
        }

        // Daarna alles opnieuw toevoegen
        foreach (var skill in dto.Skills)
        {
            coach.AddSkill(skill);
        }

        _repo.Save();
    }

    public CoachDto? GetById(Guid id)
    {
        var coach = _repo.GetById(id);
        return coach is null ? null : CoachMapper.ToDto(coach);
    }

    public IEnumerable<CoachListItemDto> GetAll()
    {
        return _repo.GetAll().Select(CoachMapper.ToListItemDto);
    }

    // Helper
    private Coach GetOrThrow(Guid id)
    {
        return _repo.GetById(id) ?? throw new Exception("Coach not found");
    }
}
