namespace easy_core.Tests.Tools;

/// <summary>
/// Tests for <see cref="MimeTypeMap"/>.
/// </summary>
public class MimeTypeMapTests
{
	[Theory]
	[InlineData("file.txt", "text/plain")]
	[InlineData("file.json", "application/json")]
	[InlineData("file.pdf", "application/pdf")]
	[InlineData("file.png", "image/png")]
	[InlineData(".html", "text/html")]
	public void GetMimeType_ReturnsExpectedMime(string fileName, string expected)
	{
		Assert.Equal(expected, MimeTypeMap.GetMimeType(fileName));
	}

	[Fact]
	public void GetMimeType_UnknownReturnsDefault()
	{
		var result = MimeTypeMap.GetMimeType("file.thisisnotreal");

		Assert.False(string.IsNullOrEmpty(result));
	}

	[Fact]
	public void GetFileTypes_ReturnsExtensionsForMime()
	{
		var types = MimeTypeMap.GetFileTypes("text/plain").ToList();

		Assert.NotEmpty(types);
		Assert.Contains(".txt", types);
	}
}
