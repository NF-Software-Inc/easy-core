namespace Easy_Core_Test.Tools;

/// <summary>
/// Tests for <see cref="EncryptionService"/>.
/// </summary>
public class EncryptionServiceTests
{
	[Fact]
	public void NewKey_ByteKey_ProducesKeyOfCorrectSize()
	{
		var settings = new EncryptionSettings();

		var key = EncryptionService.NewKey();

		Assert.Equal(settings.AesKeyByteSize, key.Length);
	}

	[Fact]
	public void NewKey_AesByteKey_ProducesKeyOfCorrectSize()
	{
		var settings = new EncryptionSettings();

		var key = EncryptionService.NewKey(useAes: true);

		Assert.Equal(settings.AesKeyByteSize, key.Length);
	}

	[Fact]
	public void NewKey_StringKey_RespectsCharacterSet()
	{
		var key = EncryptionService.NewKey(20, CharacterSetGroups.Numeric);

		Assert.Equal(20, key.Length);
		Assert.True(key.All(char.IsDigit));
	}

	[Fact]
	public void NewKey_StringKey_UppercaseOnly()
	{
		var key = EncryptionService.NewKey(15, CharacterSetGroups.Uppercase);

		Assert.Equal(15, key.Length);
		Assert.True(key.All(c => c >= 'A' && c <= 'Z'));
	}

	[Fact]
	public void EncryptSymmetric_DecryptSymmetric_StringRoundTrip()
	{
		const string message = "Hello, encrypted world!";
		const string password = "SuperSecret123!";

		var encrypted = EncryptionService.EncryptSymmetric(message, password);
		var decrypted = EncryptionService.DecryptSymmetric(encrypted, password);

		Assert.NotEqual(message, encrypted);
		Assert.Equal(message, decrypted);
	}

	[Fact]
	public void EncryptSymmetric_DecryptSymmetric_BytesRoundTrip()
	{
		var message = System.Text.Encoding.UTF8.GetBytes("binary payload");
		const string password = "another-secret";

		var encrypted = EncryptionService.EncryptSymmetric(message, password);
		var decrypted = EncryptionService.DecryptSymmetric(encrypted, password);

		Assert.Equal(message, decrypted);
	}
}
