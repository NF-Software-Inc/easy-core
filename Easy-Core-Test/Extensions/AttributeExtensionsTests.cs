using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace easy_core.Tests.Extensions;

/// <summary>
/// Tests for <see cref="AttributeExtensions"/>.
/// </summary>
public class AttributeExtensionsTests
{
	[Display(Name = "Sample Display", Description = "Sample description.")]
	private class SampleType
	{
		[Display(Name = "Display Name", Description = "Property description.")]
		public string? Name { get; set; }

		public string? Plain { get; set; }
	}

	private enum SampleEnum
	{
		[Display(Name = "First Option")]
		First,
		Second
	}

	[Fact]
	public void GetTypeDisplayName_ReadsDisplayAttribute()
	{
		Assert.Equal("Sample Display", typeof(SampleType).GetTypeDisplayName());
	}

	[Fact]
	public void GetTypeDisplayName_FallsBackToTypeName()
	{
		Assert.Equal(nameof(AttributeExtensionsTests), typeof(AttributeExtensionsTests).GetTypeDisplayName());
	}

	[Fact]
	public void GetTypeDisplayDescription_ReadsDescription()
	{
		Assert.Equal("Sample description.", typeof(SampleType).GetTypeDisplayDescription());
	}

	[Fact]
	public void GetPropertyDisplayName_ByPropertyInfo_ReadsAttribute()
	{
		var property = typeof(SampleType).GetProperty(nameof(SampleType.Name))!;

		Assert.Equal("Display Name", property.GetPropertyDisplayName());
	}

	[Fact]
	public void GetPropertyDisplayName_FallsBackToPropertyName()
	{
		var property = typeof(SampleType).GetProperty(nameof(SampleType.Plain))!;

		Assert.Equal(nameof(SampleType.Plain), property.GetPropertyDisplayName());
	}

	[Fact]
	public void GetPropertyDisplayName_ByType_AndUnknownProperty_ReturnsName()
	{
		Assert.Equal("Missing", typeof(SampleType).GetPropertyDisplayName("Missing"));
		Assert.Equal("Display Name", typeof(SampleType).GetPropertyDisplayName(nameof(SampleType.Name)));
	}

	[Fact]
	public void GetPropertyDisplayDescription_ByType_ReturnsDescription()
	{
		Assert.Equal("Property description.", typeof(SampleType).GetPropertyDisplayDescription(nameof(SampleType.Name)));
		Assert.Null(typeof(SampleType).GetPropertyDisplayDescription("Missing"));
	}

	[Fact]
	public void GetPropertyDisplayName_ByExpression_ReadsAttribute()
	{
		var instance = new SampleType { Name = "value" };

		Expression<Func<string?>> expression = () => instance.Name;

		Assert.Equal("Display Name", expression.GetPropertyDisplayName());
	}

	[Fact]
	public void GetValueDisplayName_EnumWithDisplayReturnsName()
	{
		Assert.Equal("First Option", SampleEnum.First.GetValueDisplayName());
	}

	[Fact]
	public void GetValueDisplayName_FallsBackToToString()
	{
		Assert.Equal(nameof(SampleEnum.Second), SampleEnum.Second.GetValueDisplayName());
	}

	[Fact]
	public void GetValueDisplayName_NullReturnsEmpty()
	{
		string? value = null;

		Assert.Equal(string.Empty, value.GetValueDisplayName());
	}

	[Fact]
	public void GetTypeAttribute_ReturnsAttributeOrNull()
	{
		Assert.NotNull(typeof(SampleType).GetTypeAttribute<DisplayAttribute>());
		Assert.Null(typeof(SampleType).GetTypeAttribute<ObsoleteAttribute>());
	}
}
