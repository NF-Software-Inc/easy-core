using System.Security.Cryptography;

namespace easy_core.Tests.Tools;

/// <summary>
/// Tests for <see cref="HashingService"/>.
/// </summary>
public class HashingServiceTests
{
	[Fact]
	public void CreateHash_AndCheckHash_RoundTrip()
	{
		const string message = "P@ssw0rd!";

		var hash = HashingService.CreateHash(message);

		Assert.False(string.IsNullOrWhiteSpace(hash));
		Assert.True(HashingService.CheckHash(message, hash));
	}

	[Fact]
	public void CheckHash_RejectsIncorrectMessage()
	{
		const string message = "correct";

		var hash = HashingService.CreateHash(message);

		Assert.False(HashingService.CheckHash("incorrect", hash));
	}

	[Fact]
	public void CreateHash_DifferentSaltsProduceDifferentResults()
	{
		const string message = "same message";

		var hash1 = HashingService.CreateHash(message);
		var hash2 = HashingService.CreateHash(message);

		Assert.NotEqual(hash1, hash2);
		Assert.True(HashingService.CheckHash(message, hash1));
		Assert.True(HashingService.CheckHash(message, hash2));
	}

	[Fact]
	public void CreateHash_WithKey_ProducesHmac()
	{
		var buffer = System.Text.Encoding.UTF8.GetBytes("data");

		var hash = HashingService.CreateHash("key", buffer);

		Assert.NotEmpty(hash);
		// HMAC-SHA1 produces 20 bytes
		Assert.Equal(20, hash.Length);
	}
}
