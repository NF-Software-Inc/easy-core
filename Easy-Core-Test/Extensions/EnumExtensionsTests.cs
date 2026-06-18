using System.ComponentModel.DataAnnotations;

namespace Easy_Core_Test.Extensions;

/// <summary>
/// Tests for <see cref="EnumExtensions"/>.
/// </summary>
public class EnumExtensionsTests
{
	[Flags]
	private enum Sample
	{
		None = 0b_00000000_00000000_00000000_00000000,
		A = 0b_00000000_00000000_00000000_00000001,
		B = 0b_00000000_00000000_00000000_00000010,
		C = 0b_00000000_00000000_00000000_00000100,

		[Display(Name = "Flag D")]
		D = 0b_00000000_00000000_00000000_00001000,
		AB = 0b_00000000_00000000_00000000_00000011
	}

	[Fact]
	public void GetFlags_ReturnsActiveFlags()
	{
		var value = Sample.A | Sample.C;
		var flags = value.GetFlags().ToArray();

		Assert.Contains(Sample.A, flags);
		Assert.Contains(Sample.C, flags);
		Assert.DoesNotContain(Sample.B, flags);
		Assert.DoesNotContain(Sample.D, flags);
	}

	[Fact]
	public void HasAnyFlag_ReturnsExpected()
	{
		var value = Sample.A | Sample.C;

		Assert.True(value.HasAnyFlag(Sample.A | Sample.B));
		Assert.True(value.HasAnyFlag(Sample.C));
		Assert.False(value.HasAnyFlag(Sample.B | Sample.D));
	}

	[Fact]
	public void HasAllFlags_ReturnsExpected()
	{
		var value = Sample.A | Sample.B | Sample.C;

		Assert.True(value.HasAllFlags(Sample.A | Sample.B));
		Assert.True(value.HasAllFlags(Sample.AB));
		Assert.False(value.HasAllFlags(Sample.A | Sample.D));
	}

	[Fact]
	public void SetFlag_AddsFlag()
	{
		var value = Sample.A;
		var result = value.SetFlag(Sample.B);

		Assert.Equal(Sample.A | Sample.B, result);
	}

	[Fact]
	public void UnsetFlag_RemovesFlag()
	{
		var value = Sample.A | Sample.B;

		Assert.Equal(Sample.A, value.UnsetFlag(Sample.B));
	}

	[Fact]
	public void CombineFlags_CombinesAllValues()
	{
		var flags = new[] { Sample.A, Sample.B, Sample.C };

		Assert.Equal(Sample.A | Sample.B | Sample.C, flags.CombineFlags());
	}

	[Fact]
	public void CombineFlags_EmptyReturnsDefault()
	{
		Assert.Equal(default, Array.Empty<Sample>().CombineFlags());
	}

	[Fact]
	public void GetFlaggedEnumDisplay_FallsBackToToStringOfFlagNames()
	{
		var value = Sample.A | Sample.B | Sample.D;
		var display = value.GetFlaggedEnumDisplay();

		Assert.Contains("A", display);
		Assert.Contains("B", display);
		Assert.Contains("Flag D", display);
	}
}
