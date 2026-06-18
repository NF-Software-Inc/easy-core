using System.Linq.Expressions;

namespace Easy_Core_Test.Tools;

/// <summary>
/// Tests for <see cref="PredicateBuilder"/>.
/// </summary>
public class PredicateBuilderTests
{
	[Fact]
	public void True_AlwaysReturnsTrue()
	{
		var predicate = PredicateBuilder.True<int>().Compile();

		Assert.True(predicate(0));
		Assert.True(predicate(int.MaxValue));
	}

	[Fact]
	public void False_AlwaysReturnsFalse()
	{
		var predicate = PredicateBuilder.False<int>().Compile();

		Assert.False(predicate(0));
		Assert.False(predicate(int.MaxValue));
	}

	[Fact]
	public void And_CombinesPredicates()
	{
		var isPositive = PredicateBuilder.Create<int>(x => x > 0);
		var isEven = PredicateBuilder.Create<int>(x => x % 2 == 0);
		var combined = isPositive.And(isEven).Compile();

		Assert.True(combined(2));
		Assert.False(combined(1));
		Assert.False(combined(-2));
	}

	[Fact]
	public void And_NullFirst_ReturnsSecond()
	{
		var first = PredicateBuilder.Create<int>();
		var second = PredicateBuilder.Create<int>(x => x > 0);
		var combined = first.And(second).Compile();

		Assert.True(combined(1));
		Assert.False(combined(-1));
	}

	[Fact]
	public void Or_CombinesPredicates()
	{
		var isNegative = PredicateBuilder.Create<int>(x => x < 0);
		var isLarge = PredicateBuilder.Create<int>(x => x > 100);
		var combined = isNegative.Or(isLarge).Compile();

		Assert.True(combined(-5));
		Assert.True(combined(101));
		Assert.False(combined(50));
	}

	[Fact]
	public void Not_NegatesPredicate()
	{
		var isPositive = PredicateBuilder.Create<int>(x => x > 0);
		var negated = isPositive.Not().Compile();

		Assert.False(negated(1));
		Assert.True(negated(0));
		Assert.True(negated(-1));
	}

	[Fact]
	public void Create_NullReturnsNull()
	{
		Assert.Null(PredicateBuilder.Create<int>());
	}
}
