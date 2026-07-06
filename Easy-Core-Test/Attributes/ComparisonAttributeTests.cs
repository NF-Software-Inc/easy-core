using System.ComponentModel.DataAnnotations;

namespace Easy_Core_Test.Attributes;

/// <summary>
/// Tests for the comparison and conditional validation attributes.
/// </summary>
public class ComparisonAttributeTests
{
	private class GreaterThanModel
	{
		[GreaterThan(nameof(Lower))]
		[GreaterThan(nameof(Null), IgnoreNull = true)]
		public int Higher { get; set; }
		public int Lower { get; set; }
		public int? Null { get; set; }
	}

	private class LessThanModel
	{
		[LessThan(nameof(Higher))]
		[LessThan(nameof(Null), IgnoreNull = true)]
		public int Lower { get; set; }
		public int Higher { get; set; }
		public int? Null { get; set; }
	}

	private class GreaterThanOrEqualModel
	{
		[GreaterThanOrEqual(nameof(Lower))]
		public int Higher { get; set; }
		public int Lower { get; set; }
	}

	private class LessThanOrEqualModel
	{
		[LessThanOrEqual(nameof(Higher))]
		public int Lower { get; set; }
		public int Higher { get; set; }
	}

	private class RequiredIfNullModel
	{
		[RequiredIfNull(nameof(Other))]
		public string? Value { get; set; }
		public string? Other { get; set; }
	}

	private class RequiredIfNotNullModel
	{
		[RequiredIfNotNull(nameof(Other))]
		public string? Value { get; set; }
		public string? Other { get; set; }
	}

	private static IList<ValidationResult> Validate(object instance)
	{
		var results = new List<ValidationResult>();
		Validator.TryValidateObject(instance, new ValidationContext(instance), results, validateAllProperties: true);
		return results;
	}

	[Theory]
	[InlineData(5, 1, null, true)]
	[InlineData(5, 1, 3, true)]
	[InlineData(5, 5, 5, false)]
	[InlineData(1, 5, 5, false)]
	public void GreaterThan_ValidatesProperly(int higher, int lower, int? nullTest, bool isValid)
	{
		var results = Validate(new GreaterThanModel { Higher = higher, Lower = lower, Null = nullTest });

		Assert.Equal(isValid, results.Count == 0);
	}

	[Theory]
	[InlineData(1, 5, null, true)]
	[InlineData(1, 5, 5, true)]
	[InlineData(5, 5, 5, false)]
	[InlineData(5, 1, 1, false)]
	public void LessThan_ValidatesProperly(int lower, int higher, int? nullTest, bool isValid)
	{
		var results = Validate(new LessThanModel { Lower = lower, Higher = higher, Null = nullTest });

		Assert.Equal(isValid, results.Count == 0);
	}

	[Theory]
	[InlineData(5, 1, true)]
	[InlineData(5, 5, true)]
	[InlineData(1, 5, false)]
	public void GreaterThanOrEqual_ValidatesProperly(int higher, int lower, bool isValid)
	{
		var results = Validate(new GreaterThanOrEqualModel { Higher = higher, Lower = lower });

		Assert.Equal(isValid, results.Count == 0);
	}

	[Theory]
	[InlineData(1, 5, true)]
	[InlineData(5, 5, true)]
	[InlineData(5, 1, false)]
	public void LessThanOrEqual_ValidatesProperly(int lower, int higher, bool isValid)
	{
		var results = Validate(new LessThanOrEqualModel { Lower = lower, Higher = higher });

		Assert.Equal(isValid, results.Count == 0);
	}

	[Theory]
	[InlineData(null, null, false)]
	[InlineData("", "", false)]
	[InlineData("a", null, true)]
	[InlineData(null, "b", true)]
	[InlineData("a", "b", true)]
	public void RequiredIfNull_ValidatesProperly(string? value, string? other, bool isValid)
	{
		var results = Validate(new RequiredIfNullModel { Value = value, Other = other });

		Assert.Equal(isValid, results.Count == 0);
	}

	[Theory]
	[InlineData("a", "b", true)]
	[InlineData(null, "b", false)]
	[InlineData(null, null, true)]
	[InlineData(null, "", true)]
	public void RequiredIfNotNull_ValidatesProperly(string? value, string? other, bool isValid)
	{
		var results = Validate(new RequiredIfNotNullModel { Value = value, Other = other });

		Assert.Equal(isValid, results.Count == 0);
	}
}
