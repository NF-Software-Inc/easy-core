﻿using System.ComponentModel;
using System.Data;

namespace easy_core;

/// <summary>
/// Extension methods related to collections.
/// </summary>
public static class CollectionExtensions
{
	/// <inheritdoc cref="Enumerable.ExceptBy{TSource, TKey}(IEnumerable{TSource}, IEnumerable{TKey}, Func{TSource, TKey}, IEqualityComparer{TKey}?)"/>
	public static IEnumerable<TSource> ExceptBy<TSource, TKey>(this IEnumerable<TSource> first, IEnumerable<TSource> second, Func<TSource, TKey> keySelector)
	{
		return first.ExceptBy(second.Select(keySelector), keySelector);
	}

	/// <inheritdoc cref="Enumerable.IntersectBy{TSource, TKey}(IEnumerable{TSource}, IEnumerable{TKey}, Func{TSource, TKey}, IEqualityComparer{TKey}?)"/>
	public static IEnumerable<TSource> IntersectBy<TSource, TKey>(this IEnumerable<TSource> first, IEnumerable<TSource> second, Func<TSource, TKey> keySelector)
	{
		return first.IntersectBy(second.Select(keySelector), keySelector);
	}

	/// <inheritdoc cref="Enumerable.Contains{TSource}(IEnumerable{TSource}, TSource)" />
	public static bool Contains(this IEnumerable<string> source, string value, StringComparison comparisonType)
	{
		return string.IsNullOrWhiteSpace(value) == false && source.Any(x => x.Equals(value, comparisonType));
	}

	/// <summary>
	/// Checks whether an array is null or empty.
	/// </summary>
	/// <typeparam name="TArray">The type of the array to check.</typeparam>
	/// <param name="value">The value to check.</param>
	public static bool IsNullOrEmpty<TArray>(this TArray[]? value)
	{
		return value == null || value.Length == 0;
	}

	/// <summary>
	/// Checks whether an enumerable is null or empty.
	/// </summary>
	/// <typeparam name="TItem">The type of the item in the enumerable.</typeparam>
	/// <param name="value">The value to check.</param>
	public static bool IsNullOrEmpty<TItem>(this IEnumerable<TItem>? value)
	{
		return value == null || value.Any() == false;
	}

	/// <summary>
	/// Returns each nth item of the provided list.
	/// </summary>
	/// <typeparam name="TList">The type of data contained in the list.</typeparam>
	/// <param name="list">The source collection.</param>
	/// <param name="interval">The number of items to skip between iterations.</param>
	/// <param name="start">The index of the item to start at.</param>
	public static IEnumerable<TList> GetNth<TList>(this List<TList> list, int interval, int start = 0)
	{
		for (var i = start; i < list.Count; i += interval)
			yield return list[i];
	}

	/// <inheritdoc cref="Enumerable.FirstOrDefault{TSource}(IEnumerable{TSource}, Func{TSource, bool})"/>
	/// <remarks>
	/// Ideal for allowing null return with non-nullable data types (ex. <see cref="DateTime"/>).
	/// </remarks>
	public static TSource? FirstOrNull<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate) where TSource : struct
	{
		foreach (var item in source)
			if (predicate(item))
				return item;

		return null;
	}

	/// <inheritdoc cref="Enumerable.DefaultIfEmpty{TSource}(IEnumerable{TSource}, TSource)"/>
	/// <remarks>
	/// If the source or all items are <see langword="null"/> a new collection with the provided default will be returned.
	/// </remarks>
	public static IEnumerable<TSource> DefaultIfNullOrEmpty<TSource>(this IEnumerable<TSource>? source, TSource defaultValue)
	{
		return source != null && source.Any(x => x != null) ? source : new List<TSource>() { defaultValue };
	}

	/// <inheritdoc cref="Enumerable.Max{TSource, TResult}(IEnumerable{TSource}, Func{TSource, TResult})"/>
	public static TResult? MaxOrDefault<TSource, TResult>(this IEnumerable<TSource>? source, Func<TSource, TResult> selector)
	{
		if (source == null || source.Any() == false)
			return default;
		else
			return source.Max(selector);
	}

	/// <inheritdoc cref="Enumerable.Max{TSource, TResult}(IEnumerable{TSource}, Func{TSource, TResult})"/>
	/// <param name="defaultValue">A value to return if the collection is null or empty.</param>
	public static TResult? MaxOrDefault<TSource, TResult>(this IEnumerable<TSource>? source, Func<TSource, TResult> selector, TResult defaultValue)
	{
		if (source == null || source.Any() == false)
			return defaultValue;
		else
			return source.Max(selector);
	}

	/// <inheritdoc cref="Enumerable.Min{TSource, TResult}(IEnumerable{TSource}, Func{TSource, TResult})"/>
	public static TResult? MinOrDefault<TSource, TResult>(this IEnumerable<TSource>? source, Func<TSource, TResult> selector)
	{
		if (source == null || source.Any() == false)
			return default;
		else
			return source.Min(selector);
	}

	/// <inheritdoc cref="Enumerable.Min{TSource, TResult}(IEnumerable{TSource}, Func{TSource, TResult})"/>
	/// <param name="defaultValue">A value to return if the collection is null or empty.</param>
	public static TResult? MinOrDefault<TSource, TResult>(this IEnumerable<TSource>? source, Func<TSource, TResult> selector, TResult defaultValue)
	{
		if (source == null || source.Any() == false)
			return defaultValue;
		else
			return source.Min(selector);
	}

	/// <summary>
	/// Converts the provided collection of items into a data table.
	/// </summary>
	/// <typeparam name="TSource">The data type of the items.</typeparam>
	/// <param name="values">The items to convert.</param>
	/// <param name="columns">An option list of columns to filter the resulting table to (should use nameof(Class.Property)).</param>
	public static DataTable ToDataTable<TSource>(this IEnumerable<TSource> values, params string[] columns) where TSource : class
	{
		var properties = TypeDescriptor.GetProperties(typeof(TSource)).Cast<PropertyDescriptor>();
		var ordered = new List<PropertyDescriptor>();
		var table = new DataTable();

		if (columns.Length != 0)
		{
			properties = properties.Where(x => columns.Contains(x.Name));

			foreach (var column in columns)
				ordered.Add(properties.First(x => x.Name == column));
		}
		else
		{
			ordered = properties.ToList();
		}

		foreach (var property in ordered)
			table.Columns.Add(property.Name, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);

		foreach (var item in values)
		{
			var row = table.NewRow();

			foreach (var property in ordered)
				row[property.Name] = property.GetValue(item) ?? DBNull.Value;

			table.Rows.Add(row);
		}

		return table;
	}

	/// <summary>
	/// Splits the provided source into arrays of the specified size.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="source">The collection to split.</param>
	/// <param name="partitionSize">The amount of items to return in each array.</param>
	/// <param name="includePartial">Specifies whether to return an incomplete group at the end.</param>
	public static IEnumerable<T[]> Partition<T>(this IEnumerable<T> source, int partitionSize, bool includePartial = true)
	{
		var buffer = new T[partitionSize];
		var n = 0;

		foreach (var item in source)
		{
			buffer[n] = item;
			n += 1;

			if (n == partitionSize)
			{
				yield return buffer;

				buffer = new T[partitionSize];
				n = 0;
			}
		}

		if (n > 0 && includePartial)
			yield return buffer;
	}

	/// <summary>
	/// Splits the provided source into the specified number of collections.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="source">The collection to split.</param>
	/// <param name="count">The number of collections to split the source into.</param>
	public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> source, int count)
	{
		return source.Select((Item, Index) => new { Index, Item })
			.GroupBy(x => x.Index % count)
			.Select(x => x.Select(y => y.Item));
	}

	/// <inheritdoc cref="Enumerable.Skip{TSource}(IEnumerable{TSource}, int)" />
	public static IEnumerable<TSource> SkipLong<TSource>(this IEnumerable<TSource> source, long count)
	{
		while (count > 0)
		{
			if (count > int.MaxValue)
			{
				source = source.Skip(int.MaxValue);
				count -= int.MaxValue;
			}
			else
			{
				source = source.Skip((int)count);
				count = 0;
			}
		}

		return source;
	}

	/// <summary>
	/// Returns the item after the provided value in the collection.
	/// </summary>
	/// <typeparam name="TValue"></typeparam>
	/// <param name="source">The collection to search.</param>
	/// <param name="current">An item in the collection.</param>
	/// <param name="comparer">A function to determine whether two items are equal.</param>
	public static TValue Next<TValue>(this IEnumerable<TValue> source, TValue current, Func<TValue, TValue, bool>? comparer = null)
	{
		TValue? next = default;
		comparer ??= EqualityComparer<TValue>.Default.Equals;
		bool matched = false;

		foreach (var item in source)
		{
			if (matched)
			{
				next = item;
				break;
			}

			if (comparer.Invoke(current, item))
				matched = true;
		}

		return next ?? source.First();
	}

	/// <summary>
	/// Returns the item before the provided value in the collection.
	/// </summary>
	/// <typeparam name="TValue"></typeparam>
	/// <param name="source">The collection to search.</param>
	/// <param name="current">An item in the collection.</param>
	/// <param name="comparer">A function to determine whether two items are equal.</param>
	public static TValue Previous<TValue>(this IEnumerable<TValue> source, TValue current, Func<TValue, TValue, bool>? comparer = null)
	{
		TValue? previous = default;
		comparer ??= EqualityComparer<TValue>.Default.Equals;

		foreach (var item in source)
		{
			if (comparer.Invoke(current, item))
				break;

			previous = item;
		}

		return previous ?? source.Last();
	}
}
