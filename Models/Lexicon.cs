using System.Collections;
using System.Runtime.Serialization;

namespace easy_core;

/// <summary>
/// Represents a collection of keys and values.
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
[Serializable]
public class Lexicon<TKey, TValue> : ILexicon<TKey, TValue>, IDictionary, ICollection<KeyValuePair<TKey, TValue>>, ICollection, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable, ISerializable, IDeserializationCallback where TKey : notnull
{
	private readonly Dictionary<TKey, List<TValue>> Internal;

	/// <inheritdoc />
	public ICollection<TKey> Keys => Internal.Keys;

	/// <inheritdoc />
	ICollection IDictionary.Keys => Internal.Keys;

	/// <inheritdoc />
	public int Count => Internal.Sum(x => x.Value.Count);

	/// <inheritdoc />
	public ICollection<List<TValue>> GroupedValues => Internal.Values;

	/// <inheritdoc />
	public ICollection<TValue> Values => Internal.SelectMany(x => x.Value).ToList();

	/// <inheritdoc />
	ICollection IDictionary.Values => Internal.Values;

	/// <inheritdoc cref="Dictionary{TKey, TValue}.Comparer" />
	public IEqualityComparer<TKey> Comparer => Internal.Comparer;

	/// <inheritdoc />
	public bool IsSynchronized => false;

	/// <inheritdoc />
	public object SyncRoot => ((ICollection)Internal).SyncRoot;

	/// <inheritdoc />
	public bool IsReadOnly => false;

	/// <inheritdoc />
	public bool IsFixedSize => false;

	/// <inheritdoc />
	object? IDictionary.this[object key]
	{
		get
		{
			if (key == null)
				throw new ArgumentNullException(nameof(key));
			else if (key is TKey casted)
				return this[casted];
			else
				return null;
		}
		set
		{
			if (key == null)
				throw new ArgumentNullException(nameof(key));
			else if (key is TKey casted && value is List<TValue> updated)
				this[casted] = updated;
			else
				throw new NotSupportedException("Could not cast key or value to required type.");
		}
	}

	/// <inheritdoc />
	public List<TValue> this[TKey key]
	{
		get => Internal[key];
		set => Internal[key] = value;
	}

	/// <inheritdoc />
	/// <exception cref="ArgumentException"></exception>
	/// <exception cref="KeyNotFoundException"></exception>
	public TValue this[TKey key, int index]
	{
		get
		{
			if (TryGetValue(key, out var list) && index >= 0 && index < list.Count)
				return list[index];
			else
				throw new KeyNotFoundException("Specified value does not exist.");
		}
		set
		{
			if (TryGetValue(key, out var list) && index >= 0 && index <= list.Count)
			{
				if (index == list.Count)
					list.Add(value);
				else
					list[index] = value;
			}
			else if (index == 0)
			{
				Internal.Add(key, [value]);
			}
			else
			{
				throw new ArgumentException("Invalid key and index combination provided.");
			}
		}
	}

	public Lexicon()
	{
		Internal = [];
	}

	public Lexicon(int capacity)
	{
		Internal = new Dictionary<TKey, List<TValue>>(capacity);
	}

	public Lexicon(IEqualityComparer<TKey>? comparer)
	{
		Internal = new Dictionary<TKey, List<TValue>>(comparer);
	}

	public Lexicon(int capacity, IEqualityComparer<TKey>? comparer)
	{
		Internal = new Dictionary<TKey, List<TValue>>(capacity, comparer);
	}

	public Lexicon(ILexicon<TKey, TValue> lexicon)
	{
		Internal = lexicon.GroupBy(x => x.Key).ToDictionary(x => x.Key, x => x.Select(y => y.Value).ToList());
	}

	public Lexicon(ILexicon<TKey, TValue> lexicon, IEqualityComparer<TKey>? comparer)
	{
		Internal = lexicon.GroupBy(x => x.Key).ToDictionary(x => x.Key, x => x.Select(y => y.Value).ToList(), comparer);
	}

	public Lexicon(IDictionary<TKey, TValue> dictionary)
	{
		Internal = dictionary.ToDictionary(x => x.Key, x => new List<TValue>() { x.Value });
	}

	public Lexicon(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey>? comparer)
	{
		Internal = dictionary.ToDictionary(x => x.Key, x => new List<TValue>() { x.Value }, comparer);
	}

	public Lexicon(IEnumerable<KeyValuePair<TKey, TValue>> collection)
	{
		Internal = collection.GroupBy(x => x.Key).ToDictionary(x => x.Key, x => x.Select(y => y.Value).ToList());
	}

	public Lexicon(IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey>? comparer)
	{
		Internal = collection.GroupBy(x => x.Key).ToDictionary(x => x.Key, x => x.Select(y => y.Value).ToList(), comparer);
	}

	protected Lexicon(SerializationInfo info, StreamingContext context)
	{
		Internal = (Dictionary<TKey, List<TValue>>?)info.GetValue("InternalDictionary", typeof(Dictionary<TKey, List<TValue>>)) ?? [];
	}


	/// <inheritdoc />
	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	/// <inheritdoc />
	/// <remarks>
	/// Enumerated values will be a collection of key-value pairs where the values are lists of the provided type.
	/// </remarks>
	IDictionaryEnumerator IDictionary.GetEnumerator()
	{
		return Internal.GetEnumerator();
	}

	/// <inheritdoc />
	public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
	{
		foreach (var item in Internal)
			foreach (var value in item.Value)
				yield return new KeyValuePair<TKey, TValue>(item.Key, value);
	}

	/// <inheritdoc />
	public bool Contains(KeyValuePair<TKey, TValue> item)
	{
		return Contains(item.Key, item.Value);
	}

	/// <inheritdoc />
	bool IDictionary.Contains(object key)
	{
		if (key is not TKey casted)
			return false;
		else
			return ContainsKey(casted);
	}

	/// <inheritdoc />
	public bool Contains(TKey key, TValue value)
	{
		return TryGetValue(key, out var list) && list.Contains(value);
	}

	/// <inheritdoc />
	public bool ContainsKey(TKey key)
	{
		return Internal.ContainsKey(key);
	}

	/// <inheritdoc />
	public bool ContainsValue(TValue value)
	{
		return Internal.SelectMany(x => x.Value).Contains(value);
	}

	/// <inheritdoc />
	void ICollection.CopyTo(Array array, int index)
	{
		if (array.Rank != 1)
			throw new ArgumentException("The provided array must have a single dimension.", nameof(array));
		else if (array.GetLowerBound(0) != 0)
			throw new ArgumentException("The provided array must start at an index of zero.", nameof(array));
		else if (array is KeyValuePair<TKey, TValue>[] pairs)
			CopyTo(pairs, index);
		else
			throw new ArgumentException("The provided array could not be casted to a collection of key-value pairs.", nameof(array));
	}

	/// <inheritdoc />
	public void CopyTo(KeyValuePair<TKey, TValue>[] array, int index)
	{
		if (index < 0)
			throw new ArgumentOutOfRangeException(nameof(index), "The provided index must be greater than or equal to zero.");
		else if ((uint)index > (uint)array.Length)
			throw new ArgumentOutOfRangeException(nameof(index), "The provided index must be less than the size of the array.");
		else if (array.Length - index < Count)
			throw new ArgumentException("The provided array is not large enough to hold all of the items.", nameof(index));

		foreach (var item in this.Skip(index))
			array[index++] = item;
	}

	/// <inheritdoc />
	public void Add(KeyValuePair<TKey, TValue> item)
	{
		Add(item.Key, item.Value);
	}

	/// <inheritdoc />
	void IDictionary.Add(object key, object? values)
	{
		if (key is TKey casted && values is List<TValue> list)
			Add(casted, list);
		else
			throw new NotSupportedException("Could not cast key or value to required type.");
	}

	/// <inheritdoc />
	public void Add(TKey key, TValue value)
	{
		if (TryGetValue(key, out var list))
			list.Add(value);
		else
			Internal.Add(key, [value]);
	}

	/// <inheritdoc />
	public void Add(TKey key, IEnumerable<TValue> values)
	{
		if (TryGetValue(key, out var list))
			list.AddRange(values);
		else
			Internal.Add(key, values.ToList());
	}

	/// <inheritdoc />
	public void AddRange(IEnumerable<KeyValuePair<TKey, TValue>> items)
	{
		foreach (var item in items)
			Add(item.Key, item.Value);
	}

	/// <inheritdoc />
	public bool Remove(KeyValuePair<TKey, TValue> item)
	{
		return Remove(item.Key, item.Value);
	}

	/// <inheritdoc />
	void IDictionary.Remove(object key)
	{
		if (key is not TKey casted)
			throw new NotSupportedException("Could not cast key to required type.");
		else if (Remove(casted) == false)
			throw new ArgumentNullException(nameof(key), "The provided key is not in the collection.");
	}

	/// <inheritdoc />
	public bool Remove(TKey key, TValue value)
	{
		if (TryGetValue(key, out var list) == false)
			return false;

		if (list.Count == 1 && EqualityComparer<TValue>.Default.Equals(value, list[0]))
		{
			Internal.Remove(key);
			return true;
		}
		else if (list.Count > 1)
		{
			for (var i = 0; i < list.Count; i++)
			{
				if (EqualityComparer<TValue>.Default.Equals(value, list[i]))
				{
					list.RemoveAt(i);
					return true;
				}
			}
		}

		return false;
	}

	/// <inheritdoc />
	public bool Remove(TKey key)
	{
		if (ContainsKey(key) == false)
			return false;

		Internal.Remove(key);
		return true;
	}

	/// <inheritdoc />
	public void Clear()
	{
		Internal.Clear();
	}

	/// <inheritdoc />
	public bool ChangeValue(TKey key, TValue oldvalue, TValue newValue)
	{
		if (TryGetValue(key, out var list) == false)
			return false;

		for (var i = 0; i < list.Count; i++)
		{
			if (EqualityComparer<TValue>.Default.Equals(oldvalue, list[i]))
			{
				list[i] = newValue;
				return true;
			}
		}

		return false;
	}

	/// <inheritdoc />
	public bool TryGetValue(TKey key, int index, out TValue value)
	{
		if (TryGetValue(key, out var list) && index >= 0 && index < list.Count)
		{
			value = list[index];
			return true;
		}
		else
		{
			value = default!;
			return false;
		}
	}

	/// <inheritdoc />
	public bool TryGetValue(TKey key, out List<TValue> values)
	{
		if (Internal.TryGetValue(key, out var list))
		{
			values = list;
			return true;
		}
		else
		{
			values = [];
			return false;
		}
	}

	/// <inheritdoc />
	public void TrimExcess()
	{
		Internal.TrimExcess();
	}

	/// <inheritdoc />
	public void TrimExcess(int capacity)
	{
		Internal.TrimExcess(capacity);
	}

	/// <inheritdoc />
	public void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		if (info == null)
			throw new ArgumentException("Serialization info was not provided.", nameof(info));

		info.AddValue("InternalDictionary", Internal);
	}

	/// <inheritdoc />
	public void OnDeserialization(object? sender)
	{
		Internal.OnDeserialization(sender);
	}
}
