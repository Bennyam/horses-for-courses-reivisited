using System.ComponentModel.DataAnnotations.Schema;

namespace HorsesForCourses.Domain;

public class Coach
{
  public Guid Id { get; private set; }
  public string Name { get; private set; }
  public string Email { get; private set; }

  private readonly List<string> _skills = new();
  private readonly List<Course> _assignedCourses = new();

  public IReadOnlyCollection<string> Skills => _skills.AsReadOnly();

  [NotMapped]
  public IReadOnlyCollection<Course> AssignedCourses => _assignedCourses.AsReadOnly();
  public int NumberOfCoursesAssignedTo => _assignedCourses.Count;

  private Coach()
  {
    Id = Guid.NewGuid();
    Name = string.Empty;
    Email = string.Empty;
    _skills = new();
    _assignedCourses = new();
   } // EF Core constructor
   
  public Coach(string name, string email)
  {
    if (string.IsNullOrWhiteSpace(name))
      throw new ArgumentException("Name cannot be empty.", nameof(name));
    if (string.IsNullOrWhiteSpace(email))
      throw new ArgumentException("Email cannot be empty.", nameof(email));
    if (!email.Contains("@"))
      throw new ArgumentException("Email must be a valid email address.", nameof(email));

    Id = Guid.NewGuid();
    Name = name;
    Email = email;
  }

  public void AddSkill(string skill)
  {
    if (string.IsNullOrWhiteSpace(skill))
      throw new ArgumentException("Skill cannot be empty.", nameof(skill));

    if (!_skills.Contains(skill))
      _skills.Add(skill);
  }

  public void RemoveSkill(string skill)
  {
    if (string.IsNullOrWhiteSpace(skill))
      throw new ArgumentException("Skill cannot be empty.", nameof(skill));

    if (_skills.Contains(skill))
      _skills.Remove(skill);
  }

  public bool IsSuitableFor(Course course)
  {
    if (course == null)
      throw new ArgumentNullException(nameof(course));

    return course.RequiredSkills.All(Skills => _skills.Contains(Skills));
  }

  public bool IsAvailableFor(Course course)
  {
    if (course is null)
      throw new ArgumentNullException(nameof(course));

    foreach (var assigned in _assignedCourses)
    {
      foreach (var aSlot in assigned.Timeslots)
      {
        foreach (var newSlot in course.Timeslots)
        {
          if (OverlapsWith(aSlot, newSlot))
            return false;
        }
      }
    }
    return true;
  }

  public bool OverlapsWith(Timeslot a, Timeslot b)
  {
    if (a.Day != b.Day)
      return false;

    return a.StartHour < b.EndHour && b.StartHour < a.EndHour;
  }

  public void AssignToCourse(Course course)
  {
    if (course is null)
      throw new ArgumentNullException(nameof(course));

    if (!IsSuitableFor(course))
      throw new InvalidOperationException("Coach is not suitable for this course.");

    if (!IsAvailableFor(course))
      throw new InvalidOperationException("Coach is not available for this course.");

    _assignedCourses.Add(course);
  }
}

