using HorsesForCourses.Application.Coaches;
using HorsesForCourses.Domain;

namespace HorsesForCourses.Infrastructure.Coaches;

public class InMemoryCoachRepository : ICoachRepository
{
    private readonly List<Coach> _coaches = new();

    public void Add(Coach coach)
    {
        _coaches.Add(coach);
    }

    public Coach? GetById(Guid id)
    {
        return _coaches.FirstOrDefault(c => c.Id == id);
    }

    public IEnumerable<Coach> GetAll()
    {
        return _coaches;
    }

    public void Save()
    {
        // In-memory repository does not require saving
    }
}
