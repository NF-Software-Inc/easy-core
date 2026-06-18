namespace easy_core.Tests.Tools;

/// <summary>
/// Tests for <see cref="OtpService"/>.
/// </summary>
public class OtpServiceTests
{
	[Fact]
	public void GetOtpSecret_ReturnsCorrectLength()
	{
		var secret = OtpService.GetOtpSecret();

		Assert.Equal(OtpService.SecretLength, secret.Length);
	}

	[Fact]
	public void GetOtpCode_ReturnsCodeOfRequestedLength()
	{
		var secret = OtpService.GetOtpSecret();

		var code = OtpService.GetOtpCode(secret, 1, 6);

		Assert.Equal(6, code.Length);
		Assert.True(code.All(char.IsDigit));
	}

	[Fact]
	public void GetOtpCode_DeterministicForSameInputs()
	{
		var secret = OtpService.GetOtpSecret();

		var code1 = OtpService.GetOtpCode(secret, 42, 6);
		var code2 = OtpService.GetOtpCode(secret, 42, 6);

		Assert.Equal(code1, code2);
	}

	[Fact]
	public void GetOtpCode_InvalidSecretLength_Throws()
	{
		Assert.Throws<ArgumentException>(() => OtpService.GetOtpCode("short", 1, 6));
	}

	[Fact]
	public void GetOtpCode_InvalidDigits_Throws()
	{
		var secret = OtpService.GetOtpSecret();

		Assert.Throws<ArgumentException>(() => OtpService.GetOtpCode(secret, 1, 0));
		Assert.Throws<ArgumentException>(() => OtpService.GetOtpCode(secret, 1, 11));
	}

	[Fact]
	public void GetOtpCode_NegativeIteration_Throws()
	{
		var secret = OtpService.GetOtpSecret();

		Assert.Throws<ArgumentException>(() => OtpService.GetOtpCode(secret, -1, 6));
	}

	[Fact]
	public void CheckOtpCode_ValidatesCurrentCode()
	{
		var secret = OtpService.GetOtpSecret();

		var code = OtpService.GetOtpCode(secret);

		Assert.True(OtpService.CheckOtpCode(secret, code));
		Assert.False(OtpService.CheckOtpCode(secret, "000000"));
	}

	[Fact]
	public void TimeInterval_RejectsNonPositive()
	{
		Assert.Throws<ArgumentOutOfRangeException>(() => OtpService.TimeInterval = 0);
		Assert.Throws<ArgumentOutOfRangeException>(() => OtpService.TimeInterval = -5);
	}
}
