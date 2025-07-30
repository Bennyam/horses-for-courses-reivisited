namespace HorsesForCourses.Domain;

public class Timeslot
{
  public Guid Id { get; private set; }
  public DayOfWeek Day { get; }
  public int StartHour { get; }
  public int EndHour { get; }

  public Timeslot(DayOfWeek day, int startHour, int endHour)
  {
    if (day is DayOfWeek.Saturday or DayOfWeek.Sunday)
      throw new ArgumentException("Timeslots cannot be scheduled on weekends.", nameof(day));

    if (startHour < 9 || startHour > 16)
      throw new ArgumentOutOfRangeException("Start hour must be between 9 and 16.", nameof(startHour));

    if (endHour <= startHour)
      throw new ArgumentOutOfRangeException("End hour must be greater than start hour.", nameof(endHour));

    if (endHour > 17)
      throw new ArgumentOutOfRangeException("End hour must be 17 or earlier.", nameof(endHour));

    Id = Guid.NewGuid();
    Day = day;
    StartHour = startHour;
    EndHour = endHour;
    
  }
}

