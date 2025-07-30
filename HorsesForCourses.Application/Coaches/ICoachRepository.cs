using HorsesForCourses.Domain;

namespace HorsesForCourses.Application.Coaches;

public interface ICoachRepository
{
    void Add(Coach coach);
    Coach? GetById(Guid id);
    IEnumerable<Coach> GetAll();
    void Save(); // Voor later met EF
}
