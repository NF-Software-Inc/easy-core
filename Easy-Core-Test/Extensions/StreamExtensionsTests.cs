using System.Text;

namespace Easy_Core_Test.Extensions;

/// <summary>
/// Tests for <see cref="StreamExtensions"/>.
/// </summary>
public class StreamExtensionsTests
{
	[Fact]
	public void CopyAndReset_ToNewStream_ProducesResetCopy()
	{
		using var source = new MemoryStream(Encoding.UTF8.GetBytes("hello world"));

		using var copy = source.CopyAndReset();

		Assert.Equal(0, copy.Position);
		Assert.Equal(0, source.Position);
		Assert.Equal("hello world", Encoding.UTF8.GetString(copy.ToArray()));
	}

	[Fact]
	public async Task CopyAndResetAsync_ToNewStream_ProducesResetCopy()
	{
		using var source = new MemoryStream(Encoding.UTF8.GetBytes("async data"));

		using var copy = await source.CopyAndResetAsync();

		Assert.Equal(0, copy.Position);
		Assert.Equal(0, source.Position);
		Assert.Equal("async data", Encoding.UTF8.GetString(copy.ToArray()));
	}

	[Fact]
	public void CopyAndReset_BetweenStreams_CopiesContent()
	{
		using var source = new MemoryStream(Encoding.UTF8.GetBytes("payload"));
		using var destination = new MemoryStream();

		source.CopyAndReset(destination);

		Assert.Equal(Encoding.UTF8.GetBytes("payload"), destination.ToArray());
	}
}
