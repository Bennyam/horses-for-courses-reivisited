using HorsesForCourses.Domain;

namespace HorsesForCourses.Tests;

public class CourseTests
{
    // --- Constructor ---
    [Fact]
    public void Constructor_WithValidData_SetsProperties()
    {
        var course = new Course("Agile", new DateOnly(2025, 1, 1), new DateOnly(2025, 1, 10));

        Assert.Equal("Agile", course.Name);
        Assert.NotEqual(Guid.Empty, course.Id);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_WithInvalidName_Throws(string name)
    {
        var start = new DateOnly(2025, 1, 1);
        var end = new DateOnly(2025, 1, 10);

        Assert.Throws<ArgumentException>(() => new Course(name, start, end));
    }

    [Fact]
    public void Constructor_WithEndDateBeforeStart_Throws()
    {
        var start = new DateOnly(2025, 1, 10);
        var end = new DateOnly(2025, 1, 1);

        Assert.Throws<ArgumentException>(() => new Course("Test", start, end));
    }

    [Fact]
    public void Constructor_WithDefaultDates_Throws()
    {
        var end = new DateOnly(2025, 1, 10);

        Assert.Throws<ArgumentException>(() => new Course("Test", default, end));
        Assert.Throws<ArgumentException>(() => new Course("Test", end, default));
    }

    // --- Required Skills ---
    [Fact]
    public void AddRequiredSkill_AddsSkill_WhenValid()
    {
        var course = CreateBasicCourse();
        course.AddRequiredSkill("C#");

        Assert.Contains("C#", course.RequiredSkills);
    }

    [Fact]
    public void AddRequiredSkill_DoesNotAddDuplicates()
    {
        var course = CreateBasicCourse();
        course.AddRequiredSkill("C#");
        course.AddRequiredSkill("C#");

        Assert.Single(course.RequiredSkills);
    }

    [Fact]
    public void AddRequiredSkill_WithInvalidValue_Throws()
    {
        var course = CreateBasicCourse();

        Assert.Throws<ArgumentException>(() => course.AddRequiredSkill(" "));
    }

    [Fact]
    public void AddRequiredSkill_WhenConfirmed_Throws()
    {
        var course = CreateConfirmedCourse();

        Assert.Throws<InvalidOperationException>(() => course.AddRequiredSkill("C#"));
    }

    [Fact]
    public void RemoveRequiredSkill_RemovesSkill()
    {
        var course = CreateBasicCourse();
        course.AddRequiredSkill("C#");
        course.RemoveRequiredSkill("C#");

        Assert.Empty(course.RequiredSkills);
    }

    [Fact]
    public void RemoveRequiredSkill_WhenConfirmed_Throws()
    {
        var course = CreateConfirmedCourse();

        Assert.Throws<InvalidOperationException>(() => course.RemoveRequiredSkill("C#"));
    }

    // --- Timeslots ---
    [Fact]
    public void AddTimeslot_AddsUniqueTimeslot()
    {
        var course = CreateBasicCourse();
        var timeslot = new Timeslot(DayOfWeek.Monday, 9, 11);

        course.AddTimeslot(timeslot);

        Assert.Contains(timeslot, course.Timeslots);
    }

    [Fact]
    public void AddTimeslot_DuplicateTimeslot_DoesNotAdd()
    {
        var course = CreateBasicCourse();
        var slot1 = new Timeslot(DayOfWeek.Monday, 9, 11);
        var slot2 = new Timeslot(DayOfWeek.Monday, 9, 11);

        course.AddTimeslot(slot1);
        course.AddTimeslot(slot2);

        Assert.Single(course.Timeslots);
    }

    [Fact]
    public void AddTimeslot_WithOverlap_Throws()
    {
        var course = CreateBasicCourse();
        course.AddTimeslot(new Timeslot(DayOfWeek.Monday, 9, 11));

        Assert.Throws<InvalidOperationException>(() => course.AddTimeslot(new Timeslot(DayOfWeek.Monday, 10, 12)));
    }

    [Fact]
    public void AddTimeslot_WhenConfirmed_Throws()
    {
        var course = CreateConfirmedCourse();
        var timeslot = new Timeslot(DayOfWeek.Tuesday, 10, 12);

        Assert.Throws<InvalidOperationException>(() => course.AddTimeslot(timeslot));
    }

    [Fact]
    public void RemoveTimeslot_RemovesSlotById()
    {
        var course = CreateBasicCourse();
        var timeslot = new Timeslot(DayOfWeek.Monday, 9, 11);
        course.AddTimeslot(timeslot);

        course.RemoveTimeslot(timeslot.Id);

        Assert.Empty(course.Timeslots);
    }

    [Fact]
    public void RemoveTimeslot_WhenConfirmed_Throws()
    {
        var course = CreateConfirmedCourse();

        Assert.Throws<InvalidOperationException>(() => course.RemoveTimeslot(Guid.NewGuid()));
    }

    // --- Confirm Course ---
    [Fact]
    public void Confirm_SetsIsConfirmed_WhenValid()
    {
        var course = CreateBasicCourse();
        course.AddRequiredSkill("C#");
        course.AddTimeslot(new Timeslot(DayOfWeek.Monday, 9, 11));

        course.Confirm();

        Assert.True(course.IsConfirmed);
    }

    [Fact]
    public void Confirm_WhenAlreadyConfirmed_Throws()
    {
        var course = CreateConfirmedCourse();

        Assert.Throws<InvalidOperationException>(() => course.Confirm());
    }

    [Fact]
    public void Confirm_WithoutTimeslots_Throws()
    {
        var course = CreateBasicCourse();
        course.AddRequiredSkill("C#");

        Assert.Throws<InvalidOperationException>(() => course.Confirm());
    }

    [Fact]
    public void Confirm_WithoutSkills_Throws()
    {
        var course = CreateBasicCourse();
        course.AddTimeslot(new Timeslot(DayOfWeek.Monday, 9, 11));

        Assert.Throws<InvalidOperationException>(() => course.Confirm());
    }

    // --- Assign Coach ---
    [Fact]
    public void AssignCoach_AssignsCoachSuccessfully()
    {
        var course = CreateBasicCourse();
        course.AddRequiredSkill("C#");
        course.AddTimeslot(new Timeslot(DayOfWeek.Monday, 9, 11));
        course.Confirm();

        var coach = new Coach("Ben", "ben@coach.be");
        coach.AddSkill("C#");

        course.AssignCoach(coach);

        Assert.Equal(coach, course.Coach);
        Assert.Contains(course, coach.AssignedCourses);
    }

    [Fact]
    public void AssignCoach_WhenNotConfirmed_Throws()
    {
        var course = CreateBasicCourse();
        var coach = new Coach("Ben", "ben@coach.be");

        Assert.Throws<InvalidOperationException>(() => course.AssignCoach(coach));
    }

    [Fact]
    public void AssignCoach_WhenCoachAlreadyAssigned_Throws()
    {
        var course = CreateConfirmedCourse();
        var coach = new Coach("Ben", "ben@coach.be");
        coach.AddSkill("C#");

        course.AssignCoach(coach);

        Assert.Throws<InvalidOperationException>(() => course.AssignCoach(coach));
    }

    [Fact]
    public void AssignCoach_WhenCoachNotSuitable_Throws()
    {
        var course = CreateBasicCourse();
        course.AddRequiredSkill("Python");
        course.AddTimeslot(new Timeslot(DayOfWeek.Monday, 9, 11));
        course.Confirm();

        var coach = new Coach("Ben", "ben@coach.be"); // geen skills

        Assert.Throws<InvalidOperationException>(() => course.AssignCoach(coach));
    }

    // --- Helpers ---
    private Course CreateBasicCourse()
    {
        return new Course("TestCourse", new DateOnly(2025, 1, 1), new DateOnly(2025, 1, 10));
    }

    private Course CreateConfirmedCourse()
    {
        var course = CreateBasicCourse();
        course.AddRequiredSkill("C#");
        course.AddTimeslot(new Timeslot(DayOfWeek.Monday, 9, 11));
        course.Confirm();
        return course;
    }
}
