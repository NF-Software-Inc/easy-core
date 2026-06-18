using System.Linq.Expressions;
using System.Text.Json;

namespace easy_core.Tests.Extensions;

/// <summary>
/// Tests for <see cref="GeneralExtensions"/>.
/// </summary>
public class GeneralExtensionsTests
{
	private class Sample
	{
		public int Id { get; set; }
		public string? Name { get; set; }
		public Sample? Child { get; set; }
	}

	[Fact]
	public void ToHtmlId_ReplacesNumericLeadingCharacter()
	{
		var guid = Guid.Parse("12345678-1234-1234-1234-123456789012");

		var result = guid.ToHtmlId();
		var first = result.ToString("N")[0];

		// The leading character must now be a hex letter (a-f, the lowercase output of ToString("N")).
		Assert.Contains(first, "abcdef");
	}

	[Fact]
	public void ToHtmlId_AlreadyLetter_ProducesValidLeadingLetter()
	{
		var guid = Guid.Parse("a2345678-1234-1234-1234-123456789012");

		var result = guid.ToHtmlId();
		var first = result.ToString("N")[0];

		Assert.Contains(first, "abcdef");
	}

	[Fact]
	public void Clone_CopiesOptionsAndConverters()
	{
		var original = new JsonSerializerOptions
		{
			AllowTrailingCommas = true,
			WriteIndented = true,
			MaxDepth = 12
		};

		original.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());

		var clone = original.Clone();

		Assert.True(clone.AllowTrailingCommas);
		Assert.True(clone.WriteIndented);
		Assert.Equal(12, clone.MaxDepth);
		Assert.Single(clone.Converters);
	}

	[Fact]
	public void Clone_CanExcludeConverters()
	{
		var original = new JsonSerializerOptions();
		original.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());

		var clone = original.Clone(includeConverters: false);

		Assert.Empty(clone.Converters);
	}

	[Fact]
	public void ToLambda_SimpleProperty_ReturnsExpression()
	{
		var lambda = "Name".ToLambda<Sample>();

		var compiled = lambda.Compile();

		Assert.Equal("test", compiled(new Sample { Name = "test" }));
	}

	[Fact]
	public void ToLambda_NestedProperty_ReturnsExpression()
	{
		var lambda = "Child.Name".ToLambda<Sample>();

		var compiled = lambda.Compile();

		Assert.Equal("nested", compiled(new Sample { Child = new Sample { Name = "nested" } }));
	}

	[Fact]
	public void GetExpressionPropertyName_ReturnsMemberName()
	{
		Expression<Func<Sample, int>> expression = x => x.Id;

		Assert.Equal(nameof(Sample.Id), expression.GetExpressionPropertyName());
	}

	[Fact]
	public void GetExpressionPropertyName_HandlesUnaryConversion()
	{
		Expression<Func<Sample, object>> expression = x => x.Id;

		Assert.Equal(nameof(Sample.Id), expression.GetExpressionPropertyName());
	}

	[Fact]
	public void TryUpdateModel_CopiesSelectedProperties()
	{
		var source = new Sample { Id = 5, Name = "src" };
		var destination = new Sample { Id = 1, Name = "dst" };

		var ok = source.TryUpdateModel(destination, x => x.Name);

		Assert.True(ok);
		Assert.Equal(1, destination.Id);
		Assert.Equal("src", destination.Name);
	}
}
