using HorsesForCourses.Application.Coaches;
using HorsesForCourses.Domain;
using HorsesForCourses.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HorsesForCourses.Infrastructure.Coaches;

public class EfCoachRepository : ICoachRepository
{
    private readonly HorsesForCoursesDbContext _db;

    public EfCoachRepository(HorsesForCoursesDbContext db)
    {
        _db = db;
    }

    public void Add(Coach coach)
    {
        _db.Coaches.Add(coach);
    }

    public Coach? GetById(Guid id)
    {
        return _db.Coaches
            .Include("_assignedCourses")
            .FirstOrDefault(c => c.Id == id);
    }

    public IEnumerable<Coach> GetAll()
    {
        return _db.Coaches
            .Include("_assignedCourses")
            .ToList();
    }

    public void Save()
    {
        _db.ChangeTracker.DetectChanges();
        _db.SaveChanges();
    }
}
