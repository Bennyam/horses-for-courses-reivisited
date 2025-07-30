namespace HorsesForCourses.Domain;

public class Course
{
  public Guid Id { get; private set; }
  public string Name { get; private set; }
  public DateOnly StartDate { get; private set; }
  public DateOnly EndDate { get; private set; }

  private readonly List<Timeslot> _timeslots = new();
  private readonly List<string> _requiredSkills = new();

  public IReadOnlyCollection<Timeslot> Timeslots => _timeslots.AsReadOnly();
  public IReadOnlyCollection<string> RequiredSkills => _requiredSkills.AsReadOnly();

  public Coach? Coach { get; private set; }
  public bool IsConfirmed { get; private set; }

  public Course(string name, DateOnly startDate, DateOnly endDate)
  {
    if (string.IsNullOrWhiteSpace(name))
      throw new ArgumentException("Name cannot be empty.", nameof(name));

    if (endDate < startDate)
      throw new ArgumentException("End date must be after start date.", nameof(startDate));

    if (startDate == default)
      throw new ArgumentException("Start date cannot be default value.", nameof(startDate));

    if (endDate == default)
      throw new ArgumentException("End date cannot be default value.", nameof(endDate));

    Id = Guid.NewGuid();
    Name = name;
    StartDate = startDate;
    EndDate = endDate;
  }

  public void AddRequiredSkill(string skill)
  {
    if (string.IsNullOrWhiteSpace(skill))
      throw new ArgumentException("Skill cannot be empty.", nameof(skill));

    if (IsConfirmed)
      throw new InvalidOperationException("Cannot add skills to a confirmed course.");

    if (_requiredSkills.Contains(skill))
      return;

    _requiredSkills.Add(skill);
  }

  public void RemoveRequiredSkill(string skill)
  {
    if (string.IsNullOrWhiteSpace(skill))
      throw new ArgumentException("Skill cannot be empty.", nameof(skill));

    if (IsConfirmed)
      throw new InvalidOperationException("Cannot remove skills from a confirmed course.");

    if (_requiredSkills.Contains(skill))
      _requiredSkills.Remove(skill);
  }

  public void AddTimeslot(Timeslot timeslot)
  {
    if (timeslot == null)
      throw new ArgumentNullException(nameof(timeslot));

    if (IsConfirmed)
      throw new InvalidOperationException("Cannot add timeslots to a confirmed course.");

    if (_timeslots.Any(ts => ts.Day == timeslot.Day && ts.StartHour == timeslot.StartHour && ts.EndHour == timeslot.EndHour))
      return;

    if (_timeslots.Any(ts => ts.Day == timeslot.Day && (ts.StartHour < timeslot.EndHour && timeslot.StartHour < ts.EndHour)))
      throw new InvalidOperationException("Timeslot overlaps with an existing timeslot.");

    _timeslots.Add(timeslot);
  }

  public void RemoveTimeslot(Guid timeslotId)
  {
    if (IsConfirmed)
      throw new InvalidOperationException("Cannot remove timeslots from a confirmed course.");

    var timeslot = _timeslots.FirstOrDefault(ts => ts.Id == timeslotId);
    if (timeslot is not null)
      _timeslots.Remove(timeslot);
  }

  public void Confirm()
  {
    if (IsConfirmed)
      throw new InvalidOperationException("Course is already confirmed.");

    if (!_timeslots.Any())
      throw new InvalidOperationException("Cannot confirm a course without timeslots.");

    if (!RequiredSkills.Any())
      throw new InvalidOperationException("Cannot confirm a course without required skills.");

    IsConfirmed = true;
  }

  public void AssignCoach(Coach coach)
  {
    if (!IsConfirmed)
      throw new InvalidOperationException("Cannot assign a coach to an unconfirmed course.");

    if (Coach is not null)
      throw new InvalidOperationException("Course already has a coach assigned.");

    if (!coach.IsSuitableFor(this))
      throw new InvalidOperationException("Coach is not suitable for this course.");

    if (!coach.IsAvailableFor(this))
      throw new InvalidOperationException("Coach is not available for this course.");

    Coach = coach;
    coach.AssignToCourse(this);
  }
}