using System.Net;

namespace easy_core.Tests.Extensions;

/// <summary>
/// Tests for <see cref="IpAddressExtensions"/>.
/// </summary>
public class IpAddressExtensionsTests
{
	[Theory]
	[InlineData("10.0.0.5", "10.0.0.0", "10.255.255.255", true)]
	[InlineData("172.20.0.1", "172.16.0.0", "172.31.255.255", true)]
	[InlineData("8.8.8.8", "10.0.0.0", "10.255.255.255", false)]
	public void IsInRange_ReturnsExpected(string address, string first, string last, bool expected)
	{
		Assert.Equal(expected, IPAddress.Parse(address).IsInRange(IPAddress.Parse(first), IPAddress.Parse(last)));
	}

	[Theory]
	[InlineData("10.0.0.1", true)]
	[InlineData("172.16.5.5", true)]
	[InlineData("192.168.1.1", true)]
	[InlineData("8.8.8.8", false)]
	[InlineData("172.32.0.1", false)]
	public void IsPrivate_ReturnsExpected(string address, bool expected)
	{
		Assert.Equal(expected, IPAddress.Parse(address).IsPrivate());
	}

	[Fact]
	public void IsLessThan_AndIsGreaterThan_Compare()
	{
		var low = IPAddress.Parse("10.0.0.1");
		var high = IPAddress.Parse("10.0.0.5");

		Assert.True(low.IsLessThan(high));
		Assert.True(high.IsGreaterThan(low));
		Assert.False(low.IsGreaterThan(high));
	}

	[Theory]
	[InlineData(-1)]
	[InlineData(33)]
	public void LastInRange_RejectsInvalidCidr(int cidr)
	{
		Assert.Throws<ArgumentException>(() => IPAddress.Parse("10.0.0.0").LastInRange(cidr));
	}

	[Fact]
	public void LastInRange_ProducesExpectedResultFor24()
	{
		var first = IPAddress.Parse("192.168.1.0");

		var last = first.LastInRange(24);

		// /24 range contains 256 addresses; first + 256 = 192.168.2.0
		Assert.Equal("192.168.2.0", last.ToString());
	}

	[Fact]
	public void ToLong_ConvertsConsistently()
	{
		var lower = IPAddress.Parse("10.0.0.0").ToLong();
		var higher = IPAddress.Parse("10.0.1.0").ToLong();

		Assert.True(higher > lower);
		Assert.Equal(256, higher - lower);
	}
}
