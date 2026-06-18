namespace easy_core.Tests.Models;

/// <summary>
/// Tests for <see cref="Lexicon{TKey, TValue}"/>.
/// </summary>
public class LexiconTests
{
	[Fact]
	public void Add_AndIndex_StoresMultipleValuesPerKey()
	{
		var lexicon = new Lexicon<string, int>
		{
			{ "a", 1 },
			{ "a", 2 },
			{ "b", 3 }
		};

		Assert.Equal(3, lexicon.Count);
		Assert.Equal(2, lexicon["a"].Count);
		Assert.Equal(1, lexicon["a", 0]);
		Assert.Equal(2, lexicon["a", 1]);
	}

	[Fact]
	public void Add_RangeAndContains()
	{
		var lexicon = new Lexicon<string, int>();
		lexicon.Add("a", new[] { 1, 2, 3 });

		Assert.True(lexicon.ContainsKey("a"));
		Assert.True(lexicon.Contains("a", 2));
		Assert.False(lexicon.Contains("a", 99));
		Assert.True(lexicon.ContainsValue(3));
	}

	[Fact]
	public void Remove_ByKeyAndValue()
	{
		var lexicon = new Lexicon<string, int>
		{
			{ "a", 1 },
			{ "a", 2 }
		};

		Assert.True(lexicon.Remove("a", 1));
		Assert.False(lexicon.Contains("a", 1));
		Assert.True(lexicon.Contains("a", 2));
	}

	[Fact]
	public void Remove_ByKey_RemovesAll()
	{
		var lexicon = new Lexicon<string, int>
		{
			{ "a", 1 },
			{ "a", 2 }
		};

		Assert.True(lexicon.Remove("a"));
		Assert.False(lexicon.ContainsKey("a"));
	}

	[Fact]
	public void ChangeValue_UpdatesExistingValue()
	{
		var lexicon = new Lexicon<string, int> { { "a", 1 } };

		Assert.True(lexicon.ChangeValue("a", 1, 99));
		Assert.True(lexicon.Contains("a", 99));
		Assert.False(lexicon.Contains("a", 1));
	}

	[Fact]
	public void TryGetValue_ByIndex()
	{
		var lexicon = new Lexicon<string, int>
		{
			{ "a", 10 },
			{ "a", 20 }
		};

		Assert.True(lexicon.TryGetValue("a", 1, out var value));
		Assert.Equal(20, value);
		Assert.False(lexicon.TryGetValue("a", 5, out _));
	}

	[Fact]
	public void Clear_RemovesAllItems()
	{
		var lexicon = new Lexicon<string, int> { { "a", 1 }, { "b", 2 } };

		lexicon.Clear();

		Assert.Empty(lexicon);
	}
}
