using System.Linq.Expressions;

namespace easy_core.Tests.Tools;

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
		Expression<Func<int, bool>> isPositive = x => x > 0;
		Expression<Func<int, bool>> isEven = x => x % 2 == 0;

		var combined = isPositive.And(isEven).Compile();

		Assert.True(combined(2));
		Assert.False(combined(1));
		Assert.False(combined(-2));
	}

	[Fact]
	public void And_NullFirst_ReturnsSecond()
	{
		Expression<Func<int, bool>>? first = null;
		Expression<Func<int, bool>> second = x => x > 0;

		var combined = first.And(second).Compile();

		Assert.True(combined(1));
		Assert.False(combined(-1));
	}

	[Fact]
	public void Or_CombinesPredicates()
	{
		Expression<Func<int, bool>> isNegative = x => x < 0;
		Expression<Func<int, bool>> isLarge = x => x > 100;

		var combined = isNegative.Or(isLarge).Compile();

		Assert.True(combined(-5));
		Assert.True(combined(101));
		Assert.False(combined(50));
	}

	[Fact]
	public void Not_NegatesPredicate()
	{
		Expression<Func<int, bool>> isPositive = x => x > 0;

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

	[Fact]
	public void Create_PassesThroughExpression()
	{
		Expression<Func<int, bool>> predicate = x => x == 5;

		var created = PredicateBuilder.Create(predicate).Compile();

		Assert.True(created(5));
		Assert.False(created(6));
	}
}
