using HorsesForCourses.Domain;

namespace HorsesForCourses.Tests;

public class TimeslotTests
{
    [Fact]
    public void Constructor_WithValidData_SetsProperties()
    {
        var slot = new Timeslot(DayOfWeek.Monday, 9, 12);

        Assert.Equal(DayOfWeek.Monday, slot.Day);
        Assert.Equal(9, slot.StartHour);
        Assert.Equal(12, slot.EndHour);
        Assert.NotEqual(Guid.Empty, slot.Id);
    }

    [Theory]
    [InlineData(DayOfWeek.Saturday)]
    [InlineData(DayOfWeek.Sunday)]
    public void Constructor_WithWeekend_Throws(DayOfWeek weekendDay)
    {
        Assert.Throws<ArgumentException>(() => new Timeslot(weekendDay, 9, 12));
    }

    [Theory]
    [InlineData(8)]  
    [InlineData(17)] 
    public void Constructor_WithInvalidStartHour_Throws(int startHour)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Timeslot(DayOfWeek.Monday, startHour, startHour + 1));
    }

    [Fact]
    public void Constructor_WithEndHourBeforeStart_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Timeslot(DayOfWeek.Monday, 11, 10));
    }

    [Fact]
    public void Constructor_WithEndHourTooLate_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Timeslot(DayOfWeek.Monday, 16, 18));
    }
}
