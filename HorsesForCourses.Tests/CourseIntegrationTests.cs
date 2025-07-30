using HorsesForCourses.Domain;

namespace HorsesForCourses.Tests;

public class IntegrationTests
{
  [Fact]
  public void CoachAssignedToConfirmedCourse_UpdatesCoachAndCourse()
  {
    var course = new Course("WebDev", new DateOnly(2025, 2, 1), new DateOnly(2025, 2, 28));
    course.AddRequiredSkill("HTML");
    course.AddTimeslot(new Timeslot(DayOfWeek.Monday, 9, 12));
    course.Confirm();

    var coach = new Coach("Ben", "ben@coach.be");
    coach.AddSkill("HTML");

    course.AssignCoach(coach);

    Assert.Equal(coach, course.Coach);                       // Course verwijst naar coach
    Assert.Contains(course, coach.AssignedCourses);         // Coach heeft course
  }

  [Fact]
  public void CoachCannotBeAssigned_WhenTimeslotOverlaps()
  {
    var coach = new Coach("Ben", "ben@coach.be");
    coach.AddSkill("C#");

    var oldCourse = new Course("CSharp", new DateOnly(2025, 1, 1), new DateOnly(2025, 1, 31));
    oldCourse.AddRequiredSkill("C#");
    oldCourse.AddTimeslot(new Timeslot(DayOfWeek.Monday, 9, 12));
    oldCourse.Confirm();
    coach.AssignToCourse(oldCourse);

    var newCourse = new Course("Blazor", new DateOnly(2025, 2, 1), new DateOnly(2025, 2, 28));
    newCourse.AddRequiredSkill("C#");
    newCourse.AddTimeslot(new Timeslot(DayOfWeek.Monday, 11, 13)); // overlapt
    newCourse.Confirm();

    Assert.Throws<InvalidOperationException>(() => newCourse.AssignCoach(coach));
  }

  [Fact]
  public void CourseConfirm_WithMissingSkill_AndTimeslot_Throws()
  {
    var course = new Course("AI", new DateOnly(2025, 5, 1), new DateOnly(2025, 5, 30));

    var ex1 = Assert.Throws<InvalidOperationException>(() => course.Confirm());
    Assert.Equal("Cannot confirm a course without timeslots.", ex1.Message);

    course.AddTimeslot(new Timeslot(DayOfWeek.Tuesday, 10, 12));
    var ex2 = Assert.Throws<InvalidOperationException>(() => course.Confirm());
    Assert.Equal("Cannot confirm a course without required skills.", ex2.Message);
  }
}
