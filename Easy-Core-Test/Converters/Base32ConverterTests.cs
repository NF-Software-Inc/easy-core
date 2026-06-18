namespace easy_core.Tests.Converters;

/// <summary>
/// Tests for <see cref="Base32Converter"/>.
/// </summary>
public class Base32ConverterTests
{
	[Theory]
	[InlineData("foo", "MZXW6===")]
	[InlineData("foobar", "MZXW6YTBOI======")]
	[InlineData("Hello!", "JBSWY3DPEE======")]
	public void EncodeBase32String_ProducesExpectedOutput(string input, string expected)
	{
		Assert.Equal(expected, Base32Converter.EncodeBase32String(input));
	}

	[Theory]
	[InlineData("MZXW6===", "foo")]
	[InlineData("MZXW6YTBOI======", "foobar")]
	[InlineData("JBSWY3DPEE======", "Hello!")]
	public void DecodeBase32String_ReturnsOriginalString(string input, string expected)
	{
		Assert.Equal(expected, Base32Converter.DecodeBase32String(input));
	}

	[Fact]
	public void RoundTrip_ProducesOriginal()
	{
		const string original = "easy-core test message 123";

		var encoded = Base32Converter.EncodeBase32String(original);
		var decoded = Base32Converter.DecodeBase32String(encoded);

		Assert.Equal(original, decoded);
	}

	[Fact]
	public void FromBase32String_NullOrEmpty_Throws()
	{
		Assert.Throws<ArgumentNullException>(() => Base32Converter.FromBase32String(""));
		Assert.Throws<ArgumentNullException>(() => Base32Converter.FromBase32String(null!));
	}

	[Fact]
	public void ToBase32String_NullOrEmpty_Throws()
	{
		Assert.Throws<ArgumentNullException>(() => Base32Converter.ToBase32String(Array.Empty<byte>()));
		Assert.Throws<ArgumentNullException>(() => Base32Converter.ToBase32String(null!));
	}

	[Fact]
	public void FromBase32String_InvalidCharacter_Throws()
	{
		Assert.Throws<ArgumentException>(() => Base32Converter.FromBase32String("!!!!!!!!"));
	}
}
