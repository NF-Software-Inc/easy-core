namespace Easy_Core_Test.Extensions;

/// <summary>
/// Tests for <see cref="DateAndTimeExtensions"/>.
/// </summary>
public class DateAndTimeExtensionsTests
{
	[Fact]
	public void ToDateTime_FromDateOnly_UsesMidnight()
	{
		var date = new DateOnly(2024, 5, 15);

		var result = date.ToDateTime();

		Assert.Equal(new DateTime(2024, 5, 15, 0, 0, 0), result);
	}

	[Fact]
	public void GetDaysInMonth_DateTime_ReturnsAllDays()
	{
		var date = new DateTime(2024, 2, 1);

		var days = date.GetDaysInMonth().ToList();

		Assert.Equal(29, days.Count);
		Assert.Equal(new DateTime(2024, 2, 1), days[0]);
		Assert.Equal(new DateTime(2024, 2, 29), days[^1]);
	}

	[Fact]
	public void GetDaysInMonth_DateOnly_ReturnsAllDays()
	{
		var date = new DateOnly(2023, 4, 10);

		var days = date.GetDaysInMonth().ToList();

		Assert.Equal(30, days.Count);
	}

	[Fact]
	public void GetXthDayOfWeekInMonth_FindsCorrectInstance()
	{
		var date = new DateTime(2024, 5, 1);

		// 1st Monday of May 2024 is the 6th
		var first = date.GetXthDayOfWeekInMonth(DayOfWeek.Monday, 1);

		Assert.Equal(new DateTime(2024, 5, 6), first);
	}

	[Fact]
	public void GetXthDayOfWeekInMonth_OutOfRangeThrows()
	{
		var date = new DateTime(2024, 1, 1);

		Assert.Throws<ArgumentOutOfRangeException>(() => date.GetXthDayOfWeekInMonth(DayOfWeek.Monday, 0));
		Assert.Throws<ArgumentOutOfRangeException>(() => date.GetXthDayOfWeekInMonth(DayOfWeek.Monday, 6));
	}

	[Theory]
	[InlineData(2024, 5, 1, DayOfWeek.Friday, 2024, 5, 3)]
	[InlineData(2024, 5, 3, DayOfWeek.Friday, 2024, 5, 3)]
	[InlineData(2024, 5, 1, DayOfWeek.Wednesday, 2024, 5, 1)]
	public void GetNextWeekday_DateTime_ReturnsExpected(int y, int m, int d, DayOfWeek target, int ey, int em, int ed)
	{
		var actual = new DateTime(y, m, d).GetNextWeekday(target);
		Assert.Equal(new DateTime(ey, em, ed), actual);
	}

	[Theory]
	[InlineData(2024, 5, 10, DayOfWeek.Monday, 2024, 5, 6)]
	[InlineData(2024, 5, 6, DayOfWeek.Monday, 2024, 5, 6)]
	public void GetPreviousWeekday_DateTime_ReturnsExpected(int y, int m, int d, DayOfWeek target, int ey, int em, int ed)
	{
		var actual = new DateTime(y, m, d).GetPreviousWeekday(target);
		Assert.Equal(new DateTime(ey, em, ed), actual);
	}

	[Fact]
	public void ListDaysTo_ReturnsContiguousDates()
	{
		var start = new DateTime(2024, 1, 1);
		var end = new DateTime(2024, 1, 5);

		var days = start.ListDaysTo(end).ToList();

		Assert.Equal(5, days.Count);
		Assert.Equal(start, days[0]);
		Assert.Equal(end, days[^1]);
	}

	[Fact]
	public void ListDaysTo_StepGreaterThanOne()
	{
		var start = new DateOnly(2024, 1, 1);
		var end = new DateOnly(2024, 1, 10);

		var days = start.ListDaysTo(end, 2).ToList();

		Assert.Equal(5, days.Count);
		Assert.Equal(new DateOnly(2024, 1, 9), days[^1]);
	}

	[Fact]
	public void ToCustomString_DefaultSettings_FormatsHoursMinutesSeconds()
	{
		var time = new TimeSpan(5, 30, 15);

		Assert.Equal("5:30:15", time.ToCustomString());
	}

	[Fact]
	public void Round_RoundsToNearestSecondByDefault()
	{
		var time = new TimeSpan(0, 0, 0, 5, 700);

		var rounded = time.Round();

		Assert.Equal(TimeSpan.FromSeconds(6), rounded);
	}

	[Fact]
	public void Sum_TimeSpan_AggregatesValues()
	{
		var items = new[] { TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(15) };

		Assert.Equal(TimeSpan.FromMinutes(30), items.Sum(x => x));
	}

	[Fact]
	public void ToHhMm_FormatsExpected()
	{
		Assert.Equal("13:05", new TimeSpan(13, 5, 0).ToHhMm());
		Assert.Equal("13:05", new TimeOnly(13, 5).ToHhMm());
	}

	[Fact]
	public void ToAmPm_FormatsConsistently()
	{
		var time = new TimeSpan(13, 5, 0);

		// The exact format depends on culture; verify it matches the underlying short time pattern.
		var expected = DateTime.Today.Add(time).ToString("t");

		Assert.Equal(expected, time.ToAmPm());
		Assert.Equal(expected, new TimeOnly(13, 5).ToAmPm());
	}

	[Fact]
	public void ToAmPm_OutOfRange_Throws()
	{
		Assert.Throws<ArgumentException>(() => TimeSpan.FromDays(2).ToAmPm());
		Assert.Throws<ArgumentException>(() => TimeSpan.FromMinutes(-1).ToAmPm());
	}

	[Fact]
	public void ToDateOnly_AndToTimeOnly_ConvertCorrectly()
	{
		var date = new DateTime(2024, 5, 15, 9, 30, 45);

		Assert.Equal(new DateOnly(2024, 5, 15), date.ToDateOnly());
		Assert.Equal(new TimeOnly(9, 30, 45), date.ToTimeOnly());
		Assert.Equal(new TimeOnly(9, 30, 45), new TimeSpan(9, 30, 45).ToTimeOnly());
	}

	[Fact]
	public void GetWeekOfYear_FollowsIsoCalendar()
	{
		Assert.Equal(System.Globalization.ISOWeek.GetWeekOfYear(new DateTime(2024, 1, 15)), new DateTime(2024, 1, 15).GetWeekOfYear());
		Assert.Equal(System.Globalization.ISOWeek.GetWeekOfYear(new DateTime(2024, 1, 15)), new DateOnly(2024, 1, 15).GetWeekOfYear());
	}

	[Fact]
	public void ListDatesInMonth_ReturnsAllDaysOfRequestedMonth()
	{
		var dates = DateAndTimeExtensions.ListDatesInMonth(2024, 2).ToList();

		Assert.Equal(29, dates.Count);
		Assert.All(dates, x => Assert.Equal(2, x.Month));
	}
}
