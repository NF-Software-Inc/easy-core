namespace Easy_Core_Test.Extensions;

/// <summary>
/// Tests for <see cref="CollectionExtensions"/>.
/// </summary>
public class CollectionExtensionsTests
{
	[Fact]
	public void IsNullOrEmpty_Array_ReturnsExpected()
	{
		int[]? nullArray = null;

		Assert.True(nullArray.IsNullOrEmpty());
		Assert.True(Array.Empty<int>().IsNullOrEmpty());
		Assert.False(new[] { 1 }.IsNullOrEmpty());
	}

	[Fact]
	public void IsNullOrEmpty_Enumerable_ReturnsExpected()
	{
		IEnumerable<int>? nullEnumerable = null;

		Assert.True(nullEnumerable.IsNullOrEmpty());
		Assert.True(Enumerable.Empty<int>().IsNullOrEmpty());
		Assert.False(new List<int> { 1 }.IsNullOrEmpty());
	}

	[Fact]
	public void Contains_String_RespectsComparison()
	{
		var collection = new[] { "Apple", "Banana", "Cherry" };

		Assert.True(collection.Contains("apple", StringComparison.OrdinalIgnoreCase));
		Assert.False(collection.Contains("apple", StringComparison.Ordinal));
		Assert.False(collection.Contains(string.Empty, StringComparison.Ordinal));
	}

	[Fact]
	public void GetNth_ReturnsExpectedItems()
	{
		var list = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

		Assert.Equal(new[] { 0, 2, 4, 6, 8 }, list.GetNth(2).ToArray());
		Assert.Equal(new[] { 1, 4, 7 }, list.GetNth(3, 1).ToArray());
	}

	[Fact]
	public void FirstOrNull_ReturnsNullWhenNoMatch()
	{
		var list = new[] { 1, 2, 3 };

		Assert.Equal(2, list.FirstOrNull(x => x == 2));
		Assert.Null(list.FirstOrNull(x => x == 99));
	}

	[Fact]
	public void DefaultIfNullOrEmpty_ReturnsDefaultWhenEmpty()
	{
		IEnumerable<int>? source = null;

		Assert.Equal(new[] { 5 }, source.DefaultIfNullOrEmpty(5).ToArray());
		Assert.Equal(new[] { 5 }, Enumerable.Empty<int>().DefaultIfNullOrEmpty(5).ToArray());
		Assert.Equal(new[] { 1, 2 }, new[] { 1, 2 }.DefaultIfNullOrEmpty(5).ToArray());
	}

	[Fact]
	public void MaxOrDefault_ReturnsDefaultWhenEmpty()
	{
		IEnumerable<int>? source = null;

		Assert.Equal(0, source.MaxOrDefault(x => x));
		Assert.Equal(99, Enumerable.Empty<int>().MaxOrDefault(x => x, 99));
		Assert.Equal(3, new[] { 1, 2, 3 }.MaxOrDefault(x => x, 99));
	}

	[Fact]
	public void MinOrDefault_ReturnsDefaultWhenEmpty()
	{
		IEnumerable<int>? source = null;

		Assert.Equal(0, source.MinOrDefault(x => x));
		Assert.Equal(99, Enumerable.Empty<int>().MinOrDefault(x => x, 99));
		Assert.Equal(1, new[] { 1, 2, 3 }.MinOrDefault(x => x, 99));
	}

	[Fact]
	public void Partition_SplitsIntoChunksOfRequestedSize()
	{
		var source = Enumerable.Range(1, 7);

		var partitions = source.Partition(3).ToList();

		Assert.Equal(3, partitions.Count);
		Assert.Equal(new[] { 1, 2, 3 }, partitions[0]);
		Assert.Equal(new[] { 4, 5, 6 }, partitions[1]);
		Assert.Equal(7, partitions[2][0]);
	}

	[Fact]
	public void Partition_ExcludesPartialWhenRequested()
	{
		var source = Enumerable.Range(1, 7);

		var partitions = source.Partition(3, includePartial: false).ToList();

		Assert.Equal(2, partitions.Count);
	}

	[Fact]
	public void Split_SplitsIntoRequestedNumberOfGroups()
	{
		var source = Enumerable.Range(1, 9);

		var groups = source.Split(3).Select(x => x.ToArray()).ToArray();

		Assert.Equal(3, groups.Length);
		Assert.Equal(9, groups.Sum(x => x.Length));
	}

	[Fact]
	public void SkipLong_SkipsExpectedNumberOfItems()
	{
		var source = Enumerable.Range(1, 10);

		Assert.Equal(new[] { 6, 7, 8, 9, 10 }, source.SkipLong(5).ToArray());
	}

	[Fact]
	public void Next_ReturnsNextOrFirstWhenAtEnd()
	{
		var source = new[] { "a", "b", "c", "d" };

		Assert.Equal("c", source.Next("b"));
		Assert.Equal("a", source.Next("d"));
	}

	[Fact]
	public void Previous_ReturnsPreviousOrLastWhenAtStart()
	{
		var source = new[] { "a", "b", "c", "d" };

		Assert.Equal("b", source.Previous("c"));
		Assert.Equal("d", source.Previous("a"));
	}

	[Fact]
	public void ToDataTable_BuildsTableFromObjects()
	{
		var source = new[]
		{
			new { Id = 1, Name = "A" },
			new { Id = 2, Name = "B" }
		};

		var table = source.ToDataTable();

		Assert.Equal(2, table.Columns.Count);
		Assert.Equal(2, table.Rows.Count);
		Assert.Equal("A", table.Rows[0]["Name"]);
		Assert.Equal(2, table.Rows[1]["Id"]);
	}

	[Fact]
	public void ToDataTable_FiltersToProvidedColumns()
	{
		var source = new[]
		{
			new { Id = 1, Name = "A" },
			new { Id = 2, Name = "B" }
		};

		var table = source.ToDataTable("Name");

		Assert.Single(table.Columns);
		Assert.Equal("Name", table.Columns[0].ColumnName);
	}
}
