namespace easy_core.Tests.Extensions;

/// <summary>
/// Tests for <see cref="StringExtensions"/>.
/// </summary>
public class StringExtensionsTests
{
	[Fact]
	public void Base64Encode_NullValue_ReturnsNull()
	{
		string? value = null;
		Assert.Null(value.Base64Encode());
	}

	[Theory]
	[InlineData("", "")]
	[InlineData("hello", "aGVsbG8=")]
	[InlineData("Easy-Core", "RWFzeS1Db3Jl")]
	public void Base64Encode_RoundTrip_ProducesOriginal(string original, string expected)
	{
		var encoded = original.Base64Encode();

		Assert.Equal(expected, encoded);
		Assert.Equal(original, encoded.Base64Decode());
	}

	[Fact]
	public void Base64Decode_NullValue_ReturnsNull()
	{
		string? value = null;
		Assert.Null(value.Base64Decode());
	}

	[Theory]
	[InlineData(null, "")]
	[InlineData("", "")]
	[InlineData("   ", "")]
	[InlineData("plain", "plain")]
	[InlineData("a,b", "\"a,b\"")]
	[InlineData("has \"quote\"", "\"has \"\"quote\"\"\"")]
	[InlineData(" leading", "\" leading\"")]
	[InlineData("trailing ", "\"trailing \"")]
	public void ToCsvString_HandlesQuotingAndSeparators(string? value, string expected)
	{
		Assert.Equal(expected, value.ToCsvString());
	}

	[Fact]
	public void ToCsvString_QuotesCustomCharacters()
	{
		Assert.Equal("\"a;b\"", "a;b".ToCsvString(',', ';'));
	}

	[Theory]
	[InlineData("a\nb\nc", 3)]
	[InlineData("a\r\nb\r\nc", 3)]
	[InlineData("only", 1)]
	public void ToLines_SplitsAtLineBreaks(string value, int expected)
	{
		Assert.Equal(expected, value.ToLines().Length);
	}

	[Theory]
	[InlineData("A", 1)]
	[InlineData("Z", 26)]
	[InlineData("AA", 27)]
	[InlineData("AB", 28)]
	[InlineData("AZ", 52)]
	[InlineData("BA", 53)]
	[InlineData("ZZ", 702)]
	[InlineData("aa", 27)]
	public void FromExcelColumn_ConvertsCorrectly(string column, int expected)
	{
		Assert.Equal(expected, column.FromExcelColumn());
	}

	[Theory]
	[InlineData(null)]
	[InlineData("")]
	[InlineData("   ")]
	public void FromExcelColumn_NullOrWhitespace_Throws(string? column)
	{
		Assert.Throws<ArgumentNullException>(() => column!.FromExcelColumn());
	}

	[Theory]
	[InlineData(1, "A")]
	[InlineData(26, "Z")]
	[InlineData(27, "AA")]
	[InlineData(28, "AB")]
	[InlineData(702, "ZZ")]
	[InlineData(703, "AAA")]
	public void ToExcelColumn_ConvertsCorrectly(int index, string expected)
	{
		Assert.Equal(expected, index.ToExcelColumn());
	}

	[Fact]
	public void FromExcelColumn_ToExcelColumn_RoundTrip()
	{
		for (var i = 1; i <= 1000; i++)
			Assert.Equal(i, i.ToExcelColumn().FromExcelColumn());
	}

	[Theory]
	[InlineData("Hello", "Hello", StringMatchMode.Equals, true)]
	[InlineData("Hello", "hello", StringMatchMode.Equals, false)]
	[InlineData("Hello World", "World", StringMatchMode.Contains, true)]
	[InlineData("Hello World", "Earth", StringMatchMode.Contains, false)]
	[InlineData("Hello World", "Hello", StringMatchMode.StartsWith, true)]
	[InlineData("Hello World", "World", StringMatchMode.StartsWith, false)]
	[InlineData("Hello World", "World", StringMatchMode.EndsWith, true)]
	[InlineData("Hello World", "Hello", StringMatchMode.EndsWith, false)]
	public void IsMatch_CompareModes(string value, string compareTo, StringMatchMode mode, bool expected)
	{
		Assert.Equal(expected, value.IsMatch(compareTo, mode));
	}

	[Fact]
	public void IsMatch_RespectsComparisonType()
	{
		Assert.True("Hello".IsMatch("hello", StringMatchMode.Equals, StringComparison.OrdinalIgnoreCase));
		Assert.False("Hello".IsMatch("hello", StringMatchMode.Equals, StringComparison.Ordinal));
	}
}
