using HorsesForCourses.Domain;

namespace HorsesForCourses.Tests;

public class CoachTests
{
    [Fact]
    public void Constructor_WithValidNameAndEmail_SetsProperties()
    {
        var name = "Ben";
        var email = "ben@coach.be";
        var coach = new Coach(name, email);

        Assert.Equal(name, coach.Name);
        Assert.Equal(email, coach.Email);
        Assert.NotEqual(Guid.Empty, coach.Id);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithInvalidName_Throws(string? invalidName)
    {
        var email = "ben@coach.be";

        Assert.Throws<ArgumentException>(() => new Coach(invalidName!, email));
    }

    [Fact]
    public void Constructor_WithInvalidEmail_Throws()
    {
        var name = "Ben";
        var invalidEmail = "ben_coach_be";

        Assert.Throws<ArgumentException>(() => new Coach(name, invalidEmail));
    }

    [Fact]
    public void AddSkill_AddsUniqueSkill()
    {
        var coach = new Coach("Ben", "ben@coach.be");

        coach.AddSkill("C#");

        Assert.Contains("C#", coach.Skills);
    }

    [Fact]
    public void AddSkill_DoesNotAddDuplicate()
    {
        var coach = new Coach("Ben", "ben@coach.be");
        coach.AddSkill("C#");

        coach.AddSkill("C#");

        Assert.Single(coach.Skills);
    }

    [Fact]
    public void AddSkill_WithInvalidValue_Throws()
    {
        var coach = new Coach("Ben", "ben@coach.be");

        Assert.Throws<ArgumentException>(() => coach.AddSkill(" "));
    }

    [Fact]
    public void RemoveSkill_RemovesExistingSkill()
    {
        var coach = new Coach("Ben", "ben@coach.be");
        coach.AddSkill("C#");

        coach.RemoveSkill("C#");

        Assert.Empty(coach.Skills);
    }

    [Fact]
    public void RemoveSkill_DoesNothing_WhenSkillNotPresent()
    {
        var coach = new Coach("Ben", "ben@coach.be");

        coach.RemoveSkill("C#");

        Assert.Empty(coach.Skills);
    }

    [Fact]
    public void RemoveSkill_WithInvalidValue_Throws()
    {
        var coach = new Coach("Ben", "ben@coach.be");

        Assert.Throws<ArgumentException>(() => coach.RemoveSkill("  "));
    }

    [Fact]
    public void IsSuitableFor_ReturnsTrue_WhenCoachHasAllRequiredSkills()
    {
        var coach = new Coach("Ben", "ben@coach.be");
        coach.AddSkill("C#");
        coach.AddSkill("Agile");

        var course = new Course("Backend", DateOnly.FromDateTime(DateTime.Today), DateOnly.FromDateTime(DateTime.Today.AddDays(10)));
        course.AddRequiredSkill("C#");
        course.AddRequiredSkill("Agile");

        var result = coach.IsSuitableFor(course);

        Assert.True(result);
    }

    [Fact]
    public void IsSuitableFor_ReturnsFalse_WhenCoachMissesSkill()
    {
        var coach = new Coach("Ben", "ben@coach.be");
        coach.AddSkill("C#");

        var course = new Course("Backend", DateOnly.FromDateTime(DateTime.Today), DateOnly.FromDateTime(DateTime.Today.AddDays(10)));
        course.AddRequiredSkill("C#");
        course.AddRequiredSkill("Agile");

        var result = coach.IsSuitableFor(course);

        Assert.False(result);
    }

    [Fact]
    public void IsSuitableFor_WithNullCourse_Throws()
    {
        var coach = new Coach("Ben", "ben@coach.be");

        Assert.Throws<ArgumentNullException>(() => coach.IsSuitableFor(null!));
    }

    [Fact]
    public void IsAvailableFor_ReturnsTrue_WhenNoConflicts()
    {
        var coach = new Coach("Ben", "ben@coach.be");
        coach.AddSkill("C#");

        var oldCourse = new Course("OldCourse", new DateOnly(2025, 1, 1), new DateOnly(2025, 1, 31));
        oldCourse.AddTimeslot(new Timeslot(DayOfWeek.Monday, 9, 11));
        oldCourse.AddRequiredSkill("C#");
        oldCourse.Confirm();
        coach.AssignToCourse(oldCourse);

        var newCourse = new Course("NewCourse", new DateOnly(2025, 2, 1), new DateOnly(2025, 2, 28));
        newCourse.AddTimeslot(new Timeslot(DayOfWeek.Monday, 11, 13));

        var result = coach.IsAvailableFor(newCourse);

        Assert.True(result);
    }

    [Fact]
    public void IsAvailableFor_ReturnsFalse_WhenTimeslotsOverlap()
    {
        var coach = new Coach("Ben", "ben@coach.be");
        coach.AddSkill("C#");

        var oldCourse = new Course("OldCourse", new DateOnly(2025, 1, 1), new DateOnly(2025, 1, 31));
        oldCourse.AddTimeslot(new Timeslot(DayOfWeek.Monday, 9, 11));
        oldCourse.AddRequiredSkill("C#");
        oldCourse.Confirm();
        coach.AssignToCourse(oldCourse);

        var newCourse = new Course("NewCourse", new DateOnly(2025, 2, 1), new DateOnly(2025, 2, 28));
        newCourse.AddTimeslot(new Timeslot(DayOfWeek.Monday, 10, 12)); // overlapt

        var result = coach.IsAvailableFor(newCourse);

        Assert.False(result);
    }

    [Fact]
    public void IsAvailableFor_WithNullCourse_Throws()
    {
        var coach = new Coach("Ben", "ben@coach.be");

        Assert.Throws<ArgumentNullException>(() => coach.IsAvailableFor(null!));
    }
    
    [Fact]
    public void AssignToCourse_AddsCourseToAssignedList()
    {
        var coach = new Coach("Ben", "ben@coach.be");
        coach.AddSkill("C#");

        var course = new Course("Backend", new DateOnly(2025, 1, 1), new DateOnly(2025, 1, 31));
        course.AddRequiredSkill("C#");
        course.AddTimeslot(new Timeslot(DayOfWeek.Monday, 9, 12));
        course.Confirm();

        coach.AssignToCourse(course);

        Assert.Contains(course, coach.AssignedCourses);
    }
}